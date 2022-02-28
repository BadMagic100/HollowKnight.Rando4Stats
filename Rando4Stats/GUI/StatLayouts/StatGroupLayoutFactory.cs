using MagicUI.Core;
using MagicUI.Elements;
using RandoStats.Settings;
using RandoStats.Stats;
using System;
using System.Collections.Generic;

namespace RandoStats.GUI.StatLayouts
{
    /// <summary>
    /// Base class to build configurable layouts for collections of stats
    /// </summary>
    public abstract class StatGroupLayoutFactory
    {
        /// <summary>
        /// The settings to use to configure the layout
        /// </summary>
        public StatLayoutSettings Settings { get; init; }

        /// <summary>
        /// Whether the stat is eligible to display given the current randomizer settings
        /// </summary>
        public abstract bool CanDisplay { get; }

        /// <summary>
        /// The top-level section header for the stat group, such as "Items Obtained"
        /// </summary>
        public abstract string GroupName { get; }

        /// <summary>
        /// The statistics that should display unconditionally
        /// </summary>
        protected abstract IReadOnlyCollection<RandomizerStatistic> RootStatistics { get; init; }

        /// <summary>
        /// The statistics that should display given a subcategory
        /// </summary>
        protected abstract Dictionary<string, IReadOnlyCollection<RandomizerStatistic>> SubcategoryStatistics { get; init; }

        /// <summary>
        /// All subcategories that are allowed for this stat group. These cases, and only these cases, should be handled by
        /// <see cref="SubcategoryStatistics"/>.
        /// </summary>
        protected abstract IReadOnlyCollection<string> AllowedSubcategories { get; init; }

        /// <summary>
        /// Constructs a layout factory. Note that to work with <see cref="StatLayoutHelper.GetLayoutBuilderFromSettings(StatLayoutSettings)"/>,
        /// you're expected to implement a constructor with this signature.
        /// </summary>
        public StatGroupLayoutFactory(StatLayoutSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets all the statistics that should display given a subcategory. This expects you to
        /// </summary>
        /// <param name="subcategory">The subcategory name.</param>
        private IEnumerable<RandomizerStatistic> GetStatisticsForSubcategory(string subcategory)
        {
            if (SubcategoryStatistics.ContainsKey(subcategory))
            {
                return SubcategoryStatistics[subcategory];
            }
            else
            {
                throw new NotImplementedException($"{subcategory} is not an implemented subcategory for {GetType().Name};" +
                    $" you need to add it to your {nameof(SubcategoryStatistics)}");
            }
        }

        public void HookStatsEngine(bool transient = true)
        {
            foreach (RandomizerStatistic stat in RootStatistics)
            {
                StatsEngine.Hook(stat, transient);
            }
            // slight optimization - this iterates through the subcategory dictionary, so we'll only get it once
            HashSet<string> enabledSubcategories = Settings.EnabledSubcategoryNames;
            foreach (string subcategory in AllowedSubcategories)
            {
                if (enabledSubcategories.Contains(subcategory))
                {
                    foreach (RandomizerStatistic stat in GetStatisticsForSubcategory(subcategory))
                    {
                        StatsEngine.Hook(stat, transient);
                    }
                }
            }
        }

        /// <summary>
        /// Computes the stats and builds them out in the specified layout.
        /// </summary>
        /// <param name="canvas">The visual parent to draw the UI on</param>
        /// <param name="subcategoryColumns">The number of columns allocated to subcategories' dynamic layouts.</param>
        /// <returns></returns>
        public Layout BuildLayout(LayoutRoot onLayout, int subcategoryColumns)
        {
            StackLayout layout = new(onLayout, GetType().Name)
            {
                Spacing = StatLayoutHelper.VERTICAL_SPACING_SUBGROUPS,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            layout.Children.Add(new TextObject(onLayout)
            {
                Text = GroupName,
                Font = UI.TrajanBold,
                FontSize = StatLayoutHelper.FONT_SIZE_H1,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            Layout rootGroupLayout = new DynamicUniformGrid(onLayout, $"{GetType().Name}_Root_Stats")
            {
                HorizontalSpacing = StatLayoutHelper.HORIZONTAL_SPACING,
                VerticalSpacing = StatLayoutHelper.VERTICAL_SPACING_PANEL,
                HorizontalAlignment = HorizontalAlignment.Center,
                ChildrenBeforeRollover = subcategoryColumns
            };
            layout.Children.Add(rootGroupLayout);
            foreach (RandomizerStatistic stat in RootStatistics)
            {
                if (stat.IsEnabled)
                {
                    rootGroupLayout.Children.Add(new LabeledStatTextFactory(stat).Build(onLayout));
                }
            }

            HashSet<string> enabledSubcategories = Settings.EnabledSubcategoryNames;
            foreach (string subcategory in AllowedSubcategories)
            {
                if (enabledSubcategories.Contains(subcategory))
                {
                    Layout subcategoryGroupLayout = new DynamicUniformGrid(onLayout, $"{GetType().Name}_{subcategory}")
                    {
                        HorizontalSpacing = StatLayoutHelper.HORIZONTAL_SPACING,
                        VerticalSpacing = StatLayoutHelper.VERTICAL_SPACING_PANEL,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        ChildrenBeforeRollover = subcategoryColumns
                    };
                    foreach (RandomizerStatistic stat in GetStatisticsForSubcategory(subcategory))
                    {
                        if (stat.IsEnabled)
                        {
                            subcategoryGroupLayout.Children.Add(new LabeledStatTextFactory(stat).Build(onLayout));
                        }
                    }
                    layout.Children.Add(subcategoryGroupLayout);
                }
            }

            return layout;
        }
    }
}

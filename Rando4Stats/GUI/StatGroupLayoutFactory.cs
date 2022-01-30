using MagicUI.Core;
using MagicUI.Elements;
using RandoStats.Settings;
using RandoStats.Stats;
using System.Collections.Generic;

namespace RandoStats.GUI
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
        /// Constructs a layout factory. Note that to work with <see cref="StatLayoutHelper.GetLayoutBuilderFromSettings(StatLayoutSettings)"/>,
        /// you're expected to implement a constructor with this signature.
        /// </summary>
        public StatGroupLayoutFactory(StatLayoutSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets all the statistics that should display unconditionally.
        /// </summary>
        protected abstract IEnumerable<IRandomizerStatistic> GetRootStatistics();
        /// <summary>
        /// Gets all subcategories that are allowed for this stat group. These cases, and only these cases, should be handled by
        /// <see cref="GetStatisticsForSubcategory(string)"/>.
        /// </summary>
        protected abstract IEnumerable<string> GetAllowedSubcategories();
        /// <summary>
        /// Gets all the statistics that should display given a subcategory. This expects you to
        /// </summary>
        /// <param name="subcategory">The subcategory name.</param>
        protected abstract IEnumerable<IRandomizerStatistic> GetStatisticsForSubcategory(string subcategory);

        /// <summary>
        /// Computes the stats only, without creating any UI elements or layout. This is specifically for adding
        /// stats to the stat registry.
        /// </summary>
        public void ComputeStatsOnly()
        {
            foreach (IRandomizerStatistic stat in GetRootStatistics())
            {
                if (stat.IsEnabled)
                {
                    stat.GetLabel();
                    stat.GetContent();
                }
            }
            HashSet<string> enabledSubcategories = Settings.EnabledSubcategoryNames;
            foreach (string subcategory in GetAllowedSubcategories())
            {
                if (enabledSubcategories.Contains(subcategory))
                {
                    foreach (IRandomizerStatistic stat in GetStatisticsForSubcategory(subcategory))
                    {
                        if (stat.IsEnabled)
                        {
                            stat.GetLabel();
                            stat.GetContent();
                        }
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
                Spacing = StatLayoutHelper.VERTICAL_SPACING,
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
            foreach (IRandomizerStatistic stat in GetRootStatistics())
            {
                if (stat.IsEnabled)
                {
                    layout.Children.Add(new LabeledStatTextFactory(stat).Build(onLayout));
                }
            }

            HashSet<string> enabledSubcategories = Settings.EnabledSubcategoryNames;
            foreach (string subcategory in GetAllowedSubcategories())
            {
                if (enabledSubcategories.Contains(subcategory))
                {
                    Layout subcategoryGroupLayout = new DynamicUniformGrid(onLayout, $"{GetType().Name}_{subcategory}")
                    {
                        HorizontalSpacing = StatLayoutHelper.HORIZONTAL_SPACING,
                        VerticalSpacing = StatLayoutHelper.VERTICAL_SPACING,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        ChildrenBeforeRollover = subcategoryColumns
                    };
                    foreach (IRandomizerStatistic stat in GetStatisticsForSubcategory(subcategory))
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

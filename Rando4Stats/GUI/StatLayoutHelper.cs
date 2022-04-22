using MagicUI.Core;
using MagicUI.Elements;
using RandoStats.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RandoStats.GUI
{
    internal static class StatLayoutHelper
    {
        internal const float HORIZONTAL_PADDING = 5;
        internal const float VERTICAL_PADDING = 10;

        internal const float VERTICAL_SPACING_PANEL = 8;
        internal const float VERTICAL_SPACING_SUBGROUPS = 12;
        internal const float VERTICAL_SPACING_STATS = 15;
        internal const float HORIZONTAL_SPACING = 10;

        internal const int FONT_SIZE_H1 = 25;
        internal const int FONT_SIZE_H2 = 18;
        internal const int FONT_SIZE_H3 = 15;

        /// <summary>
        /// Given a position, generates a stack layout with the correct alignment for that position
        /// </summary>
        /// <param name="onLayout">The root layout to draw the stats on</param>
        /// <param name="pos">The stat position</param>
        internal static Layout? GetLayoutForPosition(LayoutRoot onLayout, StatPosition pos)
        {
            if (pos == StatPosition.None)
            {
                return null;
            }
            else
            {
                HorizontalAlignment desiredHorizontal = pos.ToString() switch
                {
                    var left when left.EndsWith("Left") => HorizontalAlignment.Left,
                    var center when center.EndsWith("Center") => HorizontalAlignment.Center,
                    var right when right.EndsWith("Right") => HorizontalAlignment.Right,
                    _ => throw new NotImplementedException($"Can't infer horizontal alignment from {pos}")
                };
                VerticalAlignment desiredVertical = pos.ToString() switch
                {
                    var top when top.StartsWith("Top") => VerticalAlignment.Top,
                    var bottom when bottom.StartsWith("Bottom") => VerticalAlignment.Bottom,
                    _ => throw new NotImplementedException($"Can't infer vertical alignment from {pos}")
                };
                return new StackLayout(onLayout)
                {
                    Spacing = VERTICAL_SPACING_STATS,
                    HorizontalAlignment = desiredHorizontal,
                    VerticalAlignment = desiredVertical,
                    Padding = new Padding(HORIZONTAL_PADDING, VERTICAL_PADDING)
                };
            }
        }

        /// <summary>
        /// Gets the number of columns to allocate to dynamic grids in a given stat position
        /// </summary>
        /// <param name="position">The position to check</param>
        internal static int GetDynamicGridColumnsForPosition(StatPosition position) => position switch
        {
            StatPosition.TopCenter => 5,
            _ => 2
        };

        private static IEnumerable<StatGroupLayoutFactory> factoryCache = Enumerable.Empty<StatGroupLayoutFactory>();

        internal static void ConstructLayoutFactories(RandoStatsGlobalSettings fromSettings)
        {
            List<StatGroupLayoutFactory> factories = new();
            foreach (PropertyInfo prop in fromSettings.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(StatLayoutSettings))
                {
                    StatLayoutSettings settings = (StatLayoutSettings)prop.GetValue(fromSettings);
                    string factoryTypeFullName = "RandoStats.GUI.StatLayouts." + prop.Name.Replace("Settings", "Factory");
                    Type factoryType = fromSettings.GetType().Assembly.GetType(factoryTypeFullName);

                    ConstructorInfo ctor = factoryType.GetConstructor(new Type[] { typeof(StatLayoutSettings) });
                    StatGroupLayoutFactory factory = (StatGroupLayoutFactory)ctor.Invoke(new object[] { settings });
                    factories.Add(factory);
                }
            }
            factoryCache = factories;
        }

        internal static IEnumerable<StatGroupLayoutFactory> LayoutFactories => factoryCache;
    }
}

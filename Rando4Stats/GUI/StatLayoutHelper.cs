using MagicUI.Core;
using MagicUI.Elements;
using System;

namespace RandoStats.GUI
{
    internal static class StatLayoutHelper
    {
        internal const float HORIZONTAL_PADDING = 5;
        internal const float VERTICAL_PADDING = 10;

        internal const float VERTICAL_SPACING = 8;
        internal const float HORIZONTAL_SPACING = 10;

        internal const int FONT_SIZE_H1 = 25;
        internal const int FONT_SIZE_H2 = 18;
        internal const int FONT_SIZE_H3 = 15;

        /// <summary>
        /// Given a position, generates a stack layout with the correct alignment for that position
        /// </summary>
        /// <param name="onLayout">The root layout to draw the stats on</param>
        /// <param name="pos">The stat position</param>
        /// <returns></returns>
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
                    Spacing = VERTICAL_SPACING * 1.5f,
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
            StatPosition.TopCenter => 6,
            _ => 2
        };
    }
}

using MagicUI.Core;
using MagicUI.Elements;
using RandoStats.Stats;

namespace RandoStats.GUI
{
    internal class LabeledStatTextFactory
    {
        private readonly RandomizerStatistic stat;

        public LabeledStatTextFactory(RandomizerStatistic stat)
        {
            this.stat = stat;
        }

        public ArrangableElement Build(LayoutRoot onLayout)
        {
            string header = stat.GetLabel();
            string text = stat.GetContent();
            Layout statStack = new StackLayout(onLayout)
            {
                Spacing = 5,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            statStack.Children.Add(new TextObject(onLayout, "Stat_" + header)
            {
                Font = UI.TrajanBold,
                FontSize = StatLayoutHelper.FONT_SIZE_H2,
                Text = header,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            statStack.Children.Add(new TextObject(onLayout, "StatValue_" + header)
            {
                Font = UI.TrajanNormal,
                FontSize = StatLayoutHelper.FONT_SIZE_H3,
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            return statStack;
        }
    }
}

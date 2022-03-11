using MagicUI.Core;
using MagicUI.Elements;
using RandomizerMod;
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
            string ns = stat.StatNamespace;
            string header = stat.GetLabel();
            string text = stat.GetContent();
            Layout statStack = new StackLayout(onLayout)
            {
                Spacing = 5,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            statStack.Children.Add(new TextObject(onLayout, $"StatHeader_{ns}_{header}")
            {
                Font = UI.TrajanBold,
                FontSize = StatLayoutHelper.FONT_SIZE_H2,
                Text = Localization.Localize(header),
                HorizontalAlignment = HorizontalAlignment.Center
            });
            statStack.Children.Add(new TextObject(onLayout, $"StatValue_{ns}_{header}")
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

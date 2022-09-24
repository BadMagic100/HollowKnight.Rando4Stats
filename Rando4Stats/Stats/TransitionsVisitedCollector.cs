using FStats;
using FStats.StatControllers;
using RandomizerMod.RC;
using RandoStats.Menus;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats.Stats
{

    [MenuName(TITLE)]
    [MenuSubpage(StandardSubpages.AREA_SUBPAGE)]
    public class TransitionsVisitedCollector : StatController
    {
        const string TITLE = "Randomized Transitions Visited";
        const int PRIORITY = -77_000;

        public override void Initialize() { }

        public override IEnumerable<DisplayInfo> GetDisplayInfos()
        {
            if (!Rando.IsRandoSave || Rando.RS.Context.transitionPlacements == null)
            {
                yield break;
            }

            Counter visited = new();
            Counter total = new();

            foreach (TransitionPlacement transition in Rando.RS.Context.transitionPlacements)
            {
                RandoModTransition source = transition.Source;
                string area = AreaName.CleanAreaName(source.TransitionDef.SceneName) ?? AreaName.Other;
                if (Rando.RS.TrackerData.HasVisited(source.Name))
                {
                    visited[area]++;
                }
                total[area]++;
            }

            List<string> cols = FStatsMod.LS.Get<TimeByAreaStat>().AreaOrder()
                .Where(a => total[a] > 0)
                .Select(a => $"{a} - {visited[a]}/{total[a]}")
                .ToList()
                .TableColumns(10);

            if (RandoStats.Instance!.GlobalSettings.ShouldDisplay(this, StandardSubpages.AREA_SUBPAGE))
            {
                yield return new DisplayInfo()
                {
                    Title = TITLE,
                    MainStat = $"{visited.Total}/{total.Total}",
                    StatColumns = cols,
                    Priority = PRIORITY,
                };
            }
        }

        public override void Unload() { }
    }
}

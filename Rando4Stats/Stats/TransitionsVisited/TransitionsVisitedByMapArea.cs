//using ConnectionMetadataInjector.Util;
//using Modding;
//using RandomizerMod.RC;
//using RandomizerMod.Settings;
//using RandoStats.Util;
//using System.Collections.Generic;
//using System.Linq;
//using Rando = RandomizerMod.RandomizerMod;

//namespace RandoStats.Stats.TransitionsVisited
//{
//    public class TransitionsVisitedByMapArea : PercentageStatistic
//    {
//        private static readonly Loggable log = ScopedLoggers.GetLogger();

//        private readonly string areaName;
//        private readonly string areaShortName;
//        private Dictionary<string, RandoModTransition> transitionLookup = new();

//        public override string StatNamespace => base.StatNamespace + "." + areaShortName;

//        public override bool IsComputable => Rando.RS.GenerationSettings.TransitionSettings.Mode != TransitionSettings.TransitionMode.None;

//        public override bool IsEnabled => TotalSum > 0;

//        public TransitionsVisitedByMapArea(string area) : base(area == MapArea.FORGOTTEN_CROSSROADS ? "Crossroads" : area)
//        {
//            areaName = area;
//            areaShortName = area.Replace(" ", "");
//        }

//        public override void BeginCompute()
//        {
//            base.BeginCompute();
//            transitionLookup = Rando.RS.Context.transitionPlacements.ToDictionary(x => x.Source.Name, x => x.Source);
//        }

//        public override void HandleTransition(string from, string to)
//        {
//            if ((transitionLookup![from].TransitionDef?.MapArea ?? SubcategoryFinder.OTHER) == areaName)
//            {
//                if (Rando.RS.TrackerData.HasVisited(from))
//                {
//                    log.LogDebug($"Counting transition {from} towards group {areaShortName} obtains");
//                    ObtainedSum++;
//                }
//                log.LogDebug($"Counting transition {from} towards {areaShortName} total");
//                TotalSum++;
//            }
//        }
//    }
//}

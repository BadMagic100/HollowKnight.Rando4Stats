//using RandomizerMod.Settings;
//using Rando = RandomizerMod.RandomizerMod;

//namespace RandoStats.Stats.TransitionsVisited
//{
//    public class TransitionsVisitedTotal : PercentageStatistic
//    {
//        public TransitionsVisitedTotal(string label) : base(label) { }

//        public override bool IsComputable => Rando.RS.GenerationSettings.TransitionSettings.Mode != TransitionSettings.TransitionMode.None;

//        public override bool IsEnabled => true;

//        public override void HandleTransition(string from, string to)
//        {
//            if (Rando.RS.TrackerData.HasVisited(from))
//            {
//                ObtainedSum++;
//            }
//            TotalSum++;
//        }
//    }
//}

using FStats;
using FStats.Attributes;
using FStats.StatControllers;
using FStats.StatControllers.ModConditional;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Placements;
using ItemChanger.Tags;
using RandoStats.Menus;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandoStats.Stats
{
    [MenuName("Items Obtained (ItemSync Split)")]
    [DefaultHiddenScreen]
    [GlobalSettingsExclude]
    public class ItemsyncItemObtainedCollector : ModConditionalDisplay
    {
        protected override IEnumerable<string> RequiredMods()
        {
            yield return "ItemSyncMod";
        }

        public override IEnumerable<DisplayInfo> ConditionalGetDisplayInfos()
        {
            if (Ref.Settings == null || ItemSyncMod.ItemSyncMod.ISSettings == null || ItemSyncMod.ItemSyncMod.ISSettings.UserName == null)
            {
                yield break;
            }

            string localPlayerName = ItemSyncMod.ItemSyncMod.ISSettings.UserName;
            Counter obtainedLocally = new();
            Dictionary<string, Counter> obtainedRemotelyByPlayerArea = new();
            Counter total = new();

            foreach (AbstractPlacement pmt in Ref.Settings.Placements.Values.SelectValidPlacements())
            {
                string scene = string.Empty;
                if (pmt is IPrimaryLocationPlacement ip && ip.Location.name != LocationNames.Start)
                {
                    scene = ip.Location.sceneName ?? AreaName.Other;
                }
                if (string.IsNullOrEmpty(scene))
                {
                    continue;
                }

                string area = AreaName.CleanAreaName(scene);
                foreach (AbstractItem item in pmt.Items.SelectValidItems())
                {
                    if (!item.WasEverObtained())
                    {
                        continue;
                    }
                    if (item.GetTags<IInteropTag>().FirstOrDefault(x => x.Message == "SyncedItemTag") is IInteropTag t)
                    {
                        RandoStats.Instance!.Log($"Synced item {item.name} at {pmt.Name}");
                        total[area]++;
                        if (t.TryGetProperty("Local", out bool local) && local)
                        {
                            obtainedLocally[area]++;
                        }
                        else if (t.TryGetProperty("From", out string? sender) && sender != null)
                        {
                            if (!obtainedRemotelyByPlayerArea.ContainsKey(sender))
                            {
                                obtainedRemotelyByPlayerArea[sender] = new Counter();
                            }
                            obtainedRemotelyByPlayerArea[sender][area]++;
                        }
                    }
                }
            }

            if (!RandoStats.Instance!.GlobalSettings.ShouldDisplay(this))
            {
                yield break;
            }

            IEnumerable<KeyValuePair<string, Counter>> playerAreaCounters = obtainedRemotelyByPlayerArea
                .OrderBy(kv => kv.Key)
                .Prepend(new KeyValuePair<string, Counter>(localPlayerName, obtainedLocally));

            foreach (KeyValuePair<string, Counter> playerAreaCounter in playerAreaCounters)
            {
                string player = playerAreaCounter.Key;
                Counter obtainedByArea = playerAreaCounter.Value;

                List<string> columns = FStatsMod.LS.Get<TimeByAreaStat>().AreaOrder
                    .Where(a => total[a] > 0)
                    .Select(a => $"{a} - {obtainedByArea[a]}/{total[a]}")
                    .ToList()
                    .TableColumns(10);

                int percentage = Mathf.RoundToInt(100f * obtainedByArea.Total / total.Total);

                yield return new DisplayInfo()
                {
                    Title = $"Items obtained by {player}",
                    MainStat = $"{obtainedByArea.Total}/{total.Total} ({percentage}%)",
                    StatColumns = columns,
                    Priority = BuiltinScreenPriorityValues.ItemSyncData + 100,
                };
            }
        }
    }
}

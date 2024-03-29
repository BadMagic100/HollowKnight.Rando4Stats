﻿using Modding;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RandoStats.Stats
{
    public static class StatFormatRegistry
    {
        private static Loggable log = ScopedLoggers.GetLogger();

        public const string STAT_FULL = "Full";
        public const string STAT_PERCENT = "Percent";
        public const string STAT_OBTAINED = "Obtained";
        public const string STAT_TOTAL = "Total";
        public const string STAT_FRACTION = "Fraction";
        public const string STAT_TIME = "Time";

        private const string NS_BUILT_IN = "BuiltIn";

        private static readonly Dictionary<string, string> statRegistry = new();
        private static readonly Dictionary<string, string> aliases = new()
        {
            {
                "RACING_EXTENDED",
                $"{{{NS_BUILT_IN}:{STAT_PERCENT}}} {{{NS_BUILT_IN}:{STAT_TIME}}} ({{ItemsObtainedTotal:{STAT_FRACTION}}})"
            },
            { "RACING_SIMPLE", $"{{{NS_BUILT_IN}:{STAT_PERCENT}}} {{{NS_BUILT_IN}:{STAT_TIME}}}" }
        };

        private static readonly Regex aliasFinder = new(@"\$([A-Z_]+)\$");
        private static readonly Regex placeholderFinder = new(@"{([A-Za-z\.]+):([A-Za-z]+)}");

        private static string KeyOf(string ns, string stat) => $"{ns}:{stat}";

        public static void GenerateBasicStats()
        {
            PlayTime time = new() { RawTime = PlayerData.instance.playTime };
            string timeStr = time.HasHours ? $"{(int)time.Hours:0}:{(int)time.Minutes:00}"
                : time.HasMinutes ? $"{(int)time.Minutes:0}:{(int)time.Seconds:00}"
                : $"{(int)time.Seconds:0}s";
            SetStat(NS_BUILT_IN, STAT_TIME, timeStr);
            SetStat(NS_BUILT_IN, STAT_PERCENT, $"{(int)PlayerData.instance.completionPercentage}%");
        }

        public static void SetStat(string ns, string stat, string value)
        {
            string key = KeyOf(ns, stat);
            statRegistry[key] = value;
        }

        private static string TryGetAlias(string name)
        {
            if (aliases.ContainsKey(name))
            {
                return aliases[name];
            }
            return $"(No alias ${name}$)";
        }

        private static string TryGetStat(string ns, string stat)
        {
            string key = KeyOf(ns, stat);
            if (statRegistry.ContainsKey(key))
            {
                return statRegistry[key];
            }
            return $"(No stat {{{key}}})";
        }

        public static string Format(string fstring)
        {
            log.LogDebug("Stat registry: \n" + string.Join("\n", statRegistry.Select(kvp => $"{kvp.Key} - {kvp.Value}").ToArray()));
            foreach (string alias in aliasFinder.Matches(fstring).Cast<Match>().Select(x => x.Groups[1].Value).Distinct())
            {
                fstring = fstring.Replace($"${alias}$", TryGetAlias(alias));
            }
            foreach ((string ns, string stat) in placeholderFinder.Matches(fstring).Cast<Match>().Select(x => (x.Groups[1].Value, x.Groups[2].Value)).Distinct())
            {

                fstring = fstring.Replace($"{{{KeyOf(ns, stat)}}}", TryGetStat(ns, stat));
            }
            return fstring;
        }
    }
}

using Modding;
using RandomizerMod.RandomizerData;
using System;

namespace RandoStats.Util
{
    // group finding logic adapted from Phenomenol https://github.com/syyePhenomenol/HollowKnight.MapModS/blob/08f64d3454d8cdbc62773d44d20e6fbec08a9055/MapModS/Data/DataLoader.cs
    public static class PoolFinder
    {
		private static Loggable log = LogHelper.GetLogger();

		public static PoolGroup GetItemPoolGroup(string cleanItemName)
		{
			switch (cleanItemName)
			{
				case "Dreamer":
					return PoolGroup.Dreamers;
				case "Split_Mothwing_Cloak":
				case "Split_Crystal_Heart":
					return PoolGroup.Skills;
				case "Grimmchild1":
				case "Grimmchild2":
					return PoolGroup.Charms;
				case "Grub":
					return PoolGroup.Grubs;
				case "One_Geo":
					return PoolGroup.GeoChests;
				default:
					break;
			}

			foreach (PoolDef poolDef in Data.Pools)
			{
				foreach (string includeItem in poolDef.IncludeItems)
				{
					if (includeItem.StartsWith(cleanItemName))
					{
						PoolGroup group = (PoolGroup)Enum.Parse(typeof(PoolGroup), poolDef.Group);

						return group;
					}
				}
			}

			log.LogWarn($"{cleanItemName} not found in PoolDefs");

			return PoolGroup.Other;
		}

		public static PoolGroup GetLocationPoolGroup(string location)
        {
			string nameWithoutLocation = location.Split('-')[0];
			return GetItemPoolGroup(nameWithoutLocation);
        }
	}
}

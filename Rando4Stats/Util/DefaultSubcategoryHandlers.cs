using Modding;
using RandomizerMod.RandomizerData;
using System;
using System.Collections.Generic;

namespace RandoStats.Util
{
    // group finding logic adapted from Phenomenol https://github.com/syyePhenomenol/HollowKnight.MapModS/blob/08f64d3454d8cdbc62773d44d20e6fbec08a9055/MapModS/Data/DataLoader.cs
    public static class DefaultSubcategoryHandlers
    {
		private static readonly Loggable log = ScopedLoggers.GetLogger();

		private static readonly HashSet<string> ShopNames = new(new string[] {
			"Sly", "Sly_(Key)", "Iselda", "Salubra", "Salubra_(Requires_Charms)",
			"Leg_Eater", "Grubfather", "Seer", "Egg_Shop" 
		});

		public static PoolGroup? GetItemPoolGroup(string cleanItemName)
		{
			switch (cleanItemName)
			{
				case "Dreamer":
					return PoolGroup.Dreamers;
				case "Split_Mothwing_Cloak":
				case "Split_Crystal_Heart":
					return PoolGroup.Skills;
				case "Double_Mask_Shard":
				case "Full_Mask":
					return PoolGroup.MaskShards;
				case "Double_Vessel_Fragment":
				case "Full_Soul_Vessel":
					return PoolGroup.VesselFragments;
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

			return null;
		}

		public static PoolGroup? GetLocationPoolGroup(string location)
        {
			if (ShopNames.Contains(location))
            {
				return PoolGroup.Shops;
            }
			string nameWithoutLocation = location.Split('-')[0];
			return GetItemPoolGroup(nameWithoutLocation);
        }

		public static string? GetLocationMapArea(string location)
        {
			if (Data.IsLocation(location))
            {
				return Data.GetLocationDef(location).MapArea;
            }
			return null;
        }

		public static string? GetTransitionMapArea(string transition)
        {
			if (Data.IsTransition(transition))
            {
				return Data.GetTransitionDef(transition).MapArea;
            }
			return null;
        }
	}
}

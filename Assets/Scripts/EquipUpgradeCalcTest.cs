using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUpgradeCalcTest
{
	public static List<int[]> helmets, armors, ornaments;

	public static void Init()
	{
		EconomyConfig.EnsureLoaded();
		int HelmetStarting = EconomyConfig.HelmetStarting, ArmorStarting = EconomyConfig.ArmorStarting, OrnamentStarting = EconomyConfig.OrnamentStarting;
		int HelmetCombat = EconomyConfig.HelmetCombat, ArmorCombat = EconomyConfig.ArmorCombat, OrnamentCombat = EconomyConfig.OrnamentCombat;
		int HelmetIncrease = EconomyConfig.HelmetIncrease, ArmorIncrease = EconomyConfig.ArmorIncrease, OrnamentIncrease = EconomyConfig.OrnamentIncrease;
		int currentHelmetAdd = 0, currentArmorAdd = 0, currentOrnamentAdd = 0, currentHelmetCombatAdd = 0, currentArmorCombatAdd = 0, currentOrnamentCombatAdd = 0;
		int currentHelmetIncrease = HelmetIncrease, currentArmorIncrease = ArmorIncrease, currentOrnamentIncrease = OrnamentIncrease;

		helmets = new List<int[]>();
		armors = new List<int[]>();
		ornaments = new List<int[]>();

		for (int i = 0; i < 11; i++)
		{
			int[] helmet = new int[12];
			int[] armor = new int[12];
			int[] ornament = new int[12];

			for (int j = 0; j < 6; j++)
			{
				helmet[j] = HelmetStarting + currentHelmetAdd;
				armor[j] = ArmorStarting + currentArmorAdd;
				ornament[j] = OrnamentStarting + currentOrnamentAdd;

				currentHelmetAdd += currentHelmetIncrease * (j + (i * 2) + 1);
				currentArmorAdd += currentArmorIncrease * (j + (i * 2) + 1);
				currentOrnamentAdd += currentOrnamentIncrease * (j + (i * 2) + 1);
			}

			for (int j = 6; j < 12; j++)
			{
				helmet[j] = HelmetCombat + currentHelmetCombatAdd;
				armor[j] = ArmorCombat + currentArmorCombatAdd;
				ornament[j] = OrnamentCombat + currentOrnamentCombatAdd;

				currentHelmetCombatAdd += HelmetIncrease * (j + (i * 2) + 1) / 4;
				currentArmorCombatAdd += ArmorIncrease * (j + (i * 2) + 1) / 4;
				currentOrnamentCombatAdd += OrnamentIncrease * (j + (i * 2) + 1) / 4;
			}

			if (i == 0 || i == 2 || i == 6 || i == 10)
			{
				for (int j = 0; j < (i == 2 ? 2 : i == 6 || i == 10 ? 4 : 1); j++)
				{
					helmets.Add(helmet);
					armors.Add(armor);
					ornaments.Add(ornament);
				}
			}
		}
	}
}

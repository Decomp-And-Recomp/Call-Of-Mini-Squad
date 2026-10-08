using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpgradeCalcTest : MonoBehaviour
{
	private static int currentWeaponAdd, currentSkillAdd, currentWeaponIncrease, currentSkillIncrease, currentCombatAdd;

	public static List<int[]> upgrades;

	public static void Init()
	{
		EconomyConfig.EnsureLoaded();
		upgrades = new List<int[]>();
		currentWeaponAdd = 0;
		currentSkillAdd = 0;
		currentCombatAdd = 0;
		currentWeaponIncrease = EconomyConfig.WeaponIncrease;
		currentSkillIncrease = EconomyConfig.SkillIncrease;

		for (int i = 0; i < EconomyConfig.WeaponSkillRows; i++)
		{
			if (i == 0)
			{
				upgrades.Add(new int[3] { EconomyConfig.WeaponStarting, EconomyConfig.SkillStarting, EconomyConfig.CombatStarting } );
				continue;
			}

			currentWeaponAdd += currentWeaponIncrease * i;
			currentSkillAdd += currentSkillIncrease * i;
			currentCombatAdd += EconomyConfig.CombatIncrease * i;

			currentWeaponIncrease += EconomyConfig.WeaponIncreaseGrowth;
			currentSkillIncrease += EconomyConfig.SkillIncreaseGrowth;

			upgrades.Add(new int[3] { EconomyConfig.WeaponStarting + currentWeaponAdd, EconomyConfig.SkillStarting + currentSkillAdd, EconomyConfig.CombatStarting + currentCombatAdd } );
		}
	}
}

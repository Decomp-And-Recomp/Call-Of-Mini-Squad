using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpgradeCalcTest : MonoBehaviour
{
	public const int WeaponStarting = 1000, WeaponIncrease = 50, WeaponIncreaseIncrease = 20, SkillStarting = 300, SkillIncrease = 10, CombatStarting = 200, CombatIncrease = 70;

	private static int currentWeaponAdd, currentSkillAdd, currentWeaponIncrease = WeaponIncrease, currentSkillIncrease = SkillIncrease, currentCombatAdd;

	public static List<int[]> upgrades;

	public static void Init()
	{
		upgrades = new List<int[]>();
		currentWeaponAdd = 0;
		currentSkillAdd = 0;
		currentCombatAdd = 0;
		currentWeaponIncrease = WeaponIncrease;
		currentSkillIncrease = SkillIncrease;

		for (int i = 0; i < 30; i++)
		{
			if (i == 0)
			{
				upgrades.Add(new int[3] { WeaponStarting, SkillStarting, CombatStarting } );
				continue;
			}

			currentWeaponAdd += currentWeaponIncrease * i;
			currentSkillAdd += currentSkillIncrease * i;
			currentCombatAdd += CombatIncrease * i;

			currentWeaponIncrease += WeaponIncreaseIncrease;
			currentSkillIncrease += SkillIncrease;

			upgrades.Add(new int[3] { WeaponStarting + currentWeaponAdd, SkillStarting + currentSkillAdd, CombatStarting + currentCombatAdd } );
		}
	}
}

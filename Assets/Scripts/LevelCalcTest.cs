using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelCalcTest
{

	public static List<int[]> LevelRewards = new List<int[]>();

	public static void Init()
	{
		EconomyConfig.EnsureLoaded();
		LevelRewards.Clear();
		int currentCoinAdd = 0, currentXPAdd = 0, currentCombatAdd = 0;

		for (int i = 0; i < EconomyConfig.StageRewardRows; i++)
		{
			if (i == 0)
			{
				LevelRewards.Add(new int[3] { EconomyConfig.CoinsStarting, EconomyConfig.StageXPStarting, EconomyConfig.StageCombatStarting } );
				continue;
			}

			currentCoinAdd += EconomyConfig.CoinsStartIncreasing + (EconomyConfig.CoinsIncrease * (i - 1));
			currentXPAdd += EconomyConfig.StageXPStartIncreasing + (EconomyConfig.StageXPIncrease * (i - 1));
			currentCombatAdd += EconomyConfig.StageCombatStartIncreasing + (EconomyConfig.StageCombatIncrease * (i - 1));

			LevelRewards.Add(new int[3] { EconomyConfig.CoinsStarting + currentCoinAdd, EconomyConfig.StageXPStarting + currentXPAdd, EconomyConfig.StageCombatStarting + currentCombatAdd } );
		}
	}
}

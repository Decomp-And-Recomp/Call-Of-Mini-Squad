using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelRewardController
{
	public static void Get()
	{
		if (LevelCalcTest.LevelRewards.Count == 0)
		{
			LevelCalcTest.Init();
		}

		int index = Mathf.Clamp(GameProgressController.GetRewardIndex(), 0, LevelCalcTest.LevelRewards.Count - 1);
		int[] rewards = LevelCalcTest.LevelRewards[index];

		DataCenter.Save().selectLevelDropData = new LevelDropData
		{
			money = rewards[0],
			exp = rewards[1],
			recommendCombat = rewards[2]
		};
	}
}

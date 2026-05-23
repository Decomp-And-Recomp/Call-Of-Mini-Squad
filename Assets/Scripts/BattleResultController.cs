using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleResultController
{
	public static void Get()
	{
		if (DataCenter.State().battleResult != Defined.BattleResult.Win)
		{
			GameBattle.m_instance.bGetBattleResultData = true;
			return;
		}

		DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode).levelStars[DataCenter.State().selectLevelMode][DataCenter.State().selectAreaNode] = (ushort)DataCenter.State().battleStars;

		AchievementTracker.OnStageCleared();

		if (DataCenter.State().selectAreaNode + 1 <= DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode).levelProgress[(int)DataCenter.State().selectLevelMode])
		{
			if (DataCenter.State().battleStars == 3)
			{
				DataCenter.Save().selectLevelDropData.extraCrystal = 5;
				DataCenter.Save().Crystal += 5;
			}
		}
		else
		{
			DataCenter.Save().selectLevelDropData.extraCrystal = 0;
		}

		GameProgressController.Progress();

		int rewardIndex = GameProgressController.GetRewardIndex();

		int[] baseRewards = new int[2]
		{
			LevelCalcTest.LevelRewards[rewardIndex][0] * (DataCenter.State().battleStars == 3 ? 2 : 1),
			LevelCalcTest.LevelRewards[rewardIndex][1] * (DataCenter.State().battleStars == 3 ? 2 : 1),
		};

		DataCenter.Save().Money += baseRewards[0];
		DataCenter.Save().GetTeamData().teamExp += baseRewards[1];

		TeamLevelController.OnReceivedXP();

		DataCenter.Save().selectLevelDropData.money = baseRewards[0];
		DataCenter.Save().selectLevelDropData.exp = baseRewards[1];

		GameBattle.m_instance.bGetBattleResultData = true;
	}

	public static int GetExtraCrystals()
	{
		if (DataCenter.State().selectAreaNode + 1 <= DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode).levelProgress[(int)DataCenter.State().selectLevelMode])
		{
			return 5;
		}

		return 0;
	}
}

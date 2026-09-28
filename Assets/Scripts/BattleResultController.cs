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

		GameProgressData wpd = DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode);
		if (wpd == null)
		{
			GameBattle.m_instance.bGetBattleResultData = true;
			return;
		}
		Defined.LevelMode mode = DataCenter.State().selectLevelMode;
		int areaNode = DataCenter.State().selectAreaNode;
		ushort oldStars = wpd.levelStars[mode][areaNode];
		ushort newStars = (ushort)DataCenter.State().battleStars;
		if (newStars > oldStars)
		{
			wpd.levelStars[mode][areaNode] = newStars;
		}

		AchievementTracker.OnStageCleared();

		if (newStars == 3 && oldStars < 3)
		{
			DataCenter.Save().selectLevelDropData.extraCrystal = 5;
			DataCenter.Save().Crystal += 5;
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

		Save.RequestWrite();

		GameBattle.m_instance.bGetBattleResultData = true;
	}

	public static int GetExtraCrystals()
	{
		return 5;
		//LevelDropData drop = DataCenter.Save().selectLevelDropData;
		//return (drop != null) ? drop.extraCrystal : 0;
	}
}

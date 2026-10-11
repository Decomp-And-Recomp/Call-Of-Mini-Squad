using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleResultController
{
	public static void Get()
	{
		int battleGold = TakeGoldFromBattle();
		CreditCrystalsFromBattle();

		if (DataCenter.State().battleResult != Defined.BattleResult.Win)
		{
			PayGold(battleGold);
			GameBattle.m_instance.bGetBattleResultData = true;
			return;
		}

		GameProgressData wpd = DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode);
		if (wpd == null)
		{
			PayGold(battleGold);
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
			DataCenter.Save().selectLevelDropData.extraCrystal = EconomyConfig.FirstThreeStarCrystals;
			DataCenter.Save().Crystal += EconomyConfig.FirstThreeStarCrystals;
		}
		else
		{
			DataCenter.Save().selectLevelDropData.extraCrystal = 0;
		}

		GameProgressController.Progress();

		int rewardIndex = GameProgressController.GetRewardIndex();

		int baseMoney = LevelCalcTest.LevelRewards[rewardIndex][0] + battleGold;
		int baseExp = LevelCalcTest.LevelRewards[rewardIndex][1];

		int bonusMultiplier = DataCenter.State().battleStars == 3 ? Mathf.Max(EconomyConfig.ThreeStarMultiplier - 1, 0) : 0;
		int bonusMoney = baseMoney * bonusMultiplier;
		int bonusExp = baseExp * bonusMultiplier;

		DataCenter.Save().Money += baseMoney + bonusMoney;
		DataCenter.Save().GetTeamData().teamExp += baseExp + bonusExp;

		TeamLevelController.OnReceivedXP();

		DataCenter.Save().selectLevelDropData.money = baseMoney;
		DataCenter.Save().selectLevelDropData.exp = baseExp;
		DataCenter.Save().selectLevelDropData.extraMoney = bonusMoney;
		DataCenter.Save().selectLevelDropData.extraExp = bonusExp;

		Save.RequestWrite();

		GameBattle.m_instance.bGetBattleResultData = true;
	}

	private static int TakeGoldFromBattle()
	{
		if (GameBattle.m_instance == null)
		{
			return 0;
		}
		int gold = GameBattle.m_instance.getMoneyInBattle;
		GameBattle.m_instance.getMoneyInBattle = 0;
		return gold;
	}

	private static void PayGold(int gold)
	{
		if (gold <= 0)
		{
			return;
		}
		DataCenter.Save().Money += gold;
		Save.RequestWrite();
	}

	private static void CreditCrystalsFromBattle()
	{
		if (GameBattle.m_instance == null)
		{
			return;
		}
		int crystals = GameBattle.m_instance.getCrystalInBattle;
		if (crystals <= 0)
		{
			return;
		}
		GameBattle.m_instance.getCrystalInBattle = 0;
		DataCenter.Save().Crystal += crystals;
		if (DataCenter.Save().selectLevelDropData != null)
		{
			DataCenter.Save().selectLevelDropData.crystal += crystals;
		}
		Save.RequestWrite();
	}

	public static string GetStarReason()
	{
		if (!DataCenter.Save().BattleTutorialFinished)
		{
			return string.Empty;
		}
		int limit = GameBattle.StarTimeLimitSeconds();
		switch (DataCenter.State().battleStars)
		{
		case 3:
		{
			int seconds = (int)DataCenter.State().battleTime;
			return "Cleared in " + (seconds / 60) + ":" + (seconds % 60).ToString("00") + ", no one lost";
		}
		case 2:
			return "Slower than " + (limit / 60) + ":" + (limit % 60).ToString("00");
		default:
			return DataCenter.State().battleSquadMemberFell ? "A squad member fell" : "Leader below 50% HP";
		}
	}

	public static int GetExtraCrystals()
	{
		if (!DataCenter.Save().BattleTutorialFinished)
		{
			return EconomyConfig.TutorialRewardCrystals;
		}
		return EconomyConfig.FirstThreeStarCrystals;
		//LevelDropData drop = DataCenter.Save().selectLevelDropData;
		//return (drop != null) ? drop.extraCrystal : 0;
	}
}

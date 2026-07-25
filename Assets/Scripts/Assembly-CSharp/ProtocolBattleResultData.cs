using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LitJson;

public class ProtocolBattleResultData : Protocol
{
	public override string GetRequest()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["userId"] = DataCenter.Save().uuid;
		hashtable["worldIndex"] = DataCenter.State().selectWorldNode;
		hashtable["areaIndex"] = DataCenter.State().selectAreaNode;
		hashtable["areaMode"] = DataCenter.State().selectLevelMode;
		hashtable["battleResult"] = DataCenter.State().battleResult;
		hashtable["stars"] = DataCenter.State().battleStars;
		hashtable["addMoney"] = GameBattle.m_instance.getMoneyInBattle;
		hashtable["battleMode"] = ((!DataCenter.Save().squadMode) ? "normal" : "squad");
		hashtable["kills"] = GameBattle.m_instance.KillEnemiesCount;
		hashtable["loginCode"] = DataCenter.Save().loginCode;
		GameBattle.m_instance.bGetBattleResultData = false;
		return JsonMapper.ToJson(hashtable);
	}

	public override int GetResponse(string response)
	{
		GameProgressData wpd = DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode);
		if (wpd == null)
		{
			return 0;
		}
		wpd.levelStars[DataCenter.State().selectLevelMode][DataCenter.State().selectAreaNode] = (ushort)DataCenter.State().battleStars;

		int[] baseRewards = LevelCalcTest.LevelRewards[DataCenter.State().selectWorldNode];
		baseRewards[0] += GameBattle.m_instance.getMoneyInBattle;

		UnityEngine.Debug.LogError(GameBattle.m_instance.getMoneyInBattle);

		if (DataCenter.State().battleStars == 3)
		{
			baseRewards[0] *= 2;
			baseRewards[1] *= 2;

			DataCenter.Save().Crystal += 5;
			DataCenter.Save().selectLevelDropData.crystal = 5;
		}

		DataCenter.Save().Money += baseRewards[0];
		DataCenter.Save().GetTeamData().teamExp += baseRewards[1];

		DataCenter.Save().selectLevelDropData.money = baseRewards[0];
		DataCenter.Save().selectLevelDropData.exp = baseRewards[1];

		GameBattle.m_instance.bGetBattleResultData = true;

		return 0;
	}
}

//ref:-------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

	//DataCenter.Save().GetTeamData().teamExp = int.Parse(jsonData["exp"].ToString());
	//DataCenter.Save().GetTeamData().teamLevel = int.Parse(jsonData["teamLevel"].ToString());
	//DataCenter.Save().GetTeamData().teamMaxExp = (int)jsonData["teamMaxExp"];
	//DataCenter.Save().Money = int.Parse(jsonData["money"].ToString());
	//DataCenter.Save().Crystal = int.Parse(jsonData["crystal"].ToString());
	//DataCenter.Save().Honor = int.Parse(jsonData["honor"].ToString());
	//DataCenter.Save().selectLevelDropData.exp = int.Parse(jsonData["rewardExp"].ToString());
	//DataCenter.Save().selectLevelDropData.money = int.Parse(jsonData["rewardMoney"].ToString());
	//DataCenter.Save().selectLevelDropData.crystal = int.Parse(jsonData["rewardCrystal"].ToString());
	//DataCenter.Save().selectLevelDropData.honor = int.Parse(jsonData["rewardHonor"].ToString());
	//JsonData jsonData3 = jsonData["unlockInfos"];
	//for (int i = 0; i < jsonData3.Count; i++)
	//{
	//	string str = jsonData3[i].ToString();
	//	NewUnlockData.E_NewUnlockType typeByString = NewUnlockData.GetTypeByString(str);
	//	if (!UIConstant.gLsNewUnlockedInfo.Contains(typeByString))
	//	{
	//		UIConstant.gLsNewUnlockedInfo.Add(typeByString);
	//	}
	//}

//-----------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------
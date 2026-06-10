using System.Collections;
using LitJson;

public class ProtocolTeamResetGenius : Protocol
{
	public override string GetRequest()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["userId"] = DataCenter.Save().uuid;
		hashtable["loginCode"] = DataCenter.Save().loginCode;
		return JsonMapper.ToJson(hashtable);
	}

	public override int GetResponse(string response)
	{
		return TalentController.TryReset();
	}

	/*public override int GetResponse(string response)
	{
		try
		{
			JsonData jsonData = JsonMapper.ToObject(response);
			int code = Protocol.GetCode(jsonData);
			if (code != 0)
			{
				return code;
			}
			DataCenter.Save().teamAttributeSaveData.teamAttributeRemainingPoints = int.Parse(jsonData["remainingPoints"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeAssignedPoint = int.Parse(jsonData["AssignedPoint"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeExtraPoint = int.Parse(jsonData["extraPoint"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeExtraPointMax = int.Parse(jsonData["extraPointMax"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeExtraPointCost = int.Parse(jsonData["extraPointCost"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamGeniusfreeResetTimes = int.Parse(jsonData["freeResetTimes"].ToString());
			DataCenter.Save().Money = int.Parse(jsonData["money"].ToString());
			DataCenter.Save().Crystal = int.Parse(jsonData["crystal"].ToString());
			DataCenter.Save().Honor = int.Parse(jsonData["honor"].ToString());
			JsonData jsonData2 = jsonData["geniusList"];
			DataCenter.Save().teamAttributeSaveData.teamAttributeTalent = new TeamAttributeData[jsonData2.Count];
			for (int i = 0; i < jsonData2.Count; i++)
			{
				JsonData jsonData3 = jsonData2[i];
				string s = jsonData3["index"].ToString();
				string s2 = jsonData3["state"].ToString();
				string s3 = jsonData3["level"].ToString();
				string s4 = jsonData3["maxLevel"].ToString();
				string s5 = jsonData3["unlockPoint"].ToString();
				string s6 = jsonData3["cost"].ToString();
				Defined.COST_TYPE costType = GetCostType(jsonData3["costType"].ToString());
				TeamAttributeData teamAttributeData = new TeamAttributeData();
				teamAttributeData.index = int.Parse(s);
				teamAttributeData.state = (Defined.ItemState)int.Parse(s2);
				teamAttributeData.level = int.Parse(s3);
				teamAttributeData.maxLevel = int.Parse(s4);
				teamAttributeData.unlockPoint = int.Parse(s5);
				teamAttributeData.cost = int.Parse(s6);
				teamAttributeData.costType = costType;
				DataCenter.Save().teamAttributeSaveData.teamAttributeTalent[teamAttributeData.index] = teamAttributeData;
			}
			return 0;
		}
		catch (Exception)
		{
			return -1;
		}
	}*/
}

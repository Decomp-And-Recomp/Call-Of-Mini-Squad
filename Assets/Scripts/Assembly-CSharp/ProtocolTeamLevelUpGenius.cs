using System.Collections;
using LitJson;

public class ProtocolTeamLevelUpGenius : Protocol
{
	public override string GetRequest()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["userId"] = DataCenter.Save().uuid;
		hashtable["index"] = DataCenter.State().selectTalentIndex.ToString();
		hashtable["loginCode"] = DataCenter.Save().loginCode;
		return JsonMapper.ToJson(hashtable);
	}

	public override int GetResponse(string response)
	{
		return TalentController.TryLevelUp(DataCenter.State().selectTalentIndex);
	}

	/*
	private int _index = -1;

	public override string GetRequest()
	{
		_index = DataCenter.State().selectTalentIndex;
		Hashtable hashtable = new Hashtable();
		hashtable["userId"] = DataCenter.Save().uuid;
		hashtable["index"] = _index.ToString();
		hashtable["loginCode"] = DataCenter.Save().loginCode;
		return JsonMapper.ToJson(hashtable);
	}

	public override int GetResponse(string response)
	{
		try
		{
			JsonData jsonData = JsonMapper.ToObject(response);
			int code = Protocol.GetCode(jsonData);
			if (code != 0)
			{
				return code;
			}
			DataCenter.Save().teamAttributeSaveData.teamAttributeTalent[_index].level = int.Parse(jsonData["level"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeTalent[_index].maxLevel = int.Parse(jsonData["maxLevel"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeRemainingPoints = int.Parse(jsonData["remainingPoints"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeAssignedPoint = int.Parse(jsonData["AssignedPoint"].ToString());
			DataCenter.Save().Money = (int)jsonData["money"];
			DataCenter.Save().Crystal = (int)jsonData["crystal"];
			DataCenter.Save().Honor = (int)jsonData["honor"];
			if (((IDictionary)jsonData).Contains((object)"geniusList"))
			{
				JsonData jsonData2 = jsonData["geniusList"];
				DataCenter.State().lsUpdatedTalentIndexs.Clear();
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
					TeamAttributeData teamAttributeData = DataCenter.Save().teamAttributeSaveData.teamAttributeTalent[int.Parse(s)];
					teamAttributeData.state = (Defined.ItemState)int.Parse(s2);
					teamAttributeData.level = int.Parse(s3);
					teamAttributeData.maxLevel = int.Parse(s4);
					teamAttributeData.unlockPoint = int.Parse(s5);
					teamAttributeData.cost = int.Parse(s6);
					teamAttributeData.costType = costType;
					DataCenter.State().lsUpdatedTalentIndexs.Add(int.Parse(s));
				}
			}
			return 0;
		}
		catch
		{
			return -1;
		}
	}
	*/
}

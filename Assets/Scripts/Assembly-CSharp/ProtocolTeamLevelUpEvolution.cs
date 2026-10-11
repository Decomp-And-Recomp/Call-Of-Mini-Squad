using System.Collections;
using LitJson;

public class ProtocolTeamLevelUpEvolution : Protocol
{
	private int _index = -1;

	private int useCrystal;

	public override string GetRequest()
	{
		_index = DataCenter.State().selectEvolveIndex;
		useCrystal = DataCenter.State().protocoUseCrystal;
		Hashtable hashtable = new Hashtable();
		hashtable["userId"] = DataCenter.Save().uuid;
		hashtable["index"] = _index.ToString();
		hashtable["useCrystal"] = useCrystal.ToString();
		hashtable["loginCode"] = DataCenter.Save().loginCode;
		return JsonMapper.ToJson(hashtable);
	}

	public override int GetResponse(string response)
	{
		return EvolutionController.TryLevelUp(DataCenter.State().selectEvolveIndex);
	}

	/*
	public override int GetResponse(string response)
	{
		try
		{
			JsonData jsonData = JsonMapper.ToObject(response);
			int code = Protocol.GetCode(jsonData);
			if (((IDictionary)jsonData).Contains((object)"shortMoney"))
			{
				UIConstant.TradeMoneyNotEnough = (int)jsonData["shortMoney"];
			}
			else
			{
				UIConstant.TradeMoneyNotEnough = 0;
			}
			if (((IDictionary)jsonData).Contains((object)"shortHornor"))
			{
				UIConstant.TradeHornorNotEnough = (int)jsonData["shortHornor"];
			}
			else
			{
				UIConstant.TradeHornorNotEnough = 0;
			}
			if (code != 0)
			{
				return code;
			}
			DataCenter.Save().teamAttributeSaveData.teamAttributeEvolve[_index].level = int.Parse(jsonData["level"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeEvolve[_index].maxLevel = int.Parse(jsonData["maxLevel"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeEvolve[_index].cost = int.Parse(jsonData["cost"].ToString());
			DataCenter.Save().teamAttributeSaveData.teamAttributeEvolve[_index].costType = GetCostType(jsonData["costType"].ToString());
			DataCenter.Save().Money = (int)jsonData["money"];
			DataCenter.Save().Crystal = (int)jsonData["crystal"];
			DataCenter.Save().Honor = (int)jsonData["honor"];
			return 0;
		}
		catch
		{
			return -1;
		}
	}
	*/
}

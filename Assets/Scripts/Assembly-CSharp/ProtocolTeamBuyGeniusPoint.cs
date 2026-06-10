using System.Collections;
using LitJson;

public class ProtocolTeamBuyGeniusPoint : Protocol
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
		return TalentController.TryBuyExtraPoint();
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
			DataCenter.Save().Money = (int)jsonData["money"];
			DataCenter.Save().Crystal = (int)jsonData["crystal"];
			DataCenter.Save().Honor = (int)jsonData["honor"];
			return 0;
		}
		catch
		{
			return -1;
		}
	}*/
}

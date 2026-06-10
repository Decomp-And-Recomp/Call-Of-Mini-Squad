using System.Collections;
using LitJson;

public class ProtocolTeamGetGeniusListsInfo : Protocol
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
		return 0;
	}

    /*public override int GetResponse(string response)
	{
		if (!Directory.Exists(Application.persistentDataPath + "/saves"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");
		}

		string path = Application.persistentDataPath + "/saves/teamGeniusList.json";
		string playerDataPath = Application.persistentDataPath + "/saves/playerData.json";

		//if (!File.Exists(path))
		//{
			File.WriteAllText(path, JsonConvert.SerializeObject(new DummyProtocol(), Formatting.Indented));
		//}

		DummyProtocol dummyProtocol = JsonConvert.DeserializeObject<DummyProtocol>(File.ReadAllText(path));

		if (!File.Exists(playerDataPath))
		{
			File.WriteAllText(playerDataPath, JsonConvert.SerializeObject(new ProtocolPlayerData.DummyProtocol(), Formatting.Indented));
		}

		ProtocolPlayerData.DummyProtocol playerDummyProtocol = JsonConvert.DeserializeObject<ProtocolPlayerData.DummyProtocol>(File.ReadAllText(playerDataPath));

		DataCenter.Save().teamAttributeSaveData = dummyProtocol.teamAttributeSaveData;

		DataCenter.Save().Money = playerDummyProtocol.money;
		DataCenter.Save().Crystal = playerDummyProtocol.crystal;
		DataCenter.Save().Honor = playerDummyProtocol.honor;

		DataCenter.Save().teamAttributeSaveData.teamAttributeTalent = dummyProtocol.geniusList;

		DataCenter.Save().teamAttributeSaveData.teamAttributeEvolve = dummyProtocol.evolutionList;
		DataCenter.Save().teamAttributeSaveData.teamGeniusResetCostCrystalPerTimes = 20;

		return 0;
	}

	public class DummyProtocol
	{
		public DataSave.TeamAttributeSaveData teamAttributeSaveData;

		public DummyProtocol()
		{
			teamAttributeSaveData = new DataSave.TeamAttributeSaveData();
			geniusList = new TeamAttributeData[25] { new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData() };
			evolutionList = new TeamAttributeData[25] { new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData(),new TeamAttributeData() };
		}

		public TeamAttributeData[] geniusList, evolutionList;
	}*/
}

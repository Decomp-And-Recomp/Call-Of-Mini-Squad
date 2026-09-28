using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CoMDS2;
using LitJson;
using Newtonsoft.Json;
using UnityEngine;

public class ProtocolPlayerData : Protocol
{
	public override string GetRequest()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["userId"] = DataCenter.Save().uuid;
		hashtable["registerTa"] = ((!(UtilUIAccountManager.mInstance.accountData.email == string.Empty)) ? 1 : 0);
		hashtable["loginCode"] = DataCenter.Save().loginCode;
		string text = JsonMapper.ToJson(hashtable);
		Debug.LogWarning("--------ProtocolPlayerData jsonText : " + text);
		return text;
	}

	public override int GetResponse(string response)
	{
		if (!Directory.Exists(Application.persistentDataPath + "/saves"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");
		}

		string path = Application.persistentDataPath + "/saves/playerData.json";
			
		//if (!File.Exists(path))
		//{
			File.WriteAllText(path, JsonConvert.SerializeObject(new DummyProtocol(), Formatting.Indented));
		//}

		DummyProtocol dummyProtocol = JsonConvert.DeserializeObject<DummyProtocol>(File.ReadAllText(path));

		DataCenter.Save().Money = dummyProtocol.money;
		DataCenter.Save().Crystal = dummyProtocol.crystal;
		DataCenter.Save().Honor = dummyProtocol.honor;

		UIConstant.MoneyExchangRate = dummyProtocol.moneyExchangeRate;
		UIConstant.HornorExchangRate = dummyProtocol.honorExchangeRate;

		DataCenter.Save().GetTeamData().teamLevel = dummyProtocol.teamLevel;
		DataCenter.Save().GetTeamData().teamMaxLevel = dummyProtocol.teamMaxLevel;
		DataCenter.Save().GetTeamData().teamExp = dummyProtocol.teamExp;
		DataCenter.Save().GetTeamData().teamMaxExp = dummyProtocol.teamMaxExp;

		if (dummyProtocol.teamSites != null)
		{
			for (int i = 0; i < dummyProtocol.teamSites.Length; i++)
			{
				DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)i).state = dummyProtocol.teamSites[i].state;
				DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)i).unlockSiteLevel = dummyProtocol.teamSites[i].unlockTeamLevel;
				DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)i).unlockSitePrice = dummyProtocol.teamSites[i].unlockCrystal;
				DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)i).unlockSiteMoney = dummyProtocol.teamSites[i].unlockMoney;
			}
		}

		DataCenter.Save().RemoveAllHeroes();

		if (dummyProtocol.heroes != null)
		{
			for (int j = 0; j < dummyProtocol.heroes.Length; j++)
			{
				DataCenter.Save().AddHero(dummyProtocol.heroes[j]);
	
				if (dummyProtocol.heroes[j].siteNum != -1)
				{
					DataCenter.Save().SetHeroOnTeamSite(dummyProtocol.heroes[j], (Defined.TEAM_SITE)dummyProtocol.heroes[j].siteNum);
				}
			}
		}
		
		if (dummyProtocol.stuffs != null)
		{
			for (int k = 0; k < dummyProtocol.stuffs.Length; k++)
			{
				//uh? fix this if it screws with stuff ig??
				StuffData stuffData = dummyProtocol.stuffs[k];
			}
		}
			
		DataCenter.Save().GetTeamData().talents.Clear();

		if (dummyProtocol.geniusList != null)
		{
			for (int l = 0; l < dummyProtocol.geniusList.Length; l++)
			{
				DataCenter.Save().GetTeamData().talents.Add((TeamSpecialAttribute.TeamAttributeType)dummyProtocol.geniusList[l].index, dummyProtocol.geniusList[l].level);
			}
		}

		DataCenter.Save().GetTeamData().evolves.Clear();
		if (dummyProtocol.evolutionList != null)
		{
			for (int m = 0; m < dummyProtocol.evolutionList.Length; m++)
			{
				DataCenter.Save().GetTeamData().evolves.Add((TeamSpecialAttribute.TeamAttributeEvolveType)dummyProtocol.evolutionList[m].index, dummyProtocol.evolutionList[m].level);
			}
		}

		return 0;
	}

	public class DummyProtocol
	{
		public int money;

		public int crystal;

		public int honor;

		public int moneyExchangeRate;

		public int honorExchangeRate;

		public int teamLevel;

		public int teamMaxLevel;

		public int teamExp;

		public int teamMaxExp;

		public DummyProtocol()
		{
			teamLevel = 1;
			teamSites = new TeamSite[5] { new TeamSite() { state = Defined.ItemState.Available }, new TeamSite() { state = Defined.ItemState.Available }, new TeamSite() { state = Defined.ItemState.Locked }, new TeamSite() { state = Defined.ItemState.Locked }, new TeamSite() { state = Defined.ItemState.Locked } };

			List<PlayerData> heroes = new List<PlayerData>
			{
				new PlayerData
				{
					heroIndex = 0,
					siteNum = 0,
					state = Defined.ItemState.Available,
				},
				new PlayerData
				{
					heroIndex = 1,
					siteNum = 1,
					state = Defined.ItemState.Available,
				},
				new PlayerData
				{
					heroIndex = 9,
					state = Defined.ItemState.Locked,
					unlockNeedTeamLevel = 10,
					costType = Defined.COST_TYPE.Money,
					unlockCost = 10000
				},
				new PlayerData
				{
					heroIndex = 9,
					state = Defined.ItemState.Locked,
					unlockNeedTeamLevel = 10,
					costType = Defined.COST_TYPE.Money,
					unlockCost = 10000
				},
				new PlayerData
				{
					heroIndex = 7,
					state = Defined.ItemState.Locked,
					unlockNeedTeamLevel = 20,
					costType = Defined.COST_TYPE.Money,
					unlockCost = 20000
				},
				new PlayerData
				{
					heroIndex = 3,
					state = Defined.ItemState.Locked,
					unlockNeedTeamLevel = 30,
					costType = Defined.COST_TYPE.Money,
					unlockCost = 30000
				},
				new PlayerData
				{
					heroIndex = 13,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 299
				},
				new PlayerData
				{
					heroIndex = 11,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 299
				},
				new PlayerData
				{
					heroIndex = 10,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 149
				},
				new PlayerData
				{
					heroIndex = 14,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 299
				},
				new PlayerData
				{
					heroIndex = 2,
					state = Defined.ItemState.Purchase,
                    costType = Defined.COST_TYPE.Crystal,
                    unlockCost = 299
                },
				new PlayerData
				{
					heroIndex = 6,
					state = Defined.ItemState.Purchase,
                    costType = Defined.COST_TYPE.Crystal,
                    unlockCost = 399
                },
				new PlayerData
				{
					heroIndex = 12,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 149
				},
				new PlayerData
				{
					heroIndex = 15,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 299
				},
				new PlayerData
				{
					heroIndex = 17,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Crystal,
					unlockCost = 299
				},
				new PlayerData
				{
					heroIndex = 16,
					state = Defined.ItemState.Purchase,
					costType = Defined.COST_TYPE.Money,
					unlockCost = 75000
				},
			};

			foreach (PlayerData hero in heroes)
			{
				hero.equips = new System.Collections.Generic.Dictionary<Defined.EQUIP_TYPE, UserEquipData>
				{
					{Defined.EQUIP_TYPE.Acc, new UserEquipData { currEquipIndex = 22, currEquipLevel = -1 } },
					{Defined.EQUIP_TYPE.Body, new UserEquipData { currEquipIndex = 11, currEquipLevel = -1 } },
					{Defined.EQUIP_TYPE.Head, new UserEquipData { currEquipIndex = 0, currEquipLevel = -1 } }
				};

                hero.upgradeData = new UpgradeData
                {
                    helmsUpgrade = new EquipUpgradeData[11],
					ArmorsUpgrade = new EquipUpgradeData[11],
                    ornamentsUpgrade = new EquipUpgradeData[10]
                };

				hero.weaponLevel = 1;
				hero.skillLevel = 1;

				//placeholder
				hero.weaponMaxLevel = 5;
				hero.skillMaxLevel = 5;

                for (int i = 0; i < 32; i++)
				{
					if (i < 11)
					{
						hero.upgradeData.helmsUpgrade[i] = new EquipUpgradeData() { index = i + 1, equipIndex = i, level = 0, state = Defined.ItemState.Locked, unlockNeedTeamLevel = 30 };
					}
					else if (i < 22)
					{
						hero.upgradeData.ArmorsUpgrade[i - 11] = new EquipUpgradeData() { index = i - 10, equipIndex = i, level = 0, state = Defined.ItemState.Locked, unlockNeedTeamLevel = 30 };
					}
					else
					{
						hero.upgradeData.ornamentsUpgrade[i - 22] = new EquipUpgradeData() { index = i - 21, equipIndex = i, level = 0, state = Defined.ItemState.Locked, unlockNeedTeamLevel = 30 };
					}
				}
			}

			this.heroes = heroes.ToArray();
		}

		public class TeamSite
		{
			public Defined.ItemState state;

			public int unlockTeamLevel;

			public int unlockCrystal;

			public int unlockMoney;
		}

		public TeamSite[] teamSites;

		public PlayerData[] heroes;

		public StuffData[] stuffs;

		public class Genius
		{
			public int index;

			public int level;
		}

		public Genius[] geniusList;

		public class Evolution
		{
			public int index;

			public int level;
		}

		public Evolution[] evolutionList;
	}
}

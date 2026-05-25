using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using CoMDS2;
using LitJson;
using UnityEngine;

public class DataSave
{
	public class StoreSaveData
	{
		public Dictionary<Defined.ITEM_TYPE, Dictionary<int, int>> buyCount;

		public string lastOpenStoreDateTime;

		public StoreSaveData()
		{
			buyCount = new Dictionary<Defined.ITEM_TYPE, Dictionary<int, int>>();
			lastOpenStoreDateTime = DateTime.Now.ToString();
		}
	}

	public class HeroRefreshData
	{
		public Dictionary<Defined.HERO_REFRESH_TYPE, int> heroRefreshCount;

		public HeroRefreshData()
		{
			heroRefreshCount = new Dictionary<Defined.HERO_REFRESH_TYPE, int>();
		}
	}

	public class TeamAttributeSaveData
	{
		public int teamAttributeAssignedPoint;

		public int teamAttributeRemainingPoints;

		public int teamAttributeExtraPoint;

		public int teamAttributeExtraPointMax;

		public int teamAttributeExtraPointCost;

		public bool teamGeniusUnLocked;

		public bool teamEvolutionUnLocked;

		public string teamGeniusUnlockCondition;

		public string teamEvolutionUnlockCondition;

		public int teamGeniusfreeResetTimes;

		public int teamGeniusResetCostCrystalPerTimes;

		public TeamAttributeData[] teamAttributeTalent;

		public TeamAttributeData[] teamAttributeEvolve;
	}

	private const float ENCRYPT_F_KEY = 121f;

	private const int ENCRYPT_I_KEY = 169552957;

	private bool m_bHasSavaData;

	private int s_iHeroIdCounter;

	private int s_iItemIdCounter;

	private string m_sGameVersion = "1.0";

	public bool bNewUser { get; set; }

	private int m_optionMusic;

	private int m_optionSound;

	private int m_optionJoystick;

	private Defined.CameraView cameraView = Defined.CameraView.Default;

	private int m_money;

	private int m_crystal;

	private int m_score;

	private int m_honor;

	private int m_reviveItem;

	private int m_element;

	private int m_goldBoxMaker;

	private int m_premiumRefreshMaker;

	private int m_superRefreshMaker;

	public bool BattleTutorialFinished { get; set; }

	public bool tutorialChangeMode;

	public float lastLoginTime;

	public Defined.TutorialStep tutorialStep = Defined.TutorialStep.None;

	private TeamData m_teamData;

	private List<PlayerData> m_heroes;

	private BagData m_bagData;

	private int m_iNewAreaLevelUnlocked = -1;

	private int m_iNewWorldLevelUnlocked = -1;

	private int m_iNewLevelModeUnlocked = -1;

	public StoreSaveData storeSaveData;

	public HeroRefreshData heroRefreshSaveData;

	private List<GameProgressData> m_gameProgress;

	public bool squadMode = true;

	public TeamAttributeSaveData teamAttributeSaveData;

	private string m_logincode = "0";

	public Dictionary<string, string> configVersion;

	public int achTotalKills;

	public int achIapCrystals;

	public int achDailyKills;

	public int achDailyStages;

	public string achDailyResetDate = string.Empty;

	public HashSet<string> achClaimed = new HashSet<string>();

	public bool m_bManualUseSkill = true;

	public bool m_bCanChangeTeamMember;

	public bool m_bTeamMemberAutoAttack = true;

	public bool m_bRefreshEnemyByTime;

	public float m_fPlayerCautionAreaRadius = 3f;

	public float m_fPlayerCautionSectorRadius = 15f;

	public float m_fPlayerCautionSectorAngle = 160f;

	public float m_fLockRJoystickMaxTime = 0.5f;

	public float m_ffreshEnemyByTime_EnemyCount = 2f;

	public float m_ffreshEnemyByTime_TimeRate = 2f;

	public Enemy.EnemyType m_ifreshEnemyByTime_EnemyType;

	public LevelDropData selectLevelDropData { get; set; }

	public int Radar
	{
		get
		{
			StuffData stuffDataByIndex = GetStuffDataByIndex(DataCenter.Conf().radarIndex);
			if (stuffDataByIndex != null)
			{
				return stuffDataByIndex.count;
			}
			return 0;
		}
		set
		{
			if (value > 0)
			{
				StuffData stuffData = new StuffData();
				stuffData.index = DataCenter.Conf().radarIndex;
				stuffData.count = value;
				AddStuffToBag(stuffData);
			}
			else
			{
				RemoveStuffFromBag(DataCenter.Conf().radarIndex, value);
			}
		}
	}

	public string loginCode
	{
		get
		{
			return m_logincode;
		}
		set
		{
			m_logincode = value;
		}
	}

	public string uuid
	{
		get
		{
			return UtilUIAccountManager.mInstance.accountData.uuid;
		}
	}

	public string userName { get; set; }

	public string language { get; set; }

	public int Money
	{
		get
		{
			return m_money;
		}
		set
		{
			m_money = value;
		}
	}

	public int Crystal
	{
		get
		{
			return m_crystal;
		}
		set
		{
			m_crystal = value;
		}
	}

	public int Score
	{
		get
		{
			return m_score;
		}
		set
		{
			m_score = value;
		}
	}

	public int Honor
	{
		get
		{
			return m_honor;
		}
		set
		{
			m_honor = value;
		}
	}

	public int ReviveItem
	{
		get
		{
			return m_reviveItem;
		}
		set
		{
			m_reviveItem = value;
		}
	}

	public int Element
	{
		get
		{
			return m_element;
		}
		set
		{
			m_element = value;
		}
	}

	public bool PlayMusic
	{
		get
		{
			return m_optionMusic > 0;
		}
		set
		{
			m_optionMusic = (value ? 50 : 0);
			TAudioManager.instance.m_isMusicOn = value;
		}
	}

	public bool PlaySound
	{
		get
		{
			return m_optionSound > 0;
		}
		set
		{
			m_optionSound = (value ? 50 : 0);
			TAudioManager.instance.m_isSoundOn = value;
		}
	}

	public int PremiumRefreshMaker
	{
		get
		{
			return m_premiumRefreshMaker;
		}
		set
		{
			m_premiumRefreshMaker = value;
		}
	}

	public int SuperRefreshMaker
	{
		get
		{
			return m_superRefreshMaker;
		}
		set
		{
			m_superRefreshMaker = value;
		}
	}

	public Defined.CameraView CameraView
	{
		get
		{
			return cameraView;
		}
		set
		{
			cameraView = value;
		}
	}

	public int GetNewAreaLevelUnlocked
	{
		get
		{
			return m_iNewAreaLevelUnlocked;
		}
	}

	public int GetNewLevelModeUnlocked
	{
		get
		{
			return m_iNewLevelModeUnlocked;
		}
	}

	public int GetNewWorldLevelUnlock
	{
		get
		{
			return m_iNewWorldLevelUnlocked;
		}
	}

	public bool HasSaveData
	{
		get
		{
			return m_bHasSavaData;
		}
	}

	public DataSave()
	{
		int num = UnityEngine.Random.Range(0, Defined.SystemUserName.Length);
		userName = Defined.SystemUserName[num];
		m_optionMusic = 50;
		m_optionSound = 50;
		m_optionJoystick = 50;
		m_reviveItem = 20;
		m_element = 0;
		m_goldBoxMaker = 0;
		m_premiumRefreshMaker = 0;
		m_superRefreshMaker = 0;
		Money = 0;
		Crystal = 0;
		m_heroes = new List<PlayerData>();
		m_teamData = new TeamData();
		m_bagData = new BagData();
		PlayMusic = true;
		storeSaveData = new StoreSaveData();
		heroRefreshSaveData = new HeroRefreshData();
		m_gameProgress = new List<GameProgressData>();
		m_gameProgress.Clear();
		GameProgressData gameProgressData = new GameProgressData();
		gameProgressData.levelProgress[0] = 1;
		if (Util.s_debug)
		{
			gameProgressData.levelStars[Defined.LevelMode.Normal] = new ushort[30];
		}
		m_gameProgress.Add(gameProgressData);
	}

	public string GetUUID()
	{
		string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		if (deviceUniqueIdentifier == string.Empty)
		{
			deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		}
		return deviceUniqueIdentifier;
	}

	public PlayerData[] GetHeroList()
	{
		return m_heroes.ToArray();
	}

	public void AddHero(PlayerData data)
	{
		s_iHeroIdCounter++;
		m_heroes.Add(data);
	}

	public void RemoveHero(PlayerData data)
	{
		m_heroes.Remove(data);
	}

	public void RemoveAllHeroes()
	{
		for (int i = 0; i < m_heroes.Count; i++)
		{
			m_heroes[i] = null;
		}
		m_heroes.Clear();
	}

	public PlayerData GetPlayerData(int indexInBagList)
	{
		for (int i = 0; i < m_heroes.Count; i++)
		{
			if (m_heroes[i].heroIndex == indexInBagList)
			{
				return m_heroes[i];
			}
		}
		return null;
	}

	public TeamSiteData[] GetAllTeamSiteData()
	{
		return m_teamData.teamSitesData;
	}

	public TeamData GetTeamData()
	{
		return m_teamData;
	}

	public void SetTeamData(TeamData teamData)
	{
		m_teamData = teamData;
	}

	public TeamSiteData GetTeamSiteData(Defined.TEAM_SITE site)
	{
		return m_teamData.teamSitesData[(int)site];
	}

	public void SetHeroOnTeamSite(PlayerData data, Defined.TEAM_SITE site)
	{
		if (data != null)
		{
			if (m_teamData.teamSitesData[(int)site].playerData != null)
			{
				if (data.siteNum != -1)
				{
					m_teamData.teamSitesData[(int)site].playerData.siteNum = data.siteNum;
					m_teamData.teamSitesData[data.siteNum].playerData = m_teamData.teamSitesData[(int)site].playerData;
				}
				else
				{
					m_teamData.teamSitesData[(int)site].playerData.siteNum = -1;
				}
			}
			else if (data.siteNum != -1)
			{
				m_teamData.teamSitesData[data.siteNum].playerData = null;
			}
			data.siteNum = (int)site;
		}
		m_teamData.teamSitesData[(int)site].playerData = data;
	}

	public void CleanTeamSitePlayerData()
	{
		for (int num = m_teamData.teamSitesData.Length - 1; num >= 0; num--)
		{
			m_teamData.teamSitesData[num].playerData = null;
		}
	}

	public void SetEquipOnTeamSite(UserEquipData data, Defined.TEAM_SITE teamSite, Defined.EQUIP_SITE equipSite)
	{
	}

	public void AddEquipToBag(Defined.EQUIP_TYPE type, UserEquipData data)
	{
	}

	public void RemoveEquipFromBag(Defined.EQUIP_TYPE type, int id)
	{
	}

	public void AddStuffToBag(StuffData data)
	{
		if (m_bagData.otherItems.ContainsKey(data.index))
		{
			m_bagData.otherItems[data.index].count += data.count;
			return;
		}
		s_iItemIdCounter++;
		data.id = s_iItemIdCounter;
		m_bagData.otherItems.Add(data.index, data);
	}

	public void RemoveStuffFromBag(int index, int count)
	{
		if (m_bagData.otherItems.ContainsKey(index))
		{
			m_bagData.otherItems[index].count -= count;
			if (m_bagData.otherItems[index].count <= 0)
			{
				m_bagData.otherItems.Remove(index);
			}
		}
	}

	public StuffData[] GetStuffList()
	{
		List<StuffData> list = new List<StuffData>();
		foreach (StuffData value in m_bagData.otherItems.Values)
		{
			list.Add(value);
		}
		return list.ToArray();
	}

	public StuffData GetStuffDataById(int id)
	{
		foreach (StuffData value in m_bagData.otherItems.Values)
		{
			if (value.id == id)
			{
				return value;
			}
		}
		return null;
	}

	public StuffData GetStuffDataByIndex(int index)
	{
		if (m_bagData.otherItems.ContainsKey(index))
		{
			return m_bagData.otherItems[index];
		}
		return null;
	}

	public bool StuffUseable(int index)
	{
		DataConf.StuffData stuffDataByIndex = DataCenter.Conf().GetStuffDataByIndex(index);
		if (stuffDataByIndex != null)
		{
			return stuffDataByIndex.Useable;
		}
		return false;
	}

	public bool StuffUseCondition(int index)
	{
		DataConf.StuffData stuffDataByIndex = DataCenter.Conf().GetStuffDataByIndex(index);
		if (stuffDataByIndex != null && stuffDataByIndex.stuffType == Defined.STUFF_TYPE.Box)
		{
			DataConf.BoxData boxData = (DataConf.BoxData)stuffDataByIndex;
			int needKey = boxData.needKey;
			if (m_bagData.otherItems.ContainsKey(needKey) || needKey == -1)
			{
				return true;
			}
		}
		return false;
	}

	public bool UseStuff(int index, int count = 1, Defined.TEAM_SITE targetSite = Defined.TEAM_SITE.TEAM_LEADER)
	{
		if (m_bagData.otherItems.ContainsKey(index))
		{
			StuffData stuffData = m_bagData.otherItems[index];
			if (stuffData.count >= count)
			{
				DataConf.StuffData stuffDataByIndex = DataCenter.Conf().GetStuffDataByIndex(index);
				RemoveStuffFromBag(index, count);
				return true;
			}
		}
		return false;
	}

	public bool UseStuff(int index, ref BoxItemData getItem)
	{
		if (m_bagData.otherItems.ContainsKey(index))
		{
			DataConf.StuffData stuffDataByIndex = DataCenter.Conf().GetStuffDataByIndex(index);
			if (stuffDataByIndex.stuffType == Defined.STUFF_TYPE.Box)
			{
				DataConf.BoxData boxData = (DataConf.BoxData)stuffDataByIndex;
				if (boxData.needKey == -1)
				{
					RemoveStuffFromBag(index, 1);
					return true;
				}
				if (m_bagData.otherItems.ContainsKey(boxData.needKey))
				{
					RemoveStuffFromBag(index, 1);
					RemoveStuffFromBag(boxData.needKey, 1);
					if (boxData.boxItemInfo != null && boxData.boxItemInfo.Count > 0)
					{
						int num = UnityEngine.Random.Range(0, 100);
						for (int i = 0; i < boxData.boxItemInfo.Count; i++)
						{
							if (num < boxData.boxItemInfo[i].probability.left || num > boxData.boxItemInfo[i].probability.right)
							{
								continue;
							}
							int num2 = UnityEngine.Random.Range(0, boxData.boxItemInfo[i].index.Length);
							int index2 = boxData.boxItemInfo[i].index[num2];
							DataConf.StuffData stuffDataByIndex2 = DataCenter.Conf().GetStuffDataByIndex(index2);
							if (stuffDataByIndex2.rank != Defined.RANK_TYPE.PURPLE)
							{
								if (boxData.boxRank == Defined.BOX_RANK.Gold)
								{
									if (m_goldBoxMaker == 5 || m_goldBoxMaker % 10 == 0)
									{
										num2 = UnityEngine.Random.Range(0, boxData.systemEncourage.index.Length);
										index2 = boxData.systemEncourage.index[num2];
										stuffDataByIndex2 = DataCenter.Conf().GetStuffDataByIndex(index2);
										getItem.index = index2;
										getItem.count = boxData.systemEncourage.count;
										getItem.itemType = GetBoxBonus(boxData.boxItemInfo[i], index2);
									}
									else
									{
										m_goldBoxMaker++;
										getItem.index = index2;
										getItem.count = boxData.boxItemInfo[i].count;
										getItem.itemType = GetBoxBonus(boxData.boxItemInfo[i], index2);
									}
								}
							}
							else
							{
								if (boxData.boxRank == Defined.BOX_RANK.Gold)
								{
									m_goldBoxMaker -= m_goldBoxMaker % 10;
									m_goldBoxMaker = Mathf.Max(0, m_goldBoxMaker);
								}
								getItem.index = index2;
								getItem.count = boxData.boxItemInfo[i].count;
								getItem.itemType = GetBoxBonus(boxData.boxItemInfo[i], index2);
							}
						}
					}
					return true;
				}
			}
		}
		return false;
	}

	private Defined.ITEM_TYPE GetBoxBonus(DataConf.BoxItemInfo boxInfo, int index)
	{
		switch (boxInfo.type)
		{
		case "money":
			Money += boxInfo.count;
			return Defined.ITEM_TYPE.Money;
		case "stuff":
		{
			StuffData stuffData = new StuffData();
			stuffData.index = index;
			stuffData.count = boxInfo.count;
			AddStuffToBag(stuffData);
			return Defined.ITEM_TYPE.Stuff;
		}
		default:
			return Defined.ITEM_TYPE.Money;
		}
	}

	public BagData GetBagData()
	{
		return m_bagData;
	}

	public bool ItemUpgrade(int money, ref int itemLevel)
	{
		if (money > Money)
		{
			return false;
		}
		Money -= money;
		itemLevel++;
		return true;
	}

	public bool WeaponEvolution(Weapon.WeaponType type, Defined.RANK_TYPE rank)
	{
		return false;
	}

	public void SetGameProgress()
	{
	}

	public bool isGameLevelUnlocked(int worldNodeIndex, int areaNodeIndex, Defined.LevelMode levelMode)
	{
		if (worldNodeIndex < 0 || worldNodeIndex >= m_gameProgress.Count)
		{
			return false;
		}
		if (areaNodeIndex >= m_gameProgress[worldNodeIndex].levelProgress[(int)levelMode])
		{
			return false;
		}
		return true;
	}

	public int GetAreaLevelProgress(int worldNodeIndex, Defined.LevelMode levelMode)
	{
		return m_gameProgress[worldNodeIndex].levelProgress[(int)levelMode];
	}

	public bool isWorldLevelUnlocked(int worldNodeIndex)
	{
		if (worldNodeIndex < 0 || worldNodeIndex >= m_gameProgress.Count)
		{
			return false;
		}
		return true;
	}

	public bool isCurrentGameLevel(int worldNodeIndex, int areaNodeIndex, Defined.LevelMode levelMode)
	{
		if (worldNodeIndex < 0 || worldNodeIndex >= m_gameProgress.Count)
		{
			return false;
		}
		if (areaNodeIndex == m_gameProgress[worldNodeIndex].levelProgress[(int)levelMode] - 1)
		{
			return true;
		}
		return false;
	}

	public bool isCurrentWorldLevel(int worldNodeIndex)
	{
		if (worldNodeIndex < 0 || worldNodeIndex >= m_gameProgress.Count)
		{
			return false;
		}
		if (worldNodeIndex == m_gameProgress.Count - 1)
		{
			return true;
		}
		return false;
	}

	public DataConf.GameLevelNodeData GetCurrentWorldLevelNode()
	{
		DataConf.GameLevelNodeData result = null;
		DataConf.GameLevelNodeData[] gameLevelNodeList = DataCenter.Conf().GetGameLevelNodeList();
		foreach (DataConf.GameLevelNodeData gameLevelNodeData in gameLevelNodeList)
		{
			if (gameLevelNodeData.index == m_gameProgress.Count - 1)
			{
				result = gameLevelNodeData;
				break;
			}
		}
		return result;
	}

	public int GetCurrentUnlockedLevelMode(int worldNode)
	{
		if (worldNode < 0 || worldNode >= m_gameProgress.Count)
		{
			return 0;
		}
		return m_gameProgress[worldNode].modeProgress;
	}

	public void ResetGameProgress()
	{
		m_iNewAreaLevelUnlocked = -1;
		m_iNewWorldLevelUnlocked = -1;
		m_iNewLevelModeUnlocked = -1;
	}

	public void AddWorldNodeProgress(GameProgressData data)
	{
		m_gameProgress.Add(data);
	}

	public void SetWorldNodeProgress(List<GameProgressData> worldProgress)
	{
		m_gameProgress.Clear();
		m_gameProgress = worldProgress;
	}

	public List<GameProgressData> GetWorldNodeProgress()
	{
		return m_gameProgress;
	}

	public ushort GetLevelStars(int worldIndex, Defined.LevelMode mode, int levelIndex)
	{
		if (worldIndex >= m_gameProgress.Count || worldIndex < 0)
		{
			return 0;
		}
		return m_gameProgress[worldIndex].levelStars[mode][levelIndex];
	}

	public ushort[] GetLevelStars(int worldIndex, Defined.LevelMode mode)
	{
		if (worldIndex >= m_gameProgress.Count || worldIndex < 0)
		{
			return null;
		}
		return m_gameProgress[worldIndex].levelStars[mode];
	}

	public GameProgressData GetWorldProgressData(int worldIndex)
	{
		if (worldIndex >= m_gameProgress.Count || worldIndex < 0)
		{
			return null;
		}
		return m_gameProgress[worldIndex];
	}

	public void ResetTeamData()
	{
		m_teamData = new TeamData();
	}

	public void InitGameProgress()
	{
		List<GameProgressData> list = new List<GameProgressData>();
		GameProgressData gameProgressData = new GameProgressData();
		gameProgressData.levelProgress[0] = 1;
		list.Add(gameProgressData);
		SetWorldNodeProgress(list);
	}

	public void ResetTutorial()
	{
		BattleTutorialFinished = false;
		tutorialChangeMode = false;
	}

	public void SaveGameData()
	{
		int num = 0;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "no"));
		XmlElement xmlElement = xmlDocument.CreateElement("GameData");
		xmlElement.SetAttribute("Version", m_sGameVersion);
		xmlElement.SetAttribute("UUID", uuid);
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("Options");
		xmlElement2.SetAttribute("Music", m_optionMusic.ToString());
		xmlElement2.SetAttribute("Sound", m_optionSound.ToString());
		xmlElement2.SetAttribute("Joystick", m_optionJoystick.ToString());
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement3 = xmlDocument.CreateElement("CameraView");
		int num2 = (int)cameraView;
		xmlElement3.SetAttribute("View", num2.ToString());
		xmlElement.AppendChild(xmlElement3);
		XmlElement xmlElement4 = xmlDocument.CreateElement("Tutorail");
		xmlElement4.SetAttribute("BattleTutorial", (BattleTutorialFinished ? 1 : 0).ToString());
		xmlElement4.SetAttribute("tutorialChangeMode", (tutorialChangeMode ? 1 : 0).ToString());
		xmlElement.AppendChild(xmlElement4);
		XmlElement xmlElement5 = xmlDocument.CreateElement("LastLoginTime");
		xmlElement5.SetAttribute("time", lastLoginTime.ToString());
		xmlElement.AppendChild(xmlElement5);
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.NewLineChars = "\r\n";
		xmlWriterSettings.Indent = true;
		xmlWriterSettings.IndentChars = "\t";
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
		xmlDocument.Save(xmlWriter);
		string content = stringBuilder.ToString();
		string zipedcontent = string.Empty;
		Util.ZipString(content, ref zipedcontent);
		string content2 = Util.EncryptData(zipedcontent, "B;g^L%S&K*7630");
		FileUtil.WriteSave("GameData.dat", content2);
	}

	public bool LoadGameData()
	{
		if (Util.s_debug)
		{
			CreateTempTeamMate();
		}
		string text = FileUtil.ReadSave("GameData.dat");
		if (text == null || text.Length <= 0)
		{
			m_bHasSavaData = false;
			CreateGameData();
			SaveGameData();
			return false;
		}
		int num = 0;
		XmlDocument xmlDocument = new XmlDocument();
		string content = Util.DecryptData(text, "B;g^L%S&K*7630");
		string unzipedcontent = string.Empty;
		Util.UnZipString(content, ref unzipedcontent);
		xmlDocument.LoadXml(unzipedcontent);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_sGameVersion = documentElement.GetAttribute("Version");
		XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("Options").Item(0);
		int num2 = int.Parse(xmlElement.GetAttribute("Music"));
		PlayMusic = num2 != 0;
		int num3 = int.Parse(xmlElement.GetAttribute("Sound"));
		PlaySound = num3 != 0;
		m_optionJoystick = int.Parse(xmlElement.GetAttribute("Joystick"));
		xmlElement = (XmlElement)documentElement.GetElementsByTagName("CameraView").Item(0);
		CameraView = (Defined.CameraView)int.Parse(xmlElement.GetAttribute("View"));
		xmlElement = (XmlElement)documentElement.GetElementsByTagName("Tutorail").Item(0);
		if (xmlElement != null)
		{
			BattleTutorialFinished = int.Parse(xmlElement.GetAttribute("BattleTutorial")) == 1;
			tutorialChangeMode = int.Parse(xmlElement.GetAttribute("tutorialChangeMode")) == 1;
		}
		xmlElement = (XmlElement)documentElement.GetElementsByTagName("LastLoginTime").Item(0);
		if (xmlElement != null)
		{
			lastLoginTime = float.Parse(xmlElement.GetAttribute("time"));
		}
		m_bHasSavaData = true;
		return m_bHasSavaData;
	}

	private void CreateTempTeamMate()
	{
		m_teamData.evolves.Add(TeamSpecialAttribute.TeamAttributeEvolveType.MagneticField, 1);
		int[] array = new int[3] { 0, 1, 2 };
		for (int i = 0; i < array.Length; i++)
		{
			if (m_teamData.teamSitesData[i].playerData == null)
			{
				PlayerData playerData = new PlayerData();
				playerData.heroIndex = array[i];
				playerData.siteNum = i;
				m_teamData.teamSitesData[i].playerData = playerData;
			}
		}
	}

	public void CreateTeamForTutorial()
	{
		int[] array = new int[5] { 0, 1, 6, 7, 16 };
		for (int i = 0; i < array.Length; i++)
		{
			if (m_teamData.teamSitesData[i].playerData == null)
			{
				PlayerData playerData = new PlayerData();
				playerData.heroIndex = array[i];
				playerData.siteNum = i;
				m_teamData.teamSitesData[i].playerData = playerData;
			}
		}
	}

	public void CreateGameData()
	{
	}

	public void CreateDummyGameData()
	{
		Money = 99999;
		Crystal = 99999;
		Honor = 9999;
		for (int i = 0; i < 18; i++)
		{
			PlayerData playerData = new PlayerData();
			playerData.heroIndex = i;
			playerData.weaponLevel = 1;
			playerData.weaponStar = 0;
			playerData.skillLevel = 1;
			playerData.skillStar = 0;
			playerData.siteNum = -1;
			playerData.state = Defined.ItemState.Locked;
			playerData.unlockNeedTeamLevel = i;
			playerData.unlockCost = i * 100;
			playerData.costType = Defined.COST_TYPE.Money;
			UserEquipData userEquipData = new UserEquipData();
			userEquipData.currEquipIndex = 1;
			userEquipData.currEquipLevel = 1;
			playerData.equips.Add(Defined.EQUIP_TYPE.Head, userEquipData);
			UserEquipData userEquipData2 = new UserEquipData();
			userEquipData2.currEquipIndex = 16;
			userEquipData2.currEquipLevel = 1;
			playerData.equips.Add(Defined.EQUIP_TYPE.Body, userEquipData2);
			UserEquipData userEquipData3 = new UserEquipData();
			userEquipData3.currEquipIndex = 31;
			userEquipData3.currEquipLevel = 1;
			playerData.equips.Add(Defined.EQUIP_TYPE.Acc, userEquipData3);
			AddHero(playerData);
		}
		for (int j = 0; j < 30; j++)
		{
			StuffData stuffData = new StuffData();
			stuffData.index = j;
			stuffData.count = 0;
			stuffData.cost = 100;
			stuffData.costType = Defined.COST_TYPE.Money;
			AddStuffToBag(stuffData);
		}
		m_teamData.talents.Add(TeamSpecialAttribute.TeamAttributeType.Reinforcement, 1);
		m_teamData.evolves.Add(TeamSpecialAttribute.TeamAttributeEvolveType.Broken, 1);
	}

	public bool LoadServerConfigData()
	{
		string text = FileUtil.ReadSave("/C", "SConfs.dat");
		if (text == null || text.Length <= 0)
		{
			SaveServerConfigData();
			return false;
		}
		int num = 0;
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			string content = Util.DecryptData(text, "C;g^L%S&K*7640");
			string unzipedcontent = string.Empty;
			Util.UnZipString(content, ref unzipedcontent);
			xmlDocument.LoadXml(unzipedcontent);
			XmlElement documentElement = xmlDocument.DocumentElement;
			m_sGameVersion = documentElement.GetAttribute("localVersion");
			if (configVersion != null)
			{
				configVersion.Clear();
			}
			else
			{
				configVersion = new Dictionary<string, string>();
			}
			XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("ConfigData").Item(0);
			foreach (XmlElement item in xmlElement.GetElementsByTagName("ConfigVersion"))
			{
				string attribute = item.GetAttribute("name");
				string attribute2 = item.GetAttribute("version");
				configVersion.Add(attribute, attribute2);
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public void SaveServerConfigData()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "no"));
		XmlElement xmlElement = xmlDocument.CreateElement("ServerConfigData");
		xmlElement.SetAttribute("localVersion", m_sGameVersion);
		XmlElement xmlElement2 = xmlDocument.CreateElement("ConfigData");
		if (configVersion == null)
		{
			configVersion = new Dictionary<string, string>();
			foreach (int value in Enum.GetValues(typeof(DataConf.ConfigType)))
			{
				if (value == 9)
				{
					break;
				}
				string configNameByType = DataCenter.Conf().GetConfigNameByType((DataConf.ConfigType)value);
				configVersion.Add(configNameByType, "0");
			}
		}
		foreach (string key in configVersion.Keys)
		{
			XmlElement xmlElement3 = xmlDocument.CreateElement("ConfigVersion");
			xmlElement3.SetAttribute("name", key);
			xmlElement3.SetAttribute("version", configVersion[key].ToString());
			xmlElement2.AppendChild(xmlElement3);
		}
		xmlElement.AppendChild(xmlElement2);
		xmlDocument.AppendChild(xmlElement);
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.NewLineChars = "\r\n";
		xmlWriterSettings.Indent = true;
		xmlWriterSettings.IndentChars = "\t";
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
		xmlDocument.Save(xmlWriter);
		string content = stringBuilder.ToString();
		string zipedcontent = string.Empty;
		Util.ZipString(content, ref zipedcontent);
		string content2 = Util.EncryptData(zipedcontent, "C;g^L%S&K*7640");
		FileUtil.WriteSave("/C", "SConfs.dat", content2);
	}

	private void Deafult()
	{
		m_optionMusic = 50;
		m_optionSound = 50;
		m_optionJoystick = 50;
	}

	public bool FromJsonText(string jsonText)
	{
		if (jsonText == null)
		{
			return false;
		}
		if (jsonText.Length <= 0)
		{
			return false;
		}
		try
		{
			JsonData jsonData = JsonMapper.ToObject(jsonText);
			m_money = (int)jsonData["money"];
			m_crystal = (int)jsonData["crystal"];
			m_score = (int)jsonData["score"];
			JsonData jsonData2 = jsonData["teamData"];
			m_teamData.teamLevel = (int)jsonData2["teamLevel"];
			m_teamData.teamExp = (int)jsonData2["teamExp"];
			jsonData2 = jsonData["heroes"];
			if (jsonData2 != null)
			{
				m_heroes.Clear();
				for (int i = 0; i < jsonData2.Count; i++)
				{
					JsonData jsonData3 = jsonData2[i];
					PlayerData playerData = new PlayerData();
					playerData.heroIndex = (int)jsonData3["heroIndex"];
					playerData.level = (int)jsonData3["level"];
					playerData.exp = (int)jsonData3["exp"];
					playerData.weaponLevel = (int)jsonData3["weaponLevel"];
					playerData.skillLevel = (int)jsonData3["skillLevel"];
					playerData.siteNum = (int)jsonData3["siteNum"];
					if (playerData.siteNum != -1)
					{
						SetHeroOnTeamSite(playerData, (Defined.TEAM_SITE)playerData.siteNum);
					}
					m_heroes.Add(playerData);
				}
			}
			jsonData2 = jsonData["otherItems"];
			if (jsonData2 != null)
			{
				m_bagData.otherItems.Clear();
				for (int j = 0; j < jsonData2.Count; j++)
				{
					JsonData jsonData4 = jsonData2[j];
					StuffData stuffData = new StuffData();
					stuffData.index = (int)jsonData4["index"];
					stuffData.count = (int)jsonData4["count"];
					m_bagData.otherItems.Add(stuffData.index, stuffData);
				}
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public string ToJsonText()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("money", m_money);
		hashtable.Add("crystal", m_crystal);
		hashtable.Add("score", m_score);
		hashtable.Add("teamData", m_teamData);
		hashtable.Add("restHeroes", m_heroes);
		ArrayList arrayList = new ArrayList();
		foreach (StuffData value in m_bagData.otherItems.Values)
		{
			arrayList.Add(value);
		}
		hashtable.Add("otherItems", arrayList);
		return JsonMapper.ToJson(hashtable);
	}
}

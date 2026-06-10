using System;
using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class HttpRequestHandle : MonoBehaviour
{
	public enum RequestType
	{
		GET_SERVERCONFIGS = 0,
		PVP_INIT = 1,
		PVP_UPLOAD = 2,
		PVP_SEARCHUSERS = 3,
		PVP_GETDATA = 4,
		PVP_GETINFO = 5,
		PVP_BATTLE_START = 6,
		PVP_BATTLE_END = 7,
		PVP_GETTOPS = 8,
		GET_SERVERTIME = 9,
		Get_PlayerData = 10,
		CreatePlayerData = 11,
		Get_WorldNodeList = 12,
		Get_LevelReward = 13,
		BattleResult = 14,
		Store_PurchaseIAP = 15,
		Store_GetShopList = 16,
		Team_GetListGeniusInfos = 17,
		Team_LevelUpGenius = 18,
		Team_BuyGeniusPoint = 19,
		Team_UnlockGenius = 20,
		Team_UnlockEvolution = 21,
		Team_LevelUpEvolution = 22,
		Team_LoadEquipmentInfo = 23,
		Team_LevelUpWeapon = 24,
		Team_LevelUpSkill = 25,
		Team_LevelUpEquipment = 26,
		Team_UseEquipment = 27,
		Team_BuyHero = 28,
		Team_EquipHero = 29,
		Team_BuyTeamSite = 30,
		Store_GetIAPList = 31,
		Team_ResetGenius = 32,
		Team_BreakEquipment = 33,
		Team_UnlockEquipment = 34,
		Arena_SearchPVPUsers = 35,
		Arena_LoadProfile = 36,
		Arena_StartPVP = 37,
		Arena_GetRankList = 38,
		Arena_GetTopList = 39,
		Mail_Probe = 40,
		Mail_Get = 41,
		Mail_ReceiveAccessory = 42,
		Mail_Deleted = 43,
		Mail_Readed = 44,
		Achievement_Get = 45,
		Achievement_ClaimReward = 46,
		Http_LoadFile = 47,
		Chat_SendMsg = 48,
		Chat_GetMsgList = 49,
		Store_VerifyIAP = 50,
		Store_CrystalExchangMoney = 51,
		Phenix_Test = 52,
		Login = 53,
		Lesson = 54,
		CreateUser = 55,
		HeartBeat = 56,
		Account_DeviceLogin = 57,
		Account_Register = 58,
		Account_UserLogin = 59,
		Account_remindEmailPassword = 60
	}

	public delegate void OnRequestFinish(int code);

	public static HttpRequestHandle instance;

	private Dictionary<RequestType, Protocol> m_requestMap;

	private Dictionary<RequestType, OnRequestFinish> m_requestCallBack;

	public float m_fLastLoginTime;

	public long serverTimeSeconds = -1L;

	private void Awake()
	{
		instance = this;
		Init("", "", "");
	}

	public void Init(string gameURL, string accountURL, string chatURL)
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
		m_requestCallBack = new Dictionary<RequestType, OnRequestFinish>();
		HttpClient.Instance().AddServer("accountServer", accountURL, 20f, "A;g^L%S&K*7620");
		HttpClient.Instance().AddServer("chatSever", chatURL, 20f, "A;g^L%S&K*7620");
		HttpClient.Instance().AddServer("gameSever", gameURL, 20f, "A;g^L%S&K*7620");
		m_requestMap = new Dictionary<RequestType, Protocol>();
		ProtocolGetServerConfigs protocolGetServerConfigs = new ProtocolGetServerConfigs();
		protocolGetServerConfigs.Initialize("gameSever", "userHandler.loadConfigs");
		m_requestMap.Add(RequestType.GET_SERVERCONFIGS, protocolGetServerConfigs);
		ProtocolGetServerTime protocolGetServerTime = new ProtocolGetServerTime();
		protocolGetServerTime.Initialize("gameSever", "userHandler.getServerTime");
		m_requestMap.Add(RequestType.GET_SERVERTIME, protocolGetServerTime);
		ProtocolPVPInit protocolPVPInit = new ProtocolPVPInit();
		protocolPVPInit.Initialize("gameSever", "userHandler.initPvp");
		m_requestMap.Add(RequestType.PVP_INIT, protocolPVPInit);
		ProtocolPVPUpload protocolPVPUpload = new ProtocolPVPUpload();
		protocolPVPUpload.Initialize("gameSever", "userHandler.uploadPvp");
		m_requestMap.Add(RequestType.PVP_UPLOAD, protocolPVPUpload);
		ProtocolPVPSearchUsers protocolPVPSearchUsers = new ProtocolPVPSearchUsers();
		protocolPVPSearchUsers.Initialize("gameSever", "userHandler.searchPvpUsers");
		m_requestMap.Add(RequestType.PVP_SEARCHUSERS, protocolPVPSearchUsers);
		ProtocolPVPGetData protocolPVPGetData = new ProtocolPVPGetData();
		protocolPVPGetData.Initialize("gameSever", "userHandler.getPvpData");
		m_requestMap.Add(RequestType.PVP_GETDATA, protocolPVPGetData);
		ProtocolPVPGetInfo protocolPVPGetInfo = new ProtocolPVPGetInfo();
		protocolPVPGetInfo.Initialize("gameSever", "userHandler.getPvpInfo");
		m_requestMap.Add(RequestType.PVP_GETINFO, protocolPVPGetInfo);
		ProtocolPVPBattleStart protocolPVPBattleStart = new ProtocolPVPBattleStart();
		protocolPVPBattleStart.Initialize("gameSever", "userHandler.startPvp");
		m_requestMap.Add(RequestType.PVP_BATTLE_START, protocolPVPBattleStart);
		ProtocolPVPBattleEnd protocolPVPBattleEnd = new ProtocolPVPBattleEnd();
		protocolPVPBattleEnd.Initialize("gameSever", "userHandler.endPvp");
		m_requestMap.Add(RequestType.PVP_BATTLE_END, protocolPVPBattleEnd);
		ProtocolPVPGetTops protocolPVPGetTops = new ProtocolPVPGetTops();
		protocolPVPGetTops.Initialize("gameSever", "userHandler.listTops");
		m_requestMap.Add(RequestType.PVP_GETTOPS, protocolPVPGetTops);
		ProtocolPlayerData protocolPlayerData = new ProtocolPlayerData();
		protocolPlayerData.Initialize("gameSever", "userHandler.loadProfile");
		m_requestMap.Add(RequestType.Get_PlayerData, protocolPlayerData);
		ProtocolCreatePlayerData protocolCreatePlayerData = new ProtocolCreatePlayerData();
		protocolCreatePlayerData.Initialize("gameSever", "userHandler.createUserName");
		m_requestMap.Add(RequestType.CreatePlayerData, protocolCreatePlayerData);
		ProtocolGetWorldNodesData protocolGetWorldNodesData = new ProtocolGetWorldNodesData();
		protocolGetWorldNodesData.Initialize("gameSever", "userHandler.loadWorldNodes");
		m_requestMap.Add(RequestType.Get_WorldNodeList, protocolGetWorldNodesData);
		ProtocolGetLevelRewardData protocolGetLevelRewardData = new ProtocolGetLevelRewardData();
		protocolGetLevelRewardData.Initialize("gameSever", "userHandler.getWorldAreaReward");
		m_requestMap.Add(RequestType.Get_LevelReward, protocolGetLevelRewardData);
		ProtocolBattleResultData protocolBattleResultData = new ProtocolBattleResultData();
		protocolBattleResultData.Initialize("gameSever", "userHandler.battleWorldArea");
		m_requestMap.Add(RequestType.BattleResult, protocolBattleResultData);
		ProtocolStorePurchaseIAP protocolStorePurchaseIAP = new ProtocolStorePurchaseIAP();
		protocolStorePurchaseIAP.Initialize("gameSever", "userHandler.purchaseIap");
		m_requestMap.Add(RequestType.Store_PurchaseIAP, protocolStorePurchaseIAP);
		ProtocolStroreGetShopList protocolStroreGetShopList = new ProtocolStroreGetShopList();
		protocolStroreGetShopList.Initialize("gameSever", "userHandler.shopList");
		m_requestMap.Add(RequestType.Store_GetShopList, protocolStroreGetShopList);
		ProtocolTeamGetGeniusListsInfo protocolTeamGetGeniusListsInfo = new ProtocolTeamGetGeniusListsInfo();
		protocolTeamGetGeniusListsInfo.Initialize("gameSever", "userHandler.listGeniusInfos");
		m_requestMap.Add(RequestType.Team_GetListGeniusInfos, protocolTeamGetGeniusListsInfo);
		ProtocolTeamLevelUpGenius protocolTeamLevelUpGenius = new ProtocolTeamLevelUpGenius();
		protocolTeamLevelUpGenius.Initialize("gameSever", "userHandler.levelUpGenius");
		m_requestMap.Add(RequestType.Team_LevelUpGenius, protocolTeamLevelUpGenius);
		ProtocolTeamBuyGeniusPoint protocolTeamBuyGeniusPoint = new ProtocolTeamBuyGeniusPoint();
		protocolTeamBuyGeniusPoint.Initialize("gameSever", "userHandler.buyGeniusPoint");
		m_requestMap.Add(RequestType.Team_BuyGeniusPoint, protocolTeamBuyGeniusPoint);
		ProtocolTeamUnlockGenius protocolTeamUnlockGenius = new ProtocolTeamUnlockGenius();
		protocolTeamUnlockGenius.Initialize("gameSever", "userHandler.unlockGenius");
		m_requestMap.Add(RequestType.Team_UnlockGenius, protocolTeamUnlockGenius);
		ProtocolTeamUnlockEvolution protocolTeamUnlockEvolution = new ProtocolTeamUnlockEvolution();
		protocolTeamUnlockEvolution.Initialize("gameSever", "userHandler.unlockEvolution");
		m_requestMap.Add(RequestType.Team_UnlockEvolution, protocolTeamUnlockEvolution);
		ProtocolTeamLevelUpEvolution protocolTeamLevelUpEvolution = new ProtocolTeamLevelUpEvolution();
		protocolTeamLevelUpEvolution.Initialize("gameSever", "userHandler.levelUpEvolution");
		m_requestMap.Add(RequestType.Team_LevelUpEvolution, protocolTeamLevelUpEvolution);
		ProtocolTeamLoadEquipmentInfo protocolTeamLoadEquipmentInfo = new ProtocolTeamLoadEquipmentInfo();
		protocolTeamLoadEquipmentInfo.Initialize("gameSever", "userHandler.loadEquipmentInfo");
		m_requestMap.Add(RequestType.Team_LoadEquipmentInfo, protocolTeamLoadEquipmentInfo);
		ProtocolTeamLevelUpWeapon protocolTeamLevelUpWeapon = new ProtocolTeamLevelUpWeapon();
		protocolTeamLevelUpWeapon.Initialize("gameSever", "userHandler.levelUpWeapon");
		m_requestMap.Add(RequestType.Team_LevelUpWeapon, protocolTeamLevelUpWeapon);
		ProtocolTeamLevelUpSkill protocolTeamLevelUpSkill = new ProtocolTeamLevelUpSkill();
		protocolTeamLevelUpSkill.Initialize("gameSever", "userHandler.levelUpSkill");
		m_requestMap.Add(RequestType.Team_LevelUpSkill, protocolTeamLevelUpSkill);
		ProtocolTeamLevelUpEquipment protocolTeamLevelUpEquipment = new ProtocolTeamLevelUpEquipment();
		protocolTeamLevelUpEquipment.Initialize("gameSever", "userHandler.levelUpEquipment");
		m_requestMap.Add(RequestType.Team_LevelUpEquipment, protocolTeamLevelUpEquipment);
		ProtocolTeamUseEquipment protocolTeamUseEquipment = new ProtocolTeamUseEquipment();
		protocolTeamUseEquipment.Initialize("gameSever", "userHandler.useEquipment");
		m_requestMap.Add(RequestType.Team_UseEquipment, protocolTeamUseEquipment);
		ProtocolTeamBuyHero protocolTeamBuyHero = new ProtocolTeamBuyHero();
		protocolTeamBuyHero.Initialize("gameSever", "userHandler.buyHero");
		m_requestMap.Add(RequestType.Team_BuyHero, protocolTeamBuyHero);
		ProtocolTeamEquipHero protocolTeamEquipHero = new ProtocolTeamEquipHero();
		protocolTeamEquipHero.Initialize("gameSever", "userHandler.equipHero");
		m_requestMap.Add(RequestType.Team_EquipHero, protocolTeamEquipHero);
		ProtocolTeamBuyTeamSite protocolTeamBuyTeamSite = new ProtocolTeamBuyTeamSite();
		protocolTeamBuyTeamSite.Initialize("gameSever", "userHandler.buyTeamSite");
		m_requestMap.Add(RequestType.Team_BuyTeamSite, protocolTeamBuyTeamSite);
		ProtocolStoreGetIAPList protocolStoreGetIAPList = new ProtocolStoreGetIAPList();
		protocolStoreGetIAPList.Initialize("gameSever", "userHandler.iapList");
		m_requestMap.Add(RequestType.Store_GetIAPList, protocolStoreGetIAPList);
		ProtocolTeamResetGenius protocolTeamResetGenius = new ProtocolTeamResetGenius();
		protocolTeamResetGenius.Initialize("gameSever", "userHandler.resetGeniuses");
		m_requestMap.Add(RequestType.Team_ResetGenius, protocolTeamResetGenius);
		ProtocolTeamBreakthroughEquipment protocolTeamBreakthroughEquipment = new ProtocolTeamBreakthroughEquipment();
		protocolTeamBreakthroughEquipment.Initialize("gameSever", "userHandler.breakthroughEquipment");
		m_requestMap.Add(RequestType.Team_BreakEquipment, protocolTeamBreakthroughEquipment);
		ProtocolTeamUnlockEquipment protocolTeamUnlockEquipment = new ProtocolTeamUnlockEquipment();
		protocolTeamUnlockEquipment.Initialize("gameSever", "userHandler.unlockEquipment");
		m_requestMap.Add(RequestType.Team_UnlockEquipment, protocolTeamUnlockEquipment);
		ProtocolArenaSearchPVPUsers protocolArenaSearchPVPUsers = new ProtocolArenaSearchPVPUsers();
		protocolArenaSearchPVPUsers.Initialize("gameSever", "userHandler.searchPvpUsers");
		m_requestMap.Add(RequestType.Arena_SearchPVPUsers, protocolArenaSearchPVPUsers);
		ProtocolArenaLoadProfile protocolArenaLoadProfile = new ProtocolArenaLoadProfile();
		protocolArenaLoadProfile.Initialize("gameSever", "userHandler.loadPvpProfile");
		m_requestMap.Add(RequestType.Arena_LoadProfile, protocolArenaLoadProfile);
		ProtocolArenaStartPVP protocolArenaStartPVP = new ProtocolArenaStartPVP();
		protocolArenaStartPVP.Initialize("gameSever", "userHandler.startPvp");
		m_requestMap.Add(RequestType.Arena_StartPVP, protocolArenaStartPVP);
		ProtocolArenaGetRankList protocolArenaGetRankList = new ProtocolArenaGetRankList();
		protocolArenaGetRankList.Initialize("gameSever", "userHandler.listRanks");
		m_requestMap.Add(RequestType.Arena_GetRankList, protocolArenaGetRankList);
		ProtocolArenaGetTopList protocolArenaGetTopList = new ProtocolArenaGetTopList();
		protocolArenaGetTopList.Initialize("gameSever", "userHandler.listTops");
		m_requestMap.Add(RequestType.Arena_GetTopList, protocolArenaGetTopList);
		ProtocolProbeMails protocolProbeMails = new ProtocolProbeMails();
		protocolProbeMails.Initialize("gameSever", "messageHandler.newMessages");
		m_requestMap.Add(RequestType.Mail_Probe, protocolProbeMails);
		ProtocolGetMailMessages protocolGetMailMessages = new ProtocolGetMailMessages();
		protocolGetMailMessages.Initialize("gameSever", "messageHandler.listMessages");
		m_requestMap.Add(RequestType.Mail_Get, protocolGetMailMessages);
		ProtocolReceiveMailAccessory protocolReceiveMailAccessory = new ProtocolReceiveMailAccessory();
		protocolReceiveMailAccessory.Initialize("gameSever", "messageHandler.receiveMessageItems");
		m_requestMap.Add(RequestType.Mail_ReceiveAccessory, protocolReceiveMailAccessory);
		ProtocolDeleteMail protocolDeleteMail = new ProtocolDeleteMail();
		protocolDeleteMail.Initialize("gameSever", "messageHandler.removeMessages");
		m_requestMap.Add(RequestType.Mail_Deleted, protocolDeleteMail);
		ProtocolReadMails protocolReadMails = new ProtocolReadMails();
		protocolReadMails.Initialize("gameSever", "messageHandler.readMessages");
		m_requestMap.Add(RequestType.Mail_Readed, protocolReadMails);
		ProtocolAchievementGetList protocolAchievementGetList = new ProtocolAchievementGetList();
		protocolAchievementGetList.Initialize("gameSever", "userHandler.listQuests");
		m_requestMap.Add(RequestType.Achievement_Get, protocolAchievementGetList);
		ProtocolAchievementClaimReward protocolAchievementClaimReward = new ProtocolAchievementClaimReward();
		protocolAchievementClaimReward.Initialize("gameSever", "userHandler.getQuestReward");
		m_requestMap.Add(RequestType.Achievement_ClaimReward, protocolAchievementClaimReward);
		ProtocolNoticeLoadFile protocolNoticeLoadFile = new ProtocolNoticeLoadFile();
		protocolNoticeLoadFile.Initialize("gameSever", "userHandler.loadFile");
		m_requestMap.Add(RequestType.Http_LoadFile, protocolNoticeLoadFile);
		ProtocolChatSendMsg protocolChatSendMsg = new ProtocolChatSendMsg();
		protocolChatSendMsg.Initialize("chatSever", "messageHandler.chat");
		m_requestMap.Add(RequestType.Chat_SendMsg, protocolChatSendMsg);
		ProtocolChatGetMsgList protocolChatGetMsgList = new ProtocolChatGetMsgList();
		protocolChatGetMsgList.Initialize("chatSever", "messageHandler.listChats");
		m_requestMap.Add(RequestType.Chat_GetMsgList, protocolChatGetMsgList);
		ProtocolStoreVerifyIAP protocolStoreVerifyIAP = new ProtocolStoreVerifyIAP();
		protocolStoreVerifyIAP.Initialize("gameSever", "userHandler.queryIap");
		m_requestMap.Add(RequestType.Store_VerifyIAP, protocolStoreVerifyIAP);
		ProtocolStoreCrystalExchangMoney protocolStoreCrystalExchangMoney = new ProtocolStoreCrystalExchangMoney();
		protocolStoreCrystalExchangMoney.Initialize("gameSever", "userHandler.exchangeMoney");
		m_requestMap.Add(RequestType.Store_CrystalExchangMoney, protocolStoreCrystalExchangMoney);
		ProtocolPhenixTest protocolPhenixTest = new ProtocolPhenixTest();
		protocolPhenixTest.Initialize("gameSever", "userHandler.addMoneys");
		m_requestMap.Add(RequestType.Phenix_Test, protocolPhenixTest);
		ProtocolLogin protocolLogin = new ProtocolLogin();
		protocolLogin.Initialize("gameSever", "userHandler.login");
		m_requestMap.Add(RequestType.Login, protocolLogin);
		ProtocolLesson protocolLesson = new ProtocolLesson();
		protocolLesson.Initialize("gameSever", "userHandler.lesson");
		m_requestMap.Add(RequestType.Lesson, protocolLesson);
		ProtocolCreateUser protocolCreateUser = new ProtocolCreateUser();
		protocolCreateUser.Initialize("gameSever", "userHandler.createUser");
		m_requestMap.Add(RequestType.CreateUser, protocolCreateUser);
		ProtocolHeartBeat protocolHeartBeat = new ProtocolHeartBeat();
		protocolHeartBeat.Initialize("gameSever", "userHandler.heartbeat");
		m_requestMap.Add(RequestType.HeartBeat, protocolHeartBeat);
		ProtocolAccountDeviceLogin protocolAccountDeviceLogin = new ProtocolAccountDeviceLogin();
		protocolAccountDeviceLogin.Initialize("accountServer", "taHandler.deviceLogin");
		m_requestMap.Add(RequestType.Account_DeviceLogin, protocolAccountDeviceLogin);
		ProtocolAccountRegister protocolAccountRegister = new ProtocolAccountRegister();
		protocolAccountRegister.Initialize("accountServer", "taHandler.register");
		m_requestMap.Add(RequestType.Account_Register, protocolAccountRegister);
		ProtocolAccountUserLogin protocolAccountUserLogin = new ProtocolAccountUserLogin();
		protocolAccountUserLogin.Initialize("accountServer", "taHandler.userLogin");
		m_requestMap.Add(RequestType.Account_UserLogin, protocolAccountUserLogin);
		ProtocolAccountRemindEmailPassword protocolAccountRemindEmailPassword = new ProtocolAccountRemindEmailPassword();
		protocolAccountRemindEmailPassword.Initialize("accountServer", "taHandler.remindEmailPassword");
		m_requestMap.Add(RequestType.Account_remindEmailPassword, protocolAccountRemindEmailPassword);
	}

	public void ReqServerTime()
	{
		instance.SendRequest(RequestType.GET_SERVERTIME, null);
	}

	public void Update()
	{
		HttpClient.Instance().HandleResponse();
	}

	private static List<RequestType> finishedRequestTypes = new List<RequestType>
	{
		RequestType.Get_WorldNodeList,
		RequestType.Get_PlayerData,
		RequestType.Get_LevelReward,
		RequestType.Team_GetListGeniusInfos,
		RequestType.HeartBeat,
		RequestType.Chat_GetMsgList,
		RequestType.Chat_SendMsg,
		RequestType.Team_BuyTeamSite,
		RequestType.Team_LevelUpEquipment,
		RequestType.Team_UnlockEquipment,
		RequestType.Team_UseEquipment
	};

	private static List<RequestType> locallyImplementedRequestTypes = new List<RequestType>
	{
		RequestType.Team_GetListGeniusInfos,
		RequestType.Team_LevelUpGenius,
		RequestType.Team_UnlockGenius,
		RequestType.Team_BuyGeniusPoint,
		RequestType.Team_ResetGenius
	};

	public void SendRequest(RequestType requestType, OnRequestFinish callBack)
	{
		if (finishedRequestTypes.Contains(requestType))
		{
			Debug.Log("tried requesting " + requestType.ToString());
		}
		else
		{
			Debug.LogError("UNCHECKED REQUEST!! " + requestType.ToString());
		}
		int code = 0;
		if (locallyImplementedRequestTypes.Contains(requestType) && m_requestMap.ContainsKey(requestType))
		{
			code = m_requestMap[requestType].GetResponse(string.Empty);
		}
		if (callBack != null)
		{
			callBack(code);
		}
		return;
		if (!Util.IsNetworkConnected())
		{
			UIDialogManager.Instance.ShowPopupA("Check your internet.", UIWidget.Pivot.Center, false, OnSystemNetError);
			return;
		}
		if (callBack != null)
		{
			if (m_requestCallBack.ContainsKey(requestType))
			{
				m_requestCallBack[requestType] = callBack;
			}
			else
			{
				m_requestCallBack.Add(requestType, callBack);
			}
		}
		if (m_requestMap.ContainsKey(requestType))
		{
			HttpClient.Instance().SendRequest(m_requestMap[requestType].ServerName, requestType, m_requestMap[requestType].ProtocolAction, m_requestMap[requestType].GetRequest(), string.Empty);
		}
	}

	public void OnSystemNetError()
	{
		if (GameBattle.m_instance != null)
		{
			GameBattle.m_instance.OnBreakOff();
		}
		DataCenter.State().ResetData(false);
		Application.LoadLevel("UICheckUpdate");
	}

	public void OnRequest(int taskId, int result, string server, RequestType requestType, string action, string response, string param)
	{
		Debug.LogError(string.Concat("taskId:", taskId, " result:", result, " server:", server, " requestType:", requestType, " action:", action, " response:", response, " param:", param));
		return;
		if (requestType == RequestType.Login)
		{
			return;
		}
		if (result != 0 && requestType != RequestType.Chat_GetMsgList)
		{
			if (!Util.IsNetworkConnected())
			{
				UIDialogManager.Instance.ShowPopupA("No network available now. Please check network connection.", UIWidget.Pivot.Center, false, OnSystemNetError);
			}
			else if (result == -6)
			{
				UIDialogManager.Instance.ShowPopupA("Connection Timeout. Please try again later.", UIWidget.Pivot.Center, false, OnSystemNetError);
			}
			else
			{
				UIDialogManager.Instance.ShowPopupA("Unable to connect to server. Please try again later.", UIWidget.Pivot.Center, false, OnSystemNetError);
			}
			return;
		}
		try
		{
			int code = -1;
			if (m_requestMap.ContainsKey(requestType))
			{
				code = m_requestMap[requestType].GetResponse(response);
			}
			if (m_requestCallBack.ContainsKey(requestType))
			{
				m_requestCallBack[requestType](code);
				m_requestCallBack.Remove(requestType);
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (DataCenter.State().gameLoaded)
		{
			if (pause)
			{
				DataCenter.Save().lastLoginTime += Time.realtimeSinceStartup - m_fLastLoginTime;
				DataCenter.Save().SaveGameData();
			}
			else
			{
				SetLogin();
			}
		}
	}

	public void SetLogin()
	{
		m_fLastLoginTime = Time.realtimeSinceStartup;
		SendRequest(RequestType.Login, null);
		DataCenter.Save().lastLoginTime = 0f;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class UINewBaseManager : MonoBehaviour
{
	protected enum PageType
	{
		E_WorldMap = 0,
		E_AreaMap = 1,
		E_ConfirmWave = 2
	}

	private static UINewBaseManager mInstance;

	[SerializeField]
	private UtilUIPropertyInfo m_scriptUIPropertyInfo;

	[SerializeField]
	private UtilUIBaseTeamInfo m_scriptTeamInfo;

	[SerializeField]
	private UtilUIBaseMapInfo m_scriptUIBaseMapInfo;

	[SerializeField]
	private UtilUIBaseConfirmBattleInfo m_scriptUIBaseConfirmBattleInfo;

	[SerializeField]
	private UtilUIBaseOpitionInfo m_scritpUIBaseOpitionInfo;

	[SerializeField]
	private UtilUIChatInfo m_scritpUIBaseChatInfo;

	[SerializeField]
	private UtilUIBaseCreateNameInfo m_scritpUIBaseCreatePlayerInfo;

	[SerializeField]
	private UtilUICourseInfo m_scriptUIBaseCourseInfo;

	[SerializeField]
	private GameObject BaseBtnNewTipsGO;

	[SerializeField]
	private GameObject MailBtnNewTipsGO;

	[SerializeField]
	private GameObject AchievementBtnNewTipsGO;

	[SerializeField]
	private GameObject mapBlock;

	protected PageType nowPageType;

	public GameObject confirmBattleStartGO;

	public GameObject teamBtnGO;

	public int coureseType = -1;

	public static UINewBaseManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = GameObject.Find("UIManager").GetComponent<UINewBaseManager>();
			}
			return mInstance;
		}
	}

	public UtilUIPropertyInfo UIPROPERTYINFO
	{
		get
		{
			return m_scriptUIPropertyInfo;
		}
	}

	public UtilUIBaseTeamInfo UITEAMINFO
	{
		get
		{
			return m_scriptTeamInfo;
		}
	}

	public UtilUIBaseMapInfo UIBASEMAPINFO
	{
		get
		{
			return m_scriptUIBaseMapInfo;
		}
	}

	public UtilUIBaseConfirmBattleInfo UIBASECONFIRMBATTLEINFO
	{
		get
		{
			return m_scriptUIBaseConfirmBattleInfo;
		}
	}

	public UtilUIBaseOpitionInfo UIBASEOPTITIONINFO
	{
		get
		{
			return m_scritpUIBaseOpitionInfo;
		}
	}

	public UtilUIChatInfo UIBASECHATINFO
	{
		get
		{
			return m_scritpUIBaseChatInfo;
		}
	}

	public UtilUIBaseCreateNameInfo UIBASECREATEPLAYERINFO
	{
		get
		{
			return m_scritpUIBaseCreatePlayerInfo;
		}
	}

	public UtilUICourseInfo UIBASECOURSEINFO
	{
		get
		{
			return m_scriptUIBaseCourseInfo;
		}
	}

	public void AddStaticCourse()
	{
		if (DataCenter.Save().tutorialStep == Defined.TutorialStep.CreateNameFinish || DataCenter.Save().tutorialStep == Defined.TutorialStep.UnlockedTeamSite || DataCenter.Save().tutorialStep == Defined.TutorialStep.EnterBattle || DataCenter.Save().tutorialStep == Defined.TutorialStep.FinishStageOneWaveOne)
		{
			coureseType = 0;
			UIBASECOURSEINFO.AddCursor(11, teamBtnGO, teamBtnGO.transform.position);
			UIBASECOURSEINFO.GetCourse(11).SetMoveToUnderTargetTrans(teamBtnGO.transform.Find("CoureseTrans"));
		}
		else if (DataCenter.Save().tutorialStep == Defined.TutorialStep.SetTeam)
		{
			coureseType = 1;
			UIBASECOURSEINFO.AddCursor(10, confirmBattleStartGO, confirmBattleStartGO.transform.position);
		}
	}

	private void Start()
	{
		UIDialogManager.Instance.SetPropertyScript(UIPROPERTYINFO);
		BlindFunction();
		AddStaticCourse();
		Init();
		CheckBaseNewThingsUnlocked(false, true);
		CheckNewMsg();
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.HeartBeat, OnGetHeartBeatFinished);
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 47);
		UIDialogManager.Instance.ShowBlock(47);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_PlayerData, OnGetPlayerDataFinished);
		if (DataCenter.State().lastSceneType == Defined.SceneType.Battle)
		{
			if (DataCenter.Save().bNewUser)
			{
				DataConf.GameLevelNodeData currentWorldLevelNode = DataCenter.Save().GetCurrentWorldLevelNode();
				EnterWorldMapImmediately(SolidMapCameraControl.mInstance.levelPointsInfo[currentWorldLevelNode.index].stagePartStageCameraTrans.localPosition);
			}
			else
			{
				CheckNewUnlockFromBattle(DataCenter.Save().GetNewAreaLevelUnlocked, DataCenter.Save().GetNewLevelModeUnlocked, DataCenter.Save().GetNewWorldLevelUnlock);
			}
		}
		else if (UIUtil.bStageMapChangedUI)
		{
			DataConf.GameLevelNodeData currentWorldLevelNode2 = DataCenter.Save().GetCurrentWorldLevelNode();
			EnterWorldMapImmediately(SolidMapCameraControl.mInstance.levelPointsInfo[currentWorldLevelNode2.index].stagePartStageCameraTrans.localPosition);
		}
		else
		{
			DataConf.GameLevelData gameLevelData = DataCenter.Conf().GetCurrentGameLevelList(DataCenter.State().selectLevelMode)[DataCenter.State().selectAreaNode];
			Vector3 localPosition = SolidMapCameraControl.mInstance.levelPointsInfo[DataCenter.State().selectWorldNode].wavePartStageCameraTrans.localPosition;
			EnterAreaMapImmediately(localPosition);
		}
		DataCenter.State().lastSceneType = Defined.SceneType.Menu;
		Invoke("RepeatUpdateChatInfo", 0.1f);
	}

	private void BlindFunction()
	{
		UIPROPERTYINFO.SetBackBtnClickDelegate(HandleBackBtnClickEvent);
		UIPROPERTYINFO.SetIAPBtnClickDelegate(HandleOpenShopBtnClickEvent);
		UIBASEMAPINFO.SetCustomsDifficultyTogglesStateChangedEvent(HandleCustomsDifficultyToggleStateChangedEvent);
		UIBASEMAPINFO.SetCustomsDifficultyDisableBtnClickEvent(HandleCustomsDifficultDisableBtnClickEvent);
		UIBASECONFIRMBATTLEINFO.SetCancelBtnClickDelegate(HandleBackBtnClickEvent);
		UIBASECONFIRMBATTLEINFO.SetBattleBtnClickDelegate(HandleStartGameBattleEvent);
	}

	private void Init()
	{
		UITEAMINFO.UpdateName(DataCenter.Save().userName);
		UITEAMINFO.UpdateLevel(string.Empty + DataCenter.Save().GetTeamData().teamLevel);
		UITEAMINFO.UpdateCombat(string.Empty + DataCenter.Save().GetTeamData().GetTeamCombat());
		if (DataCenter.Save().GetTeamData().teamLevel < DataCenter.Save().GetTeamData().teamMaxLevel)
		{
			UITEAMINFO.UpdateExpPercent(DataCenter.Save().GetTeamData().teamExp, DataCenter.Save().GetTeamData().teamMaxExp);
			UITEAMINFO.UpdateDetailInfo(DataCenter.Save().GetTeamData().teamLevel, DataCenter.Save().GetTeamData().teamExp, DataCenter.Save().GetTeamData().teamMaxExp);
		}
		else
		{
			UITEAMINFO.UpdateExpPercent(0f, 0f);
			UITEAMINFO.UpdateDetailInfo("Team Level:" + DataCenter.Save().GetTeamData().teamMaxLevel + "\nYour team has reached maximum\n level!");
		}
		UpdatePropertyInfoPart("Map", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}

	public void CheckBaseNewThingsUnlocked(bool bRemove, bool bShowTips)
	{
		string text = string.Empty;
		BaseBtnNewTipsGO.SetActive(false);
		if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Hero))
		{
			text += " new hero,";
			BaseBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Hero);
			}
		}
		if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Equipment))
		{
			text += " new equipment,";
			BaseBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Equipment);
			}
		}
		if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Genius))
		{
			text += " new genius,";
			BaseBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Genius);
			}
		}
		if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Evolution))
		{
			text += " new evolution, ";
			BaseBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Evolution);
			}
		}
		if (bShowTips && text != string.Empty)
		{
			text = text.Remove(text.Length - 1);
			string str = "Unlocked" + text + ".\nDo you want to configure in the BASE?";
			UIDialogManager.Instance.ShowPopupA(str, UIWidget.Pivot.Center, false, HandleTeamBtn);
		}
	}

	public void CheckNewMsg()
	{
		MailBtnNewTipsGO.SetActive(UIConstant.gProbeUnreadMsgsCount > 0);
		AchievementBtnNewTipsGO.SetActive(UIConstant.gProbeNewAchievementCount > 0);
	}

	public void UpdatePropertyInfoPart(string name, int rank, int gold, int crystal)
	{
		if (name != "null#")
		{
			UIPROPERTYINFO.UpdateName(name);
		}
		if (rank > 0)
		{
			UIPROPERTYINFO.UpdateRank(rank.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFO.UpdateRank(0 + string.Empty);
		}
		if (gold > 0)
		{
			UIPROPERTYINFO.UpdateGold(gold.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFO.UpdateGold(0 + string.Empty);
		}
		if (crystal > 0)
		{
			UIPROPERTYINFO.UpdateCrystal(crystal.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFO.UpdateCrystal(0 + string.Empty);
		}
	}

	private void CheckNewUnlockFromBattle(int iNewWorldLevelUnlocked, int iNewAreaLevelUnlocked, int iNewLevelModeUnlocked)
	{
		int worldNode = DataCenter.State().selectWorldNode;
		DataConf.GameLevelNodeData[] nodeList = DataCenter.Conf().GetGameLevelNodeList();
		if (nodeList == null || worldNode < 0 || worldNode >= nodeList.Length)
		{
			return;
		}
		string gameLevelID = nodeList[worldNode].gameLevelID;
		DataCenter.Conf().LoadSelectGameLevelDataFromDisk(gameLevelID);
		if (SolidMapCameraControl.mInstance == null || SolidMapCameraControl.mInstance.levelPointsInfo == null
			|| worldNode >= SolidMapCameraControl.mInstance.levelPointsInfo.Count)
		{
			return;
		}
		Vector3 localPosition = SolidMapCameraControl.mInstance.levelPointsInfo[worldNode].wavePartStageCameraTrans.localPosition;
		EnterAreaMapImmediately(localPosition);
	}

	public void EnterWorldMapImmediately(Vector3 pos)
	{
		SolidMapCameraControl.mInstance.TweenCameraLocalPos(pos, 0f, null, string.Empty);
		InitWorldMap();
	}

	private void EnterWorldMap()
	{
		DataConf.GameLevelNodeData currentWorldLevelNode = DataCenter.Save().GetCurrentWorldLevelNode();
		Vector3 localPosition = SolidMapCameraControl.mInstance.levelPointsInfo[currentWorldLevelNode.index].stagePartStageCameraTrans.localPosition;
		EnterWorldMap(localPosition);
	}

	private void EnterWorldMap(Vector3 pos)
	{
		mapBlock.SetActive(true);
		SolidMapCameraControl.mInstance.TweenCameraLocalPos(pos, 0.6f, base.gameObject, "InitWorldMap");
	}

	private void InitWorldMap()
	{
		mapBlock.SetActive(false);
		UITEAMINFO.SetDetailVisable(false);
		if (coureseType == 1)
		{
			SolidMapCameraControl.mInstance.SetCameraTrans(SolidMapCameraControl.mInstance.globalTutorialCameraTrans);
		}
		UIBASEMAPINFO.ShowWorldMap();
		UpdateStagePoinInfo();
		UIUtil.bStageMapChangedUI = true;
		nowPageType = PageType.E_WorldMap;
		UpdatePropertyInfoPart("Map", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		UIPROPERTYINFO.SetBackBtnVisable(false);
		if (DataCenter.Save().bNewUser)
		{
			UITEAMINFO.UpdateName("newbie");
			UIBASECREATEPLAYERINFO.SetVisable(true);
		}
		if (coureseType == 1)
		{
			UIBASECOURSEINFO.GetCourse(1).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
		if (coureseType == 0)
		{
			UIBASECOURSEINFO.GetCourse(11).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	private void UpdateStagePoinInfo()
	{
		for (int i = 0; i < SolidMapCameraControl.mInstance.levelPointsInfo.Count; i++)
		{
			DataConf.GameLevelNodeData gameLevelNodeData = DataCenter.Conf().GetGameLevelNodeList()[i];
			SolidMapCameraControl.LevelPointsInfo levelPointsInfo = SolidMapCameraControl.mInstance.levelPointsInfo[i];
			bool bUnlock = DataCenter.Save().isWorldLevelUnlocked(gameLevelNodeData.index);
			bool flag = false;
			flag = DataCenter.Save().isCurrentWorldLevel(gameLevelNodeData.index);
			if (gameLevelNodeData.index == 0 && !DataCenter.Save().isWorldLevelUnlocked(1))
			{
				flag = true;
			}
			Defined.LevelMode currentUnlockedLevelMode = (Defined.LevelMode)DataCenter.Save().GetCurrentUnlockedLevelMode(gameLevelNodeData.index);
			bool bClear = currentUnlockedLevelMode >= Defined.LevelMode.Hard;
			GameObject gameObject = UIBASEMAPINFO.CreateStagePoint(gameLevelNodeData.index, bUnlock, levelPointsInfo.stageTrans, new Vector3(gameLevelNodeData.mapNodePos.x, gameLevelNodeData.mapNodePos.y, 0f), HandleStagePointClickEvent, bClear, flag, gameLevelNodeData.iconName, gameLevelNodeData.name);
			if (gameLevelNodeData.index == 0 && coureseType == 1)
			{
				UIBASECOURSEINFO.AddCursor(1, gameObject, gameObject.transform.Find("CoureseTrans").position);
				UIBASECOURSEINFO.GetCourse(1).SetMoveToUnderTargetTrans(gameObject.transform.Find("CoureseTrans"));
			}
		}
		UpdateWorldMapEffect();
	}

	private void UpdateWorldMapEffect()
	{
		for (int i = 0; i < SolidMapCameraControl.mInstance.levelPointsInfo.Count; i++)
		{
			DataConf.GameLevelNodeData gameLevelNodeData = DataCenter.Conf().GetGameLevelNodeList()[i];
			int index = gameLevelNodeData.index;
			bool bUnlock = DataCenter.Save().isWorldLevelUnlocked(index);
			bool flag = false;
			flag = DataCenter.Save().isCurrentWorldLevel(index);
			if (index == 0 && !DataCenter.Save().isWorldLevelUnlocked(1))
			{
				flag = true;
			}
			Defined.LevelMode currentUnlockedLevelMode = (Defined.LevelMode)DataCenter.Save().GetCurrentUnlockedLevelMode(index);
			bool bClear = currentUnlockedLevelMode >= Defined.LevelMode.Hard;
			UtilUIBase3DMapStateControlManager.mInstance.SetState(index, flag, bUnlock, bClear);
		}
	}

	public void EnterAreaMapImmediately(Vector3 pos)
	{
		SolidMapCameraControl.mInstance.TweenCameraLocalPos(pos, 0f, null, string.Empty);
		InitAreaMap();
	}

	private void EnterAreaMap()
	{
		Vector3 localPosition = SolidMapCameraControl.mInstance.levelPointsInfo[DataCenter.State().selectWorldNode].wavePartStageCameraTrans.localPosition;
		EnterAreaMap(localPosition);
	}

	private void EnterAreaMap(Vector3 pos)
	{
		mapBlock.SetActive(true);
		SolidMapCameraControl.mInstance.TweenCameraLocalPos(pos, 0.6f, base.gameObject, "InitAreaMap");
	}

	private void InitAreaMap()
	{
		InitAreaMap(DataCenter.State().selectLevelMode, DataCenter.State().selectWorldNode);
	}

	private void InitAreaMap(Defined.LevelMode lm, int worldIndex)
	{
		mapBlock.SetActive(false);
		UITEAMINFO.SetDetailVisable(false);
		DataConf.GameLevelNodeData gameLevelNodeData = DataCenter.Conf().GetGameLevelNodeList()[worldIndex];
		UIBASEMAPINFO.ShowAreaMap(worldIndex);
		UpdateWorldMapEffect();
		UIBASEMAPINFO.SetCustomDifficultyTUnlockState((Defined.LevelMode)DataCenter.Save().GetCurrentUnlockedLevelMode(worldIndex));
		UIBASEMAPINFO.SetCustomsDifficultyChecked((int)lm, true);
		HandleCustomsDifficultyToggleStateChangedEvent((int)lm, true);
		UIUtil.bStageMapChangedUI = false;
		nowPageType = PageType.E_AreaMap;
		UpdatePropertyInfoPart(gameLevelNodeData.name, DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		UIPROPERTYINFO.SetBackBtnVisable(true);
		if (coureseType == 0)
		{
			UIBASECOURSEINFO.GetCourse(11).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	private void UpdateWavePoinInfo(int worldIndex, Defined.LevelMode levelMode)
	{
		List<Transform> list = SolidMapCameraControl.mInstance.levelPointsInfo[worldIndex].waveTrans[(int)levelMode];
		for (int i = 0; i < list.Count; i++)
		{
			DataConf.GameLevelData gameLevelData = DataCenter.Conf().GetCurrentGameLevelList(levelMode)[i];
			bool flag = DataCenter.Save().isGameLevelUnlocked(worldIndex, gameLevelData.index, levelMode);
			bool flag2 = false;
			int num = 0;
			num = ((!flag) ? (-1) : DataCenter.Save().GetLevelStars(DataCenter.State().selectWorldNode, levelMode, gameLevelData.index));
			flag2 = DataCenter.Save().GetNewAreaLevelUnlocked - 1 == gameLevelData.index;
			if (flag && !DataCenter.Save().isGameLevelUnlocked(DataCenter.State().selectWorldNode, gameLevelData.index + 1, levelMode))
			{
				flag2 = true;
			}
			Vector3 vector = SolidMapCameraControl.mInstance.WorldToScreenViewPort(list[i].position);
			Vector3 vector2 = new Vector3((float)Screen.width * vector.x, (float)Screen.height * vector.y, 0f);
			Vector3 position = new Vector3(vector2.x - (float)Screen.width / 2f, vector2.y - (float)Screen.height / 2f, 0f);
			GameObject gameObject = UIBASEMAPINFO.CreateWavePoint(gameLevelData.index, gameLevelData.isBossLevel, num, list[i], position, HandleWavePointClickEvent, flag2, flag2, gameLevelData.depth, gameLevelData.name);
			if (gameLevelData.index == 0 && coureseType == 1)
			{
				UIBASECOURSEINFO.AddCursor(2, gameObject, gameObject.transform.Find("CoureseTrans").position);
			}
		}
	}

	private void RepeatUpdateChatInfo()
	{
		DataCenter.State().selectArenaRankTypeByLanguage = UIUtil.GetProtocolLaguageCode();
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Chat_GetMsgList, OnGetChatMsgInfoFinished);
	}

	public void HandleOpenShopBtnClickEvent()
	{
		UIDialogManager.Instance.ShowShopDialogUI(HandleBuyIAPFinishedEvent);
	}

	public void HandleBackBtnClickEvent()
	{
		if (nowPageType != 0)
		{
			if (nowPageType == PageType.E_AreaMap)
			{
				EnterWorldMap();
				UIBASEMAPINFO.ClearWavePoints();
			}
			else if (nowPageType == PageType.E_ConfirmWave)
			{
				UIBASECONFIRMBATTLEINFO.SetVisable(false);
				nowPageType = PageType.E_AreaMap;
			}
		}
	}

	public void HandleStagePointClickEvent(UtilUIBaseStagePointInfoData data)
	{
		if (data.BUNLOCK)
		{
			DataConf.GameLevelNodeData gameLevelNodeData = DataCenter.Conf().GetGameLevelNodeList()[data.ID];
			string gameLevelID = gameLevelNodeData.gameLevelID;
			DataCenter.Conf().LoadSelectGameLevelDataFromDisk(gameLevelID);
			DataCenter.State().selectWorldNode = data.ID;
			DataCenter.State().selectLevelMode = (Defined.LevelMode)DataCenter.Save().GetCurrentUnlockedLevelMode(DataCenter.State().selectWorldNode);
			EnterAreaMap();
		}
		else
		{
			string text = DataCenter.Conf().GetGameLevelNodeList()[data.ID - 1].name;
			UIDialogManager.Instance.ShowDriftMsgInfoUI("Complete " + text + " to unlock.");
		}
		if (UIBASECOURSEINFO.TutorialInProgress && data.ID == 0 && UIBASECOURSEINFO.GetCourse(1).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(1).STATE = UtilUICourseInfo.CoursePhaseState.Done;
		}
	}

	public void HandleWavePointClickEvent(UtilUIBaseWavePointInfoData data)
	{
		if (data.STARS >= 0)
		{
			UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 5);
			UIDialogManager.Instance.ShowBlock(5);
			DataCenter.State().selectAreaNode = data.ID;
			LevelRewardController.Get();
			HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_LevelReward, OnGetLevelRewardDataFinished);
			DataConf.GameLevelData gameLevelData = DataCenter.Conf().GetCurrentGameLevelList(DataCenter.State().selectLevelMode)[data.ID];
			DataCenter.Conf().SetCurrentGameLevel(DataCenter.State().selectLevelMode, gameLevelData.index);
			DataCenter.Conf().GetCurrentGameLevelData().level = GameProgressController.GetRewardIndex();
		}
		else
		{
			UIDialogManager.Instance.ShowDriftMsgInfoUI("Complete last mission to unlock.");
		}
		if (UIBASECOURSEINFO.TutorialInProgress && data.ID == 0 && UIBASECOURSEINFO.GetCourse(2).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(2).STATE = UtilUICourseInfo.CoursePhaseState.Done;
		}
	}

	public void HandleCustomsDifficultyToggleStateChangedEvent(int index, bool bChecked)
	{
		if (bChecked)
		{
			UIBASEMAPINFO.ClearWavePoints();
			DataCenter.State().selectLevelMode = (Defined.LevelMode)index;
			UIBASEMAPINFO.SetMapUIBackgroundByCustomsDifficulty(DataCenter.State().selectLevelMode);
			UpdateWavePoinInfo(DataCenter.State().selectWorldNode, DataCenter.State().selectLevelMode);
			if (coureseType == 1)
			{
				UIBASECOURSEINFO.GetCourse(2).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
			}
		}
	}

	public void HandleCustomsDifficultDisableBtnClickEvent(Defined.LevelMode levelMode)
	{
		switch (levelMode)
		{
		case Defined.LevelMode.Hell:
			UIDialogManager.Instance.ShowDriftMsgInfoUI("Hell is locked.");
			break;
		case Defined.LevelMode.Hard:
			UIDialogManager.Instance.ShowDriftMsgInfoUI("Nightmare is locked.");
			break;
		default:
			UIDialogManager.Instance.ShowDriftMsgInfoUI("Normal is locked.");
			break;
		}
	}

	public void HandleStartGameBattleEvent()
	{
		DataConf.GameLevelData gameLevelData = DataCenter.Conf().GetCurrentGameLevelList(DataCenter.State().selectLevelMode)[DataCenter.State().selectAreaNode];
		if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(10).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(10).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			DataCenter.Save().tutorialStep = Defined.TutorialStep.EnterBattle;
		}
		if (gameLevelData.index == 0 && DataCenter.State().selectWorldNode == 0 && DataCenter.Save().tutorialStep != Defined.TutorialStep.Finish)
		{
			DataCenter.Save().tutorialStep = Defined.TutorialStep.FinishStageOneWaveOne;
		}
		UIBASECONFIRMBATTLEINFO.SetBattleBtnEnable(false);
		DataCenter.Save().SaveGameData();
		SceneLoadingManager.s_lastSceneName = "UIBase";
		SceneLoadingManager.s_nextSceneName = DataCenter.Conf().GetCurrentGameLevelData().id;
		SceneManager.Instance.SwitchScene("Loading");
		UIUtil.HideOpenClik();
	}

	public void OnGetPlayerDataFinished_Init(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 9);
		UIDialogManager.Instance.HideBlock(9);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(11).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(11).STATE = UtilUICourseInfo.CoursePhaseState.Done;
		}
		HeroListController.Refresh();
		SceneLoadingManager.s_lastSceneName = "UIBase";
		SceneLoadingManager.s_nextSceneName = "UITeam";
		SceneManager.Instance.SwitchScene("Loading");
		UIUtil.HideOpenClik();
	}

	public void HandleTeamBtn()
	{
		CheckBaseNewThingsUnlocked(true, false);
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 9);
		UIDialogManager.Instance.ShowBlock(9);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_PlayerData, OnGetPlayerDataFinished_Init);
	}

	public void HandleFormationBtn()
	{
		SceneManager.Instance.SwitchScene("UIFormation");
		UIUtil.HideOpenClik();
	}

	public void HandleShopBtn()
	{
		SceneManager.Instance.SwitchScene("UIShop");
		UIUtil.HideOpenClik();
	}

	public void HandleBagBtn()
	{
		SceneManager.Instance.SwitchScene("UIBag");
		UIUtil.HideOpenClik();
	}

	public void HandleNetBtn()
	{
		SceneManager.Instance.SwitchScene("UIArena");
		UIUtil.HideOpenClik();
	}

	public void HandleChatBtn()
	{
		UIBASECHATINFO.SetVisable(!UIBASECHATINFO.gameObject.activeSelf);
	}

	public void HandleOpenSettingBtn()
	{
		UIBASEOPTITIONINFO.Init();
	}

	public void HandleOpenMailBtn()
	{
		SceneManager.Instance.SwitchScene("UIMail");
		UIUtil.HideOpenClik();
	}

	public void HandleOpenAchievementBtn()
	{
		SceneManager.Instance.SwitchScene("UIAchievements");
		UIUtil.HideOpenClik();
	}

	public void HandleBuyIAPFinishedEvent(int code)
	{
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}

	public void OnGetLevelRewardDataFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 5);
		UIDialogManager.Instance.HideBlock(5);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		DataConf.GameLevelData gameLevelData = DataCenter.Conf().GetCurrentGameLevelList(DataCenter.State().selectLevelMode)[DataCenter.State().selectAreaNode];
		UIBASECONFIRMBATTLEINFO.UpdateTitel(gameLevelData.name);
		UIBASECONFIRMBATTLEINFO.UpdateScenceIconTexture(UIUtil.GetEquipTextureMaterial(DataCenter.Conf().GetGameLevelNodeList()[DataCenter.State().selectWorldNode].texIcon).mainTexture);
		UIBASECONFIRMBATTLEINFO.UpdateDifficultWarningColorAndScenceCombat(DataCenter.Save().selectLevelDropData.recommendCombat, DataCenter.Save().GetTeamData().GetTeamCombat());
		UIBASECONFIRMBATTLEINFO.UpdateTeamCombat(string.Empty + DataCenter.Save().GetTeamData().GetTeamCombat());
		UIBASECONFIRMBATTLEINFO.UpdateShowPartTitle(gameLevelData.title);
		UIBASECONFIRMBATTLEINFO.UpdateShowPartConent(gameLevelData.description);
		List<string> list = new List<string>();
		TeamSiteData[] teamSitesData = DataCenter.Save().GetTeamData().teamSitesData;
		foreach (TeamSiteData teamSiteData in teamSitesData)
		{
			if (teamSiteData.playerData != null)
			{
				DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(teamSiteData.playerData.heroIndex);
				list.Add(heroDataByIndex.iconFileName);
			}
			else
			{
				list.Add(string.Empty);
			}
		}
		UIBASECONFIRMBATTLEINFO.UpdateTeamPlayerInfo(list);
		UIBASECONFIRMBATTLEINFO.UpdateEXPReward(DataCenter.Save().selectLevelDropData.exp + string.Empty);
		UIBASECONFIRMBATTLEINFO.UpdateGoldReward(DataCenter.Save().selectLevelDropData.money + string.Empty);
		if (DataCenter.Save().selectLevelDropData.stuffs.Count > 0)
		{
			UIBASECONFIRMBATTLEINFO.UpdateItemReward(DataCenter.Save().selectLevelDropData.stuffs[0].count + string.Empty);
			UIBASECONFIRMBATTLEINFO.UpdateItemRewardIcon(DataCenter.Conf().GetStuffDataByIndex(DataCenter.Save().selectLevelDropData.stuffs[0].index).fileName);
		}
		else
		{
			UIBASECONFIRMBATTLEINFO.UpdateItemReward("0");
		}
		UIBASECONFIRMBATTLEINFO.SetVisable(true);
		nowPageType = PageType.E_ConfirmWave;
		if (coureseType == 1)
		{
			UIBASECOURSEINFO.GetCourse(10).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	public void OnGetChatMsgInfoFinished(int code)
	{
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		UIBASECHATINFO.UpdateChatListInfo(UIConstant.gLsChatData);
		if (UIBASECHATINFO.bChatRoomIsUsing && UIConstant.gLsChatData.Count > 0)
		{
			UIConstant.gLastChatMsgTime = UIConstant.gLsChatData[UIConstant.gLsChatData.Count - 1].dateSeconds;
		}
		if (UIConstant.gLsChatData.Count > 0 && UIConstant.gLsChatData[UIConstant.gLsChatData.Count - 1].dateSeconds > UIConstant.gLastChatMsgTime)
		{
			UIBASECHATINFO.ChatBtnNewTipsGO.SetActive(true);
		}
		else
		{
			UIBASECHATINFO.ChatBtnNewTipsGO.SetActive(false);
		}
		Invoke("RepeatUpdateChatInfo", 15f);
	}

	public void OnGetHeartBeatFinished(int code)
	{
		CheckNewMsg();
	}

	public void OnGetPlayerDataFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 47);
		UIDialogManager.Instance.HideBlock(47);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
		}
		else
		{
			Init();
		}
	}
}

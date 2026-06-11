using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class UINewTeamManager : MonoBehaviour
{
	public enum PageType
	{
		E_TeamInfoPage = 0,
		E_PlayerDetailInfoPage = 1,
		E_BonusPage = 2,
		E_ExhibitionPage = 3
	}

	[SerializeField]
	private GameObject mBackgroundGO;

	[SerializeField]
	private GameObject mTeamInfoContentGO;

	[SerializeField]
	private GameObject mTeamBonusContentGO;

	[SerializeField]
	private GameObject mExhibitionContentGO;

	[SerializeField]
	private UtilUIPropertyInfo m_scriptUIPropertyInfo;

	[SerializeField]
	private UtilUITeamPlayersInfo m_scriptUITeamPlayersInfo;

	[SerializeField]
	private UtilUIStandbyPlayersInfo m_scriptUIStandbyPlayersInfo;

	[SerializeField]
	private UtilUITeamPlayerDetailInfo m_scriptUITeamPlayerDetailInfo;

	[SerializeField]
	private UtilUITeamBonusEvolve m_scriptUITeamBonusEvolve;

	[SerializeField]
	private UtilUITeamBonusTalent m_scriptUITeamBonusTalent;

	[SerializeField]
	private UITeamTeamInfoTweenPosReset m_scriptUITweenPos;

	[SerializeField]
	private UtilUITeamExhibitionInfo m_scriptUIExhibition;

	[SerializeField]
	private GameObject cursorGO;

	[SerializeField]
	private UISprite cursorIcon;

	[SerializeField]
	private UtilUICourseInfo m_scriptUIBaseCourseInfo;

	[SerializeField]
	private GameObject TeamBuffBtnNewTipsGO;

	[SerializeField]
	private GameObject EquipmentBtnNewTipsGO;

	[SerializeField]
	private Camera gExhibitionCamera;

	[SerializeField]
	private UITeamTeamInfoTweenPosReset resetTeamInfoTweenScript;

	[SerializeField]
	private UtilUITeamCursorInfo cursorInfo;

	[SerializeField]
	private GameObject EquipmentBtnUBNewTipsGO;

	protected int lastSelectTeamSitenum;

	protected List<int> lsShowingArrows = new List<int>();

	public PageType nowPageType;

	public GameObject teamSiteBtnGO;

	public GameObject equipmentBtnGO;

	public GameObject weaponUpgradeBtnGO;

	public GameObject skillUpgradeBtnGO;

	public GameObject helmsToggleGO;

	public GameObject helmsEquipmentUpgradeGO;

	public GameObject armorsToggleGO;

	public GameObject armorsEquipmentUpgradeGO;

	public GameObject ornamentsToggleGO;

	public GameObject ornamentsEquipmentUpgradeGO;

	public GameObject backGO;

	public int coureseType = -1;

	private float pressTime = -1f;

	private UtilUIStandbyPlayersInfo.PLAYERINFO gPlayerInfo;

	private bool bStartDragAvibalCharacter;

	private Vector2 currentStartPos = Vector2.zero;

	public UtilUIPropertyInfo UIPROPERTYINFOSCRIPT
	{
		get
		{
			return m_scriptUIPropertyInfo;
		}
	}

	public UtilUITeamPlayersInfo UITEAMPLAYERSINFOSCRIPT
	{
		get
		{
			return m_scriptUITeamPlayersInfo;
		}
	}

	public UtilUIStandbyPlayersInfo UISTANDBYPLAYERSINFOSCRIPT
	{
		get
		{
			return m_scriptUIStandbyPlayersInfo;
		}
	}

	public UtilUITeamPlayerDetailInfo UITEAMPLAYERDETAILINFOSCRIPT
	{
		get
		{
			return m_scriptUITeamPlayerDetailInfo;
		}
	}

	public UtilUITeamBonusEvolve UITEAMBONUSEVOLVESCRIPT
	{
		get
		{
			return m_scriptUITeamBonusEvolve;
		}
	}

	public UtilUITeamBonusTalent UITEAMBONUSTALENTSCRIPT
	{
		get
		{
			return m_scriptUITeamBonusTalent;
		}
	}

	public UtilUITeamExhibitionInfo UITEAMEXHIBITIONSCRIPT
	{
		get
		{
			return m_scriptUIExhibition;
		}
	}

	public UtilUICourseInfo UIBASECOURSEINFO
	{
		get
		{
			return m_scriptUIBaseCourseInfo;
		}
	}

	public void Start()
	{
		TAudioManager.instance.AudioListener.transform.position = new Vector3(0f, 10000f, 0f);
		if (DataCenter.Save().tutorialStep == Defined.TutorialStep.CreateNameFinish)
		{
			if (DataCenter.Save().GetTeamSiteData(Defined.TEAM_SITE.TEAMMATE_1).state != Defined.ItemState.Purchase)
			{
				coureseType = -1;
			}
			else
			{
				coureseType = 0;
			}
		}
		else if (DataCenter.Save().tutorialStep == Defined.TutorialStep.UnlockedTeamSite)
		{
			coureseType = 1;
		}
		else if (DataCenter.Save().tutorialStep == Defined.TutorialStep.EnterBattle || DataCenter.Save().tutorialStep == Defined.TutorialStep.FinishStageOneWaveOne)
		{
			coureseType = 2;
		}
		UIDialogManager.Instance.SetPropertyScript(UIPROPERTYINFOSCRIPT);
		CheckNewThingsUnlocked(false);
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 1);
		UIDialogManager.Instance.ShowBlock(1);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_GetListGeniusInfos, OnGetListGeniusInfoFinished);
		BlindPropertyInfoPartFunction(HandlePropertyInfoPartBackBtnClick, HandlePropertyInfoPartIAPBtnClick);
		UpdatePropertyInfoPart("Base", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		BlindTeamInfoPartFunction(HandleTeamInfoPartTeamBounsBtnClick, HandleTeamInfoPartToggleStateChange, HandleTeamInfoPartPlayerUnlcokStateBtnClick);
		BlindStandbyPlayerInfoPartFunction(HandletandbyPlayerItemClicked, HandletandbyPlayerItemUnlockClicked, HandleStandbyPlayerFormationControlPressEvent, HandleStandbyPlayerFormationControlDragStartEvent, HandleStandbyPlayerFormationControlDragMovedEvent, HandleStandPlayerFormationControlDragReleasedEvent);
		InitStandbyPlayerInfo();
		InitTeamPlayerDetailInfo();
		if (coureseType == 0)
		{
			UIBASECOURSEINFO.AddCursor(1050, teamSiteBtnGO, teamSiteBtnGO.transform.position);
			UIBASECOURSEINFO.GetCourse(1050).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
		else if (coureseType == 1)
		{
			UtilUIStandbyPlayersInfo.PLAYERINFO playerInfoById = UISTANDBYPLAYERSINFOSCRIPT.GetPlayerInfoById(UIConstant.gCouseHeroId);
			Object.Destroy(playerInfoById.formationControl.gameObject.GetComponent<UIDragScrollView>());
			UIBASECOURSEINFO.AddCursor(1051, playerInfoById.go, playerInfoById.go.transform.position);
			UIBASECOURSEINFO.GetCourse(1051).SetExplainGO(playerInfoById.go.transform.Find("ExplainGOPress").gameObject);
			UIBASECOURSEINFO.GetCourse(1051).SetMoveToUnderTargetTrans(playerInfoById.go.transform.Find("CoureseTrans"));
			UIBASECOURSEINFO.GetCourse(1051).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
		else if (coureseType == 2)
		{
			UIBASECOURSEINFO.AddCursor(1054, equipmentBtnGO, equipmentBtnGO.transform.position);
			UIBASECOURSEINFO.GetCourse(1054).SetMoveToUnderTargetTrans(equipmentBtnGO.transform.Find("CoureseTrans"));
			UIBASECOURSEINFO.GetCourse(1054).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	public void CheckNewThingsUnlocked(bool bRemove)
	{
		TeamBuffBtnNewTipsGO.SetActive(false);
		EquipmentBtnNewTipsGO.SetActive(false);
		if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Hero))
		{
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Hero);
			}
		}
		else if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Equipment))
		{
			EquipmentBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Equipment);
			}
		}
		else if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Genius))
		{
			TeamBuffBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Genius);
			}
		}
		else if (UIConstant.gLsNewUnlockedInfo.Contains(NewUnlockData.E_NewUnlockType.E_Evolution))
		{
			TeamBuffBtnNewTipsGO.SetActive(true);
			if (bRemove)
			{
				UIConstant.gLsNewUnlockedInfo.Remove(NewUnlockData.E_NewUnlockType.E_Evolution);
			}
		}
	}

	public void RequestLoadHeroEquipment(int heroIndex)
	{
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 48);
		UIDialogManager.Instance.ShowBlock(48);
		DataCenter.State().selectHeroIndex = heroIndex;
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_LoadEquipmentInfo, OnLoadHeroEquipmentInfoFinished);
	}

	public void OnGetListGeniusInfoFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 1);
		UIDialogManager.Instance.HideBlock(1);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		InitTeamBounsInfo();
		InitTeamInfoPlayersInfo();
	}

	public void OnTeamBuyTeamSiteFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 2);
		UIDialogManager.Instance.HideBlock(2);
		if (code != 0)
		{
			string strInput = "unlock site " + DataCenter.State().selectTeamSiteIndex + "?";
			int num = Mathf.CeilToInt((float)(UIConstant.TradeMoneyNotEnough + UIConstant.TradeHornorNotEnough) / (float)UIConstant.MoneyExchangRate);
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code, strInput, RequestBuyTeamSiteUseCrystal, Defined.COST_TYPE.Money, UIConstant.TradeMoneyNotEnough, Defined.COST_TYPE.Honor, UIConstant.TradeHornorNotEnough, " " + num);
			return;
		}
		UpdateTeamUI();
		UpdatePropertyInfoPart("Base", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1050).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(1050).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			DataCenter.Save().tutorialStep = Defined.TutorialStep.UnlockedTeamSite;
			UtilUIStandbyPlayersInfo.PLAYERINFO playerInfoById = UISTANDBYPLAYERSINFOSCRIPT.GetPlayerInfoById(UIConstant.gCouseHeroId);
			Object.Destroy(playerInfoById.formationControl.gameObject.GetComponent<UIDragScrollView>());
			UIBASECOURSEINFO.AddCursor(1051, playerInfoById.go, playerInfoById.go.transform.position);
			UIBASECOURSEINFO.GetCourse(1051).SetExplainGO(playerInfoById.go.transform.Find("ExplainGOPress").gameObject);
			UIBASECOURSEINFO.GetCourse(1051).SetMoveToUnderTargetTrans(playerInfoById.go.transform.Find("CoureseTrans"));
			UIBASECOURSEINFO.GetCourse(1051).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	public void OnTeamBuyHeroFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 3);
		UIDialogManager.Instance.HideBlock(3);
		UtilUIStandbyPlayersInfo.PLAYERINFO playerInfoById = UISTANDBYPLAYERSINFOSCRIPT.GetPlayerInfoById(DataCenter.State().selectHeroIndex);
		if (code != 0)
		{
			string strInput = "unlock " + playerInfoById.data.HEROCONF.name + "?";
			int num = Mathf.CeilToInt((float)(UIConstant.TradeMoneyNotEnough + UIConstant.TradeHornorNotEnough) / (float)UIConstant.MoneyExchangRate);
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code, strInput, RequestUnlockHeroUseCrystal, Defined.COST_TYPE.Money, UIConstant.TradeMoneyNotEnough, Defined.COST_TYPE.Honor, UIConstant.TradeHornorNotEnough, " " + num);
		}
		else
		{
			UISTANDBYPLAYERSINFOSCRIPT.UpdateBaseUI(DataCenter.State().selectHeroIndex, false);
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.UpdatePageStateBtn(playerInfoById.data.PLAYERDATA.state == Defined.ItemState.Available);
			UpdatePropertyInfoPart("Base", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		}
	}

	public void OnTeamEquipHeroFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 4);
		UIDialogManager.Instance.HideBlock(4);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		DataCenter.Save().SetHeroOnTeamSite(DataCenter.Save().GetPlayerData(DataCenter.State().selectHeroIndex), (Defined.TEAM_SITE)DataCenter.State().selectTeamSiteIndex);
		UISTANDBYPLAYERSINFOSCRIPT.UpdateAllStandbyPlayersUI();
		UISTANDBYPLAYERSINFOSCRIPT.SelectPlayerInTheCenterOfTheView(DataCenter.State().selectHeroIndex);
		SetTeamSiteArrow(DataCenter.State().selectTeamSiteIndex, false);
		lastSelectTeamSitenum = DataCenter.State().selectTeamSiteIndex;
		UpdateTeamUI();
		UITEAMPLAYERSINFOSCRIPT.SetFormationSiteAccessReset();
		UITEAMPLAYERSINFOSCRIPT.PlayEquipedHeroSound(DataCenter.State().selectTeamSiteIndex);
		if (UIBASECOURSEINFO.TutorialInProgress && (UIBASECOURSEINFO.GetCourse(1052).STATE == UtilUICourseInfo.CoursePhaseState.InProgress || UIBASECOURSEINFO.GetCourse(1053).STATE == UtilUICourseInfo.CoursePhaseState.InProgress))
		{
			UIBASECOURSEINFO.GetCourse(1052).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			UIBASECOURSEINFO.GetCourse(1053).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			DataCenter.Save().tutorialStep = Defined.TutorialStep.SetTeam;
			UIBASECOURSEINFO.AddCursor(1064, backGO, backGO.transform.position);
			UIBASECOURSEINFO.GetCourse(1064).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	public void OnLoadHeroEquipmentInfoFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 48);
		UIDialogManager.Instance.HideBlock(48);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
		}
		else if (UITEAMPLAYERDETAILINFOSCRIPT.playdata.heroIndex == DataCenter.State().selectHeroIndex)
		{
			PlayerData playerData = DataCenter.Save().GetPlayerData(DataCenter.State().selectHeroIndex);
			UpdateAllEquipsmentCanUpgradeTips(playerData);
		}
	}

	public void OnTeamLoadEquipmentInfoFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 6);
		UIDialogManager.Instance.HideBlock(6);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
		}
		else
		{
			ShowTeamPlayerDetailInfo();
		}
	}

	protected void HandlePropertyInfoPartBackBtnClick()
	{
		if (nowPageType == PageType.E_TeamInfoPage)
		{
			UtilIAPVerify.Instance().SetPurchaseIAP_SelfFinishedSuccessedEventDelegate(null);
			SceneManager.Instance.SwitchScene("UIBase");
			if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1064).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
			{
				UIBASECOURSEINFO.GetCourse(1064).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			}
		}
		else if (nowPageType == PageType.E_BonusPage)
		{
			nowPageType = PageType.E_TeamInfoPage;
			mTeamInfoContentGO.SetActive(true);
			mTeamBonusContentGO.SetActive(false);
		}
		else if (nowPageType == PageType.E_PlayerDetailInfoPage)
		{
			UIPlayTween[] components = UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.GetComponents<UIPlayTween>();
			foreach (UIPlayTween uIPlayTween in components)
			{
				uIPlayTween.Play(true);
			}
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.transform.Find("Label").GetComponent<UILabel>().text = "EQUIP";
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "pic_decal041";
			nowPageType = PageType.E_TeamInfoPage;
			if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1063).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
			{
				UIBASECOURSEINFO.GetCourse(1063).STATE = UtilUICourseInfo.CoursePhaseState.Done;
				UIBASECOURSEINFO.AddCursor(1064, backGO, backGO.transform.position);
				UIBASECOURSEINFO.GetCourse(1064).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
			}
		}
		else if (nowPageType == PageType.E_ExhibitionPage)
		{
			mTeamInfoContentGO.SetActive(true);
			mBackgroundGO.SetActive(true);
			mExhibitionContentGO.SetActive(false);
			UITEAMEXHIBITIONSCRIPT.Exit();
		}
	}

	protected void HandlePropertyInfoPartIAPBtnClick()
	{
		UIDialogManager.Instance.ShowShopDialogUI(HandleBuyIAPFinishedEvent);
	}

	public void HandleBuyIAPFinishedEvent(int code)
	{
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		UISTANDBYPLAYERSINFOSCRIPT.UpdateAllStandbyPlayersUI();
	}

	public void BlindPropertyInfoPartFunction(UtilUIPropertyInfo_BackBtnClick_Delegate back, UtilUIPropertyInfo_IAPBtnClick_Delegate iap)
	{
		UIPROPERTYINFOSCRIPT.SetBackBtnClickDelegate(back);
		UIPROPERTYINFOSCRIPT.SetIAPBtnClickDelegate(iap);
	}

	public void UpdatePropertyInfoPart(string name, int rank, int gold, int crystal)
	{
		if (name != "null#")
		{
			UIPROPERTYINFOSCRIPT.UpdateName(name);
		}
		if (rank > 0)
		{
			UIPROPERTYINFOSCRIPT.UpdateRank(rank.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFOSCRIPT.UpdateRank(0 + string.Empty);
		}
		if (gold > 0)
		{
			UIPROPERTYINFOSCRIPT.UpdateGold(gold.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFOSCRIPT.UpdateGold(0 + string.Empty);
		}
		if (crystal > 0)
		{
			UIPROPERTYINFOSCRIPT.UpdateCrystal(crystal.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFOSCRIPT.UpdateCrystal(0 + string.Empty);
		}
	}

	protected void HandleTeamInfoPartTeamBounsBtnClick(bool bUnlock)
	{
		if (bUnlock)
		{
			nowPageType = PageType.E_BonusPage;
			mTeamBonusContentGO.SetActive(true);
			mTeamInfoContentGO.SetActive(false);
		}
		else
		{
			UIDialogManager.Instance.ShowDriftMsgInfoUI(DataCenter.Save().teamAttributeSaveData.teamGeniusUnlockCondition);
		}
		CheckNewThingsUnlocked(true);
	}

	protected void HandleTeamInfoPartToggleStateChange(int index, bool bCheck, bool bMTSelect)
	{
		TeamSiteData teamSiteData = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)index);
		if (teamSiteData.playerData != null)
		{
			UITEAMPLAYERDETAILINFOSCRIPT.SetData(teamSiteData.playerData.heroIndex);
			DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(teamSiteData.playerData.heroIndex);
			DataConf.HeroSkillInfo heroSkillInfo = DataCenter.Conf().GetHeroSkillInfo(heroDataByIndex.characterType, teamSiteData.playerData.skillLevel, teamSiteData.playerData.skillStar);
			DataCenter.State().selectHeroIndex = teamSiteData.playerData.heroIndex;
			DataCenter.State().selectTeamSiteIndex = index;
			UITEAMPLAYERDETAILINFOSCRIPT.UpdateDetailUIInfo(heroDataByIndex.name, heroDataByIndex.iconFileName, teamSiteData.playerData.Hp + string.Empty, Mathf.CeilToInt(teamSiteData.playerData.Damage) + string.Empty, teamSiteData.playerData.Defense * 100f + "%", teamSiteData.playerData.CritRate + "%", teamSiteData.playerData.Speed.ToString("0.0") + string.Empty, teamSiteData.playerData.Hit + "%", teamSiteData.playerData.Dodge + "%", string.Empty + teamSiteData.playerData.Combat, heroDataByIndex.description, heroDataByIndex.profession, UIUtil.GetEquipTextureMaterial(DataCenter.Conf().GetWeaponDataByType(heroDataByIndex.weaponType).iconFileName).mainTexture, UIUtil.GetEquipTextureMaterial(heroSkillInfo.fileName).mainTexture);
			UISTANDBYPLAYERSINFOSCRIPT.SelectPlayerInTheCenterOfTheView(teamSiteData.playerData.heroIndex);
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.UpdatePageStateBtn(teamSiteData.playerData.state == Defined.ItemState.Available);
			lastSelectTeamSitenum = index;
			if (teamSiteData.playerData.upgradeData == null)
			{
				RequestLoadHeroEquipment(teamSiteData.playerData.heroIndex);
			}
			else
			{
				UpdateAllEquipsmentCanUpgradeTips(teamSiteData.playerData);
			}
		}
		else
		{
			UITEAMPLAYERSINFOSCRIPT.Select(lastSelectTeamSitenum);
		}
	}

	protected void HandleTeamInfoPartPlayerUnlcokStateBtnClick(int index)
	{
		DataCenter.State().selectTeamSiteIndex = index;
		RequestBuyTeamSite(0);
	}

	protected void RequestBuyTeamSiteUseCrystal()
	{
		RequestBuyTeamSite(1);
	}

	protected void RequestBuyTeamSite(int useCrystal)
	{
		DataCenter.State().protocoUseCrystal = useCrystal;
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 2);
		UIDialogManager.Instance.ShowBlock(2);
		//HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_BuyTeamSite, OnTeamBuyTeamSiteFinished);
		OnTeamBuyTeamSiteFinished(HeroListController.BuySite());
	}

	private List<int> GetCanEnqueueList(int siteNum, bool includeFreeSeat)
	{
		List<int> list = new List<int>();
		if (siteNum == 0)
		{
			includeFreeSeat = false;
		}
		TeamSiteData[] allTeamSiteData = DataCenter.Save().GetAllTeamSiteData();
		for (int i = 0; i < allTeamSiteData.Length; i++)
		{
			if (siteNum != i && allTeamSiteData[i].state != 0 && allTeamSiteData[i].state != Defined.ItemState.Purchase)
			{
				if (includeFreeSeat)
				{
					list.Add(i);
				}
				else if (allTeamSiteData[i].playerData != null)
				{
					list.Add(i);
				}
			}
		}
		return list;
	}

	protected void SetTeamSiteArrow(int siteNum, bool includeFreeSeat)
	{
		lsShowingArrows = GetCanEnqueueList(siteNum, includeFreeSeat);
	}

	public void BlindTeamInfoPartFunction(UtilUITeamPlayersInfo_TeamBonusBtnClick_Delegate teamBounsBtn, UtilUITeamPlayersInfo_TeamPlayersToggleStateChange_Delegate toggleStateChange, UtilUITeamPlayersInfo_TeamPlayerUnlockStateBtnClick_Delegate teamplayerUnlockStateBtn)
	{
		UITEAMPLAYERSINFOSCRIPT.SetTeamBonusBtnClickDelegate(teamBounsBtn);
		UITEAMPLAYERSINFOSCRIPT.SetTeamPlayersToggleStateChangeDelegate(toggleStateChange);
		UITEAMPLAYERSINFOSCRIPT.SetTeamPlayerUnlockStateBtnClickDelegate(teamplayerUnlockStateBtn);
	}

	public void InitTeamInfoPlayersInfo()
	{
		UpdateTeamUI();
		SetTeamSiteArrow(0, false);
		lastSelectTeamSitenum = 0;
		PlayerData playerData = DataCenter.Save().GetTeamSiteData(Defined.TEAM_SITE.TEAM_LEADER).playerData;
		DataCenter.State().selectHeroIndex = playerData.heroIndex;
		DataCenter.State().selectTeamSiteIndex = 0;
		UITEAMPLAYERSINFOSCRIPT.SetTeamBonusBUnlocked(DataCenter.Save().teamAttributeSaveData.teamGeniusUnLocked);
	}

	public void UpdateTeamUI()
	{
		TeamData teamData = DataCenter.Save().GetTeamData();
		for (int i = 0; i < teamData.teamSitesData.Length; i++)
		{
			TeamSiteData teamSiteData = teamData.teamSitesData[i];
			UITEAMPLAYERSINFOSCRIPT.SetPlayerEnableState(i, false, string.Empty);
			UITEAMPLAYERSINFOSCRIPT.SetPlayerUnlockState(i, false, -1, -1);
			if (teamSiteData.state == Defined.ItemState.Locked)
			{
				UITEAMPLAYERSINFOSCRIPT.SetPlayerEnableState(i, true, "team level " + teamSiteData.unlockSiteLevel);
			}
			else if (teamSiteData.state == Defined.ItemState.Purchase)
			{
				UITEAMPLAYERSINFOSCRIPT.SetPlayerUnlockState(i, true, teamSiteData.unlockSitePrice, teamSiteData.unlockSiteMoney);
			}
			else if (teamSiteData.playerData != null)
			{
				DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(teamSiteData.playerData.heroIndex);
				UITEAMPLAYERSINFOSCRIPT.UpdatePlayerIcon(i, heroDataByIndex.iconFileName);
			}
			else
			{
				UITEAMPLAYERSINFOSCRIPT.UpdatePlayerIcon(i, string.Empty);
			}
		}
		UITEAMPLAYERSINFOSCRIPT.UpdateTeamCombat(string.Empty + teamData.GetTeamCombat());
	}

	protected void UpdateCursorPosition(Vector3 touchPos)
	{
		Vector3 localPosition = Util.ScreenPointToNGUIForAnroid(touchPos);
		cursorGO.transform.localPosition = localPosition;
	}

	protected void HandletandbyPlayerItemClicked(UtilUIStandbyPlayersInfo.PLAYERINFO pi)
	{
		if (pi.data.SITEINDEX >= 0)
		{
			UITEAMPLAYERSINFOSCRIPT.Select(pi.data.SITEINDEX);
			lastSelectTeamSitenum = pi.data.SITEINDEX;
		}
		SetTeamSiteArrow(pi.data.SITEINDEX, true);
		UITEAMPLAYERDETAILINFOSCRIPT.SetData(pi.data.PLAYERDATA.heroIndex);
		PlayerData pLAYERDATA = pi.data.PLAYERDATA;
		DataConf.HeroData hEROCONF = pi.data.HEROCONF;
		DataConf.HeroSkillInfo heroSkillInfo = DataCenter.Conf().GetHeroSkillInfo(hEROCONF.characterType, pi.data.PLAYERDATA.skillLevel, pi.data.PLAYERDATA.skillStar);
		DataCenter.State().selectHeroIndex = pi.data.ID;
		UITEAMPLAYERDETAILINFOSCRIPT.UpdateDetailUIInfo(hEROCONF.name, hEROCONF.iconFileName, pLAYERDATA.Hp + string.Empty, Mathf.CeilToInt(pLAYERDATA.Damage) + string.Empty, pLAYERDATA.Defense * 100f + "%", pLAYERDATA.CritRate + "%", pLAYERDATA.Speed.ToString("0.0") + string.Empty, pLAYERDATA.Hit + "%", pLAYERDATA.Dodge + "%", string.Empty + pLAYERDATA.Combat, hEROCONF.description, hEROCONF.profession, UIUtil.GetEquipTextureMaterial(DataCenter.Conf().GetWeaponDataByType(hEROCONF.weaponType).iconFileName).mainTexture, UIUtil.GetEquipTextureMaterial(heroSkillInfo.fileName).mainTexture);
		UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.UpdatePageStateBtn(pi.data.PLAYERDATA.state == Defined.ItemState.Available);
		if (pi.data.PLAYERDATA.state == Defined.ItemState.Available || pi.data.PLAYERDATA.state == Defined.ItemState.Purchase)
		{
		}
		if (pi.data.PLAYERDATA.upgradeData == null)
		{
			RequestLoadHeroEquipment(pi.data.PLAYERDATA.heroIndex);
		}
		else
		{
			UpdateAllEquipsmentCanUpgradeTips(pi.data.PLAYERDATA);
		}
	}

	public void HandletandbyPlayerItemUnlockClicked(UtilUIStandbyPlayersInfo.PLAYERINFO pi)
	{
		DataCenter.State().selectHeroIndex = pi.data.PLAYERDATA.heroIndex;
		RequestUnlockHero(0);
	}

	private void Update()
	{
		if (pressTime > 0f && Time.time - pressTime > 0.1f)
		{
			HandleStandbyPlayerFormationControlDragStartEvent(gPlayerInfo);
			pressTime = -1f;
		}
	}

	protected void HandleStandbyPlayerFormationControlPressEvent(UtilUIStandbyPlayersInfo.PLAYERINFO playerInfo, bool isPressed)
	{
		if (isPressed)
		{
			pressTime = Time.time;
			gPlayerInfo = playerInfo;
			currentStartPos = UICamera.currentTouch.pos;
		}
		else
		{
			HandleStandPlayerFormationControlDragReleasedEvent(UICamera.hoveredObject);
			pressTime = -1f;
		}
		if (UIBASECOURSEINFO.TutorialInProgress && isPressed && UIBASECOURSEINFO.GetCourse(1051).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(1051).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			UIBASECOURSEINFO.AddCursor(1052, teamSiteBtnGO, teamSiteBtnGO.transform.position);
			UIBASECOURSEINFO.GetCourse(1052).SetExplainGO(null);
			UIBASECOURSEINFO.GetCourse(1052).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
			UtilUIStandbyPlayersInfo.PLAYERINFO playerInfoById = UISTANDBYPLAYERSINFOSCRIPT.GetPlayerInfoById(UIConstant.gCouseHeroId);
			Object.Destroy(playerInfoById.formationControl.gameObject.GetComponent<UIDragScrollView>());
			UIBASECOURSEINFO.AddCursor(1053, playerInfoById.go, playerInfoById.go.transform, teamSiteBtnGO.transform);
			UIBASECOURSEINFO.GetCourse(1053).SetExplainGO(playerInfoById.go.transform.Find("ExplainGODrag").gameObject);
			UIBASECOURSEINFO.GetCourse(1053).SetMoveToUnderTargetTrans(playerInfoById.go.transform.Find("CoureseTrans"));
			UIBASECOURSEINFO.GetCourse(1053).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	public void StandbyPlayerDragStart()
	{
		HandleStandPlayerFormationControlDragReleasedEvent(UICamera.hoveredObject);
		pressTime = -1f;
	}

	protected void HandleStandbyPlayerFormationControlDragStartEvent(UtilUIStandbyPlayersInfo.PLAYERINFO pi)
	{
		bStartDragAvibalCharacter = false;
		if (pi.data.PLAYERDATA.state == Defined.ItemState.Available)
		{
			DataConf.HeroData hEROCONF = pi.data.HEROCONF;
			cursorGO.SetActive(true);
			cursorIcon.spriteName = hEROCONF.iconFileName;
			UpdateCursorPosition(currentStartPos);
			bStartDragAvibalCharacter = true;
		}
	}

	protected void HandleStandbyPlayerFormationControlDragMovedEvent(GameObject go, Vector3 delta)
	{
		if (!bStartDragAvibalCharacter)
		{
			return;
		}
		UpdateCursorPosition(UICamera.currentTouch.pos);
		int num = cursorInfo.CrashDetectionIndex();
		if (num != -1)
		{
			if (lsShowingArrows.Contains(num))
			{
				UITEAMPLAYERSINFOSCRIPT.SetFormationSiteAccess(num);
			}
		}
		else
		{
			UITEAMPLAYERSINFOSCRIPT.SetFormationSiteAccessReset();
		}
	}

	protected void HandleStandPlayerFormationControlDragReleasedEvent(GameObject go)
	{
		if (!bStartDragAvibalCharacter)
		{
			return;
		}
		cursorGO.SetActive(false);
		int num = cursorInfo.CrashDetectionIndex();
		if (UIBASECOURSEINFO.TutorialInProgress)
		{
			if (num == 1)
			{
				DataCenter.State().selectHeroIndex = UISTANDBYPLAYERSINFOSCRIPT.GetNowSelectItemIndex();
				DataCenter.State().selectTeamSiteIndex = num;
				UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 4);
				UIDialogManager.Instance.ShowBlock(4);
				HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_EquipHero, OnTeamEquipHeroFinished);
			}
			else
			{
				UITEAMPLAYERSINFOSCRIPT.SetFormationSiteAccessReset();
				if (UIBASECOURSEINFO.GetCourse(1052).STATE == UtilUICourseInfo.CoursePhaseState.InProgress || UIBASECOURSEINFO.GetCourse(1053).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
				{
					UIBASECOURSEINFO.GetCourse(1052).STATE = UtilUICourseInfo.CoursePhaseState.Done;
					UIBASECOURSEINFO.GetCourse(1053).STATE = UtilUICourseInfo.CoursePhaseState.Done;
					UtilUIStandbyPlayersInfo.PLAYERINFO playerInfoById = UISTANDBYPLAYERSINFOSCRIPT.GetPlayerInfoById(UIConstant.gCouseHeroId);
					Object.Destroy(playerInfoById.formationControl.gameObject.GetComponent<UIDragScrollView>());
					UIBASECOURSEINFO.AddCursor(1051, playerInfoById.go, playerInfoById.go.transform.position);
					UIBASECOURSEINFO.GetCourse(1051).SetExplainGO(playerInfoById.go.transform.Find("ExplainGOPress").gameObject);
					UIBASECOURSEINFO.GetCourse(1051).SetMoveToUnderTargetTrans(playerInfoById.go.transform.Find("CoureseTrans"));
					UIBASECOURSEINFO.GetCourse(1051).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
				}
			}
		}
		else if (num != -1)
		{
			TeamSiteData teamSiteData = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)num);
			if (lsShowingArrows.Contains(num))
			{
				DataCenter.State().selectHeroIndex = UISTANDBYPLAYERSINFOSCRIPT.GetNowSelectItemIndex();
				DataCenter.State().selectTeamSiteIndex = num;
				UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 4);
				UIDialogManager.Instance.ShowBlock(4);
				HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_EquipHero, OnTeamEquipHeroFinished);
			}
			else
			{
				UITEAMPLAYERSINFOSCRIPT.SetFormationSiteAccessReset();
			}
		}
		else
		{
			UITEAMPLAYERSINFOSCRIPT.SetFormationSiteAccessReset();
		}
		bStartDragAvibalCharacter = false;
	}

	public void BlindStandbyPlayerInfoPartFunction(UtilUIStandbyPlayersInfo_AppearanceItemBtnClicked_Delegate itemClick, UtilUIStandbyPlayersInfo_AppearanceItemUnlockBtnClicked_Delegate itemUnlockClick, UtilUIStandbyPlayersInfo_FormationControlOnPressTouchGO press, UtilUIStandbyPlayersInfo_FormationControlOnDragDropStartTouchGO start, UtilUIStandbyPlayersInfo_FormationControlOnDragDropMovedTouchGO move, UtilUIStandbyPlayersInfo_FormationControlOnDragDropReleasedTouchGO release)
	{
		UISTANDBYPLAYERSINFOSCRIPT.SetStandbyPlayerAppearanceItemBtnClickedDelegate(itemClick);
		UISTANDBYPLAYERSINFOSCRIPT.SetStandbyPlayerAppearanceItemUnlockBtnClickedDelegate(itemUnlockClick);
		UISTANDBYPLAYERSINFOSCRIPT.BlindFunction(press, start, move, release);
	}

	public void InitStandbyPlayerInfo()
	{
		if (DataCenter.Save().GetTeamSiteData(Defined.TEAM_SITE.TEAM_LEADER).playerData == null)
		{
			UISTANDBYPLAYERSINFOSCRIPT.Init(DataCenter.Save().GetHeroList(), DataCenter.Save().GetTeamData(), DataCenter.Save().GetHeroList()[0].heroIndex);
			UITEAMPLAYERDETAILINFOSCRIPT.UpdateDetailUIInfo(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null);
			return;
		}
		UISTANDBYPLAYERSINFOSCRIPT.Init(DataCenter.Save().GetHeroList(), DataCenter.Save().GetTeamData(), DataCenter.Save().GetTeamSiteData(Defined.TEAM_SITE.TEAM_LEADER).playerData.heroIndex);
		PlayerData playerData = DataCenter.Save().GetTeamSiteData(Defined.TEAM_SITE.TEAM_LEADER).playerData;
		DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
		DataConf.HeroSkillInfo heroSkillInfo = DataCenter.Conf().GetHeroSkillInfo(heroDataByIndex.characterType, playerData.skillLevel, playerData.skillStar);
		UITEAMPLAYERDETAILINFOSCRIPT.SetData(playerData.heroIndex);
		UITEAMPLAYERDETAILINFOSCRIPT.UpdateDetailUIInfo(heroDataByIndex.name, heroDataByIndex.iconFileName, playerData.Hp + string.Empty, Mathf.CeilToInt(playerData.Damage) + string.Empty, playerData.Defense * 100f + "%", playerData.CritRate + "%", playerData.Speed.ToString("0.0") + string.Empty, playerData.Hit + "%", playerData.Dodge + "%", string.Empty + playerData.Combat, heroDataByIndex.description, heroDataByIndex.profession, UIUtil.GetEquipTextureMaterial(DataCenter.Conf().GetWeaponDataByType(heroDataByIndex.weaponType).iconFileName).mainTexture, UIUtil.GetEquipTextureMaterial(heroSkillInfo.fileName).mainTexture);
		if (playerData.upgradeData == null)
		{
			RequestLoadHeroEquipment(playerData.heroIndex);
		}
		else
		{
			UpdateAllEquipsmentCanUpgradeTips(playerData);
		}
	}

	protected void RequestUnlockHeroUseCrystal()
	{
		RequestUnlockHero(1);
	}

	protected void RequestUnlockHero(int useCrystal)
	{
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 3);
		UIDialogManager.Instance.ShowBlock(3);
		//HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_BuyHero, OnTeamBuyHeroFinished);
		OnTeamBuyHeroFinished(HeroListController.BuyHero());
	}

	public void InitTeamPlayerDetailInfo()
	{
		UITEAMPLAYERDETAILINFOSCRIPT.Init(OnEquipmentChanged_Delegate, HandleTeamPlayerDetailInfoStateChangeBtnClickEvent, HandleExTitleStateChangeEvent);
	}

	public void UpdateAllEquipsmentCanUpgradeTips(PlayerData playdata)
	{
		bool flag = UpdateHeroDetailWSPartToggleNewTips(playdata);
		bool flag2 = UpdateHeroDetailHeadPartToggleNewTips(playdata);
		bool flag3 = UpdateHeroDetailBodyPartToggleNewTips(playdata);
		bool flag4 = UpdateHeroDetailAccPartToggleNewTips(playdata);
		if ((flag || flag2 || flag3 || flag4) && playdata.state == Defined.ItemState.Available)
		{
			EquipmentBtnUBNewTipsGO.SetActive(true);
		}
		else
		{
			EquipmentBtnUBNewTipsGO.SetActive(false);
		}
	}

	public bool UpdateHeroDetailWSPartToggleNewTips(PlayerData playdata)
	{
		bool flag = false;
		if (playdata.upgradeData.weaponCanUpgrade || playdata.upgradeData.weaponCanBk || playdata.upgradeData.skillCanUpgrade || playdata.upgradeData.skillCanBk)
		{
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(0, true);
			return true;
		}
		UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(0, false);
		return false;
	}

	public bool UpdateHeroDetailHeadPartToggleNewTips(PlayerData playdata)
	{
		bool result = false;
		for (int i = 0; i < playdata.upgradeData.helmsUpgrade.Length; i++)
		{
			if (playdata.upgradeData.helmsUpgrade[i].equipIndex == playdata.equips[Defined.EQUIP_TYPE.Head].currEquipIndex)
			{
				if (playdata.upgradeData.helmsUpgrade[i].canUpgrade)
				{
					UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(1, true);
					result = true;
				}
				else
				{
					UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(1, false);
					result = false;
				}
				break;
			}
		}
		return result;
	}

	public bool UpdateHeroDetailBodyPartToggleNewTips(PlayerData playdata)
	{
		bool result = false;
		for (int i = 0; i < playdata.upgradeData.ArmorsUpgrade.Length; i++)
		{
			if (playdata.upgradeData.ArmorsUpgrade[i].equipIndex == playdata.equips[Defined.EQUIP_TYPE.Body].currEquipIndex)
			{
				if (playdata.upgradeData.ArmorsUpgrade[i].canUpgrade)
				{
					UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(2, true);
					result = true;
				}
				else
				{
					UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(2, false);
					result = false;
				}
				break;
			}
		}
		return result;
	}

	public bool UpdateHeroDetailAccPartToggleNewTips(PlayerData playdata)
	{
		bool result = false;
		for (int i = 0; i < playdata.upgradeData.ornamentsUpgrade.Length; i++)
		{
			if (playdata.upgradeData.ornamentsUpgrade[i].equipIndex == playdata.equips[Defined.EQUIP_TYPE.Acc].currEquipIndex)
			{
				if (playdata.upgradeData.ornamentsUpgrade[i].canUpgrade)
				{
					UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(3, true);
					result = true;
				}
				else
				{
					UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILEXDATAINFOSCRIPT.SetToggleTipsVisable(3, false);
					result = false;
				}
				break;
			}
		}
		return result;
	}

	public void ShowTeamPlayerDetailInfo()
	{
		UITEAMPLAYERDETAILINFOSCRIPT.UpdatExDetailUIInfo();
		UITEAMPLAYERDETAILINFOSCRIPT.SetExSelectTitle(0);
		UpdateAllEquipsmentCanUpgradeTips(UITEAMPLAYERDETAILINFOSCRIPT.playdata);
		UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.transform.Find("Label").GetComponent<UILabel>().text = "BASE";
		UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "pic_decal040";
		nowPageType = PageType.E_PlayerDetailInfoPage;
		if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1054).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(1054).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			UIBASECOURSEINFO.AddCursor(1055, weaponUpgradeBtnGO, weaponUpgradeBtnGO.transform.position);
			UIBASECOURSEINFO.GetCourse(1055).SetMoveToUnderTargetTrans(weaponUpgradeBtnGO.transform.Find("CoureseTrans"));
			UIBASECOURSEINFO.GetCourse(1055).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	protected void HandleExTitleStateChangeEvent(Defined.EQUIP_TYPE _type)
	{
		if (_type == Defined.EQUIP_TYPE.Head && UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1057).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
		{
			UIBASECOURSEINFO.GetCourse(1057).STATE = UtilUICourseInfo.CoursePhaseState.Done;
			UIBASECOURSEINFO.AddCursor(1058, helmsEquipmentUpgradeGO, helmsEquipmentUpgradeGO.transform.position);
			UIBASECOURSEINFO.GetCourse(1058).SetMoveToUnderTargetTrans(helmsEquipmentUpgradeGO.transform.Find("CoureseTrans"));
			UIBASECOURSEINFO.GetCourse(1058).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
		}
	}

	protected void OnEquipmentChanged_Delegate(UtilUITeamPlayerDetailInfo.EquipmentChangedType _key)
	{
		PlayerData playerData = DataCenter.Save().GetPlayerData(UISTANDBYPLAYERSINFOSCRIPT.GetNowSelectItemIndex());
		DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
		DataConf.HeroSkillInfo heroSkillInfo = DataCenter.Conf().GetHeroSkillInfo(heroDataByIndex.characterType, playerData.skillLevel, playerData.skillStar);
		UITEAMPLAYERSINFOSCRIPT.UpdateTeamCombat(string.Empty + DataCenter.Save().GetTeamData().GetTeamCombat());
		UITEAMPLAYERDETAILINFOSCRIPT.UpdateDetailUIInfo(heroDataByIndex.name, heroDataByIndex.iconFileName, playerData.Hp + string.Empty, Mathf.CeilToInt(playerData.Damage) + string.Empty, playerData.Defense * 100f + "%", playerData.CritRate + "%", playerData.Speed.ToString("0.0") + string.Empty, playerData.Hit + "%", playerData.Dodge + "%", string.Empty + playerData.Combat, heroDataByIndex.description, heroDataByIndex.profession, UIUtil.GetEquipTextureMaterial(DataCenter.Conf().GetWeaponDataByType(heroDataByIndex.weaponType).iconFileName).mainTexture, UIUtil.GetEquipTextureMaterial(heroSkillInfo.fileName).mainTexture);
		UISTANDBYPLAYERSINFOSCRIPT.UpdateBaseUI(UISTANDBYPLAYERSINFOSCRIPT.GetNowSelectItemIndex(), false);
		if (playerData.upgradeData == null)
		{
			RequestLoadHeroEquipment(playerData.heroIndex);
		}
		else
		{
			UpdateAllEquipsmentCanUpgradeTips(playerData);
		}
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		switch (_key)
		{
		case UtilUITeamPlayerDetailInfo.EquipmentChangedType.E_WeaponUpgrade:
			if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1055).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
			{
				UIBASECOURSEINFO.GetCourse(1055).STATE = UtilUICourseInfo.CoursePhaseState.Done;
				UIBASECOURSEINFO.AddCursor(1056, skillUpgradeBtnGO, skillUpgradeBtnGO.transform.position);
				UIBASECOURSEINFO.GetCourse(1056).SetMoveToUnderTargetTrans(skillUpgradeBtnGO.transform.Find("CoureseTrans"));
				UIBASECOURSEINFO.GetCourse(1056).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
			}
			break;
		case UtilUITeamPlayerDetailInfo.EquipmentChangedType.E_SkillUpgrade:
			if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1056).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
			{
				UIBASECOURSEINFO.GetCourse(1056).STATE = UtilUICourseInfo.CoursePhaseState.Done;
				UIBASECOURSEINFO.AddCursor(1057, helmsToggleGO, helmsToggleGO.transform.position);
				UIBASECOURSEINFO.GetCourse(1057).STATE = UtilUICourseInfo.CoursePhaseState.InProgress;
			}
			break;
		case UtilUITeamPlayerDetailInfo.EquipmentChangedType.E_EquipHeadUpgrade:
			if (UIBASECOURSEINFO.TutorialInProgress && UIBASECOURSEINFO.GetCourse(1058).STATE == UtilUICourseInfo.CoursePhaseState.InProgress)
			{
				UIBASECOURSEINFO.GetCourse(1058).STATE = UtilUICourseInfo.CoursePhaseState.Done;
				DataCenter.Save().tutorialStep = Defined.TutorialStep.Finish;
				DataCenter.Save().SaveGameData();
			}
			break;
		}
	}

	protected void HandleShowTipsBtnClickedEvent()
	{
		mExhibitionContentGO.SetActive(true);
		gExhibitionCamera.enabled = true;
		UITEAMEXHIBITIONSCRIPT.Enter(UITEAMPLAYERDETAILINFOSCRIPT.playdata.heroIndex);
	}

	protected void HandleHideTipsBtnClickedEvent()
	{
		mExhibitionContentGO.SetActive(false);
		gExhibitionCamera.enabled = false;
		UITEAMEXHIBITIONSCRIPT.Exit();
	}

	protected void HandleTeamPlayerDetailInfoStateChangeBtnClickEvent()
	{
		UIPlayTween[] componentsInChildren = UITEAMPLAYERDETAILINFOSCRIPT.stateChangeGO.GetComponentsInChildren<UIPlayTween>();
		resetTeamInfoTweenScript.HandleBtnToCheck();
		if (nowPageType == PageType.E_TeamInfoPage)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Play(true);
			}
			if (DataCenter.Save().GetPlayerData(DataCenter.State().selectHeroIndex).upgradeData == null)
			{
				UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 6);
				UIDialogManager.Instance.ShowBlock(6);
				HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_LoadEquipmentInfo, OnTeamLoadEquipmentInfoFinished);
			}
			else
			{
				ShowTeamPlayerDetailInfo();
			}
		}
		else if (nowPageType == PageType.E_PlayerDetailInfoPage)
		{
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].Play(false);
			}
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.transform.Find("Label").GetComponent<UILabel>().text = "EQUIP";
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.PageStateBtn.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "pic_decal041";
			nowPageType = PageType.E_TeamInfoPage;
		}
		if (UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.GetTipsVisable())
		{
			UITEAMPLAYERDETAILINFOSCRIPT.TPDETAILDATAINFOSCRIPT.SetTipsVisable(false);
		}
		CheckNewThingsUnlocked(true);
	}

	public void InitTeamBounsInfo()
	{
		UITEAMBONUSTALENTSCRIPT.Init(DataCenter.Save().teamAttributeSaveData, OnTeamTalentChaned);
		UITEAMBONUSEVOLVESCRIPT.Init(DataCenter.Save().teamAttributeSaveData, OnTeamEvolveChaned);
		UITEAMBONUSEVOLVESCRIPT.SetTeamEnvolBUnlocked(DataCenter.Save().teamAttributeSaveData.teamEvolutionUnLocked);
	}

	public void HandleTeamBounsToggleCopyBtnClick()
	{
		UIDialogManager.Instance.ShowDriftMsgInfoUI(DataCenter.Save().teamAttributeSaveData.teamEvolutionUnlockCondition);
	}

	public void OnTeamTalentChaned()
	{
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}

	public void OnTeamEvolveChaned()
	{
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}
}

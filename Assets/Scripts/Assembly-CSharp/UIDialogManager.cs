using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class UIDialogManager : MonoBehaviour
{
	private static UIDialogManager mInstance;

	[SerializeField]
	private UIDriftMessageReminder driftMessageReminderScript;

	[SerializeField]
	private UtilUIShopControl shopControlScript;

	[SerializeField]
	private UtilUIDialogPopupControl popupControlScript;

	[SerializeField]
	private UtilUIDialogPopupBControl popupBControlScript;

	[SerializeField]
	private GameObject block;

	public UtilUIReviewDialog reviewDialogScript;

	private UtilUIPropertyInfo m_scriptUIPropertyInfo;

	[SerializeField]
	private List<int> blockIds = new List<int>();

	private UtilUIShopControl_BuyIAPFinished_Delegate buyIAPfinishEvent;

	private bool paused;

	private float pausedTime;

	public static UIDialogManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = GameObject.Find("UI Dialog").GetComponent<UIDialogManager>();
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

	public void SetPropertyScript(UtilUIPropertyInfo scr)
	{
		m_scriptUIPropertyInfo = scr;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		if (Application.loadedLevelName.StartsWith("UI") || Application.loadedLevelName.StartsWith("Load"))
		{
			UIRootAutoSet();
		}
	}

	private void Start()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDestroy()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		if (!isActiveAndEnabled)
		{
			return;
		}
		int level = scene.buildIndex;
		Instance.HideShopDialogUI();
		Instance.ClearBlocks();
		UIEffectManager.Instance.ClearEffects();
		UtilUIAccountManager.mInstance.HideAll();
		if (Application.loadedLevelName.StartsWith("UIEntry") || Application.loadedLevelName.StartsWith("UICheckUpdate"))
		{
			HidePopupA();
		}
		if (level != 0 && (Application.loadedLevelName.StartsWith("UI") || Application.loadedLevelName.StartsWith("Load")))
		{
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}
		if (Application.loadedLevelName.StartsWith("UI") || Application.loadedLevelName.StartsWith("Load"))
		{
			UIRootAutoSet();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!DataCenter.Save().BattleTutorialFinished)
		{
			return;
		}
		paused = pauseStatus;
		if (paused)
		{
			pausedTime = Time.realtimeSinceStartup;
			return;
		}
		if (Time.realtimeSinceStartup - pausedTime >= 10f && UIConstant.bNeedLoseConnect)
		{
			if (GameBattle.m_instance != null)
			{
				GameBattle.m_instance.OnBreakOff();
			}
			DataCenter.State().ResetData(false);
			Application.LoadLevel("UICheckUpdate");
		}
		pausedTime = Time.realtimeSinceStartup;
	}

	public void ShowShopDialogUI(UtilUIShopControl_BuyIAPFinished_Delegate _buyIAPfinishEvent)
	{
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 7);
		Instance.ShowBlock(7);
		buyIAPfinishEvent = _buyIAPfinishEvent;
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Store_GetIAPList, OnGetListIAPItemsFinished);
	}

	public void HideShopDialogUI()
	{
		shopControlScript.SetVisable(false);
		UIConstant.bNeedLoseConnect = true;
	}

	public void ShowDriftMsgInfoUI(string str)
	{
		driftMessageReminderScript.UpdateLabel(str);
		driftMessageReminderScript.Do();
	}

	public void ShowPopupA(string _str, UIWidget.Pivot _pivot, bool bShowCloseBtn, UtilUIDialogPopupControl_OKBtnClicked_Delegate _dele = null)
	{
		ShowPopupA(_str, _pivot, string.Empty, "OK", bShowCloseBtn, _dele);
	}

	public void ShowPopupA(string _str, UIWidget.Pivot _pivot, string btnIcon, string btnLabel, bool bShowCloseBtn, UtilUIDialogPopupControl_OKBtnClicked_Delegate _dele)
	{
		popupControlScript.Init(_str, _pivot, btnIcon, btnLabel, bShowCloseBtn, _dele);
		popupControlScript.SetVisable(true);
		UIPlaySound component = popupControlScript.gameObject.GetComponent<UIPlaySound>();
		if (component != null)
		{
			component.Play();
		}
	}

	public void HidePopupA()
	{
		popupControlScript.SetVisable(false);
	}

	public void ShowPopupB(string _str, UIWidget.Pivot _pivot, UtilUIDialogPopupBControl_LBtnClicked_Delegate _lDele, UtilUIDialogPopupBControl_RBtnClicked_Delegate _rDele, string _leftLabel, string _rightLabel)
	{
		ShowPopupB(_str, _pivot, _lDele, _rDele, Defined.COST_TYPE.Money, 0, Defined.COST_TYPE.Honor, 0, _leftLabel, _rightLabel, false);
	}

	public void ShowPopupB(string _str, UIWidget.Pivot _pivot, UtilUIDialogPopupBControl_LBtnClicked_Delegate _lDele, UtilUIDialogPopupBControl_RBtnClicked_Delegate _rDele, Defined.COST_TYPE r1CT, int r1Num, Defined.COST_TYPE r2CT, int r2Num, string _leftLabel, string _rightLabel, bool bRBtnIconVisable)
	{
		popupBControlScript.Init(_str, _pivot, r1CT, r1Num, r2CT, r2Num, _leftLabel, _rightLabel, bRBtnIconVisable, _lDele, _rDele);
		popupBControlScript.SetVisable(true);
		UIPlaySound component = popupBControlScript.gameObject.GetComponent<UIPlaySound>();
		if (component != null)
		{
			component.Play();
		}
	}

	public void ShowHttpFeedBackMsg(int code)
	{
		ShowHttpFeedBackMsg(code, string.Empty, null, null, Defined.COST_TYPE.Money, 0, Defined.COST_TYPE.Crystal, 0, "Cancel", "OK", false);
	}

	public void ShowHttpFeedBackMsg(int code, string _strInput)
	{
		ShowHttpFeedBackMsg(code, _strInput, null, null, Defined.COST_TYPE.Money, 0, Defined.COST_TYPE.Crystal, 0, "Cancel", "OK", false);
	}

	public void ShowHttpFeedBackMsg(int code, string _strInput, UtilUIDialogPopupBControl_RBtnClicked_Delegate _rDele, Defined.COST_TYPE r1CT, int r1Num, Defined.COST_TYPE r2CT, int r2Num, string _rightLabel)
	{
		ShowHttpFeedBackMsg(code, _strInput, null, _rDele, r1CT, r1Num, r2CT, r2Num, "Cancel", _rightLabel, true);
	}

	public void ShowHttpFeedBackMsg(int code, string _strInput, UtilUIDialogPopupBControl_LBtnClicked_Delegate _lDele, UtilUIDialogPopupBControl_RBtnClicked_Delegate _rDele, Defined.COST_TYPE r1CT, int r1Num, Defined.COST_TYPE r2CT, int r2Num, string _leftLabel, string _rightLabel, bool bRBtnIconVisable)
	{
		string text = string.Empty;
		if (code >= 1000)
		{
			switch (code)
			{
			case 1000:
				text = "Not enough tCrystals.";
				break;
			case 1001:
				text = "Buy missing resources to " + _strInput + string.Empty;
				break;
			case 1002:
				text = "Buy missing resources to " + _strInput + string.Empty;
				break;
			case 1003:
				text = "Buy missing resources to " + _strInput + string.Empty;
				break;
			case 1004:
				text = string.Empty;
				break;
			case 1005:
				text = "Not enough points.";
				break;
			case 1006:
				text = "Unlock at team level 10.";
				break;
			case 1007:
				text = "Reach maximum level.";
				break;
			case 1008:
				text = "You cannot buy any genius points.";
				break;
			case 1009:
				text = "geniusUnlocked.";
				break;
			case 1010:
				text = "Do you want to spend $0.99 to unlock the feature of resetting genius points?";
				break;
			case 1011:
				text = "Unlock at team level 20.";
				break;
			case 1012:
				text = string.Empty;
				break;
			case 1013:
				text = "Reach maximum level.";
				break;
			case 1014:
				text = string.Empty;
				break;
			case 1015:
				text = "Reach maximum level.";
				break;
			case 1016:
				text = string.Empty;
				break;
			case 1017:
				text = "Reach maximum level.";
				break;
			case 1018:
				text = string.Empty;
				break;
			case 1019:
				text = string.Empty;
				break;
			case 1020:
				text = string.Empty;
				break;
			case 1021:
				text = "Reach maximum level.";
				break;
			case 1022:
				text = string.Empty;
				break;
			case 1023:
				text = "Reach maximum level.";
				break;
			case 1024:
				text = string.Empty;
				break;
			case 1025:
				text = "Reach maximum level.";
				break;
			case 1026:
				text = string.Empty;
				break;
			case 1027:
				text = string.Empty;
				break;
			case 1028:
				text = string.Empty;
				break;
			case 1029:
				text = "Reach maximum level.";
				break;
			case 1030:
				text = _strInput + " locked.";
				break;
			case 1031:
				text = _strInput + " locked.";
				break;
			case 1032:
				text = _strInput + " locked.";
				break;
			case 1033:
				text = string.Empty;
				break;
			case 1034:
				text = string.Empty;
				break;
			case 1035:
				text = string.Empty;
				break;
			case 1036:
				text = string.Empty;
				break;
			case 1037:
				text = "Chapter not reachable";
				break;
			case 1038:
				text = string.Empty;
				break;
			case 1039:
				text = string.Empty;
				break;
			case 1040:
				text = string.Empty;
				break;
			case 1041:
				text = string.Empty;
				break;
			case 1042:
				text = string.Empty;
				break;
			case 1043:
				text = "Team leader cannot be empty.";
				break;
			case 1044:
				text = string.Empty;
				break;
			case 1045:
				text = string.Empty;
				break;
			case 1046:
				text = string.Empty;
				break;
			case 1047:
				text = string.Empty;
				break;
			case 1048:
				text = string.Empty;
				break;
			case 1049:
				text = string.Empty;
				break;
			case 1050:
				text = "You can't unlock the slot now!";
				break;
			case 1051:
				text = "User name is repeated!";
				break;
			case 1053:
				text = "Invalid user name.";
				break;
			case 1054:
				text = "Already got it.";
				break;
			case 1055:
				text = "No Items.";
				break;
			case 1062:
				text = "Account has already been logged in another device, Please try to connect again!";
				Debug.LogWarning("Receive the message kickedOff !!!!");
				break;
			}
			if (text != string.Empty)
			{
				switch (code)
				{
				case 1000:
					ShowDriftMsgInfoUI(text);
					HandleOpenShopBtnClickEvent();
					break;
				case 1001:
				case 1002:
				case 1003:
					ShowPopupB(text, UIWidget.Pivot.Center, _lDele, _rDele, r1CT, r1Num, r2CT, r2Num, _leftLabel, _rightLabel, bRBtnIconVisable);
					break;
				case 1062:
					ShowPopupA(text, UIWidget.Pivot.Center, false, OnSystemNetError);
					break;
				default:
					ShowDriftMsgInfoUI(text);
					break;
				}
			}
		}
		//else if (!Util.IsNetworkConnected())
		//{
		//	ShowPopupA("No network available now. Please check network connection.", UIWidget.Pivot.Center, false, OnSystemNetError);
		//}
		//else if (code == -6)
		//{
		//	ShowPopupA("Connection Timeout. Please try again later...", UIWidget.Pivot.Center, false, OnSystemNetError);
		//}
		//else
		//{
		//	ShowPopupA("Unable to connect to server. Please try again later.", UIWidget.Pivot.Center, false, OnSystemNetError);
		//}
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

	public void ShowBlock(int id)
	{
		if (!blockIds.Contains(id))
		{
			block.SetActive(true);
			blockIds.Add(id);
		}
	}

	public void HideBlock(int id)
	{
		if (blockIds.Contains(id))
		{
			blockIds.Remove(id);
		}
		if (blockIds.Count <= 0)
		{
			block.SetActive(false);
		}
	}

	public void ClearBlocks()
	{
		for (int i = 0; i < blockIds.Count; i++)
		{
		}
		blockIds.Clear();
		block.SetActive(false);
	}

	protected void HandleOpenShopBtnClickEvent()
	{
		Instance.ShowShopDialogUI(HandleBuyIAPFinishedEvent);
	}

	public void HandleBuyIAPFinishedEvent(int code)
	{
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}

	public void UpdatePropertyInfoPart(string name, int rank, int gold, int crystal)
	{
		if (UIPROPERTYINFO != null)
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
	}

	public void OnGetListIAPItemsFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 7);
		Instance.HideBlock(7);
		if (code != 0)
		{
			Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		//shopControlScript.InitIAPShop(UIConstant.gLSIAPItemsData, buyIAPfinishEvent);
		//shopControlScript.SetVisable(true);
	}

	public static void UIRootAutoSet()
	{
		if (!(Application.loadedLevelName == "UIEntry") && !(Application.loadedLevelName == "UICheckUpdate") && !Application.loadedLevelName.Contains("Load"))
		{
			List<UIRoot> list = UIRoot.list;
		}
	}
}

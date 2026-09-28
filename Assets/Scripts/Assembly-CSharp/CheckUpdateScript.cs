using System;
using System.Collections.Generic;
using ChartboostSDK;
using CoMDS2;
using UnityEngine;

public class CheckUpdateScript : MonoBehaviour
{
	public enum CheckPhase
	{
		Connecting = 0,
		DownLoading = 1,
		Loading = 2,
		Finish = 3
	}

	public GameObject noticePanelGO;

	public UILabel noticeLabel;

	public DownloadText downloadText;

	private string[] m_labelTextConnecting = new string[3] { "Connecting...", "Connecting.", "Connecting.." };

	private string[] m_labelTextDownLoading = new string[3] { "Downloading...", "Downloading.", "Downloading.." };

	private string[] m_labelTextLoading = new string[3] { "Loading...", "Loading.", "Loading.." };

	private string[] theMoment = new string[] { "Enemies", "Heroes", "Stuffs", "Weapons", "Equips", "HeroSkill", "SpecialAttribute", "TeamSpecialAttribute", "LoadingTips", "Achievements" };

	private string m_labelPressAnykey = "Press any key to continue.";

	private CheckPhase m_phase;

	public static CheckUpdateScript s_instance;

	private bool m_checkDownLoad;

	public UILabel m_UILabel;

	public UISlider m_UISlider;

	private int m_labelTextPlayIndex;

	private float m_timer;

	public List<KeyValuePair<string, string>> needDownloadConfigList;

	private bool m_bKeyDown;

	public CheckPhase Phase
	{
		set
		{
			m_phase = value;
			switch (m_phase)
			{
			case CheckPhase.Connecting:
				m_labelTextPlayIndex = 0;
				m_UILabel.text = m_labelTextConnecting[m_labelTextPlayIndex];
				m_UISlider.gameObject.SetActive(false);
				break;
			case CheckPhase.DownLoading:
				m_UISlider.gameObject.SetActive(true);
				m_UISlider.foregroundWidget.color = new Color(1f, 1f, 0.25f, 0.65f);
				m_labelTextPlayIndex = 0;
				m_UILabel.text = m_labelTextDownLoading[m_labelTextPlayIndex];
				break;
			case CheckPhase.Loading:
				m_UISlider.gameObject.SetActive(true);
				m_UISlider.foregroundWidget.color = new Color(0.25f, 1f, 0.31f, 0.75f);
				SetSliderBarSteps(15);
				SetSliderValue(0f);
				m_labelTextPlayIndex = 0;
				m_UILabel.text = m_labelTextLoading[m_labelTextPlayIndex];
				break;
			case CheckPhase.Finish:
				m_UILabel.text = string.Empty;
				m_UISlider.gameObject.SetActive(false);
				break;
			}
		}
	}

	private void Awake()
	{
		s_instance = this;
		downloadText.m_DownLoadErrorEvent = OnDownLoadErrorEvent;
		downloadText.m_DownLoadOKEvent = OnDownLoadOKEvent;
		needDownloadConfigList = new List<KeyValuePair<string, string>>();
		DataCenter.State().lastSceneType = Defined.SceneType.Menu;
		DataCenter.Save().LoadServerConfigData();
		//MyTapjoy.Instance();
		if (GameObject.Find("_AndroidPlatform") == null)
		{
			GameObject gameObject = new GameObject("_AndroidPlatform");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			DevicePlugin.InitAndroidPlatform();
			gameObject.AddComponent<TrinitiAdAndroidPlugin>();
			gameObject.AddComponent<AndroidQuit>();
		}
		foreach (int value in Enum.GetValues(typeof(DataConf.ConfigType)))
		{
			if (value == 10)
			{
				break;
			}
			string text = FileUtil.LoadResourcesFile("Configs/" + theMoment[value]);
			LoadConfigData((DataConf.ConfigType)value, text);
			//int num = value;
			//string configNameByType = DataCenter.Conf().GetConfigNameByType((DataConf.ConfigType)value);
			//string filename = "C/C" + num + ".dat";
			//try
			//{
			//	string input_data = FileUtil.ReadSave(filename);
			//	string content = Util.DecryptData(input_data, "C;g^L%S&K*7640");
			//	string unzipedcontent = string.Empty;
			//	Util.UnZipString(content, ref unzipedcontent);
			//	LoadConfigData((DataConf.ConfigType)value, unzipedcontent);
			//}
			//catch (Exception)
			//{
			//	DataCenter.Save().configVersion[configNameByType] = "0";
			//}
		}
		if (!Util.s_debug)
		{
		}
	}

	private void Start()
	{
		noticePanelGO.SetActive(false);
		m_checkDownLoad = true;
		m_timer = 0f;
		Phase = CheckPhase.Loading;
		m_bKeyDown = false;
		UtilUIAccountManager.mInstance.SetAccountDelegates(StartGameServer, ShowNotice);
	}

	private void Update()
	{
		switch (m_phase)
		{
		case CheckPhase.Connecting:
			m_timer += Time.deltaTime;
			if (m_timer >= 0.5f)
			{
				m_timer = 0f;
				m_labelTextPlayIndex++;
				if (m_labelTextPlayIndex >= m_labelTextConnecting.Length)
				{
					m_labelTextPlayIndex = 0;
				}
				m_UILabel.text = m_labelTextConnecting[m_labelTextPlayIndex];
			}
			break;
		case CheckPhase.DownLoading:
		{
			if (m_labelTextPlayIndex >= needDownloadConfigList.Count)
			{
				Phase = CheckPhase.Loading;
				DataCenter.Save().SaveServerConfigData();
				break;
			}
			KeyValuePair<string, string> keyValuePair = needDownloadConfigList[m_labelTextPlayIndex];
			DataConf.ConfigType configTypeByName = DataCenter.Conf().GetConfigTypeByName(keyValuePair.Key);
			LoadConfigData(configTypeByName, keyValuePair.Value);
			SaveConfigData(configTypeByName, keyValuePair.Value);
			m_labelTextPlayIndex++;
			SetSliderValue(1f / (float)needDownloadConfigList.Count * (float)m_labelTextPlayIndex);
			break;
		}
		case CheckPhase.Loading:
			switch (m_labelTextPlayIndex)
			{
			case 0:
				DataCenter.State().gameLoaded = false;
				if (DataCenter.Save().PlayMusic)
				{
					BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.UI_BG);
				}
				m_labelTextPlayIndex++;
				break;
			case 1:
				DataCenter.Conf().LoadAnimationDataFromDisk();
				DataCenter.Conf().LoadEffectDataFromDisk();
				m_labelTextPlayIndex++;
				break;
			case 2:
				DataCenter.Conf().LoadBulletDataFromDisk();
				m_labelTextPlayIndex++;
				break;
			case 3:
				DataCenter.Conf().LoadGameLevelNodeDataFromDisk();
				m_labelTextPlayIndex++;
				break;
			case 4:
				DataCenter.Conf().LoadSpecailEffectDataFromDisk();
				m_labelTextPlayIndex++;
				break;
			case 5:
				m_labelTextPlayIndex++;
				break;
			case 6:
				m_labelTextPlayIndex++;
				break;
			case 7:
				//HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_PlayerData, GetPlayerDataCallBack);
				HeroListController.Refresh();
				m_labelTextPlayIndex++;
				m_labelTextPlayIndex++;
				break;
			case 9:
				//if (!DataCenter.Save().bNewUser)
				//{
				//	HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.CreatePlayerData, CreatePlayerDataCallBack);
				//}
				//else
				//{
					m_labelTextPlayIndex++;
				//}
				break;
			case 10:
				m_labelTextPlayIndex++;
				break;
			case 11:
				m_labelTextPlayIndex++;
				break;
			case 12:
				//if (!DataCenter.Save().bNewUser)
				//{
				//	HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_WorldNodeList, CreateWorldNodeListCallBack);
				//}
				//else
				//{
					m_labelTextPlayIndex++;
				//}
				break;
			case 13:
				m_labelTextPlayIndex++;
				break;
			case 14:
				m_labelTextPlayIndex++;
				break;
			case 15:
				m_labelTextPlayIndex++;
				break;
			case 16:
				m_timer += Time.deltaTime;
				if (m_timer >= 0.5f)
				{
					m_timer = 0f;
					DataCenter.Save().SaveServerConfigData();
					Phase = CheckPhase.Finish;
				}
				break;
			}
			SetSliderValue(1f / 14f * (float)(m_labelTextPlayIndex / 1));
			break;
		case CheckPhase.Finish:
			m_timer += Time.deltaTime;
			if (!(m_timer >= 0.5f))
			{
				break;
			}
			m_timer = 0f;
			DataCenter.State().gameLoaded = true;
			//HttpRequestHandle.instance.SetLogin();
			if (!m_bKeyDown)
			{
				m_bKeyDown = true;
				DataCenter.Save().BattleTutorialFinished = !DataCenter.Save().bNewUser;
				if (DataCenter.Save().BattleTutorialFinished)
				{
					HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_WorldNodeList, null);
					HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_GetListGeniusInfos, CreateWorldNodeListCallBack);
					SceneManager.Instance.SwitchScene("UIBase");
					Chartboost.showInterstitial(CBLocation.Default);
					UIUtil.ShowOpenClik(true);
					UIConstant.bNeedLoseConnect = true;
				}
				else
				{
					DataCenter.Conf().SetCurrentGameLevel(DataCenter.State().selectLevelMode, -1);
					Application.LoadLevel("UINewPlayerCGExcessive");
				}
			}
			break;
		}
	}

	public void LoadConfigData(DataConf.ConfigType configType, string strContent)
	{
		switch (configType)
		{
		case DataConf.ConfigType.Enemy:
			DataCenter.Conf().LoadEnemyData(strContent);
			break;
		case DataConf.ConfigType.Hero:
			DataCenter.Conf().LoadHeroData(strContent);
			break;
		case DataConf.ConfigType.Stuff:
			DataCenter.Conf().LoadStuffData(strContent);
			break;
		case DataConf.ConfigType.Weapon:
			DataCenter.Conf().LoadWeaponData(strContent);
			break;
		case DataConf.ConfigType.Equip:
			DataCenter.Conf().LoadEquipData(strContent);
			break;
		case DataConf.ConfigType.HeroSkill:
			DataCenter.Conf().LoadHeroSkillData(strContent);
			break;
		case DataConf.ConfigType.SpecialAttribute:
			DataCenter.Conf().LoadSpecialAttributeData(strContent);
			break;
		case DataConf.ConfigType.TeamSpecialAttribute:
			DataCenter.Conf().LoadTeamAttributeData(strContent);
			break;
		case DataConf.ConfigType.LoadingTips:
			DataCenter.Conf().LoadLoadingTips(strContent);
			break;
		case DataConf.ConfigType.Achievement:
			DataCenter.Conf().LoadAchievements(strContent);
			break;
		}
	}

	public void SaveConfigData(DataConf.ConfigType configType, string strContent)
	{
		int num = (int)configType;
		string zipedcontent = string.Empty;
		Util.ZipString(strContent, ref zipedcontent);
		string content = Util.EncryptData(zipedcontent, "C;g^L%S&K*7640");
		FileUtil.WriteSave("/C", "C" + num + ".dat", content);
	}

	public void StartGameServer()
	{
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.GET_SERVERCONFIGS, OnGetConfigsRequestFinish);
	}

	public void OnNoticeOKBtnClickEvent()
	{
		noticePanelGO.SetActive(false);
		UtilUIAccountManager.mInstance.Begin();
	}

	public void OnNewVersionNeedUpgradeBtnClicked()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.trinitigame.android.callofminidoubleshot2");
		UIDialogManager.Instance.ShowPopupA("There's a new version available. Download now!", UIWidget.Pivot.Center, false, OnNewVersionNeedUpgradeBtnClicked);
	}

	public void OnDownLoadOKEvent(string str)
	{
		VersionValidationScript.Instance().LoadData(str);
		noticeLabel.text = VersionValidationScript.Instance().m_otherInfo.m_noticContent;
		bool bNeedUpgrade = false;
		string uRL = VersionValidationScript.Instance().GetURL(ref bNeedUpgrade);
		string accountURL = VersionValidationScript.Instance().GetAccountURL();
		string chatURL = VersionValidationScript.Instance().GetChatURL();
		if (bNeedUpgrade)
		{
			UIDialogManager.Instance.ShowPopupA("There's a new version available. Download now!", UIWidget.Pivot.Center, false, OnNewVersionNeedUpgradeBtnClicked);
			return;
		}
		HttpRequestHandle.instance.Init(uRL, accountURL, chatURL);
		HttpRequestHandle.instance.ReqServerTime();
		UtilUIAccountManager.mInstance.GetTAccountByLocalDeviceID();
	}

	public void ShowNotice()
	{
		//noticePanelGO.SetActive(true);
	}

	public void OnDownLoadErrorEvent(string str)
	{
		UIDialogManager.Instance.ShowHttpFeedBackMsg(-1);
	}

	public void OnGetConfigsRequestFinish(int code)
	{
		if (needDownloadConfigList.Count <= 0)
		{
			Phase = CheckPhase.Loading;
		}
	}

	public void SetSliderBarSteps(int step)
	{
		m_UISlider.numberOfSteps = step;
	}

	public void SetSliderValue(float value)
	{
		m_UISlider.value = value;
	}

	public void GetPlayerDataCallBack(int code)
	{
		//switch (code)
		//{
		//case 0:
		//	m_labelTextPlayIndex += 3;
		//	if (DataCenter.Save().userName == "newbie")
		//	{
		//		DataCenter.Save().bNewUser = true;
		//	}
		//	else
		//	{
		//		DataCenter.Save().bNewUser = false;
		//	}
		//	break;
		//case 1004:
		//	m_labelTextPlayIndex++;
		//	DataCenter.Save().bNewUser = true;
		//	HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.CreateUser, null);
		//	break;
		//default:
		//	UIDialogManager.Instance.ShowHttpFeedBackMsg(-999);
		//	break;
		//}
	}

	public void CreatePlayerDataCallBack(int code)
	{
		if (code == 0)
		{
			m_labelTextPlayIndex++;
		}
	}

	public void CreateWorldNodeListCallBack(int code)
	{
		if (code == 0)
		{
			m_labelTextPlayIndex++;
		}
	}

	/*public string GetNoticeServerInfo(string _url)
	{
		string empty = string.Empty;
		WWW wWW = NGUITools.OpenURL(_url);
		return wWW.text;
	}*/
}

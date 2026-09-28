using System;
using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class GameBattle : MonoBehaviour
{
	public enum PreLoadObjectBufferType
	{
		NinjiaKillSpawnBullet = 0,
		EnemySpecAttrLeakage = 1,
		EnemySpecAttrFireBall = 2,
		EnemySpecAttrIceBall = 3,
		Gold = 4
	}

	public enum State
	{
		Ready = 0,
		Game = 1,
		Pause = 2,
		Win = 3,
		Failed = 4,
		Retreat = 5,
		DialogStart = 6,
		DialogEnd = 7
	}

	private enum CameraState
	{
		Normal = 0,
		AdjustWideAngle = 1,
		Zoom = 2,
		Quake = 3,
		Move = 4,
		MoveAndZoom = 5
	}

	private enum CameraZoomState
	{
		Normal = 0,
		ZoomIn = 1,
		ZoomOut = 2
	}

	public static GameBattle m_instance;

	private GameObject mainCamera;

	private GameObject mainCameraModel;

	private Transform m_cameraPoint;

	private Rect m_worldRect;

	private Animator m_cameraQuakeControl;

	private Player m_player;

	private List<Player> m_teammateList;

	private List<DS2ActiveObject> m_cliqueComputerList;

	private List<DS2ActiveObject> m_cliquePlayerList;

	private List<DS2Object> m_interactObjectList;

	private List<DS2ActiveObject> m_betrayList;

	private List<DS2ActiveObject> m_cliquePlayerAliveList;

	private List<DS2ActiveObject> m_cliquePlayerDeadList;

	private List<DS2ActiveObject> m_cliqueComputerDeadList;

	private List<Player> m_playerBetrayList;

	private int m_iCliqueComputerObjCount;

	private int m_iCliquePlayerObjCount;

	private SquadController m_squadController;

	public static List<EnemySpawnPointMain.EnemyWaitSpawnInfo> s_enemyWaitToVisibleList = new List<EnemySpawnPointMain.EnemyWaitSpawnInfo>();

	public static List<DS2ActiveObject> s_objectWaitToTeleport = new List<DS2ActiveObject>();

	private Dictionary<int, NodeGroup> m_nodeGroupMap;

	private NodeGroup m_nearestNodeGroup;

	private List<DS2ActiveObject> m_cliquePlayerNeedDelete;

	private List<DS2ActiveObject> m_cliqueComputerNeedDelete;

	private List<DS2Object> m_interactObjectNeedDelete;

	private Transform enemySpawnPoint;

	public static int s_enemyCount = 0;

	public static int s_enemyLimitCount = 0;

	public static int s_storyPoint = 0;

	public static int s_envetProcessingCount = 0;

	public bool bMainPointInProcess;

	private int m_killEnemieCount;

	public static bool s_bInputLocked = false;

	[HideInInspector]
	public bool m_bFadeIn = true;

	[HideInInspector]
	public bool bGameStateToWin;

	private float m_gameStateToWinTimer;

	public int getMoneyInBattle;

	private State m_gameState;

	private float m_cameraViewDisY = 10f;

	private float m_cameraViewDisZ = 10f;

	private float m_lastCameraViewDisY;

	private float m_lastCameraViewDisZ;

	private DS2Object m_cameraFocusOn;

	private GameObject m_UIButtonPause;

	private GameObject m_UIScreenFailed;

	private GameObject m_UIScreenPause;

	private GameObject m_UIScreenWin;

	[HideInInspector]
	public GameObject m_UITargetPanel;

	[HideInInspector]
	public GameObject m_UIGamePanel;

	[HideInInspector]
	public GameObject m_UISkill;

	[HideInInspector]
	public GameObject m_UIDialog;

	private UIImageButton m_UIDialogButtonNext;

	[HideInInspector]
	public GameObject m_UIScreenReady;

	[HideInInspector]
	public GameObject m_UIChangeMode;

	public DS2ActiveObject UIVisableTarget;

	public static float s_timeScale = 1f;

	private bool m_bPause;

	public static int s_debugCameraMode = 2;

	public float combatAffectPercentDamage;

	public bool bGetBattleResultData;

	public float changeModeLimitTime;

	private float m_skillCommonalityTotalCDTime = 1f;

	private float m_skillCommonalityCDTime;

	private GameObject m_tmpMcGO;

	private Transform m_tmpMC;

	private UnityEngine.AI.NavMeshAgent m_tmpAgentMC;

	private float m_camera_adjustX;

	private float m_camera_adjustZ;

	private float m_camera_offesX;

	private float m_camera_offesZ;

	private CameraState m_cameraState;

	private bool m_cameraLocked;

	private float m_camera_zoomOffes;

	private float m_camera_zoomTargetOffes;

	private float m_camera_zoomSpeed;

	private float m_camera_zoomTimeScale;

	private float m_camera_zoomTimeToOut;

	private float m_camera_zoomTimer;

	private CameraZoomState m_camera_zoomState;

	private float m_camera_quake_range;

	private float m_camera_quake_frequence;

	private float m_cameara_quake_tiemr;

	private float m_cameara_quake_tiem;

	private int m_camera_quake_mark;

	private float m_camera_quake_offes;

	private float m_camera_move_speed;

	private Vector3 m_camera_move_dir;

	private DS2Object m_camera_move_focus;

	private float m_camera_move_time;

	private float m_camera_move_timer;

	private Transform m_targetSpawnTransform { get; set; }

	public float SkillCommonnalityTotalCDTime
	{
		get
		{
			return m_skillCommonalityTotalCDTime;
		}
		set
		{
			m_skillCommonalityTotalCDTime = value;
		}
	}

	public float SkillCommonnalityCDTime
	{
		get
		{
			return m_skillCommonalityCDTime;
		}
		set
		{
			m_skillCommonalityCDTime = value;
		}
	}

	public float CameraViewDisY
	{
		get
		{
			return m_cameraViewDisY;
		}
		set
		{
			m_lastCameraViewDisY = m_cameraViewDisY;
			m_cameraViewDisY = value;
		}
	}

	public float CameraViewDisZ
	{
		get
		{
			return m_cameraViewDisZ;
		}
		set
		{
			m_lastCameraViewDisZ = m_cameraViewDisZ;
			m_cameraViewDisZ = value;
		}
	}

	public static bool IsPauseBlockedByTutorial()
	{
		if (!DataCenter.Save().BattleTutorialFinished) return true;
		if (Tutorial.Instance != null && Tutorial.Instance.TutorialInProgress) return true;
		return false;
	}

	public State GameState
	{
		get
		{
			return m_gameState;
		}
		set
		{
			if (value == State.Pause && IsPauseBlockedByTutorial())
			{
				return;
			}
			if (value == State.Win)
			{
				if (DataCenter.Save().BattleTutorialFinished)
				{
					if (DataCenter.Save().GetLevelStars(DataCenter.State().selectWorldNode, DataCenter.State().selectLevelMode, DataCenter.State().selectAreaNode) <= 0 && DataCenter.Conf().GetCurrentGameLevelData().dialogEnd != null && DataCenter.Conf().GetCurrentGameLevelData().dialogEnd.Count > 0)
					{
						if (GameState != State.DialogEnd)
						{
							DataCenter.State().battleTime = Time.time - DataCenter.State().battleTime;
							GameState = State.DialogEnd;
							return;
						}
					}
					else
					{
						DataCenter.State().battleTime = Time.time - DataCenter.State().battleTime;
					}
				}
				else
				{
					DataCenter.State().battleTime = Time.time - DataCenter.State().battleTime;
				}
			}
			m_gameState = value;
			switch (m_gameState)
			{
			case State.Game:
				IsPause = false;
				DataCenter.State().battleStars = 0;
				if (m_UIButtonPause != null)
				{
					m_UIButtonPause.SetActive(!IsPauseBlockedByTutorial());
				}
				if (m_UIGamePanel != null)
				{
					m_UIGamePanel.SetActive(true);
				}
				break;
			case State.Failed:
			{
				if (m_UIGamePanel != null)
				{
					m_UIGamePanel.SetActive(false);
				}
				if (m_UIButtonPause != null)
				{
					m_UIButtonPause.SetActive(false);
				}
				if (m_UIScreenFailed != null)
				{
					m_UIScreenFailed.SetActive(true);
				}
				int reviveItem = DataCenter.Save().ReviveItem;
				DataCenter.State().battleResult = Defined.BattleResult.Failed;
				if (DataCenter.Save().BattleTutorialFinished)
				{
					if (DataCenter.State().isPVPMode)
					{
						HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.PVP_BATTLE_END, null);
					}
					else
					{
						BattleResultController.Get();
						HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.BattleResult, null);
					}
				}
				BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.BGM_Failure);
				break;
			}
			case State.Pause:
				if (m_UIGamePanel != null)
				{
					m_UIGamePanel.SetActive(false);
				}
				if (m_UIButtonPause != null)
				{
					m_UIButtonPause.SetActive(false);
				}
				if (m_UIScreenPause != null)
				{
					m_UIScreenPause.SetActive(true);
				}
				if (!IsPause)
				{
					IsPause = true;
				}
				break;
			case State.Win:
			{
				DataCenter.State().battleResult = Defined.BattleResult.Win;
				int hp = GetPlayer().hp;
				int hpMax = GetPlayer().hpMax;
				bool flag = (float)hp > (float)hpMax * 0.5f && DataCenter.State().battleStars != 1;
				int num = 4;
				if (DataCenter.State().selectLevelMode == Defined.LevelMode.Hard)
				{
					num = 5;
				}
				else if (DataCenter.State().selectLevelMode == Defined.LevelMode.Hell)
				{
					num = 6;
				}
				if ((DataCenter.State().battleTime < (float)(num * 60 + 30) && flag) || !DataCenter.Save().BattleTutorialFinished)
				{
					DataCenter.State().battleStars = 3;
				}
				else if (flag)
				{
					DataCenter.State().battleStars = 2;
				}
				else
				{
					DataCenter.State().battleStars = 1;
				}
				for (int i = 0; i < m_cliquePlayerList.Count; i++)
				{
					if (m_cliquePlayerList[i].Alive())
					{
						m_cliquePlayerList[i].SwitchFSM(m_cliquePlayerList[i].GetAIState("Idle"));
						m_cliquePlayerList[i].ResetColor();
						if (m_cliquePlayerList[i].objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || m_cliquePlayerList[i].objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
						{
							Character character = (Character)m_cliquePlayerList[i];
							character.SetMove(false, character.MoveDirection);
						}
					}
				}
				if (m_UIGamePanel != null)
				{
					m_UIGamePanel.SetActive(false);
				}
				if (m_UIButtonPause != null)
				{
					m_UIButtonPause.SetActive(false);
				}
				if (m_UIScreenWin != null)
				{
					if (DataCenter.State().isPVPMode)
					{
						m_UIScreenWin.SetActive(true);
					}
					else
					{
						BattleUIEvent component2 = m_UIScreenWin.GetComponent<BattleUIEvent>();
						component2.EnableScreenGameWin();
					}
				}
				if (DataCenter.State().selectWorldNode == 0 && DataCenter.State().selectAreaNode == 1)
				{
					ushort levelStars = DataCenter.Save().GetLevelStars(0, Defined.LevelMode.Normal, 1);
					if (levelStars <= 0)
					{
						UIUtil.ShowReviewMessageBox();
					}
				}
				if (!DataCenter.Save().BattleTutorialFinished && DataCenter.Save().selectLevelDropData != null)
				{
					DataCenter.Save().selectLevelDropData.money = 0;
					DataCenter.Save().selectLevelDropData.exp = 0;
					DataCenter.Save().selectLevelDropData.extraMoney = 0;
					DataCenter.Save().selectLevelDropData.extraCrystal = 0;
					DataCenter.Save().selectLevelDropData.extraHonor = 0;
				}
				if (DataCenter.Save().BattleTutorialFinished)
				{
					DataCenter.Save().SetGameProgress();
					if (DataCenter.State().isPVPMode)
					{
						HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.PVP_BATTLE_END, null);
					}
					else
					{
						BattleResultController.Get();
						HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.BattleResult, null);
					}
				}
				else
				{
					bGetBattleResultData = true;
				}
				BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.BGM_Victory);
				break;
			}
			case State.Ready:
				if (m_UIGamePanel != null)
				{
					m_UIGamePanel.SetActive(false);
				}
				m_UIScreenReady.SetActive(true);
				if (!DataCenter.State().isPVPMode)
				{
				}
				break;
			case State.DialogStart:
			case State.DialogEnd:
			{
				if (m_UIGamePanel != null)
				{
					m_UIGamePanel.SetActive(false);
				}
				m_UIScreenReady.SetActive(false);
				m_UIDialog.SetActive(true);
				m_UIDialogButtonNext.gameObject.SetActive(true);
				BattleUIEvent component = m_UIDialogButtonNext.gameObject.GetComponent<BattleUIEvent>();
				component.DialogInit();
				break;
			}
			case State.Retreat:
				break;
			}
		}
	}

	public bool IsInBattle
	{
		get
		{
			return s_enemyCount > 0;
		}
	}

	public DS2Object CameraFocus
	{
		get
		{
			return m_cameraFocusOn;
		}
		set
		{
			m_cameraFocusOn = value;
			TAudioManager.instance.AudioListener.transform.parent = m_cameraFocusOn.GetTransform();
		}
	}

	public int KillEnemiesCount
	{
		get
		{
			return Util.DecryptInt(m_killEnemieCount);
		}
		set
		{
			m_killEnemieCount = Util.EncryptInt(value);
		}
	}

	public bool IsPause
	{
		get
		{
			return m_bPause;
		}
		set
		{
			m_bPause = value;
			if (m_bPause)
			{
				if (Time.timeScale > 0f)
				{
					s_timeScale = Time.timeScale;
				}
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = s_timeScale;
			}
		}
	}

	public void Awake()
	{
		m_instance = this;
		m_cliqueComputerList = new List<DS2ActiveObject>();
		m_cliquePlayerList = new List<DS2ActiveObject>();
		m_interactObjectList = new List<DS2Object>();
		m_cliqueComputerList.Clear();
		m_cliquePlayerList.Clear();
		m_interactObjectList.Clear();
		m_iCliqueComputerObjCount = 0;
		m_iCliquePlayerObjCount = 0;
		m_betrayList = new List<DS2ActiveObject>();
		m_betrayList.Clear();
		m_playerBetrayList = new List<Player>();
		m_playerBetrayList.Clear();
		m_cliquePlayerNeedDelete = new List<DS2ActiveObject>();
		m_cliqueComputerNeedDelete = new List<DS2ActiveObject>();
		m_interactObjectNeedDelete = new List<DS2Object>();
		m_cliquePlayerNeedDelete.Clear();
		m_cliqueComputerNeedDelete.Clear();
		m_interactObjectNeedDelete.Clear();
		m_cliquePlayerAliveList = new List<DS2ActiveObject>();
		m_cliquePlayerAliveList.Clear();
		m_cliquePlayerDeadList = new List<DS2ActiveObject>();
		m_cliquePlayerDeadList.Clear();
		m_cliqueComputerDeadList = new List<DS2ActiveObject>();
		m_cliqueComputerDeadList.Clear();
		m_teammateList = new List<Player>();
		m_teammateList.Clear();
		m_nodeGroupMap = new Dictionary<int, NodeGroup>();
		m_nodeGroupMap.Clear();
		DataCenter.Conf().LoadEnemySpawnInfoFromDisk();
	}

	public void Start()
	{
		UIUtil.HideOpenClik();
		if (DataCenter.Save().bNewUser && !Util.s_debug)
		{
			DataCenter.Save().CreateTeamForTutorial();
		}
		Initialze();
		ChangeCameraState(CameraState.AdjustWideAngle);
		m_UIScreenPause = UIControlManager.Instance.GetControl(100);
		if (DataCenter.State().isPVPMode)
		{
			m_UIScreenWin = UIControlManager.Instance.GetControl(331);
			m_UIScreenFailed = UIControlManager.Instance.GetControl(332);
			GameObject gameObject = GameObject.Find("GUI_PVP");
			FadeInfoScript.Instance.transform.position = gameObject.transform.position;
		}
		else
		{
			GameObject gameObject2 = GameObject.Find("GUI");
			gameObject2.transform.position = Vector3.zero;
			FadeInfoScript.Instance.transform.position = gameObject2.transform.position;
			m_UIScreenWin = UIControlManager.Instance.GetControl(115);
			m_UIScreenFailed = UIControlManager.Instance.GetControl(130);
			m_UISkill = UIControlManager.Instance.GetControl(0);
			m_UITargetPanel = UIControlManager.Instance.GetControl(21);
			m_UIButtonPause = UIControlManager.Instance.GetControl(160);
			m_UIDialog = UIControlManager.Instance.GetControl(450);
			m_UIDialogButtonNext = UIControlManager.Instance.GetControl(451).GetComponent<UIImageButton>();
			m_UIScreenReady = UIControlManager.Instance.GetControl(201);
			m_UIDialog.SetActive(false);
			m_UIChangeMode = UIControlManager.Instance.GetControl(453);
		}
		m_UIGamePanel = UIControlManager.Instance.GetControl(200);
		DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
		if (currentGameLevelData.isBossLevel)
		{
			BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.BGM_BOSS);
		}
		else
		{
			BackgroundMusicManager.Instance().PlayBackgroundMusic(currentGameLevelData.sBGM);
		}
		DataCenter.Save().ResetGameProgress();
		DataCenter.State().lastSceneType = Defined.SceneType.Battle;
		if (Util.s_debug)
		{
			//DataCenter.Save().BattleTutorialFinished = true;
			GameState = State.Ready;
		}
		if (!DataCenter.Save().BattleTutorialFinished)
		{
			m_UIChangeMode.SetActive(false);
			Tutorial.Instance.TutorialPahseMove = Tutorial.TutorialPhaseState.InProgress;
			GameState = State.Ready;
			return;
		}
		if (DataCenter.Save().tutorialChangeMode)
		{
			Tutorial.Instance.gameObject.SetActive(false);
		}
		else
		{
			m_UIChangeMode.SetActive(false);
		}
		if (DataCenter.Save().GetLevelStars(DataCenter.State().selectWorldNode, DataCenter.State().selectLevelMode, DataCenter.State().selectAreaNode) <= 0 && DataCenter.Conf().GetCurrentGameLevelData().dialogStart != null && DataCenter.Conf().GetCurrentGameLevelData().dialogStart.Count > 0)
		{
			GameState = State.DialogStart;
		}
		else
		{
			GameState = State.Ready;
		}
	}

	private void LoadBuilding()
	{
	}

	public void Update()
	{
		if (m_bFadeIn)
		{
			m_bFadeIn = false;
			FadeInfoScript.Instance.FadeIn();
		}
		if (m_bPause)
		{
			return;
		}
		if (bGameStateToWin)
		{
			m_gameStateToWinTimer += Time.deltaTime;
			if (m_gameStateToWinTimer >= 3f)
			{
				bGameStateToWin = false;
				GameState = State.Win;
			}
		}
		if (GameState == State.Game)
		{
			UpdateScene(Time.deltaTime);
		}
	}

	public void OnDestroy()
	{
		Dispose();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
		{
			PauseWhenLeavingGame();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			PauseWhenLeavingGame();
		}
	}

	private void PauseWhenLeavingGame()
	{
		if (Application.isEditor)
		{
			return;
		}
		if (GameState == State.Game)
		{
			GameState = State.Pause;
		}
	}

	public void Initialze()
	{
		s_enemyCount = 0;
		s_enemyLimitCount = 0;
		s_storyPoint = 0;
		s_envetProcessingCount = 0;
		s_bInputLocked = false;
		s_enemyWaitToVisibleList.Clear();
		s_objectWaitToTeleport.Clear();
		s_timeScale = 1f;
		s_debugCameraMode = 2;
		bGameStateToWin = false;
		m_gameStateToWinTimer = 0f;
		getMoneyInBattle = 0;
		KillEnemiesCount = 0;
		if ((float)DataCenter.Save().GetTeamData().GetTeamCombat() < (float)DataCenter.Save().selectLevelDropData.recommendCombat * 0.5f)
		{
			combatAffectPercentDamage = 0.75f;
		}
		else if ((float)DataCenter.Save().GetTeamData().GetTeamCombat() >= (float)DataCenter.Save().selectLevelDropData.recommendCombat * 0.5f && (float)DataCenter.Save().GetTeamData().GetTeamCombat() < (float)DataCenter.Save().selectLevelDropData.recommendCombat * 0.75f)
		{
			combatAffectPercentDamage = 0.5f;
		}
		else if ((float)DataCenter.Save().GetTeamData().GetTeamCombat() >= (float)DataCenter.Save().selectLevelDropData.recommendCombat * 0.75f && (float)DataCenter.Save().GetTeamData().GetTeamCombat() < (float)DataCenter.Save().selectLevelDropData.recommendCombat * 1f)
		{
			combatAffectPercentDamage = 0.25f;
		}
		else
		{
			combatAffectPercentDamage = 0f;
		}
		int num = 0;
		for (int i = 0; i < DataCenter.Save().GetTeamData().teamSitesData.Length; i++)
		{
			if (DataCenter.Save().GetTeamData().teamSitesData[i].playerData != null)
			{
				num++;
			}
		}
		if (Util.s_debug)
		{
//			DataCenter.Save().bNewUser = false;
		}
		if (DataCenter.Save().squadMode && num <= 1)
		{
			DataCenter.Save().squadMode = false;
		}
		else
		{
			DataCenter.Save().squadMode = true;
		}
		if (!DataCenter.State().isPVPMode)
		{
			DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
			DataCenter.State().isEncounterLevel = currentGameLevelData.isEncounter;
			if (currentGameLevelData.isEncounter)
			{
				DataCenter.Conf().LoadEncounterEnemyTeam(((DataConf.GameLevelEncounterModeData)currentGameLevelData).enenyTeamID);
				DataCenter.State().isPVPMode = true;
			}
		}
		else
		{
			DataCenter.State().isEncounterLevel = false;
		}
		SpecialAttribute.Init();
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Models/Scenes/Ground") as GameObject) as GameObject;
		gameObject.name = "Ground";
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.identity;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
		gameObject2.transform.position = new Vector3(gameObject2.transform.position.x, 0.195f, gameObject2.transform.position.z);
		Character.s_spawnTransform = gameObject2.transform;
		if (DataCenter.State().isPVPMode)
		{
			GameObject gameObject3 = GameObject.Find("TargetSpawnPoint");
			m_targetSpawnTransform = gameObject3.transform;
			gameObject3.SetActive(false);
		}
		gameObject2.SetActive(false);
		mainCamera = GameObject.Find("Main Camera");
		mainCameraModel = mainCamera.transform.Find("Model").gameObject;
		m_cameraQuakeControl = mainCameraModel.GetComponent<Animator>();
		m_cameraQuakeControl.enabled = false;
		m_cameraPoint = mainCameraModel.transform.Find("CameraPoint");
		Material material = Util.LoadEquipIconMaterial("armor001");
		SetCameraView(DataCenter.Save().CameraView);
		m_squadController = new SquadController();
		GameObject prefab = Resources.Load<GameObject>("Game/SquadController");
		m_squadController.Initialize(prefab, "SquadController", Character.s_spawnTransform.position, Quaternion.identity, 0);
		m_squadController.clique = DS2ActiveObject.Clique.Player;
		CreatePlayerData();
		GetSquadController().UpdatePointsPosition();
		if (Player.s_allySeat == null)
		{
			Player.s_allySeat = new AllySeat();
			Player.s_allySeat.Init(GetPlayerObjCount());
		}
		int num2 = 0;
		foreach (DS2ActiveObject cliquePlayer in m_cliquePlayerList)
		{
			if (cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player2 = (Player)cliquePlayer;
				if (DataCenter.Save().squadMode)
				{
					player2.CurrentController = false;
				}
				else if (num2 == 0)
				{
					player2.CurrentController = true;
				}
				else
				{
					player2.CurrentController = false;
				}
				num2++;
			}
		}
		if (DataCenter.Save().squadMode)
		{
			CameraFocus = m_squadController;
		}
		else
		{
			CameraFocus = m_player;
		}
		if (DataCenter.State().isPVPMode)
		{
			if (!Util.s_pvp_atutoPlay)
			{
				mainCamera.transform.position = new Vector3(m_player.GetTransform().position.x, m_player.GetTransform().position.y + 17f, m_player.GetTransform().position.z - 12.7f);
			}
		}
		else
		{
			mainCamera.transform.position = new Vector3(CameraFocus.GetTransform().position.x, CameraFocus.GetTransform().position.y + CameraViewDisY, CameraFocus.GetTransform().position.z - CameraViewDisZ);
		}
		SetSquadMode(DataCenter.Save().squadMode);
		ChangeCameraState(CameraState.AdjustWideAngle, true);
		if (!Util.s_pvp_atutoPlay)
		{
		}
		s_bInputLocked = false;
		CreateNodeGroup();
		if (DataCenter.Save().m_bRefreshEnemyByTime)
		{
			GameObject gameObject4 = GameObject.Find("EnemySpawnPointGeneral");
			EnemySpawnPointByTime enemySpawnPointByTime = gameObject4.GetComponent<EnemySpawnPointByTime>();
			if (enemySpawnPointByTime == null)
			{
				enemySpawnPointByTime = gameObject4.AddComponent<EnemySpawnPointByTime>();
			}
			enemySpawnPointByTime.triggerDistance = 2.5f;
			enemySpawnPointByTime.type = DataCenter.Save().m_ifreshEnemyByTime_EnemyType;
			enemySpawnPointByTime.frequency = DataCenter.Save().m_ffreshEnemyByTime_TimeRate;
			enemySpawnPointByTime.spawnOneTime = (int)DataCenter.Save().m_ffreshEnemyByTime_EnemyCount;
		}
	}

	public void Restart()
	{
		m_player.Restart();
	}

	public void UpdateScene(float deltaTime)
	{
		if (GameState == State.Game)
		{
			if (!Util.s_pvp_atutoPlay)
			{
				UpdateCamera();
			}
			if (DataCenter.Save().squadMode && m_squadController != null)
			{
				m_squadController.Update(Time.deltaTime);
			}
			TAudioManager.instance.AudioListener.transform.localPosition = Vector3.zero;
			TAudioManager.instance.AudioListener.transform.localRotation = Quaternion.identity;
			TAudioManager.instance.AudioListener.transform.localScale = Vector3.one;
			UpdateGameObject(deltaTime);
			DeleteObjectsFromList();
			if (!DataCenter.State().isPVPMode)
			{
				UpdateNodeGroup();
			}
			UpdateInput();
			if (m_skillCommonalityCDTime > 0f)
			{
				m_skillCommonalityCDTime -= deltaTime;
				m_skillCommonalityCDTime = Mathf.Max(0f, m_skillCommonalityCDTime);
			}
			if (!DataCenter.Save().tutorialChangeMode && !DataCenter.Save().squadMode && !s_bInputLocked && Tutorial.Instance.TutorialPahseChangeMode == Tutorial.TutorialPhaseState.Done && Tutorial.Instance.TutorialPahseChangePlayer == Tutorial.TutorialPhaseState.None && Tutorial.Instance.NextStep)
			{
				Tutorial.Instance.TutorialPahseChangePlayer = Tutorial.TutorialPhaseState.InProgress;
			}
		}
	}

	private void UpdateCamera()
	{
		switch (m_cameraState)
		{
		case CameraState.Normal:
			break;
		case CameraState.AdjustWideAngle:
			updateCameraAdjustWideAngle();
			break;
		case CameraState.Zoom:
			updateCameraAdjustWideAngle();
			UpdateCameraZoom();
			break;
		case CameraState.Quake:
			updateCameraAdjustWideAngle();
			UpdateCameraQuake();
			break;
		case CameraState.Move:
			UpdateCameraMove();
			break;
		case CameraState.MoveAndZoom:
			UpdateCameraMoveAndZoom();
			break;
		}
	}

	private bool ChangeCameraState(CameraState state, bool forceSwith = false)
	{
		if (forceSwith)
		{
			m_cameraLocked = false;
		}
		if (m_cameraLocked)
		{
			return false;
		}
		m_cameraState = state;
		return true;
	}

	private void updateCameraAdjustWideAngle()
	{
		float num = CameraViewDisY;
		float num2 = CameraViewDisZ;
		if (DataCenter.State().isPVPMode && !Util.s_pvp_atutoPlay)
		{
			num = 17f;
			num2 = 12.7f;
		}
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		Vector3 position = mainCamera.transform.position;
		if (CameraFocus.GetTransform().position.x < mainCamera.transform.position.x - m_camera_quake_offes && flag)
		{
			position.x = CameraFocus.GetTransform().position.x - m_camera_quake_offes;
		}
		if (CameraFocus.GetTransform().position.x > mainCamera.transform.position.x - m_camera_quake_offes && flag3)
		{
			position.x = CameraFocus.GetTransform().position.x - m_camera_quake_offes;
		}
		if (CameraFocus.GetTransform().position.z - num2 - m_camera_zoomOffes > mainCamera.transform.position.z && flag4)
		{
			position.z = CameraFocus.GetTransform().position.z - num2 - m_camera_zoomOffes;
		}
		if (CameraFocus.GetTransform().position.z - num2 - m_camera_zoomOffes < mainCamera.transform.position.z && flag2)
		{
			position.z = CameraFocus.GetTransform().position.z - num2 - m_camera_zoomOffes;
		}
		position.y = CameraFocus.GetTransform().position.y + num + m_camera_zoomOffes + m_camera_quake_offes;
		mainCamera.transform.position = position;
		if (flag5)
		{
		}
		if (s_debugCameraMode != 2)
		{
			float num3 = 0f;
			float num4 = 0f;
			num3 = m_player.FaceDirection.x * 2f;
			num4 = m_player.FaceDirection.z * 2f;
			if (s_debugCameraMode == 0 && num4 > 0f)
			{
				num4 = ((!GetPlayer().Alive()) ? 0f : 0.5f);
			}
			m_camera_adjustX = Mathf.Lerp(m_camera_adjustX, num3, 0.05f);
			m_camera_adjustZ = Mathf.Lerp(m_camera_adjustZ, num4, 0.05f);
			mainCamera.transform.Translate(m_camera_adjustX, 0f, m_camera_adjustZ);
		}
	}

	private void UpdateCameraZoom()
	{
		if (m_camera_zoomState == CameraZoomState.ZoomIn)
		{
			if (Mathf.Abs(m_camera_zoomTargetOffes - m_camera_zoomOffes) > Mathf.Abs(m_camera_zoomSpeed))
			{
				m_camera_zoomOffes += m_camera_zoomSpeed;
				return;
			}
			Time.timeScale = m_camera_zoomTimeScale;
			m_camera_zoomState = CameraZoomState.Normal;
		}
		else if (m_camera_zoomState == CameraZoomState.ZoomOut)
		{
			if (Mathf.Abs(m_camera_zoomTargetOffes - m_camera_zoomOffes) > Mathf.Abs(m_camera_zoomSpeed))
			{
				m_camera_zoomOffes -= m_camera_zoomSpeed;
				return;
			}
			m_camera_zoomState = CameraZoomState.Normal;
			m_cameraLocked = false;
			ChangeCameraState(CameraState.AdjustWideAngle);
		}
		else if (m_camera_zoomState == CameraZoomState.Normal && m_camera_zoomTimeToOut > 0f && Time.realtimeSinceStartup - m_camera_zoomTimer >= m_camera_zoomTimeToOut)
		{
			Time.timeScale = 1f;
			SetCameraZoomOut();
		}
	}

	public void SetCameraZoomIn(float time, float targetOffes, float timeScale = 1f, float timeToOut = 0f)
	{
		ChangeCameraState(CameraState.Zoom);
		m_cameraLocked = true;
		m_camera_zoomState = CameraZoomState.ZoomIn;
		m_camera_zoomTargetOffes = targetOffes;
		m_camera_zoomSpeed = (targetOffes - m_camera_zoomOffes) / time * Time.deltaTime;
		m_camera_zoomTimeScale = timeScale;
		m_camera_zoomTimeToOut = timeToOut;
		m_camera_zoomTimer = Time.realtimeSinceStartup;
	}

	public void SetCameraZoomOut(float time = 0.3f)
	{
		m_camera_zoomState = CameraZoomState.ZoomOut;
		m_camera_zoomTargetOffes = 0f;
		m_camera_zoomTimeScale = 1f;
		m_camera_zoomSpeed = m_camera_zoomOffes / time * Time.deltaTime;
		Time.timeScale = m_camera_zoomTimeScale;
	}

	private void UpdateCameraQuake()
	{
		if (m_cameraQuakeControl.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			m_cameraQuakeControl.enabled = false;
			mainCameraModel.transform.localPosition = Vector3.zero;
			ChangeCameraState(CameraState.AdjustWideAngle, true);
		}
	}

	public void SetCameraQuake(float time, float range = 0.5f)
	{
	}

	public void SetCameraQuake(Defined.CameraQuakeType quakeId)
	{
		ChangeCameraState(CameraState.Quake);
		if (m_cameraState == CameraState.Quake)
		{
			m_cameraQuakeControl.enabled = true;
			m_cameraQuakeControl.SetInteger("quakeId", (int)quakeId);
			m_cameraQuakeControl.SetTrigger("quake");
		}
	}

	public void SetCameraMove(Vector3 targetPos, float speed, DS2Object lookAt = null, bool forceSwith = false)
	{
		if (!(mainCamera == null))
		{
			s_bInputLocked = true;
			ChangeCameraState(CameraState.Move, forceSwith);
			m_cameraLocked = true;
			m_camera_move_speed = speed;
			m_camera_move_timer = 0f;
			m_camera_move_focus = lookAt;
			float cameraViewDisY = CameraViewDisY;
			float cameraViewDisZ = CameraViewDisZ;
			if (DataCenter.State().isPVPMode && !Util.s_pvp_atutoPlay)
			{
				cameraViewDisY = 17f;
				cameraViewDisZ = 12.7f;
			}
			m_camera_move_dir = targetPos - mainCamera.transform.position;
			float magnitude = m_camera_move_dir.magnitude;
			m_camera_move_time = magnitude / m_camera_move_speed;
		}
	}

	private void UpdateCameraMove()
	{
		float cameraViewDisY = CameraViewDisY;
		float cameraViewDisZ = CameraViewDisZ;
		m_camera_move_timer += Time.deltaTime;
		if (m_camera_move_timer >= m_camera_move_time)
		{
			mainCamera.transform.position = new Vector3(CameraFocus.GetTransform().position.x, CameraFocus.GetTransform().position.y + cameraViewDisY, CameraFocus.GetTransform().position.z - cameraViewDisZ);
			if (m_camera_move_focus != null)
			{
			}
			ChangeCameraState(CameraState.AdjustWideAngle);
			s_bInputLocked = false;
			m_cameraLocked = false;
			m_camera_adjustX = 0f;
			m_camera_adjustZ = 0f;
			m_camera_offesX = 0f;
			m_camera_offesZ = 0f;
		}
		else
		{
			mainCamera.transform.Translate(m_camera_move_dir.normalized * m_camera_move_speed * Time.deltaTime, Space.World);
			if (m_camera_move_focus == null)
			{
			}
		}
	}

	public void SetCameraMoveAndZoomIn(DS2Object target, float moveSpeed, float targetOffes, float timeScale = 1f, float timeToOut = 0f)
	{
		if (!(mainCamera == null))
		{
			ChangeCameraState(CameraState.MoveAndZoom);
			s_bInputLocked = true;
			m_cameraLocked = true;
			m_camera_move_focus = target;
			m_camera_move_speed = 10f;
			CameraFocus = target;
			m_camera_zoomOffes = 0f;
			m_camera_zoomState = CameraZoomState.ZoomIn;
			m_camera_zoomTargetOffes = targetOffes;
			m_camera_zoomSpeed = (targetOffes - m_camera_zoomOffes) / 0.5f * Time.deltaTime;
			m_camera_zoomTimeScale = timeScale;
			m_camera_zoomTimeToOut = timeToOut;
			m_camera_zoomTimer = Time.realtimeSinceStartup;
		}
	}

	public void UpdateCameraMoveAndZoom()
	{
		float num = CameraViewDisY;
		float num2 = CameraViewDisZ;
		if (DataCenter.State().isPVPMode && !Util.s_pvp_atutoPlay)
		{
			num = 17f;
			num2 = 12.7f;
		}
		Vector3 vector = new Vector3(m_camera_move_focus.GetTransform().position.x, m_camera_move_focus.GetTransform().position.y + num + m_camera_zoomTargetOffes, m_camera_move_focus.GetTransform().position.z - num2 - m_camera_zoomTargetOffes * (num2 / num));
		m_camera_move_dir = vector - mainCamera.transform.position;
		float sqrMagnitude = m_camera_move_dir.sqrMagnitude;
		if (sqrMagnitude < 0.010000001f)
		{
			m_camera_adjustX = 0f;
			m_camera_adjustZ = 0f;
			m_camera_offesX = 0f;
			m_camera_offesZ = 0f;
			if (m_camera_zoomState == CameraZoomState.ZoomIn)
			{
				Time.timeScale = m_camera_zoomTimeScale;
				m_camera_zoomState = CameraZoomState.Normal;
			}
			else if (m_camera_zoomState == CameraZoomState.ZoomOut)
			{
				m_camera_zoomState = CameraZoomState.Normal;
				m_cameraLocked = false;
				s_bInputLocked = false;
				ChangeCameraState(CameraState.AdjustWideAngle);
			}
		}
		else
		{
			mainCamera.transform.Translate(m_camera_move_dir * m_camera_move_speed * Time.deltaTime, Space.World);
		}
		if (m_camera_zoomState != CameraZoomState.ZoomIn && m_camera_zoomState != CameraZoomState.ZoomOut && m_camera_zoomState == CameraZoomState.Normal && m_camera_zoomTimeToOut > 0f && Time.realtimeSinceStartup - m_camera_zoomTimer >= m_camera_zoomTimeToOut)
		{
			Time.timeScale = 1f;
			m_camera_zoomTargetOffes *= -1f;
			m_camera_zoomTargetOffes = 0f;
			m_camera_zoomState = CameraZoomState.ZoomOut;
		}
	}

	public GameObject GetCameraWrap()
	{
		return mainCamera;
	}

	public Camera GetCamera()
	{
		return mainCameraModel.GetComponent<Camera>();
	}

	public Transform GetCameraPoint()
	{
		return m_cameraPoint;
	}

	private void CreatePlayerData()
	{
		if (DataCenter.State().isPVPMode)
		{
			TeamData teamData = DataCenter.Save().GetTeamData();
			TeamSiteData[] teamSitesData = teamData.teamSitesData;
			for (int i = 0; i < teamSitesData.Length; i++)
			{
				PlayerData playerData = teamSitesData[i].playerData;
				if (playerData != null)
				{
					DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
					Vector3 position = new Vector3(Character.s_spawnTransform.position.x + (float)Defined.TEAMMATE_POSTION_OFFSET_PVP[i, 0], Character.s_spawnTransform.position.y, Character.s_spawnTransform.transform.position.z + (float)Defined.TEAMMATE_POSTION_OFFSET_PVP[i, 1]);
					Player player2 = CharacterBuilder.CreatePlayerByCharacterType(teamData, i, position, Character.s_spawnTransform.rotation, 9);
					if (Util.s_pvp_atutoPlay)
					{
						player2.GetModelTransform().forward = Character.s_spawnTransform.right;
					}
					else
					{
						player2.GetModelTransform().forward = Character.s_spawnTransform.forward;
					}
					player2.FaceDirection = player2.GetTransform().forward;
					Weapon weapon = WeaponBuilder.CreateWeaponPlayer(player2.getWeaponType(), playerData.weaponLevel);
					player2.AddWeapon(0, weapon);
					player2.UseWeapon(0);
					player2.UpdateAnimationSpeed();
					AddObjToPlayerList(player2);
					if (i == 0)
					{
						ChangeCurrentContorlPlayer(player2);
					}
					else
					{
						player2.AnimationPlay(player2.GetAnimationNameByWeapon("Idle"), true);
						AddToTeammateList(player2);
					}
					player2.SetHalo();
				}
			}
			TeamData team = DataCenter.User().PVP_SelectTarget.team;
			TeamSiteData[] teamSitesData2 = team.teamSitesData;
			for (int j = 0; j < teamSitesData2.Length; j++)
			{
				PlayerData playerData2 = teamSitesData2[j].playerData;
				if (playerData2 != null)
				{
					DataConf.HeroData heroDataByIndex2 = DataCenter.Conf().GetHeroDataByIndex(playerData2.heroIndex);
					Vector3 position2 = new Vector3(m_targetSpawnTransform.position.x + (float)Defined.TEAMMATE_POSTION_OFFSET_PVP[j, 0], m_targetSpawnTransform.position.y, m_targetSpawnTransform.position.z + (float)Defined.TEAMMATE_POSTION_OFFSET_PVP[j, 1]);
					Player player3 = CharacterBuilder.CreatePlayerByCharacterType(team, j, position2, m_targetSpawnTransform.rotation, 11);
					if (Util.s_pvp_atutoPlay)
					{
						player3.GetModelTransform().forward = m_targetSpawnTransform.right * -1f;
					}
					else
					{
						player3.GetModelTransform().forward = m_targetSpawnTransform.forward * -1f;
					}
					player3.FaceDirection = player3.GetModelTransform().forward;
					Weapon weapon2 = WeaponBuilder.CreateWeaponPlayer(player3.getWeaponType(), playerData2.weaponLevel);
					player3.AddWeapon(0, weapon2);
					player3.UseWeapon(0);
					player3.UpdateAnimationSpeed();
					AddObjToComputerList(player3);
				}
			}
			return;
		}
		TeamData teamData2 = DataCenter.Save().GetTeamData();
		TeamSiteData[] teamSitesData3 = teamData2.teamSitesData;
		int num = 0;
		float num2 = 9999f;
		for (int k = 0; k < teamSitesData3.Length; k++)
		{
			PlayerData playerData3 = teamSitesData3[k].playerData;
			if (playerData3 != null)
			{
				DataConf.HeroData heroDataByIndex3 = DataCenter.Conf().GetHeroDataByIndex(playerData3.heroIndex);
				Vector3 position3 = new Vector3(Character.s_spawnTransform.position.x + (float)Defined.TEAMMATE_POSTION_OFFSET[k, 0], Character.s_spawnTransform.position.y, Character.s_spawnTransform.transform.position.z + (float)Defined.TEAMMATE_POSTION_OFFSET[k, 1]);
				Player player4 = CharacterBuilder.CreatePlayerByCharacterType(teamData2, k, position3, Character.s_spawnTransform.rotation, 9);
				player4.FaceDirection = Character.s_spawnTransform.forward;
				player4.SquadSite = num;
				num++;
				Weapon weapon3 = WeaponBuilder.CreateWeaponPlayer(player4.getWeaponType(), playerData3.weaponLevel);
				player4.AddWeapon(0, weapon3);
				player4.UseWeapon(0);
				player4.UpdateAnimationSpeed();
				AddObjToPlayerList(player4);
				if (k == 0)
				{
					ChangeCurrentContorlPlayer(player4);
				}
				else
				{
					AddToTeammateList(player4);
				}
				player4.SetHalo();
				if (num2 >= player4.MoveSpeed)
				{
					num2 = player4.MoveSpeed;
				}
			}
		}
		m_squadController.OriginalSpeed = num2;
		foreach (DS2ActiveObject cliquePlayer in m_cliquePlayerList)
		{
			if (cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player5 = (Player)cliquePlayer;
				player5.MoveSpeed = (player5.baseAttribute.moveSpeed = num2);
			}
		}
	}

	private void CreateNodeGroup()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("NodeGroup");
		if (array == null || array.Length == 0)
		{
			return;
		}
		m_worldRect = new Rect(9999f, 9999f, -9999f, -9999f);
		for (int num = array.Length - 1; num >= 0; num--)
		{
			NodeGroup nodeGroup = new NodeGroup();
			Renderer component = array[num].GetComponent<Renderer>();
			nodeGroup.position = array[num].transform.position;
			nodeGroup.bounds = component.bounds;
			if (m_worldRect.xMin > nodeGroup.bounds.min.x)
			{
				m_worldRect.xMin = nodeGroup.bounds.min.x;
			}
			if (m_worldRect.yMin > nodeGroup.bounds.min.z)
			{
				m_worldRect.yMin = nodeGroup.bounds.min.z;
			}
			if (m_worldRect.xMax < nodeGroup.bounds.max.x)
			{
				m_worldRect.xMax = nodeGroup.bounds.max.x;
			}
			if (m_worldRect.yMax < nodeGroup.bounds.max.z)
			{
				m_worldRect.yMax = nodeGroup.bounds.max.z;
			}
			GameObject gameObject = array[num].transform.parent.Find("SpawnPointInNodeGroup").gameObject;
			EnemySpawnPointGeneral[] componentsInChildren = gameObject.GetComponentsInChildren<EnemySpawnPointGeneral>(true);
			for (int num2 = componentsInChildren.Length - 1; num2 >= 0; num2--)
			{
				EnemySpawnNode enemySpawnNode = new EnemySpawnNode();
				enemySpawnNode.position = componentsInChildren[num2].gameObject.transform.position;
				enemySpawnNode.type = componentsInChildren[num2].type;
				nodeGroup.AddNode(enemySpawnNode);
			}
			int instanceID = array[num].transform.parent.gameObject.GetInstanceID();
			if (!gameObject.activeInHierarchy)
			{
				nodeGroup.Enable = false;
			}
			if (!m_nodeGroupMap.ContainsKey(instanceID))
			{
				m_nodeGroupMap.Add(instanceID, nodeGroup);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private void UpdateNodeGroup()
	{
		if (m_nodeGroupMap == null || m_nodeGroupMap.Count <= 0)
		{
			return;
		}
		float num = float.MaxValue;
		foreach (NodeGroup value in m_nodeGroupMap.Values)
		{
			if (!value.Enable)
			{
				continue;
			}
			Vector3 position = GetPlayer().GetTransform().position;
			float sqrMagnitude = (position - value.position).sqrMagnitude;
			if (!(sqrMagnitude >= 1225f))
			{
				if (num > sqrMagnitude)
				{
					num = sqrMagnitude;
					m_nearestNodeGroup = value;
				}
				value.Update();
			}
		}
	}

	public NodeGroup GetNearestNodeGroup()
	{
		return m_nearestNodeGroup;
	}

	public void SetNodeGroupEnable(int groupID, bool enable)
	{
		if (m_nodeGroupMap.ContainsKey(groupID))
		{
			m_nodeGroupMap[groupID].Enable = enable;
		}
	}

	private void UpdateGameObject(float deltaTime)
	{
		for (int i = 0; i < m_cliquePlayerList.Count; i++)
		{
			m_cliquePlayerList[i].Update(deltaTime);
		}
		for (int j = 0; j < m_cliqueComputerList.Count; j++)
		{
			m_cliqueComputerList[j].Update(deltaTime);
		}
		for (int k = 0; k < m_interactObjectList.Count; k++)
		{
			m_interactObjectList[k].Update(deltaTime);
		}
		for (int l = 0; l < m_betrayList.Count; l++)
		{
			DS2ActiveObject dS2ActiveObject = m_betrayList[l];
			if (dS2ActiveObject.clique == DS2ActiveObject.Clique.Player)
			{
				dS2ActiveObject.Betray(DS2ActiveObject.Clique.Computer);
				AddToPlayerNeedDeleteList(dS2ActiveObject);
				AddObjToComputerList(dS2ActiveObject);
				if (dS2ActiveObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || dS2ActiveObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
				{
					AddPlayToBetrayList((Player)dS2ActiveObject);
				}
			}
			else if (dS2ActiveObject.clique == DS2ActiveObject.Clique.Computer)
			{
				dS2ActiveObject.Betray(DS2ActiveObject.Clique.Player);
				AddToComputerNeedDeleteList(dS2ActiveObject);
				AddObjToPlayerList(dS2ActiveObject);
				if ((dS2ActiveObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || dS2ActiveObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY) && m_playerBetrayList.Contains((Player)dS2ActiveObject))
				{
					m_playerBetrayList.Remove((Player)dS2ActiveObject);
				}
			}
		}
		m_betrayList.Clear();
	}

	public void DeleteObjectsFromList()
	{
		foreach (DS2ActiveObject item in m_cliquePlayerNeedDelete)
		{
			RemoveObjFromPlayerList(item);
		}
		m_cliquePlayerNeedDelete.Clear();
		foreach (DS2ActiveObject item2 in m_cliqueComputerNeedDelete)
		{
			m_cliqueComputerList.Remove(item2);
			m_iCliqueComputerObjCount--;
		}
		m_cliqueComputerNeedDelete.Clear();
		foreach (DS2Object item3 in m_interactObjectNeedDelete)
		{
			m_interactObjectList.Remove(item3);
		}
		m_interactObjectNeedDelete.Clear();
	}

	public Player GetPlayer(int index)
	{
		if (index == -1)
		{
			return m_player;
		}
		if (index < m_teammateList.Count)
		{
			return m_teammateList[index];
		}
		return null;
	}

	public Player GetPlayer()
	{
		return m_player;
	}

	public void AddObjToPlayerList(DS2ActiveObject obj)
	{
		obj.clique = DS2ActiveObject.Clique.Player;
		m_cliquePlayerList.Add(obj);
		m_cliquePlayerAliveList.Add(obj);
		m_iCliquePlayerObjCount++;
	}

	public void AddObjToPlayerAliveList(DS2ActiveObject obj)
	{
		if (!m_cliquePlayerAliveList.Contains(obj))
		{
			m_cliquePlayerAliveList.Add(obj);
		}
		if (m_cliquePlayerDeadList.Contains(obj))
		{
			m_cliquePlayerDeadList.Remove(obj);
		}
	}

	public void AddObjToPlayerDeadList(DS2ActiveObject obj)
	{
		if (m_cliquePlayerAliveList.Contains(obj))
		{
			m_cliquePlayerAliveList.Remove(obj);
		}
		if (!m_cliquePlayerDeadList.Contains(obj))
		{
			m_cliquePlayerDeadList.Add(obj);
		}
	}

	public DS2ActiveObject[] GetPlayerAliveList()
	{
		return m_cliquePlayerAliveList.ToArray();
	}

	public DS2ActiveObject[] GetPlayerDeadList()
	{
		return m_cliquePlayerDeadList.ToArray();
	}

	public DS2ActiveObject[] GetPlayerList()
	{
		return m_cliquePlayerList.ToArray();
	}

	public DS2ActiveObject[] GetEnemyList()
	{
		return m_cliqueComputerList.ToArray();
	}

	public DS2ActiveObject GetPlayerByIndex(int index)
	{
		if (index < 0 || index >= m_iCliquePlayerObjCount)
		{
			return null;
		}
		return m_cliquePlayerList[index];
	}

	public Player GetPlayerBySiteNum(int siteNum)
	{
		foreach (DS2ActiveObject cliquePlayer in m_cliquePlayerList)
		{
			if (cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player2 = (Player)cliquePlayer;
				if (player2.playerData.siteNum == siteNum)
				{
					return player2;
				}
			}
		}
		return null;
	}

	public Player GetPlayerBySquadSite(int siteNum)
	{
		foreach (DS2ActiveObject cliquePlayer in m_cliquePlayerList)
		{
			if (cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player2 = (Player)cliquePlayer;
				if (player2.SquadSite == siteNum)
				{
					return player2;
				}
			}
		}
		return null;
	}

	public int GetPlayerObjCount()
	{
		return m_iCliquePlayerObjCount;
	}

	public Player[] GetTeammateList()
	{
		return m_teammateList.ToArray();
	}

	public void AddToTeammateList(Player player)
	{
		if (player != null)
		{
			m_teammateList.Add(player);
		}
	}

	public void RemoveFromTeammateList(Player player)
	{
		if (m_teammateList.Contains(player))
		{
			m_teammateList.Remove(player);
		}
	}

	public Player GetTeammateByIndex(int index)
	{
		if (index < 0 || index >= m_teammateList.Count)
		{
			return null;
		}
		return m_teammateList[index];
	}

	public void RemoveObjFromPlayerList(DS2ActiveObject obj)
	{
		m_cliquePlayerList.Remove(obj);
		if (m_cliquePlayerAliveList.Contains(obj))
		{
			m_cliquePlayerAliveList.Remove(obj);
		}
		m_iCliquePlayerObjCount--;
	}

	public DS2ActiveObject GetNearestObjFromPlayerList(Vector3 position, bool playerOnly = false)
	{
		if (playerOnly || m_teammateList.Count <= 0)
		{
			return m_player;
		}
		DS2ActiveObject dS2ActiveObject = null;
		if (m_teammateList.Count > 0)
		{
			dS2ActiveObject = m_teammateList[0];
			float sqrMagnitude = (dS2ActiveObject.GetTransform().position - position).sqrMagnitude;
			foreach (Player teammate in m_teammateList)
			{
				if (teammate.Alive() && teammate.Visible)
				{
					float sqrMagnitude2 = (teammate.GetTransform().position - position).sqrMagnitude;
					if (sqrMagnitude > sqrMagnitude2)
					{
						dS2ActiveObject = teammate;
					}
				}
			}
		}
		return dS2ActiveObject;
	}

	public void AddObjToComputerList(DS2ActiveObject obj)
	{
		obj.clique = DS2ActiveObject.Clique.Computer;
		m_cliqueComputerList.Add(obj);
		m_iCliqueComputerObjCount++;
	}

	public DS2ActiveObject[] GetComputerList()
	{
		return m_cliqueComputerList.ToArray();
	}

	public int GetComputerObjCount()
	{
		return m_iCliqueComputerObjCount;
	}

	public DS2ActiveObject GetComputerObjByIndex(int index)
	{
		if (index < 0 || index >= m_cliqueComputerList.Count)
		{
			return null;
		}
		return m_cliqueComputerList[index];
	}

	private void RemoveObjFromComputerList(DS2ActiveObject obj)
	{
		if (m_cliqueComputerList.Contains(obj))
		{
			m_cliqueComputerList.Remove(obj);
			m_iCliqueComputerObjCount--;
		}
	}

	public DS2ActiveObject GetNearestObjFromComputerList(Vector3 position, ref float distance)
	{
		DS2ActiveObject result = null;
		if (m_cliqueComputerList.Count > 0)
		{
			float num = float.PositiveInfinity;
			foreach (DS2ActiveObject cliqueComputer in m_cliqueComputerList)
			{
				if (cliqueComputer.Alive() && cliqueComputer.Visible)
				{
					float sqrMagnitude = (cliqueComputer.GetTransform().position - position).sqrMagnitude;
					if (num > sqrMagnitude)
					{
						num = (distance = sqrMagnitude);
						result = cliqueComputer;
					}
				}
			}
		}
		return result;
	}

	public void AddObjToInteractObjectList(DS2Object obj)
	{
		m_interactObjectList.Add(obj);
	}

	private void RemoveObjFromActiveObjectList(DS2Object obj)
	{
		if (m_interactObjectList.Contains(obj))
		{
			m_interactObjectList.Remove(obj);
		}
	}

	public void AddToPlayerNeedDeleteList(DS2ActiveObject obj)
	{
		m_cliquePlayerNeedDelete.Add(obj);
	}

	public void AddToComputerNeedDeleteList(DS2ActiveObject obj)
	{
		m_cliqueComputerNeedDelete.Add(obj);
	}

	public void AddToInteractObjectNeedDeleteList(DS2Object obj)
	{
		m_interactObjectNeedDelete.Add(obj);
	}

	public DS2ActiveObject[] GetTargetList(DS2ActiveObject.Clique clique)
	{
		switch (clique)
		{
		case DS2ActiveObject.Clique.Player:
			return m_cliqueComputerList.ToArray();
		case DS2ActiveObject.Clique.Computer:
			return m_cliquePlayerList.ToArray();
		default:
			return null;
		}
	}

	public int GetTargetCount(DS2ActiveObject.Clique clique)
	{
		switch (clique)
		{
		case DS2ActiveObject.Clique.Player:
			return m_iCliqueComputerObjCount;
		case DS2ActiveObject.Clique.Computer:
			return m_iCliquePlayerObjCount;
		default:
			return 0;
		}
	}

	public DS2ActiveObject GetNearestObjFromTargetList(Vector3 position, DS2ActiveObject.Clique clique)
	{
		DS2ActiveObject result = null;
		float distance = 0f;
		switch (clique)
		{
		case DS2ActiveObject.Clique.Computer:
			result = GetNearestObjFromPlayerList(position, true);
			break;
		case DS2ActiveObject.Clique.Player:
			result = GetNearestObjFromComputerList(position, ref distance);
			break;
		}
		return result;
	}

	public DS2ActiveObject GetNearestObjFromTargetList(Character source)
	{
		if (source.lockedTarget != null && source.lockedTarget.Alive() && source.lockedTarget.Visible && source.clique != source.lockedTarget.clique)
		{
			return source.lockedTarget;
		}
		DS2ActiveObject result = null;
		float distance = 0f;
		if (source.clique == DS2ActiveObject.Clique.Computer)
		{
			int num = UnityEngine.Random.Range(0, 100);
			bool playerOnly = num < 60;
			result = (source.lockedTarget = GetNearestObjFromPlayerList(source.GetTransform().position, playerOnly));
		}
		else if (source.clique == DS2ActiveObject.Clique.Player)
		{
			result = GetNearestObjFromComputerList(source.GetTransform().position, ref distance);
		}
		return result;
	}

	public DS2ActiveObject GetNearestObjFromTargetListWithDistance(Character source, ref float distance)
	{
		if (source.lockedTarget != null && source.lockedTarget.Alive() && source.lockedTarget.Visible)
		{
			distance = (source.lockedTarget.GetTransform().position - source.GetTransform().position).sqrMagnitude;
			return source.lockedTarget;
		}
		DS2ActiveObject result = null;
		if (source.clique == DS2ActiveObject.Clique.Computer)
		{
			int num = UnityEngine.Random.Range(0, 100);
			bool playerOnly = num < 60;
			result = (source.lockedTarget = GetNearestObjFromPlayerList(source.GetTransform().position, playerOnly));
		}
		else if (source.clique == DS2ActiveObject.Clique.Player)
		{
			result = GetNearestObjFromComputerList(source.GetTransform().position, ref distance);
		}
		return result;
	}

	public DS2ActiveObject GetRandomObjFromTargetList(Character source, bool noControl = false)
	{
		DS2ActiveObject result = null;
		if (source.clique == DS2ActiveObject.Clique.Computer)
		{
			if (m_cliquePlayerAliveList.Count <= 0)
			{
				return null;
			}
			if (noControl)
			{
				if (m_cliquePlayerAliveList.Count > 1)
				{
					int index = UnityEngine.Random.Range(1, m_cliquePlayerAliveList.Count);
					result = m_cliquePlayerAliveList[index];
				}
			}
			else
			{
				int index2 = UnityEngine.Random.Range(0, m_cliquePlayerAliveList.Count);
				result = m_cliquePlayerAliveList[index2];
			}
		}
		else if (source.clique == DS2ActiveObject.Clique.Player)
		{
			int index3 = UnityEngine.Random.Range(0, m_cliqueComputerList.Count);
			result = m_cliqueComputerList[index3];
		}
		return result;
	}

	public DS2ActiveObject[] GetTeammateList(DS2ActiveObject.Clique clique)
	{
		switch (clique)
		{
		case DS2ActiveObject.Clique.Computer:
			return m_cliqueComputerList.ToArray();
		case DS2ActiveObject.Clique.Player:
			return m_cliquePlayerList.ToArray();
		default:
			return null;
		}
	}

	public void AddObjToBetrayList(DS2ActiveObject obj)
	{
		m_betrayList.Add(obj);
	}

	public void AddPlayToBetrayList(Player p)
	{
		if (!m_playerBetrayList.Contains(p))
		{
			m_playerBetrayList.Add(p);
		}
	}

	public Player[] GetPlayerBetrayList()
	{
		return m_playerBetrayList.ToArray();
	}

	private static bool IsBusyState(Player player)
	{
		if (player == null) return false;
		AIState state = player.GetCurrentAIState();
		if (state == null) return false;
		string n = state.name;
		return n == "Reload" || n == "Shift" || n == "Skill" || n == "SkillReady" || n == "SkillFindTarget" || n == "ChangeWeapon";
	}

	public void ChangeCurrentContorlPlayer(Player player)
	{
		if (player == m_player || player == null || !player.Alive())
		{
			return;
		}
		if (IsBusyState(player))
		{
			return;
		}
		if (DataCenter.Save().squadMode)
		{
			Player player2 = m_player;
			AddToTeammateList(m_player);
			m_player = null;
			m_player = player;
			RemoveFromTeammateList(m_player);
			return;
		}
		Player player3 = m_player;
		AddToTeammateList(m_player);
		m_player = null;
		m_player = player;
		m_player.CurrentController = true;
		SetCameraMoveAndZoomIn(m_player, 10f, -4f - (CameraViewDisY - 8.5f), 0.1f, 39f / 85f);
		if (player3 != null)
		{
			player3.CurrentController = false;
			player3 = null;
		}
		RemoveFromTeammateList(m_player);
	}

	public void ChangeCurrentControlPlayerByIndex(int index)
	{
		if (index < 0 || index >= m_teammateList.Count)
		{
			return;
		}
		for (int i = 0; i < m_teammateList.Count; i++)
		{
			if (m_teammateList[i].heroIndex == index)
			{
				ChangeCurrentContorlPlayer(m_teammateList[i]);
			}
		}
	}

	public void SetTeammateToCurrentControlPlayer(int index)
	{
		if (index >= 0 && index < m_teammateList.Count)
		{
			ChangeCurrentContorlPlayer(m_teammateList[index]);
		}
	}

	public void UpdateInput()
	{
		if (!Tutorial.Instance.TutorialInProgress && DataCenter.Save().m_bCanChangeTeamMember && !s_bInputLocked && !BattleUIEvent.s_anyButtonDown && !TUIHandleManager.s_anyUIButtonDown)
		{
			TUIInput[] input = TUIInputManager.GetInput();
			Player currentSelectPlayer = GetCurrentSelectPlayer(input);
			if (currentSelectPlayer != null)
			{
				ChangeCurrentContorlPlayer(currentSelectPlayer);
			}
		}
	}

	public Player GetCurrentSelectPlayer(TUIInput[] inputs)
	{
		Player result = null;
		for (int i = 0; i < inputs.Length; i++)
		{
			TUIInput tUIInput = inputs[i];
			if (tUIInput.inputType != 0)
			{
				continue;
			}
			Ray ray = GetCamera().ScreenPointToRay(tUIInput.position);
			RaycastHit[] array = Physics.RaycastAll(ray, 100f, 1536);
			if (array.Length > 1)
			{
				Array.Sort(array, (RaycastHit r1, RaycastHit r2) => r1.distance.CompareTo(r2.distance));
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].collider.gameObject.activeInHierarchy)
					{
						return DS2ObjectStub.GetObject<Player>(array[j].collider.gameObject);
					}
				}
			}
			else if (array.Length == 1 && array[0].collider.gameObject.activeInHierarchy)
			{
				return DS2ObjectStub.GetObject<Player>(array[0].collider.gameObject);
			}
		}
		return result;
	}

	public SquadController GetSquadController()
	{
		return m_squadController;
	}

	public void SetSquadMode(bool enable)
	{
		Util.s_squadMode = enable;
		if (Util.s_squadMode)
		{
			m_squadController.GetTransform().position = GetPlayer().GetTransform().position;
			m_squadController.GetGameObject().SetActive(true);
			DataCenter.Save().m_bManualUseSkill = true;
			DataCenter.Save().m_bCanChangeTeamMember = false;
			BattleUIEvent component = UIControlManager.Instance.GetControl(422).GetComponent<BattleUIEvent>();
			component.SetManualSkill();
			GetSquadController().UpdatePointsPosition();
		}
		else
		{
			m_squadController.GetGameObject().SetActive(false);
			DataCenter.Save().m_bManualUseSkill = true;
			DataCenter.Save().m_bCanChangeTeamMember = true;
			BattleUIEvent component2 = UIControlManager.Instance.GetControl(422).GetComponent<BattleUIEvent>();
			component2.SetManualSkill();
		}
		foreach (DS2ActiveObject cliquePlayer in m_cliquePlayerList)
		{
			if (cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || cliquePlayer.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player2 = (Player)cliquePlayer;
				player2.SetSquadMode(enable);
				player2.SetHalo(!enable);
			}
		}
		if (Util.s_squadMode)
		{
			CameraFocus = m_squadController;
			m_squadController.MoveSpeed = m_squadController.OriginalSpeed;
		}
		else
		{
			CameraFocus = m_player;
			Vector3 targetPos = new Vector3(CameraFocus.GetTransform().position.x, CameraFocus.GetTransform().position.y + m_instance.CameraViewDisY, m_instance.CameraFocus.GetTransform().position.z - m_instance.CameraViewDisZ);
			m_instance.SetCameraMove(targetPos, 15f, null, true);
		}
	}

	public void SetCameraView(Defined.CameraView viewType)
	{
		switch (viewType)
		{
		case Defined.CameraView.Default:
			m_instance.CameraViewDisY = 10f;
			m_instance.CameraViewDisZ = 10f;
			break;
		case Defined.CameraView.Far:
			m_instance.CameraViewDisY = 13f;
			m_instance.CameraViewDisZ = 13f;
			break;
		case Defined.CameraView.Close:
			m_instance.CameraViewDisY = 8.5f;
			m_instance.CameraViewDisZ = 8.5f;
			break;
		}
	}

	public void KillAllEnemy(DS2ActiveObject.Clique clique)
	{
		switch (clique)
		{
		case DS2ActiveObject.Clique.Player:
		{
			for (int j = 0; j < m_cliqueComputerList.Count; j++)
			{
				if (m_cliqueComputerList[j].Alive())
				{
					m_cliqueComputerList[j].OnDeath();
				}
			}
			break;
		}
		case DS2ActiveObject.Clique.Computer:
		{
			for (int i = 0; i < m_cliquePlayerList.Count; i++)
			{
				if (m_cliquePlayerList[i].Alive())
				{
					m_cliquePlayerList[i].OnDeath();
				}
			}
			break;
		}
		}
	}

	public void Dispose()
	{
		UIControlManager.Instance.Clear();
		m_player.Destroy(true);
		m_player = null;
		for (int num = m_cliqueComputerList.Count - 1; num >= 0; num--)
		{
			if (m_cliqueComputerList[num] != null)
			{
				m_cliqueComputerList[num].Destroy(true);
			}
		}
		m_cliqueComputerList.Clear();
		m_cliqueComputerList = null;
		for (int num2 = m_cliquePlayerList.Count - 1; num2 >= 0; num2--)
		{
			if (m_cliquePlayerList[num2] != null)
			{
				m_cliquePlayerList[num2].Destroy(true);
			}
		}
		m_cliquePlayerList.Clear();
		m_cliquePlayerList = null;
		for (int num3 = m_interactObjectList.Count - 1; num3 >= 0; num3--)
		{
			if (m_interactObjectList[num3] != null)
			{
				m_interactObjectList[num3].Destroy(true);
			}
		}
		m_interactObjectList.Clear();
		m_interactObjectList = null;
	}

	public void OnBreakOff()
	{
		Time.timeScale = 1f;
		s_enemyCount = 0;
		TAudioManager.instance.AudioListener.transform.parent = null;
		TAudioManager.instance.AudioListener.transform.localPosition = Vector3.zero;
		TAudioManager.instance.AudioListener.transform.localScale = Vector3.one;
	}
}

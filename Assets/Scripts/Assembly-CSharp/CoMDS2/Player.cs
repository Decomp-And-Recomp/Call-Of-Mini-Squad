using System.Collections.Generic;
using UnityEngine;

namespace CoMDS2
{
	public class Player : Character
	{
		public enum PlayerType
		{
			None = -1,
			Soldier = 0,
			Mike = 1,
			Swat = 2,
			Armer = 3,
			Policewoman = 4,
			Researcher = 5,
			Superman = 6,
			Ninjia_Fire = 7,
			Ninjia_Ice = 8
		}

		public enum CharacterType
		{
			Mike = 0,
			Chris = 1,
			Lili = 2,
			Vasily = 3,
			Claire = 4,
			FireDragon = 5,
			Zero = 6,
			Arnoud = 7,
			XJohnX = 8,
			Clint = 9,
			Eva = 10,
			Jason = 11,
			Tanya = 12,
			Bourne = 13,
			Rock = 14,
			Wesker = 15,
			Oppenheimer = 16,
			Shepard = 17
		}

		public const float ALLY_TOFOLLOW_INBATTLE_DIS = 10f;

		private float m_checkReloadTime;

		private bool m_bCurrentController;

		public int seatId;

		public static float ALLY_TOFOLLOW_DIS = 3.5f;

		public static float ALLY_TOFOLLOW_PVP_DIS = (float)Screen.height * 0.5f;

		private bool m_fireKeyDown;

		private bool m_moveKeyDown;

		private bool m_bAutoControl;

		private bool m_bHalfAutoControl;

		private bool m_bChangeToControl;

		private float m_fChangeToControlTimer;

		private bool m_bFaceToMoveDirection = true;

		private GameObject m_moveCopy;

		public static GameObject s_leaderHaloGameObject;

		public GameObject haloGameObject;

		public GameObject haloGameObjectAlly;

		public GameObject haloGameObjectLeader;

		public static AllySeat s_allySeat;

		private int m_squadSite;

		protected bool m_skillNeedFindTarget;

		private Transform m_hpBar;

		public bool m_bPlayerLockAttackDirection;

		public DS2ActiveObject m_objAutoShootAreaTarget;

		public float m_def;

		protected ushort m_proStab;

		protected float m_atkFrequencyPercent;

		protected float m_atkRange;

		private ushort m_proResilience;

		private float m_clipPercent;

		private float m_clipCDTime;

		private float m_critDamagePercent;

		public float UIHurtFlashTimer;

		private List<TeamSpecialAttribute.TeamAttributeType> m_teamAttributeTalentList;

		private List<TeamSpecialAttribute.TeamAttributeEvolveType> m_teamAttributeEvolveList;

		private int m_nextAttackCritTimes;

		private int m_shieldHp;

		private GameObject m_shieldHpEffectCallBack;

		private bool m_finalBattleEnable;

		public float originalMoveSpeed;

		private int betrayHpMark;

		public AudioTalkManager audioTalkManager;

		public bool willResurgence;

		private float m_playerRotateAngle;

		private float m_playerRotateAngleMark;

		private float m_left_lparam;

		private float m_left_last_lparam;

		private float m_right_last_lparam;

		private float m_right_lparam_lasttime = -1f;

		private float m_checkSkillConditionTimer;

		private float m_checkSkillConditionInterval = 1f;

		public PlayerType playerType { get; set; }

		public CharacterType characterType { get; set; }

		public new string name { get; set; }

		public int heroIndex { get; set; }

		public Defined.RANK_TYPE rank { get; set; }

		public string iconName { get; set; }

		public PlayerData playerData { get; set; }

		public TeamData teamData { get; set; }

		public DataConf.HeroSkillInfo skillInfo { get; set; }

		public bool FireKeyDown
		{
			get
			{
				return m_fireKeyDown;
			}
			set
			{
				m_fireKeyDown = value;
				if (!Util.s_autoControl)
				{
					return;
				}
				if (m_fireKeyDown)
				{
					AIState aIState = GetAIState("Idle");
					SetDefaultAIState(aIState);
					if (MoveAble)
					{
						SwitchFSM(aIState);
					}
					AutoControl = false;
					HalfAutoControl = false;
					m_bChangeToControl = false;
					m_fire = true;
					m_fChangeToControlTimer = 0f;
				}
				else
				{
					m_bChangeToControl = true;
					m_fChangeToControlTimer = 0f;
				}
			}
		}

		public bool MoveKeyDown
		{
			get
			{
				return m_moveKeyDown;
			}
			set
			{
				m_moveKeyDown = value;
				if (Util.s_autoControl)
				{
					if (m_moveKeyDown)
					{
						if (!m_fireKeyDown)
						{
							AIState aIState = GetAIState("AutoControl");
							SetDefaultAIState(aIState);
							SwitchFSM(aIState);
							StopNav(false);
							HalfAutoControl = true;
						}
					}
					else if (DataCenter.State().isPVPMode && !m_fireKeyDown)
					{
						AIState aIState2 = GetAIState("GuardPVP");
						SetDefaultAIState(aIState2);
						SwitchFSM(aIState2);
						ResumeNav();
						HalfAutoControl = true;
					}
				}
				else
				{
					SetMove(false, FaceDirection);
				}
			}
		}

		public bool AutoControl
		{
			get
			{
				return m_bAutoControl;
			}
			set
			{
				m_bAutoControl = value;
			}
		}

		public bool HalfAutoControl
		{
			get
			{
				return m_bHalfAutoControl;
			}
			set
			{
				m_bHalfAutoControl = value;
			}
		}

		public bool FaceToMoveDirection
		{
			get
			{
				return m_bFaceToMoveDirection;
			}
			set
			{
				m_bFaceToMoveDirection = value;
			}
		}

		public override Vector3 FaceDirection
		{
			get
			{
				return base.FaceDirection;
			}
			set
			{
				base.FaceDirection = value;
				m_moveCopy.transform.forward = value;
			}
		}

		public bool CurrentController
		{
			get
			{
				return m_bCurrentController;
			}
			set
			{
				if (DataCenter.Save().squadMode && GameBattle.m_instance != null)
				{
					if (SquadSite == 0)
					{
						base.Layer = 9;
					}
					else
					{
						base.Layer = 10;
					}
					m_bCurrentController = false;
				}
				else
				{
					m_bCurrentController = value;
					SetHalo();
				}
				m_bChangeToControl = false;
				m_fChangeToControlTimer = 0f;
				if (m_bCurrentController)
				{
					if (DataCenter.State().isPVPMode)
					{
						if (Util.s_pvp_atutoPlay)
						{
							AIStatePVPGuard aIStatePVPGuard = GetAIState("GuardPVP") as AIStatePVPGuard;
							aIStatePVPGuard.SetGuard(playerData.siteNum);
							SetDefaultAIState(aIStatePVPGuard);
							if (Alive())
							{
								SwitchFSM(aIStatePVPGuard);
							}
						}
						else
						{
							AIState aIState = GetAIState("Idle");
							SetDefaultAIState(aIState);
							if (Alive())
							{
								SwitchFSM(aIState);
							}
						}
					}
					else if (Util.s_autoControl)
					{
						AIState aIState2 = GetAIState("AutoControl");
						SetDefaultAIState(aIState2);
						AutoControl = true;
						if (Alive())
						{
							SwitchFSM(aIState2);
							FaceDirection = GetModelTransform().forward;
							SetNavDesination(Vector3.zero);
							StopNav(false);
						}
					}
					else
					{
						AIState aIState3 = GetAIState("Idle");
						SetDefaultAIState(aIState3);
						if (Alive())
						{
							SwitchFSM(aIState3);
						}
					}
					GetGameObject().layer = 9;
					if (GameBattle.m_instance != null && GameBattle.m_instance.GameState == GameBattle.State.Game)
					{
						audioTalkManager.PlaySelect();
					}
					return;
				}
				if (Alive() || willResurgence)
				{
					if (DataCenter.Save().squadMode)
					{
						AIStateSquadGuard aIStateSquadGuard = GetAIState("SquadGuard") as AIStateSquadGuard;
						aIStateSquadGuard.SetGuard(SquadSite);
						SetDefaultAIState(aIStateSquadGuard);
						if (Alive())
						{
							SwitchFSM(aIStateSquadGuard);
						}
						return;
					}
					if (DataCenter.State().isPVPMode)
					{
						AIStatePVPGuard aIStatePVPGuard2 = GetAIState("GuardPVP") as AIStatePVPGuard;
						aIStatePVPGuard2.SetGuard(playerData.siteNum);
						SetDefaultAIState(aIStatePVPGuard2);
						if (Alive())
						{
							SwitchFSM(aIStatePVPGuard2);
						}
					}
					else
					{
						AIStateAllyGuard aIStateAllyGuard = GetAIState("Guard") as AIStateAllyGuard;
						aIStateAllyGuard.SetGuard(GameBattle.m_instance.GetPlayer());
						SetDefaultAIState(aIStateAllyGuard);
						if (Alive())
						{
							SwitchFSM(aIStateAllyGuard);
						}
					}
				}
				if (GetGameObject().layer == 9)
				{
					GetGameObject().layer = 10;
				}
			}
		}

		public bool isAlly
		{
			get
			{
				return !CurrentController;
			}
		}

		public int SquadSite
		{
			get
			{
				return m_squadSite;
			}
			set
			{
				m_squadSite = value;
				AIStateSquadGuard aIStateSquadGuard = GetAIState("SquadGuard") as AIStateSquadGuard;
				aIStateSquadGuard.SetGuard(m_squadSite);
			}
		}

		public override float MoveSpeed
		{
			get
			{
				return base.MoveSpeed;
			}
			set
			{
				base.MoveSpeed = value;
				if (DataCenter.Save().squadMode && GameBattle.m_instance != null && SquadSite == 0)
				{
					GameBattle.m_instance.GetSquadController().MoveSpeed = value;
				}
			}
		}

		public bool HasTalent(TeamSpecialAttribute.TeamAttributeType type)
		{
			if (base.clique != 0)
			{
				return false;
			}
			return m_teamAttributeTalentList.Contains(type);
		}

		public bool HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType type)
		{
			return m_teamAttributeEvolveList.Contains(type);
		}

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			AddAnimationEvents();
			m_weaponMap = new Dictionary<int, Weapon>();
			m_weapon = null;
			m_weaponChange = null;
			base.meleeAble = true;
			base.shootAble = true;
			AIStateMove aIStateMove = new AIStateMove(this, "Move", AIState.Controller.Player);
			aIStateMove.speed = MoveSpeed;
			AIStateIdle aIStateIdle = new AIStateIdle(this, "Idle", AIState.Controller.Player);
			AIStateShootWithWeapon aIStateShootWithWeapon = new AIStateShootWithWeapon(this, "Shoot", AIState.Controller.Player);
			AIStateMoveShoot aIStateMoveShoot = new AIStateMoveShoot(this, "MoveShoot", AIState.Controller.Player);
			AIStateHurt aIStateHurt = new AIStateHurt(this, "Hurt", AIState.Controller.Player);
			AIStateDeath aIStateDeath = new AIStateDeath(this, "Death", AIState.Controller.Player);
			AIStateRepel aIStateRepel = new AIStateRepel(this, "Repel", AIState.Controller.Player);
			AIStateChangeWeapon aIStateChangeWeapon = new AIStateChangeWeapon(this, "Shift", AIState.Controller.Player);
			AIStateReload aIStateReload = new AIStateReload(this, "Reload", AIState.Controller.Player);
			AIStateLowerMove aIStateLowerMove = new AIStateLowerMove(this, "LowerMove", AIState.Controller.Player);
			aIStateLowerMove.speed = MoveSpeed;
			AIStateFireReady aIStateFireReady = new AIStateFireReady(this, "FireReady");
			AIStateFollow aIStateFollow = new AIStateFollow(this, "AllyFollow", AIState.Controller.Player);
			if (GameBattle.m_instance != null)
			{
				aIStateFollow.SetFollow(GameBattle.m_instance.GetPlayer(), 5f);
			}
			AIStateAllyFindSeat aIStateAllyFindSeat = new AIStateAllyFindSeat(this, "FindSeat", AIState.Controller.Player);
			AIStateUpperFire aIStateUpperFire = new AIStateUpperFire(this, "UpperFire", AIState.Controller.Player);
			AIStateSeriousInjury aIStateSeriousInjury = new AIStateSeriousInjury(this, "SeriousInjury", AIState.Controller.Player);
			AIStateStun aIStateStun = new AIStateStun(this, "Stun");
			AIStateFrozen aIStateFrozen = new AIStateFrozen(this, "Frozen");
			AIStateUpperReload aIStateUpperReload = new AIStateUpperReload(this, "UpperReload");
			AIStateAutoControl aIStateAutoControl = new AIStateAutoControl(this, "AutoControl", AIState.Controller.Player);
			AIStateSquadGuard aIStateSquadGuard = new AIStateSquadGuard(this, "SquadGuard", AIState.Controller.Player);
			AddAIState(aIStateSquadGuard.name, aIStateSquadGuard);
			AIStateStuck aIStateStuck = new AIStateStuck(this, "Stuck", AIState.Controller.Player);
			AddAIState(aIStateMove.name, aIStateMove);
			AddAIState(aIStateIdle.name, aIStateIdle);
			AddAIState(aIStateShootWithWeapon.name, aIStateShootWithWeapon);
			AddAIState(aIStateMoveShoot.name, aIStateMoveShoot);
			AddAIState(aIStateHurt.name, aIStateHurt);
			AddAIState(aIStateDeath.name, aIStateDeath);
			AddAIState(aIStateRepel.name, aIStateRepel);
			AddAIState(aIStateChangeWeapon.name, aIStateChangeWeapon);
			AddAIState(aIStateReload.name, aIStateReload);
			AddAIState(aIStateLowerMove.name, aIStateLowerMove);
			AddAIState(aIStateFireReady.name, aIStateFireReady);
			AddAIState(aIStateFollow.name, aIStateFollow);
			AddAIState(aIStateAllyFindSeat.name, aIStateAllyFindSeat);
			AddAIState(aIStateUpperFire.name, aIStateUpperFire);
			AddAIState(aIStateSeriousInjury.name, aIStateSeriousInjury);
			AddAIState(aIStateStun.name, aIStateStun);
			AddAIState(aIStateFrozen.name, aIStateFrozen);
			AddAIState(aIStateUpperReload.name, aIStateUpperReload);
			AddAIState(aIStateAutoControl.name, aIStateAutoControl);
			AddAIState(aIStateStuck.name, aIStateStuck);
			AIStatePVPGuard aIStatePVPGuard = new AIStatePVPGuard(this, "GuardPVP", AIState.Controller.Player);
			aIStatePVPGuard.SetGuard(playerData.siteNum, 3f, 6f);
			AddAIState(aIStatePVPGuard.name, aIStatePVPGuard);
			AIStateAllyGuard aIStateAllyGuard = new AIStateAllyGuard(this, "Guard", AIState.Controller.Player);
			if (GameBattle.m_instance != null)
			{
				aIStateAllyGuard.SetGuard(GameBattle.m_instance.GetPlayer());
			}
			AddAIState(aIStateAllyGuard.name, aIStateAllyGuard);
			if (DataCenter.State().isPVPMode)
			{
				AIStateAvoid aIStateAvoid = new AIStateAvoid(this, "Avoid", AIState.Controller.Player);
				AddAIState(aIStateAvoid.name, aIStateAvoid);
			}
			InitMoveAnimations();
			base.hitInfo.damage = baseAttribute.damage;
			base.hitInfo.critRate = (int)base.critRate;
			base.hitInfo.critDamage = base.critDamage;
			base.hitInfo.hitRate = base.hitRate;
			base.hitInfo.stabRate = m_proStab;
			m_skillTotalCDTime = 20f;
			m_moveCopy = new GameObject();
			m_moveCopy.name = "moveCopy";
			if (BattleBufferManager.s_objectRoot != null)
			{
				m_moveCopy.transform.parent = BattleBufferManager.s_objectRoot.transform;
			}
			if (HasNavigation())
			{
				GetNavMeshAgent().obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
			}
			if (audioTalkManager == null)
			{
				audioTalkManager = GetTransform().Find("AudioTalk").GetComponent<AudioTalkManager>();
			}
		}

		protected override void InitMoveAnimations()
		{
			base.InitMoveAnimations();
			moveAnimations = new MoveAnimation[4];
			moveAnimations[0] = new MoveAnimation();
			moveAnimations[0].animName = "Lower_Body_Run_F";
			moveAnimations[0].velocity = new Vector3(0f, 0f, 6f);
			moveAnimations[0].Init();
			moveAnimations[1] = new MoveAnimation();
			moveAnimations[1].animName = "Lower_Body_Run_B";
			moveAnimations[1].velocity = new Vector3(0f, 0f, -6f);
			moveAnimations[1].Init();
			moveAnimations[2] = new MoveAnimation();
			moveAnimations[2].animName = "Lower_Body_Run_R";
			moveAnimations[2].velocity = new Vector3(6f, 0f, 0f);
			moveAnimations[2].Init();
			moveAnimations[3] = new MoveAnimation();
			moveAnimations[3].animName = "Lower_Body_Run_L";
			moveAnimations[3].velocity = new Vector3(-6f, 0f, 0f);
			moveAnimations[3].Init();
		}

		public override Transform GetMoveTransform()
		{
			return Character.s_spawnTransform;
		}

		protected override void SetAnimationsMixing()
		{
			Transform mix = m_transform.Find("Bip01/Spine_00/Bip01 Spine");
			if (m_fixedWeapon)
			{
				if (name == null || name == string.Empty)
				{
					return;
				}
				DataConf.AnimData newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(name, "Hurt");
				if (newCharacterAnim.count > 1)
				{
					for (int i = 0; i < newCharacterAnim.count; i++)
					{
						m_gameObject.GetComponent<Animation>()[newCharacterAnim.name + "0" + (i + 1)].layer = 2;
						m_gameObject.GetComponent<Animation>()[newCharacterAnim.name + "0" + (i + 1)].AddMixingTransform(mix);
					}
				}
				else
				{
					m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].layer = 2;
					m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].AddMixingTransform(mix);
				}
				newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(name, "Reload");
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].layer = 2;
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].AddMixingTransform(mix);
				newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(name, "Shift");
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].layer = 2;
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].AddMixingTransform(mix);
				newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(name, "Shoting");
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].layer = 2;
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].AddMixingTransform(mix);
				newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(name, "ReadyFire");
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].layer = 2;
				m_gameObject.GetComponent<Animation>()[newCharacterAnim.name].AddMixingTransform(mix);
				return;
			}
			string weaponName = getWeaponName();
			DataConf.AnimData characterAnim = DataCenter.Conf().GetCharacterAnim("Hurt");
			string empty = string.Empty;
			for (int j = 0; j < characterAnim.count; j++)
			{
				empty = ((!characterAnim.name.Contains("Rifle")) ? weaponName : string.Empty);
				m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].layer = 2;
				m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].AddMixingTransform(mix);
			}
			characterAnim = DataCenter.Conf().GetCharacterAnim("Reload");
			empty = ((!characterAnim.name.Contains("Rifle")) ? weaponName : string.Empty);
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].layer = 2;
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].AddMixingTransform(mix);
			characterAnim = DataCenter.Conf().GetCharacterAnim("Shift");
			empty = ((!characterAnim.name.Contains("Rifle")) ? weaponName : string.Empty);
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].layer = 2;
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].AddMixingTransform(mix);
			characterAnim = DataCenter.Conf().GetCharacterAnim("Shoting");
			empty = ((!characterAnim.name.Contains("Rifle")) ? weaponName : string.Empty);
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].layer = 2;
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].AddMixingTransform(mix);
			characterAnim = DataCenter.Conf().GetCharacterAnim("ReadyFire");
			empty = ((!characterAnim.name.Contains("Rifle")) ? weaponName : string.Empty);
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].layer = 2;
			m_gameObject.GetComponent<Animation>()[empty + characterAnim.name].AddMixingTransform(mix);
		}

		public override void Update(float deltaTime)
		{
			AIState currentAIState = GetCurrentAIState();
			if (Alive() && m_move)
			{
				UpdateLowerBodyMoveDir4();
				if (!AnimationPlaying(base.animLowerBody))
				{
					AnimationCrossFade(base.animLowerBody, true, false, 0.1f);
				}
			}
			base.Update(deltaTime);
			UpdateSkillOnAIControl();
			UpdateChangeToAutoControl();
			if (DataCenter.Save().squadMode && GameBattle.m_instance != null)
			{
				if (SquadSite == 0 && Tutorial.Instance.TutorialPahseMove == Tutorial.TutorialPhaseState.Done && Tutorial.Instance.TutorialPahseSkill == Tutorial.TutorialPhaseState.None && Tutorial.Instance.NextStep)
				{
					int layerMask = ((base.clique != 0) ? 1536 : 2048);
					Ray ray = new Ray(m_effectPoint.position, GetModelTransform().forward);
					RaycastHit[] array = Physics.SphereCastAll(ray, 2f, 4f, layerMask);
					int num = array.Length;
					if (num > 0)
					{
						Tutorial.Instance.TutorialPahseSkill = Tutorial.TutorialPhaseState.InProgress;
					}
				}
			}
			else if (Alive() && !GameBattle.s_bInputLocked && currentAIState.name != "Reload" && currentAIState.name != "Shift" && currentAIState.name != "SkillReady" && currentAIState.name != "Skill" && !base.isStuck && !base.isFrozen && !AnimationPlaying("Hurt") && m_weapon.NeedReload())
			{
				m_checkReloadTime += Time.deltaTime;
				if (m_weapon.ReloadFinished())
				{
					m_weapon.DoReload();
				}
				else if (m_checkReloadTime >= 0f)
				{
					m_checkReloadTime = 0f;
					if (CurrentController)
					{
						m_weapon.PlayAudioReload();
					}
					SwitchFSM(GetAIState("Reload"));
				}
			}
			if (!DataCenter.State().isPVPMode)
			{
				if (!CurrentController)
				{
					if (Alive() && !DataCenter.Save().squadMode)
					{
						if (!GameBattle.s_objectWaitToTeleport.Contains(this))
						{
							Player player = GameBattle.m_instance.GetPlayer();
							float sqrMagnitude = (player.GetTransform().position - GetTransform().position).sqrMagnitude;
							if (sqrMagnitude >= 1600f)
							{
								GameBattle.s_objectWaitToTeleport.Add(this);
							}
						}
						FaceDirection = GetModelTransform().forward;
					}
				}
				else if (Alive() && HasNavigation())
				{
					GetNavMeshAgent().Stop(false);
				}
			}
			if (CurrentController && FaceToMoveDirection && !m_fire && GameBattle.m_instance != null)
			{
				float y = m_moveCopy.transform.eulerAngles.y;
				if (Mathf.Abs(m_playerRotateAngleMark - y) > 10f)
				{
					m_playerRotateAngle = Mathf.LerpAngle(m_playerRotateAngle, y, 0.2f);
					m_playerRotateAngleMark = Mathf.Lerp(m_playerRotateAngleMark, y, 0.2f);
					GetModelTransform().eulerAngles = new Vector3(0f, m_playerRotateAngle, 0f);
					FaceDirection = GetTransform().forward;
				}
				else
				{
					GetModelTransform().eulerAngles = new Vector3(0f, m_playerRotateAngle, 0f);
				}
			}
			if (!DataCenter.Save().BattleTutorialFinished && (float)base.hp <= (float)base.hpMax * 0.5f)
			{
				base.hp = base.hpMax;
			}
		}

		private void AddAnimationEvents()
		{
		}

		public void UpdateInput(float left_wparam, float left_lparam, float right_wparam, float right_lparam)
		{
			if (base.isStuck)
			{
				return;
			}
			if (left_lparam != 0f || left_wparam != 0f)
			{
				DataCenter.State().lastLeftKeyDownTime = Time.realtimeSinceStartup;
				if (!base.isFrozen && !base.isStuck)
				{
					if (m_left_lparam < 1f)
					{
						m_left_lparam += 0.1f;
						if (m_left_lparam > 1f)
						{
							m_left_lparam = 1f;
						}
					}
					else if (left_wparam < 1f && m_left_last_lparam != left_wparam)
					{
						m_left_lparam = left_wparam;
					}
					m_left_last_lparam = left_wparam;
					Vector3 moveDirection = new Vector3(Mathf.Cos(left_lparam), 0f, Mathf.Sin(left_lparam)) * m_left_lparam;
					SetMove(true, moveDirection);
					if (!MoveKeyDown)
					{
						MoveKeyDown = true;
					}
					if (!m_fire && FaceToMoveDirection && Vector3.Angle(FaceDirection, moveDirection) < 10f)
					{
						FaceDirection = moveDirection;
						GetTransform().forward = FaceDirection;
					}
				}
			}
			else
			{
				m_left_lparam = 0f;
				if (DataCenter.State().isPVPMode)
				{
					if (MoveKeyDown)
					{
						MoveKeyDown = false;
					}
				}
				else
				{
					SetMove(false, FaceDirection);
				}
			}
			if (right_lparam != 0f)
			{
				DataCenter.State().lastRightKeyDownTime = Time.realtimeSinceStartup;
				Vector3 fireDirection = new Vector3(Mathf.Cos(right_lparam), 0f, Mathf.Sin(right_lparam));
				if (m_right_last_lparam == right_lparam)
				{
					if (m_right_lparam_lasttime == -1f)
					{
						m_right_lparam_lasttime = Time.time;
					}
					if (Time.time - m_right_lparam_lasttime >= DataCenter.Save().m_fLockRJoystickMaxTime)
					{
						m_bPlayerLockAttackDirection = false;
					}
				}
				else
				{
					m_bPlayerLockAttackDirection = false;
					m_objAutoShootAreaTarget = null;
					m_right_lparam_lasttime = -1f;
				}
				m_right_last_lparam = right_lparam;
				float num = 90f - right_lparam * 57.29578f;
				if (m_objAutoShootAreaTarget == null)
				{
					m_playerRotateAngle = Mathf.LerpAngle(m_playerRotateAngle, num, 0.2f);
					m_playerRotateAngleMark = Mathf.Lerp(m_playerRotateAngleMark, num, 0.2f);
					GetModelTransform().eulerAngles = new Vector3(0f, m_playerRotateAngle, 0f);
					FaceDirection = GetTransform().forward;
				}
				if (!m_fire && Mathf.Abs(m_playerRotateAngleMark - num) < 10f)
				{
					SetFire(true, fireDirection);
				}
				if (!FireKeyDown)
				{
					FireKeyDown = true;
				}
				FaceToMoveDirection = false;
			}
			else if (Time.realtimeSinceStartup - DataCenter.State().lastRightKeyDownTime > 0.3f && !AutoControl && !HalfAutoControl)
			{
				if (FireKeyDown)
				{
					FireKeyDown = false;
				}
				if (m_fire)
				{
					SetFire(false, m_fireDirection);
				}
			}
		}

		public override void SetFire(bool fire, Vector3 fireDirection)
		{
			base.SetFire(fire, fireDirection);
			if (fire)
			{
				FaceToMoveDirection = false;
			}
		}

		public override void SetFaceDirectionImmediately(Vector3 direction)
		{
			Vector3 vector = direction;
			GetTransform().forward = vector;
			base.FaceDirection = vector;
			m_moveCopy.transform.forward = direction;
			m_playerRotateAngleMark = (m_playerRotateAngle = m_moveCopy.transform.eulerAngles.y);
		}

		public override HitResultInfo OnHit(HitInfo hitInfo)
		{
			HitResultInfo hitResultInfo = base.OnHit(hitInfo);
			if (!hitResultInfo.isHit)
			{
				return hitResultInfo;
			}
			int num = Random.Range(0, 100);
			if (num < hitInfo.stabRate)
			{
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Resolve))
				{
					float num2 = (1f - (float)base.hp / (float)base.hpMax) / (DataCenter.Conf().m_teamAttributeResolve.hpReducePercent * 100f);
					float num3 = DataCenter.Conf().m_teamAttributeResolve.attackPercent[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Resolve] - 1];
					float l = baseAttribute.damage.left + baseAttribute.damage.left * num2 * num3;
					float r = baseAttribute.damage.right + baseAttribute.damage.right * num2 * num3;
					base.hitInfo.damage = new NumberSection<float>(l, r);
					base.hitInfo.critDamage = base.hitInfo.critDamage + base.hitInfo.critDamage * num2 * num3;
					BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.heores_Aimed_Shot, m_effectPoint.position, 0f, m_effectPoint);
				}
			}
			else
			{
				HitInfo hitInfo2 = new HitInfo();
				hitInfo2.Copy(hitInfo);
				float a = hitInfo2.damage.left - hitInfo2.damage.left * m_def;
				a = Mathf.Max(a, 0f);
				float a2 = hitInfo2.damage.right - hitInfo2.damage.right * m_def;
				a2 = Mathf.Max(a2, 0f);
				hitInfo2.critRate -= (int)m_proResilience;
				hitInfo2.damage = new NumberSection<float>(a, a2);
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Resolve))
				{
					float num4 = (1f - (float)base.hp / (float)base.hpMax) / (DataCenter.Conf().m_teamAttributeResolve.hpReducePercent * 100f);
					float num5 = DataCenter.Conf().m_teamAttributeResolve.attackPercent[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Resolve] - 1];
					float l2 = baseAttribute.damage.left + baseAttribute.damage.left * num4 * num5;
					float r2 = baseAttribute.damage.right + baseAttribute.damage.right * num4 * num5;
					base.hitInfo.damage = new NumberSection<float>(l2, r2);
					base.hitInfo.critDamage = base.hitInfo.critDamage + base.hitInfo.critDamage * num4 * num5;
				}
			}
			if (!m_finalBattleEnable && HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.EndBattle))
			{
				TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.EndBattle);
				float num6 = teamAttributeEvolveData.probability[0];
				if ((float)(base.hp / base.hpMax) <= num6)
				{
					m_finalBattleEnable = true;
					float num7 = teamAttributeEvolveData.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.EndBattle] - 1];
					baseAttribute.damage = new NumberSection<float>(baseAttribute.damage.left * (1f + num7), baseAttribute.damage.right * (1f + num7));
					base.hitInfo.damage = baseAttribute.damage;
					baseAttribute.critDamage = baseAttribute.damage.left * m_critDamagePercent;
					base.critDamage = baseAttribute.critDamage;
					base.hitInfo.critDamage = base.critDamage;
				}
			}
			if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Thorns))
			{
				TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData2 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Thorns);
				float num8 = teamAttributeEvolveData2.probability[0];
				if (teamAttributeEvolveData2.probability.Length > 1)
				{
					num8 = teamAttributeEvolveData2.probability[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Thorns] - 1];
				}
				int max = 1000;
				num8 *= 10f;
				if ((float)Random.Range(0, max) < num8)
				{
					float num9 = teamAttributeEvolveData2.percent[0];
					if (teamAttributeEvolveData2.percent.Length > 1)
					{
						num9 = teamAttributeEvolveData2.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Thorns] - 1];
					}
					HitInfo hitInfo3 = new HitInfo();
					hitInfo3.damage = new NumberSection<float>(hitResultInfo.damage * num9);
					if (hitInfo.source != null)
					{
						hitInfo.source.OnHit(hitInfo3);
					}
				}
			}
			return hitResultInfo;
		}

		public override void OnHurt(bool switchHurtState)
		{
			base.OnHurt(switchHurtState);
			UIHurtFlashTimer = 1f;
			if (CurrentController)
			{
				audioTalkManager.PlayHurt();
			}
		}

		public override void OnDeath()
		{
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Resurrection))
			{
				ushort num = DataCenter.Conf().m_teamAttributeResurrection.proAvoidDead[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Resurrection] - 1];
				if (Random.Range(0, 100) < num)
				{
					willResurgence = true;
				}
				else
				{
					willResurgence = false;
					if (HasNavigation())
					{
						StopNav();
						GetNavMeshAgent().enabled = false;
					}
				}
			}
			else
			{
				willResurgence = false;
				if (HasNavigation())
				{
					StopNav();
					GetNavMeshAgent().enabled = false;
				}
			}
			base.OnDeath();
			GameBattle.m_instance.AddObjToPlayerDeadList(this);
			if (m_shieldHpEffectCallBack != null)
			{
				m_shieldHpEffectCallBack.SetActive(false);
			}
			audioTalkManager.PlayDeath();
			if (DataCenter.State().isPVPMode)
			{
				if (base.clique == Clique.Player && !Alive() && !CurrentController)
				{
					SwitchFSM(GetAIState("SeriousInjury"));
				}
			}
			else if (!Alive() && !CurrentController)
			{
				SwitchFSM(GetAIState("SeriousInjury"));
			}
			DataCenter.State().battleStars = 1;
		}

		public void Restart()
		{
			Reset();
			foreach (Weapon value in m_weaponMap.Values)
			{
				value.Reset();
			}
			ResumeNav();
			GetTransform().position = Character.s_spawnTransform.position;
			GetModelTransform().rotation = Character.s_spawnTransform.rotation;
			FaceDirection = GetModelTransform().forward;
			SwitchFSM(GetAIState("Idle"));
		}

		public bool ManualReload()
		{
			if (!Alive() || base.isStuck || base.isFrozen || GameBattle.s_bInputLocked)
			{
				return false;
			}
			if (m_weapon == null || !m_weapon.NeedReload())
			{
				return false;
			}
			AIState currentAIState = GetCurrentAIState();
			if (currentAIState.name == "Reload" || currentAIState.name == "Shift" || currentAIState.name == "SkillReady" || currentAIState.name == "Skill" || currentAIState.name == "Hurt")
			{
				return false;
			}
			m_checkReloadTime = 0f;
			if (CurrentController)
			{
				m_weapon.PlayAudioReload();
			}
			SwitchFSM(GetAIState("Reload"));
			return true;
		}

		public void Move(Vector3 move_dir)
		{
			CharacterController component = GetGameObject().GetComponent<CharacterController>();
			component.Move(move_dir);
		}

		public override IPathFinding GetPathFinding()
		{
			return this;
		}

		public virtual string getWeaponName()
		{
			switch (playerType)
			{
			case PlayerType.Soldier:
				return "Grenade";
			case PlayerType.Swat:
				return "MachineGun";
			case PlayerType.Mike:
				return "ShotGun";
			case PlayerType.Armer:
				return "Sniper";
			case PlayerType.Ninjia_Fire:
				return "Ninjia_Fire";
			case PlayerType.Ninjia_Ice:
				return "Ninjia_Ice";
			case PlayerType.Policewoman:
				return "Pistol";
			case PlayerType.Researcher:
				return "Virus";
			case PlayerType.Superman:
				return "Laser";
			default:
				return string.Empty;
			}
		}

		public virtual Weapon.WeaponType getWeaponType()
		{
			DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
			return heroDataByIndex.weaponType;
		}

		public override void Reset()
		{
			GetNavMeshAgent().enabled = true;
			ResumeNav();
			GameBattle.m_instance.AddObjToPlayerAliveList(this);
		}

		public override void Betray(Clique toClique)
		{
			base.Betray(toClique);
			StopNav();
			SetMove(false, MoveDirection);
			if (toClique == Clique.Computer)
			{
				base.isBetray = true;
				betrayHpMark = base.hpLast;
				GameBattle.m_instance.RemoveFromTeammateList(this);
				AIStatePVPGuard aIStatePVPGuard = GetAIState("GuardPVP") as AIStatePVPGuard;
				aIStatePVPGuard.SetGuard(-1);
				SetDefaultAIState(aIStatePVPGuard);
				SwitchFSM(aIStatePVPGuard);
				GetGameObject().layer = 11;
				base.hitInfo.damage = new NumberSection<float>(baseAttribute.damage.left * 0.2f, baseAttribute.damage.right * 0.2f);
				return;
			}
			base.isBetray = false;
			base.hp = betrayHpMark;
			GameBattle.m_instance.AddToTeammateList(this);
			if (DataCenter.Save().squadMode)
			{
				AIStateSquadGuard aIStateSquadGuard = GetAIState("SquadGuard") as AIStateSquadGuard;
				SetDefaultAIState(aIStateSquadGuard);
				SwitchFSM(aIStateSquadGuard);
			}
			else
			{
				AIStateAllyGuard aIStateAllyGuard = GetAIState("Guard") as AIStateAllyGuard;
				aIStateAllyGuard.SetGuard(GameBattle.m_instance.GetPlayer());
				SetDefaultAIState(aIStateAllyGuard);
				SwitchFSM(aIStateAllyGuard);
			}
			GetGameObject().layer = 10;
			base.hitInfo.damage = baseAttribute.damage;
		}

		public void SetGameData(PlayerData data, int teamLevel)
		{
			playerData = data;
			baseAttribute = default(CharacterAttribute);
			DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
			heroIndex = data.heroIndex;
			iconName = heroDataByIndex.iconFileName;
			m_fixedWeapon = true;
			float moveSpeed = heroDataByIndex.moveSpeed;
			DataConf.WeaponData weaponDataByType = DataCenter.Conf().GetWeaponDataByType(heroDataByIndex.weaponType);
			float num = heroDataByIndex.critDamage;
			ushort num2 = 0;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			ushort num9 = 0;
			foreach (UserEquipData value in playerData.equips.Values)
			{
				if (value != null)
				{
					DataConf.EquipData equipDataByIndex = DataCenter.Conf().GetEquipDataByIndex(value.currEquipIndex);
					num += equipDataByIndex.critDamagePercent;
					num9 += equipDataByIndex.proResilience;
					num2 += equipDataByIndex.proStab;
					num3 += equipDataByIndex.atkPercent;
					num4 += equipDataByIndex.atkFrequencyPercent;
					num5 += equipDataByIndex.reduceDamagePercent;
					num6 += equipDataByIndex.moveSpeedPercent;
					num7 += equipDataByIndex.atkRange;
				}
			}
			MoveSpeed = heroDataByIndex.moveSpeed + heroDataByIndex.moveSpeed * num6;
			originalMoveSpeed = (baseAttribute.moveSpeed = MoveSpeed);
			baseAttribute.hpMax = playerData.Hp;
			if (!DataCenter.Save().BattleTutorialFinished && !Util.s_debug)
			{
				baseAttribute.hpMax *= 300;
			}
			int num11 = (base.hpMax = baseAttribute.hpMax);
			base.hp = num11;
			base.reduceDamage = num5;
			baseAttribute.damage = new NumberSection<float>(playerData.Damage, playerData.Damage);
			m_def = playerData.Defense;
			base.critRate = (ushort)playerData.CritRate;
			m_critDamagePercent = num;
			baseAttribute.critDamage = baseAttribute.damage.left * num;
			base.critDamage = baseAttribute.critDamage;
			base.dodgeRate = (ushort)playerData.Dodge;
			base.hitRate = (ushort)playerData.Hit;
			m_proStab = num2;
			m_atkFrequencyPercent = num4;
			m_atkRange = num7;
			m_proResilience = num9;
			if (DataCenter.State().isPVPMode)
			{
				baseAttribute.hpMax = (int)((float)base.hpMax * ((m_gameLayer != 11) ? 3.75f : 5f));
				num11 = (base.hpMax = baseAttribute.hpMax);
				base.hp = num11;
			}
		}

		public void SetTeamAttributeData(TeamData teamData)
		{
			this.teamData = teamData;
			if (m_teamAttributeTalentList == null)
			{
				m_teamAttributeTalentList = new List<TeamSpecialAttribute.TeamAttributeType>();
			}
			if (m_teamAttributeEvolveList == null)
			{
				m_teamAttributeEvolveList = new List<TeamSpecialAttribute.TeamAttributeEvolveType>();
			}
			foreach (TeamSpecialAttribute.TeamAttributeType key in teamData.talents.Keys)
			{
				int num = teamData.talents[key];
				if (num > 0)
				{
					switch (key)
					{
					case TeamSpecialAttribute.TeamAttributeType.Reinforcement:
					{
						float l = baseAttribute.damage.left * (1f + DataCenter.Conf().m_teamAttributeReinforcement.damage[num - 1]);
						float r = baseAttribute.damage.right * (1f + DataCenter.Conf().m_teamAttributeReinforcement.damage[num - 1]);
						baseAttribute.damage = new NumberSection<float>(l, r);
						break;
					}
					case TeamSpecialAttribute.TeamAttributeType.UrgentTreatment:
					{
						float value = DataCenter.Conf().m_teamAttributeUrgentTreatment.hpPercent[num - 1];
						Buff buff = new Buff(Buff.AffectType.SpecialAttributeHp, value, DataCenter.Conf().m_teamAttributeUrgentTreatment.frequency, float.MaxValue, Buff.CalcType.Percentage, 0f);
						GetBuffManager().AddBuff(buff);
						break;
					}
					case TeamSpecialAttribute.TeamAttributeType.SpecialTraining:
					{
						baseAttribute.hpMax = (int)((float)baseAttribute.hpMax * (1f + DataCenter.Conf().m_teamAttributeSpecialTraining.hpMaxPercent[num - 1]));
						int num4 = (base.hpMax = baseAttribute.hpMax);
						base.hp = num4;
						break;
					}
					case TeamSpecialAttribute.TeamAttributeType.Armsmaster:
						m_clipPercent = DataCenter.Conf().m_teamAttributeArmsmaster.clipPercent[num - 1];
						m_clipCDTime = DataCenter.Conf().m_teamAttributeArmsmaster.cdPercent[num - 1];
						break;
					case TeamSpecialAttribute.TeamAttributeType.Biobomb:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.EFFECT_BOMB_1);
						break;
					case TeamSpecialAttribute.TeamAttributeType.LivingCells:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.EFFECT_ADD_HP);
						break;
					case TeamSpecialAttribute.TeamAttributeType.Mania:
						base.critRate = (ushort)(base.critRate * (1 + DataCenter.Conf().m_teamAttributeMania.proCrit[num - 1]));
						base.critDamage *= 1f + DataCenter.Conf().m_teamAttributeMania.critDamagePercent[num - 1];
						break;
					case TeamSpecialAttribute.TeamAttributeType.VolatileBomb:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.EFFECT_BOMB_1);
						break;
					case TeamSpecialAttribute.TeamAttributeType.ShadowWalk:
						base.dodgeRate = (ushort)(base.dodgeRate * (1 + DataCenter.Conf().m_teamAttributeShadowWalk.proDodge[num - 1]));
						break;
					case TeamSpecialAttribute.TeamAttributeType.AimedShot:
					{
						ushort num2 = DataCenter.Conf().m_teamAttributeAimedShot.proIgnoreDefence[num - 1];
						m_proStab += num2;
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.heores_Aimed_Shot);
						break;
					}
					case TeamSpecialAttribute.TeamAttributeType.Vampire:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.monsters_Vampire_0);
						break;
					case TeamSpecialAttribute.TeamAttributeType.Cooling:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.heores_Relentless_01);
						break;
					case TeamSpecialAttribute.TeamAttributeType.Executioner:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.heores_Focus);
						break;
					case TeamSpecialAttribute.TeamAttributeType.Resurrection:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.heores_Prayer);
						break;
					case TeamSpecialAttribute.TeamAttributeType.Splash:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.heores_Splash_01);
						break;
					}
					m_teamAttributeTalentList.Add(key);
				}
			}
			foreach (TeamSpecialAttribute.TeamAttributeEvolveType key2 in teamData.evolves.Keys)
			{
				int num5 = teamData.evolves[key2];
				if (num5 > 0)
				{
					switch (key2)
					{
					case TeamSpecialAttribute.TeamAttributeEvolveType.MagneticField:
						BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.EFFECT_ENERGYSHIELD_COVER, 5);
						break;
					}
					m_teamAttributeEvolveList.Add(key2);
				}
			}
		}

		public override HitInfo GetHitInfo()
		{
			if (base.skillAttack)
			{
				HitInfo hitInfo = new HitInfo(base.skillHitInfo);
				if (Util.s_cheatAddDamage > 1f)
				{
					hitInfo.damage = new NumberSection<float>(hitInfo.damage.right * Util.s_cheatAddDamage);
				}
				return hitInfo;
			}
			HitInfo hitInfo2 = new HitInfo(base.hitInfo);
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Stun))
			{
				ushort num = DataCenter.Conf().m_teamAttributeStun.proStun[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Stun] - 1];
				if (Random.Range(0, 100) < num)
				{
					float stunTime = DataCenter.Conf().m_teamAttributeStun.stunTime;
					SpecialHitInfo specialHitInfo = new SpecialHitInfo();
					specialHitInfo.time = stunTime;
					specialHitInfo.disposable = true;
					hitInfo2.AddSpecialHit(Defined.SPECIAL_HIT_TYPE.STUN, specialHitInfo);
				}
			}
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.RelentlessRage))
			{
				float num2 = DataCenter.Conf().m_teamAttributeRelentlessRage.attackPercent[teamData.talents[TeamSpecialAttribute.TeamAttributeType.RelentlessRage] - 1];
				float num3 = DataCenter.Conf().m_teamAttributeRelentlessRage.proCrit[teamData.talents[TeamSpecialAttribute.TeamAttributeType.RelentlessRage] - 1];
				HitInfo hitInfo3 = new HitInfo(hitInfo2);
				int num4 = m_weapon.m_iBulletCombo;
				if (num4 > 20)
				{
					num4 = 20;
				}
				float l = hitInfo3.damage.left + hitInfo3.damage.left * (float)num4 * num2;
				float r = hitInfo3.damage.right + hitInfo3.damage.right * (float)num4 * num2;
				hitInfo3.damage = new NumberSection<float>(l, r);
				hitInfo3.critRate += hitInfo3.critRate * (float)num4 * num3;
				return hitInfo3;
			}
			if (m_nextAttackCritTimes > 0)
			{
				m_nextAttackCritTimes--;
				HitInfo hitInfo4 = new HitInfo(hitInfo2);
				hitInfo4.critRate = 100f;
				return hitInfo4;
			}
			hitInfo2.critRate = baseAttribute.proCritical;
			if (Util.s_cheatAddDamage > 1f)
			{
				hitInfo2.damage = new NumberSection<float>(baseAttribute.damage.right * Util.s_cheatAddDamage);
				return hitInfo2;
			}
			return hitInfo2;
		}

		public override HitResultInfo GetHitDamage(HitInfo hitInfo)
		{
			DS2ActiveObject source = hitInfo.source;
			HitResultInfo hitDamage = base.GetHitDamage(hitInfo);
			if (m_shieldHp > 0)
			{
				if ((float)m_shieldHp > hitDamage.damage)
				{
					m_shieldHp -= (int)hitDamage.damage;
					EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Shield, (int)hitDamage.damage, m_effectPoint.position);
					hitDamage.damage = 0f;
				}
				else
				{
					EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Shield, m_shieldHp, m_effectPoint.position);
					hitDamage.damage -= m_shieldHp;
					if (m_shieldHpEffectCallBack != null)
					{
						m_shieldHpEffectCallBack.SetActive(false);
					}
				}
			}
			if (hitDamage.isHit)
			{
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.LinkedLives))
				{
					ushort num = DataCenter.Conf().m_teamAttributeLinkedLives.proShareDamage[teamData.talents[TeamSpecialAttribute.TeamAttributeType.LinkedLives] - 1];
					if (Random.Range(0, 100) < num)
					{
						DS2ActiveObject[] teammateList = GameBattle.m_instance.GetTeammateList(base.clique);
						int num2 = teammateList.Length;
						hitDamage.damage /= num2;
						for (int i = 0; i < teammateList.Length; i++)
						{
							if (teammateList[i] != this)
							{
								teammateList[i].hp -= (int)hitDamage.damage;
							}
						}
					}
				}
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.LivingCells))
				{
					ushort num3 = DataCenter.Conf().m_teamAttributeLivingCells.proResHp[teamData.talents[TeamSpecialAttribute.TeamAttributeType.LivingCells] - 1];
					if (Random.Range(0, 100) < num3)
					{
						BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.EFFECT_ADD_HP, GetTransform().position, 0f, GetTransform());
						base.hp += (int)((float)base.hpMax * DataCenter.Conf().m_teamAttributeLivingCells.resHpPercent);
					}
				}
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.VolatileBomb))
				{
					ushort num4 = DataCenter.Conf().m_teamAttributeVolatileBomb.proBomb[teamData.talents[TeamSpecialAttribute.TeamAttributeType.VolatileBomb] - 1];
					float time = DataCenter.Conf().m_teamAttributeVolatileBomb.stunTime[teamData.talents[TeamSpecialAttribute.TeamAttributeType.VolatileBomb] - 1];
					if (Random.Range(0, 100) < num4)
					{
						source.OnDeath();
						SpecialHitInfo specialHitInfo = new SpecialHitInfo();
						specialHitInfo.repelDistance = new NumberSection<float>(3f, 4f);
						specialHitInfo.time = time;
						specialHitInfo.disposable = true;
						base.hitInfo.AddSpecialHit(Defined.SPECIAL_HIT_TYPE.STUN, specialHitInfo);
						BattleBufferManager.Instance.GenerateBomb(source.GetTransform().position, 3.5f, this);
					}
				}
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.ForceField))
				{
					ushort num5 = DataCenter.Conf().m_teamAttributeForceField.proReduceDamage[teamData.talents[TeamSpecialAttribute.TeamAttributeType.ForceField] - 1];
					float reduceDamagePercent = DataCenter.Conf().m_teamAttributeForceField.reduceDamagePercent;
					if (Random.Range(0, 100) < num5)
					{
						hitDamage.damage -= hitDamage.damage * reduceDamagePercent;
					}
				}
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Prayer))
				{
					float num6 = DataCenter.Conf().m_teamAttributePrayer.reduceDamagePercent[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Prayer] - 1];
					hitDamage.damage -= hitDamage.damage * num6;
				}
				if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.MagneticField))
				{
					TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.MagneticField);
					float num7 = teamAttributeEvolveData.probability[0];
					if (teamAttributeEvolveData.probability.Length > 1)
					{
						num7 = teamAttributeEvolveData.probability[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.MagneticField] - 1];
					}
					int max = 1000;
					num7 *= 10f;
					if ((float)Random.Range(0, max) < num7)
					{
						float num8 = teamAttributeEvolveData.percent[0];
						if (teamAttributeEvolveData.percent.Length > 1)
						{
							num8 = teamAttributeEvolveData.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.MagneticField] - 1];
						}
						m_shieldHp = (int)((float)base.hpMax * num8);
						if (m_shieldHpEffectCallBack != null)
						{
							m_shieldHpEffectCallBack.SetActive(false);
						}
						m_shieldHpEffectCallBack = BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.EFFECT_ENERGYSHIELD_COVER, m_effectPoint.position, -1f, m_effectPoint);
					}
				}
				if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.StrikeBack))
				{
					TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData2 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.StrikeBack);
					float num9 = teamAttributeEvolveData2.probability[0];
					if (teamAttributeEvolveData2.probability.Length > 1)
					{
						num9 = teamAttributeEvolveData2.probability[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.StrikeBack] - 1];
					}
					int max2 = 1000;
					num9 *= 10f;
					if ((float)Random.Range(0, max2) < num9)
					{
						HitInfo hitInfo2 = new HitInfo();
						float num10 = teamAttributeEvolveData2.percent[0];
						if (teamAttributeEvolveData2.percent.Length > 1)
						{
							num10 = teamAttributeEvolveData2.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.StrikeBack] - 1];
						}
						hitInfo2.damage = new NumberSection<float>(hitDamage.damage * num10);
						source.OnHit(hitInfo2);
						hitDamage.damage = 0f;
					}
				}
				if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Blessed))
				{
					TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData3 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Blessed);
					float num11 = teamAttributeEvolveData3.probability[0];
					if (teamAttributeEvolveData3.probability.Length > 1)
					{
						num11 = teamAttributeEvolveData3.probability[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Blessed] - 1];
					}
					int max3 = 1000;
					num11 *= 10f;
					if ((float)Random.Range(0, max3) < num11)
					{
						hitDamage.damage = 0f;
					}
				}
				if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Stealth))
				{
					TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData4 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Stealth);
					float num12 = teamAttributeEvolveData4.probability[0];
					if (teamAttributeEvolveData4.probability.Length > 1)
					{
						num12 = teamAttributeEvolveData4.probability[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Stealth] - 1];
					}
					int max4 = 100;
					if ((float)Random.Range(0, max4) < num12)
					{
						float num13 = teamAttributeEvolveData4.percent[0];
						if (teamAttributeEvolveData4.percent.Length > 1)
						{
							num13 = teamAttributeEvolveData4.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Stealth] - 1];
						}
						float time2 = teamAttributeEvolveData4.time;
						IBuffManager buffManager = source.GetBuffManager();
						if (buffManager != null)
						{
							Buff buff = new Buff(Buff.AffectType.MoveSpeed, 0f - num13, time2, time2, Buff.CalcType.Percentage, 0f);
							buffManager.AddBuff(buff);
							Buff buff2 = new Buff(Buff.AffectType.AtkPrequency, num13, time2, time2, Buff.CalcType.Percentage, 0f);
							buffManager.AddBuff(buff2);
						}
					}
				}
			}
			return hitDamage;
		}

		public override void HitResult(HitResultInfo result)
		{
			base.HitResult(result);
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Freeze))
			{
				ushort num = DataCenter.Conf().m_teamAttributeFreeze.proReduceMoveSpeedAndAttack[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Freeze] - 1];
				if (Random.Range(0, 100) < num)
				{
					float reduceMoveSpeedAndAttackPercent = DataCenter.Conf().m_teamAttributeFreeze.reduceMoveSpeedAndAttackPercent;
					float time = DataCenter.Conf().m_teamAttributeFreeze.time;
					IBuffManager buffManager = result.target.GetBuffManager();
					if (buffManager != null)
					{
						Buff buff = new Buff(Buff.AffectType.MoveSpeed, 0f - reduceMoveSpeedAndAttackPercent, time, time, Buff.CalcType.Percentage, 0f);
						buffManager.AddBuff(buff);
						Buff buff2 = new Buff(Buff.AffectType.ReduceATK, reduceMoveSpeedAndAttackPercent, time, time, Buff.CalcType.Percentage, 0f);
						buffManager.AddBuff(buff2);
					}
				}
			}
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Splash))
			{
				ushort num2 = DataCenter.Conf().m_teamAttributeSplash.probability[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Splash] - 1];
				if (Random.Range(0, 100) < num2)
				{
					if (result.target != null)
					{
						BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.heores_Splash_01, result.target.m_effectPoint.position, 0f, result.target.m_effectPoint);
					}
					int layerMask = ((base.clique != Clique.Computer) ? 2048 : 67636736);
					Collider[] array = Physics.OverlapSphere(GetTransform().position, radius, layerMask);
					Collider[] array2 = array;
					foreach (Collider collider in array2)
					{
						DS2ActiveObject @object = DS2ObjectStub.GetObject<DS2ActiveObject>(collider.gameObject);
						HitInfo hitInfo = new HitInfo();
						hitInfo.damage = new NumberSection<float>(GetHitInfo().damage.right * 0.25f);
						hitInfo.repelDirection = @object.GetTransform().position - GetTransform().position;
						@object.OnHit(hitInfo);
					}
				}
			}
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Vampire))
			{
				ushort num3 = DataCenter.Conf().m_teamAttributeVampire.proResHp[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Vampire] - 1];
				if (Random.Range(0, 100) < num3)
				{
					float resHpPercent = DataCenter.Conf().m_teamAttributeVampire.resHpPercent;
					int num4 = (int)(result.damage * resHpPercent);
					base.hp += num4;
					EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Heal, num4, m_effectPoint.position);
					BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.monsters_Vampire_0, m_effectPoint.position, 0f, m_effectPoint);
				}
			}
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Executioner))
			{
				float num5 = DataCenter.Conf().m_teamAttributeExecutioner.probability[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Executioner] - 1];
				int num6 = (int)(num5 * 10f);
				if ((float)Random.Range(0, 1000) < num5)
				{
					m_nextAttackCritTimes = DataCenter.Conf().m_teamAttributeExecutioner.critTimes;
					BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.heores_Focus, m_effectPointUpHead.position, 0f, m_effectPointUpHead);
				}
			}
			if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Biobomb))
			{
				ushort num7 = DataCenter.Conf().m_teamAttributeBiobomb.proBomb[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Biobomb] - 1];
				if (Random.Range(0, 100) < num7)
				{
					BattleBufferManager.Instance.GenerateBomb(result.target.GetTransform().position, 2.5f, this);
				}
			}
			if (result.target.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER && HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Broken))
			{
				TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Broken);
				float num8 = teamAttributeEvolveData.probability[0];
				if (teamAttributeEvolveData.probability.Length > 1)
				{
					num8 = teamAttributeEvolveData.probability[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Broken] - 1];
				}
				int max = 1000;
				num8 *= 10f;
				if ((float)Random.Range(0, max) < num8)
				{
					IBuffManager buffManager2 = result.target.GetBuffManager();
					if (buffManager2 != null)
					{
						float num9 = teamAttributeEvolveData.percent[0];
						if (teamAttributeEvolveData.percent.Length > 1)
						{
							num9 = teamAttributeEvolveData.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Broken] - 1];
						}
						Buff buff3 = new Buff(Buff.AffectType.Defense, 0f - num9, teamAttributeEvolveData.time, teamAttributeEvolveData.time, Buff.CalcType.Percentage, 0f);
						buffManager2.AddBuff(buff3);
					}
				}
			}
			if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Weak))
			{
				TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData2 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Weak);
				float num10 = teamAttributeEvolveData2.probability[0];
				int max2 = 1000;
				num10 *= 10f;
				if ((float)Random.Range(0, max2) < num10)
				{
					IBuffManager buffManager3 = result.target.GetBuffManager();
					if (buffManager3 != null)
					{
						float value = teamAttributeEvolveData2.percent[0];
						if (teamAttributeEvolveData2.percent.Length > 1)
						{
							value = teamAttributeEvolveData2.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Weak] - 1];
						}
						Buff buff4 = new Buff(Buff.AffectType.ReduceATK, value, teamAttributeEvolveData2.time, teamAttributeEvolveData2.time, Buff.CalcType.Percentage, 0f);
						buffManager3.AddBuff(buff4);
					}
				}
			}
			if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Shortsightedness))
			{
				TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData3 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Shortsightedness);
				float num11 = teamAttributeEvolveData3.probability[0];
				int max3 = 1000;
				num11 *= 10f;
				if ((float)Random.Range(0, max3) < num11)
				{
					IBuffManager buffManager4 = result.target.GetBuffManager();
					if (buffManager4 != null)
					{
						float num12 = teamAttributeEvolveData3.percent[0];
						if (teamAttributeEvolveData3.percent.Length > 1)
						{
							num12 = teamAttributeEvolveData3.percent[teamData.evolves[TeamSpecialAttribute.TeamAttributeEvolveType.Shortsightedness] - 1];
						}
						Buff buff5 = new Buff(Buff.AffectType.ShootRange, 0f - num12, teamAttributeEvolveData3.time, teamAttributeEvolveData3.time, Buff.CalcType.Percentage, 0f);
						buffManager4.AddBuff(buff5);
					}
				}
			}
			if (HasEvolve(TeamSpecialAttribute.TeamAttributeEvolveType.Dumdum))
			{
				TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData4 = DataCenter.Conf().GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType.Dumdum);
				float num13 = teamAttributeEvolveData4.probability[0];
				int max4 = 1000;
				num13 *= 10f;
				if ((float)Random.Range(0, max4) < num13)
				{
					HitInfo hitInfo2 = new HitInfo();
					hitInfo2.damage = new NumberSection<float>(result.damage);
					result.target.OnHit(hitInfo2);
					result.target.OnHit(hitInfo2);
				}
			}
			if (!result.isCirt)
			{
				return;
			}
			if (SkillInCDTime() && HasTalent(TeamSpecialAttribute.TeamAttributeType.Cooling))
			{
				ushort num14 = DataCenter.Conf().m_teamAttributeCooling.proNoCd[teamData.talents[TeamSpecialAttribute.TeamAttributeType.Cooling] - 1];
				if (Random.Range(0, 100) < num14)
				{
					m_skillCDTime -= 1f;
					m_skillCDTime = Mathf.Max(m_skillCDTime, 0f);
					BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.heores_Relentless_01, m_effectPointUpHead.position, 0f, m_effectPointUpHead);
				}
			}
			if ((result.target.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || result.target.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY) && HasTalent(TeamSpecialAttribute.TeamAttributeType.SealingTechnique))
			{
				ushort num15 = DataCenter.Conf().m_teamAttributeSealingTechnique.proCd[teamData.talents[TeamSpecialAttribute.TeamAttributeType.SealingTechnique] - 1];
				if (Random.Range(0, 100) < num15)
				{
					Player player = (Player)result.target;
					player.FillCDTime();
				}
			}
		}

		public override void SetMove(bool move, Vector3 moveDirection)
		{
			base.SetMove(move, moveDirection);
			m_moveCopy.transform.forward = moveDirection;
		}

		public override void UseWeapon(int index)
		{
			if (m_weapon != null)
			{
				m_weapon.SetActive(false);
				m_weapon = null;
			}
			if (m_weaponMap.ContainsKey(index))
			{
				m_weaponIndex = index;
				m_weapon = m_weaponMap[index];
				m_weapon.SetActive(true);
				base.shootRange = m_weapon.attribute.attackRange;
				if (DataCenter.State().isPVPMode && m_gameLayer == 11)
				{
					base.shootRange *= 1.2f;
				}
				base.shootRange += base.shootRange * m_atkRange;
				m_weapon.attribute.fireFrequency = m_weapon.attribute.fireFrequency + m_weapon.attribute.fireFrequency * m_atkFrequencyPercent;
				base.hitInfo.hitStrength = m_weapon.attribute.hitStrength;
				base.hitInfo.repelDistance = m_weapon.attribute.repelDis;
				if (HasTalent(TeamSpecialAttribute.TeamAttributeType.Armsmaster))
				{
					m_weapon.attribute.clip = (int)((float)m_weapon.attribute.clip * (1f + m_clipPercent));
					m_weapon.attribute.reloadTime = m_weapon.attribute.reloadTime * (1f + m_clipCDTime);
					m_weapon.Reset();
				}
				AIStateReload aIStateReload = GetAIState("Reload") as AIStateReload;
				string animationName = GetAnimationName("Reload");
				if (m_gameObject.GetComponent<Animation>()[animationName] != null)
				{
					aIStateReload.animSpeed = m_gameObject.GetComponent<Animation>()[animationName].length;
				}
				AIStateSquadGuard aIStateSquadGuard = GetAIState("SquadGuard") as AIStateSquadGuard;
				if (m_gameObject.GetComponent<Animation>()[animationName] != null)
				{
					aIStateSquadGuard.ReloadAnimSpeed = m_gameObject.GetComponent<Animation>()[animationName].length;
				}
				UpdateWeaponAnimation();
				SetAnimationsAttribute();
			}
		}

		private void UpdateChangeToAutoControl()
		{
			if (!m_bChangeToControl)
			{
				return;
			}
			m_fChangeToControlTimer += Time.deltaTime;
			if (!(m_fChangeToControlTimer >= 1f) || m_weapon.NeedReload() || base.skillAttack)
			{
				return;
			}
			m_bChangeToControl = false;
			m_fChangeToControlTimer = 0f;
			if (DataCenter.State().isPVPMode)
			{
				AIState aIState = GetAIState("GuardPVP");
				SetDefaultAIState(aIState);
				SwitchFSM(aIState);
				if (m_moveKeyDown)
				{
					HalfAutoControl = true;
					AutoControl = false;
				}
				else
				{
					HalfAutoControl = false;
					AutoControl = true;
				}
			}
			else
			{
				AIState aIState2 = GetAIState("AutoControl");
				SetDefaultAIState(aIState2);
				SwitchFSM(aIState2);
				if (m_moveKeyDown)
				{
					HalfAutoControl = true;
					AutoControl = false;
				}
				else
				{
					HalfAutoControl = false;
					AutoControl = true;
				}
			}
		}

		public override void UpdateAnimationSpeed()
		{
			float num = m_moveSpeed / 6f;
			string animationName = GetAnimationName("Lower_Body_Run_F");
			SetAnimationSpeed(animationName, num);
			string animationName2 = GetAnimationName("Lower_Body_Run_B");
			SetAnimationSpeed(animationName2, num);
			string animationName3 = GetAnimationName("Lower_Body_Run_L");
			SetAnimationSpeed(animationName3, num);
			string animationName4 = GetAnimationName("Lower_Body_Run_R");
			SetAnimationSpeed(animationName4, num);
		}

		public void SetAnimationsAttribute()
		{
		}

		public void SetHalo(bool enable = true)
		{
			if (haloGameObjectAlly == null)
			{
				haloGameObjectAlly = Object.Instantiate(Resources.Load("Models/Characters/LeaderHaloAlly") as GameObject) as GameObject;
				haloGameObjectAlly.name = "LeaderHaloAlly";
				haloGameObjectAlly.transform.parent = GetTransform();
				haloGameObjectAlly.transform.localRotation = Quaternion.identity;
				haloGameObjectAlly.transform.localPosition = Vector3.zero;
				haloGameObjectAlly.transform.localScale = Vector3.one;
			}
			if (haloGameObjectLeader == null)
			{
				haloGameObjectLeader = Object.Instantiate(Resources.Load("Models/Characters/LeaderHalo") as GameObject) as GameObject;
				haloGameObjectLeader.name = "LeaderHaloLeader";
				haloGameObjectLeader.transform.parent = GetTransform();
				haloGameObjectLeader.transform.localRotation = Quaternion.identity;
				haloGameObjectLeader.transform.localPosition = Vector3.zero;
				haloGameObjectLeader.transform.localScale = Vector3.one;
			}
			if (CurrentController)
			{
				haloGameObjectLeader.SetActive(enable);
				haloGameObjectAlly.SetActive(false);
			}
			else
			{
				haloGameObjectAlly.SetActive(enable);
				haloGameObjectLeader.SetActive(false);
			}
		}

		public void SetSquadMode(bool enable)
		{
			if (enable)
			{
				CurrentController = false;
				MoveSpeed = (baseAttribute.moveSpeed = GameBattle.m_instance.GetSquadController().OriginalSpeed);
				return;
			}
			MoveSpeed = (baseAttribute.moveSpeed = originalMoveSpeed);
			if (Alive() || willResurgence)
			{
				GetNavMeshAgent().Resume();
				if (SquadSite == 0)
				{
					CurrentController = true;
					GameBattle.m_instance.ChangeCurrentContorlPlayer(this);
				}
				else
				{
					CurrentController = false;
				}
			}
		}

		public override void UseSkill(int skillId)
		{
			if (base.isStuck || SkillInCDTime())
			{
				return;
			}
			if (m_skillNeedFindTarget && !DataCenter.Save().squadMode && (!DataCenter.Save().squadMode || SquadSite != 0) && !CurrentController)
			{
				UseSkillNeedFindTarget(skillId);
				return;
			}
			audioTalkManager.PlaySkill();
			if (GameBattle.m_instance != null)
			{
				m_skillCDTime = m_skillTotalCDTime;
			}
			if (DataCenter.Save().m_bManualUseSkill && GameBattle.m_instance != null)
			{
				GameBattle.m_instance.SkillCommonnalityCDTime = GameBattle.m_instance.SkillCommonnalityTotalCDTime;
			}
			base.UseSkill(skillId);
			if (HasNavigation())
			{
				StopNav(false);
			}
			AIState currentAIState = GetCurrentAIState();
			if (currentAIState.name != "SkillReady" && currentAIState.name != "Skill")
			{
				SwitchFSM(GetAIState("Skill"));
			}
		}

		public virtual void UseSkillNeedFindTarget(int skillId)
		{
			AIState currentAIState = GetCurrentAIState();
			if (currentAIState.name != "SkillFindTarget")
			{
				if (!base.isStuck && !SkillInCDTime())
				{
					audioTalkManager.PlaySkill();
					if (DataCenter.Save().m_bManualUseSkill)
					{
						GameBattle.m_instance.SkillCommonnalityCDTime = GameBattle.m_instance.SkillCommonnalityTotalCDTime;
					}
					AIStateSkillFindTarget state = GetAIState("SkillFindTarget") as AIStateSkillFindTarget;
					SwitchFSM(state);
				}
				return;
			}
			m_skillCDTime = m_skillTotalCDTime;
			SkillStart();
			if (HasNavigation())
			{
				StopNav(false);
			}
			if (currentAIState.name != "SkillReady" && currentAIState.name != "Skill")
			{
				effectPlayManager.PlayEffect("Glitter");
				ChangeAIState("Skill");
			}
		}

		public virtual void UpdateSkillOnAIControl()
		{
			if (DataCenter.State().isPVPMode)
			{
				if (DataCenter.Save().m_bManualUseSkill || CurrentController)
				{
					return;
				}
			}
			else if (DataCenter.Save().m_bManualUseSkill || CurrentController)
			{
				return;
			}
			if (!Alive())
			{
				return;
			}
			if (m_checkSkillConditionTimer < m_checkSkillConditionInterval)
			{
				m_checkSkillConditionTimer += Time.deltaTime;
				return;
			}
			m_checkSkillConditionTimer = 0f;
			if (CheckSkillConditions())
			{
				UseSkill(0);
			}
		}

		public override void SkillStart()
		{
			base.SkillStart();
			SetGodTime(999999f);
		}

		public override void SkillEnd()
		{
			base.SkillEnd();
			SetGodTime(0f);
		}
	}
}

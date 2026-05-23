using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoMDS2
{
	public class Enemy : NPC
	{
		public enum EnemyType
		{
			Zombie = 0,
			ZombieBomb = 1,
			ZombieNurse = 2,
			Haoke = 3,
			Wrestler = 4,
			FatCook = 5,
			PolicemanPistol = 6,
			PolicemanShotgun = 7,
			Pestilence = 8,
			Cowboy = 9,
			Hook = 10,
			DeadLight = 11,
			Spore = 12,
			GuterTrennung = 13,
			Butcher = 14,
			Blaze = 15,
			PestilenceJar = 16,
			ZombieNurse_Purple = 17,
			Haoke_Purple = 18,
			FatCook_Purple = 19,
			Pestilence_Purple = 20,
			Cowboy_Purple = 21,
			DeadLight_Purple = 22,
			Spore_Purple = 23,
			Butcher_Purple = 24,
			Blaze_Purple = 25,
			PestilenceJar_Purple = 26,
			Zombie_Purple = 27,
			ZombieBomb_Purple = 28
		}

		public enum EnemyEliteType
		{
			None = 0,
			Rank1 = 1,
			Rank2 = 2,
			Rank3 = 3
		}

		public class SpawnInfo
		{
			public EnemySpawnPoint spawnPoint;

			public int spawnWave;
		}

		public enum ReleaseSkillState
		{
			Ready = 0,
			Release = 1,
			End = 2,
			Over = 3
		}

		public int proResurge;

		public string enemyAnimTag;

		private int m_hpNormal;

		private NumberSection<float> m_damageNormal;

		private float m_speedNormal;

		protected List<Material> m_materials;

		protected Shader m_shaderDiffuse;

		protected Shader m_shaderCOL;

		protected Shader m_shaderModelEdge;

		public List<SpecialAttribute.AttributeType> specialAttribute;

		public List<int> m_specialAttributeCreateId;

		private Dictionary<int, SpecialAttribute.SpecialAttributeGameData> m_recieveSpecialAttribute;

		private Dictionary<SpecialAttribute.AttributeType, float> m_specialAttrTimer;

		private Dictionary<SpecialAttribute.AttributeType, float> m_specialAttrTimer2;

		private Dictionary<int, float> m_recieveSpecialAttrTimer;

		private Dictionary<SpecialAttribute.AttributeType, int> m_recieveSpecialAttributeCount;

		private List<int> m_removeSpeAttrId;

		private Dictionary<SpecialAttribute.SpecialAttributeEffectType, DS2HalfStaticObject> m_specialEffects;

		private DS2ObjectBuffer m_specAttrBullutBuffer;

		private float eliteScale = 1.1f;

		private float bossScale = 1.1f;

		protected float m_baseMeleeRange;

		private float m_resHpTimer;

		protected int m_usedSkillCount;

		protected float m_useSkillCD;

		public DS2ActiveObject elegyTarget;

		protected float m_attFrequency;

		protected float m_proFirstAttackLose;

		protected UISlider m_hpBar;

		private float m_showHpBarTimer;

		public bool isBig;

		protected ReleaseSkillState m_releaseSkillState;

		protected float m_skillTimer;

		protected float m_shootAbleTimer;

		protected Vector3 m_skillTargetPos = Vector3.zero;

		protected Vector3 m_skillMoveDir = Vector3.zero;

		protected DS2ActiveObject m_skillTarget;

		protected float m_skillTargetHorizDis;

		protected float m_attackTimer;

		protected string m_attackAnimName;

		protected string m_waitAttackAnimName;

		public SpawnInfo spawnInfo { get; set; }

		public EnemyType enemyType { get; set; }

		public string enemyName { get; set; }

		public bool isBoss { get; set; }

		public EnemyEliteType eliteType { get; set; }

		public override float AtkFrequency
		{
			get
			{
				return m_attFrequency;
			}
			set
			{
				m_attFrequency = value;
			}
		}

		public override float MoveSpeed
		{
			get
			{
				return m_moveSpeed;
			}
			set
			{
				LastMoveSpeed = m_moveSpeed;
				m_moveSpeed = value;
				m_moveSpeed = Mathf.Max(m_moveSpeed, 0f);
				if (HasNavigation())
				{
					SetNavSpeed(m_moveSpeed);
				}
				UpdateAnimationSpeed(isBoss || eliteType != EnemyEliteType.None);
			}
		}

		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
				if (m_hpBar != null)
				{
					m_hpBar.gameObject.SetActive(base.Visible);
				}
			}
		}

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			m_usedSkillCount = 0;
			m_useSkillCD = UnityEngine.Random.Range(DataConf.s_enemySkillCdTime.left, DataConf.s_enemySkillCdTime.right);
			SetAttribute();
			SetAnimationsMixing();
			m_materials = new List<Material>();
			Renderer[] componentsInChildren = m_gameObject.GetComponentsInChildren<Renderer>(false);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				m_materials.AddRange(componentsInChildren[i].materials);
			}
			m_shaderDiffuse = Shader.Find("Triniti/Character/COL");
			m_shaderModelEdge = Shader.Find("Triniti/Model/ModelEdge");
			AIStateIdle aIStateIdle = new AIStateIdle(this, "Idle");
			AIStateGuard aIStateGuard = new AIStateGuard(this, "Guard");
			AIStateChase aIStateChase = new AIStateChase(this, "Chase");
			AIStateHurt aIStateHurt = new AIStateHurt(this, "Hurt");
			AIStateRepel aIStateRepel = new AIStateRepel(this, "Repel");
			AIStateBorn aIStateBorn = new AIStateBorn(this, "Born");
			AIStateDeath aIStateDeath = new AIStateDeath(this, "Death");
			AIStateStun aIStateStun = new AIStateStun(this, "Stun");
			AIStateFrozen aIStateFrozen = new AIStateFrozen(this, "Frozen");
			aIStateGuard.SetGuard(10f);
			aIStateChase.SetChase(1000f, baseAttribute.moveSpeed);
			string text = (aIStateChase.animName = GetAnimationName("Move"));
			base.animLowerBody = text;
			ClearAIState();
			AddAIState(aIStateIdle.name, aIStateIdle);
			AddAIState(aIStateGuard.name, aIStateGuard);
			AddAIState(aIStateChase.name, aIStateChase);
			AddAIState(aIStateHurt.name, aIStateHurt);
			AddAIState(aIStateRepel.name, aIStateRepel);
			AddAIState(aIStateDeath.name, aIStateDeath);
			AddAIState(aIStateBorn.name, aIStateBorn);
			AddAIState(aIStateStun.name, aIStateStun);
			AddAIState(aIStateFrozen.name, aIStateFrozen);
			SetDefaultAIState(aIStateChase);
			SwitchFSM(aIStateBorn);
			base.meleeAble = true;
			base.shootAble = false;
			base.hitRate = 100;
			base.dodgeRate = 0;
			spawnInfo = new SpawnInfo();
			specialAttribute = new List<SpecialAttribute.AttributeType>();
			m_recieveSpecialAttribute = new Dictionary<int, SpecialAttribute.SpecialAttributeGameData>();
			m_specialAttributeCreateId = new List<int>();
			m_removeSpeAttrId = new List<int>();
			m_removeSpeAttrId.Clear();
			m_specialEffects = new Dictionary<SpecialAttribute.SpecialAttributeEffectType, DS2HalfStaticObject>();
			m_specialEffects.Clear();
			m_recieveSpecialAttributeCount = new Dictionary<SpecialAttribute.AttributeType, int>();
			m_recieveSpecialAttributeCount.Clear();
			m_specialAttrTimer = new Dictionary<SpecialAttribute.AttributeType, float>();
			m_specialAttrTimer2 = new Dictionary<SpecialAttribute.AttributeType, float>();
			m_recieveSpecialAttrTimer = new Dictionary<int, float>();
		}

		public void Clone(Enemy enemy)
		{
			isBoss = enemy.isBoss;
			eliteType = enemy.eliteType;
			enemyType = enemy.enemyType;
			GameObject gameObject = UnityEngine.Object.Instantiate(enemy.GetGameObject()) as GameObject;
			Initialize(gameObject);
			m_AIStateMap = enemy.m_AIStateMap;
			SetDefaultAIState(enemy.GetDefaultAIState());
			SwitchFSM(enemy.GetDefaultAIState());
			baseAttribute = enemy.baseAttribute;
			base.hitInfo = new HitInfo(enemy.hitInfo);
			base.skillHitInfo = new HitInfo(enemy.skillHitInfo);
			base.hp = enemy.hp;
			base.hpMax = enemy.hpMax;
			m_attFrequency = enemy.baseAttribute.attackFrequence;
			base.meleeAble = enemy.meleeAble;
			base.shootAble = enemy.shootAble;
			base.hitRate = enemy.hitRate;
			base.dodgeRate = enemy.dodgeRate;
			spawnInfo = new SpawnInfo();
			specialAttribute = new List<SpecialAttribute.AttributeType>();
			m_recieveSpecialAttribute = new Dictionary<int, SpecialAttribute.SpecialAttributeGameData>();
			m_specialAttributeCreateId = new List<int>();
			m_removeSpeAttrId = new List<int>();
			m_removeSpeAttrId.Clear();
			m_specialEffects = new Dictionary<SpecialAttribute.SpecialAttributeEffectType, DS2HalfStaticObject>();
			m_specialEffects.Clear();
			m_recieveSpecialAttributeCount = new Dictionary<SpecialAttribute.AttributeType, int>();
			m_recieveSpecialAttributeCount.Clear();
			m_specialAttrTimer = new Dictionary<SpecialAttribute.AttributeType, float>();
			m_specialAttrTimer2 = new Dictionary<SpecialAttribute.AttributeType, float>();
			m_recieveSpecialAttrTimer = new Dictionary<int, float>();
		}

		protected override void SetAnimationsMixing()
		{
			if (enemyAnimTag == null || enemyAnimTag == string.Empty)
			{
				return;
			}
			Transform mix = m_transform.Find("Bip01/Bip01 Spine");
			if (enemyType == EnemyType.ZombieBomb)
			{
				mix = m_transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine");
			}
			DataConf.AnimData enemyAnim = DataCenter.Conf().GetEnemyAnim(enemyAnimTag, "Hurt");
			if (enemyAnim.count > 1)
			{
				for (int i = 0; i < enemyAnim.count; i++)
				{
					m_gameObject.GetComponent<Animation>()[enemyAnim.name + "0" + (i + 1)].layer = 2;
					m_gameObject.GetComponent<Animation>()[enemyAnim.name + "0" + (i + 1)].AddMixingTransform(mix);
				}
			}
		}

		public virtual void SetEnemy(Vector3 position, Quaternion rotation)
		{
			GetTransform().position = position;
			GetModelTransform().rotation = rotation;
		}

		public override void Update(float deltaTime)
		{
			AIState currentAIState = GetCurrentAIState();
			base.Update(deltaTime);
			updateSkills();
			ReceiveSpecailAttributeFromCentre();
			UpdateSpecialAttribute();
			if (!Alive())
			{
				return;
			}
			if (m_hpBar != null)
			{
				if (m_showHpBarTimer < 2f)
				{
					m_showHpBarTimer += deltaTime;
					m_hpBar.value = (float)base.hp / (float)base.hpMax;
					Vector3 p = GameBattle.m_instance.GetCamera().WorldToScreenPoint(m_effectPointUpHead.position);
					p = Util.ScreenPointToNGUI(p);
					m_hpBar.transform.localPosition = p;
					if (m_showHpBarTimer >= 2f)
					{
						m_hpBar.gameObject.SetActive(false);
					}
				}
			}
			else if (currentAIState.name != "Death")
			{
				OnDeath();
			}
		}

		public override void OnDeath()
		{
			GameBattle.m_instance.KillEnemiesCount++;
			AchievementTracker.OnZombieKilled();
			if (HasNavigation())
			{
				StopNav();
				GetNavMeshAgent().enabled = false;
			}
			base.audioManager.PlayAudio("Death");
			base.OnDeath();
			base.currentSkill = null;
			base.isRage = false;
			DoSpecialAttributeOnDeath();
			specialAttribute.Clear();
			specialAttribute.Clear();
			m_recieveSpecialAttribute.Clear();
			m_specialAttributeCreateId.Clear();
			m_removeSpeAttrId.Clear();
			m_recieveSpecialAttributeCount.Clear();
			m_specialAttrTimer.Clear();
			m_specialAttrTimer2.Clear();
			m_recieveSpecialAttrTimer.Clear();
			elegyTarget = null;
			foreach (DS2HalfStaticObject value in m_specialEffects.Values)
			{
				EffectPlayCombie component = value.GetGameObject().GetComponent<EffectPlayCombie>();
				if (component != null)
				{
					component.StopEmit();
				}
			}
			m_specialEffects.Clear();
			if (m_hpBar != null)
			{
				m_hpBar.gameObject.SetActive(false);
			}
			if (!DataCenter.Save().BattleTutorialFinished)
			{
				return;
			}
			DataConf.EnemyDropData dropData = DataCenter.Conf().GetEnemyDataByType(enemyType).dropData;
			int num = UnityEngine.Random.Range(0, 100);
			if (num >= dropData.probability)
			{
				return;
			}
			int num2 = 1;
			int num3 = (int)((float)dropData.money * (1f + (float)m_level * dropData.increase));
			if (eliteType == EnemyEliteType.None)
			{
				if (num < dropData.eruptProbability)
				{
					num2 = UnityEngine.Random.Range(3, 6);
				}
				else if (num < dropData.probability)
				{
					num2 = 1;
				}
			}
			else
			{
				num2 = UnityEngine.Random.Range(6, 13);
			}
			float num4 = 360 / num2;
			for (int i = 0; i < num2; i++)
			{
				DS2StaticObject dS2StaticObject = BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.Gold) as DS2StaticObject;
				if (dS2StaticObject != null)
				{
					dS2StaticObject.GetTransform().position = m_effectPoint.position;
					dS2StaticObject.GetGameObject().SetActive(true);
					Gold component2 = dS2StaticObject.GetGameObject().GetComponent<Gold>();
					component2.money = num3;
					if (num3 >= 100)
					{
						dS2StaticObject.GetTransform().localScale = new Vector3(1.3f, 1.3f, 1.3f);
					}
					else if (num3 >= 30 && num3 < 100)
					{
						dS2StaticObject.GetTransform().localScale = new Vector3(1f, 1f, 1f);
					}
					else
					{
						dS2StaticObject.GetTransform().localScale = new Vector3(0.7f, 0.7f, 0.7f);
					}
					GameBattle.m_instance.getMoneyInBattle += num3;
					Vector3 vector = new Vector3(Mathf.Cos(num4 * (float)i * ((float)Math.PI / 180f)), 300f, Mathf.Sin(num4 * (float)i * ((float)Math.PI / 180f)));
					int num5 = UnityEngine.Random.Range(20, 50);
					int num6 = 300 + i * 10;
					component2.IdleTime = 5f * (1f + (float)i * 10f / 300f);
					vector = new Vector3(vector.x * (float)num5, num6, vector.z * (float)num5);
					component2.Emit(vector);
				}
			}
		}

		public override void OnDeathOver()
		{
			if (isBoss)
			{
				bool bGameStateToWin = true;
				if (links != null && links.ContainsKey(Defined.ObjectLinkType.Cloned))
				{
					for (int i = 0; i < links[Defined.ObjectLinkType.Cloned].Count; i++)
					{
						if (links[Defined.ObjectLinkType.Cloned][i].Alive())
						{
							bGameStateToWin = false;
							break;
						}
					}
				}
				if (Util.s_debug)
				{
					GameBattle.m_instance.bGameStateToWin = bGameStateToWin;
				}
			}
			Destroy();
		}

		public override void Destroy(bool destroy = false)
		{
			base.Destroy(destroy);
			if (!destroy)
			{
				if (spawnInfo != null && spawnInfo.spawnPoint != null)
				{
					spawnInfo.spawnPoint.KillEnemy(spawnInfo.spawnWave);
				}
				GameBattle.m_instance.AddToComputerNeedDeleteList(this);
				GameBattle.s_enemyCount--;
				if (GameBattle.s_enemyCount < 0)
				{
					GameBattle.s_enemyCount = 0;
				}
			}
		}

		public override void Reset()
		{
			baseAttribute.attackFrequence = m_attFrequency;
			base.hpMax = baseAttribute.hpMax;
			MoveSpeed = baseAttribute.moveSpeed;
			base.hitInfo.damage = baseAttribute.damage;
			base.hitInfo.critRate = baseAttribute.proCritical;
			base.hitInfo.critDamage = baseAttribute.critDamage;
			base.isEndure = false;
			base.isIgnoreSkillDamage = false;
			if (links != null)
			{
				links.Clear();
			}
			base.Reset();
			base.isRage = false;
			SwitchFSM(GetDefaultAIState());
			SetAttackCollider(false);
			SetSpecialAttibute();
			UpdateAnimationSpeed(isBoss || eliteType != EnemyEliteType.None);
			if (isBoss)
			{
				BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.BGM_BOSS);
			}
			if (isBoss || eliteType != 0)
			{
				if (GameBattle.m_instance.UIVisableTarget == null || !GameBattle.m_instance.UIVisableTarget.Alive())
				{
					GameBattle.m_instance.UIVisableTarget = this;
					GameObject uITargetPanel = GameBattle.m_instance.m_UITargetPanel;
					if (uITargetPanel != null)
					{
						uITargetPanel.SetActive(true);
						BattleUIEvent component = uITargetPanel.GetComponent<BattleUIEvent>();
						component.SetTarget(this);
					}
				}
				UnityEngine.AI.NavMeshAgent navMeshAgent = GetNavMeshAgent();
				if (navMeshAgent != null)
				{
					navMeshAgent.enabled = true;
					navMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
					ResumeNav();
				}
			}
			else
			{
				UnityEngine.AI.NavMeshAgent navMeshAgent2 = GetNavMeshAgent();
				if (navMeshAgent2 != null)
				{
					navMeshAgent2.enabled = true;
					navMeshAgent2.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
					ResumeNav();
				}
			}
			GameObject enemyHpBarFromBuffer = UIControlManager.Instance.GetEnemyHpBarFromBuffer();
			if (enemyHpBarFromBuffer != null)
			{
				enemyHpBarFromBuffer.SetActive(true);
				m_hpBar = enemyHpBarFromBuffer.GetComponent<UISlider>();
				m_hpBar.transform.localScale = Vector3.one;
				m_hpBar.value = (float)base.hp / (float)base.hpMax;
				Vector3 p = GameBattle.m_instance.GetCamera().WorldToScreenPoint(m_effectPointUpHead.position);
				p = Util.ScreenPointToNGUI(p);
				m_hpBar.transform.localPosition = p;
				m_hpBar.gameObject.SetActive(false);
			}
		}

		public virtual void SetSpecialAttibute()
		{
			if (isBoss)
			{
				if (eliteType == EnemyEliteType.None)
				{
					eliteType = EnemyEliteType.Rank1;
				}
				foreach (Material material in m_materials)
				{
					material.shader = m_shaderModelEdge;
					material.SetColor("_Color", Color.white);
					material.SetColor("_AtmoColor", Color.yellow);
				}
				SetEliteAttribute();
				GetTransform().localScale = new Vector3(bossScale, bossScale, bossScale);
				base.meleeRange = m_baseMeleeRange * bossScale;
				base.hp = (base.hpMax *= 3);
				baseAttribute.hpMax = base.hpMax;
				NegativeAffectEnable = false;
				return;
			}
			if (eliteType != 0)
			{
				if (eliteType == EnemyEliteType.Rank1)
				{
					foreach (Material material2 in m_materials)
					{
						material2.shader = m_shaderModelEdge;
						material2.SetColor("_Color", Color.white);
						material2.SetColor("_AtmoColor", UIUtil._UIGreenColor);
					}
				}
				else if (eliteType == EnemyEliteType.Rank2)
				{
					foreach (Material material3 in m_materials)
					{
						material3.shader = m_shaderModelEdge;
						material3.SetColor("_Color", Color.white);
						material3.SetColor("_AtmoColor", UIUtil._UIGreenColor);
					}
				}
				else if (eliteType == EnemyEliteType.Rank3)
				{
					foreach (Material material4 in m_materials)
					{
						material4.shader = m_shaderModelEdge;
						material4.SetColor("_Color", Color.white);
						material4.SetColor("_AtmoColor", UIUtil._UIGreenColor);
					}
				}
				GetTransform().localScale = new Vector3(eliteScale, eliteScale, eliteScale);
				base.meleeRange = m_baseMeleeRange * eliteScale;
				SetEliteAttribute();
				NegativeAffectEnable = false;
				return;
			}
			foreach (Material material5 in m_materials)
			{
				material5.shader = m_shaderDiffuse;
				material5.SetColor("_Color", Color.white);
			}
			base.meleeRange = m_baseMeleeRange;
			GetTransform().localScale = Vector3.one;
			base.hitInfo.damage = m_damageNormal;
			int num = (base.hpMax = m_hpNormal);
			base.hp = num;
			baseAttribute.hpMax = base.hpMax;
			MoveSpeed = m_speedNormal;
			NegativeAffectEnable = true;
		}

		public virtual void ReceiveSpecailAttributeFromCentre()
		{
			if (SpecialAttribute.s_enemySpecailAttributesCentre == null || !Alive())
			{
				return;
			}
			foreach (int key2 in SpecialAttribute.s_enemySpecailAttributesCentre.Keys)
			{
				if (m_recieveSpecialAttribute.ContainsKey(key2))
				{
					continue;
				}
				SpecialAttribute.SpecialAttributeGameData specialAttributeGameData = SpecialAttribute.s_enemySpecailAttributesCentre[key2];
				switch (specialAttributeGameData.type)
				{
				case SpecialAttribute.AttributeType.Reinforcement:
				{
					float l = base.hitInfo.damage.left + baseAttribute.damage.left * DataCenter.Conf().m_specialAttributeReinforcement.damage;
					float r = base.hitInfo.damage.right + baseAttribute.damage.right * DataCenter.Conf().m_specialAttributeReinforcement.damage;
					base.hitInfo.damage = new NumberSection<float>(l, r);
					if (specialAttributeGameData.creater == this || m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData.type))
					{
						break;
					}
					DS2HalfStaticObject dS2HalfStaticObject2 = BattleBufferManager.Instance.GetSpecialEffectByType(DataCenter.Conf().m_specialAttributeReinforcement.effects[1].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject2 != null)
					{
						dS2HalfStaticObject2.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject2.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component2 = dS2HalfStaticObject2.GetGameObject().GetComponent<EffectPlayCombie>();
						component2.StartEmit();
						if (!m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeReinforcement.effects[1].type))
						{
							m_specialEffects.Add(DataCenter.Conf().m_specialAttributeReinforcement.effects[1].type, dS2HalfStaticObject2);
						}
					}
					break;
				}
				case SpecialAttribute.AttributeType.Mania:
				{
					base.hitInfo.critRate = base.hitInfo.critRate + (float)(int)DataCenter.Conf().m_specialAttributeMania.proCrit;
					base.hitInfo.critDamage = base.hitInfo.critDamage + base.critDamage * DataCenter.Conf().m_specialAttributeMania.critDamage;
					if (specialAttributeGameData.creater == this || m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData.type))
					{
						break;
					}
					DS2HalfStaticObject dS2HalfStaticObject4 = BattleBufferManager.Instance.GetSpecialEffectByType(DataCenter.Conf().m_specialAttributeMania.effects[1].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject4 != null)
					{
						dS2HalfStaticObject4.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject4.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component4 = dS2HalfStaticObject4.GetGameObject().GetComponent<EffectPlayCombie>();
						component4.StartEmit();
						if (!m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeMania.effects[1].type))
						{
							m_specialEffects.Add(DataCenter.Conf().m_specialAttributeMania.effects[1].type, dS2HalfStaticObject4);
						}
					}
					break;
				}
				case SpecialAttribute.AttributeType.Rapid:
				{
					m_attFrequency -= baseAttribute.attackFrequence - baseAttribute.attackFrequence * (1f / DataCenter.Conf().m_specialAttributeRapid.attFrequency);
					MoveSpeed += baseAttribute.moveSpeed * DataCenter.Conf().m_specialAttributeRapid.addMoveSpeedPercent;
					if (specialAttributeGameData.creater == this || m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData.type))
					{
						break;
					}
					DS2HalfStaticObject dS2HalfStaticObject3 = BattleBufferManager.Instance.GetSpecialEffectByType(DataCenter.Conf().m_specialAttributeRapid.effects[1].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject3 != null)
					{
						dS2HalfStaticObject3.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject3.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component3 = dS2HalfStaticObject3.GetGameObject().GetComponent<EffectPlayCombie>();
						component3.StartEmit();
						if (!m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeRapid.effects[1].type))
						{
							m_specialEffects.Add(DataCenter.Conf().m_specialAttributeRapid.effects[1].type, dS2HalfStaticObject3);
						}
					}
					break;
				}
				case SpecialAttribute.AttributeType.Dodge:
				{
					base.dodgeRate += DataCenter.Conf().m_specialAttributeDodge.proDodge;
					if (specialAttributeGameData.creater == this || m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData.type))
					{
						break;
					}
					DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(DataCenter.Conf().m_specialAttributeDodge.effects[1].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject != null)
					{
						dS2HalfStaticObject.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
						component.StartEmit();
						if (!m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeDodge.effects[1].type))
						{
							m_specialEffects.Add(DataCenter.Conf().m_specialAttributeDodge.effects[1].type, dS2HalfStaticObject);
						}
					}
					break;
				}
				}
				m_recieveSpecialAttribute.Add(key2, specialAttributeGameData);
				if (m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData.type))
				{
					Dictionary<SpecialAttribute.AttributeType, int> recieveSpecialAttributeCount;
					Dictionary<SpecialAttribute.AttributeType, int> dictionary = (recieveSpecialAttributeCount = m_recieveSpecialAttributeCount);
					SpecialAttribute.AttributeType type;
					SpecialAttribute.AttributeType key = (type = specialAttributeGameData.type);
					int num = recieveSpecialAttributeCount[type];
					dictionary[key] = num + 1;
				}
				else
				{
					m_recieveSpecialAttributeCount.Add(specialAttributeGameData.type, 1);
				}
			}
			m_removeSpeAttrId.Clear();
			foreach (int key3 in m_recieveSpecialAttribute.Keys)
			{
				if (!SpecialAttribute.s_enemySpecailAttributesCentre.ContainsKey(key3))
				{
					m_removeSpeAttrId.Add(key3);
				}
			}
			for (int i = 0; i < m_removeSpeAttrId.Count; i++)
			{
				RemoveSpecialAttribteById(m_removeSpeAttrId[i]);
			}
		}

		private void RemoveSpecialAttribteById(int speAttrId)
		{
			SpecialAttribute.SpecialAttributeGameData specialAttributeGameData = m_recieveSpecialAttribute[speAttrId];
			Dictionary<SpecialAttribute.AttributeType, int> recieveSpecialAttributeCount;
			Dictionary<SpecialAttribute.AttributeType, int> dictionary = (recieveSpecialAttributeCount = m_recieveSpecialAttributeCount);
			SpecialAttribute.AttributeType type;
			SpecialAttribute.AttributeType key = (type = specialAttributeGameData.type);
			int num = recieveSpecialAttributeCount[type];
			dictionary[key] = num - 1;
			int num2 = m_recieveSpecialAttributeCount[specialAttributeGameData.type];
			bool flag = num2 <= 0;
			if (flag)
			{
				m_recieveSpecialAttributeCount.Remove(specialAttributeGameData.type);
			}
			m_recieveSpecialAttribute.Remove(speAttrId);
			RemoveRecieveSpecialAttribte(specialAttributeGameData.type, flag, specialAttributeGameData.creater.id == base.id);
		}

		private void RemoveSpecialAttribte(SpecialAttribute.AttributeType type, bool disableEffect, bool creater)
		{
			switch (type)
			{
			case SpecialAttribute.AttributeType.Decay:
				if (creater)
				{
					Transform transform2 = GetTransform().Find("BuffTriggerPoison");
					if (transform2 != null)
					{
						UnityEngine.Object.Destroy(transform2.gameObject);
					}
				}
				break;
			case SpecialAttribute.AttributeType.Rifrigerate:
				if (creater)
				{
					Transform transform = GetTransform().Find("BuffTriggerRifrigerate");
					if (transform != null)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
					}
				}
				break;
			case SpecialAttribute.AttributeType.Harden:
				base.reduceDamage -= DataCenter.Conf().m_specialAttributeHarden.reduceDamage;
				break;
			case SpecialAttribute.AttributeType.Variation:
			{
				float l = base.hitInfo.damage.left - baseAttribute.damage.left * DataCenter.Conf().m_specialAttributeVariation.damage;
				float r = base.hitInfo.damage.right - baseAttribute.damage.right * DataCenter.Conf().m_specialAttributeVariation.damage;
				base.hitInfo.damage = new NumberSection<float>(l, r);
				m_attFrequency += baseAttribute.attackFrequence - baseAttribute.attackFrequence * (1f / DataCenter.Conf().m_specialAttributeVariation.attFrequency);
				MoveSpeed -= baseAttribute.moveSpeed * DataCenter.Conf().m_specialAttributeVariation.addMoveSpeedPercent;
				GetTransform().localScale = GetTransform().localScale - GetTransform().localScale * 0.3f;
				base.meleeRange -= base.meleeRange * 0.3f;
				break;
			}
			case SpecialAttribute.AttributeType.Strengthen:
				base.hpMax -= (int)((float)baseAttribute.hpMax * DataCenter.Conf().m_specialAttributeStrengthen.hpMax);
				break;
			case SpecialAttribute.AttributeType.InvalidSkill:
				base.isIgnoreSkillDamage = false;
				break;
			case SpecialAttribute.AttributeType.Tornado:
				if (links != null && links.ContainsKey(Defined.ObjectLinkType.Summon))
				{
					for (int i = 0; i < links[Defined.ObjectLinkType.Summon].Count; i++)
					{
						links[Defined.ObjectLinkType.Summon][i].Destroy();
					}
				}
				break;
			}
		}

		private void RemoveRecieveSpecialAttribte(SpecialAttribute.AttributeType type, bool disableEffect, bool creater)
		{
			switch (type)
			{
			case SpecialAttribute.AttributeType.Reinforcement:
			{
				float l = base.hitInfo.damage.left - baseAttribute.damage.left * DataCenter.Conf().m_specialAttributeReinforcement.damage;
				float r = base.hitInfo.damage.right - baseAttribute.damage.right * DataCenter.Conf().m_specialAttributeReinforcement.damage;
				base.hitInfo.damage = new NumberSection<float>(l, r);
				if (disableEffect && m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeReinforcement.effects[1].type))
				{
					DS2HalfStaticObject dS2HalfStaticObject3 = m_specialEffects[DataCenter.Conf().m_specialAttributeReinforcement.effects[1].type];
					EffectPlayCombie component3 = dS2HalfStaticObject3.GetGameObject().GetComponent<EffectPlayCombie>();
					component3.StopEmit();
					m_specialEffects.Remove(DataCenter.Conf().m_specialAttributeReinforcement.effects[1].type);
				}
				break;
			}
			case SpecialAttribute.AttributeType.Mania:
				base.hitInfo.critRate -= (int)DataCenter.Conf().m_specialAttributeMania.proCrit;
				base.hitInfo.critDamage = base.hitInfo.critDamage - base.critDamage * DataCenter.Conf().m_specialAttributeMania.critDamage;
				if (disableEffect && m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeMania.effects[1].type))
				{
					DS2HalfStaticObject dS2HalfStaticObject2 = m_specialEffects[DataCenter.Conf().m_specialAttributeMania.effects[1].type];
					EffectPlayCombie component2 = dS2HalfStaticObject2.GetGameObject().GetComponent<EffectPlayCombie>();
					component2.StopEmit();
					m_specialEffects.Remove(DataCenter.Conf().m_specialAttributeMania.effects[1].type);
				}
				break;
			case SpecialAttribute.AttributeType.Rapid:
				m_attFrequency += baseAttribute.attackFrequence - baseAttribute.attackFrequence * (1f / DataCenter.Conf().m_specialAttributeRapid.attFrequency);
				MoveSpeed -= baseAttribute.moveSpeed * DataCenter.Conf().m_specialAttributeRapid.addMoveSpeedPercent;
				if (disableEffect && m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeRapid.effects[1].type))
				{
					DS2HalfStaticObject dS2HalfStaticObject4 = m_specialEffects[DataCenter.Conf().m_specialAttributeRapid.effects[1].type];
					EffectPlayCombie component4 = dS2HalfStaticObject4.GetGameObject().GetComponent<EffectPlayCombie>();
					component4.StopEmit();
					m_specialEffects.Remove(DataCenter.Conf().m_specialAttributeRapid.effects[1].type);
				}
				break;
			case SpecialAttribute.AttributeType.Dodge:
				base.dodgeRate -= DataCenter.Conf().m_specialAttributeDodge.proDodge;
				if (disableEffect && m_specialEffects.ContainsKey(DataCenter.Conf().m_specialAttributeDodge.effects[1].type))
				{
					DS2HalfStaticObject dS2HalfStaticObject = m_specialEffects[DataCenter.Conf().m_specialAttributeDodge.effects[1].type];
					EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
					component.StopEmit();
					m_specialEffects.Remove(DataCenter.Conf().m_specialAttributeDodge.effects[1].type);
				}
				break;
			}
		}

		private void UpdateSpecialAttribute()
		{
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Hellfire))
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary = (specialAttrTimer = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key2 = (key = SpecialAttribute.AttributeType.Hellfire);
				float num = specialAttrTimer[key];
				dictionary[key2] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.Hellfire] >= DataCenter.Conf().m_specialAttributeHellfire.frequency)
				{
					m_specialAttrTimer[SpecialAttribute.AttributeType.Hellfire] = 0f;
					DoSpecialAttributeEmitBalls(SpecialAttribute.AttributeType.Hellfire);
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Endure) && m_specialAttrTimer[SpecialAttribute.AttributeType.Endure] > 0f)
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer2;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary2 = (specialAttrTimer2 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key3 = (key = SpecialAttribute.AttributeType.Endure);
				float num = specialAttrTimer2[key];
				dictionary2[key3] = num - Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.Endure] <= 0f)
				{
					base.isEndure = false;
					m_specialAttrTimer[SpecialAttribute.AttributeType.Endure] = 0f;
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Elegy) && elegyTarget == null)
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer3;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary3 = (specialAttrTimer3 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key4 = (key = SpecialAttribute.AttributeType.Elegy);
				float num = specialAttrTimer3[key];
				dictionary3[key4] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.Elegy] >= DataCenter.Conf().m_specialAttributeElegy.frequency)
				{
					m_specialAttrTimer[SpecialAttribute.AttributeType.Elegy] = 0f;
					DS2ActiveObject randomObjFromTargetList = GameBattle.m_instance.GetRandomObjFromTargetList(this, true);
					if (randomObjFromTargetList != null)
					{
						elegyTarget = randomObjFromTargetList;
						float num2 = DataCenter.Conf().m_specialAttributeElegy.hp;
						float value = (float)randomObjFromTargetList.hpMax * num2;
						Buff buff = new Buff(Buff.AffectType.Confuse, value, 0f, -1f);
						buff.creater = this;
						randomObjFromTargetList.GetBuffManager().AddBuff(buff);
						if (base.lockedTarget == randomObjFromTargetList)
						{
							base.lockedTarget = null;
						}
					}
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.God))
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer4;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary4 = (specialAttrTimer4 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key5 = (key = SpecialAttribute.AttributeType.God);
				float num = specialAttrTimer4[key];
				dictionary4[key5] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.God] >= DataCenter.Conf().m_specialAttributeGod.frequency)
				{
					m_specialAttrTimer[SpecialAttribute.AttributeType.God] = 0f;
					SetGodTime(DataCenter.Conf().m_specialAttributeGod.duration);
					SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.God);
					DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject != null)
					{
						dS2HalfStaticObject.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
						DS2ObjectDestroy dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().GetComponent<DS2ObjectDestroy>();
						if (dS2ObjectDestroy == null)
						{
							dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().AddComponent<DS2ObjectDestroy>();
						}
						dS2ObjectDestroy.TimeDestroy(DataCenter.Conf().m_specialAttributeGod.duration, false);
						EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
						component.StartEmit();
					}
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.PusBlood))
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer5;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary5 = (specialAttrTimer5 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key6 = (key = SpecialAttribute.AttributeType.PusBlood);
				float num = specialAttrTimer5[key];
				dictionary5[key6] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.PusBlood] >= DataCenter.Conf().m_specialAttributePusBlood.frequency)
				{
					Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer6;
					Dictionary<SpecialAttribute.AttributeType, float> dictionary6 = (specialAttrTimer6 = m_specialAttrTimer2);
					SpecialAttribute.AttributeType key7 = (key = SpecialAttribute.AttributeType.PusBlood);
					num = specialAttrTimer6[key];
					dictionary6[key7] = num + Time.deltaTime;
					float num3 = 0.2f;
					SpecialAttribute.SpecialAttributeData specialAttributeData2 = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.PusBlood);
					if (m_specialAttrTimer2[SpecialAttribute.AttributeType.PusBlood] >= num3)
					{
						m_specialAttrTimer2[SpecialAttribute.AttributeType.PusBlood] = 0f;
						Spore spore = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData2.effects[0].type) as Spore;
						if (spore != null)
						{
							spore.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
							NumberSection<float> numberSection = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * DataCenter.Conf().m_specialAttributePusBlood.damage, base.hitInfo.damage.right + base.hitInfo.damage.right * DataCenter.Conf().m_specialAttributePusBlood.damage);
							float damage = UnityEngine.Random.Range(numberSection.left, numberSection.right);
							spore.SetDamage(damage);
							spore.SetActive(true, this);
							GameBattle.m_instance.AddObjToInteractObjectList(spore);
						}
						else
						{
							m_specialAttrTimer[SpecialAttribute.AttributeType.PusBlood] = 0f;
							m_specialAttrTimer2[SpecialAttribute.AttributeType.PusBlood] = 0f;
						}
					}
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Bomb))
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer7;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary7 = (specialAttrTimer7 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key8 = (key = SpecialAttribute.AttributeType.Bomb);
				float num = specialAttrTimer7[key];
				dictionary7[key8] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.Bomb] >= DataCenter.Conf().m_specialAttributeBomb.frequency)
				{
					Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer8;
					Dictionary<SpecialAttribute.AttributeType, float> dictionary8 = (specialAttrTimer8 = m_specialAttrTimer2);
					SpecialAttribute.AttributeType key9 = (key = SpecialAttribute.AttributeType.Bomb);
					num = specialAttrTimer8[key];
					dictionary8[key9] = num + Time.deltaTime;
					float num4 = 0.2f;
					SpecialAttribute.SpecialAttributeData specialAttributeData3 = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.Bomb);
					if (m_specialAttrTimer2[SpecialAttribute.AttributeType.Bomb] >= num4)
					{
						m_specialAttrTimer2[SpecialAttribute.AttributeType.Bomb] = 0f;
						Spore spore2 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData3.effects[0].type) as Spore;
						if (spore2 != null)
						{
							spore2.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
							NumberSection<float> numberSection2 = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * DataCenter.Conf().m_specialAttributeBomb.damage, base.hitInfo.damage.right + base.hitInfo.damage.right * DataCenter.Conf().m_specialAttributeBomb.damage);
							float damage2 = UnityEngine.Random.Range(numberSection2.left, numberSection2.right);
							spore2.SetDamage(damage2);
							spore2.SetActive(true, this);
							GameBattle.m_instance.AddObjToInteractObjectList(spore2);
						}
						else
						{
							m_specialAttrTimer[SpecialAttribute.AttributeType.Bomb] = 0f;
							m_specialAttrTimer2[SpecialAttribute.AttributeType.Bomb] = 0f;
						}
					}
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.LiquidNitrogen))
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer9;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary9 = (specialAttrTimer9 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key10 = (key = SpecialAttribute.AttributeType.LiquidNitrogen);
				float num = specialAttrTimer9[key];
				dictionary9[key10] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.LiquidNitrogen] >= DataCenter.Conf().m_specialAttributeLiquidNitrogen.frequency)
				{
					Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer10;
					Dictionary<SpecialAttribute.AttributeType, float> dictionary10 = (specialAttrTimer10 = m_specialAttrTimer2);
					SpecialAttribute.AttributeType key11 = (key = SpecialAttribute.AttributeType.LiquidNitrogen);
					num = specialAttrTimer10[key];
					dictionary10[key11] = num + Time.deltaTime;
					float num5 = 0.2f;
					SpecialAttribute.SpecialAttributeData specialAttributeData4 = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.LiquidNitrogen);
					if (m_specialAttrTimer2[SpecialAttribute.AttributeType.LiquidNitrogen] >= num5)
					{
						m_specialAttrTimer2[SpecialAttribute.AttributeType.LiquidNitrogen] = 0f;
						Spore spore3 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData4.effects[0].type) as Spore;
						if (spore3 != null)
						{
							spore3.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
							NumberSection<float> numberSection3 = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * DataCenter.Conf().m_specialAttributeLiquidNitrogen.damage, base.hitInfo.damage.right + base.hitInfo.damage.right * DataCenter.Conf().m_specialAttributeLiquidNitrogen.damage);
							float damage3 = UnityEngine.Random.Range(numberSection3.left, numberSection3.right);
							spore3.SetDamage(damage3);
							spore3.SetActive(true, this);
							GameBattle.m_instance.AddObjToInteractObjectList(spore3);
						}
						else
						{
							m_specialAttrTimer[SpecialAttribute.AttributeType.LiquidNitrogen] = 0f;
							m_specialAttrTimer2[SpecialAttribute.AttributeType.LiquidNitrogen] = 0f;
						}
					}
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Summon))
			{
				Dictionary<SpecialAttribute.AttributeType, float> specialAttrTimer11;
				Dictionary<SpecialAttribute.AttributeType, float> dictionary11 = (specialAttrTimer11 = m_specialAttrTimer);
				SpecialAttribute.AttributeType key;
				SpecialAttribute.AttributeType key12 = (key = SpecialAttribute.AttributeType.Summon);
				float num = specialAttrTimer11[key];
				dictionary11[key12] = num + Time.deltaTime;
				if (m_specialAttrTimer[SpecialAttribute.AttributeType.Summon] >= DataCenter.Conf().m_specialAttributeSummon.frequency)
				{
					m_specialAttrTimer[SpecialAttribute.AttributeType.Summon] = 0f;
					DoSpecialAttributeSummon();
				}
			}
		}

		private void DoSpecialAttributeOnDeath()
		{
			for (int i = 0; i < m_specialAttributeCreateId.Count; i++)
			{
				SpecialAttribute.RemoveEnemySpecialAttribute(m_specialAttributeCreateId[i]);
				RemoveSpecialAttribteById(m_specialAttributeCreateId[i]);
			}
			for (int j = 0; j < specialAttribute.Count; j++)
			{
				SpecialAttribute.AttributeType type = specialAttribute[j];
				RemoveSpecialAttribte(type, true, true);
			}
			foreach (int key in m_recieveSpecialAttribute.Keys)
			{
				SpecialAttribute.SpecialAttributeGameData specialAttributeGameData = m_recieveSpecialAttribute[key];
				switch (specialAttributeGameData.type)
				{
				case SpecialAttribute.AttributeType.Rifrigerate:
				{
					SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.Rifrigerate);
					DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[1].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject != null)
					{
						dS2HalfStaticObject.GetTransform().position = m_effectPointGround.transform.position;
						EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
						component.StartEmit();
					}
					int layerMask = ((base.clique != 0) ? 1536 : 526336);
					Collider[] array = Physics.OverlapSphere(GetTransform().position, 2f, layerMask);
					Collider[] array2 = array;
					foreach (Collider collider in array2)
					{
						DS2ActiveObject @object = DS2ObjectStub.GetObject<DS2ActiveObject>(collider.gameObject);
						if (@object == null)
						{
						}
						HitInfo hitInfo = new HitInfo(specialAttributeGameData.creater.GetHitInfo());
						SpecialHitInfo specialHitInfo = new SpecialHitInfo();
						specialHitInfo.time = DataCenter.Conf().m_specialAttributeRifrigerate.frozenTime;
						hitInfo.AddSpecialHit(Defined.SPECIAL_HIT_TYPE.FROZEN, specialHitInfo);
						if (@object.OnHit(hitInfo).isHit)
						{
						}
					}
					break;
				}
				case SpecialAttribute.AttributeType.DeadSpawnBomb:
				{
					int num = UnityEngine.Random.Range(0, 100);
					if (num < 10)
					{
						EnemyHealthBomb enemyHealthBomb = BattleBufferManager.Instance.GetSpecialEffectByType(DataCenter.Conf().m_specialAttributeDeadSpawnBomb.effects[0].type) as EnemyHealthBomb;
						if (enemyHealthBomb != null)
						{
							enemyHealthBomb.GetTransform().position = GetTransform().position;
							enemyHealthBomb.SetActive(true, this);
							GameBattle.m_instance.AddObjToInteractObjectList(enemyHealthBomb);
						}
					}
					break;
				}
				}
				RemoveSpecialAttribte(specialAttributeGameData.type, true, specialAttributeGameData.creater.id == base.id);
			}
		}

		private void DoSpecialAttributeOnHit(SpecialAttribute.AttributeType type)
		{
			switch (type)
			{
			case SpecialAttribute.AttributeType.Electric:
			{
				int num2 = UnityEngine.Random.Range(0, 100);
				if (num2 > DataCenter.Conf().m_specialAttributeElectric.probability)
				{
					break;
				}
				Bullet bullet = BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.EnemySpecAttrLeakage) as Bullet;
				if (bullet == null)
				{
					break;
				}
				bullet.hitInfo.damage = new NumberSection<float>(base.hitInfo.damage.left * 0.3f, base.hitInfo.damage.right * 0.3f);
				DS2ActiveObject randomObjFromTargetList = GameBattle.m_instance.GetRandomObjFromTargetList(this);
				if (randomObjFromTargetList != null)
				{
					Vector3 direction = randomObjFromTargetList.GetTransform().position - GetTransform().position;
					if (base.clique == Clique.Computer)
					{
						bullet.GetGameObject().layer = 23;
					}
					else
					{
						bullet.GetGameObject().layer = 22;
					}
					bullet.hitInfo.source = this;
					bullet.SetBullet(this, bullet.attribute, m_effectPoint.position, direction);
					bullet.Emit(15f);
				}
				break;
			}
			case SpecialAttribute.AttributeType.Endure:
			{
				int num = UnityEngine.Random.Range(0, 100);
				if (num > DataCenter.Conf().m_specialAttributeEndure.probability)
				{
					break;
				}
				base.isEndure = true;
				m_specialAttrTimer[type] = DataCenter.Conf().m_specialAttributeEndure.duration;
				SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.Endure);
				DataConf.SpecialEffectData specialEffectDataByIndex = DataCenter.Conf().GetSpecialEffectDataByIndex((int)specialAttributeData.effects[0].type);
				DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as DS2HalfStaticObject;
				if (dS2HalfStaticObject != null)
				{
					dS2HalfStaticObject.GetTransform().parent = m_effectPointGround;
					dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
					DS2ObjectDestroy dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().GetComponent<DS2ObjectDestroy>();
					if (dS2ObjectDestroy == null)
					{
						dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().AddComponent<DS2ObjectDestroy>();
					}
					dS2ObjectDestroy.TimeDestroy(DataCenter.Conf().m_specialAttributeEndure.duration, false);
					EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
					component.StartEmit();
				}
				break;
			}
			case SpecialAttribute.AttributeType.Bloodsucking:
			{
				float addSpeedPercent = DataCenter.Conf().m_specialAttributeBloodsucking.addSpeedPercent;
				ushort accumulate = DataCenter.Conf().m_specialAttributeBloodsucking.accumulate;
				float duration = DataCenter.Conf().m_specialAttributeBloodsucking.duration;
				float damage = DataCenter.Conf().m_specialAttributeBloodsucking.damage;
				Buff buff = new Buff(Buff.AffectType.MoveSpeed, addSpeedPercent, -1f, duration, Buff.CalcType.Percentage, 0f);
				GetBuffManager().AddBuff(buff, accumulate);
				Buff buff2 = new Buff(Buff.AffectType.AddATK, damage, -1f, duration, Buff.CalcType.Percentage, 0f);
				GetBuffManager().AddBuff(buff2, accumulate);
				break;
			}
			case SpecialAttribute.AttributeType.StrickBack:
				DoSpecialAttributeEmitBalls(SpecialAttribute.AttributeType.StrickBack);
				break;
			case SpecialAttribute.AttributeType.Split:
				if (links != null && links.ContainsKey(Defined.ObjectLinkType.Cloned))
				{
					break;
				}
				if (links == null)
				{
					links = new Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>>();
				}
				if ((float)base.hp < (float)base.hpMax * DataCenter.Conf().m_specialAttributeSplit.hp)
				{
					float newHp = DataCenter.Conf().m_specialAttributeSplit.newHp;
					base.hp = (int)((float)base.hpMax * newHp);
					Enemy enemy = CharacterBuilder.CreateEnemy(enemyType, GetTransform().position, GetTransform().rotation, m_gameLayer);
					enemy.isBoss = isBoss;
					enemy.eliteType = eliteType;
					enemy.GetGameObject().SetActive(true);
					enemy.Reset();
					enemy.hp = base.hp;
					if (links.ContainsKey(Defined.ObjectLinkType.Cloned))
					{
						links[Defined.ObjectLinkType.Cloned].Add(enemy);
					}
					else
					{
						List<DS2ActiveObject> list = new List<DS2ActiveObject>();
						list.Add(enemy);
						links.Add(Defined.ObjectLinkType.Cloned, list);
					}
					if (enemy.links == null)
					{
						enemy.links = new Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>>();
					}
					if (enemy.links.ContainsKey(Defined.ObjectLinkType.Cloned))
					{
						enemy.links[Defined.ObjectLinkType.Cloned].Add(this);
					}
					else
					{
						List<DS2ActiveObject> list2 = new List<DS2ActiveObject>();
						list2.Add(this);
						enemy.links.Add(Defined.ObjectLinkType.Cloned, list2);
					}
					GameBattle.s_enemyCount++;
					GameBattle.m_instance.AddObjToComputerList(enemy);
					DoSpecialPlayEffect(SpecialAttribute.AttributeType.Split, this, 0, true);
					DoSpecialPlayEffect(SpecialAttribute.AttributeType.Split, enemy, 0, true);
				}
				break;
			}
		}

		private void DoSpecialAttributeOnBeforeHit(SpecialAttribute.AttributeType type, HitInfo hitInfo)
		{
			switch (type)
			{
			case SpecialAttribute.AttributeType.RangeWeaken:
				if (hitInfo.source != null)
				{
					float magnitude = (hitInfo.source.GetTransform().position - GetTransform().position).magnitude;
					float num = magnitude * DataCenter.Conf().m_specialAttributeRangeWeaken.reduceDamage;
					hitInfo.damage = new NumberSection<float>(hitInfo.damage.left * num, hitInfo.damage.right * num);
				}
				break;
			case SpecialAttribute.AttributeType.Territory:
				if (hitInfo.source != null)
				{
					hitInfo.damage = new NumberSection<float>(hitInfo.damage.left * 0.1f, hitInfo.damage.right * 0.1f);
					hitInfo.critDamage *= 5f;
				}
				break;
			}
		}

		private void DoSpecialAttributeEmitBalls(SpecialAttribute.AttributeType type)
		{
			switch (type)
			{
			case SpecialAttribute.AttributeType.Hellfire:
			{
				DoSpecialPlayEffect(SpecialAttribute.AttributeType.Hellfire, this, 0, true);
				int num2 = 8;
				for (int j = 0; j < num2; j++)
				{
					Quaternion rotation = Quaternion.AngleAxis((float)j * 360f / (float)num2, Vector3.up);
					Bullet bullet2 = BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.EnemySpecAttrFireBall) as Bullet;
					if (bullet2 != null)
					{
						bullet2.hitInfo.damage = new NumberSection<float>(base.hitInfo.damage.left * 0.3f, base.hitInfo.damage.right * 0.3f);
						bullet2.hitInfo.source = this;
						if (base.clique == Clique.Computer)
						{
							bullet2.GetGameObject().layer = 23;
						}
						else
						{
							bullet2.GetGameObject().layer = 22;
						}
						bullet2.SetBullet(this, m_effectPoint.position, rotation);
						bullet2.Emit(20f);
					}
				}
				break;
			}
			case SpecialAttribute.AttributeType.StrickBack:
			{
				int num = 1;
				for (int i = 0; i < num; i++)
				{
					DS2ActiveObject randomObjFromTargetList = GameBattle.m_instance.GetRandomObjFromTargetList(this);
					if (randomObjFromTargetList == null)
					{
						continue;
					}
					Vector3 direction = randomObjFromTargetList.GetTransform().position - GetTransform().position;
					Bullet bullet = BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.EnemySpecAttrIceBall) as Bullet;
					if (bullet != null)
					{
						float bombDamage = DataCenter.Conf().m_specialAttributeStrickBack.bombDamage;
						bullet.hitInfo.damage = new NumberSection<float>(base.hitInfo.damage.left * 1f, base.hitInfo.damage.right * 1f);
						if (base.clique == Clique.Computer)
						{
							bullet.GetGameObject().layer = 23;
						}
						else
						{
							bullet.GetGameObject().layer = 22;
						}
						bullet.hitInfo.source = this;
						bullet.SetBullet(this, bullet.attribute, m_effectPoint.position, direction);
						bullet.Emit(15f);
					}
				}
				break;
			}
			}
		}

		private void DoSpecialAttributeSpawnSpores(Spore.SporeType type)
		{
			switch (type)
			{
			case Spore.SporeType.PusBlood:
			{
				SpecialAttribute.SpecialAttributeData specialAttributeData2 = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.PusBlood);
				for (int j = 0; j < specialAttributeData2.effects[0].bufferCount; j++)
				{
					Spore spore2 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData2.effects[0].type) as Spore;
					if (spore2 != null)
					{
						spore2.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
						NumberSection<float> numberSection2 = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * DataCenter.Conf().m_specialAttributePusBlood.damage, base.hitInfo.damage.right + base.hitInfo.damage.right * DataCenter.Conf().m_specialAttributePusBlood.damage);
						float damage2 = UnityEngine.Random.Range(numberSection2.left, numberSection2.right);
						spore2.SetDamage(damage2);
						spore2.SetActive(true, this);
						GameBattle.m_instance.AddObjToInteractObjectList(spore2);
					}
				}
				break;
			}
			case Spore.SporeType.Bomb:
			{
				SpecialAttribute.SpecialAttributeData specialAttributeData3 = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.Bomb);
				for (int k = 0; k < specialAttributeData3.effects[0].bufferCount; k++)
				{
					Spore spore3 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData3.effects[0].type) as Spore;
					if (spore3 != null)
					{
						spore3.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
						NumberSection<float> numberSection3 = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * DataCenter.Conf().m_specialAttributeBomb.damage, base.hitInfo.damage.right + base.hitInfo.damage.right * DataCenter.Conf().m_specialAttributeBomb.damage);
						float damage3 = UnityEngine.Random.Range(numberSection3.left, numberSection3.right);
						spore3.SetDamage(damage3);
						spore3.SetActive(true, this);
						GameBattle.m_instance.AddObjToInteractObjectList(spore3);
					}
				}
				break;
			}
			case Spore.SporeType.LiquidNitrogen:
			{
				SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.LiquidNitrogen);
				for (int i = 0; i < specialAttributeData.effects[0].bufferCount; i++)
				{
					Spore spore = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as Spore;
					if (spore != null)
					{
						spore.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
						NumberSection<float> numberSection = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * DataCenter.Conf().m_specialAttributeLiquidNitrogen.damage, base.hitInfo.damage.right + base.hitInfo.damage.right * DataCenter.Conf().m_specialAttributeLiquidNitrogen.damage);
						float damage = UnityEngine.Random.Range(numberSection.left, numberSection.right);
						spore.SetDamage(damage);
						spore.SetActive(true, this);
						GameBattle.m_instance.AddObjToInteractObjectList(spore);
					}
				}
				break;
			}
			}
		}

		private void DoSpecialAttributeSummon()
		{
			for (int i = 0; i < 5; i++)
			{
				EnemySpawnPointMain.EnemyWaitSpawnInfo item = default(EnemySpawnPointMain.EnemyWaitSpawnInfo);
				item.type = EnemyType.Zombie;
				item.spawnInfo = new SpawnInfo();
				item.isBoss = false;
				item.eliteType = EnemyEliteType.None;
				item.specialAttribute = new List<SpecialAttribute.AttributeType>();
				GameBattle.s_enemyWaitToVisibleList.Add(item);
			}
			for (int j = 0; j < 3; j++)
			{
				EnemySpawnPointMain.EnemyWaitSpawnInfo item2 = default(EnemySpawnPointMain.EnemyWaitSpawnInfo);
				item2.type = EnemyType.ZombieBomb;
				item2.spawnInfo = new SpawnInfo();
				item2.isBoss = false;
				item2.eliteType = EnemyEliteType.None;
				item2.specialAttribute = new List<SpecialAttribute.AttributeType>();
				GameBattle.s_enemyWaitToVisibleList.Add(item2);
			}
			SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(SpecialAttribute.AttributeType.Summon);
			DataConf.SpecialEffectData specialEffectDataByIndex = DataCenter.Conf().GetSpecialEffectDataByIndex((int)specialAttributeData.effects[0].type);
			DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as DS2HalfStaticObject;
			if (dS2HalfStaticObject != null)
			{
				dS2HalfStaticObject.GetTransform().parent = m_effectPoint;
				dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
				DS2ObjectDestroy dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().GetComponent<DS2ObjectDestroy>();
				if (dS2ObjectDestroy == null)
				{
					dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().AddComponent<DS2ObjectDestroy>();
				}
				dS2ObjectDestroy.TimeDestroy(specialEffectDataByIndex.playTime, false);
				EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
				component.StartEmit();
			}
		}

		private void DoSpecialPlayEffect(SpecialAttribute.AttributeType type, Character creater, int effectIndex, bool autoDestroy)
		{
			SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(type);
			DataConf.SpecialEffectData specialEffectDataByIndex = DataCenter.Conf().GetSpecialEffectDataByIndex((int)specialAttributeData.effects[0].type);
			DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[effectIndex].type) as DS2HalfStaticObject;
			if (dS2HalfStaticObject == null)
			{
				return;
			}
			dS2HalfStaticObject.GetTransform().parent = creater.m_effectPoint;
			dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
			dS2HalfStaticObject.GetTransform().localScale = Vector3.one;
			if (autoDestroy)
			{
				DS2ObjectDestroy dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().GetComponent<DS2ObjectDestroy>();
				if (dS2ObjectDestroy == null)
				{
					dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().AddComponent<DS2ObjectDestroy>();
				}
				dS2ObjectDestroy.TimeDestroy(specialEffectDataByIndex.playTime, false);
			}
			EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
			component.StartEmit();
		}

		protected void SetEliteAttribute()
		{
			float num = ((!isBoss) ? eliteScale : bossScale);
			if (eliteType != 0)
			{
				eliteType = EnemyEliteType.Rank1;
			}
			if (eliteType == EnemyEliteType.Rank1)
			{
				int num3 = (base.hpMax = (int)((float)m_hpNormal * 5f));
				base.hp = num3;
				baseAttribute.hpMax = base.hpMax;
				base.hitInfo.damage = new NumberSection<float>(m_damageNormal.left * 2.5f, m_damageNormal.right * 2.5f);
				MoveSpeed = m_speedNormal * num;
				baseAttribute.moveSpeed = MoveSpeed;
				if (!DataCenter.Save().BattleTutorialFinished)
				{
					base.hp = (base.hpMax *= 3);
				}
			}
			else if (eliteType == EnemyEliteType.Rank2)
			{
				int num3 = (base.hpMax = (int)((float)m_hpNormal * 5f));
				base.hp = num3;
				baseAttribute.hpMax = base.hpMax;
				base.hitInfo.damage = new NumberSection<float>(m_damageNormal.left * 5f, m_damageNormal.right * 5f);
				MoveSpeed = m_speedNormal * num;
				baseAttribute.moveSpeed = MoveSpeed;
			}
			else if (eliteType == EnemyEliteType.Rank3)
			{
				int num3 = (base.hpMax = (int)((float)m_hpNormal * 10f));
				base.hp = num3;
				baseAttribute.hpMax = base.hpMax;
				base.hitInfo.damage = new NumberSection<float>(m_damageNormal.left * 10f, m_damageNormal.right * 10f);
				MoveSpeed = m_speedNormal * num;
				baseAttribute.moveSpeed = MoveSpeed;
			}
			for (int i = 0; i < specialAttribute.Count; i++)
			{
				SpecialAttribute.AttributeType attributeType = specialAttribute[i];
				switch (attributeType)
				{
				case SpecialAttribute.AttributeType.Reinforcement:
				case SpecialAttribute.AttributeType.Mania:
				case SpecialAttribute.AttributeType.Rapid:
				case SpecialAttribute.AttributeType.Dodge:
				case SpecialAttribute.AttributeType.Rifrigerate:
				{
					SpecialAttribute.SpecialAttributeData specialAttributeData7 = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					SpecialAttribute.SpecialAttributeGameData specialAttributeGameData4 = new SpecialAttribute.SpecialAttributeGameData(attributeType, this);
					int item2 = SpecialAttribute.AddEnemySpecailAttribute(specialAttributeGameData4);
					m_specialAttributeCreateId.Add(item2);
					if (!m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData4.type))
					{
						DS2HalfStaticObject dS2HalfStaticObject6 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData7.effects[0].type) as DS2HalfStaticObject;
						if (dS2HalfStaticObject6 != null)
						{
							dS2HalfStaticObject6.GetTransform().parent = m_effectPointGround;
							dS2HalfStaticObject6.GetTransform().localPosition = Vector3.zero;
							EffectPlayCombie component7 = dS2HalfStaticObject6.GetGameObject().GetComponent<EffectPlayCombie>();
							component7.StartEmit();
							if (!m_specialEffects.ContainsKey(specialAttributeData7.effects[0].type))
							{
								m_specialEffects.Add(specialAttributeData7.effects[0].type, dS2HalfStaticObject6);
							}
						}
					}
					switch (attributeType)
					{
					case SpecialAttribute.AttributeType.Rifrigerate:
					{
						GameObject gameObject2 = new GameObject();
						gameObject2.name = "BuffTriggerRifrigerate";
						gameObject2.transform.parent = GetTransform();
						gameObject2.transform.localPosition = Vector3.zero;
						gameObject2.layer = 20;
						BuffTrigger buffTrigger2 = gameObject2.AddComponent<BuffTrigger>();
						buffTrigger2.TriggerDistance = 7f;
						buffTrigger2.SetTrigger(Buff.AffectType.MoveSpeed, DataCenter.Conf().m_specialAttributeRifrigerate.reduceSpeed, -1f, -1f, Buff.CalcType.General);
						break;
					}
					case SpecialAttribute.AttributeType.InvalidSkill:
						base.isIgnoreSkillDamage = true;
						break;
					}
					break;
				}
				case SpecialAttribute.AttributeType.Territory:
				case SpecialAttribute.AttributeType.InvalidSkill:
				{
					SpecialAttribute.SpecialAttributeData specialAttributeData6 = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					SpecialAttribute.SpecialAttributeGameData specialAttributeGameData3 = new SpecialAttribute.SpecialAttributeGameData(attributeType, this);
					if (m_recieveSpecialAttributeCount.ContainsKey(specialAttributeGameData3.type))
					{
						break;
					}
					DS2HalfStaticObject dS2HalfStaticObject5 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData6.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject5 != null)
					{
						dS2HalfStaticObject5.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject5.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component6 = dS2HalfStaticObject5.GetGameObject().GetComponent<EffectPlayCombie>();
						component6.StartEmit();
						if (!m_specialEffects.ContainsKey(specialAttributeData6.effects[0].type))
						{
							m_specialEffects.Add(specialAttributeData6.effects[0].type, dS2HalfStaticObject5);
						}
					}
					break;
				}
				case SpecialAttribute.AttributeType.Decay:
				{
					SpecialAttribute.SpecialAttributeData specialAttributeData4 = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					SpecialAttribute.SpecialAttributeGameData specialAttributeGameData = new SpecialAttribute.SpecialAttributeGameData(attributeType, this);
					DS2HalfStaticObject dS2HalfStaticObject3 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData4.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject3 != null)
					{
						dS2HalfStaticObject3.GetTransform().parent = m_effectPointGround;
						dS2HalfStaticObject3.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component4 = dS2HalfStaticObject3.GetGameObject().GetComponent<EffectPlayCombie>();
						component4.StartEmit();
						if (!m_specialEffects.ContainsKey(specialAttributeData4.effects[0].type))
						{
							m_specialEffects.Add(specialAttributeData4.effects[0].type, dS2HalfStaticObject3);
						}
					}
					GameObject gameObject = new GameObject();
					gameObject.name = "BuffTriggerPoison";
					gameObject.transform.parent = GetTransform();
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.layer = 20;
					BuffTrigger buffTrigger = gameObject.AddComponent<BuffTrigger>();
					buffTrigger.TriggerDistance = 10f;
					buffTrigger.SetTrigger(Buff.AffectType.Poison, DataCenter.Conf().m_specialAttributeDecay.reduceHpPercent, DataCenter.Conf().m_specialAttributeDecay.frequency, -1f, Buff.CalcType.Percentage);
					break;
				}
				case SpecialAttribute.AttributeType.Harden:
					base.reduceDamage += DataCenter.Conf().m_specialAttributeHarden.reduceDamage;
					break;
				case SpecialAttribute.AttributeType.Variation:
				{
					float l = base.hitInfo.damage.left + baseAttribute.damage.left * DataCenter.Conf().m_specialAttributeVariation.damage;
					float r = base.hitInfo.damage.right + baseAttribute.damage.right * DataCenter.Conf().m_specialAttributeVariation.damage;
					base.hitInfo.damage = new NumberSection<float>(l, r);
					m_attFrequency -= baseAttribute.attackFrequence - baseAttribute.attackFrequence * (1f / DataCenter.Conf().m_specialAttributeVariation.attFrequency);
					MoveSpeed += baseAttribute.moveSpeed * DataCenter.Conf().m_specialAttributeVariation.addMoveSpeedPercent;
					GetTransform().localScale = GetTransform().localScale + GetTransform().localScale * 0.3f;
					base.meleeRange += base.meleeRange * 0.3f;
					SpecialAttribute.SpecialAttributeData specialAttributeData5 = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					SpecialAttribute.SpecialAttributeGameData specialAttributeGameData2 = new SpecialAttribute.SpecialAttributeGameData(attributeType, this);
					DS2HalfStaticObject dS2HalfStaticObject4 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData5.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject4 != null)
					{
						dS2HalfStaticObject4.GetTransform().parent = m_effectPoint;
						dS2HalfStaticObject4.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component5 = dS2HalfStaticObject4.GetGameObject().GetComponent<EffectPlayCombie>();
						component5.StartEmit();
						if (!m_specialEffects.ContainsKey(specialAttributeData5.effects[0].type))
						{
							m_specialEffects.Add(specialAttributeData5.effects[0].type, dS2HalfStaticObject4);
						}
					}
					break;
				}
				case SpecialAttribute.AttributeType.Hellfire:
				case SpecialAttribute.AttributeType.Endure:
				case SpecialAttribute.AttributeType.Elegy:
				case SpecialAttribute.AttributeType.God:
				case SpecialAttribute.AttributeType.Summon:
					m_specialAttrTimer.Add(attributeType, 0f);
					break;
				case SpecialAttribute.AttributeType.PusBlood:
				case SpecialAttribute.AttributeType.Bomb:
				case SpecialAttribute.AttributeType.LiquidNitrogen:
					m_specialAttrTimer.Add(attributeType, 0f);
					m_specialAttrTimer2.Add(attributeType, 0f);
					break;
				case SpecialAttribute.AttributeType.Strengthen:
					base.hpMax += (int)((float)baseAttribute.hpMax * DataCenter.Conf().m_specialAttributeStrengthen.hpMax);
					base.hp = base.hpMax;
					break;
				case SpecialAttribute.AttributeType.DeadSpawnBomb:
				case SpecialAttribute.AttributeType.Vampire:
				{
					SpecialAttribute.SpecialAttributeGameData attrData = new SpecialAttribute.SpecialAttributeGameData(attributeType, this);
					int item = SpecialAttribute.AddEnemySpecailAttribute(attrData);
					m_specialAttributeCreateId.Add(item);
					break;
				}
				case SpecialAttribute.AttributeType.RangeWeaken:
				{
					SpecialAttribute.SpecialAttributeData specialAttributeData3 = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					DS2HalfStaticObject dS2HalfStaticObject2 = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData3.effects[0].type) as DS2HalfStaticObject;
					dS2HalfStaticObject2.GetTransform().parent = m_effectPoint.transform;
					dS2HalfStaticObject2.GetTransform().localPosition = Vector3.zero;
					dS2HalfStaticObject2.GetGameObject().SetActive(true);
					m_specialEffects.Add(specialAttributeData3.effects[0].type, dS2HalfStaticObject2);
					break;
				}
				case SpecialAttribute.AttributeType.Split:
					if (links == null)
					{
						links = new Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>>();
					}
					break;
				case SpecialAttribute.AttributeType.Tornado:
				{
					SpecialAttribute.SpecialAttributeData specialAttributeData2 = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					for (int j = 0; j < specialAttributeData2.effects[0].bufferCount; j++)
					{
						Tornado tornado = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData2.effects[0].type) as Tornado;
						float num6 = UnityEngine.Random.Range(DataCenter.Conf().m_specialAttributeTornado.damage.left, DataCenter.Conf().m_specialAttributeTornado.damage.right);
						if (num6 > 0f)
						{
							num6 *= -1f;
						}
						tornado.SetDamage(num6);
						tornado.SetAroundObj(this);
						tornado.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
						tornado.GetGameObject().layer = 19;
						tornado.SetActive(true, this);
						if (links == null)
						{
							links = new Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>>();
						}
						if (links.ContainsKey(Defined.ObjectLinkType.Summon))
						{
							links[Defined.ObjectLinkType.Summon].Add(tornado);
						}
						else
						{
							List<DS2ActiveObject> list = new List<DS2ActiveObject>();
							list.Add(tornado);
							links.Add(Defined.ObjectLinkType.Summon, list);
						}
						GameBattle.m_instance.AddObjToInteractObjectList(tornado);
					}
					break;
				}
				case SpecialAttribute.AttributeType.LifeLink:
				{
					Enemy enemyFromBuffer = BattleBufferManager.Instance.GetEnemyFromBuffer(EnemyType.Wrestler);
					Enemy enemyFromBuffer2 = BattleBufferManager.Instance.GetEnemyFromBuffer(EnemyType.Haoke);
					SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(attributeType);
					DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject != null)
					{
						dS2HalfStaticObject.GetTransform().parent = m_effectPoint;
						dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
						component.StartEmit();
						if (!m_specialEffects.ContainsKey(specialAttributeData.effects[0].type))
						{
							m_specialEffects.Add(specialAttributeData.effects[0].type, dS2HalfStaticObject);
						}
					}
					if (enemyFromBuffer != null)
					{
						enemyFromBuffer.eliteType = eliteType;
						enemyFromBuffer.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
						enemyFromBuffer.GetGameObject().SetActive(true);
						enemyFromBuffer.Reset();
						enemyFromBuffer.hp = base.hp;
						enemyFromBuffer.hpMax = base.hpMax;
						GameBattle.m_instance.AddObjToComputerList(enemyFromBuffer);
						GameBattle.s_enemyCount++;
						LinkToShareLife(enemyFromBuffer);
						dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as DS2HalfStaticObject;
						if (dS2HalfStaticObject != null)
						{
							dS2HalfStaticObject.GetTransform().parent = enemyFromBuffer.m_effectPoint;
							dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
							EffectPlayCombie component2 = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
							component2.StartEmit();
							if (!enemyFromBuffer.m_specialEffects.ContainsKey(specialAttributeData.effects[0].type))
							{
								enemyFromBuffer.m_specialEffects.Add(specialAttributeData.effects[0].type, dS2HalfStaticObject);
							}
						}
					}
					if (enemyFromBuffer2 == null)
					{
						break;
					}
					enemyFromBuffer2.eliteType = eliteType;
					enemyFromBuffer2.GetTransform().position = GameBattle.m_instance.GetNearestNodeGroup().GetRandomSpawnPointPosition();
					enemyFromBuffer2.GetGameObject().SetActive(true);
					enemyFromBuffer2.Reset();
					enemyFromBuffer2.hp = base.hp;
					enemyFromBuffer2.hpMax = base.hpMax;
					GameBattle.m_instance.AddObjToComputerList(enemyFromBuffer2);
					GameBattle.s_enemyCount++;
					LinkToShareLife(enemyFromBuffer2);
					if (enemyFromBuffer != null)
					{
						enemyFromBuffer2.LinkToShareLife(enemyFromBuffer);
					}
					dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(specialAttributeData.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject != null)
					{
						dS2HalfStaticObject.GetTransform().parent = enemyFromBuffer2.m_effectPoint;
						dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
						EffectPlayCombie component3 = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
						component3.StartEmit();
						if (!enemyFromBuffer2.m_specialEffects.ContainsKey(specialAttributeData.effects[0].type))
						{
							enemyFromBuffer2.m_specialEffects.Add(specialAttributeData.effects[0].type, dS2HalfStaticObject);
						}
					}
					break;
				}
				}
			}
		}

		protected void SetEliteSkillAttribute()
		{
			if (eliteType == EnemyEliteType.Rank1)
			{
				base.skillHitInfo.damage = new NumberSection<float>(base.skillHitInfo.damage.left * 2.5f, base.skillHitInfo.damage.right * 2.5f);
			}
			else if (eliteType == EnemyEliteType.Rank2)
			{
				base.skillHitInfo.damage = new NumberSection<float>(base.skillHitInfo.damage.left * 5f, base.skillHitInfo.damage.right * 5f);
			}
			else if (eliteType == EnemyEliteType.Rank3)
			{
				base.skillHitInfo.damage = new NumberSection<float>(base.skillHitInfo.damage.left * 10f, base.skillHitInfo.damage.right * 10f);
			}
		}

		public void StateToGuard()
		{
			AIStateChase aIStateChase = GetAIState("Chase") as AIStateChase;
			if (aIStateChase != null)
			{
				SwitchFSM(aIStateChase);
			}
		}

		public virtual void UpdateAnimationSpeed(bool isBoss)
		{
			string animationName = GetAnimationName("Move");
			float num = m_moveSpeed / 4f;
			if (isBoss)
			{
				float num2 = ((!this.isBoss) ? (0.8f * (1.3f / eliteScale)) : (0.8f * (1.3f / bossScale)));
				num *= num2;
			}
			SetAnimationSpeed(animationName, num);
		}

		public virtual void UpdateAnimationSpeed(string animName, float speed)
		{
			float num = m_moveSpeed / 4f;
			if (isBoss)
			{
				num *= 0.8f;
			}
			SetAnimationSpeed(animName, num);
		}

		protected void SetAttribute()
		{
			DataConf.EnemyData enemyDataByType = DataCenter.Conf().GetEnemyDataByType(enemyType);
			CharacterAttribute characterAttribute = default(CharacterAttribute);
			characterAttribute.moveSpeed = enemyDataByType.moveSpeed;
			characterAttribute.damage = enemyDataByType.damage;
			characterAttribute.proCritical = enemyDataByType.proCritical;
			characterAttribute.critDamage = enemyDataByType.critDamage;
			characterAttribute.proStun = enemyDataByType.proStun;
			characterAttribute.attackFrequence = enemyDataByType.attackPrequence;
			baseAttribute = characterAttribute;
			eliteScale = enemyDataByType.eliteScale;
			bossScale = enemyDataByType.bossScale;
			DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
			enemyName = enemyDataByType.name;
			int num2 = (base.hpMax = enemyDataByType.hp);
			base.hp = num2;
			MoveSpeed = enemyDataByType.moveSpeed;
			base.hitInfo.damage = baseAttribute.damage;
			base.hitInfo.repelTime = enemyDataByType.repelTime;
			base.hitInfo.repelDistance = enemyDataByType.repelDis;
			base.hitInfo.hitRate = 100;
			base.critRate = (ushort)baseAttribute.proCritical;
			base.critDamage = baseAttribute.critDamage;
			base.hitInfo.critRate = baseAttribute.proCritical;
			base.hitInfo.critDamage = baseAttribute.damage.left + baseAttribute.damage.left * base.critDamage;
			m_proFirstAttackLose = enemyDataByType.proFirstAttackLose;
			num2 = (base.hpMax = (int)((float)enemyDataByType.hp + (float)(enemyDataByType.hp * (currentGameLevelData.level - 1)) * 0.07f));
			base.hp = num2;
			if (enemyType == EnemyType.Zombie_Purple)
			{
				MoveSpeed *= 2f;
				hpMax *= 2;
				hp *= 2;
			}
			else if (enemyType == EnemyType.ZombieBomb_Purple)
			{
				MoveSpeed *= 2f;
			}
			base.hitInfo.damage = new NumberSection<float>(base.hitInfo.damage.left + base.hitInfo.damage.left * (float)(currentGameLevelData.level - 1) * 0.05f, base.hitInfo.damage.right + base.hitInfo.damage.right * (float)(currentGameLevelData.level - 1) * 0.05f);
			if (DataCenter.State().selectLevelMode == Defined.LevelMode.Hard)
			{
				MoveSpeed *= 1.15f;
			}
			else if (DataCenter.State().selectLevelMode == Defined.LevelMode.Hell)
			{
				MoveSpeed *= 1.25f;
			}
			m_hpNormal = base.hp;
			m_damageNormal = base.hitInfo.damage;
			m_speedNormal = MoveSpeed;
			base.hitInfo.critDamage = baseAttribute.damage.left + baseAttribute.damage.left * baseAttribute.critDamage;
			base.skillHitInfo.critRate = (int)base.critRate;
			base.skillInfos = enemyDataByType.skillInfos;
			m_attFrequency = baseAttribute.attackFrequence;
			baseAttribute.hpMax = base.hpMax;
			baseAttribute.moveSpeed = MoveSpeed;
			baseAttribute.damage = base.hitInfo.damage;
			baseAttribute.proCritical = base.hitInfo.critRate;
			baseAttribute.critDamage = base.hitInfo.critDamage;
		}

		public override void UseSkill(int skillId)
		{
			if (skillId >= 0 && skillId < base.skillInfos.Count)
			{
				base.UseSkill(skillId);
				base.currentSkill = base.skillInfos[skillId];
				OnReleaseSkillToBegin();
			}
		}

		public virtual void updateSkills()
		{
			if (base.currentSkill != null)
			{
				switch (m_releaseSkillState)
				{
				case ReleaseSkillState.Ready:
					OnReleaseSkillBegin();
					break;
				case ReleaseSkillState.Release:
					OnReleaseSkillUpdate();
					break;
				case ReleaseSkillState.End:
					OnReleaseSkillEnd();
					break;
				}
			}
		}

		public virtual void OnReleaseSkillToBegin()
		{
			m_releaseSkillState = ReleaseSkillState.Ready;
			if (base.currentSkill.animReady == null || base.currentSkill.animReady == string.Empty)
			{
				OnReleaseSkillToUpdate();
				return;
			}
			base.isRage = true;
			AnimationCrossFade(base.currentSkill.animReady, false);
			m_skillTimer = 0f;
			switch (base.currentSkill.type)
			{
			case SkillType.Dash:
			case SkillType.Grab:
				base.isRage = true;
				if (base.lockedTarget != null)
				{
					m_skillTarget = base.lockedTarget;
				}
				else
				{
					m_skillTarget = GameBattle.m_instance.GetNearestObjFromTargetList(this);
				}
				break;
			case SkillType.Jump:
				base.isRage = true;
				if (base.lockedTarget != null)
				{
					m_skillTarget = base.lockedTarget;
				}
				else
				{
					m_skillTarget = GameBattle.m_instance.GetNearestObjFromTargetList(this);
				}
				m_skillTargetPos = m_skillTarget.GetTransform().position;
				GetTransform().forward = m_skillTarget.GetTransform().position - GetTransform().position;
				break;
			case SkillType.Throw:
				m_skillTarget = base.lockedTarget;
				break;
			}
		}

		public virtual void OnReleaseSkillBegin()
		{
			if (!AnimationPlaying(base.currentSkill.animReady))
			{
				OnReleaseSkillToUpdate();
			}
		}

		public virtual void OnReleaseSkillToUpdate()
		{
			m_releaseSkillState = ReleaseSkillState.Release;
			if (base.currentSkill.animProcess == null || base.currentSkill.animProcess == string.Empty)
			{
				OnReleaseSkillToEnd();
				return;
			}
			AnimationCrossFade(base.currentSkill.animProcess, true, false, 0.1f);
			switch (base.currentSkill.type)
			{
			case SkillType.Dash:
				SetAttackCollider(true, AttackCollider.AttackColliderType.Dash);
				UpdateAnimationSpeed(base.currentSkill.animProcess, base.currentSkill.speed);
				break;
			case SkillType.Grab:
				SetAttackCollider(true, AttackCollider.AttackColliderType.Grab);
				UpdateAnimationSpeed(base.currentSkill.animProcess, base.currentSkill.speed);
				break;
			case SkillType.Jump:
				if (m_skillTarget != null)
				{
					m_skillMoveDir = m_skillTargetPos - GetTransform().position;
					Vector2 vector = new Vector2(m_skillTargetPos.x - GetTransform().position.x, m_skillTargetPos.z - GetTransform().position.z);
					m_skillTargetHorizDis = vector.magnitude;
					base.SpeedY = 16f;
					GetModelTransform().LookAt(m_skillTargetPos);
				}
				break;
			}
			SetSkillHitInfo();
		}

		public virtual void OnReleaseSkillUpdate()
		{
			switch (base.currentSkill.type)
			{
			case SkillType.Dash:
			case SkillType.Grab:
				m_skillTimer += Time.deltaTime;
				if (m_skillTimer >= base.currentSkill.time)
				{
					OnReleaseSkillToEnd();
				}
				else if (m_characterController != null)
				{
					m_characterController.Move(GetModelTransform().forward * base.currentSkill.speed * Time.deltaTime);
				}
				break;
			case SkillType.Jump:
			{
				if (!(m_characterController != null))
				{
					break;
				}
				float num = base.currentSkill.speed * Time.deltaTime;
				num = 25f * Time.deltaTime;
				Vector3 normalized = new Vector3(m_skillMoveDir.x, base.SpeedY, m_skillMoveDir.z).normalized;
				GetTransform().Translate(normalized * num, Space.World);
				if (base.SpeedY > 0f)
				{
					Vector2 vector = new Vector2(m_skillTargetPos.x - GetTransform().position.x, m_skillTargetPos.z - GetTransform().position.z);
					float magnitude = vector.magnitude;
					if (magnitude <= m_skillTargetHorizDis / 2f)
					{
						base.SpeedY *= -1f;
						string animName = base.currentSkill.animProcess.Substring(0, base.currentSkill.animProcess.IndexOf('_') + 1) + "c";
						if (HasAnimation(animName))
						{
							AnimationPlay(animName, false);
						}
					}
					break;
				}
				string animName2 = base.currentSkill.animProcess.Substring(0, base.currentSkill.animProcess.IndexOf('_') + 1) + "c";
				string text = base.currentSkill.animProcess.Substring(0, base.currentSkill.animProcess.IndexOf('_') + 1) + "d";
				if (HasAnimation(animName2) && !AnimationPlaying(animName2) && !AnimationPlaying(text))
				{
					AnimationCrossFade(text, true, false, 0.1f);
				}
				Vector2 vector2 = new Vector2(m_skillTargetPos.x - GetTransform().position.x, m_skillTargetPos.z - GetTransform().position.z);
				float magnitude2 = vector2.magnitude;
				if (GetTransform().position.y < 2f)
				{
					base.SpeedY = 0f;
					OnReleaseSkillToEnd();
				}
				break;
			}
			case SkillType.Throw:
				break;
			}
		}

		public virtual void OnReleaseSkillToEnd()
		{
			m_releaseSkillState = ReleaseSkillState.End;
			if (base.currentSkill.animProcess == null || base.currentSkill.animProcess == string.Empty)
			{
				m_releaseSkillState = ReleaseSkillState.Over;
				base.currentSkill = null;
				base.isRage = false;
				SkillEnd();
				return;
			}
			AnimationCrossFade(base.currentSkill.animEnd, false);
			switch (base.currentSkill.type)
			{
			case SkillType.Dash:
				SetAttackCollider(false, AttackCollider.AttackColliderType.Dash);
				break;
			case SkillType.Jump:
				GetNavMeshAgent().enabled = true;
				ResumeNav();
				m_characterController.enabled = true;
				break;
			case SkillType.Throw:
				break;
			case SkillType.Grab:
				SetAttackCollider(false, AttackCollider.AttackColliderType.Grab);
				break;
			}
		}

		public virtual void OnReleaseSkillEnd()
		{
			if (!AnimationPlaying(base.currentSkill.animEnd))
			{
				m_releaseSkillState = ReleaseSkillState.Over;
				base.currentSkill = null;
				base.isRage = false;
				SkillEnd();
			}
		}

		protected virtual void SetSkillHitInfo()
		{
			DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
			base.skillHitInfo.repelDistance = new NumberSection<float>(base.currentSkill.repelDis, base.currentSkill.repelDis);
			base.skillHitInfo.repelTime = base.currentSkill.repelTime;
			base.skillHitInfo.damage = new NumberSection<float>(base.currentSkill.damage, base.currentSkill.damage);
			base.skillHitInfo.damage = new NumberSection<float>(base.skillHitInfo.damage.left + (float)((currentGameLevelData.level - 1) * 1), base.skillHitInfo.damage.left + (float)((currentGameLevelData.level - 1) * 3));
			SetEliteSkillAttribute();
			base.skillHitInfo.critDamage = base.skillHitInfo.damage.left + base.skillHitInfo.damage.left * base.critDamage;
			base.skillHitInfo.percentDamage = base.currentSkill.percentDamage;
		}

		public override HitResultInfo OnHit(HitInfo hitInfo)
		{
			if (hitInfo.repelDistance.left > 0f)
			{
				if (isBoss)
				{
					hitInfo.repelDistance = new NumberSection<float>(hitInfo.repelDistance.left - hitInfo.repelDistance.left * 0.7f, hitInfo.repelDistance.right - hitInfo.repelDistance.right * 0.7f);
				}
				else if (eliteType != 0)
				{
					hitInfo.repelDistance = new NumberSection<float>(hitInfo.repelDistance.left - hitInfo.repelDistance.left * 0.5f, hitInfo.repelDistance.right - hitInfo.repelDistance.right * 0.5f);
				}
			}
			AIStateHurt aIStateHurt = GetAIState("Hurt") as AIStateHurt;
			AIStateRepel aIStateRepel = GetAIState("Repel") as AIStateRepel;
			float num = 0f;
			if (isBoss)
			{
				num = 0f + (float)(hitInfo.hitStrength - 1) * 0.2f / 4f;
				num = Mathf.Clamp(num, 0f, 0.2f);
			}
			else if (eliteType != 0)
			{
				num = 0.1f + (float)(hitInfo.hitStrength - 1) * 0.1f / 4f;
				num = Mathf.Clamp(num, 0.1f, 0.2f);
			}
			else
			{
				num = 0.3f + (float)(hitInfo.hitStrength - 1) * 0.39999998f / 4f;
				num = Mathf.Clamp(num, 0.3f, 0.7f);
			}
			if (aIStateHurt != null)
			{
				aIStateHurt.HurtTime = num;
			}
			if (aIStateRepel != null)
			{
				aIStateRepel.HurtTime = num;
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.RangeWeaken))
			{
				DoSpecialAttributeOnBeforeHit(SpecialAttribute.AttributeType.RangeWeaken, hitInfo);
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Territory))
			{
				DoSpecialAttributeOnBeforeHit(SpecialAttribute.AttributeType.Territory, hitInfo);
			}
			HitResultInfo hitResultInfo = base.OnHit(hitInfo);
			if (!Alive() || !Visible)
			{
				return hitResultInfo;
			}
			if (hitResultInfo.isHit)
			{
				if (links != null && links.ContainsKey(Defined.ObjectLinkType.ShareLife))
				{
					for (int i = 0; i < links[Defined.ObjectLinkType.ShareLife].Count; i++)
					{
						links[Defined.ObjectLinkType.ShareLife][i].hp = base.hp;
						if (!links[Defined.ObjectLinkType.ShareLife][i].Alive())
						{
							links[Defined.ObjectLinkType.ShareLife][i].OnDeath();
						}
					}
				}
				if (specialAttribute.Contains(SpecialAttribute.AttributeType.Electric))
				{
					DoSpecialAttributeOnHit(SpecialAttribute.AttributeType.Electric);
				}
				if (specialAttribute.Contains(SpecialAttribute.AttributeType.Endure))
				{
					DoSpecialAttributeOnHit(SpecialAttribute.AttributeType.Endure);
				}
				if (specialAttribute.Contains(SpecialAttribute.AttributeType.Bloodsucking))
				{
					DoSpecialAttributeOnHit(SpecialAttribute.AttributeType.Bloodsucking);
				}
				if (specialAttribute.Contains(SpecialAttribute.AttributeType.Split))
				{
					DoSpecialAttributeOnHit(SpecialAttribute.AttributeType.Split);
				}
			}
			if (hitResultInfo.isCirt && specialAttribute.Contains(SpecialAttribute.AttributeType.StrickBack))
			{
				DoSpecialAttributeOnHit(SpecialAttribute.AttributeType.StrickBack);
			}
			m_hpBar.gameObject.SetActive(true);
			m_showHpBarTimer = 0f;
			return hitResultInfo;
		}

		public override void HitResult(HitResultInfo result)
		{
			base.HitResult(result);
			if (!m_recieveSpecialAttributeCount.ContainsKey(SpecialAttribute.AttributeType.Vampire))
			{
				return;
			}
			foreach (SpecialAttribute.SpecialAttributeGameData value in m_recieveSpecialAttribute.Values)
			{
				if (value.type == SpecialAttribute.AttributeType.Vampire)
				{
					value.creater.hp += (int)result.damage;
					EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Heal, (int)result.damage, value.creater.m_effectPoint.position);
					DS2HalfStaticObject dS2HalfStaticObject = BattleBufferManager.Instance.GetSpecialEffectByType(DataCenter.Conf().m_specialAttributeVampire.effects[0].type) as DS2HalfStaticObject;
					if (dS2HalfStaticObject != null)
					{
						dS2HalfStaticObject.GetTransform().parent = value.creater.m_effectPoint;
						dS2HalfStaticObject.GetTransform().localPosition = Vector3.zero;
						dS2HalfStaticObject.GetTransform().localRotation = Quaternion.identity;
						EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
						component.StartEmit();
					}
				}
			}
		}

		public override HitInfo GetHitInfo()
		{
			HitInfo hitInfo = new HitInfo(base.GetHitInfo());
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.PoisonClaw))
			{
				int num = UnityEngine.Random.Range(0, 100);
				if (num < DataCenter.Conf().m_specialAttributePoisonClaw.probability)
				{
					float num2 = UnityEngine.Random.Range(DataCenter.Conf().m_specialAttributePoisonClaw.damage.left, DataCenter.Conf().m_specialAttributePoisonClaw.damage.right);
					if (num2 > 0f)
					{
						num2 *= -1f;
					}
					float duration = DataCenter.Conf().m_specialAttributePoisonClaw.duration;
					hitInfo.buffs.Clear();
					hitInfo.buffs.Add(new Buff(Buff.AffectType.Poison, num2, 1f, duration, Buff.CalcType.Percentage, 0f));
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Exacerbate))
			{
				int num3 = UnityEngine.Random.Range(0, 100);
				if (num3 < DataCenter.Conf().m_specialAttributeExacerbate.probability)
				{
					float value = UnityEngine.Random.Range(DataCenter.Conf().m_specialAttributeExacerbate.damage.left, DataCenter.Conf().m_specialAttributeExacerbate.damage.right);
					float duration2 = DataCenter.Conf().m_specialAttributeExacerbate.duration;
					hitInfo.buffs.Clear();
					hitInfo.buffs.Add(new Buff(Buff.AffectType.Exacerbate, value, -1f, duration2));
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Knell))
			{
				int num4 = UnityEngine.Random.Range(0, 100);
				if (num4 < DataCenter.Conf().m_specialAttributeKnell.probability)
				{
					float time = 30f;
					hitInfo.buffs.Clear();
					Buff buff = new Buff(Buff.AffectType.Knell, 0f, -1f, time);
					buff.creater = this;
					hitInfo.buffs.Add(buff);
				}
			}
			if (specialAttribute.Contains(SpecialAttribute.AttributeType.Grind))
			{
				hitInfo.buffs.Clear();
				float duration3 = DataCenter.Conf().m_specialAttributeGrind.duration;
				float reduceAtt = DataCenter.Conf().m_specialAttributeGrind.reduceAtt;
				float reduceMoveSpeed = DataCenter.Conf().m_specialAttributeGrind.reduceMoveSpeed;
				Buff item = new Buff(Buff.AffectType.ReduceATK, reduceAtt, duration3, duration3, Buff.CalcType.Percentage, 0f);
				Buff item2 = new Buff(Buff.AffectType.MoveSpeed, reduceMoveSpeed, duration3, duration3, Buff.CalcType.Percentage, 0f);
				hitInfo.buffs.Add(item);
				hitInfo.buffs.Add(item2);
			}
			return hitInfo;
		}

		protected bool CheckUseSkill(ref float time)
		{
			if (time < m_useSkillCD)
			{
				return false;
			}
			time = 0f;
			bool flag = m_usedSkillCount == 0;
			m_usedSkillCount = ((m_usedSkillCount++ <= DataConf.s_enemyRandomSkillInterval) ? m_usedSkillCount : 0);
			if (!flag)
			{
				flag = (float)UnityEngine.Random.Range(0, 100) < DataConf.s_enemyRandomSkillOdds;
			}
			return flag;
		}

		public virtual void OnMelee(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
			{
				IPathFinding pathFinding = GetPathFinding();
				if (pathFinding != null && pathFinding.HasNavigation())
				{
					pathFinding.StopNav();
				}
				m_attackAnimName = GetAnimationName("Attack");
				AnimationStop(base.animUpperBody);
				int num2 = UnityEngine.Random.Range(0, 100);
				if ((float)num2 < m_proFirstAttackLose)
				{
					m_waitAttackAnimName = GetAnimationName("Idle");
					AnimationCrossFade(m_waitAttackAnimName, true);
				}
				else
				{
					AnimationCrossFade(m_attackAnimName, false);
				}
				if (base.lockedTarget != null)
				{
					LookAt(base.lockedTarget.GetTransform());
				}
				m_attackTimer = 0f;
				break;
			}
			case AIState.AIPhase.Update:
			{
				DS2ActiveObject nearestObjFromTargetList = GameBattle.m_instance.GetNearestObjFromTargetList(GetTransform().position, base.clique);
				if (nearestObjFromTargetList != null)
				{
					float num = base.meleeRange * base.meleeRange;
					float sqrMagnitude = (nearestObjFromTargetList.GetTransform().position - GetTransform().position).sqrMagnitude;
					if (sqrMagnitude > num && !AnimationPlaying(m_attackAnimName))
					{
						ChangeAIState("Chase");
						break;
					}
				}
				m_attackTimer += Time.deltaTime;
				if (m_attackTimer >= m_attFrequency)
				{
					if (!AnimationPlaying(m_attackAnimName))
					{
						if (base.shootAble)
						{
							ChangeAIState("Shoot");
							break;
						}
						m_attackTimer = 0f;
						LookAt(nearestObjFromTargetList.GetTransform());
						m_attackAnimName = GetAnimationName("Attack");
						AnimationPlay(m_attackAnimName, false);
					}
				}
				else if (!AnimationPlaying(m_attackAnimName))
				{
					m_waitAttackAnimName = GetAnimationName("Idle");
					AnimationCrossFade(m_waitAttackAnimName, true);
				}
				break;
			}
			case AIState.AIPhase.Exit:
				SetAttackCollider(false);
				break;
			}
		}

		public virtual void OnSpecialAttack(AIState.AIPhase phase)
		{
		}

		public void SetPlayerModelOutLineEffectVisable(bool bShow, GameObject go, Color color, float outLine = 5f)
		{
			SkinnedMeshRenderer[] componentsInChildren = go.GetComponentsInChildren<SkinnedMeshRenderer>();
			MeshRenderer[] componentsInChildren2 = go.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (bShow)
				{
					Material material = componentsInChildren[i].materials[0];
					Material material2 = new Material(material);
					material2.shader = Shader.Find("Triniti/Model/CartoonRenderingCharacter");
					material2.SetColor("_OutlineColor", color);
					material2.SetFloat("_Outline", outLine);
					Material[] materials = new Material[2] { material2, material };
					componentsInChildren[i].materials = materials;
				}
				else
				{
					Material material3 = componentsInChildren[i].materials[1];
					Material[] materials2 = new Material[1] { material3 };
					componentsInChildren[i].materials = materials2;
				}
			}
		}
	}
}

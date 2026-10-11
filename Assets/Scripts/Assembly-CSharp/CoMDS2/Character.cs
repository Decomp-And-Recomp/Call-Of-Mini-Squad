using System.Collections.Generic;
using UnityEngine;

namespace CoMDS2
{
	public class Character : DS2ActiveObject, IPathFinding
	{
		public struct CharacterAttribute
		{
			public int hpMax;

			public float moveSpeed;

			public NumberSection<float> damage;

			public float proCritical;

			public float attackFrequence;

			public float critDamage;

			public int proStun;
		}

		public enum SkillType
		{
			Dash = 0,
			Jump = 1,
			Throw = 2,
			Grab = 3
		}

		public class MoveAnimation
		{
			public string animName;

			public Vector3 velocity;

			public float weight;

			public bool currentBest;

			public float speed;

			public float angle;

			public void Init()
			{
				speed = velocity.magnitude;
				angle = MathEx.HorizontalAngle(velocity);
			}
		}

		public CharacterAttribute baseAttribute;

		protected float m_skillTotalCDTime;

		protected float m_skillCDTime;

		private GameObject m_shadowGameObject;

		private GameObject m_hpBarGameObject;

		public bool m_move;

		private Vector3 m_moveDirection;

		public float m_moveSpeed;

		private float m_lastMoveSpeed;

		protected float m_speedY;

		public bool m_fire;

		public Vector3 m_fireDirection;

		private Vector3 m_faceDirection;

		protected Dictionary<int, Weapon> m_weaponMap;

		public Weapon m_weapon;

		public Weapon m_weaponChange;

		public int m_weaponIndex;

		public float m_godTime;

		public float m_unableRepelTime;

		private bool m_bStuck;

		private bool m_bRage;

		private bool m_bFrozen;

		private bool m_bNegativeAffectEnable = true;

		protected List<Character> m_grabList = new List<Character>();

		public Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>> links;

		private BuffManager m_buffManager;

		protected int m_iRankAnimIndex;

		private bool m_bInReload;

		public int m_level;

		public int m_rank;

		private Dictionary<AttackCollider.AttackColliderType, List<AttackCollider>> m_attackCollider;

		private UnityEngine.AI.NavMeshAgent m_navMeshAgent;

		protected CharacterController m_characterController;

		public float radius;

		private float m_tempTime;

		private Vector3 velocity = Vector3.zero;

		private Vector3 lastPosition = Vector3.zero;

		private Vector3 localVelocity = Vector3.zero;

		private float speed;

		private float angle;

		protected MoveAnimation[] moveAnimations;

		private MoveAnimation bestAnimation;

		protected bool m_fixedWeapon;

		private float m_skillReadyTimer;

		public float reduceDamage { get; set; }

		public float addDamage { get; set; }

		public ushort hitRate { get; set; }

		public ushort dodgeRate { get; set; }

		public ushort critRate { get; set; }

		public float critDamage { get; set; }

		public bool isEndure { get; set; }

		public bool isIgnoreSkillDamage { get; set; }

		public virtual float AtkFrequency
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public List<DataConf.SkillInfo> skillInfos { get; set; }

		public DataConf.SkillInfo currentSkill { get; set; }

		public HitInfo skillHitInfo { get; set; }

		public bool skillAttack { get; set; }

		public static Transform s_spawnTransform { get; set; }

		public bool shootAble { get; set; }

		public float shootRange { get; set; }

		public bool meleeAble { get; set; }

		public float meleeRange { get; set; }

		public DS2ActiveObject lockedTarget { get; set; }

		public AudioManager audioManager { get; set; }

		public virtual Vector3 FaceDirection
		{
			get
			{
				return m_faceDirection;
			}
			set
			{
				m_faceDirection = value;
			}
		}

		public virtual Vector3 MoveDirection
		{
			get
			{
				return m_moveDirection;
			}
			set
			{
				m_moveDirection = value;
			}
		}

		public virtual float MoveSpeed
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
				UpdateAnimationSpeed();
			}
		}

		public virtual float LastMoveSpeed
		{
			get
			{
				return m_lastMoveSpeed;
			}
			set
			{
				m_lastMoveSpeed = value;
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
				if (m_shadowGameObject != null)
				{
					m_shadowGameObject.SetActive(base.Visible);
				}
			}
		}

		public bool isStuck
		{
			get
			{
				return m_bStuck;
			}
			set
			{
				m_bStuck = value;
			}
		}

		public bool isRage
		{
			get
			{
				return m_bRage || isEndure;
			}
			set
			{
				m_bRage = value;
			}
		}

		public float SpeedY
		{
			get
			{
				return m_speedY;
			}
			set
			{
				m_speedY = value;
				if (m_speedY == 0f)
				{
					m_characterController.enabled = true;
					GetNavMeshAgent().enabled = true;
					ResumeNav();
				}
				else if (GetNavMeshAgent().enabled)
				{
					StopNav();
					GetNavMeshAgent().enabled = false;
					m_characterController.enabled = false;
				}
			}
		}

		public bool isFrozen
		{
			get
			{
				return m_bFrozen;
			}
			set
			{
				m_bFrozen = value;
			}
		}

		public bool isInReload
		{
			get
			{
				return m_bInReload;
			}
			set
			{
				m_bInReload = value;
			}
		}

		public virtual bool MoveAble
		{
			get
			{
				return !isStuck && !isFrozen && m_unableRepelTime <= 0f;
			}
		}

		public virtual bool NegativeAffectEnable
		{
			get
			{
				return m_bNegativeAffectEnable;
			}
			set
			{
				m_bNegativeAffectEnable = value;
			}
		}

		public int CurrentRandAnimIndex
		{
			get
			{
				return m_iRankAnimIndex;
			}
		}

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			CreateCharacterAttribute();
			m_move = false;
			MoveDirection = Vector3.zero;
			m_fire = false;
			m_fireDirection = Vector3.zero;
			m_navMeshAgent = GetGameObject().GetComponent<UnityEngine.AI.NavMeshAgent>();
			Transform transform = GetTransform().Find("Shadow");
			if (transform != null)
			{
				m_shadowGameObject = transform.gameObject;
			}
			if (m_buffManager == null)
			{
				m_buffManager = new BuffManager(this);
			}
			if (audioManager == null)
			{
				audioManager = new AudioManager(this);
			}
			skillHitInfo = new HitInfo();
			skillHitInfo.bSkill = true;
			skillHitInfo.source = this;
			isEndure = false;
			isIgnoreSkillDamage = false;
		}

		public virtual void SetFaceDirectionImmediately(Vector3 direction)
		{
			FaceDirection = direction;
		}

		protected virtual void CreateCharacterAttribute()
		{
			m_effectPoint = GetTransform().Find("EffectPointCenter");
			if (m_effectPoint == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "EffectPointCenter";
				gameObject.transform.parent = GetTransform();
				gameObject.transform.localPosition = new Vector3(0f, 0.8f, 0f);
				m_effectPoint = gameObject.transform;
			}
			m_effectPointFoward = GetTransform().Find("EffectPointFront");
			if (m_effectPointFoward == null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "EffectPointFront";
				gameObject2.transform.parent = GetTransform();
				gameObject2.transform.localPosition = new Vector3(0f, 0.8f, 0.5f);
				m_effectPointFoward = gameObject2.transform;
			}
			m_effectPointGround = GetTransform().Find("EffectPointGround");
			if (m_effectPointGround == null)
			{
				GameObject gameObject3 = new GameObject();
				gameObject3.name = "EffectPointGround";
				gameObject3.AddComponent<AutoOnLand>();
				gameObject3.transform.parent = GetTransform();
				gameObject3.transform.localPosition = Vector3.zero;
				m_effectPointGround = gameObject3.transform;
			}
			m_effectPointUpHead = GetTransform().Find("EffectPointUpHead");
			if (m_effectPointUpHead == null)
			{
				GameObject gameObject4 = new GameObject();
				gameObject4.name = "EffectPointUpHead";
				gameObject4.AddComponent<AutoOnLand>();
				gameObject4.transform.parent = GetTransform();
				gameObject4.transform.localPosition = new Vector3(0f, 2.1f, 0.5f);
				m_effectPointUpHead = gameObject4.transform;
			}
			AttackCollider[] componentsInChildren = GetGameObject().GetComponentsInChildren<AttackCollider>();
			if (componentsInChildren != null)
			{
				int num = componentsInChildren.Length;
				m_attackCollider = new Dictionary<AttackCollider.AttackColliderType, List<AttackCollider>>();
				for (int i = 0; i < num; i++)
				{
					componentsInChildren[i].belong = GetGameObject();
					componentsInChildren[i].gameObject.SetActive(false);
					AddAttackCollider(componentsInChildren[i]);
				}
			}
			m_characterController = GetGameObject().GetComponent<CharacterController>();
			radius = m_characterController.radius;
			SetAnimationsMixing();
		}

		protected virtual string DebugExtraInfo()
		{
			return string.Empty;
		}

		public override void Update(float deltaTime)
		{
#if UNITY_EDITOR
			DS2ObjectStub debugStub = GetGameObject().GetComponent<DS2ObjectStub>();
			if (debugStub != null)
			{
				AIState debugState = GetCurrentAIState();
				debugStub.RefreshDebug(debugState != null ? debugState.name : "(none)", base.hp, base.hpMax, isRage, m_godTime > 0f, isStuck, 0, DebugExtraInfo());
			}
#endif
			if (m_godTime > 0f)
			{
				m_godTime -= deltaTime;
			}
			if (m_unableRepelTime > 0f)
			{
				m_unableRepelTime -= deltaTime;
			}
			if (m_skillCDTime > 0f)
			{
				m_skillCDTime -= deltaTime;
				m_skillCDTime = Mathf.Max(0f, m_skillCDTime);
			}
			if (m_buffManager != null && Alive())
			{
				m_buffManager.UpdateBuffs();
			}
			AIState currentAIState = GetCurrentAIState();
			base.Update(deltaTime);
			if (m_weapon != null)
			{
				m_weapon.Update(m_fire);
			}
		}

		private void UpdateSplash()
		{
		}

		public Transform GetSpawnTransform()
		{
			return s_spawnTransform;
		}

		public virtual void SetMove(bool move, Vector3 moveDirection)
		{
			m_move = move;
			MoveDirection = moveDirection;
		}

		public void SetMove(float speed, Vector3 moveDirection)
		{
			MoveSpeed = speed;
			MoveDirection = moveDirection;
		}

		public virtual void UpdateAnimationSpeed()
		{
		}

		public virtual void SetFire(bool fire, Vector3 fireDirection)
		{
			if (m_weapon == null)
			{
				return;
			}
			AIState currentAIState = GetCurrentAIState();
			if (isInReload)
			{
				return;
			}
			m_fire = fire;
			m_fireDirection = fireDirection;
			if (!fire)
			{
				if (m_weapon.rapidFiring)
				{
					m_fire = true;
				}
				else
				{
					m_weapon.StopFire();
				}
			}
		}

		public override void OnCollide(ICollider collider)
		{
		}

		public virtual void SetAttackCollider(bool enable, AttackCollider.AttackColliderType type = AttackCollider.AttackColliderType.Normal, int index = -1)
		{
			if (!m_attackCollider.ContainsKey(type) || m_attackCollider[type].Count <= 0)
			{
				return;
			}
			int num = 0;
			foreach (AttackCollider item in m_attackCollider[type])
			{
				if (index == -1 || index == num)
				{
					item.clique = base.clique;
					if (base.clique == Clique.Player)
					{
						item.gameObject.layer = 14;
					}
					else if (base.clique == Clique.Computer)
					{
						item.gameObject.layer = 15;
					}
					item.gameObject.SetActive(enable);
					num++;
				}
			}
		}

		public virtual void AddAttackCollider(AttackCollider attackCollider)
		{
			if (m_attackCollider.ContainsKey(attackCollider.type))
			{
				m_attackCollider[attackCollider.type].Add(attackCollider);
				return;
			}
			List<AttackCollider> list = new List<AttackCollider>();
			list.Add(attackCollider);
			m_attackCollider.Add(attackCollider.type, list);
		}

		public virtual void RemoveAttackCollider(AttackCollider attackCollider)
		{
			if (m_attackCollider.ContainsKey(attackCollider.type))
			{
				m_attackCollider[attackCollider.type].Remove(attackCollider);
				if (m_attackCollider[attackCollider.type].Count == 0)
				{
					m_attackCollider.Remove(attackCollider.type);
				}
			}
		}

		public override void Destroy(bool destroy = false)
		{
			base.Destroy(destroy);
			if (!destroy)
			{
				return;
			}
			m_attackCollider = null;
			m_navMeshAgent = null;
			if (m_weaponMap == null)
			{
				return;
			}
			foreach (int key in m_weaponMap.Keys)
			{
				m_weaponMap[key].Dispose();
			}
			m_weaponMap.Clear();
			m_weaponMap = null;
		}

		public virtual void SetGodTime(float time)
		{
			m_godTime = time;
		}

		public override HitInfo GetHitInfo()
		{
			if (skillAttack)
			{
				return skillHitInfo;
			}
			return base.GetHitInfo();
		}

		public virtual void LinkToShareLife(Character target)
		{
			if (links == null)
			{
				links = new Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>>();
			}
			if (links.ContainsKey(Defined.ObjectLinkType.ShareLife))
			{
				links[Defined.ObjectLinkType.ShareLife].Add(target);
			}
			else
			{
				List<DS2ActiveObject> list = new List<DS2ActiveObject>();
				list.Add(target);
				links.Add(Defined.ObjectLinkType.ShareLife, list);
			}
			if (target.links == null)
			{
				target.links = new Dictionary<Defined.ObjectLinkType, List<DS2ActiveObject>>();
			}
			if (target.links.ContainsKey(Defined.ObjectLinkType.ShareLife))
			{
				target.links[Defined.ObjectLinkType.ShareLife].Add(this);
				return;
			}
			List<DS2ActiveObject> list2 = new List<DS2ActiveObject>();
			list2.Add(this);
			target.links.Add(Defined.ObjectLinkType.ShareLife, list2);
		}

		public string GetAnimationName(string animName, int count = 1)
		{
			if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				return GetAnimationNameByWeapon(animName, count);
			}
			if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
			{
				string empty = string.Empty;
				string enemyAnimTag = ((Enemy)this).enemyAnimTag;
				DataConf.AnimData enemyAnim = DataCenter.Conf().GetEnemyAnim(enemyAnimTag, animName);
				if (enemyAnim == null)
				{
					return string.Empty;
				}
				if (enemyAnim.count > 1)
				{
					return enemyAnim.name + "0" + Random.Range(1, enemyAnim.count + 1);
				}
				return enemyAnim.name;
			}
			return string.Empty;
		}

		protected virtual void InitMoveAnimations()
		{
		}

		public void UpdateLowerBodyMoveDir4()
		{
			float num = Vector3.Angle(FaceDirection, MoveDirection);
			if (num < 45f)
			{
				base.animLowerBody = GetAnimationNameByWeapon("Lower_Body_Run_F");
			}
			else if (num > 135f)
			{
				base.animLowerBody = GetAnimationNameByWeapon("Lower_Body_Run_B");
			}
			else
			{
				if (moveAnimations == null)
				{
					return;
				}
				velocity = (GetTransform().position - lastPosition) / Time.deltaTime;
				localVelocity = GetTransform().InverseTransformDirection(velocity);
				localVelocity.y = 0f;
				speed = localVelocity.magnitude;
				num = MathEx.HorizontalAngle(localVelocity);
				lastPosition = GetTransform().position;
				if (speed > 0f)
				{
					float num2 = float.PositiveInfinity;
					for (int i = 0; i < moveAnimations.Length; i++)
					{
						MoveAnimation moveAnimation = moveAnimations[i];
						float num3 = Mathf.Abs(Mathf.DeltaAngle(num, moveAnimation.angle));
						float num4 = Mathf.Abs(speed - moveAnimation.speed);
						float num5 = num3 + num4;
						if (moveAnimation == bestAnimation)
						{
							num5 *= 0.9f;
						}
						if (num5 < num2)
						{
							bestAnimation = moveAnimation;
							num2 = num5;
						}
					}
				}
				base.animLowerBody = GetAnimationNameByWeapon(bestAnimation.animName);
			}
		}

		public void AddWeapon(int index, Weapon weapon)
		{
			if (!m_weaponMap.ContainsKey(index))
			{
				weapon.SetActive(false);
				weapon.owner = this;
				weapon.Mount(m_transform, m_transform.Find("Bip01/Spine_00/Bip01 Spine/Spine_0/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 Prop1"), GetTransform().Find("FireLightPoint"));
				m_weaponMap.Add(index, weapon);
			}
		}

		protected virtual void SetAnimationsMixing()
		{
		}

		public void RemoveWeapon(int index)
		{
			if (m_weaponMap.ContainsKey(index))
			{
				Weapon weapon = m_weaponMap[index];
				weapon.SetActive(false);
				weapon.Unmount();
				if (m_weapon == weapon)
				{
					m_weapon = null;
				}
				m_weaponMap.Remove(index);
			}
		}

		public virtual void UseWeapon(int index)
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
				base.hitInfo.damage = m_weapon.attribute.damage;
				base.hitInfo.repelTime = 0.2f;
				base.hitInfo.repelDistance = m_weapon.attribute.repelDis;
				shootRange = m_weapon.attribute.attackRange;
				if (!m_fixedWeapon)
				{
					UpdateWeaponAnimation();
				}
			}
		}

		public void ChangeWeapon(bool forward = true, int index = -1)
		{
			if (index != -1)
			{
				if (m_weaponMap.ContainsKey(index))
				{
					m_weaponIndex = index;
					m_weaponChange = m_weaponMap[index];
				}
			}
			else if (forward)
			{
				m_weaponIndex++;
				if (m_weaponIndex >= m_weaponMap.Count)
				{
					m_weaponIndex = 0;
				}
			}
			else
			{
				m_weaponIndex--;
				if (m_weaponIndex < 0)
				{
					m_weaponIndex = m_weaponMap.Count - 1;
				}
			}
			SwitchFSM(GetAIState("Shift"));
		}

		public void DoChangeWeapon()
		{
			UseWeapon(m_weaponIndex);
		}

		public void UpdateWeaponAnimation()
		{
			if ((base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY) && m_weapon != null)
			{
				string text = (base.animLowerBody = GetAnimationNameByWeapon("Idle"));
				string text2 = text;
				if (text2 != string.Empty)
				{
					AnimationPlay(text2, true);
				}
			}
		}

		public virtual int GetAnimationCountByName(string animName)
		{
			if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				string characterName = ((Player)this).name;
				DataConf.AnimData newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(characterName, animName);
				return newCharacterAnim.count;
			}
			if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
			{
				string enemyAnimTag = ((Enemy)this).enemyAnimTag;
				DataConf.AnimData enemyAnim = DataCenter.Conf().GetEnemyAnim(enemyAnimTag, animName);
				return enemyAnim.count;
			}
			return 0;
		}

		public virtual string GetAnimationNameByWeapon(string animName, int count = 1)
		{
			string empty = string.Empty;
			DataConf.AnimData newCharacterAnim;
			if (m_fixedWeapon)
			{
				string characterName = ((Player)this).name;
				newCharacterAnim = DataCenter.Conf().GetNewCharacterAnim(characterName, animName);
				if (newCharacterAnim == null)
				{
					return string.Empty;
				}
				if (newCharacterAnim.count > 1)
				{
					m_iRankAnimIndex = Random.Range(1, newCharacterAnim.count + 1);
					empty = newCharacterAnim.name + "0" + m_iRankAnimIndex;
					m_iRankAnimIndex--;
				}
				else
				{
					empty = newCharacterAnim.name;
				}
				return empty;
			}
			newCharacterAnim = DataCenter.Conf().GetCharacterAnim(animName);
			if (newCharacterAnim == null || m_weapon == null)
			{
				return string.Empty;
			}
			int num = ((count != 1) ? count : newCharacterAnim.count);
			string prefixNameByWeaponType = GetPrefixNameByWeaponType(m_weapon.type);
			string text = ((!newCharacterAnim.name.Contains("Rifle")) ? prefixNameByWeaponType : string.Empty);
			if (num > 1)
			{
				m_iRankAnimIndex = Random.Range(1, num + 1);
				empty = text + newCharacterAnim.name + "0" + m_iRankAnimIndex;
				m_iRankAnimIndex--;
			}
			else
			{
				empty = text + newCharacterAnim.name;
			}
			return empty;
		}

		public static string GetAnimationNameByWeapon(string animName, Weapon.WeaponType type)
		{
			string empty = string.Empty;
			string prefixNameByWeaponType = GetPrefixNameByWeaponType(type);
			return prefixNameByWeaponType + "_" + animName;
		}

		public static string GetPrefixNameByWeaponType(Weapon.WeaponType type)
		{
			string result = string.Empty;
			switch (type)
			{
			case Weapon.WeaponType.Grenade_01:
				result = "Grenade";
				break;
			case Weapon.WeaponType.ShotGun_01:
			case Weapon.WeaponType.ShotGun_02:
			case Weapon.WeaponType.ShotGun_04:
				result = "ShotGun";
				break;
			case Weapon.WeaponType.Sniper_03:
				result = "Sniper";
				break;
			case Weapon.WeaponType.Pistol_04:
				result = "Pistol";
				break;
			case Weapon.WeaponType.Laser_02:
				result = "Laser";
				break;
			case Weapon.WeaponType.Machinegun2:
			case Weapon.WeaponType.Machinegun4:
				result = "MachineGun";
				break;
			case Weapon.WeaponType.Ninjia_Fire_03:
			case Weapon.WeaponType.Ninjia_Ice_02:
				result = "Ninjia";
				break;
			}
			return result;
		}

		public override HitResultInfo OnHit(HitInfo hitInfo)
		{
			HitResultInfo hitResultInfo = new HitResultInfo();
			hitResultInfo.target = this;
			if (!Alive() || !Visible)
			{
				if (hitInfo.source != null)
				{
					hitInfo.source.HitResult(hitResultInfo);
				}
				return hitResultInfo;
			}
			if (Util.s_cheatGodMode && base.clique == Clique.Player)
			{
				if (hitInfo.source != null)
				{
					hitInfo.source.HitResult(hitResultInfo);
				}
				EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Ignore, 0f, m_effectPoint.position);
				return hitResultInfo;
			}
			if (m_godTime > 0f)
			{
				if (hitInfo.source != null)
				{
					hitInfo.source.HitResult(hitResultInfo);
				}
				if (m_godTime > 0f)
				{
					EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Ignore, 0f, m_effectPoint.position);
				}
				return hitResultInfo;
			}
			if (hitInfo.bSkill && ChestBuffs.SkillDamageBonus > 0f && base.clique == Clique.Computer)
			{
				hitInfo = new HitInfo(hitInfo);
				hitInfo.damage = new NumberSection<float>(hitInfo.damage.left * (1f + ChestBuffs.SkillDamageBonus), hitInfo.damage.right * (1f + ChestBuffs.SkillDamageBonus));
			}
			if (hitInfo.bSkill && isIgnoreSkillDamage)
			{
				EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Ignore, 0f, m_effectPoint.position);
				if (hitInfo.source != null)
				{
					hitInfo.source.HitResult(hitResultInfo);
				}
				return hitResultInfo;
			}
			if (GetBuffManager().HasBuff(Buff.AffectType.Exacerbate))
			{
				Buff buff = GetBuffManager().GetBuff(Buff.AffectType.Exacerbate);
				hitInfo.damage = new NumberSection<float>(hitInfo.damage.left * buff.GetValue(), hitInfo.damage.right * buff.GetValue());
			}
			int num = Random.Range(0, 100);
			int num2 = hitInfo.hitRate - dodgeRate;
			if (num >= num2 && !hitInfo.bSkill)
			{
				EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Miss, 0f, m_effectPoint.position);
				return hitResultInfo;
			}
			hitResultInfo = GetHitDamage(hitInfo);
			if (hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA) && GetCurrentAIState().name == "Frozen" && Random.Range(0, 100) < hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA].specialHitProbability)
			{
				hitResultInfo.damage = Mathf.Floor(hitResultInfo.damage * (hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA].specialHitParam + 1f));
			}
			if (hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.CAMERA_QUAKE))
			{
				GameBattle.m_instance.SetCameraQuake(Defined.CameraQuakeType.Quake_D);
			}
			if (hitResultInfo.damage > 0f)
			{
				EffectNumber.EffectNumType type = (hitResultInfo.isCirt ? EffectNumber.EffectNumType.Crit : EffectNumber.EffectNumType.Damge);
				EffectNumManager.instance.GenerageEffectNum(type, (int)hitResultInfo.damage, m_effectPoint.position);
				base.hp -= (int)hitResultInfo.damage;
				if (hitInfo.hitSpawnInfo != null)
				{
					HitToSpawn(hitInfo);
				}
			}
			if (!DataCenter.State().isPVPMode && hitInfo.source != null && (hitInfo.source.GetGameObject().layer == 9 || hitInfo.source.GetGameObject().layer == 10 || hitInfo.source.GetGameObject().layer == 11))
			{
				Character character = (Character)hitInfo.source;
				if (!hitInfo.bSkill)
				{
					character.effectPlayManager.PlayEffect("Hit");
				}
				character.lockedTarget = this;
				if (hitInfo.source.GetGameObject().layer == 9 || hitInfo.source.GetGameObject().layer == 10)
				{
					Player player = (Player)character;
					if ((player.CurrentController || DataCenter.Save().squadMode) && base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
					{
						Enemy enemy = (Enemy)this;
						if (enemy.eliteType != 0)
						{
							GameObject uITargetPanel = GameBattle.m_instance.m_UITargetPanel;
							if (uITargetPanel != null)
							{
								uITargetPanel.SetActive(true);
								BattleUIEvent component = uITargetPanel.GetComponent<BattleUIEvent>();
								component.SetTarget(this);
								GameBattle.m_instance.UIVisableTarget = this;
							}
						}
					}
				}
			}
			if (!Alive())
			{
				if (base.isBetray)
				{
					if (GetBuffManager().HasBuff(Buff.AffectType.Confuse))
					{
						GetBuffManager().RemoveBuff(Buff.AffectType.Confuse);
					}
					return hitResultInfo;
				}
				if (HasNavigation())
				{
					ResumeNav();
				}
				SetSplash(false);
				AIStateDeath aIStateDeath = GetAIState("Death") as AIStateDeath;
				if (aIStateDeath != null)
				{
					float num3 = 0f;
					num3 = ((hitInfo.deadRepelDistance == null) ? Random.Range(hitInfo.repelDistance.left, hitInfo.repelDistance.right) : Random.Range(hitInfo.deadRepelDistance.left, hitInfo.deadRepelDistance.right));
					if (num3 > 0f && NegativeAffectEnable)
					{
						Vector3 vector = hitInfo.repelDirection * -1f;
						if (vector.sqrMagnitude > 0.0001f)
						{
							GetModelTransform().forward = vector;
						}
						Vector3 faceDirectionImmediately = vector;
						SetFaceDirectionImmediately(faceDirectionImmediately);
						float time = ((!hitInfo.bViolent) ? hitInfo.repelTime : (hitInfo.repelTime * 3f));
						aIStateDeath.SetRepel(num3, time, hitInfo.repelDirection);
					}
					aIStateDeath.bViolent = hitInfo.bViolent;
				}
				if (hitInfo.deadSpawnInfo != null)
				{
					DeadToSpawn(hitInfo);
				}
				OnDeath();
				if (hitInfo.source != null && (hitInfo.source.GetGameObject().layer == 9 || hitInfo.source.GetGameObject().layer == 10))
				{
					Player player2 = (Player)hitInfo.source;
					player2.audioTalkManager.PlayKill();
				}
			}
			else
			{
				for (int i = 0; i < hitInfo.buffs.Count; i++)
				{
					GetBuffManager().AddBuff(hitInfo.buffs[i]);
				}
				if (hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.FROZEN))
				{
					if (Random.Range(0, 100) < hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN].specialHitProbability && NegativeAffectEnable)
					{
						AIStateFrozen aIStateFrozen = GetAIState("Frozen") as AIStateFrozen;
						aIStateFrozen.SetFrozen(hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN].time);
						SwitchFSM(aIStateFrozen);
						if (hitInfo.source != null)
						{
							hitInfo.source.HitResult(hitResultInfo);
						}
						if (hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN].disposable)
						{
							hitInfo.specialHit.Remove(Defined.SPECIAL_HIT_TYPE.FROZEN);
						}
						return hitResultInfo;
					}
				}
				else if (hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA) && Random.Range(0, 100) < hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA].specialHitProbability && NegativeAffectEnable)
				{
					AIStateFrozen aIStateFrozen2 = GetAIState("Frozen") as AIStateFrozen;
					aIStateFrozen2.SetFrozen(hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA].time);
					SwitchFSM(aIStateFrozen2);
					if (hitInfo.source != null)
					{
						hitInfo.source.HitResult(hitResultInfo);
					}
					if (hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA].disposable)
					{
						hitInfo.specialHit.Remove(Defined.SPECIAL_HIT_TYPE.FROZEN_NOVA);
					}
					return hitResultInfo;
				}
				SetSplash(true);
				if ((isRage || isEndure) && !hitInfo.bSkill)
				{
					if (hitInfo.source != null)
					{
						hitInfo.source.HitResult(hitResultInfo);
					}
					return hitResultInfo;
				}
				if (!MoveAble)
				{
					base.animUpperBody = GetAnimationName("Hurt");
					AnimationCrossFade(base.animUpperBody, false);
					if (hitInfo.source != null)
					{
						hitInfo.source.HitResult(hitResultInfo);
					}
					OnHurt(false);
					return hitResultInfo;
				}
				float num4 = Random.Range(hitInfo.repelDistance.left, hitInfo.repelDistance.right);
				if (num4 > 0f)
				{
					effectPlayManager.PlayEffect("Blood");
					AIStateRepel aIStateRepel = GetAIState("Repel") as AIStateRepel;
					if (aIStateRepel != null)
					{
						if (HasNavigation())
						{
							ResumeNav();
						}
						if (hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.STUN) && NegativeAffectEnable && Random.Range(0, 100) < hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.STUN].specialHitProbability && hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.STUN) && NegativeAffectEnable)
						{
							AIStateStun aIStateStun = GetAIState("Stun") as AIStateStun;
							aIStateStun.SetStun(hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.STUN].time);
							base.autoNextState = GetAIState("Stun");
						}
						Vector3 vector = hitInfo.repelDirection * -1f;
						if (vector.sqrMagnitude > 0.0001f)
						{
							GetModelTransform().forward = vector;
						}
						Vector3 faceDirectionImmediately2 = vector;
						SetFaceDirectionImmediately(faceDirectionImmediately2);
						aIStateRepel.SetRepel(num4, hitInfo.repelTime, hitInfo.repelDirection);
						SwitchFSM(aIStateRepel);
					}
				}
				else
				{
					if (hitInfo.specialHit.ContainsKey(Defined.SPECIAL_HIT_TYPE.STUN) && NegativeAffectEnable)
					{
						AIStateStun aIStateStun2 = GetAIState("Stun") as AIStateStun;
						aIStateStun2.SetStun(hitInfo.specialHit[Defined.SPECIAL_HIT_TYPE.STUN].time);
						base.autoNextState = GetAIState("Stun");
					}
					if (!DataCenter.State().isPVPMode || (DataCenter.State().isPVPMode && Random.Range(0, 100) < 20))
					{
						base.animUpperBody = GetAnimationName("Hurt");
						AnimationCrossFade(base.animUpperBody, false);
					}
					OnHurt(false);
				}
			}
			if (hitInfo.source != null)
			{
				hitInfo.source.HitResult(hitResultInfo);
			}
			return hitResultInfo;
		}

		private void DoSpecialHit()
		{
		}

		public virtual HitResultInfo GetHitDamage(HitInfo hitInfo)
		{
			HitResultInfo hitResultInfo = new HitResultInfo();
			hitResultInfo.isHit = true;
			DS2ActiveObject source = hitInfo.source;
			hitResultInfo.target = this;
			if (source != null && source.clique == base.clique)
			{
				hitResultInfo.damage = -1f;
			}
			else
			{
				int num = Random.Range(0, 100);
				if ((float)num < hitInfo.critRate)
				{
					hitResultInfo.isCirt = true;
					hitResultInfo.damage = (int)hitInfo.critDamage;
				}
				else
				{
					hitResultInfo.isCirt = false;
					hitResultInfo.damage = (int)Random.Range(hitInfo.damage.left, hitInfo.damage.right);
				}
				int num2 = (int)((float)base.hpMax * hitInfo.percentDamage);
				int num3 = (int)(hitResultInfo.damage + (float)num2);
				num3 = (int)((float)num3 + (float)num3 * (addDamage - reduceDamage));
				if (base.clique == Clique.Player)
				{
					num3 += (int)((float)num3 * GameBattle.m_instance.combatAffectPercentDamage);
				}
				else if (base.clique == Clique.Computer)
				{
					num3 -= (int)((float)num3 * GameBattle.m_instance.combatAffectPercentDamage);
				}
				if (num3 < 0)
				{
					num3 = 0;
				}
				else if (num3 == 0 && base.clique == Clique.Computer && (hitInfo.damage.right > 0f || hitInfo.critDamage > 0f))
				{
					num3 = Mathf.Max(1, (int)((float)base.hpMax * 0.005f));
				}
				hitResultInfo.damage = num3;
			}
			return hitResultInfo;
		}

		public override void OnHurt(bool switchHurtState)
		{
			if (!(m_godTime > 0f))
			{
				if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
				{
					base.OnHurt(true);
				}
				else if (!DataCenter.State().isPVPMode || (DataCenter.State().isPVPMode && Random.Range(0, 100) < 20))
				{
					base.animUpperBody = GetAnimationName("Hurt");
					AnimationCrossFade(base.animUpperBody, false);
				}
				effectPlayManager.PlayEffect("Blood");
			}
		}

		public override void OnDeath()
		{
			SendMessageToImplicate("Death");
			m_buffManager.RemoveAllBuffs();
			base.OnDeath();
			effectPlayManager.PlayEffect("DeadBlood");
		}

		public void DeadToSpawn(HitInfo hitInfo)
		{
			DeadSpawnInfo.DeadSpawnType spawnType = hitInfo.deadSpawnInfo.spawnType;
			if ((spawnType != 0 && spawnType != DeadSpawnInfo.DeadSpawnType.Ninjia_Ice) || hitInfo.source == null)
			{
				return;
			}
			Player player = (Player)hitInfo.source;
			DS2Object[] targetList = GameBattle.m_instance.GetTargetList(player.clique);
			List<DS2Object> list = new List<DS2Object>();
			for (int i = 0; i < 2 && i < targetList.Length; i++)
			{
				list.Add(targetList[i]);
			}
			Vector3 direction = Vector3.zero;
			for (int j = 0; j < 2; j++)
			{
				Bullet bullet = ((hitInfo.deadSpawnInfo.spawnType != 0) ? (BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.NinjiaKillSpawnBulletIce) as Bullet) : (BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.NinjiaKillSpawnBulletFire) as Bullet));
				if (bullet == null)
				{
					break;
				}
				if (list.Count == 0)
				{
					switch (j)
					{
					case 0:
						direction = GetModelTransform().forward;
						break;
					case 1:
						direction = GetModelTransform().forward * -1f;
						break;
					}
				}
				if (j < list.Count)
				{
					direction = list[j].GetTransform().position - GetTransform().position;
				}
				else
				{
					direction *= -1f;
				}
				if (player.clique == Clique.Computer)
				{
					bullet.GetGameObject().layer = 23;
				}
				else if (player.isAlly)
				{
					bullet.GetGameObject().layer = 22;
				}
				else
				{
					bullet.GetGameObject().layer = 21;
				}
				bullet.hitInfo = player.GetHitInfo();
				bullet.hitInfo.repelDistance = new NumberSection<float>(0f, 0f);
				bullet.hitInfo.hitStrength = 1;
				bullet.SetBullet(player, player.m_weapon.bulletAttribute, m_effectPoint.position, direction);
				bullet.Emit(10f);
			}
		}

		public void HitToSpawn(HitInfo hitInfo)
		{
			DeadSpawnInfo.DeadSpawnType spawnType = hitInfo.hitSpawnInfo.spawnType;
			if (spawnType != DeadSpawnInfo.DeadSpawnType.Zombie_Nurse_Venom)
			{
				return;
			}
			DS2HalfStaticObject effectFromBuffer = BattleBufferManager.Instance.GetEffectFromBuffer(Defined.EFFECT_TYPE.ZOMBIE_NURSE_VENOM_ATTACK);
			if (effectFromBuffer != null)
			{
				BuffTrigger component = effectFromBuffer.GetGameObject().GetComponent<BuffTrigger>();
				Character character = (Character)hitInfo.source;
				if (character.clique == Clique.Computer)
				{
					effectFromBuffer.GetGameObject().layer = 15;
				}
				else if (character.clique == Clique.Player)
				{
					effectFromBuffer.GetGameObject().layer = 14;
				}
				else
				{
					effectFromBuffer.GetGameObject().layer = 20;
				}
				effectFromBuffer.GetTransform().parent = BattleBufferManager.s_effectObjectRoot.transform;
				effectFromBuffer.GetTransform().position = hitInfo.hitPoint;
				effectFromBuffer.GetGameObject().SetActive(true);
			}
		}

		public override IPathFinding GetPathFinding()
		{
			return null;
		}

		public virtual bool HasNavigation()
		{
			if (GameBattle.m_instance == null)
			{
				return false;
			}
			return m_navMeshAgent != null && m_navMeshAgent.enabled;
		}

		public virtual UnityEngine.AI.NavMeshAgent GetNavMeshAgent()
		{
			return m_navMeshAgent;
		}

		public virtual void SetNavDesination(Vector3 target)
		{
			m_move = true;
			m_navMeshAgent.SetDestination(target);
			m_navMeshAgent.Resume();
		}

		public virtual void SetNavSpeed(float speed)
		{
			m_navMeshAgent.speed = speed;
		}

		public virtual void StopNav(bool stopUpdate = true)
		{
			m_move = false;
			m_navMeshAgent.Stop(false);
		}

		public virtual void ResumeNav()
		{
			m_navMeshAgent.enabled = true;
			m_navMeshAgent.Resume();
			m_navMeshAgent.Stop(false);
			m_move = false;
		}

		public override IBuffManager GetBuffManager()
		{
			if (m_buffManager == null)
			{
				m_buffManager = new BuffManager(this);
			}
			return m_buffManager;
		}

		public virtual void UseSkill(int skillId)
		{
			SkillStart();
		}

		public virtual void SkillStart()
		{
			skillAttack = true;
		}

		public virtual void SkillEnd()
		{
			skillAttack = false;
		}

		public virtual bool SkillInCDTime()
		{
			return m_skillCDTime > 0f;
		}

		public virtual bool CheckSkillConditions()
		{
			if (SkillInCDTime())
			{
				return false;
			}
			return true;
		}

		public float GetSkillCDTime()
		{
			return m_skillCDTime;
		}

		public float GetSkillTotalCDTime()
		{
			return m_skillTotalCDTime;
		}

		public void ClearSkillCDTime()
		{
			m_skillCDTime = 0f;
		}

		public void FillCDTime()
		{
			m_skillCDTime = m_skillTotalCDTime;
		}

		public virtual void OnSkillReady(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
				AnimationStop(base.animUpperBody);
				base.animLowerBody = GetAnimationName("Idle");
				AnimationCrossFade(base.animLowerBody, false);
				if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
				{
					Player player2 = (Player)this;
					if (!DataCenter.Save().squadMode && player2.CurrentController)
					{
						GameBattle.s_bInputLocked = true;
					}
				}
				isRage = true;
				SetGodTime(float.PositiveInfinity);
				BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.EFFECT_GLITTER, m_effectPoint.position, 0.7f, m_effectPoint);
				if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
				{
					Player player3 = (Player)this;
					if (player3.CurrentController)
					{
						GameBattle.m_instance.SetCameraZoomIn(0.35f, -4.5f, 0.75f);
					}
				}
				m_skillReadyTimer = 0f;
				break;
			case AIState.AIPhase.Update:
				m_skillReadyTimer += Time.deltaTime;
				if (m_skillReadyTimer >= 0.7f)
				{
					ChangeAIState("Skill");
				}
				break;
			case AIState.AIPhase.Exit:
				isRage = false;
				SetGodTime(0f);
				if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
				{
					Player player = (Player)this;
					if (!DataCenter.Save().squadMode && player.CurrentController)
					{
						GameBattle.s_bInputLocked = false;
					}
				}
				break;
			}
		}

		public void TargetDead()
		{
			if (m_bStuck)
			{
				m_bStuck = false;
			}
		}

		public void AddToGrabList(Character character)
		{
			m_grabList.Add(character);
		}

		public void RemoveFromGrabList(Character character)
		{
			if (m_grabList.Contains(character))
			{
				m_grabList.Remove(character);
			}
		}

		public void ClearGrabList()
		{
			m_grabList.Clear();
		}
	}
}

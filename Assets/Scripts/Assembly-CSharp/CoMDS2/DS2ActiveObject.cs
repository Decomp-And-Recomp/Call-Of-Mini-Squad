using System.Collections.Generic;
using UnityEngine;

namespace CoMDS2
{
	public abstract class DS2ActiveObject : DS2Object, ICollider, IFighter
	{
		public enum Clique
		{
			Player = 0,
			Neutral = 1,
			Computer = 2
		}

		private int m_hp;

		private Clique sliqueaa;

		private bool m_visible;

		public bool isBuilding;

		protected List<GameObject> m_implicate = new List<GameObject>();

		public EffectPlayManager effectPlayManager;

		public Transform m_effectPoint;

		public Transform m_effectPointFoward;

		public Transform m_effectPointGround;

		public Transform m_effectPointUpHead;

		private ColorAnimationScript[] m_splash;

		protected Dictionary<string, AIState> m_AIStateMap = new Dictionary<string, AIState>();

		private AIState m_state;

		private AIState m_lastState;

		public AIState m_defaultState;

		private int bb;

		public HitInfo hitInfo { get; set; }

		public int hpLast { get; set; }

		public int hp
		{
			get
			{
				return m_hp;
			}
			set
			{
				hpLast = hp;
				m_hp = value;
				m_hp = Mathf.Clamp(m_hp, 0, hpMax);
			}
		}

		public int hpMax { get; set; }

		public Clique clique
		{
			get
			{
				return sliqueaa;
			}
			set
			{
				sliqueaa = value;
			}
		}

		public bool isBetray { get; set; }

		public string animLowerBody { get; set; }

		public string animUpperBody { get; set; }

		public float hpPercent
		{
			get
			{
				return hp / hpMax;
			}
		}

		public virtual bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				m_visible = value;
			}
		}

		public AIState autoNextState { get; set; }

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			hitInfo = new HitInfo();
			hitInfo.source = this;
			m_splash = m_gameObject.GetComponentsInChildren<ColorAnimationScript>();
			effectPlayManager = new EffectPlayManager(this);
			Visible = true;
			isBetray = false;
		}

		public override void Initialize(GameObject gameObject)
		{
			base.Initialize(gameObject);
			hitInfo = new HitInfo();
			hitInfo.source = this;
			m_splash = m_gameObject.GetComponentsInChildren<ColorAnimationScript>();
			ExcuteAnimationEvent component = m_gameObject.GetComponent<ExcuteAnimationEvent>();
			if (component == null)
			{
				component = m_gameObject.AddComponent<ExcuteAnimationEvent>();
				component.belong = GetGameObject();
			}
			else
			{
				component.belong = GetGameObject();
			}
			Visible = true;
			isBetray = false;
		}

		public virtual void OnCollide(ICollider collider)
		{
		}

		public virtual void UpdateFire(float deltaTime)
		{
		}

		public override ICollider GetCollider()
		{
			return this;
		}

		public override IFighter GetFighter()
		{
			return this;
		}

		public virtual HitInfo GetHitInfo()
		{
			return hitInfo;
		}

		public virtual IPathFinding GetPathFinding()
		{
			return null;
		}

		public virtual IBuffManager GetBuffManager()
		{
			return null;
		}

		public override void Update(float deltaTime)
		{
			UpdateFSM(deltaTime);
			GetModelTransform().eulerAngles = new Vector3(0f, GetModelTransform().eulerAngles.y, GetModelTransform().eulerAngles.z);
		}

		public virtual bool Alive()
		{
			return hp > 0;
		}

		public virtual void Reset()
		{
			hp = hpMax;
			GetGameObject().layer = m_gameLayer;
		}

		public virtual void Betray(Clique toClique)
		{
			clique = toClique;
		}

		public override void Destroy(bool destroy = false)
		{
			if (destroy)
			{
				Object.Destroy(m_wrapGameObject);
				RemoveAllAIState();
				m_wrapGameObject = null;
				m_gameObject = null;
				m_transform = null;
				hitInfo = null;
			}
			else
			{
				m_wrapGameObject.SetActive(false);
			}
		}

		public virtual HitResultInfo OnHit(HitInfo hitInfo)
		{
			HitResultInfo hitResultInfo = new HitResultInfo();
			if (!Alive())
			{
				return hitResultInfo;
			}
			int num = (int)Random.Range(hitInfo.damage.left, hitInfo.damage.right);
			int num2 = (int)((float)hp * hitInfo.percentDamage);
			int num3 = num + num2;
			if (num3 < 0)
			{
				num3 = 0;
			}
			hp -= num3;
			hitResultInfo.damage = num3;
			if (hitInfo.source != null && hitInfo.source.GetGameObject().layer == 9)
			{
				Player player = (Player)hitInfo.source;
				if (player.CurrentController || DataCenter.Save().squadMode)
				{
					player.lockedTarget = this;
					if (base.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
					{
						Enemy enemy = (Enemy)this;
						if (enemy.isBoss | (enemy.eliteType != Enemy.EnemyEliteType.None))
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
			hitResultInfo.isHit = true;
			hitResultInfo.target = this;
			if (hitInfo.source != null)
			{
				hitInfo.source.HitResult(hitResultInfo);
			}
			return hitResultInfo;
		}

		public virtual void OnHurt(bool switchHurtState)
		{
			if (switchHurtState)
			{
				AIStateHurt aIStateHurt = GetAIState("Hurt") as AIStateHurt;
				if (aIStateHurt.HurtTime > 0f && aIStateHurt.ShouldHurt)
				{
					SwitchFSM(aIStateHurt);
				}
			}
		}

		public virtual void OnDeath()
		{
			hp = 0;
			SwitchFSM(GetAIState("Death"));
		}

		public virtual void OnDeathOver()
		{
		}

		public virtual void HitResult(HitResultInfo result)
		{
		}

		public void SetSplash(bool enable)
		{
			if (enable)
			{
				if (m_splash != null)
				{
					ColorAnimationScript[] splash = m_splash;
					foreach (ColorAnimationScript colorAnimationScript in splash)
					{
						colorAnimationScript.PlayColorAnimation();
					}
				}
			}
			else if (m_splash != null)
			{
				ColorAnimationScript[] splash2 = m_splash;
				foreach (ColorAnimationScript colorAnimationScript2 in splash2)
				{
					colorAnimationScript2.ResetColorAnimation();
				}
			}
		}

		public void ChangeColor(Color color)
		{
			ColorAnimationScript[] splash = m_splash;
			foreach (ColorAnimationScript colorAnimationScript in splash)
			{
				if (colorAnimationScript != null)
				{
					colorAnimationScript.ChangeColor(color);
				}
			}
		}

		public void ResetColor()
		{
			ColorAnimationScript[] splash = m_splash;
			foreach (ColorAnimationScript colorAnimationScript in splash)
			{
				if (colorAnimationScript != null)
				{
					colorAnimationScript.ResetColor();
				}
			}
		}

		public void AddImplicate(GameObject obj)
		{
			m_implicate.Add(obj);
		}

		public void RemoveImplicate(GameObject obj)
		{
			if (m_implicate.Contains(obj))
			{
				m_implicate.Remove(obj);
			}
		}

		public void RemoveAllImplicate()
		{
			m_implicate.Clear();
		}

		public void SendMessageToImplicate(string eventName)
		{
			if (m_implicate.Count <= 0 || !(eventName == "Death"))
			{
				return;
			}
			foreach (GameObject item in m_implicate)
			{
				if (item != null)
				{
					item.SendMessage("TargetDead", this);
				}
			}
			m_implicate.Clear();
		}

		protected void AddAIState(string name, AIState state)
		{
			if (m_AIStateMap.ContainsKey(name))
			{
				m_AIStateMap[name] = null;
				m_AIStateMap[name] = state;
			}
			else
			{
				m_AIStateMap.Add(name, state);
			}
		}

		protected void RemoveAIState(string name)
		{
			m_AIStateMap.Remove(name);
		}

		protected void RemoveAllAIState()
		{
			m_AIStateMap.Clear();
		}

		protected virtual void UpdateFSM(float deltaTime)
		{
			if (m_state == null)
			{
				return;
			}
			m_state.Update(deltaTime);
			AIState aIState = m_state.NextState();
			if (aIState != m_state && IsDeadEnemyLeavingDeath(aIState))
			{
				aIState = m_state;
			}
			if (aIState != m_state)
			{
				m_state.Exit();
				m_lastState = m_state;
				if (m_lastState == null)
				{
				}
				m_state = aIState;
				if (m_state != null)
				{
					m_state.Enter();
				}
			}
		}

		private bool IsDeadEnemyLeavingDeath(AIState next)
		{
			return objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY && !Alive() && m_state != null && m_state.name == "Death" && (next == null || next.name != "Death");
		}

		public virtual void SwitchFSM(AIState state)
		{
			if (IsDeadEnemyLeavingDeath(state))
			{
				return;
			}
			if (m_state != null)
			{
				m_state.Exit();
				m_state = null;
			}
			m_lastState = m_state;
			m_state = state;
			m_state.NextState(state);
			if (m_state != null)
			{
				m_state.Enter();
			}
		}

		public virtual bool ChangeAIState(string stateName, bool useDefaul = true)
		{
			if (m_AIStateMap.ContainsKey(stateName))
			{
				AIState aIState = m_AIStateMap[stateName];
				if (aIState != null && aIState.active)
				{
					m_state.NextState(aIState);
					return true;
				}
			}
			else if (useDefaul && m_defaultState != null)
			{
				m_state.NextState(m_defaultState);
				return true;
			}
			return false;
		}

		public virtual bool ChangeAIState(AIState state, bool useDefaul = true)
		{
			if (state != null && state.active)
			{
				m_state.NextState(state);
				return true;
			}
			if (useDefaul && m_defaultState != null)
			{
				m_state.NextState(m_defaultState);
				return true;
			}
			return false;
		}

		public virtual void SetDefaultAIState(AIState state)
		{
			m_defaultState = state;
		}

		public virtual void ChangeToDefaultAIState()
		{
			if (autoNextState != null)
			{
				m_state.NextState(autoNextState);
				autoNextState = null;
			}
			else if (m_defaultState != null)
			{
				m_state.NextState(m_defaultState);
			}
		}

		public virtual void ChangeToLastAIState()
		{
			if (autoNextState != null)
			{
				m_state.NextState(autoNextState);
				autoNextState = null;
			}
			else if (m_lastState != null)
			{
				m_state.NextState(m_lastState);
			}
			else if (m_defaultState != null)
			{
				m_state.NextState(m_defaultState);
			}
		}

		public virtual AIState GetAIState(string name)
		{
			if (m_AIStateMap.ContainsKey(name))
			{
				return m_AIStateMap[name];
			}
			return null;
		}

		public virtual AIState GetCurrentAIState()
		{
			return m_state;
		}

		public virtual AIState GetLastAIState()
		{
			return m_lastState;
		}

		public virtual AIState GetDefaultAIState()
		{
			return m_defaultState;
		}

		public virtual void ClearAIState()
		{
			m_AIStateMap.Clear();
		}

		public virtual void AnimationPlay(string name, bool loop, bool clamp = false)
		{
			if (name != null && !(name == string.Empty) && !(m_gameObject == null))
			{
				if (clamp)
				{
					m_gameObject.GetComponent<Animation>().Rewind(name);
					m_gameObject.GetComponent<Animation>()[name].wrapMode = WrapMode.ClampForever;
				}
				else
				{
					m_gameObject.GetComponent<Animation>()[name].wrapMode = ((!loop) ? WrapMode.Once : WrapMode.Loop);
				}
				m_gameObject.GetComponent<Animation>().Play(name);
			}
		}

		public virtual void AnimationStop(string name)
		{
			if (name != null && !(name == string.Empty) && !(m_gameObject == null))
			{
				m_gameObject.GetComponent<Animation>()[name].wrapMode = WrapMode.Once;
			}
		}

		public virtual bool AnimationPlaying(string name)
		{
			return m_gameObject.GetComponent<Animation>().IsPlaying(name);
		}

		public virtual bool HasAnimation(string animName)
		{
			return m_gameObject.GetComponent<Animation>().GetClip(animName) != null;
		}

		public virtual void AnimationStop()
		{
			m_gameObject.GetComponent<Animation>().Stop();
		}

		public virtual void AnimationCrossFade(string name, bool loop, bool clamp = false, float fadeLength = 0.2f)
		{
			if (name != null && !(name == string.Empty) && !(m_gameObject == null))
			{
				if (clamp)
				{
					m_gameObject.GetComponent<Animation>().Rewind(name);
					m_gameObject.GetComponent<Animation>()[name].wrapMode = WrapMode.ClampForever;
				}
				else
				{
					m_gameObject.GetComponent<Animation>().Rewind(name);
					m_gameObject.GetComponent<Animation>()[name].wrapMode = ((!loop) ? WrapMode.Once : WrapMode.Loop);
				}
				m_gameObject.GetComponent<Animation>().CrossFade(name, fadeLength);
			}
		}

		public virtual float AnimationLength(string name)
		{
			return m_gameObject.GetComponent<Animation>()[name].length;
		}

		public virtual void SetAnimationSpeed(string name, float speed)
		{
			if (name != null && !(name == string.Empty) && !(m_gameObject == null))
			{
				m_gameObject.GetComponent<Animation>()[name].speed = speed;
			}
		}

		public virtual AnimationState GetAnimationState(string name)
		{
			return m_gameObject.GetComponent<Animation>()[name];
		}

		public virtual Transform GetMoveTransform()
		{
			return GetTransform();
		}
	}
}

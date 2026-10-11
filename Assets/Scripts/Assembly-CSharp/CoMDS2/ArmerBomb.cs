using UnityEngine;

namespace CoMDS2
{
	public class ArmerBomb : Character
	{
		private AIStateChase stateChase;

		private float m_explodeWaitTime = 4f;

		private float m_timer;

		private Player m_creator;

		private EffectParticleContinuous m_effectGlitter;

		private float m_damageRadius = 2.5f;

		private ArmerBomb()
		{
		}

		public ArmerBomb(Player creator)
		{
			m_creator = creator;
			base.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_OTHERS;
		}

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			GetTransform().parent = BattleBufferManager.s_activeObjectRoot.transform;
			base.hitInfo = new HitInfo(m_creator.skillHitInfo);
			DataConf.SkillWesker skillWesker = m_creator.skillInfo as DataConf.SkillWesker;
			m_explodeWaitTime = skillWesker.limitTime;
			m_damageRadius = skillWesker.damageRadius;
			base.hitInfo = m_creator.skillHitInfo;
			base.hitInfo.repelDistance = skillWesker.repelDis;
			base.hitInfo.repelTime = 0.2f;
			AIState aIState = new AIState(this, "Idle");
			aIState.SetCustomFunc(OnIdle);
			stateChase = new AIStateChase(this, "Chase");
			stateChase.SetChase(1000f, skillWesker.moveSpeed);
			stateChase.animName = "Walk";
			stateChase.SetCustomFunc(OnChase);
			AIState aIState2 = new AIState(this, "Death");
			aIState2.SetCustomFunc(OnBomb);
			AddAIState(aIState.name, aIState);
			AddAIState(stateChase.name, stateChase);
			AddAIState(aIState2.name, aIState2);
			SetDefaultAIState(aIState);
			SwitchFSM(aIState);
			m_effectGlitter = GetGameObject().GetComponentInChildren<EffectParticleContinuous>();
		}

		protected override void LoadResources()
		{
			base.LoadResources();
			BattleBufferManager.Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.EFFECT_BOMB_1, 6);
		}

		public void SetActive(bool active)
		{
			if (active)
			{
				GetGameObject().SetActive(active);
				base.clique = m_creator.clique;
				m_effectGlitter.StartEmit();
				if (HasNavigation())
				{
					ResumeNav();
				}
				SwitchFSM(GetAIState("Idle"));
			}
			else
			{
				GetGameObject().SetActive(active);
			}
		}

		public override HitResultInfo OnHit(HitInfo hitInfo)
		{
			HitResultInfo hitResultInfo = new HitResultInfo();
			hitResultInfo.isHit = true;
			return hitResultInfo;
		}

		private static void ShakeCamera()
		{
			if (GameBattle.m_instance != null)
			{
				GameBattle.m_instance.SetCameraQuake(Defined.CameraQuakeType.Quake_D);
			}
		}

		public void OnIdle(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
				m_timer = 0f;
				base.animLowerBody = "Standby";
				AnimationCrossFade(base.animLowerBody, true);
				break;
			case AIState.AIPhase.Update:
			{
				m_timer += Time.deltaTime;
				if (!(m_timer >= 0.3f))
				{
					break;
				}
				DS2ActiveObject dS2ActiveObject = null;
				if (DataCenter.State().isPVPMode)
				{
					dS2ActiveObject = base.lockedTarget;
					if (dS2ActiveObject == null)
					{
						dS2ActiveObject = GameBattle.m_instance.GetNearestObjFromTargetList(this);
					}
				}
				else if (GameBattle.m_instance != null)
				{
					dS2ActiveObject = GameBattle.m_instance.GetNearestObjFromTargetList(this);
				}
				if (dS2ActiveObject != null)
				{
					ChangeAIState("Chase");
				}
				else if (m_timer >= m_explodeWaitTime + 3f)
				{
					ShakeCamera();
					ChangeAIState("Death");
				}
				break;
			}
			}
		}

		public void OnChase(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
				m_timer = 0f;
				break;
			case AIState.AIPhase.Update:
			{
				m_timer += Time.deltaTime;
				if (m_timer >= m_explodeWaitTime)
				{
					ChangeAIState("Death");
					break;
				}
				DS2ActiveObject target = stateChase.target;
				if (target == null)
				{
					break;
				}
				if (target is Character)
				{
					if (Vector3.Distance(target.GetTransform().position, GetTransform().position) <= radius + ((Character)target).radius + 0.3f)
					{
						ShakeCamera();
						ChangeAIState("Death");
					}
				}
				else if (Vector3.Distance(target.GetTransform().position, GetTransform().position) <= 4f)
				{
					ShakeCamera();
					ChangeAIState("Death");
				}
				break;
			}
			}
		}

		public void OnBomb(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
				m_timer = 0f;
				break;
			case AIState.AIPhase.Update:
				m_timer += Time.deltaTime;
				if (m_timer >= 0.1f)
				{
					BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.EFFECT_BOMB_1, new Vector3(GetTransform().position.x, (GameBattle.m_instance != null) ? 1f : (GetTransform().position.y + 1f), GetTransform().position.z), 2f);
					int layerMask = ((m_creator.clique != 0) ? 1536 : 2048);
					Collider[] array = Physics.OverlapSphere(GetTransform().position, m_damageRadius, layerMask);
					Collider[] array2 = array;
					foreach (Collider collider in array2)
					{
						DS2ActiveObject @object = DS2ObjectStub.GetObject<DS2ActiveObject>(collider.gameObject);
						base.hitInfo.repelDirection = @object.GetTransform().position - GetTransform().position;
						base.hitInfo.source = m_creator;
						@object.OnHit(base.hitInfo);
					}
					Destroy();
				}
				break;
			}
		}

		public override void Destroy(bool destroy = false)
		{
			base.Destroy(destroy);
			if (GameBattle.m_instance != null)
			{
				GameBattle.m_instance.AddToInteractObjectNeedDeleteList(this);
			}
			else
			{
				BattleBufferManager.Instance.AddToInteractObjectNeedDeleteListForUIExhibition(this);
			}
		}

		public void StateToChase()
		{
		}

		public override IPathFinding GetPathFinding()
		{
			return this;
		}
	}
}

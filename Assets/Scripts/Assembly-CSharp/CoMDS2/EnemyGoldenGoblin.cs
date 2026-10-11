using UnityEngine;

namespace CoMDS2
{
	public class EnemyGoldenGoblin : Enemy
	{
		private enum Phase
		{
			Move = 0,
			Stay = 1
		}

		private float appearTime;

		private string disappearReadyAnim = string.Empty;

		private string disappearProcessAnim = string.Empty;

		private float stayTime = 3f;

		private float runTime = 3f;

		private float aliveTime = 30f;

		private float disappearTime = 25f;

		private float bAlreadyDisappearTime = -1f;

		private Player m_aroundTarget;

		private Vector3 m_targetPosition;

		private float m_time;

		private NumberSection<float> m_range;

		private float m_stayTime;

		private Phase m_phase = Phase.Stay;

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			ClearAIState();
			AIStateDeath aIStateDeath = new AIStateDeath(this, "Death");
			AIStateHurt aIStateHurt = new AIStateHurt(this, "Hurt");
			AIState aIStateBasicLogic = new AIState(this, "PersonBasicLogicState");
			aIStateBasicLogic.SetCustomFunc(OnPersonBasicLogicState);
			AIState aIStateDisappear = new AIState(this, "DisappearState");
			aIStateDisappear.SetCustomFunc(OnDisappearState);
			SetAttackCollider(false);
			AddAIState(aIStateHurt.name, aIStateHurt);
			AddAIState(aIStateDeath.name, aIStateDeath);
			AddAIState(aIStateBasicLogic.name, aIStateBasicLogic);
			AddAIState(aIStateDisappear.name, aIStateDisappear);
			SetDefaultAIState(aIStateBasicLogic);
			if (base.skillInfos != null && base.skillInfos.Count > 1)
			{
				disappearReadyAnim = base.skillInfos[0].animReady;
				disappearProcessAnim = base.skillInfos[0].animProcess;
				stayTime = base.skillInfos[1].attackRange;
				runTime = base.skillInfos[1].speed;
				aliveTime = base.skillInfos[1].time;
				disappearTime = base.skillInfos[1].damage;
			}
			m_range = new NumberSection<float>(-10f, 10f);
		}

		public override void Reset()
		{
			base.Reset();
			appearTime = Time.realtimeSinceStartup;
			bAlreadyDisappearTime = -1f;
		}

		public override void SetSpecialAttibute()
		{
			base.SetSpecialAttibute();
			foreach (Material material in m_materials)
			{
				material.shader = m_shaderModelEdge;
				material.SetColor("_Color", Color.white);
				material.SetColor("_AtmoColor", new Color(1f, 0.82f, 0.1f, 1f));
			}
			ColorAnimationScript[] splash = GetTransform().GetComponentsInChildren<ColorAnimationScript>();
			foreach (ColorAnimationScript colorAnimationScript in splash)
			{
				colorAnimationScript.SetColorAnimation();
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (!Alive())
			{
				return;
			}
			AIState currentAIState = GetCurrentAIState();
			if (disappearTime < Time.realtimeSinceStartup - appearTime && currentAIState != null && currentAIState.name != "DisappearState")
			{
				AIState aIState = GetAIState("DisappearState");
				if (aIState != null)
				{
					SwitchFSM(aIState);
				}
			}
		}

		private Vector3 CalTargetPos(Player p)
		{
			Vector3 vector = new Vector3(Random.Range(m_range.left, m_range.right), 0f, Random.Range(m_range.left, m_range.right));
			vector += -p.GetTransform().forward * Random.Range(2, 5);
			return p.GetTransform().TransformPoint(vector);
		}

		private void stateToMove()
		{
			Player player = GameBattle.m_instance.GetPlayer();
			if (player == null)
			{
				return;
			}
			m_aroundTarget = player;
			m_targetPosition = CalTargetPos(m_aroundTarget);
			m_phase = Phase.Move;
			m_time = 0f;
			GetTransform().LookAt(m_targetPosition);
			AnimationCrossFade("Forward", true);
			SetNavSpeed(MoveSpeed);
			SetNavDesination(m_targetPosition);
		}

		private void updatePhaseMove()
		{
			IPathFinding pathFinding = GetPathFinding();
			if (pathFinding == null || !pathFinding.HasNavigation())
			{
				m_time += Time.deltaTime;
				if (m_time >= runTime)
				{
					m_time = 0f;
					stateToStay();
				}
				return;
			}
			pathFinding.SetNavDesination(m_targetPosition);
			GetTransform().LookAt(m_targetPosition);
			m_time += Time.deltaTime;
			UnityEngine.AI.NavMeshAgent navMeshAgent = pathFinding.GetNavMeshAgent();
			if (navMeshAgent == null)
			{
				return;
			}
			UnityEngine.AI.NavMeshHit hit;
			navMeshAgent.SamplePathPosition(-1, 1f, out hit);
			if (hit.distance != 0f)
			{
				Vector3 position = GetTransform().position;
				bool arrived = position.x == m_targetPosition.x && position.z == m_targetPosition.z;
				if (!arrived && m_time < runTime)
				{
					return;
				}
			}
			m_time = 0f;
			stateToStay();
		}

		private void stateToStay()
		{
			m_stayTime = 0f;
			m_phase = Phase.Stay;
			AnimationCrossFade("Idle", true);
			StopNav();
		}

		private void updatePhaseStay()
		{
			m_stayTime += Time.deltaTime;
			if (m_stayTime >= stayTime)
			{
				m_stayTime = 0f;
				stateToMove();
			}
		}

		public void OnPersonBasicLogicState(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
				stateToMove();
				break;
			case AIState.AIPhase.Update:
				if (m_phase == Phase.Stay)
				{
					updatePhaseStay();
				}
				else if (m_phase == Phase.Move)
				{
					updatePhaseMove();
				}
				break;
			}
		}

		public void OnDisappearState(AIState.AIPhase phase)
		{
			switch (phase)
			{
			case AIState.AIPhase.Enter:
				AnimationPlay(disappearReadyAnim, false);
				break;
			case AIState.AIPhase.Update:
				if (AnimationPlaying(disappearProcessAnim) && aliveTime <= Time.realtimeSinceStartup - appearTime && bAlreadyDisappearTime < 0f)
				{
					bAlreadyDisappearTime = Time.realtimeSinceStartup;
					effectPlayManager.PlayEffect("Clown_disappear", true);
					AnimationStop(disappearProcessAnim);
					Transform transform = GetTransform().Find("Model");
					if (transform != null)
					{
						Vector3 localPosition = transform.localPosition;
						transform.localPosition = new Vector3(localPosition.x, localPosition.y + 10000f, localPosition.z);
					}
					if (m_hpBar != null)
					{
						m_hpBar.gameObject.SetActive(false);
					}
				}
				if (bAlreadyDisappearTime > 0f && Time.realtimeSinceStartup - bAlreadyDisappearTime > 1f)
				{
					if (m_hpBar != null)
					{
						m_hpBar.gameObject.SetActive(false);
					}
					Destroy(false);
				}
				break;
			}
		}

		public void DisaapearAnimationPartIFinished()
		{
			AnimationPlay(disappearProcessAnim, true);
		}
	}
}

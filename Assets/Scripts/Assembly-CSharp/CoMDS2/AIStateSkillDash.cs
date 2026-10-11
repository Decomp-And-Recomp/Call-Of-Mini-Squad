using UnityEngine;

namespace CoMDS2
{
	public class AIStateSkillDash : AIState
	{
		private enum DashPhase
		{
			Dash = 0,
			Fatigue = 1
		}

		private DashPhase m_phase;

		private float m_dashDis;

		private float m_dashTime;

		private Vector3 m_direction;

		private float m_timer;

		private float m_dashSpeed;

		private float m_fatigueTime;

		private float m_timerFatigue;

		private float m_cameraZoomOffes = 3.5f;

		private int m_cameraZoomWaitFrame = 5;

		private int m_cameraZoomWaitFrameCounter;

		private Character m_character;

		private CharacterController m_characterController;

		private string m_animChargeLower;

		private bool m_lockedInput;

		private bool m_slowMotion;

		private UnityEngine.AI.NavMeshAgent m_dashAgent;

		private UnityEngine.AI.NavMeshAgent m_pausedAgent;

		public AIStateSkillDash(Character obj, string name, Controller controller = Controller.System)
			: base(obj, name, controller)
		{
			m_character = obj;
			m_characterController = m_character.GetGameObject().GetComponent<CharacterController>();
			m_dashDis = 12f;
			m_dashTime = 0.5f;
			m_dashSpeed = m_dashDis / m_dashTime;
			m_fatigueTime = 0.5f;
		}

		public void SetSkillInfo()
		{
		}

		public void SetDash(float distance, float time = 0.5f)
		{
			m_dashDis = distance;
			m_dashTime = time;
		}

		protected override void OnEnter()
		{
			base.OnEnter();
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER && ((Player)m_character).CurrentController)
			{
				GameBattle.s_bInputLocked = true;
				m_lockedInput = true;
			}
			m_phase = DashPhase.Dash;
			m_timer = 0f;
			UnityEngine.AI.NavMeshAgent agent = m_character.GetNavMeshAgent();
			m_dashAgent = (agent != null && agent.enabled && agent.isOnNavMesh) ? agent : null;
			m_pausedAgent = null;
			Player dashPlayer = m_character as Player;
			if (agent != null && agent.enabled && dashPlayer != null && !dashPlayer.CurrentController)
			{
				agent.enabled = false;
				m_pausedAgent = agent;
				m_dashAgent = null;
			}
			m_character.AnimationStop(m_activeObject.animUpperBody);
			m_character.AnimationStop(m_activeObject.animLowerBody);
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY || m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				m_character.MoveDirection = GetDashDirection();
				m_character.UpdateLowerBodyMoveDir4();
				base.animName = m_character.animLowerBody;
				DS2ActiveObject activeObject = m_activeObject;
				string animUpperBody = (base.animName2 = m_character.GetAnimationNameByWeapon("Skill"));
				activeObject.animUpperBody = animUpperBody;
			}
			m_character.AnimationPlay(base.animName, true);
			m_character.SetAnimationSpeed(base.animName, 2f);
			m_character.AnimationPlay(base.animName2, false, true);
			m_direction = m_character.MoveDirection;
			m_character.SetAttackCollider(true, AttackCollider.AttackColliderType.Dash);
			m_character.SetGodTime(float.PositiveInfinity);
			if (m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				Player player = (Player)m_activeObject;
				if (player.CurrentController && GameBattle.m_instance != null)
				{
					GameBattle.m_instance.SetCameraZoomIn(m_dashTime, 0f - m_cameraZoomOffes);
				}
			}
			m_timerFatigue = 0f;
			m_cameraZoomWaitFrameCounter = 0;
			m_animChargeLower = m_character.GetAnimationNameByWeapon("Charge_Lower");
		}

		private Vector3 GetDashDirection()
		{
			Vector3 direction = m_character.GetModelTransform().forward;
			Player player = m_character as Player;
			if (player != null && !player.CurrentController)
			{
				direction = m_character.GetTransform().forward;
				if (GameBattle.m_instance != null)
				{
					DS2ActiveObject target = GameBattle.m_instance.GetNearestObjFromTargetList(m_character.GetTransform().position, m_character.clique);
					if (target != null && target.Alive())
					{
						Vector3 toTarget = target.GetTransform().position - m_character.GetTransform().position;
						toTarget.y = 0f;
						if (toTarget.sqrMagnitude > 0.0001f)
						{
							direction = toTarget;
						}
					}
				}
			}
			direction.y = 0f;
			if (direction.sqrMagnitude < 0.0001f)
			{
				direction = m_character.GetTransform().forward;
				direction.y = 0f;
			}
			direction.Normalize();
			if (player != null && !player.CurrentController)
			{
				m_character.GetModelTransform().forward = direction;
			}
			return direction;
		}

		protected override void OnExit()
		{
			base.OnExit();
			if (m_lockedInput)
			{
				GameBattle.s_bInputLocked = false;
				m_lockedInput = false;
			}
			if (m_slowMotion)
			{
				Time.timeScale = 1f;
				m_slowMotion = false;
				if (GameBattle.m_instance != null)
				{
					GameBattle.m_instance.SetCameraZoomOut();
				}
			}
			m_dashAgent = null;
			if (m_pausedAgent != null)
			{
				UnityEngine.AI.NavMeshHit hit;
				Vector3 endPosition = m_character.GetTransform().position;
				if (UnityEngine.AI.NavMesh.SamplePosition(endPosition, out hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
				{
					endPosition = hit.position;
				}
				m_pausedAgent.enabled = true;
				m_pausedAgent.Warp(endPosition);
				m_pausedAgent.ResetPath();
				m_pausedAgent = null;
			}
			m_character.SetAttackCollider(false, AttackCollider.AttackColliderType.Dash);
			m_character.SetGodTime(0f);
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER ||m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player = (Player)m_character;
				if (player.CurrentController)
				{
					player.m_objAutoShootAreaTarget = null;
					player.m_bPlayerLockAttackDirection = false;
					if (player.m_fire)
					{
						player.SetFire(false, player.m_fireDirection);
					}
					player.FaceToMoveDirection = true;
				}
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			switch (m_phase)
			{
			case DashPhase.Dash:
				updatePhaseDash(deltaTime);
				break;
			case DashPhase.Fatigue:
				updatePhaseFatigue(deltaTime);
				break;
			}
		}

		protected void stateToFatigue()
		{
			m_phase = DashPhase.Fatigue;
			m_character.MoveSpeed = m_character.MoveSpeed;
			m_character.AnimationStop(base.animName);
			m_character.AnimationStop(base.animName2);
			Character character = m_character;
			string animLowerBody = (base.animName = m_animChargeLower);
			character.animLowerBody = animLowerBody;
			m_character.AnimationPlay(base.animName, false, true);
			Character character2 = m_character;
			animLowerBody = (base.animName2 = string.Empty);
			character2.animUpperBody = animLowerBody;
			m_character.SetAttackCollider(false, AttackCollider.AttackColliderType.Dash);
		}

		protected void updatePhaseFatigue(float deltaTime)
		{
			m_timerFatigue += deltaTime;
			if (m_timerFatigue > m_fatigueTime)
			{
				m_timer = 0f;
				m_character.ChangeToDefaultAIState();
			}
			else if (m_timerFatigue > m_fatigueTime / 12f)
			{
				if (GameBattle.m_instance != null)
				{
					GameBattle.m_instance.SetCameraZoomOut();
				}
				Time.timeScale = 1f;
				m_slowMotion = false;
			}
			else if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				Player player = (Player)m_character;
				if (player.CurrentController)
				{
					Time.timeScale = 0.1f;
					m_slowMotion = true;
				}
			}
		}

		protected void updatePhaseDash(float deltaTime)
		{
			m_timer += deltaTime;
			if (m_timer >= m_dashTime)
			{
				stateToFatigue();
			}
			else if (GameBattle.m_instance != null)
			{
				if (m_dashAgent != null && m_dashAgent.enabled && m_dashAgent.isOnNavMesh)
				{
					m_dashAgent.Move(m_direction.normalized * m_dashSpeed * deltaTime);
				}
				else if (m_pausedAgent != null)
				{
					Vector3 position = m_character.GetTransform().position;
					Vector3 step = m_direction.normalized * m_dashSpeed * deltaTime;
					UnityEngine.AI.NavMeshHit edge;
					if (UnityEngine.AI.NavMesh.Raycast(position, position + step, out edge, UnityEngine.AI.NavMesh.AllAreas))
					{
						step = edge.position - position;
						step.y = 0f;
						m_timer = m_dashTime;
					}
					if ((bool)m_characterController)
					{
						m_characterController.Move(step);
					}
					else
					{
						m_character.GetTransform().Translate(step, Space.World);
					}
				}
				else if ((bool)m_characterController)
				{
					m_characterController.Move(m_direction.normalized * m_dashSpeed * deltaTime);
				}
				else
				{
					m_character.GetTransform().Translate(m_direction.normalized * m_dashSpeed * deltaTime, Space.World);
				}
			}
		}
	}
}

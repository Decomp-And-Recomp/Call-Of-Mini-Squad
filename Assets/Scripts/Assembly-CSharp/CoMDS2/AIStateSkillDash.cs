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
			}
			m_phase = DashPhase.Dash;
			m_character.AnimationStop(m_activeObject.animUpperBody);
			m_character.AnimationStop(m_activeObject.animLowerBody);
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY || m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				m_character.MoveDirection = m_character.GetModelTransform().forward;
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

		protected override void OnExit()
		{
			base.OnExit();
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER && ((Player)m_character).CurrentController && GameBattle.m_instance != null)
			{
				GameBattle.s_bInputLocked = false;
			}
			m_character.SetAttackCollider(false, AttackCollider.AttackColliderType.Dash);
			m_character.SetGodTime(0f);
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
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
			}
			else if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				Player player = (Player)m_character;
				if (player.CurrentController)
				{
					Time.timeScale = 0.1f;
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
				if ((bool)m_characterController)
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

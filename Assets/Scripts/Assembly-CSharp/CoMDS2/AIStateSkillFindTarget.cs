using UnityEngine;

namespace CoMDS2
{
	public class AIStateSkillFindTarget : AIState
	{
		private float m_timer;

		private Character m_character;

		private CharacterController m_characterController;

		private float m_stoppingDistance = 1f;

		private DS2ActiveObject m_target;

		private string m_animChargeLower;

		public AIStateSkillFindTarget(Character obj, string name, Controller controller = Controller.System)
			: base(obj, name, controller)
		{
			m_character = obj;
			m_characterController = m_character.GetGameObject().GetComponent<CharacterController>();
		}

		public void SetSkillFindTarget(DS2ActiveObject target)
		{
			m_target = target;
		}

		public void SetStoppingDistance(float dis)
		{
			m_stoppingDistance = dis;
		}

		private const float MaxSeekTime = 1.5f;

		protected override void OnEnter()
		{
			base.OnEnter();
			m_timer = 0f;
		}

		protected override void OnExit()
		{
			base.OnExit();
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (m_character.objectType != 0 && m_character.objectType != Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				return;
			}
			Player player = GameBattle.m_instance.GetPlayer();
			Player player2 = (Player)m_character;
			m_target = GameBattle.m_instance.GetNearestObjFromTargetList(player.GetTransform().position, player.clique);
			if (m_target == null)
			{
				m_target = GameBattle.m_instance.GetNearestObjFromTargetList(m_character.GetTransform().position, player2.clique);
				if (m_target == null)
				{
					m_character.UseSkill(0);
					return;
				}
			}
			IPathFinding pathFinding = m_character.GetPathFinding();
			if (pathFinding != null && pathFinding.HasNavigation())
			{
				pathFinding.SetNavDesination(m_target.GetTransform().position);
			}
			m_timer += deltaTime;
			float sqrMagnitude = (m_target.GetTransform().position - m_activeObject.GetTransform().position).sqrMagnitude;
			if (sqrMagnitude < m_stoppingDistance * m_stoppingDistance || m_timer >= MaxSeekTime)
			{
				Vector3 toTarget = m_target.GetTransform().position - player2.GetTransform().position;
				toTarget.y = 0f;
				if (toTarget.sqrMagnitude > 0.0001f)
				{
					player2.GetTransform().forward = toTarget;
				}
				player2.UseSkill(0);
			}
		}
	}
}

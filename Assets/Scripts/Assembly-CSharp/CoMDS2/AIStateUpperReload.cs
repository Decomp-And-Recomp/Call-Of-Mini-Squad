namespace CoMDS2
{
	public class AIStateUpperReload : AIState
	{
		private Character m_character;

		public AIStateUpperReload(Character character, string name, Controller controller = Controller.System)
			: base(character, name, controller)
		{
			m_character = character;
		}

		protected override void OnEnter()
		{
			if (m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player = (Player)m_activeObject;
				if (player.m_weapon != null)
				{
					m_character.animUpperBody = m_character.GetAnimationNameByWeapon("Reload");
					base.animName2 = m_character.animUpperBody;
				}
			}
			m_activeObject.AnimationCrossFade(base.animName2, false);
			m_character.isInReload = true;
		}

		protected override void OnExit()
		{
			m_character.isInReload = false;
			Player player = m_character as Player;
			if (player != null && m_character.m_weapon != null && !m_character.m_weapon.NeedReload())
			{
				player.ResumeHeldFireAfterReload();
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}

using UnityEngine;

namespace CoMDS2
{
	public class AIStateReload : AIState
	{
		private Character m_character;

		public GameObject target { get; set; }

		public AIStateReload(Character character, string name, Controller controller = Controller.System)
			: base(character, name, controller)
		{
			m_character = character;
		}

		protected override void OnEnter()
		{
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				Player player = (Player)m_character;
				if (player.CurrentController)
				{
					Push(m_character.GetAIState("LowerMove"));
				}
				else if (m_character.m_move)
				{
					Push(m_character.GetAIState("AllyFollow"));
					base.animName = m_character.animLowerBody;
				}
				m_character.animUpperBody = m_character.GetAnimationNameByWeapon("Reload");
				base.animName2 = m_character.animUpperBody;
			}
			else if (m_character.objectType != Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
			{
			}
			m_activeObject.AnimationCrossFade(base.animName2, false);
			m_activeObject.SetAnimationSpeed(base.animName2, base.animSpeed / m_character.m_weapon.attribute.reloadTime);
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
			if (m_character.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				Player player = (Player)m_character;
				if (!player.CurrentController && !player.m_move)
				{
					if (!DataCenter.State().isPVPMode)
					{
						Player player2 = GameBattle.m_instance.GetPlayer();
						float sqrMagnitude = (player2.GetTransform().position - m_character.GetTransform().position).sqrMagnitude;
						float num = Player.ALLY_TOFOLLOW_DIS;
						if (DataCenter.State().isPVPMode)
						{
							num = m_character.shootRange;
						}
						else if (Util.s_allyMoveAttack && GameBattle.m_instance.IsInBattle)
						{
							num = 10f;
						}
						if (sqrMagnitude > num * num)
						{
							Push(m_character.GetAIState("AllyFollow"));
						}
					}
					else if (Random.Range(0, 100) < 50)
					{
						Push(m_character.GetAIState("Avoid"));
					}
				}
			}
			if (m_character.m_weapon.ReloadFinished())
			{
				m_character.m_weapon.DoReload();
				if (!m_activeObject.ChangeAIState("FireReady"))
				{
					m_activeObject.ChangeToLastAIState();
				}
			}
		}
	}
}

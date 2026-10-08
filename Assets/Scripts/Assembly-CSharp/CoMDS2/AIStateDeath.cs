using UnityEngine;

namespace CoMDS2
{
	public class AIStateDeath : AIState
	{
		private float m_repelDis;

		private float m_repelTime;

		private float m_repelSpeed;

		private Vector3 m_direction;

		private float m_timer;

		private bool m_bRepel;

		private CharacterController m_characterController;

		private bool m_bCheckWin;

		private bool m_bToResurgence;

		private bool m_bToDestroy;

		public bool bViolent;

		public AIStateDeath(DS2ActiveObject obj, string name, Controller controller = Controller.System)
			: base(obj, name, controller)
		{
			m_characterController = obj.GetGameObject().GetComponent<CharacterController>();
		}

		public void SetRepel(float dis, float time, Vector3 dir)
		{
			m_repelDis = dis;
			m_repelTime = time;
			m_repelSpeed = dis / time;
			m_direction = dir;
			m_timer = 0f;
			m_bRepel = true;
		}

		protected override void OnEnter()
		{
			m_bCheckWin = false;
			m_bToResurgence = false;
			m_bToDestroy = false;
			m_characterController = m_activeObject.GetGameObject().GetComponent<CharacterController>();
			m_activeObject.AnimationStop(m_activeObject.animUpperBody);
			if (m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				if (m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER && m_activeObject.clique == DS2ActiveObject.Clique.Player)
				{
					Player player = (Player)m_activeObject;
					if (player.CurrentController && !player.willResurgence)
					{
						bool flag = false;
						Player[] teammateList = GameBattle.m_instance.GetTeammateList();
						for (int i = 0; i < teammateList.Length; i++)
						{
							if (teammateList[i].Alive())
							{
								GameBattle.m_instance.SetTeammateToCurrentControlPlayer(i);
								flag = true;
								m_activeObject.ChangeAIState("SeriousInjury");
								break;
							}
						}
						if (!flag)
						{
							GameBattle.s_bInputLocked = true;
							GameBattle.m_instance.SetCameraZoomIn(0.5f, -5f, 0.5f);
							((Character)m_activeObject).FaceDirection = Vector3.forward;
						}
					}
					else if (!player.willResurgence)
					{
						DS2ActiveObject[] playerList = GameBattle.m_instance.GetPlayerList();
						for (int j = 0; j < playerList.Length; j++)
						{
							Player player2 = (Player)playerList[j];
							if (player2 != player && player2.SquadSite > player.SquadSite)
							{
								player2.SquadSite--;
								if (player2.SquadSite == 0)
								{
									GameBattle.m_instance.ChangeCurrentContorlPlayer(player2);
								}
							}
						}
						player.SquadSite = playerList.Length - 1;
					}
				}
				DS2ActiveObject activeObject = m_activeObject;
				string animLowerBody = (base.animName = ((Character)m_activeObject).GetAnimationNameByWeapon("Death"));
				activeObject.animLowerBody = animLowerBody;
			}
			else if (m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
			{
				if (bViolent)
				{
					base.animName = ((Enemy)m_activeObject).GetAnimationName("DeathViolent");
				}
				if (string.IsNullOrEmpty(base.animName) || !bViolent)
				{
					base.animName = ((Enemy)m_activeObject).GetAnimationName("Death");
				}
				m_timer = 0f;
				m_bToDestroy = true;
			}
			m_activeObject.AnimationCrossFade(base.animName, false);
		}

		protected override void OnExit()
		{
			if (m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER)
			{
				GameBattle.s_bInputLocked = false;
				GameBattle.m_instance.SetCameraZoomOut();
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (m_bRepel)
			{
				m_timer += deltaTime;
				if (m_timer >= m_repelTime)
				{
					m_timer = 0f;
					m_bRepel = false;
				}
				else if ((bool)m_characterController)
				{
					m_characterController.Move(m_direction.normalized * m_repelSpeed * deltaTime);
				}
				else
				{
					m_activeObject.GetTransform().Translate(m_direction.normalized * m_repelSpeed * deltaTime, Space.World);
				}
			}
			if ((m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || m_activeObject.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY) && !m_bCheckWin && !m_activeObject.AnimationPlaying(base.animName))
			{
				m_bRepel = false;
				Player player = (Player)m_activeObject;
				if (player.willResurgence)
				{
					m_timer = 0f;
					m_bToResurgence = true;
					player.audioTalkManager.PlayLife();
					BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.heores_Prayer, player.m_effectPointUpHead.position, 0f, player.m_effectPointUpHead);
					m_bCheckWin = true;
					return;
				}
				if (DataCenter.Save().squadMode)
				{
					GameBattle.m_instance.GetSquadController().UpdatePointsPosition();
				}
				if (DataCenter.State().isPVPMode && m_activeObject.clique == DS2ActiveObject.Clique.Computer)
				{
					DS2ActiveObject[] computerList = GameBattle.m_instance.GetComputerList();
					bool flag = true;
					for (int i = 0; i < computerList.Length; i++)
					{
						if (computerList[i].Alive())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						GameBattle.m_instance.GameState = GameBattle.State.Win;
					}
				}
				else
				{
					GameBattle.m_instance.GameState = GameBattle.State.Failed;
				}
				m_bCheckWin = true;
			}
			if (m_bToDestroy)
			{
				m_timer += Time.deltaTime;
				if (m_timer >= 3f)
				{
					m_activeObject.OnDeathOver();
				}
			}
			else if (m_bToResurgence)
			{
				m_timer += Time.deltaTime;
				if (m_timer >= 2f)
				{
					Player player2 = (Player)m_activeObject;
					float resHpPercent = DataCenter.Conf().m_teamAttributeResurrection.resHpPercent;
					player2.hp = (int)((float)player2.hpMax * resHpPercent);
					player2.Reset();
					player2.SwitchFSM(player2.GetDefaultAIState());
				}
			}
		}
	}
}

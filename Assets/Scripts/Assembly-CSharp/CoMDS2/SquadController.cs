using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoMDS2
{
	public class SquadController : Character
	{
		public enum SquadControllerStateType
		{
			SquadControllerStateIdle = 0,
			SquadControllerStateMove = 1,
			SquadControllerStateAttack = 2,
			SquadControllerStateMoveAttack = 3
		}

		private SquadControllerStateType m_state;

		private bool m_fireKeyDown;

		private bool m_moveKeyDown;

		private bool m_bFaceToMoveDirection = true;

		private GameObject m_moveCopy;

		public Vector3[] SquadSite = new Vector3[5];

		private float m_squadRadius = 0.5f;

		private Transform[] points = new Transform[5];

		private float m_teammemberGuardDistance = 5f;

		private GameObject m_squadPoints;

		private List<DS2ActiveObject> m_enemiesInTargetArea;

		private MeshRenderer[] m_meshRenderer;

		private float m_originalSpeed;

		private float m_reduceSpeed;

		public float neareastDistanceFromTeammember = float.MaxValue;

		private bool m_stayToWait;

		private int m_teamCount = -1;

		private float m_playerRotateAngle;

		private float m_playerRotateAngleMark;

		private float m_left_lparam;

		private float m_left_last_lparam;

		private float m_right_last_lparam;

		private float m_right_lparam_lasttime = -1f;

		private bool m_tempShowSC;

		public SquadControllerStateType SquadControllerState
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}

		public bool FaceToMoveDirection
		{
			get
			{
				return m_bFaceToMoveDirection;
			}
			set
			{
				m_bFaceToMoveDirection = value;
			}
		}

		public bool FireKeyDown
		{
			get
			{
				return m_fireKeyDown;
			}
			set
			{
				m_fireKeyDown = value;
				if (m_fireKeyDown)
				{
					if (m_move)
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateMoveAttack;
					}
					else
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateAttack;
					}
				}
				else if (m_move)
				{
					SquadControllerState = SquadControllerStateType.SquadControllerStateMove;
				}
				else
				{
					SquadControllerState = SquadControllerStateType.SquadControllerStateIdle;
				}
			}
		}

		public bool MoveKeyDown
		{
			get
			{
				return m_moveKeyDown;
			}
			set
			{
				m_moveKeyDown = value;
				SetMove(m_moveKeyDown, FaceDirection);
				if (m_moveKeyDown)
				{
					SquadControllerState = SquadControllerStateType.SquadControllerStateMove;
				}
			}
		}

		public override Vector3 FaceDirection
		{
			get
			{
				return base.FaceDirection;
			}
			set
			{
				base.FaceDirection = value;
			}
		}

		public float SquadRadius
		{
			get
			{
				return m_squadRadius;
			}
			set
			{
				m_squadRadius = value;
				m_characterController.radius = m_squadRadius + 0.5f;
				UpdatePointsPosition();
			}
		}

		public float TeammemberGuardDistance
		{
			get
			{
				return m_teammemberGuardDistance;
			}
			set
			{
				m_teammemberGuardDistance = value;
			}
		}

		public float OriginalSpeed
		{
			get
			{
				return m_originalSpeed;
			}
			set
			{
				m_originalSpeed = value;
			}
		}

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, 29);
			base.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_SQUADCONTROLLER;
			MoveSpeed = 4f;
			m_meshRenderer = GetGameObject().GetComponentsInChildren<MeshRenderer>();
			m_squadPoints = new GameObject();
			m_squadPoints.name = "SquadPoints";
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = GetTransform().Find("point" + i);
				points[i].parent = m_squadPoints.transform;
			}
			float num = Vector3.Angle(Vector3.zero, new Vector3(-0.1f, 0f, 1f));
			SquadRadius = 1.8f;
			TeammemberGuardDistance = 5f;
			m_tempShowSC = true;
			ShowSquadController();
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Models/Characters/LeaderHaloSquad") as GameObject) as GameObject;
			gameObject.name = "LeaderHaloSquad";
			gameObject.transform.parent = GetTransform();
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localPosition = Vector2.zero;
			m_enemiesInTargetArea = new List<DS2ActiveObject>();
		}

		protected override void CreateCharacterAttribute()
		{
			m_characterController = GetGameObject().GetComponent<CharacterController>();
		}

		public override Transform GetMoveTransform()
		{
			return Character.s_spawnTransform;
		}

		public override void Update(float deltaTime)
		{
			float num = Vector3.Angle(Vector3.forward, FaceDirection);
			Player player = GameBattle.m_instance.GetPlayer();
			m_enemiesInTargetArea.Clear();
			int layerMask = ((base.clique != Clique.Computer) ? 526336 : 1536);
			int num2 = ((base.clique != Clique.Computer) ? 526336 : 1536);
			Ray ray = new Ray(new Vector3(GetTransform().position.x, 1f, GetTransform().position.z), FaceDirection);
			RaycastHit[] array = Physics.SphereCastAll(ray, SquadRadius, player.m_weapon.attribute.attackRange, layerMask);
			if (array != null && array.Length > 0)
			{
				RaycastHit[] array2 = array;
				foreach (RaycastHit raycastHit in array2)
				{
					DS2ActiveObject @object = DS2ObjectStub.GetObject<DS2ActiveObject>(raycastHit.collider.gameObject);
					m_enemiesInTargetArea.Add(@object);
				}
			}
			if (!m_fire)
			{
				GetTransform().forward = FaceDirection;
				m_squadPoints.transform.position = GetTransform().position;
				m_squadPoints.transform.rotation = GetTransform().rotation;
			}
			else
			{
				m_squadPoints.transform.position = GetTransform().position;
			}
			for (int j = 0; j < points.Length; j++)
			{
				SquadSite[0] = points[0].position;
				SquadSite[1] = points[1].position;
				SquadSite[2] = points[2].position;
				SquadSite[3] = points[3].position;
				SquadSite[4] = points[4].position;
			}
			switch (m_state)
			{
			case SquadControllerStateType.SquadControllerStateMove:
				if (!m_move)
				{
					if (m_fire)
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateAttack;
					}
					else
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateIdle;
					}
				}
				else
				{
					Move(MoveDirection);
				}
				break;
			case SquadControllerStateType.SquadControllerStateMoveAttack:
				if (!m_move)
				{
					if (m_fire)
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateAttack;
					}
					else
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateIdle;
					}
				}
				else
				{
					Move(MoveDirection);
				}
				if (!m_fire)
				{
					if (m_move)
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateMove;
					}
					else
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateIdle;
					}
				}
				break;
			case SquadControllerStateType.SquadControllerStateAttack:
				if (!m_fire)
				{
					if (m_move)
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateMove;
					}
					else
					{
						SquadControllerState = SquadControllerStateType.SquadControllerStateIdle;
					}
				}
				break;
			}
			if (neareastDistanceFromTeammember > 5f)
			{
				m_stayToWait = true;
				return;
			}
			if (neareastDistanceFromTeammember < 2f)
			{
				m_stayToWait = false;
			}
			neareastDistanceFromTeammember = float.MaxValue;
		}

		private bool IsMovingTowardTeammates(Vector3 move_dir)
		{
			Vector3 position = GetTransform().position;
			Vector3 lhs = Vector3.zero;
			float num = 0f;
			DS2ActiveObject[] playerAliveList = GameBattle.m_instance.GetPlayerAliveList();
			foreach (DS2ActiveObject dS2ActiveObject in playerAliveList)
			{
				Vector3 vector = dS2ActiveObject.GetTransform().position - position;
				vector.y = 0f;
				if (vector.sqrMagnitude > num)
				{
					num = vector.sqrMagnitude;
					lhs = vector;
				}
			}
			return num > 0f && Vector3.Dot(lhs.normalized, move_dir.normalized) > 0f;
		}

		public void UpdateInput(float left_wparam, float left_lparam, float right_wparam, float right_lparam)
		{
			if (base.isStuck)
			{
				return;
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				Vector3 vector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
				if (vector.x != 0f || vector.z != 0f)
				{
					if (!base.isFrozen && !base.isStuck)
					{
						SetMove(true, vector);
						if (!MoveKeyDown)
						{
							MoveKeyDown = true;
						}
						if (!m_fire)
						{
							FaceDirection = vector;
							GetTransform().forward = FaceDirection;
						}
					}
				}
				else if (left_lparam != 0f || left_wparam != 0f)
				{
					if (!base.isFrozen && !base.isStuck)
					{
						if (m_left_lparam < 1f)
						{
							m_left_lparam += 0.1f;
							if (m_left_lparam > 1f)
							{
								m_left_lparam = 1f;
							}
						}
						else if (left_wparam < 1f && m_left_last_lparam != left_wparam)
						{
							m_left_lparam = left_wparam;
						}
						vector = new Vector3(Mathf.Cos(left_lparam), 0f, Mathf.Sin(left_lparam)) * m_left_lparam;
						SetMove(true, vector);
						m_left_last_lparam = left_wparam;
						if (!MoveKeyDown)
						{
							MoveKeyDown = true;
						}
						if (!m_fire)
						{
							FaceDirection = vector;
							GetTransform().forward = FaceDirection;
						}
					}
				}
				else
				{
					m_left_lparam = 0f;
					if (DataCenter.State().isPVPMode)
					{
						if (MoveKeyDown)
						{
							MoveKeyDown = false;
						}
					}
					else
					{
						SetMove(false, FaceDirection);
						if (MoveKeyDown)
						{
							MoveKeyDown = false;
						}
					}
				}
			}
			else if (left_lparam != 0f)
			{
				if (!base.isFrozen && !base.isStuck)
				{
					if (m_left_lparam < 1f)
					{
						m_left_lparam += 0.1f;
						if (m_left_lparam > 1f)
						{
							m_left_lparam = 1f;
						}
					}
					else if (left_wparam < 1f && m_left_last_lparam != left_wparam)
					{
						m_left_lparam = left_wparam;
					}
					m_left_last_lparam = left_wparam;
					Vector3 vector2 = new Vector3(Mathf.Cos(left_lparam), 0f, Mathf.Sin(left_lparam)) * m_left_lparam;
					SetMove(true, vector2);
					if (!MoveKeyDown)
					{
						MoveKeyDown = true;
					}
					if (!m_fire)
					{
						FaceDirection = vector2;
						GetTransform().forward = FaceDirection;
					}
				}
			}
			else if (DataCenter.State().isPVPMode)
			{
				if (MoveKeyDown)
				{
					MoveKeyDown = false;
				}
			}
			else
			{
				SetMove(false, FaceDirection);
				if (MoveKeyDown)
				{
					MoveKeyDown = false;
				}
			}
			if (right_lparam != 0f)
			{
				Vector3 vector3 = new Vector3(Mathf.Cos(right_lparam), 0f, Mathf.Sin(right_lparam));
				m_right_last_lparam = right_lparam;
				float num = 90f - right_lparam * 57.29578f;
				if (!m_fire)
				{
					SetFire(true, vector3);
				}
				if (!FireKeyDown)
				{
					FireKeyDown = true;
				}
				FaceToMoveDirection = false;
				FaceDirection = vector3;
				GetTransform().forward = FaceDirection;
			}
			else
			{
				if (FireKeyDown)
				{
					FireKeyDown = false;
				}
				if (m_fire)
				{
					SetFire(false, m_fireDirection);
				}
			}
		}

		public override void SetFire(bool fire, Vector3 fireDirection)
		{
			m_fire = fire;
		}

		public override HitResultInfo OnHit(HitInfo hitInfo)
		{
			int num = UnityEngine.Random.Range(0, 100);
			return base.OnHit(hitInfo);
		}

		public override void OnHurt(bool switchHurtState)
		{
		}

		public override void OnDeath()
		{
		}

		public override IPathFinding GetPathFinding()
		{
			return null;
		}

		public override void SetMove(bool move, Vector3 moveDirection)
		{
			base.SetMove(move, moveDirection);
		}

		private void Move(Vector3 move_dir)
		{
			if (!m_stayToWait || IsMovingTowardTeammates(move_dir))
			{
				CharacterController component = GetGameObject().GetComponent<CharacterController>();
				if (component != null)
				{
					component.Move(move_dir * MoveSpeed * Time.deltaTime);
				}
			}
		}

		public void ShowSquadController()
		{
			m_tempShowSC = !m_tempShowSC;
			for (int i = 0; i < m_meshRenderer.Length; i++)
			{
				m_meshRenderer[i].enabled = m_tempShowSC;
			}
		}

		public DS2ActiveObject[] GetEnemyListInTargetArea()
		{
			return m_enemiesInTargetArea.ToArray();
		}

		public DS2ActiveObject GetNeareastEnemyInTargetArea(Character source)
		{
			DS2ActiveObject dS2ActiveObject = null;
			if (m_enemiesInTargetArea.Count > 0)
			{
				dS2ActiveObject = m_enemiesInTargetArea[0];
				float sqrMagnitude = (dS2ActiveObject.GetTransform().position - source.GetTransform().position).sqrMagnitude;
				foreach (DS2ActiveObject item in m_enemiesInTargetArea)
				{
					if (item.Alive())
					{
						float sqrMagnitude2 = (item.GetTransform().position - source.GetTransform().position).sqrMagnitude;
						if (sqrMagnitude > sqrMagnitude2)
						{
							dS2ActiveObject = item;
						}
					}
				}
			}
			return dS2ActiveObject;
		}

		public void UpdatePointsPosition()
		{
			int num = GameBattle.m_instance.GetPlayerAliveList().Length;
			if (m_teamCount == num || num <= 0)
			{
				return;
			}
			m_teamCount = num;
			float num2 = 0f;
			if (m_teamCount == 3)
			{
				num2 = 90f;
			}
			else if (m_teamCount == 4)
			{
				num2 = 45f;
			}
			else if (m_teamCount == 5)
			{
				num2 = 90f;
			}
			for (int i = 0; i < points.Length; i++)
			{
				if (i < m_teamCount)
				{
					float num3 = num2 + (float)i * (360f / (float)m_teamCount);
					Vector3 vector = new Vector3(Mathf.Cos(num3 * ((float)Math.PI / 180f)), 0f, Mathf.Sin(num3 * ((float)Math.PI / 180f)));
					points[i].localPosition = vector * SquadRadius;
					SphereCollider component = points[i].gameObject.GetComponent<SphereCollider>();
					component.radius = TeammemberGuardDistance;
				}
			}
			SquadSite[0] = points[0].position;
			SquadSite[1] = points[1].position;
			SquadSite[2] = points[2].position;
			SquadSite[3] = points[3].position;
			SquadSite[4] = points[4].position;
		}
	}
}

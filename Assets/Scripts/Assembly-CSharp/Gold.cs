using CoMDS2;
using UnityEngine;

public class Gold : MonoBehaviour
{
	public enum GoldState
	{
		Jump = 0,
		Trace = 1
	}

	public int money;

	public bool isCrystal;

	private GoldState m_state;

	private Rigidbody m_rigidBody;

	private float m_idleTime = 5f;

	private float m_timer;

	private float speed = 10f;

	private float accelerate = 0.04f;

	public float IdleTime
	{
		get
		{
			return m_idleTime;
		}
		set
		{
			m_idleTime = value;
		}
	}

	private Defined.EFFECT_TYPE PickupEffect
	{
		get
		{
			return isCrystal ? Defined.EFFECT_TYPE.crystal : Defined.EFFECT_TYPE.gold;
		}
	}

	private void Awake()
	{
		m_rigidBody = base.gameObject.GetComponent<Rigidbody>();
	}

	private void Start()
	{
		m_timer = 0f;
		speed = 10f;
		m_state = GoldState.Jump;
	}

	private void FixedUpdate()
	{
		Player player2 = GameBattle.m_instance.GetPlayer();
		if (m_state == GoldState.Jump)
		{
			Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
			Vector2 vector2 = new Vector2(player2.m_effectPointGround.position.x, player2.m_effectPointGround.position.z);
			float sqrMagnitude = (vector2 - vector).sqrMagnitude;
			m_timer += Time.deltaTime;
			if (sqrMagnitude <= 1f && m_timer > 1f)
			{
				BattleBufferManager.Instance.GenerateEffectFromBuffer(PickupEffect, player2.m_effectPoint.position, 0f, player2.m_effectPoint);
				Object.DestroyImmediate(m_rigidBody);
				base.gameObject.SetActive(false);
			}
			else if (m_timer >= m_idleTime)
			{
				m_state = GoldState.Trace;
			}
		}
		else
		{
			Vector3 vector3 = player2.m_effectPointUpHead.position - base.transform.position;
			float sqrMagnitude2 = vector3.sqrMagnitude;
			base.transform.Translate(vector3.normalized * speed * Time.deltaTime, Space.World);
			speed *= 1f + accelerate;
			if (sqrMagnitude2 <= 1f)
			{
				BattleBufferManager.Instance.GenerateEffectFromBuffer(PickupEffect, player2.m_effectPoint.position, 0f, player2.m_effectPoint);
				Object.DestroyImmediate(m_rigidBody);
				base.gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		base.transform.Rotate(Vector3.up, 720f * Time.deltaTime);
	}

	public void Emit()
	{
		m_state = GoldState.Jump;
		m_timer = 0f;
		int num = Random.Range(50, 100);
		int num2 = Random.Range(300, 350);
		num = ((Random.Range(0, 2) != 0) ? (-num) : num);
		Emit(num, num2, num);
	}

	public void Emit(Vector3 force)
	{
		speed = 10f;
		m_state = GoldState.Jump;
		m_timer = 0f;
		m_rigidBody = base.gameObject.GetComponent<Rigidbody>();
		if (m_rigidBody == null)
		{
			m_rigidBody = base.gameObject.AddComponent<Rigidbody>();
		}
		m_rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
		m_rigidBody.AddForce(force);
	}

	public void Emit(float forceX, float forceY, float forceZ)
	{
		m_state = GoldState.Jump;
		m_timer = 0f;
		m_rigidBody.AddForce(forceX, forceY, forceZ);
	}
}

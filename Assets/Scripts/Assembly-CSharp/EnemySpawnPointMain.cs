using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class EnemySpawnPointMain : EnemySpawnPoint
{
	public struct EnemyWaitSpawnInfo
	{
		public Enemy.EnemyType type;

		public Enemy.SpawnInfo spawnInfo;

		public Enemy.EnemyEliteType eliteType;

		public bool isBoss;

		public List<SpecialAttribute.AttributeType> specialAttribute;
	}

	public List<SpawnEnemyWave> spawnWaves;

	public float frequency = 0.5f;

	public int spawnOneTime = 1;

	public int storyPoint;

	public GameObject openDoor;

	private float m_time;

	private int m_iWave;

	private bool m_triggered;

	private List<int> m_currentSpawnWaves;

	private List<int> m_spawnDoneWaves;

	private List<int> m_timeToSpawnWaves;

	private List<int> m_timeToSpawnDoneWaves;

	private float m_totalTime;

	public bool isTriggered
	{
		get
		{
			return m_triggered;
		}
	}

	private void Awake()
	{
		m_currentSpawnWaves = new List<int>();
		m_spawnDoneWaves = new List<int>();
		m_spawnDoneWaves.Clear();
		m_timeToSpawnWaves = new List<int>();
		m_timeToSpawnWaves.Clear();
		m_timeToSpawnDoneWaves = new List<int>();
		m_timeToSpawnDoneWaves.Clear();
		m_iWave = 0;
	}

	public void Start()
	{
		AddCurrentSpawnWave(0);
		for (int i = 0; i < spawnWaves.Count; i++)
		{
			spawnWaves[i].Init(i);
			if (spawnWaves[i].waitSpawnTime > 0f)
			{
				m_timeToSpawnWaves.Add(i);
			}
		}
	}

	public override void Update()
	{
		if (GameBattle.m_instance.GameState != GameBattle.State.Game || GameBattle.s_storyPoint < storyPoint || GameBattle.s_enemyCount >= 30)
		{
			return;
		}
		if (!m_triggered)
		{
			DS2ActiveObject nearestObjFromPlayerList = GameBattle.m_instance.GetNearestObjFromPlayerList(base.gameObject.transform.position, true);
			if (nearestObjFromPlayerList != null)
			{
				float sqrMagnitude = (nearestObjFromPlayerList.GetTransform().position - base.gameObject.transform.position).sqrMagnitude;
				if (triggerDistance == -1f || sqrMagnitude <= triggerDistance * triggerDistance)
				{
					GameBattle.s_envetProcessingCount++;
					GameBattle.s_enemyLimitCount = 30;
					m_triggered = true;
					GoldenGoblinSpawner.OnChainTriggered(this);
					ChestSpawner.OnChainTriggered(this);
					GameBattle.m_instance.bMainPointInProcess = true;
					MinimapNGUI.instance.SetSpawnPointInBattle(base.gameObject.GetInstanceID());
					GameBattle.m_instance.GetPlayer().audioTalkManager.PlayFire();
					if (!DataCenter.Save().BattleTutorialFinished)
					{
						if (Tutorial.Instance.TutorialPahseFire == Tutorial.TutorialPhaseState.None)
						{
							Tutorial.Instance.TutorialPahseFire = Tutorial.TutorialPhaseState.InProgress;
						}
					}
					else if (!DataCenter.Save().tutorialChangeMode && Tutorial.Instance.TutorialPahseChangeMode == Tutorial.TutorialPhaseState.None)
					{
						Tutorial.Instance.TutorialPahseChangeMode = Tutorial.TutorialPhaseState.InProgress;
					}
				}
			}
		}
		if (!m_triggered)
		{
			return;
		}
		m_totalTime += Time.deltaTime;
		if (m_timeToSpawnWaves.Count > 0)
		{
			for (int i = 0; i < m_timeToSpawnWaves.Count; i++)
			{
				if (m_totalTime >= spawnWaves[m_timeToSpawnWaves[i]].waitSpawnTime)
				{
					AddCurrentSpawnWave(m_timeToSpawnWaves[i]);
					m_timeToSpawnDoneWaves.Add(i);
				}
			}
		}
		if (m_timeToSpawnDoneWaves.Count > 0)
		{
			for (int j = 0; j < m_timeToSpawnDoneWaves.Count; j++)
			{
				m_timeToSpawnWaves.Remove(m_timeToSpawnDoneWaves[j]);
			}
			m_timeToSpawnDoneWaves.Clear();
		}
		m_time += Time.deltaTime;
		if (m_time > frequency)
		{
			m_time = 0f;
			SpawnEnemy();
		}
	}

	protected override void SpawnEnemy()
	{
		if (spawnWaves == null || spawnWaves.Count == 0)
		{
			return;
		}
		for (int i = 0; i < spawnOneTime; i++)
		{
			for (int j = 0; j < m_currentSpawnWaves.Count; j++)
			{
				int num = m_currentSpawnWaves[j];
				spawnWaves[num].SpawnEnemy(Vector3.zero, Quaternion.identity, this);
				if (spawnWaves[num].WaveEnd())
				{
					if (m_iWave == num)
					{
						m_iWave++;
					}
					m_spawnDoneWaves.Add(num);
				}
			}
			AddCurrentSpawnWave(m_iWave);
			for (int k = 0; k < m_spawnDoneWaves.Count; k++)
			{
				m_currentSpawnWaves.Remove(m_spawnDoneWaves[k]);
			}
			m_spawnDoneWaves.Clear();
			if (m_currentSpawnWaves.Count != 0)
			{
				continue;
			}
			Object.Destroy(base.gameObject);
			GameBattle.s_envetProcessingCount--;
			if (GameBattle.s_envetProcessingCount <= 0)
			{
				GameBattle.s_storyPoint++;
				GameBattle.s_envetProcessingCount = 0;
				GameBattle.s_enemyLimitCount = 0;
				GameBattle.m_instance.bMainPointInProcess = false;
				MinimapNGUI.instance.SetSpawnPointEnable(base.gameObject.GetInstanceID(), false);
				if (openDoor != null)
				{
					TriggerEvent component = openDoor.GetComponent<TriggerEvent>();
					component.Enable = true;
				}
				ChestSpawner.OnAreaCleared(this, MinimapNGUI.instance.GetSpawnPointLeftCount() <= 0);
				if (MinimapNGUI.instance.GetSpawnPointLeftCount() <= 0)
				{
					GameBattle.m_instance.GameState = GameBattle.State.Win;
				}
			}
			break;
		}
	}

	protected virtual void AddCurrentSpawnWave(int wave)
	{
		if (wave >= 0 && wave < spawnWaves.Count && !m_currentSpawnWaves.Contains(wave))
		{
			m_currentSpawnWaves.Add(wave);
		}
	}

	public override void KillEnemy(int wave = 0)
	{
		spawnWaves[wave].Kill();
	}

	protected override void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 0f, 1f, 0.8f);
		Gizmos.DrawWireSphere(base.transform.position, triggerDistance);
		Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
		foreach (SpawnEnemyWave spawnWave in spawnWaves)
		{
			if (spawnWave.spawnPosition != null)
			{
				Gizmos.DrawLine(base.transform.position, spawnWave.spawnPosition.transform.position);
			}
		}
		if (openDoor != null)
		{
			Gizmos.color = new Color(0f, 0f, 1f, 0.8f);
			Gizmos.DrawLine(base.transform.position, openDoor.transform.position);
		}
	}

	protected override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 0f, 1f, 0.8f);
		Gizmos.DrawSphere(base.transform.position, 1f);
		Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
		Gizmos.DrawWireSphere(base.transform.position, triggerDistance);
		Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
		foreach (SpawnEnemyWave spawnWave in spawnWaves)
		{
			if (spawnWave.spawnPosition != null)
			{
				Gizmos.DrawLine(base.transform.position, spawnWave.spawnPosition.transform.position);
			}
		}
		if (openDoor != null)
		{
			Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
			Gizmos.DrawLine(base.transform.position, openDoor.transform.position);
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoMDS2
{
	public class BattleBufferManager : MonoBehaviour
	{
		public enum PreLoadObjectBufferType
		{
			NinjiaKillSpawnBulletFire = 0,
			NinjiaKillSpawnBulletIce = 1,
			EnemySpecAttrLeakage = 2,
			EnemySpecAttrFireBall = 3,
			EnemySpecAttrIceBall = 4,
			Gold = 5
		}

		private static BattleBufferManager m_instance;

		private DS2ObjectBuffer[] m_bulletBuffer;

		private Dictionary<Enemy.EnemyType, DS2ObjectBuffer> m_enemyBuffer;

		private Dictionary<Defined.EFFECT_TYPE, DS2ObjectBuffer> m_effectBuffer;

		private Dictionary<PreLoadObjectBufferType, DS2ObjectBuffer> m_objectBuffer;

		private Dictionary<SpecialAttribute.SpecialAttributeEffectType, DS2ObjectBuffer> m_specialEffectBuffer;

		private List<DS2Object> m_interactObjectListForUIExhibition;

		private List<DS2Object> m_interactObjectNeedDeleteListForUIExhibition;

		public static GameObject s_enemyGameObjectRoot;

		public static GameObject s_activeObjectRoot;

		public static GameObject s_effectObjectRoot;

		public static GameObject s_bulletObjectRoot;

		public static GameObject s_specialEffectObjectRoot;

		public static GameObject s_objectRoot;

		[HideInInspector]
		public GameObject m_miasmaUI;

		[HideInInspector]
		public EffectControl m_miasmaCoverOn;

		[HideInInspector]
		public EffectControl m_miasmaCoverEnd;

		public static BattleBufferManager Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new BattleBufferManager();
				}
				return m_instance;
			}
		}

		private void Awake()
		{
			if (m_instance == null)
			{
				m_instance = this;
				Init();
			}
		}

		private void Init()
		{
			m_effectBuffer = new Dictionary<Defined.EFFECT_TYPE, DS2ObjectBuffer>();
			m_effectBuffer.Clear();
			m_enemyBuffer = new Dictionary<Enemy.EnemyType, DS2ObjectBuffer>();
			m_specialEffectBuffer = new Dictionary<SpecialAttribute.SpecialAttributeEffectType, DS2ObjectBuffer>();
			m_interactObjectListForUIExhibition = new List<DS2Object>();
			m_interactObjectListForUIExhibition.Clear();
			m_interactObjectNeedDeleteListForUIExhibition = new List<DS2Object>();
			m_interactObjectNeedDeleteListForUIExhibition.Clear();
			s_enemyGameObjectRoot = new GameObject();
			s_enemyGameObjectRoot.name = "Enemy";
			s_activeObjectRoot = new GameObject();
			s_activeObjectRoot.name = "ActiveObjects";
			s_effectObjectRoot = new GameObject();
			s_effectObjectRoot.name = "Effect";
			s_bulletObjectRoot = new GameObject();
			s_bulletObjectRoot.name = "Bullet";
			s_objectRoot = new GameObject();
			s_objectRoot.name = "objects";
			s_specialEffectObjectRoot = new GameObject();
			s_specialEffectObjectRoot.name = "SpecialEffects";
			s_specialEffectObjectRoot.transform.parent = s_effectObjectRoot.transform;
			if (Util.s_debug || DataCenter.State().isPVPMode)
			{
				DataCenter.Conf().LoadSelectGameLevelDataFromDisk("GameLevel_001");
				DataCenter.Conf().SetCurrentGameLevel(DataCenter.State().selectLevelMode, 18);
				DataCenter.Save().selectLevelDropData = new LevelDropData();
			}
			if (!DataCenter.State().isPVPMode)
			{
				CreateEnemyBuffer();
			}
			CreateEffectBuffer();
			CreateObjectBuffer();
			CreateGoldBuffer();
		}

		public Bullet GetBulletFromBuffer(Bullet.BULLET_TYPE type)
		{
			return m_bulletBuffer[(int)type].GetObject() as Bullet;
		}

		private void CreateEnemyBuffer()
		{
			GameObject gameObject = GameObject.Find("EnemySpawnPoint");
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = gameObject.transform.Find("SpawnPointChain").gameObject;
			m_enemyBuffer = new Dictionary<Enemy.EnemyType, DS2ObjectBuffer>();
			EnemySpawnPointMain[] componentsInChildren = gameObject2.GetComponentsInChildren<EnemySpawnPointMain>();
			Dictionary<Enemy.EnemyType, int> dictionary = new Dictionary<Enemy.EnemyType, int>();
			Dictionary<Enemy.EnemyType, int> dictionary2 = new Dictionary<Enemy.EnemyType, int>();
			List<SpecialAttribute.AttributeType> list = new List<SpecialAttribute.AttributeType>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				dictionary.Clear();
				for (int j = 0; j < componentsInChildren[i].spawnWaves.Count; j++)
				{
					for (int k = 0; k < componentsInChildren[i].spawnWaves[j].m_spawnInfos.Count; k++)
					{
						SpawnEnemyWave.SpawnInfo spawnInfo = componentsInChildren[i].spawnWaves[j].m_spawnInfos[k];
						if (dictionary.ContainsKey(spawnInfo.type))
						{
							Dictionary<Enemy.EnemyType, int> dictionary3;
							Dictionary<Enemy.EnemyType, int> dictionary4 = (dictionary3 = dictionary);
							Enemy.EnemyType type;
							Enemy.EnemyType key = (type = spawnInfo.type);
							int num = dictionary3[type];
							dictionary4[key] = num + spawnInfo.count;
						}
						else
						{
							dictionary.Add(spawnInfo.type, spawnInfo.count);
						}
						if (spawnInfo.specialAttribute != null && spawnInfo.specialAttribute.Count > 0)
						{
							for (int l = 0; l < spawnInfo.specialAttribute.Count; l++)
							{
								list.Add(spawnInfo.specialAttribute[l]);
							}
						}
					}
				}
				foreach (Enemy.EnemyType key3 in dictionary.Keys)
				{
					if (key3 == Enemy.EnemyType.FatCook)
					{
						if (dictionary2.ContainsKey(Enemy.EnemyType.Zombie))
						{
							dictionary2[Enemy.EnemyType.Zombie] = Math.Max(dictionary2[Enemy.EnemyType.Zombie], 20);
						}
						else
						{
							dictionary2.Add(Enemy.EnemyType.Zombie, 20);
						}
						if (dictionary2.ContainsKey(Enemy.EnemyType.ZombieBomb))
						{
							dictionary2[Enemy.EnemyType.ZombieBomb] = Math.Max(dictionary2[Enemy.EnemyType.ZombieBomb], 10);
						}
						else
						{
							dictionary2.Add(Enemy.EnemyType.ZombieBomb, 10);
						}
					}
					if (dictionary2.ContainsKey(key3))
					{
						if (dictionary2[key3] < dictionary[key3])
						{
							dictionary2[key3] = dictionary[key3];
						}
					}
					else
					{
						dictionary2.Add(key3, dictionary[key3]);
					}
				}
			}
			EnemySpawnPointEvent[] componentsInChildren2 = gameObject2.GetComponentsInChildren<EnemySpawnPointEvent>();
			for (int m = 0; m < componentsInChildren2.Length; m++)
			{
				Enemy.EnemyType type2 = componentsInChildren2[m].type;
				if (type2 == Enemy.EnemyType.FatCook)
				{
					if (dictionary2.ContainsKey(Enemy.EnemyType.Zombie))
					{
						dictionary2[Enemy.EnemyType.Zombie] = Math.Max(dictionary2[Enemy.EnemyType.Zombie], 20);
					}
					else
					{
						dictionary2.Add(Enemy.EnemyType.Zombie, 20);
					}
					if (dictionary2.ContainsKey(Enemy.EnemyType.ZombieBomb))
					{
						dictionary2[Enemy.EnemyType.ZombieBomb] = Math.Max(dictionary2[Enemy.EnemyType.ZombieBomb], 10);
					}
					else
					{
						dictionary2.Add(Enemy.EnemyType.ZombieBomb, 10);
					}
				}
				if (dictionary2.ContainsKey(type2))
				{
					Dictionary<Enemy.EnemyType, int> dictionary5;
					Dictionary<Enemy.EnemyType, int> dictionary6 = (dictionary5 = dictionary2);
					Enemy.EnemyType type;
					Enemy.EnemyType key2 = (type = type2);
					int num = dictionary5[type];
					dictionary6[key2] = num + componentsInChildren2[m].count;
				}
				else
				{
					dictionary2.Add(type2, componentsInChildren2[m].count);
				}
			}
			foreach (Enemy.EnemyType key4 in dictionary2.Keys)
			{
				CreateEnemyBufferByType(key4, dictionary2[key4]);
			}
			m_specialEffectBuffer = new Dictionary<SpecialAttribute.SpecialAttributeEffectType, DS2ObjectBuffer>();
			for (int n = 0; n < list.Count; n++)
			{
				SpecialAttribute.AttributeType type3 = list[n];
				CreateSpecialAttributeEffectBufferByType(type3);
			}
		}

		public void CreateEnemyBufferByType(Enemy.EnemyType type, int count)
		{
			DataConf.EnemyData enemyDataByType = DataCenter.Conf().GetEnemyDataByType(type);
			if (enemyDataByType == null)
			{
				Debug.LogError(type.ToString() + " is null!");
			}
			GameObject gameObject = new GameObject();
			gameObject.name = enemyDataByType.name;
			gameObject.transform.parent = s_enemyGameObjectRoot.transform;
			int num = ((count <= 30) ? count : 30);
			if (m_enemyBuffer.ContainsKey(type))
			{
				int size = m_enemyBuffer[type].Size;
				if (size + num > 30)
				{
					num = 30 - size;
					if (num < 0)
					{
						return;
					}
				}
			}
			DS2ObjectBuffer dS2ObjectBuffer = new DS2ObjectBuffer(num);
			for (int i = 0; i < num; i++)
			{
				Enemy enemy = CharacterBuilder.CreateEnemy(type, Vector3.zero, Quaternion.identity, 11);
				enemy.GetTransform().parent = gameObject.transform;
				dS2ObjectBuffer.AddObj(enemy);
			}
			if (m_enemyBuffer.ContainsKey(type))
			{
				m_enemyBuffer[type].AddBuffer(dS2ObjectBuffer);
			}
			else
			{
				m_enemyBuffer.Add(type, dS2ObjectBuffer);
			}
		}

		public void CreateSpecialAttributeEffectBufferByType(SpecialAttribute.AttributeType type)
		{
			switch (type)
			{
			case SpecialAttribute.AttributeType.Electric:
			{
				DataConf.SpecialEffectData specialEffectDataByIndex3 = DataCenter.Conf().GetSpecialEffectDataByIndex((int)DataCenter.Conf().m_specialAttributeElectric.effects[0].type);
				DS2ObjectBuffer dS2ObjectBuffer5 = new DS2ObjectBuffer(DataCenter.Conf().m_specialAttributeElectric.effects[0].bufferCount);
				for (int m = 0; m < dS2ObjectBuffer5.Size; m++)
				{
					Bullet bullet3 = new Bullet(null);
					bullet3.Initialize(Resources.Load<GameObject>("Models/Bullets/" + specialEffectDataByIndex3.fileName), specialEffectDataByIndex3.fileName, Vector3.zero, Quaternion.identity, 0);
					GameObject gameObject4 = bullet3.GetGameObject();
					gameObject4.AddComponent<LinearMoveToDestroy>();
					gameObject4.AddComponent<BulletTriggerScript>();
					bullet3.hitInfo.hitEffect = Defined.EFFECT_TYPE.NONE;
					if (bullet3.attribute == null)
					{
						bullet3.attribute = new Bullet.BulletAttribute();
					}
					bullet3.attribute.speed = 4f;
					bullet3.attribute.effectHit = Defined.EFFECT_TYPE.Leakage_Attack;
					bullet3.hitInfo.bSkill = true;
					gameObject4.SetActive(false);
					gameObject4.transform.parent = s_specialEffectObjectRoot.transform;
					dS2ObjectBuffer5.AddObj(bullet3);
				}
				Instance.AddObjectToPreloadBuffer(PreLoadObjectBufferType.EnemySpecAttrLeakage, dS2ObjectBuffer5);
				Instance.CreateEffectBufferByType(Defined.EFFECT_TYPE.Leakage_Attack);
				return;
			}
			case SpecialAttribute.AttributeType.DeadSpawnBomb:
			{
				SpecialAttribute.SpecialAttributeData specialAttributeData2 = DataCenter.Conf().GetSpecialAttributeData(type);
				DataConf.SpecialEffectData specialEffectDataByIndex2 = DataCenter.Conf().GetSpecialEffectDataByIndex((int)DataCenter.Conf().m_specialAttributeDeadSpawnBomb.effects[0].type);
				DS2ObjectBuffer dS2ObjectBuffer4 = new DS2ObjectBuffer(20);
				for (int l = 0; l < dS2ObjectBuffer4.Size; l++)
				{
					EnemyHealthBomb enemyHealthBomb = new EnemyHealthBomb();
					enemyHealthBomb.Initialize(Resources.Load<GameObject>("Models/ActiveObjects/" + specialEffectDataByIndex2.fileName), specialEffectDataByIndex2.fileName, Vector3.zero, Quaternion.identity, 0);
					GameObject gameObject3 = enemyHealthBomb.GetGameObject();
					gameObject3.SetActive(false);
					gameObject3.transform.parent = s_activeObjectRoot.transform;
					dS2ObjectBuffer4.AddObj(enemyHealthBomb);
				}
				if (m_specialEffectBuffer.ContainsKey(specialAttributeData2.effects[0].type))
				{
					m_specialEffectBuffer[specialAttributeData2.effects[0].type].AddBuffer(dS2ObjectBuffer4, 30);
				}
				else
				{
					m_specialEffectBuffer.Add(specialAttributeData2.effects[0].type, dS2ObjectBuffer4);
				}
				return;
			}
			case SpecialAttribute.AttributeType.StrickBack:
			{
				DataConf.BulletData bulletDataByIndex2 = DataCenter.Conf().GetBulletDataByIndex(26);
				DS2ObjectBuffer dS2ObjectBuffer2 = new DS2ObjectBuffer(16);
				for (int j = 0; j < dS2ObjectBuffer2.Size; j++)
				{
					Bullet bullet2 = new Bullet(null);
					bullet2.Initialize(Resources.Load<GameObject>("Models/Bullets/" + bulletDataByIndex2.fileNmae), bulletDataByIndex2.fileNmae, Vector3.zero, Quaternion.identity, 0);
					GameObject gameObject2 = bullet2.GetGameObject();
					gameObject2.AddComponent<LinearMoveToDestroy>();
					gameObject2.AddComponent<BulletTriggerScript>();
					bullet2.hitInfo.bSkill = true;
					SpecialHitInfo specialHitInfo = new SpecialHitInfo();
					specialHitInfo.time = DataCenter.Conf().m_specialAttributeStrickBack.frozenTime;
					specialHitInfo.disposable = false;
					bullet2.hitInfo.AddSpecialHit(Defined.SPECIAL_HIT_TYPE.FROZEN, specialHitInfo);
					bullet2.hitInfo.hitEffect = (Defined.EFFECT_TYPE)bulletDataByIndex2.hitType;
					if (bullet2.attribute == null)
					{
						bullet2.attribute = new Bullet.BulletAttribute();
					}
					bullet2.attribute.speed = 2f;
					bullet2.attribute.effectHit = (Defined.EFFECT_TYPE)bulletDataByIndex2.hitType;
					gameObject2.SetActive(false);
					gameObject2.transform.parent = s_specialEffectObjectRoot.transform;
					dS2ObjectBuffer2.AddObj(bullet2);
				}
				Instance.AddObjectToPreloadBuffer(PreLoadObjectBufferType.EnemySpecAttrIceBall, dS2ObjectBuffer2);
				return;
			}
			case SpecialAttribute.AttributeType.PoisonClaw:
				CreateEffectBufferByType(Defined.EFFECT_TYPE.POISON);
				return;
			case SpecialAttribute.AttributeType.Exacerbate:
				CreateEffectBufferByType(Defined.EFFECT_TYPE.CURSE);
				return;
			case SpecialAttribute.AttributeType.Knell:
				CreateEffectBufferByType(Defined.EFFECT_TYPE.KNELL);
				return;
			case SpecialAttribute.AttributeType.Elegy:
				CreateEffectBufferByType(Defined.EFFECT_TYPE.CONFUSE);
				return;
			case SpecialAttribute.AttributeType.Grind:
				CreateEffectBufferByType(Defined.EFFECT_TYPE.WEAKNESS);
				return;
			case SpecialAttribute.AttributeType.Tornado:
			{
				SpecialAttribute.SpecialAttributeData specialAttributeData3 = DataCenter.Conf().GetSpecialAttributeData(type);
				DataConf.SpecialEffectData specialEffectDataByIndex4 = DataCenter.Conf().GetSpecialEffectDataByIndex((int)specialAttributeData3.effects[0].type);
				DS2ObjectBuffer dS2ObjectBuffer6 = new DS2ObjectBuffer(specialAttributeData3.effects[0].bufferCount);
				for (int n = 0; n < dS2ObjectBuffer6.Size; n++)
				{
					Tornado tornado = new Tornado();
					tornado.Initialize(Resources.Load<GameObject>("Models/ActiveObjects/" + specialEffectDataByIndex4.fileName), specialEffectDataByIndex4.fileName, Vector3.zero, Quaternion.identity, 0);
					tornado.SetActive(false, null);
					dS2ObjectBuffer6.AddObj(tornado);
				}
				if (Tornado.s_allySeat == null)
				{
					Tornado.s_allySeat = new AllySeat();
					Tornado.s_allySeat.Init(dS2ObjectBuffer6.Size);
				}
				if (m_specialEffectBuffer.ContainsKey(specialAttributeData3.effects[0].type))
				{
					m_specialEffectBuffer[specialAttributeData3.effects[0].type].AddBuffer(dS2ObjectBuffer6);
				}
				else
				{
					m_specialEffectBuffer.Add(specialAttributeData3.effects[0].type, dS2ObjectBuffer6);
				}
				return;
			}
			case SpecialAttribute.AttributeType.PusBlood:
			case SpecialAttribute.AttributeType.Bomb:
			case SpecialAttribute.AttributeType.LiquidNitrogen:
			{
				SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(type);
				DataConf.SpecialEffectData specialEffectDataByIndex = DataCenter.Conf().GetSpecialEffectDataByIndex((int)specialAttributeData.effects[0].type);
				DS2ObjectBuffer dS2ObjectBuffer3 = new DS2ObjectBuffer(specialAttributeData.effects[0].bufferCount);
				Spore.SporeType type2 = Spore.SporeType.Bomb;
				switch (type)
				{
				case SpecialAttribute.AttributeType.PusBlood:
					type2 = Spore.SporeType.PusBlood;
					break;
				case SpecialAttribute.AttributeType.LiquidNitrogen:
					type2 = Spore.SporeType.LiquidNitrogen;
					break;
				}
				for (int k = 0; k < dS2ObjectBuffer3.Size; k++)
				{
					Spore spore = new Spore(type2);
					spore.Initialize(Resources.Load<GameObject>("Models/ActiveObjects/" + specialEffectDataByIndex.fileName), specialEffectDataByIndex.fileName, Vector3.zero, Quaternion.identity, 0);
					spore.SetActive(false, null);
					dS2ObjectBuffer3.AddObj(spore);
				}
				if (m_specialEffectBuffer.ContainsKey(specialAttributeData.effects[0].type))
				{
					m_specialEffectBuffer[specialAttributeData.effects[0].type].AddBuffer(dS2ObjectBuffer3);
				}
				else
				{
					m_specialEffectBuffer.Add(specialAttributeData.effects[0].type, dS2ObjectBuffer3);
				}
				return;
			}
			case SpecialAttribute.AttributeType.LifeLink:
				CreateEnemyBufferByType(Enemy.EnemyType.Wrestler, 1);
				CreateEnemyBufferByType(Enemy.EnemyType.Haoke, 1);
				break;
			case SpecialAttribute.AttributeType.Summon:
				CreateEnemyBufferByType(Enemy.EnemyType.Zombie, DataCenter.Conf().m_specialAttributeSummon.zombie);
				CreateEnemyBufferByType(Enemy.EnemyType.ZombieBomb, DataCenter.Conf().m_specialAttributeSummon.zombieBomb);
				break;
			case SpecialAttribute.AttributeType.Hellfire:
			{
				DataConf.BulletData bulletDataByIndex = DataCenter.Conf().GetBulletDataByIndex(25);
				DS2ObjectBuffer dS2ObjectBuffer = new DS2ObjectBuffer(16);
				for (int i = 0; i < dS2ObjectBuffer.Size; i++)
				{
					Bullet bullet = new Bullet(null);
					bullet.Initialize(Resources.Load<GameObject>("Models/Bullets/" + bulletDataByIndex.fileNmae), bulletDataByIndex.fileNmae, Vector3.zero, Quaternion.identity, 0);
					GameObject gameObject = bullet.GetGameObject();
					gameObject.AddComponent<LinearMoveToDestroy>();
					gameObject.AddComponent<BulletTriggerScript>();
					bullet.hitInfo.bSkill = true;
					bullet.hitInfo.hitEffect = Defined.EFFECT_TYPE.NONE;
					if (bullet.attribute == null)
					{
						bullet.attribute = new Bullet.BulletAttribute();
					}
					bullet.attribute.speed = 10f;
					bullet.attribute.effectHit = Defined.EFFECT_TYPE.NONE;
					gameObject.SetActive(false);
					gameObject.transform.parent = s_specialEffectObjectRoot.transform;
					dS2ObjectBuffer.AddObj(bullet);
				}
				Instance.AddObjectToPreloadBuffer(PreLoadObjectBufferType.EnemySpecAttrFireBall, dS2ObjectBuffer);
				break;
			}
			}
			SpecialAttribute.SpecialAttributeData specialAttributeData4 = DataCenter.Conf().GetSpecialAttributeData(type);
			if (specialAttributeData4 == null)
			{
				return;
			}
			for (int num = 0; num < specialAttributeData4.effects.Count; num++)
			{
				int num2 = ((specialAttributeData4.effects[num].bufferCount != -1) ? specialAttributeData4.effects[num].bufferCount : 30);
				if (num2 <= 0)
				{
					continue;
				}
				DataConf.SpecialEffectData specialEffectDataByIndex5 = DataCenter.Conf().GetSpecialEffectDataByIndex((int)specialAttributeData4.effects[num].type);
				DS2ObjectBuffer dS2ObjectBuffer7 = new DS2ObjectBuffer(num2);
				for (int num3 = 0; num3 < dS2ObjectBuffer7.Size; num3++)
				{
					DS2HalfStaticObject dS2HalfStaticObject = new DS2HalfStaticObject();
					dS2HalfStaticObject.Initialize(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/" + specialEffectDataByIndex5.fileName)) as GameObject);
					EffectPlayCombie component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectPlayCombie>();
					if (component == null)
					{
						component = dS2HalfStaticObject.GetGameObject().AddComponent<EffectPlayCombie>();
						component.StopEmit();
					}
					dS2HalfStaticObject.GetGameObject().transform.parent = s_specialEffectObjectRoot.transform;
					dS2HalfStaticObject.GetGameObject().name = specialEffectDataByIndex5.fileName;
					dS2HalfStaticObject.GetGameObject().SetActive(false);
					dS2ObjectBuffer7.AddObj(dS2HalfStaticObject);
				}
				if (m_specialEffectBuffer.ContainsKey(specialAttributeData4.effects[num].type))
				{
					m_specialEffectBuffer[specialAttributeData4.effects[num].type].AddBuffer(dS2ObjectBuffer7, 30);
				}
				else
				{
					m_specialEffectBuffer.Add(specialAttributeData4.effects[num].type, dS2ObjectBuffer7);
				}
			}
		}

		public DS2Object GetSpecialEffectByType(SpecialAttribute.SpecialAttributeEffectType type)
		{
			if (m_specialEffectBuffer != null && m_specialEffectBuffer.ContainsKey(type))
			{
				return m_specialEffectBuffer[type].GetObject();
			}
			return null;
		}

		public Enemy GetEnemyFromBuffer(Enemy.EnemyType type)
		{
			if (m_enemyBuffer == null)
			{
				m_enemyBuffer = new Dictionary<Enemy.EnemyType, DS2ObjectBuffer>();
			}
			if (!m_enemyBuffer.ContainsKey(type))
			{
				CreateEnemyBufferByType(type, 30);
			}
			if (!m_enemyBuffer.ContainsKey(type))
			{
				return null;
			}
			return m_enemyBuffer[type].GetObject() as Enemy;
		}

		private void CreateEffectBuffer()
		{
		}

		public void CreateEffectBufferByType(Defined.EFFECT_TYPE type, int count = -1, int accumulate = -1)
		{
			if (accumulate != -1 && m_effectBuffer.ContainsKey(type) && m_effectBuffer[type].Size > accumulate)
			{
				return;
			}
			DataConf.EffectData effectDataByIndex = DataCenter.Conf().GetEffectDataByIndex((int)type);
			int count2 = ((count != -1) ? count : effectDataByIndex.bufferNum);
			GameObject gameObject = new GameObject();
			gameObject.name = effectDataByIndex.fileName;
			gameObject.transform.parent = s_effectObjectRoot.transform;
			DS2ObjectBuffer dS2ObjectBuffer = new DS2ObjectBuffer(count2);
			for (int i = 0; i < dS2ObjectBuffer.Size; i++)
			{
				DS2HalfStaticObject dS2HalfStaticObject = new DS2HalfStaticObject();
				dS2HalfStaticObject.Initialize(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/" + effectDataByIndex.fileName)) as GameObject);
				if (effectDataByIndex.playTime != -1f)
				{
					DS2ObjectDestroy dS2ObjectDestroy = dS2HalfStaticObject.GetGameObject().AddComponent<DS2ObjectDestroy>();
					dS2ObjectDestroy.TimeDestroy(effectDataByIndex.playTime, false);
					if (effectDataByIndex.animType == Defined.EffectAnimType.Particle)
					{
						EffectParticleContinuous component = dS2HalfStaticObject.GetGameObject().GetComponent<EffectParticleContinuous>();
						if (component == null)
						{
							dS2HalfStaticObject.GetGameObject().AddComponent<EffectParticleContinuous>();
						}
					}
				}
				dS2HalfStaticObject.GetGameObject().transform.parent = gameObject.transform;
				dS2HalfStaticObject.GetGameObject().name = effectDataByIndex.fileName;
				dS2HalfStaticObject.GetGameObject().SetActive(false);
				dS2ObjectBuffer.AddObj(dS2HalfStaticObject);
			}
			if (m_effectBuffer.ContainsKey(type))
			{
				m_effectBuffer[type].AddBuffer(dS2ObjectBuffer);
			}
			else
			{
				m_effectBuffer.Add(type, dS2ObjectBuffer);
			}
		}

		public DS2HalfStaticObject GetEffectFromBuffer(Defined.EFFECT_TYPE type)
		{
			if (m_effectBuffer.ContainsKey(type))
			{
				return m_effectBuffer[type].GetObject() as DS2HalfStaticObject;
			}
			return null;
		}

		public GameObject GenerateEffectFromBuffer(Defined.EFFECT_TYPE type, Vector3 position, float time = -1f, Transform bindObject = null, bool playAudio = true)
		{
			if (type == Defined.EFFECT_TYPE.NONE)
			{
				return null;
			}
			DS2HalfStaticObject effectFromBuffer = Instance.GetEffectFromBuffer(type);
			if (effectFromBuffer != null)
			{
				if (bindObject != null)
				{
					effectFromBuffer.GetTransform().parent = bindObject;
					effectFromBuffer.GetTransform().localPosition = Vector3.zero;
				}
				else
				{
					effectFromBuffer.GetTransform().position = position;
				}
				DS2ObjectDestroy component = effectFromBuffer.GetGameObject().GetComponent<DS2ObjectDestroy>();
				if (time != -1f)
				{
					component.enabled = true;
					if ((double)time > 0.0)
					{
						component.TimeDestroy(time, false);
					}
					else
					{
						component.TimeDestroy();
					}
				}
				else if (null != component)
				{
					component.enabled = false;
				}
				effectFromBuffer.GetGameObject().SetActive(true);
				EffectPlayCombie component2 = effectFromBuffer.GetGameObject().GetComponent<EffectPlayCombie>();
				if (component2 != null)
				{
					component2.StartEmit(playAudio);
				}
				else
				{
					EffectControl component3 = effectFromBuffer.GetGameObject().GetComponent<EffectControl>();
					if (component3 != null)
					{
						component3.StartEmit(playAudio);
					}
				}
				return effectFromBuffer.GetGameObject();
			}
			return null;
		}

		public void GenerateBomb(Vector3 position, float radius, DS2ActiveObject source)
		{
			Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.EFFECT_BOMB_1, position, 2f);
			int layerMask = ((source.clique != DS2ActiveObject.Clique.Computer) ? 2048 : 67636736);
			Collider[] array = Physics.OverlapSphere(position, radius, layerMask);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				DS2ActiveObject @object = DS2ObjectStub.GetObject<DS2ActiveObject>(collider.gameObject);
				HitInfo hitInfo = source.GetHitInfo();
				hitInfo.repelDirection = @object.GetTransform().position - position;
				hitInfo.source = null;
				@object.OnHit(hitInfo);
			}
		}

		private void CreateObjectBuffer()
		{
		}

		public void AddObjectToPreloadBuffer(PreLoadObjectBufferType bufferType, DS2ObjectBuffer buffer)
		{
			if (m_objectBuffer == null)
			{
				m_objectBuffer = new Dictionary<PreLoadObjectBufferType, DS2ObjectBuffer>();
			}
			if (m_objectBuffer.ContainsKey(bufferType))
			{
				m_objectBuffer[bufferType].AddBuffer(buffer);
			}
			else
			{
				m_objectBuffer.Add(bufferType, buffer);
			}
		}

		public DS2Object GetObjectFromBuffer(PreLoadObjectBufferType type)
		{
			return m_objectBuffer[type].GetObject();
		}

		private void CreateGoldBuffer()
		{
			CreateEffectBufferByType(Defined.EFFECT_TYPE.gold);
			GameObject gameObject = new GameObject();
			gameObject.name = "Gold";
			gameObject.transform.parent = s_objectRoot.transform;
			DS2ObjectBuffer dS2ObjectBuffer = new DS2ObjectBuffer(30);
			for (int i = 0; i < dS2ObjectBuffer.Size; i++)
			{
				DS2StaticObject dS2StaticObject = new DS2StaticObject();
				dS2StaticObject.Initialize(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Models/Items/Gold")) as GameObject);
				dS2StaticObject.Layer = 28;
				dS2StaticObject.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ITEM;
				dS2StaticObject.GetGameObject().transform.parent = gameObject.transform;
				dS2StaticObject.GetGameObject().name = "gold";
				dS2StaticObject.GetGameObject().SetActive(false);
				dS2ObjectBuffer.AddObj(dS2StaticObject);
			}
			AddObjectToPreloadBuffer(PreLoadObjectBufferType.Gold, dS2ObjectBuffer);
		}

		public void AddObjToInteractObjectListForUIExhibition(DS2Object obj)
		{
			m_interactObjectListForUIExhibition.Add(obj);
		}

		public void AddToInteractObjectNeedDeleteListForUIExhibition(DS2Object obj)
		{
			m_interactObjectNeedDeleteListForUIExhibition.Add(obj);
		}

		public void RemoveObjFromActiveObjectListForUIExhibition(DS2Object obj)
		{
			if (m_interactObjectListForUIExhibition.Contains(obj))
			{
				m_interactObjectListForUIExhibition.Remove(obj);
			}
		}

		public void RemoveAllObjFromActiveObjectListForUIExhibition()
		{
			foreach (DS2Object item in m_interactObjectListForUIExhibition)
			{
				item.Destroy();
			}
			m_interactObjectListForUIExhibition.Clear();
		}

		public void UpdateInteractObjListForUIExhibition(float deltaTime)
		{
			foreach (DS2Object item in m_interactObjectListForUIExhibition)
			{
				item.Update(deltaTime);
			}
			foreach (DS2Object item2 in m_interactObjectNeedDeleteListForUIExhibition)
			{
				m_interactObjectListForUIExhibition.Remove(item2);
			}
			m_interactObjectNeedDeleteListForUIExhibition.Clear();
		}
	}
}

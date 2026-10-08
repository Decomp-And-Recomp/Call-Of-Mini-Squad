using System.Collections.Generic;
using System.Xml;
using CoMDS2;
using UnityEngine;

public class DataConf
{
	public class SpecialEffectData
	{
		public int index;

		public string fileName;

		public Defined.EffectAnimType animType;

		public float playTime;
	}

	public enum ConfigType
	{
		Enemy = 0,
		Hero = 1,
		Stuff = 2,
		Weapon = 3,
		Equip = 4,
		HeroSkill = 5,
		SpecialAttribute = 6,
		TeamSpecialAttribute = 7,
		LoadingTips = 8,
		Achievement = 9,
		None = 10
	}

	public class SkillInfo
	{
		public string name;

		public Character.SkillType type;

		public float attackRange;

		public float speed;

		public float time;

		public float damage;

		public float percentDamage;

		public float repelDis;

		public float repelTime;

		public string animReady;

		public string animProcess;

		public string animEnd;
	}

	public class WeaponData
	{
		public Weapon.WeaponType type;

		public string name;

		public string modelFileName;

		public string iconFileName;

		public int bulletType;

		public float bulletSpeed;

		public float attackRange;

		public int hitType;

		public NumberSection<float> repelDis;

		public float repelTime;

		public int effectHit;

		public int hitStrength;

		public float[] extra;

		public float emitTimeInAnimation;

		public int deviationStart;

		public float deviationRecoverTime;

		public float deviationDeltaAngle;

		public float deviationMaxAngle;

		public List<WeaponUpgradePhase> upgradePhaseList;

		public float GetDamage(int level, int stars, int levelMax, bool bNext = false)
		{
			int num = 0;
			int num2 = 0;
			if (!bNext)
			{
				num = stars - 1;
				num2 = level - (levelMax - (s_weaponLevelPerPhase - 1));
			}
			else if (level == levelMax)
			{
				num = stars - 1 + 1;
				num2 = level - (levelMax - (s_weaponLevelPerPhase - 1));
			}
			else
			{
				num = stars - 1;
				num2 = level + 1 - (levelMax - (s_weaponLevelPerPhase - 1));
			}
			if (num < 0)
			{
				num = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			WeaponUpgradePhase weaponUpgradePhase = upgradePhaseList[num];
			return weaponUpgradePhase.damage.right + weaponUpgradePhase.damageIncr * (float)num2;
		}

		public int Ammo(int level)
		{
			int index = level / s_weaponLevelPerPhase;
			return upgradePhaseList[index].ammo;
		}

		public float FireFrequence(int level)
		{
			int index = level / s_weaponLevelPerPhase;
			return upgradePhaseList[index].fireFrequence;
		}
	}

	public class WeaponUpgradePhase
	{
		public NumberSection<float> damage;

		public float damageIncr;

		public int ammo;

		public float reloadTime;

		public float fireFrequence;

		public int rapid;

		public float rapidInterval;
	}

	public struct WeaponEvolutionData
	{
		public int money;

		public int stuff;
	}

	public class EnemyData
	{
		public int index;

		public string name;

		public string modelFileName;

		public string iconFileName;

		public int hp;

		public float eliteScale;

		public float bossScale;

		public float moveSpeed;

		public int level;

		public int rank;

		public NumberSection<float> damage;

		public int proCritical;

		public float critDamage;

		public float attackPrequence;

		public int proFirstAttackLose;

		public float repelTime;

		public NumberSection<float> repelDis;

		public int proStun;

		public int proResurge;

		public EnemyDropData dropData;

		public List<SkillInfo> skillInfos;
	}

	public class EnemyDropData
	{
		public ushort probability;

		public ushort eruptProbability;

		public int money;

		public float increase;
	}

	public struct EnemyResurgeImpactData
	{
		public KeyValuePair<int, int> moveSpeed;

		public KeyValuePair<int, int> Dodge;
	}

	private struct EnemyLevelupFormulaData
	{
		private Formula formulaHp;

		private Formula formulaMoveSpeed;

		private Formula formulaDamage;
	}

	public class AnimData
	{
		public string name = string.Empty;

		public int count = 1;
	}

	public class EffectData
	{
		public int index;

		public string fileName;

		public Defined.EffectAnimType animType;

		public float playTime;

		public int bufferNum;
	}

	public class BulletData
	{
		public int index;

		public string fileNmae;

		public bool isModel;

		public int hitType;
	}

	public class HeroData
	{
		public int index;

		public Player.CharacterType characterType;

		public Weapon.WeaponType weaponType;

		public float equipAdditionPercent;

		public string name;

		public string profession;

		public string modelFileName;

		public string iconFileName;

		public int hp;

		public float moveSpeed;

		public int hitRate;

		public ushort proCritical;

		public float critDamage;

		public int dodge;

		public string description;
	}

	public class HeroSkillInfo
	{
		public string name;

		public string fileName;

		public float CDTime;

		public string description;

		public int phase;

		public int phaseLevel;

		public void SetSkillPhase(int level, int star)
		{
			phase = star - 1;
			phase = Mathf.Max(0, phase);
			level--;
			phaseLevel = level - phase * s_skillLevelPerPhase;
		}

		public virtual NumberSection<float> GetATK()
		{
			return null;
		}

		public virtual float GetBR()
		{
			return -9999f;
		}

		public virtual float GetDR()
		{
			return -9999f;
		}

		public virtual float GetTargetHitRateDecrease()
		{
			return -9999f;
		}

		public virtual float GetTargetMoveSpeedDecrease()
		{
			return -9999f;
		}

		public virtual float GetTargetAtkDecrease()
		{
			return -9999f;
		}

		public virtual int GetMINECOUNT()
		{
			return -9999;
		}
	}

	public class SkillDamageUpgrade
	{
		public NumberSection<float> damage;

		public float damageIncr;
	}

	public class SkillMike : HeroSkillInfo
	{
		public NumberSection<float> repelDis;

		public float stunTime;

		public float reduceSpeed;

		public float reduceSpeedTime;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillChris : HeroSkillInfo
	{
		public float dashDistance;

		public float dashTime;

		public NumberSection<float> repelDis;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillLiliUpgrade
	{
		public float healPercent;

		public float healValue;

		public float healValueGrow;

		public float reduceDamage;
	}

	public class SkillLili : HeroSkillInfo
	{
		public float healTime;

		public float healInterval;

		public List<SkillLiliUpgrade> upgradePhaseList;

		public float GetHealPercent()
		{
			SkillLiliUpgrade skillLiliUpgrade = upgradePhaseList[phase];
			return skillLiliUpgrade.healPercent;
		}

		public override float GetBR()
		{
			SkillLiliUpgrade skillLiliUpgrade = upgradePhaseList[phase];
			return skillLiliUpgrade.healValue + skillLiliUpgrade.healValueGrow * (float)phaseLevel;
		}

		public override float GetDR()
		{
			SkillLiliUpgrade skillLiliUpgrade = upgradePhaseList[phase];
			return skillLiliUpgrade.reduceDamage;
		}
	}

	public class SkillVasily : HeroSkillInfo
	{
		public int bulletType;

		public float speed;

		public float range;

		public int hitType;

		public int effectHitType;

		public float fireFrequence;

		public float lifeTime;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillClaire : HeroSkillInfo
	{
		public int missileCount;

		public float missileAngle;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillFireDragon : SkillChris
	{
	}

	public class SkillZero : HeroSkillInfo
	{
		public float summonTime;

		public float damageInterval;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillArnoud : HeroSkillInfo
	{
		public float damageRadius;

		public float speed;

		public float range;

		public float fireFrequence;

		public float lifeTime;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillXJohnX : HeroSkillInfo
	{
		public float range;

		public float laserInterval;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillClint : HeroSkillInfo
	{
		public float keepTime;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillEvaUpgrade
	{
		public float debuffHitRate;

		public float debuffMoveSpeed;

		public float debuffAtk;

		public float debuffGrowth;
	}

	public class SkillEva : HeroSkillInfo
	{
		public float debuffTime;

		public List<SkillEvaUpgrade> upgradePhaseList;

		private float GetDebuffGrowth()
		{
			SkillEvaUpgrade skillEvaUpgrade = upgradePhaseList[phase];
			return skillEvaUpgrade.debuffGrowth * (float)phaseLevel;
		}

		public override float GetTargetHitRateDecrease()
		{
			SkillEvaUpgrade skillEvaUpgrade = upgradePhaseList[phase];
			return 0f - skillEvaUpgrade.debuffHitRate - GetDebuffGrowth();
		}

		public override float GetTargetMoveSpeedDecrease()
		{
			SkillEvaUpgrade skillEvaUpgrade = upgradePhaseList[phase];
			return 0f - skillEvaUpgrade.debuffMoveSpeed - GetDebuffGrowth();
		}

		public override float GetTargetAtkDecrease()
		{
			SkillEvaUpgrade skillEvaUpgrade = upgradePhaseList[phase];
			return 0f - skillEvaUpgrade.debuffAtk - GetDebuffGrowth();
		}
	}

	public class SkillJasonUpgrade : SkillDamageUpgrade
	{
		public int mineCount;
	}

	public class SkillJason : HeroSkillInfo
	{
		public float setRadius;

		public float explodeRadius;

		public List<SkillJasonUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillJasonUpgrade skillJasonUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillJasonUpgrade.damage.left + skillJasonUpgrade.damageIncr * (float)phaseLevel, skillJasonUpgrade.damage.right + skillJasonUpgrade.damageIncr * (float)phaseLevel);
		}

		public override int GetMINECOUNT()
		{
			SkillJasonUpgrade skillJasonUpgrade = upgradePhaseList[phase];
			return skillJasonUpgrade.mineCount;
		}
	}

	public class SkillTanyaUpgrade : SkillDamageUpgrade
	{
		public int fireCount;
	}

	public class SkillTanya : HeroSkillInfo
	{
		public float damageRadius;

		public NumberSection<float> repelDis;

		public List<SkillTanyaUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillBourne : HeroSkillInfo
	{
		public float shootTime;

		public int damageCount;

		public float damageRadius;

		public NumberSection<float> repelDis;

		public float debuffMoveSpeed;

		public float debuffTime;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillRock : SkillChris
	{
	}

	public class SkillWesker : HeroSkillInfo
	{
		public float damageRadius;

		public NumberSection<float> repelDis;

		public float limitTime;

		public float moveSpeed;

		public int count;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillOppenheimerUpgrade : SkillDamageUpgrade
	{
		public float novaPlus;
	}

	public class SkillOppenheimer : HeroSkillInfo
	{
		public float frozeTime;

		public List<SkillOppenheimerUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class SkillShepard : HeroSkillInfo
	{
		public float speed;

		public float radius;

		public float time;

		public float damageInterval;

		public List<SkillDamageUpgrade> upgradePhaseList;

		public override NumberSection<float> GetATK()
		{
			SkillDamageUpgrade skillDamageUpgrade = upgradePhaseList[phase];
			return new NumberSection<float>(skillDamageUpgrade.damage.left + skillDamageUpgrade.damageIncr * (float)phaseLevel, skillDamageUpgrade.damage.right + skillDamageUpgrade.damageIncr * (float)phaseLevel);
		}
	}

	public class StuffData
	{
		public int index;

		public Defined.STUFF_TYPE stuffType;

		public string name;

		public string fileName;

		public Defined.RANK_TYPE rank;

		public int cost;

		public string description;

		public bool Useable
		{
			get
			{
				return stuffType == Defined.STUFF_TYPE.Box;
			}
		}
	}

	public class BoxData : StuffData
	{
		public Defined.BOX_RANK boxRank;

		public int needKey;

		public List<BoxItemInfo> boxItemInfo;

		public BoxItemInfo systemEncourage;
	}

	public class ExpTomeData : StuffData
	{
		public int value;

		public Defined.CalType calType;
	}

	public class BoxItemInfo
	{
		public string type;

		public int[] index;

		public int count;

		public NumberSection<int> probability;
	}

	public class EquipData
	{
		public int index;

		public string name;

		public string fileName;

		public Defined.EQUIP_TYPE equipType;

		public Defined.RANK_TYPE rank;

		public int[] hp;

		public float def;

		public ushort proCrit;

		public float critDamagePercent;

		public ushort proResilience;

		public ushort proHit;

		public float hpPercent;

		public ushort proDodge;

		public ushort proStab;

		public float atkPercent;

		public float atkFrequencyPercent;

		public float reduceDamagePercent;

		public float moveSpeedPercent;

		public float atkRange;

		public ushort specialAttr;

		public string description;
	}

	public struct RewardStuffData
	{
		public ushort index;

		public ushort count;
	}

	public struct GameLevelDialogData
	{
		public int playerID;

		public string dialog;
	}

	public class GameLevelData
	{
		public ushort index;

		public string id;

		public string name = string.Empty;

		public int depth = -1;

		public Vector2 mapNodePos;

		public Defined.LevelMode mode;

		public int level;

		public Defined.BattleMode battleMode;

		public string sBGM;

		public string title;

		public string description;

		public List<GameLevelDialogData> dialogStart;

		public List<GameLevelDialogData> dialogEnd;

		public bool isBossLevel
		{
			get
			{
				return battleMode == Defined.BattleMode.Boss;
			}
		}

		public bool isEncounter
		{
			get
			{
				return battleMode == Defined.BattleMode.Encounter;
			}
		}
	}

	public class GameLevelEncounterModeData : GameLevelData
	{
		public string enenyTeamID;
	}

	public class GameLevelNodeData
	{
		public ushort index;

		public string gameLevelID;

		public string name;

		public string texIcon;

		public string iconName;

		public Vector2 mapNodePos;
	}

	public class HeroRefreshItemData
	{
		public Defined.RANK_TYPE rank;

		public int[] index;

		public NumberSection<int> probability;
	}

	public class HeroRefreshData
	{
		public Defined.COST_TYPE costType;

		public int cost;

		public int limitTimeOneDay;

		public Dictionary<Defined.RANK_TYPE, HeroRefreshItemData> refreshItemData;
	}

	public class StoreItemData
	{
		public Defined.ITEM_TYPE itemType;

		public int index;

		public Defined.COST_TYPE costType;

		public int cost;

		public int limitCountOneDay;
	}

	public SpecialAttribute.SpecialAttributeReinforcement m_specialAttributeReinforcement;

	public SpecialAttribute.SpecialAttributeMania m_specialAttributeMania;

	public SpecialAttribute.SpecialAttributeRapid m_specialAttributeRapid;

	public SpecialAttribute.SpecialAttributeDodge m_specialAttributeDodge;

	public SpecialAttribute.SpecialAttributeDecay m_specialAttributeDecay;

	public SpecialAttribute.SpecialAttributeHarden m_specialAttributeHarden;

	public SpecialAttribute.SpecialAttributeRifrigerate m_specialAttributeRifrigerate;

	public SpecialAttribute.SpecialAttributeElectric m_specialAttributeElectric;

	public SpecialAttribute.SpecialAttributeVariation m_specialAttributeVariation;

	public SpecialAttribute.SpecialAttributeHellfire m_specialAttributeHellfire;

	public SpecialAttribute.SpecialAttributeStrengthen m_specialAttributeStrengthen;

	public SpecialAttribute.SpecialAttributeEndure m_specialAttributeEndure;

	public SpecialAttribute.SpecialAttributeDeadSpawnBomb m_specialAttributeDeadSpawnBomb;

	public SpecialAttribute.SpecialAttributeBloodsucking m_specialAttributeBloodsucking;

	public SpecialAttribute.SpecialAttributeRangeWeaken m_specialAttributeRangeWeaken;

	public SpecialAttribute.SpecialAttributeRangeTerritory m_specialAttributeRangeTerritory;

	public SpecialAttribute.SpecialAttributeStrickBack m_specialAttributeStrickBack;

	public SpecialAttribute.SpecialAttributeInvalidSkill m_specialAttributeInvalidSkill;

	public SpecialAttribute.SpecialAttributePoisonClaw m_specialAttributePoisonClaw;

	public SpecialAttribute.SpecialAttributeExacerbate m_specialAttributeExacerbate;

	public SpecialAttribute.SpecialAttributeKnell m_specialAttributeKnell;

	public SpecialAttribute.SpecialAttributeElegy m_specialAttributeElegy;

	public SpecialAttribute.SpecialAttributeVampire m_specialAttributeVampire;

	public SpecialAttribute.SpecialAttributeGrind m_specialAttributeGrind;

	public SpecialAttribute.SpecialAttributeSplit m_specialAttributeSplit;

	public SpecialAttribute.SpecialAttributeTornado m_specialAttributeTornado;

	public SpecialAttribute.SpecialAttributeGod m_specialAttributeGod;

	public SpecialAttribute.SpecialAttributePusBlood m_specialAttributePusBlood;

	public SpecialAttribute.SpecialAttributeBomb m_specialAttributeBomb;

	public SpecialAttribute.SpecialAttributeLiquidNitrogen m_specialAttributeLiquidNitrogen;

	public SpecialAttribute.SpecialAttributeSummon m_specialAttributeSummon;

	public SpecialAttribute.SpecialAttributeLifeLink m_specialAttributeLifeLink;

	private Dictionary<SpecialAttribute.AttributeType, SpecialAttribute.SpecialAttributeData> m_specialAttributeData;

	private List<SpecialEffectData> m_specialEffectData;

	public TeamSpecialAttribute.TeamAttributeReinforcement m_teamAttributeReinforcement;

	public TeamSpecialAttribute.TeamAttributeUrgentTreatment m_teamAttributeUrgentTreatment;

	public TeamSpecialAttribute.TeamAttributeArmsmaster m_teamAttributeArmsmaster;

	public TeamSpecialAttribute.TeamAttributeSpecialTraining m_teamAttributeSpecialTraining;

	public TeamSpecialAttribute.TeamAttributeBiobomb m_teamAttributeBiobomb;

	public TeamSpecialAttribute.TeamAttributeLinkedLives m_teamAttributeLinkedLives;

	public TeamSpecialAttribute.TeamAttributeVolatileBomb m_teamAttributeVolatileBomb;

	public TeamSpecialAttribute.TeamAttributeMania m_teamAttributeMania;

	public TeamSpecialAttribute.TeamAttributeLivingCells m_teamAttributeLivingCells;

	public TeamSpecialAttribute.TeamAttributeForceField m_teamAttributeForceField;

	public TeamSpecialAttribute.TeamAttributePrayer m_teamAttributePrayer;

	public TeamSpecialAttribute.TeamAttributeStun m_teamAttributeStun;

	public TeamSpecialAttribute.TeamAttributeNanotech m_teamAttributeNanotech;

	public TeamSpecialAttribute.TeamAttributeSealingTechnique m_teamAttributeSealingTechnique;

	public TeamSpecialAttribute.TeamAttributeProlongedFirepower m_teamAttributeProlongedFirepower;

	public TeamSpecialAttribute.TeamAttributeFreeze m_teamAttributeFreeze;

	public TeamSpecialAttribute.TeamAttributeAimedShot m_teamAttributeAimedShot;

	public TeamSpecialAttribute.TeamAttributeShadowWalk m_teamAttributeShadowWalk;

	public TeamSpecialAttribute.TeamAttributeSplash m_teamAttributeSplash;

	public TeamSpecialAttribute.TeamAttributeResolve m_teamAttributeResolve;

	public TeamSpecialAttribute.TeamAttributeCooling m_teamAttributeCooling;

	public TeamSpecialAttribute.TeamAttributeRelentlessRage m_teamAttributeRelentlessRage;

	public TeamSpecialAttribute.TeamAttributeVampire m_teamAttributeVampire;

	public TeamSpecialAttribute.TeamAttributeResurrection m_teamAttributeResurrection;

	public TeamSpecialAttribute.TeamAttributeExecutioner m_teamAttributeExecutioner;

	private Dictionary<TeamSpecialAttribute.TeamAttributeType, TeamSpecialAttribute.TeamAttributeData> m_teamAttributeData;

	private Dictionary<TeamSpecialAttribute.TeamAttributeEvolveType, TeamSpecialAttribute.TeamAttributeEvolveData> m_teamAttributeEvolve;

	public static int s_weaponLevelPerPhase;

	private List<string> m_weaponList;

	private Dictionary<Weapon.WeaponType, WeaponData> m_weaponData;

	public static NumberSection<float> s_enemySkillCdTime;

	public static int s_enemyRandomSkillInterval;

	public static float s_enemyRandomSkillOdds;

	private Dictionary<Enemy.EnemyType, EnemyData> m_enemyData;

	private Dictionary<string, AnimData> m_characterAnimMap;

	private Dictionary<string, Dictionary<string, AnimData>> m_newCharacterAnimMap;

	private Dictionary<string, Dictionary<string, AnimData>> m_enemyAnimMap;

	private Dictionary<string, Dictionary<string, string>> m_weaponAnimMap;

	private List<EffectData> m_effectData;

	private List<BulletData> m_bulletData;

	private Dictionary<int, HeroData> m_heroData;

	public static int s_skillLevelPerPhase;

	public Dictionary<Player.CharacterType, HeroSkillInfo> m_characterSkills;

	public int radarIndex = -1;

	private Dictionary<int, StuffData> m_stuffData;

	private Dictionary<int, EquipData> m_equipData;

	private List<GameLevelNodeData> m_gameLevelNodes;

	private Dictionary<Defined.LevelMode, List<GameLevelData>> m_selectGameLevelNode;

	private GameLevelData m_selectedGameLevelData;

	private Dictionary<Defined.HERO_REFRESH_TYPE, HeroRefreshData> m_heroRefreshData;

	private Dictionary<Defined.STORE_ITEM_TYPE, List<StoreItemData>> m_storeData;

	private List<StoreItemData> m_storeExchangeData;

	private List<string> m_loadingTips;

    private Dictionary<string, AchievementData> m_achievementDataDict = new Dictionary<string, AchievementData>();

    public Dictionary<string, AchievementData> GetAchievementDataMap()
    {
        return m_achievementDataDict;
    }

    public DataConf()
	{
		LoadConf();
	}

	public SpecialAttribute.SpecialAttributeData GetSpecialAttributeData(SpecialAttribute.AttributeType type)
	{
		if (m_specialAttributeData.ContainsKey(type))
		{
			return m_specialAttributeData[type];
		}
		return null;
	}

    public void LoadAchievementsFromDisk()
    {
        string text = FileUtil.LoadResourcesFile("Configs/Achievements");
        if (!string.IsNullOrEmpty(text))
        {
            LoadAchievements(text);
        }
    }

    public void LoadAchievements(string text)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(text);
        XmlElement documentElement = xmlDocument.DocumentElement;

        if (m_achievementDataDict == null)
        {
            m_achievementDataDict = new Dictionary<string, AchievementData>();
        }
        m_achievementDataDict.Clear();

        int indexCounter = 0;
        foreach (XmlElement element in documentElement.GetElementsByTagName("Quest"))
        {
            AchievementData achievementData = new AchievementData();
            achievementData.id = element.GetAttribute("questId");
            achievementData.site = indexCounter++;
            achievementData.title = element.GetAttribute("name");
            achievementData.des = element.GetAttribute("desc");
            achievementData.money = int.Parse(element.GetAttribute("money"));
            achievementData.crystal = int.Parse(element.GetAttribute("crystal"));
            achievementData.honor = int.Parse(element.GetAttribute("honor"));
            achievementData.hero = int.Parse(element.GetAttribute("hero"));
            achievementData.state = int.Parse(element.GetAttribute("state"));
            achievementData.scheduleMin = int.Parse(element.GetAttribute("process"));
            achievementData.scheduleMax = int.Parse(element.GetAttribute("maxProcess"));
            achievementData.bDaily = int.Parse(element.GetAttribute("daily")) != 0;
            achievementData.counter = element.GetAttribute("counter");
            achievementData.counterArg = element.GetAttribute("counterArg");

            m_achievementDataDict.Add(achievementData.id, achievementData);
        }
    }

    public void LoadSpecialAttribteDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/SpecialAttribute");
		if (text != null && text.Length > 0)
		{
			LoadSpecialAttributeData(text);
		}
	}

	public void LoadSpecialAttributeData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_specialAttributeData = new Dictionary<SpecialAttribute.AttributeType, SpecialAttribute.SpecialAttributeData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Attribute"))
		{
			SpecialAttribute.AttributeType attributeType = (SpecialAttribute.AttributeType)int.Parse(item.GetAttribute("index"));
			string attribute = item.GetAttribute("name");
			string attribute2 = item.GetAttribute("effectFileName");
			string attribute3 = item.GetAttribute("description");
			List<SpecialAttribute.SpecialAttributeEffectData> list = new List<SpecialAttribute.SpecialAttributeEffectData>();
			foreach (XmlElement item2 in item.GetElementsByTagName("SpecialEffect"))
			{
				SpecialAttribute.SpecialAttributeEffectData specialAttributeEffectData = new SpecialAttribute.SpecialAttributeEffectData();
				specialAttributeEffectData.type = (SpecialAttribute.SpecialAttributeEffectType)int.Parse(item2.GetAttribute("type"));
				specialAttributeEffectData.bufferCount = int.Parse(item2.GetAttribute("bufferCount"));
				list.Add(specialAttributeEffectData);
			}
			SpecialAttribute.SpecialAttributeData specialAttributeData = null;
			switch (attributeType)
			{
			case SpecialAttribute.AttributeType.Reinforcement:
				specialAttributeData = new SpecialAttribute.SpecialAttributeReinforcement();
				m_specialAttributeReinforcement = (SpecialAttribute.SpecialAttributeReinforcement)specialAttributeData;
				m_specialAttributeReinforcement.damage = float.Parse(item.GetAttribute("damage"));
				break;
			case SpecialAttribute.AttributeType.Mania:
				specialAttributeData = new SpecialAttribute.SpecialAttributeMania();
				m_specialAttributeMania = (SpecialAttribute.SpecialAttributeMania)specialAttributeData;
				m_specialAttributeMania.proCrit = ushort.Parse(item.GetAttribute("proCrit"));
				m_specialAttributeMania.critDamage = float.Parse(item.GetAttribute("critDamage"));
				break;
			case SpecialAttribute.AttributeType.Rapid:
				specialAttributeData = new SpecialAttribute.SpecialAttributeRapid();
				m_specialAttributeRapid = (SpecialAttribute.SpecialAttributeRapid)specialAttributeData;
				m_specialAttributeRapid.attFrequency = float.Parse(item.GetAttribute("attFrequency"));
				m_specialAttributeRapid.addMoveSpeedPercent = float.Parse(item.GetAttribute("addMoveSpeedPercent"));
				break;
			case SpecialAttribute.AttributeType.Dodge:
				specialAttributeData = new SpecialAttribute.SpecialAttributeDodge();
				m_specialAttributeDodge = (SpecialAttribute.SpecialAttributeDodge)specialAttributeData;
				m_specialAttributeDodge.proDodge = ushort.Parse(item.GetAttribute("proDodge"));
				break;
			case SpecialAttribute.AttributeType.Decay:
				specialAttributeData = new SpecialAttribute.SpecialAttributeDecay();
				m_specialAttributeDecay = (SpecialAttribute.SpecialAttributeDecay)specialAttributeData;
				m_specialAttributeDecay.reduceHpPercent = float.Parse(item.GetAttribute("reduceHpPercent"));
				if (m_specialAttributeDecay.reduceHpPercent > 0f)
				{
					m_specialAttributeDecay.reduceHpPercent *= -1f;
				}
				m_specialAttributeDecay.frequency = float.Parse(item.GetAttribute("frequency"));
				break;
			case SpecialAttribute.AttributeType.Harden:
				specialAttributeData = new SpecialAttribute.SpecialAttributeHarden();
				m_specialAttributeHarden = (SpecialAttribute.SpecialAttributeHarden)specialAttributeData;
				m_specialAttributeHarden.reduceDamage = float.Parse(item.GetAttribute("reduceDamage"));
				break;
			case SpecialAttribute.AttributeType.Rifrigerate:
				specialAttributeData = new SpecialAttribute.SpecialAttributeRifrigerate();
				m_specialAttributeRifrigerate = (SpecialAttribute.SpecialAttributeRifrigerate)specialAttributeData;
				m_specialAttributeRifrigerate.reduceSpeed = float.Parse(item.GetAttribute("reduceSpeed"));
				m_specialAttributeRifrigerate.probability = ushort.Parse(item.GetAttribute("probability"));
				if (m_specialAttributeRifrigerate.reduceSpeed > 0f)
				{
					m_specialAttributeRifrigerate.reduceSpeed *= -1f;
				}
				m_specialAttributeRifrigerate.frozenTime = float.Parse(item.GetAttribute("frozenTime"));
				break;
			case SpecialAttribute.AttributeType.Electric:
				specialAttributeData = new SpecialAttribute.SpecialAttributeElectric();
				m_specialAttributeElectric = (SpecialAttribute.SpecialAttributeElectric)specialAttributeData;
				m_specialAttributeElectric.probability = ushort.Parse(item.GetAttribute("probability"));
				m_specialAttributeElectric.reduceDamage = float.Parse(item.GetAttribute("reduceDamage"));
				break;
			case SpecialAttribute.AttributeType.Variation:
				specialAttributeData = new SpecialAttribute.SpecialAttributeVariation();
				m_specialAttributeVariation = (SpecialAttribute.SpecialAttributeVariation)specialAttributeData;
				m_specialAttributeVariation.damage = float.Parse(item.GetAttribute("damage"));
				m_specialAttributeVariation.attFrequency = float.Parse(item.GetAttribute("attFrequency"));
				m_specialAttributeVariation.addMoveSpeedPercent = float.Parse(item.GetAttribute("addMoveSpeedPercent"));
				break;
			case SpecialAttribute.AttributeType.Hellfire:
				specialAttributeData = new SpecialAttribute.SpecialAttributeHellfire();
				m_specialAttributeHellfire = (SpecialAttribute.SpecialAttributeHellfire)specialAttributeData;
				m_specialAttributeHellfire.frequency = float.Parse(item.GetAttribute("frequency"));
				break;
			case SpecialAttribute.AttributeType.Strengthen:
				specialAttributeData = new SpecialAttribute.SpecialAttributeStrengthen();
				m_specialAttributeStrengthen = (SpecialAttribute.SpecialAttributeStrengthen)specialAttributeData;
				m_specialAttributeStrengthen.hpMax = float.Parse(item.GetAttribute("hpMax"));
				break;
			case SpecialAttribute.AttributeType.Endure:
				specialAttributeData = new SpecialAttribute.SpecialAttributeEndure();
				m_specialAttributeEndure = (SpecialAttribute.SpecialAttributeEndure)specialAttributeData;
				m_specialAttributeEndure.probability = ushort.Parse(item.GetAttribute("probability"));
				m_specialAttributeEndure.duration = float.Parse(item.GetAttribute("duration"));
				break;
			case SpecialAttribute.AttributeType.DeadSpawnBomb:
				specialAttributeData = new SpecialAttribute.SpecialAttributeDeadSpawnBomb();
				m_specialAttributeDeadSpawnBomb = (SpecialAttribute.SpecialAttributeDeadSpawnBomb)specialAttributeData;
				m_specialAttributeDeadSpawnBomb.probability = ushort.Parse(item.GetAttribute("probability"));
				break;
			case SpecialAttribute.AttributeType.Bloodsucking:
				specialAttributeData = new SpecialAttribute.SpecialAttributeBloodsucking();
				m_specialAttributeBloodsucking = (SpecialAttribute.SpecialAttributeBloodsucking)specialAttributeData;
				m_specialAttributeBloodsucking.addSpeedPercent = float.Parse(item.GetAttribute("addSpeedPercent"));
				m_specialAttributeBloodsucking.damage = float.Parse(item.GetAttribute("damage"));
				m_specialAttributeBloodsucking.duration = float.Parse(item.GetAttribute("duration"));
				m_specialAttributeBloodsucking.accumulate = ushort.Parse(item.GetAttribute("accumulate"));
				break;
			case SpecialAttribute.AttributeType.RangeWeaken:
				specialAttributeData = new SpecialAttribute.SpecialAttributeRangeWeaken();
				m_specialAttributeRangeWeaken = (SpecialAttribute.SpecialAttributeRangeWeaken)specialAttributeData;
				m_specialAttributeRangeWeaken.reduceDamage = float.Parse(item.GetAttribute("reduceDamage"));
				break;
			case SpecialAttribute.AttributeType.Territory:
				specialAttributeData = new SpecialAttribute.SpecialAttributeRangeTerritory();
				m_specialAttributeRangeTerritory = (SpecialAttribute.SpecialAttributeRangeTerritory)specialAttributeData;
				m_specialAttributeRangeTerritory.critDamage = float.Parse(item.GetAttribute("critDamage"));
				m_specialAttributeRangeTerritory.normalDamage = float.Parse(item.GetAttribute("normalDamage"));
				break;
			case SpecialAttribute.AttributeType.StrickBack:
				specialAttributeData = new SpecialAttribute.SpecialAttributeStrickBack();
				m_specialAttributeStrickBack = (SpecialAttribute.SpecialAttributeStrickBack)specialAttributeData;
				m_specialAttributeStrickBack.bombDamage = float.Parse(item.GetAttribute("bombDamage"));
				m_specialAttributeStrickBack.frozenTime = float.Parse(item.GetAttribute("frozenTime"));
				m_specialAttributeStrickBack.probability = ushort.Parse(item.GetAttribute("probability"));
				break;
			case SpecialAttribute.AttributeType.InvalidSkill:
				specialAttributeData = new SpecialAttribute.SpecialAttributeInvalidSkill();
				m_specialAttributeInvalidSkill = (SpecialAttribute.SpecialAttributeInvalidSkill)specialAttributeData;
				break;
			case SpecialAttribute.AttributeType.PoisonClaw:
			{
				specialAttributeData = new SpecialAttribute.SpecialAttributePoisonClaw();
				m_specialAttributePoisonClaw = (SpecialAttribute.SpecialAttributePoisonClaw)specialAttributeData;
				m_specialAttributePoisonClaw.probability = ushort.Parse(item.GetAttribute("probability"));
				m_specialAttributePoisonClaw.duration = float.Parse(item.GetAttribute("duration"));
				string attribute4 = item.GetAttribute("damage");
				string[] array = attribute4.Split('-');
				m_specialAttributePoisonClaw.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
				break;
			}
			case SpecialAttribute.AttributeType.Exacerbate:
			{
				specialAttributeData = new SpecialAttribute.SpecialAttributeExacerbate();
				m_specialAttributeExacerbate = (SpecialAttribute.SpecialAttributeExacerbate)specialAttributeData;
				m_specialAttributeExacerbate.probability = ushort.Parse(item.GetAttribute("probability"));
				m_specialAttributeExacerbate.duration = float.Parse(item.GetAttribute("duration"));
				string attribute4 = item.GetAttribute("damage");
				string[] array = attribute4.Split('-');
				m_specialAttributeExacerbate.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
				break;
			}
			case SpecialAttribute.AttributeType.Knell:
				specialAttributeData = new SpecialAttribute.SpecialAttributeKnell();
				m_specialAttributeKnell = (SpecialAttribute.SpecialAttributeKnell)specialAttributeData;
				m_specialAttributeKnell.probability = ushort.Parse(item.GetAttribute("probability"));
				break;
			case SpecialAttribute.AttributeType.Elegy:
				specialAttributeData = new SpecialAttribute.SpecialAttributeElegy();
				m_specialAttributeElegy = (SpecialAttribute.SpecialAttributeElegy)specialAttributeData;
				m_specialAttributeElegy.frequency = float.Parse(item.GetAttribute("frequency"));
				m_specialAttributeElegy.hp = float.Parse(item.GetAttribute("hp"));
				break;
			case SpecialAttribute.AttributeType.Vampire:
				specialAttributeData = new SpecialAttribute.SpecialAttributeVampire();
				m_specialAttributeVampire = (SpecialAttribute.SpecialAttributeVampire)specialAttributeData;
				m_specialAttributeVampire.description = attribute3;
				break;
			case SpecialAttribute.AttributeType.Grind:
				specialAttributeData = new SpecialAttribute.SpecialAttributeGrind();
				m_specialAttributeGrind = (SpecialAttribute.SpecialAttributeGrind)specialAttributeData;
				m_specialAttributeGrind.reduceAtt = float.Parse(item.GetAttribute("reduceAtt"));
				m_specialAttributeGrind.reduceMoveSpeed = float.Parse(item.GetAttribute("reduceMoveSpeed"));
				if (m_specialAttributeGrind.reduceMoveSpeed > 0f)
				{
					m_specialAttributeGrind.reduceMoveSpeed *= -1f;
				}
				m_specialAttributeGrind.duration = float.Parse(item.GetAttribute("duration"));
				break;
			case SpecialAttribute.AttributeType.Split:
				specialAttributeData = new SpecialAttribute.SpecialAttributeSplit();
				m_specialAttributeSplit = (SpecialAttribute.SpecialAttributeSplit)specialAttributeData;
				m_specialAttributeSplit.hp = float.Parse(item.GetAttribute("hp"));
				m_specialAttributeSplit.newHp = float.Parse(item.GetAttribute("newHp"));
				break;
			case SpecialAttribute.AttributeType.Tornado:
			{
				specialAttributeData = new SpecialAttribute.SpecialAttributeTornado();
				m_specialAttributeTornado = (SpecialAttribute.SpecialAttributeTornado)specialAttributeData;
				m_specialAttributeTornado.attFrequency = float.Parse(item.GetAttribute("attFrequency"));
				string attribute4 = item.GetAttribute("damage");
				string[] array = attribute4.Split('-');
				m_specialAttributeTornado.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
				break;
			}
			case SpecialAttribute.AttributeType.God:
				specialAttributeData = new SpecialAttribute.SpecialAttributeGod();
				m_specialAttributeGod = (SpecialAttribute.SpecialAttributeGod)specialAttributeData;
				m_specialAttributeGod.frequency = float.Parse(item.GetAttribute("frequency"));
				m_specialAttributeGod.duration = float.Parse(item.GetAttribute("duration"));
				break;
			case SpecialAttribute.AttributeType.PusBlood:
				specialAttributeData = new SpecialAttribute.SpecialAttributePusBlood();
				m_specialAttributePusBlood = (SpecialAttribute.SpecialAttributePusBlood)specialAttributeData;
				m_specialAttributePusBlood.frequency = float.Parse(item.GetAttribute("frequency"));
				m_specialAttributePusBlood.damage = float.Parse(item.GetAttribute("damage"));
				m_specialAttributePusBlood.poisonDuration = float.Parse(item.GetAttribute("poisonDuration"));
				m_specialAttributePusBlood.poisonFrequency = float.Parse(item.GetAttribute("poisonFrequency"));
				break;
			case SpecialAttribute.AttributeType.Bomb:
				specialAttributeData = new SpecialAttribute.SpecialAttributeBomb();
				m_specialAttributeBomb = (SpecialAttribute.SpecialAttributeBomb)specialAttributeData;
				m_specialAttributeBomb.frequency = float.Parse(item.GetAttribute("frequency"));
				m_specialAttributeBomb.damage = float.Parse(item.GetAttribute("damage"));
				break;
			case SpecialAttribute.AttributeType.LiquidNitrogen:
				specialAttributeData = new SpecialAttribute.SpecialAttributeLiquidNitrogen();
				m_specialAttributeLiquidNitrogen = (SpecialAttribute.SpecialAttributeLiquidNitrogen)specialAttributeData;
				m_specialAttributeLiquidNitrogen.frequency = float.Parse(item.GetAttribute("frequency"));
				m_specialAttributeLiquidNitrogen.damage = float.Parse(item.GetAttribute("damage"));
				m_specialAttributeLiquidNitrogen.frozenTime = float.Parse(item.GetAttribute("frozenTime"));
				break;
			case SpecialAttribute.AttributeType.Summon:
				specialAttributeData = new SpecialAttribute.SpecialAttributeSummon();
				m_specialAttributeSummon = (SpecialAttribute.SpecialAttributeSummon)specialAttributeData;
				m_specialAttributeSummon.frequency = float.Parse(item.GetAttribute("frequency"));
				m_specialAttributeSummon.zombie = ushort.Parse(item.GetAttribute("zombie"));
				m_specialAttributeSummon.zombieBomb = ushort.Parse(item.GetAttribute("zombieBomb"));
				m_specialAttributeSummon.addSpeedPercent = float.Parse(item.GetAttribute("addSpeedPercent"));
				break;
			case SpecialAttribute.AttributeType.LifeLink:
				specialAttributeData = new SpecialAttribute.SpecialAttributeLifeLink();
				m_specialAttributeLifeLink = (SpecialAttribute.SpecialAttributeLifeLink)specialAttributeData;
				break;
			}
			if (specialAttributeData != null)
			{
				specialAttributeData.name = attribute;
				specialAttributeData.effects = null;
				specialAttributeData.effects = list;
				specialAttributeData.description = attribute3;
				specialAttributeData.type = attributeType;
				m_specialAttributeData.Add(specialAttributeData.type, specialAttributeData);
			}
		}
	}

	public List<SpecialEffectData> GetSpecialEffectsData()
	{
		return m_specialEffectData;
	}

	public SpecialEffectData GetSpecialEffectDataByIndex(int index)
	{
		if (index < 0 || index >= m_specialEffectData.Count)
		{
			return null;
		}
		return m_specialEffectData[index];
	}

	public void LoadSpecailEffectDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/SpecialAttributeEffect");
		LoadSpecialEffectData(text);
	}

	private void LoadSpecialEffectData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_specialEffectData = new List<SpecialEffectData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Effect"))
		{
			SpecialEffectData specialEffectData = new SpecialEffectData();
			specialEffectData.index = int.Parse(item.GetAttribute("index"));
			specialEffectData.fileName = item.GetAttribute("fileName");
			specialEffectData.animType = (Defined.EffectAnimType)int.Parse(item.GetAttribute("animType"));
			specialEffectData.playTime = float.Parse(item.GetAttribute("playTime"));
			m_specialEffectData.Add(specialEffectData);
		}
	}

	public TeamSpecialAttribute.TeamAttributeData GetTeamAttributeData(TeamSpecialAttribute.TeamAttributeType type)
	{
		if (m_teamAttributeData.ContainsKey(type))
		{
			return m_teamAttributeData[type];
		}
		return null;
	}

	public TeamSpecialAttribute.TeamAttributeEvolveData GetTeamAttributeEvolveData(TeamSpecialAttribute.TeamAttributeEvolveType type)
	{
		if (m_teamAttributeEvolve.ContainsKey(type))
		{
			return m_teamAttributeEvolve[type];
		}
		return null;
	}

	public void LoadTeamAttribteDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/TeamSpecialAttribute");
		if (text != null && text.Length > 0)
		{
			LoadTeamAttributeData(text);
		}
	}

	public void LoadTeamAttributeData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("Talent").Item(0);
		m_teamAttributeData = new Dictionary<TeamSpecialAttribute.TeamAttributeType, TeamSpecialAttribute.TeamAttributeData>();
		foreach (XmlElement item in xmlElement.GetElementsByTagName("Attribute"))
		{
			TeamSpecialAttribute.TeamAttributeType teamAttributeType = (TeamSpecialAttribute.TeamAttributeType)int.Parse(item.GetAttribute("index"));
			string attribute = item.GetAttribute("name");
			string attribute2 = item.GetAttribute("iconFileName");
			string attribute3 = item.GetAttribute("description");
			List<SpecialAttribute.SpecialAttributeEffectData> list = new List<SpecialAttribute.SpecialAttributeEffectData>();
			foreach (XmlElement item2 in item.GetElementsByTagName("SpecialEffect"))
			{
				SpecialAttribute.SpecialAttributeEffectData specialAttributeEffectData = new SpecialAttribute.SpecialAttributeEffectData();
				specialAttributeEffectData.type = (SpecialAttribute.SpecialAttributeEffectType)int.Parse(item2.GetAttribute("type"));
				specialAttributeEffectData.bufferCount = int.Parse(item2.GetAttribute("bufferCount"));
				list.Add(specialAttributeEffectData);
			}
			TeamSpecialAttribute.TeamAttributeData teamAttributeData = null;
			switch (teamAttributeType)
			{
			case TeamSpecialAttribute.TeamAttributeType.Reinforcement:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeReinforcement();
				m_teamAttributeReinforcement = (TeamSpecialAttribute.TeamAttributeReinforcement)teamAttributeData;
				string attribute4 = item.GetAttribute("damage");
				string[] array = attribute4.Split(',');
				m_teamAttributeReinforcement.damage = new float[array.Length];
				for (int num2 = m_teamAttributeReinforcement.damage.Length - 1; num2 >= 0; num2--)
				{
					m_teamAttributeReinforcement.damage[num2] = float.Parse(array[num2]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.UrgentTreatment:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeUrgentTreatment();
				m_teamAttributeUrgentTreatment = (TeamSpecialAttribute.TeamAttributeUrgentTreatment)teamAttributeData;
				string attribute4 = item.GetAttribute("hpPercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeUrgentTreatment.hpPercent = new float[array.Length];
				for (int num9 = m_teamAttributeUrgentTreatment.hpPercent.Length - 1; num9 >= 0; num9--)
				{
					m_teamAttributeUrgentTreatment.hpPercent[num9] = float.Parse(array[num9]);
				}
				m_teamAttributeUrgentTreatment.frequency = float.Parse(item.GetAttribute("frequency"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Armsmaster:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeArmsmaster();
				m_teamAttributeArmsmaster = (TeamSpecialAttribute.TeamAttributeArmsmaster)teamAttributeData;
				string attribute4 = item.GetAttribute("clipPercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeArmsmaster.clipPercent = new float[array.Length];
				for (int num19 = m_teamAttributeArmsmaster.clipPercent.Length - 1; num19 >= 0; num19--)
				{
					m_teamAttributeArmsmaster.clipPercent[num19] = float.Parse(array[num19]);
				}
				attribute4 = item.GetAttribute("cdPercent");
				array = attribute4.Split(',');
				m_teamAttributeArmsmaster.cdPercent = new float[array.Length];
				for (int num20 = m_teamAttributeArmsmaster.cdPercent.Length - 1; num20 >= 0; num20--)
				{
					m_teamAttributeArmsmaster.cdPercent[num20] = float.Parse(array[num20]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.SpecialTraining:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeSpecialTraining();
				m_teamAttributeSpecialTraining = (TeamSpecialAttribute.TeamAttributeSpecialTraining)teamAttributeData;
				string attribute4 = item.GetAttribute("hpMaxPercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeSpecialTraining.hpMaxPercent = new float[array.Length];
				for (int num6 = m_teamAttributeSpecialTraining.hpMaxPercent.Length - 1; num6 >= 0; num6--)
				{
					m_teamAttributeSpecialTraining.hpMaxPercent[num6] = float.Parse(array[num6]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Biobomb:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeBiobomb();
				m_teamAttributeBiobomb = (TeamSpecialAttribute.TeamAttributeBiobomb)teamAttributeData;
				string attribute4 = item.GetAttribute("proBomb");
				string[] array = attribute4.Split(',');
				m_teamAttributeBiobomb.proBomb = new ushort[array.Length];
				for (int num23 = m_teamAttributeBiobomb.proBomb.Length - 1; num23 >= 0; num23--)
				{
					m_teamAttributeBiobomb.proBomb[num23] = ushort.Parse(array[num23]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.LinkedLives:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeLinkedLives();
				m_teamAttributeLinkedLives = (TeamSpecialAttribute.TeamAttributeLinkedLives)teamAttributeData;
				string attribute4 = item.GetAttribute("proShareDamage");
				string[] array = attribute4.Split(',');
				m_teamAttributeLinkedLives.proShareDamage = new ushort[array.Length];
				for (int num17 = m_teamAttributeLinkedLives.proShareDamage.Length - 1; num17 >= 0; num17--)
				{
					m_teamAttributeLinkedLives.proShareDamage[num17] = ushort.Parse(array[num17]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.VolatileBomb:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeVolatileBomb();
				m_teamAttributeVolatileBomb = (TeamSpecialAttribute.TeamAttributeVolatileBomb)teamAttributeData;
				string attribute4 = item.GetAttribute("proBomb");
				string[] array = attribute4.Split(',');
				m_teamAttributeVolatileBomb.proBomb = new ushort[array.Length];
				for (int num27 = m_teamAttributeVolatileBomb.proBomb.Length - 1; num27 >= 0; num27--)
				{
					m_teamAttributeVolatileBomb.proBomb[num27] = ushort.Parse(array[num27]);
				}
				attribute4 = item.GetAttribute("stunTime");
				array = attribute4.Split(',');
				m_teamAttributeVolatileBomb.stunTime = new float[array.Length];
				for (int num28 = m_teamAttributeVolatileBomb.stunTime.Length - 1; num28 >= 0; num28--)
				{
					m_teamAttributeVolatileBomb.stunTime[num28] = float.Parse(array[num28]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Mania:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeMania();
				m_teamAttributeMania = (TeamSpecialAttribute.TeamAttributeMania)teamAttributeData;
				string attribute4 = item.GetAttribute("proCrit");
				string[] array = attribute4.Split(',');
				m_teamAttributeMania.proCrit = new ushort[array.Length];
				for (int num13 = m_teamAttributeMania.proCrit.Length - 1; num13 >= 0; num13--)
				{
					m_teamAttributeMania.proCrit[num13] = ushort.Parse(array[num13]);
				}
				attribute4 = item.GetAttribute("critDamagePercent");
				array = attribute4.Split(',');
				m_teamAttributeMania.critDamagePercent = new float[array.Length];
				for (int num14 = m_teamAttributeMania.critDamagePercent.Length - 1; num14 >= 0; num14--)
				{
					m_teamAttributeMania.critDamagePercent[num14] = float.Parse(array[num14]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.LivingCells:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeLivingCells();
				m_teamAttributeLivingCells = (TeamSpecialAttribute.TeamAttributeLivingCells)teamAttributeData;
				string attribute4 = item.GetAttribute("proResHp");
				string[] array = attribute4.Split(',');
				m_teamAttributeLivingCells.proResHp = new ushort[array.Length];
				for (int num3 = m_teamAttributeLivingCells.proResHp.Length - 1; num3 >= 0; num3--)
				{
					m_teamAttributeLivingCells.proResHp[num3] = ushort.Parse(array[num3]);
				}
				m_teamAttributeLivingCells.resHpPercent = float.Parse(item.GetAttribute("resHpPercent"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.ForceField:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeForceField();
				m_teamAttributeForceField = (TeamSpecialAttribute.TeamAttributeForceField)teamAttributeData;
				string attribute4 = item.GetAttribute("proReduceDamage");
				string[] array = attribute4.Split(',');
				m_teamAttributeForceField.proReduceDamage = new ushort[array.Length];
				for (int num25 = m_teamAttributeForceField.proReduceDamage.Length - 1; num25 >= 0; num25--)
				{
					m_teamAttributeForceField.proReduceDamage[num25] = ushort.Parse(array[num25]);
				}
				m_teamAttributeForceField.reduceDamagePercent = float.Parse(item.GetAttribute("reduceDamagePercent"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Prayer:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributePrayer();
				m_teamAttributePrayer = (TeamSpecialAttribute.TeamAttributePrayer)teamAttributeData;
				string attribute4 = item.GetAttribute("reduceDamagePercent");
				string[] array = attribute4.Split(',');
				m_teamAttributePrayer.reduceDamagePercent = new float[array.Length];
				for (int num21 = m_teamAttributePrayer.reduceDamagePercent.Length - 1; num21 >= 0; num21--)
				{
					m_teamAttributePrayer.reduceDamagePercent[num21] = float.Parse(array[num21]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Stun:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeStun();
				m_teamAttributeStun = (TeamSpecialAttribute.TeamAttributeStun)teamAttributeData;
				string attribute4 = item.GetAttribute("proStun");
				string[] array = attribute4.Split(',');
				m_teamAttributeStun.proStun = new ushort[array.Length];
				for (int num15 = m_teamAttributeStun.proStun.Length - 1; num15 >= 0; num15--)
				{
					m_teamAttributeStun.proStun[num15] = ushort.Parse(array[num15]);
				}
				m_teamAttributeStun.stunTime = float.Parse(item.GetAttribute("stunTime"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Nanotech:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeNanotech();
				m_teamAttributeNanotech = (TeamSpecialAttribute.TeamAttributeNanotech)teamAttributeData;
				string attribute4 = item.GetAttribute("reduceDebuffPercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeNanotech.reduceDebuffPercent = new float[array.Length];
				for (int num12 = m_teamAttributeNanotech.reduceDebuffPercent.Length - 1; num12 >= 0; num12--)
				{
					m_teamAttributeNanotech.reduceDebuffPercent[num12] = float.Parse(array[num12]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.SealingTechnique:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeSealingTechnique();
				m_teamAttributeSealingTechnique = (TeamSpecialAttribute.TeamAttributeSealingTechnique)teamAttributeData;
				string attribute4 = item.GetAttribute("proCd");
				string[] array = attribute4.Split(',');
				m_teamAttributeSealingTechnique.proCd = new ushort[array.Length];
				for (int num8 = m_teamAttributeSealingTechnique.proCd.Length - 1; num8 >= 0; num8--)
				{
					m_teamAttributeSealingTechnique.proCd[num8] = ushort.Parse(array[num8]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.ProlongedFirepower:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeProlongedFirepower();
				m_teamAttributeProlongedFirepower = (TeamSpecialAttribute.TeamAttributeProlongedFirepower)teamAttributeData;
				string attribute4 = item.GetAttribute("skillEmplacementTimePercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeProlongedFirepower.skillEmplacementTimePercent = new float[array.Length];
				for (int num5 = m_teamAttributeProlongedFirepower.skillEmplacementTimePercent.Length - 1; num5 >= 0; num5--)
				{
					m_teamAttributeProlongedFirepower.skillEmplacementTimePercent[num5] = float.Parse(array[num5]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Freeze:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeFreeze();
				m_teamAttributeFreeze = (TeamSpecialAttribute.TeamAttributeFreeze)teamAttributeData;
				string attribute4 = item.GetAttribute("proReduceMoveSpeedAndAttack");
				string[] array = attribute4.Split(',');
				m_teamAttributeFreeze.proReduceMoveSpeedAndAttack = new ushort[array.Length];
				for (int num29 = m_teamAttributeFreeze.proReduceMoveSpeedAndAttack.Length - 1; num29 >= 0; num29--)
				{
					m_teamAttributeFreeze.proReduceMoveSpeedAndAttack[num29] = ushort.Parse(array[num29]);
				}
				m_teamAttributeFreeze.reduceMoveSpeedAndAttackPercent = float.Parse(item.GetAttribute("reduceMoveSpeedAndAttackPercent"));
				m_teamAttributeFreeze.time = float.Parse(item.GetAttribute("time"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.AimedShot:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeAimedShot();
				m_teamAttributeAimedShot = (TeamSpecialAttribute.TeamAttributeAimedShot)teamAttributeData;
				string attribute4 = item.GetAttribute("proIgnoreDefence");
				string[] array = attribute4.Split(',');
				m_teamAttributeAimedShot.proIgnoreDefence = new ushort[array.Length];
				for (int num26 = m_teamAttributeAimedShot.proIgnoreDefence.Length - 1; num26 >= 0; num26--)
				{
					m_teamAttributeAimedShot.proIgnoreDefence[num26] = ushort.Parse(array[num26]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.ShadowWalk:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeShadowWalk();
				m_teamAttributeShadowWalk = (TeamSpecialAttribute.TeamAttributeShadowWalk)teamAttributeData;
				string attribute4 = item.GetAttribute("proDodge");
				string[] array = attribute4.Split(',');
				m_teamAttributeShadowWalk.proDodge = new ushort[array.Length];
				for (int num24 = m_teamAttributeShadowWalk.proDodge.Length - 1; num24 >= 0; num24--)
				{
					m_teamAttributeShadowWalk.proDodge[num24] = ushort.Parse(array[num24]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Splash:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeSplash();
				m_teamAttributeSplash = (TeamSpecialAttribute.TeamAttributeSplash)teamAttributeData;
				string attribute4 = item.GetAttribute("probability");
				string[] array = attribute4.Split(',');
				m_teamAttributeSplash.probability = new ushort[array.Length];
				for (int num22 = m_teamAttributeSplash.probability.Length - 1; num22 >= 0; num22--)
				{
					m_teamAttributeSplash.probability[num22] = ushort.Parse(array[num22]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Resolve:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeResolve();
				m_teamAttributeResolve = (TeamSpecialAttribute.TeamAttributeResolve)teamAttributeData;
				string attribute4 = item.GetAttribute("attackPercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeResolve.attackPercent = new float[array.Length];
				for (int num18 = m_teamAttributeResolve.attackPercent.Length - 1; num18 >= 0; num18--)
				{
					m_teamAttributeResolve.attackPercent[num18] = float.Parse(array[num18]);
				}
				m_teamAttributeResolve.hpReducePercent = float.Parse(item.GetAttribute("hpReducePercent"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Cooling:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeCooling();
				m_teamAttributeCooling = (TeamSpecialAttribute.TeamAttributeCooling)teamAttributeData;
				string attribute4 = item.GetAttribute("proNoCd");
				string[] array = attribute4.Split(',');
				m_teamAttributeCooling.proNoCd = new ushort[array.Length];
				for (int num16 = m_teamAttributeCooling.proNoCd.Length - 1; num16 >= 0; num16--)
				{
					m_teamAttributeCooling.proNoCd[num16] = ushort.Parse(array[num16]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.RelentlessRage:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeRelentlessRage();
				m_teamAttributeRelentlessRage = (TeamSpecialAttribute.TeamAttributeRelentlessRage)teamAttributeData;
				string attribute4 = item.GetAttribute("attackPercent");
				string[] array = attribute4.Split(',');
				m_teamAttributeRelentlessRage.attackPercent = new float[array.Length];
				for (int num10 = m_teamAttributeRelentlessRage.attackPercent.Length - 1; num10 >= 0; num10--)
				{
					m_teamAttributeRelentlessRage.attackPercent[num10] = float.Parse(array[num10]);
				}
				attribute4 = item.GetAttribute("proCrit");
				array = attribute4.Split(',');
				m_teamAttributeRelentlessRage.proCrit = new float[array.Length];
				for (int num11 = m_teamAttributeRelentlessRage.proCrit.Length - 1; num11 >= 0; num11--)
				{
					m_teamAttributeRelentlessRage.proCrit[num11] = float.Parse(array[num11]);
				}
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Vampire:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeVampire();
				m_teamAttributeVampire = (TeamSpecialAttribute.TeamAttributeVampire)teamAttributeData;
				string attribute4 = item.GetAttribute("proResHp");
				string[] array = attribute4.Split(',');
				m_teamAttributeVampire.proResHp = new ushort[array.Length];
				for (int num7 = m_teamAttributeVampire.proResHp.Length - 1; num7 >= 0; num7--)
				{
					m_teamAttributeVampire.proResHp[num7] = ushort.Parse(array[num7]);
				}
				m_teamAttributeVampire.resHpPercent = float.Parse(item.GetAttribute("resHpPercent"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Resurrection:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeResurrection();
				m_teamAttributeResurrection = (TeamSpecialAttribute.TeamAttributeResurrection)teamAttributeData;
				string attribute4 = item.GetAttribute("proAvoidDead");
				string[] array = attribute4.Split(',');
				m_teamAttributeResurrection.proAvoidDead = new ushort[array.Length];
				for (int num4 = m_teamAttributeResurrection.proAvoidDead.Length - 1; num4 >= 0; num4--)
				{
					m_teamAttributeResurrection.proAvoidDead[num4] = ushort.Parse(array[num4]);
				}
				m_teamAttributeResurrection.resHpPercent = float.Parse(item.GetAttribute("resHpPercent"));
				break;
			}
			case TeamSpecialAttribute.TeamAttributeType.Executioner:
			{
				teamAttributeData = new TeamSpecialAttribute.TeamAttributeExecutioner();
				m_teamAttributeExecutioner = (TeamSpecialAttribute.TeamAttributeExecutioner)teamAttributeData;
				string attribute4 = item.GetAttribute("probability");
				string[] array = attribute4.Split(',');
				m_teamAttributeExecutioner.probability = new float[array.Length];
				for (int num = m_teamAttributeExecutioner.probability.Length - 1; num >= 0; num--)
				{
					m_teamAttributeExecutioner.probability[num] = float.Parse(array[num]);
				}
				m_teamAttributeExecutioner.critTimes = int.Parse(item.GetAttribute("critTimes"));
				break;
			}
			}
			if (teamAttributeData != null)
			{
				teamAttributeData.name = attribute;
				teamAttributeData.iconFileName = attribute2;
				teamAttributeData.effects = null;
				teamAttributeData.effects = list;
				teamAttributeData.description = attribute3;
				teamAttributeData.type = teamAttributeType;
				m_teamAttributeData.Add(teamAttributeData.type, teamAttributeData);
			}
		}
		XmlElement xmlElement4 = (XmlElement)documentElement.GetElementsByTagName("Evolve").Item(0);
		m_teamAttributeEvolve = new Dictionary<TeamSpecialAttribute.TeamAttributeEvolveType, TeamSpecialAttribute.TeamAttributeEvolveData>();
		foreach (XmlElement item3 in xmlElement4.GetElementsByTagName("Attribute"))
		{
			TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeEvolveData = new TeamSpecialAttribute.TeamAttributeEvolveData();
			teamAttributeEvolveData.name = item3.GetAttribute("name");
			teamAttributeEvolveData.iconFileName = item3.GetAttribute("iconFileName");
			teamAttributeEvolveData.type = (TeamSpecialAttribute.TeamAttributeEvolveType)int.Parse(item3.GetAttribute("index"));
			string attribute4 = item3.GetAttribute("probability");
			string[] array = attribute4.Split(',');
			teamAttributeEvolveData.probability = new float[array.Length];
			for (int num30 = teamAttributeEvolveData.probability.Length - 1; num30 >= 0; num30--)
			{
				teamAttributeEvolveData.probability[num30] = float.Parse(array[num30]);
			}
			attribute4 = item3.GetAttribute("percent");
			array = attribute4.Split(',');
			teamAttributeEvolveData.percent = new float[array.Length];
			for (int num31 = teamAttributeEvolveData.percent.Length - 1; num31 >= 0; num31--)
			{
				teamAttributeEvolveData.percent[num31] = float.Parse(array[num31]);
			}
			teamAttributeEvolveData.time = float.Parse(item3.GetAttribute("time"));
			teamAttributeEvolveData.description = item3.GetAttribute("description");
			teamAttributeEvolveData.effects = new List<SpecialAttribute.SpecialAttributeEffectData>();
			foreach (XmlElement item4 in item3.GetElementsByTagName("SpecialEffect"))
			{
				SpecialAttribute.SpecialAttributeEffectData specialAttributeEffectData2 = new SpecialAttribute.SpecialAttributeEffectData();
				specialAttributeEffectData2.type = (SpecialAttribute.SpecialAttributeEffectType)int.Parse(item4.GetAttribute("type"));
				specialAttributeEffectData2.bufferCount = int.Parse(item4.GetAttribute("bufferCount"));
				teamAttributeEvolveData.effects.Add(specialAttributeEffectData2);
			}
			m_teamAttributeEvolve.Add(teamAttributeEvolveData.type, teamAttributeEvolveData);
		}
	}

	private void LoadConf()
	{
		if (Util.s_debug)
		{
			LoadWeaponDataFromDisk();
			LoadEnemyDataFromDisk();
			LoadAnimationDataFromDisk();
			LoadEffectDataFromDisk();
			LoadBulletDataFromDisk();
			LoadHeroDataFromDisk();
			LoadStuffDataFromDisk();
			LoadEquipDataFromDisk();
			LoadHeroSkillDataFromDisk();
			LoadGameLevelNodeDataFromDisk();
			LoadHeroRefreshData();
			LoadStoreData();
			LoadStoreExchangeData();
			LoadSpecialAttribteDataFromDisk();
			LoadSpecailEffectDataFromDisk();
			LoadTeamAttribteDataFromDisk();
			LoadLoadingTipsFromDisk();
			LoadAchievementsFromDisk();
		}
	}

	public string GetConfigNameByType(ConfigType type)
	{
		switch (type)
		{
		case ConfigType.Enemy:
			return "enemiesConfig";
		case ConfigType.Hero:
			return "heroesConfig";
		case ConfigType.Stuff:
			return "stuffsConfig";
		case ConfigType.Weapon:
			return "weaponsConfig";
		case ConfigType.Equip:
			return "equipsConfig";
		case ConfigType.HeroSkill:
			return "heroSkillConfig";
		case ConfigType.SpecialAttribute:
			return "specialAttributeConfig";
		case ConfigType.TeamSpecialAttribute:
			return "teamSpecialAttributeConfig";
		case ConfigType.LoadingTips:
			return "loadingTips";
		default:
			return string.Empty;
		}
	}

	public ConfigType GetConfigTypeByName(string name)
	{
		switch (name)
		{
		case "enemiesConfig":
			return ConfigType.Enemy;
		case "heroesConfig":
			return ConfigType.Hero;
		case "stuffsConfig":
			return ConfigType.Stuff;
		case "weaponsConfig":
			return ConfigType.Weapon;
		case "equipsConfig":
			return ConfigType.Equip;
		case "heroSkillConfig":
			return ConfigType.HeroSkill;
		case "specialAttributeConfig":
			return ConfigType.SpecialAttribute;
		case "teamSpecialAttributeConfig":
			return ConfigType.TeamSpecialAttribute;
		case "loadingTips":
			return ConfigType.LoadingTips;
		default:
			return ConfigType.None;
		}
	}

	public List<string> GetWeaponList()
	{
		return m_weaponList;
	}

	public WeaponData GetWeaponDataByType(Weapon.WeaponType type)
	{
		if (m_weaponData.ContainsKey(type))
		{
			return m_weaponData[type];
		}
		return null;
	}

	private void LoadWeaponDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Weapons");
		LoadWeaponData(text);
	}

	public void LoadWeaponData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_weaponData = new Dictionary<Weapon.WeaponType, WeaponData>();
		s_weaponLevelPerPhase = int.Parse(documentElement.GetAttribute("levelPerPhase"));
		foreach (XmlElement item in documentElement.GetElementsByTagName("Weapon"))
		{
			WeaponData weaponData = new WeaponData();
			weaponData.type = (Weapon.WeaponType)int.Parse(item.GetAttribute("type"));
			weaponData.name = item.GetAttribute("name");
			weaponData.modelFileName = item.GetAttribute("modelFileName");
			weaponData.iconFileName = item.GetAttribute("iconFileName");
			XmlElement xmlElement2 = (XmlElement)item.SelectSingleNode("Bullet");
			weaponData.bulletType = int.Parse(xmlElement2.GetAttribute("type"));
			weaponData.bulletSpeed = float.Parse(xmlElement2.GetAttribute("speed"));
			weaponData.attackRange = float.Parse(xmlElement2.GetAttribute("range"));
			weaponData.hitType = int.Parse(xmlElement2.GetAttribute("hitType"));
			string attribute = xmlElement2.GetAttribute("repelDis");
			string[] array = attribute.Split('-');
			weaponData.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			weaponData.repelTime = float.Parse(xmlElement2.GetAttribute("repelTime"));
			weaponData.effectHit = int.Parse(xmlElement2.GetAttribute("effectHitType"));
			weaponData.hitStrength = int.Parse(xmlElement2.GetAttribute("hitStrength"));
			weaponData.extra = new float[5];
			for (int i = 0; i < weaponData.extra.Length; i++)
			{
				string name = "extra" + (i + 1);
				if (xmlElement2.HasAttribute(name))
				{
					weaponData.extra[i] = float.Parse(xmlElement2.GetAttribute(name));
				}
			}
			XmlElement xmlElement3 = (XmlElement)item.SelectSingleNode("Feature");
			weaponData.emitTimeInAnimation = float.Parse(xmlElement3.GetAttribute("emitTimeInAnimation"));
			weaponData.deviationStart = int.Parse(xmlElement3.GetAttribute("deviationStart"));
			weaponData.deviationRecoverTime = float.Parse(xmlElement3.GetAttribute("deviationRecoverTime"));
			weaponData.deviationDeltaAngle = float.Parse(xmlElement3.GetAttribute("deviationDeltaAngle"));
			weaponData.deviationMaxAngle = float.Parse(xmlElement3.GetAttribute("deviationMaxAngle"));
			weaponData.upgradePhaseList = new List<WeaponUpgradePhase>();
			XmlElement xmlElement4 = (XmlElement)item.SelectSingleNode("UpgradePhase");
			foreach (XmlElement item2 in xmlElement4.GetElementsByTagName("Phase"))
			{
				WeaponUpgradePhase weaponUpgradePhase = new WeaponUpgradePhase();
				attribute = item2.GetAttribute("damage");
				array = attribute.Split('-');
				weaponUpgradePhase.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
				weaponUpgradePhase.damageIncr = float.Parse(item2.GetAttribute("damageIncr"));
				weaponUpgradePhase.ammo = int.Parse(item2.GetAttribute("ammo"));
				weaponUpgradePhase.reloadTime = float.Parse(item2.GetAttribute("reload"));
				int num = int.Parse(item2.GetAttribute("fireFrequence"));
				weaponUpgradePhase.fireFrequence = 1f / ((float)num / 60f);
				weaponUpgradePhase.rapid = int.Parse(item2.GetAttribute("rapid"));
				weaponUpgradePhase.rapidInterval = float.Parse(item2.GetAttribute("rapidInterval"));
				weaponData.upgradePhaseList.Add(weaponUpgradePhase);
			}
			m_weaponData.Add(weaponData.type, weaponData);
		}
	}

	public EnemyData GetEnemyDataByType(Enemy.EnemyType type)
	{
		switch (type)
		{
		case Enemy.EnemyType.Zombie_Purple:
			type = Enemy.EnemyType.Zombie;
			break;
		case Enemy.EnemyType.ZombieBomb_Purple:
			type = Enemy.EnemyType.ZombieBomb;
			break;
		case Enemy.EnemyType.ZombieNurse_Purple:
			type = Enemy.EnemyType.ZombieNurse;
			break;
		case Enemy.EnemyType.Haoke_Purple:
			type = Enemy.EnemyType.Haoke;
			break;
		case Enemy.EnemyType.FatCook_Purple:
			type = Enemy.EnemyType.FatCook;
			break;
		case Enemy.EnemyType.Pestilence_Purple:
			type = Enemy.EnemyType.Pestilence;
			break;
		case Enemy.EnemyType.Cowboy_Purple:
			type = Enemy.EnemyType.Cowboy;
			break;
		case Enemy.EnemyType.DeadLight_Purple:
			type = Enemy.EnemyType.DeadLight;
			break;
		case Enemy.EnemyType.Spore_Purple:
			type = Enemy.EnemyType.Spore;
			break;
		case Enemy.EnemyType.Butcher_Purple:
			type = Enemy.EnemyType.Butcher;
			break;
		case Enemy.EnemyType.Blaze_Purple:
			type = Enemy.EnemyType.Blaze;
			break;
		case Enemy.EnemyType.PestilenceJar_Purple:
			type = Enemy.EnemyType.PestilenceJar;
			break;
		}
		if (m_enemyData == null)
		{
			LoadEnemyDataFromDisk();
		}
		if (m_enemyData != null && m_enemyData.ContainsKey(type))
		{
			return m_enemyData[type];
		}
		return null;
	}

	private void LoadEnemyDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Enemies");
		LoadEnemyData(text);
	}

	public void LoadEnemyData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		string attribute = documentElement.GetAttribute("skillcdTime");
		string[] array = attribute.Split('-');
		s_enemySkillCdTime = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		s_enemyRandomSkillInterval = int.Parse(documentElement.GetAttribute("randomSkillInterval"));
		s_enemyRandomSkillOdds = float.Parse(documentElement.GetAttribute("randomSkillOdds"));
		m_enemyData = new Dictionary<Enemy.EnemyType, EnemyData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Enemy"))
		{
			EnemyData enemyData = new EnemyData();
			enemyData.index = int.Parse(item.GetAttribute("index"));
			enemyData.name = item.GetAttribute("name");
			enemyData.modelFileName = item.GetAttribute("modelName");
			enemyData.iconFileName = item.GetAttribute("iconFileName");
			enemyData.hp = int.Parse(item.GetAttribute("hp"));
			enemyData.eliteScale = float.Parse(item.GetAttribute("eliteScale"));
			enemyData.bossScale = float.Parse(item.GetAttribute("bossScale"));
			enemyData.moveSpeed = float.Parse(item.GetAttribute("moveSpeed"));
			enemyData.level = int.Parse(item.GetAttribute("level"));
			enemyData.rank = int.Parse(item.GetAttribute("rank"));
			attribute = item.GetAttribute("damage");
			array = attribute.Split('-');
			enemyData.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			enemyData.proCritical = int.Parse(item.GetAttribute("proCritical"));
			enemyData.critDamage = float.Parse(item.GetAttribute("critDamage"));
			enemyData.attackPrequence = float.Parse(item.GetAttribute("attackFrequence"));
			enemyData.proFirstAttackLose = int.Parse(item.GetAttribute("proFirstAttackLose"));
			enemyData.repelTime = float.Parse(item.GetAttribute("repelTime"));
			attribute = item.GetAttribute("repelDis");
			array = attribute.Split('-');
			enemyData.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			enemyData.proStun = int.Parse(item.GetAttribute("proStun"));
			enemyData.proResurge = int.Parse(item.GetAttribute("proResurge"));
			XmlElement xmlElement2 = (XmlElement)item.GetElementsByTagName("Drops").Item(0);
			enemyData.dropData = new EnemyDropData();
			enemyData.dropData.probability = ushort.Parse(xmlElement2.GetAttribute("probability"));
			enemyData.dropData.eruptProbability = ushort.Parse(xmlElement2.GetAttribute("eruptProbability"));
			enemyData.dropData.money = int.Parse(xmlElement2.GetAttribute("money"));
			enemyData.dropData.increase = float.Parse(xmlElement2.GetAttribute("increase"));
			XmlElement xmlElement3 = (XmlElement)item.GetElementsByTagName("Skills").Item(0);
			if (xmlElement3 != null)
			{
				enemyData.skillInfos = new List<SkillInfo>();
				foreach (XmlElement item2 in xmlElement3.GetElementsByTagName("Skill"))
				{
					SkillInfo skillInfo = new SkillInfo();
					skillInfo.name = item2.GetAttribute("name");
					skillInfo.type = (Character.SkillType)int.Parse(item2.GetAttribute("type"));
					skillInfo.attackRange = float.Parse(item2.GetAttribute("attackRange"));
					skillInfo.speed = float.Parse(item2.GetAttribute("speed"));
					skillInfo.time = float.Parse(item2.GetAttribute("time"));
					skillInfo.damage = float.Parse(item2.GetAttribute("damage"));
					skillInfo.percentDamage = float.Parse(item2.GetAttribute("percentDamage"));
					skillInfo.repelDis = float.Parse(item2.GetAttribute("repelDis"));
					skillInfo.repelTime = float.Parse(item2.GetAttribute("repelTime"));
					skillInfo.animReady = item2.GetAttribute("animReady");
					skillInfo.animProcess = item2.GetAttribute("animProcess");
					skillInfo.animEnd = item2.GetAttribute("animEnd");
					enemyData.skillInfos.Add(skillInfo);
				}
			}
			m_enemyData.Add((Enemy.EnemyType)enemyData.index, enemyData);
		}
	}

	public Dictionary<string, AnimData> GetCharacterAnimMap()
	{
		return m_characterAnimMap;
	}

	public AnimData GetCharacterAnim(string animName)
	{
		if (m_characterAnimMap.ContainsKey(animName))
		{
			return m_characterAnimMap[animName];
		}
		return null;
	}

	public AnimData GetNewCharacterAnim(string characterName, string animName)
	{
		if (m_newCharacterAnimMap.ContainsKey(characterName) && m_newCharacterAnimMap[characterName].ContainsKey(animName))
		{
			return m_newCharacterAnimMap[characterName][animName];
		}
		return null;
	}

	public AnimData GetEnemyAnim(string enemyName, string animName)
	{
		if (m_enemyAnimMap.ContainsKey(enemyName) && m_enemyAnimMap[enemyName].ContainsKey(animName))
		{
			return m_enemyAnimMap[enemyName][animName];
		}
		return null;
	}

	public void LoadAnimationDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Animation");
		LoadAnimationData(text);
	}

	public void LoadAnimationData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_characterAnimMap = new Dictionary<string, AnimData>();
		XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("CharacterAnimation").Item(0);
		foreach (XmlElement item in xmlElement.GetElementsByTagName("Anim"))
		{
			string attribute = item.GetAttribute("typeName");
			AnimData animData = new AnimData();
			animData.name = item.GetAttribute("fileName");
			animData.count = int.Parse(item.GetAttribute("count"));
			m_characterAnimMap.Add(attribute, animData);
		}
		m_newCharacterAnimMap = new Dictionary<string, Dictionary<string, AnimData>>();
		XmlElement xmlElement3 = (XmlElement)documentElement.GetElementsByTagName("NewCharacterAnimation").Item(0);
		if (xmlElement3 != null)
		{
			foreach (XmlElement item2 in xmlElement3.GetElementsByTagName("Character"))
			{
				string attribute2 = item2.GetAttribute("name");
				Dictionary<string, AnimData> dictionary = new Dictionary<string, AnimData>();
				foreach (XmlElement item3 in item2.GetElementsByTagName("Anim"))
				{
					AnimData animData2 = new AnimData();
					string attribute3 = item3.GetAttribute("typeName");
					animData2.name = item3.GetAttribute("fileName");
					animData2.count = int.Parse(item3.GetAttribute("count"));
					dictionary.Add(attribute3, animData2);
				}
				m_newCharacterAnimMap.Add(attribute2, dictionary);
			}
		}
		m_enemyAnimMap = new Dictionary<string, Dictionary<string, AnimData>>();
		XmlElement xmlElement6 = (XmlElement)documentElement.GetElementsByTagName("EnemyAnimation").Item(0);
		foreach (XmlElement item4 in xmlElement6.GetElementsByTagName("Enemy"))
		{
			string attribute4 = item4.GetAttribute("name");
			Dictionary<string, AnimData> dictionary2 = new Dictionary<string, AnimData>();
			foreach (XmlElement item5 in item4.GetElementsByTagName("Anim"))
			{
				AnimData animData3 = new AnimData();
				string attribute5 = item5.GetAttribute("typeName");
				animData3.name = item5.GetAttribute("fileName");
				animData3.count = int.Parse(item5.GetAttribute("count"));
				dictionary2.Add(attribute5, animData3);
			}
			m_enemyAnimMap.Add(attribute4, dictionary2);
		}
		m_weaponAnimMap = new Dictionary<string, Dictionary<string, string>>();
		XmlElement xmlElement9 = (XmlElement)documentElement.GetElementsByTagName("WeaponAnimation").Item(0);
		foreach (XmlElement item6 in xmlElement9.GetElementsByTagName("WeaponType"))
		{
			string attribute6 = item6.GetAttribute("name");
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			foreach (XmlElement item7 in item6.GetElementsByTagName("Anim"))
			{
				string attribute7 = item7.GetAttribute("typeName");
				string attribute8 = item7.GetAttribute("fileName");
				dictionary3.Add(attribute7, attribute8);
			}
			m_weaponAnimMap.Add(attribute6, dictionary3);
		}
	}

	public List<EffectData> GetEffectsData()
	{
		return m_effectData;
	}

	public EffectData GetEffectDataByIndex(int index)
	{
		if (index < 0 || index >= m_effectData.Count)
		{
			return null;
		}
		return m_effectData[index];
	}

	public void LoadEffectDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Effect");
		LoadEffectData(text);
	}

	public void LoadEffectData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_effectData = new List<EffectData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Effect"))
		{
			EffectData effectData = new EffectData();
			effectData.index = int.Parse(item.GetAttribute("index"));
			effectData.fileName = item.GetAttribute("fileName");
			effectData.animType = (Defined.EffectAnimType)int.Parse(item.GetAttribute("animType"));
			effectData.playTime = float.Parse(item.GetAttribute("playTime"));
			effectData.bufferNum = int.Parse(item.GetAttribute("bufferNum"));
			m_effectData.Add(effectData);
		}
	}

	public List<BulletData> GetBulletsData()
	{
		return m_bulletData;
	}

	public BulletData GetBulletDataByIndex(int index)
	{
		if (index < 0 || index >= m_bulletData.Count)
		{
			return null;
		}
		return m_bulletData[index];
	}

	public void LoadBulletDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Bullet");
		LoadBulletData(text);
	}

	public void LoadBulletData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_bulletData = new List<BulletData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Bullet"))
		{
			BulletData bulletData = new BulletData();
			bulletData.index = int.Parse(item.GetAttribute("index"));
			bulletData.fileNmae = item.GetAttribute("fileName");
			bulletData.isModel = ((!item.GetAttribute("isModel").Equals("0")) ? true : false);
			bulletData.hitType = int.Parse(item.GetAttribute("hitType"));
			m_bulletData.Add(bulletData);
		}
	}

	public HeroData GetHeroDataByIndex(int index)
	{
		if (m_heroData.ContainsKey(index))
		{
			return m_heroData[index];
		}
		return null;
	}

	private void LoadHeroDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Heroes");
		LoadHeroData(text);
	}

	public void LoadHeroData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_heroData = new Dictionary<int, HeroData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Hero"))
		{
			HeroData heroData = new HeroData();
			heroData.index = int.Parse(item.GetAttribute("index"));
			heroData.characterType = (Player.CharacterType)int.Parse(item.GetAttribute("characterType"));
			heroData.weaponType = (Weapon.WeaponType)int.Parse(item.GetAttribute("weaponType"));
			heroData.equipAdditionPercent = float.Parse(item.GetAttribute("equipAdditionPercent"));
			heroData.name = item.GetAttribute("name");
			heroData.profession = item.GetAttribute("profession");
			heroData.modelFileName = item.GetAttribute("modelFileName");
			heroData.iconFileName = item.GetAttribute("iconFileName");
			XmlElement xmlElement2 = (XmlElement)item.SelectSingleNode("Property");
			heroData.hp = int.Parse(xmlElement2.GetAttribute("hp"));
			heroData.moveSpeed = float.Parse(xmlElement2.GetAttribute("moveSpeed"));
			heroData.hitRate = int.Parse(xmlElement2.GetAttribute("hitRate"));
			heroData.proCritical = ushort.Parse(xmlElement2.GetAttribute("proCritical"));
			heroData.critDamage = float.Parse(xmlElement2.GetAttribute("critDamage"));
			heroData.dodge = int.Parse(xmlElement2.GetAttribute("dodge"));
			XmlElement xmlElement3 = (XmlElement)item.SelectSingleNode("Description");
			heroData.description = xmlElement3.GetAttribute("content");
			m_heroData.Add(heroData.index, heroData);
		}
	}

	public HeroSkillInfo GetHeroSkillInfo(Player.CharacterType characterType, int skillRank, int skillStar)
	{
		if (m_characterSkills.ContainsKey(characterType))
		{
			HeroSkillInfo heroSkillInfo = m_characterSkills[characterType];
			heroSkillInfo.SetSkillPhase(skillRank, skillStar);
			return heroSkillInfo;
		}
		return null;
	}

	private void LoadHeroSkillDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/HeroSkill");
		LoadHeroSkillData(text);
	}

	private void LoadHeroSkillBaseData(XmlElement xmlEle, HeroSkillInfo skill)
	{
		XmlElement xmlElement = (XmlElement)xmlEle.SelectSingleNode("Basic");
		skill.name = xmlElement.GetAttribute("name");
		skill.fileName = xmlElement.GetAttribute("fileName");
		skill.CDTime = float.Parse(xmlElement.GetAttribute("CD"));
		xmlElement = (XmlElement)xmlEle.SelectSingleNode("Description");
		skill.description = xmlElement.GetAttribute("content");
	}

	public void LoadHeroSkillData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		s_skillLevelPerPhase = int.Parse(documentElement.GetAttribute("levelPerPhase"));
		m_characterSkills = new Dictionary<Player.CharacterType, HeroSkillInfo>();
		SkillMike skillMike = new SkillMike();
		XmlElement xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillMike");
		LoadHeroSkillBaseData(xmlElement, skillMike);
		XmlElement xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		string attribute = xmlElement2.GetAttribute("repelDis");
		string[] array = attribute.Split('-');
		skillMike.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		skillMike.stunTime = float.Parse(xmlElement2.GetAttribute("stunTime"));
		skillMike.reduceSpeed = float.Parse(xmlElement2.GetAttribute("debuffMoveSpd"));
		skillMike.reduceSpeedTime = float.Parse(xmlElement2.GetAttribute("debuffTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillMike.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade = new SkillDamageUpgrade();
			attribute = item.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade.damageIncr = float.Parse(item.GetAttribute("damageIncr"));
			skillMike.upgradePhaseList.Add(skillDamageUpgrade);
		}
		m_characterSkills.Add(Player.CharacterType.Mike, skillMike);
		SkillChris skillChris = new SkillChris();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillChris");
		LoadHeroSkillBaseData(xmlElement, skillChris);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillChris.dashDistance = float.Parse(xmlElement2.GetAttribute("dashDistance"));
		skillChris.dashTime = float.Parse(xmlElement2.GetAttribute("dashTime"));
		attribute = xmlElement2.GetAttribute("repelDis");
		array = attribute.Split('-');
		skillChris.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillChris.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item2 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade2 = new SkillDamageUpgrade();
			attribute = item2.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade2.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade2.damageIncr = float.Parse(item2.GetAttribute("damageIncr"));
			skillChris.upgradePhaseList.Add(skillDamageUpgrade2);
		}
		m_characterSkills.Add(Player.CharacterType.Chris, skillChris);
		SkillLili skillLili = new SkillLili();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillLili");
		LoadHeroSkillBaseData(xmlElement, skillLili);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillLili.healTime = float.Parse(xmlElement2.GetAttribute("healTime"));
		skillLili.healInterval = float.Parse(xmlElement2.GetAttribute("healInterval"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillLili.upgradePhaseList = new List<SkillLiliUpgrade>();
		foreach (XmlElement item3 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillLiliUpgrade skillLiliUpgrade = new SkillLiliUpgrade();
			skillLiliUpgrade.healPercent = float.Parse(item3.GetAttribute("healPercent"));
			skillLiliUpgrade.healValue = float.Parse(item3.GetAttribute("healValue"));
			skillLiliUpgrade.healValueGrow = float.Parse(item3.GetAttribute("healValueGrow"));
			skillLiliUpgrade.reduceDamage = float.Parse(item3.GetAttribute("reduceDamage"));
			skillLili.upgradePhaseList.Add(skillLiliUpgrade);
		}
		m_characterSkills.Add(Player.CharacterType.Lili, skillLili);
		SkillVasily skillVasily = new SkillVasily();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillVasily");
		LoadHeroSkillBaseData(xmlElement, skillVasily);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillVasily.bulletType = int.Parse(xmlElement2.GetAttribute("bulletType"));
		skillVasily.speed = float.Parse(xmlElement2.GetAttribute("speed"));
		skillVasily.range = float.Parse(xmlElement2.GetAttribute("range"));
		skillVasily.hitType = int.Parse(xmlElement2.GetAttribute("hitType"));
		skillVasily.effectHitType = int.Parse(xmlElement2.GetAttribute("effectHitType"));
		skillVasily.fireFrequence = float.Parse(xmlElement2.GetAttribute("fireFrequence"));
		skillVasily.lifeTime = float.Parse(xmlElement2.GetAttribute("lifeTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillVasily.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item4 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade3 = new SkillDamageUpgrade();
			attribute = item4.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade3.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade3.damageIncr = float.Parse(item4.GetAttribute("damageIncr"));
			skillVasily.upgradePhaseList.Add(skillDamageUpgrade3);
		}
		m_characterSkills.Add(Player.CharacterType.Vasily, skillVasily);
		SkillClaire skillClaire = new SkillClaire();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillClaire");
		LoadHeroSkillBaseData(xmlElement, skillClaire);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillClaire.missileCount = int.Parse(xmlElement2.GetAttribute("missileCount"));
		skillClaire.missileAngle = float.Parse(xmlElement2.GetAttribute("missileAngle"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillClaire.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item5 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade4 = new SkillDamageUpgrade();
			attribute = item5.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade4.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade4.damageIncr = float.Parse(item5.GetAttribute("damageIncr"));
			skillClaire.upgradePhaseList.Add(skillDamageUpgrade4);
		}
		m_characterSkills.Add(Player.CharacterType.Claire, skillClaire);
		SkillFireDragon skillFireDragon = new SkillFireDragon();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillFireDragon");
		LoadHeroSkillBaseData(xmlElement, skillFireDragon);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillFireDragon.dashDistance = float.Parse(xmlElement2.GetAttribute("dashDistance"));
		skillFireDragon.dashTime = float.Parse(xmlElement2.GetAttribute("dashTime"));
		attribute = xmlElement2.GetAttribute("repelDis");
		array = attribute.Split('-');
		skillFireDragon.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillFireDragon.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item6 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade5 = new SkillDamageUpgrade();
			attribute = item6.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade5.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade5.damageIncr = float.Parse(item6.GetAttribute("damageIncr"));
			skillFireDragon.upgradePhaseList.Add(skillDamageUpgrade5);
		}
		m_characterSkills.Add(Player.CharacterType.FireDragon, skillFireDragon);
		SkillZero skillZero = new SkillZero();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillZero");
		LoadHeroSkillBaseData(xmlElement, skillZero);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillZero.summonTime = float.Parse(xmlElement2.GetAttribute("summonTime"));
		skillZero.damageInterval = float.Parse(xmlElement2.GetAttribute("damageInterval"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillZero.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item7 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade6 = new SkillDamageUpgrade();
			attribute = item7.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade6.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade6.damageIncr = float.Parse(item7.GetAttribute("damageIncr"));
			skillZero.upgradePhaseList.Add(skillDamageUpgrade6);
		}
		m_characterSkills.Add(Player.CharacterType.Zero, skillZero);
		SkillArnoud skillArnoud = new SkillArnoud();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillArnoud");
		LoadHeroSkillBaseData(xmlElement, skillArnoud);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillArnoud.damageRadius = float.Parse(xmlElement2.GetAttribute("damageRadius"));
		skillArnoud.speed = float.Parse(xmlElement2.GetAttribute("speed"));
		skillArnoud.range = float.Parse(xmlElement2.GetAttribute("range"));
		skillArnoud.fireFrequence = float.Parse(xmlElement2.GetAttribute("fireFrequence"));
		skillArnoud.lifeTime = float.Parse(xmlElement2.GetAttribute("lifeTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillArnoud.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item8 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade7 = new SkillDamageUpgrade();
			attribute = item8.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade7.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade7.damageIncr = float.Parse(item8.GetAttribute("damageIncr"));
			skillArnoud.upgradePhaseList.Add(skillDamageUpgrade7);
		}
		m_characterSkills.Add(Player.CharacterType.Arnoud, skillArnoud);
		SkillXJohnX skillXJohnX = new SkillXJohnX();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillXJohnX");
		LoadHeroSkillBaseData(xmlElement, skillXJohnX);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillXJohnX.range = float.Parse(xmlElement2.GetAttribute("range"));
		skillXJohnX.laserInterval = float.Parse(xmlElement2.GetAttribute("laserInterval"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillXJohnX.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item9 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade8 = new SkillDamageUpgrade();
			attribute = item9.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade8.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade8.damageIncr = float.Parse(item9.GetAttribute("damageIncr"));
			skillXJohnX.upgradePhaseList.Add(skillDamageUpgrade8);
		}
		m_characterSkills.Add(Player.CharacterType.XJohnX, skillXJohnX);
		SkillClint skillClint = new SkillClint();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillClint");
		LoadHeroSkillBaseData(xmlElement, skillClint);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillClint.keepTime = float.Parse(xmlElement2.GetAttribute("keepTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillClint.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item10 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade9 = new SkillDamageUpgrade();
			attribute = item10.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade9.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade9.damageIncr = float.Parse(item10.GetAttribute("damageIncr"));
			skillClint.upgradePhaseList.Add(skillDamageUpgrade9);
		}
		m_characterSkills.Add(Player.CharacterType.Clint, skillClint);
		SkillEva skillEva = new SkillEva();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillEva");
		LoadHeroSkillBaseData(xmlElement, skillEva);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillEva.debuffTime = float.Parse(xmlElement2.GetAttribute("debuffTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillEva.upgradePhaseList = new List<SkillEvaUpgrade>();
		foreach (XmlElement item11 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillEvaUpgrade skillEvaUpgrade = new SkillEvaUpgrade();
			skillEvaUpgrade.debuffHitRate = float.Parse(item11.GetAttribute("debuffHitRate"));
			skillEvaUpgrade.debuffMoveSpeed = float.Parse(item11.GetAttribute("debuffMoveSpeed"));
			skillEvaUpgrade.debuffAtk = float.Parse(item11.GetAttribute("debuffAtk"));
			skillEvaUpgrade.debuffGrowth = float.Parse(item11.GetAttribute("debuffGrowth"));
			skillEva.upgradePhaseList.Add(skillEvaUpgrade);
		}
		m_characterSkills.Add(Player.CharacterType.Eva, skillEva);
		SkillJason skillJason = new SkillJason();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillJason");
		LoadHeroSkillBaseData(xmlElement, skillJason);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillJason.setRadius = float.Parse(xmlElement2.GetAttribute("setRadius"));
		skillJason.explodeRadius = float.Parse(xmlElement2.GetAttribute("explodeRadius"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillJason.upgradePhaseList = new List<SkillJasonUpgrade>();
		foreach (XmlElement item12 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillJasonUpgrade skillJasonUpgrade = new SkillJasonUpgrade();
			attribute = item12.GetAttribute("damage");
			array = attribute.Split('-');
			skillJasonUpgrade.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillJasonUpgrade.damageIncr = float.Parse(item12.GetAttribute("damageIncr"));
			skillJasonUpgrade.mineCount = int.Parse(item12.GetAttribute("mineCount"));
			skillJason.upgradePhaseList.Add(skillJasonUpgrade);
		}
		m_characterSkills.Add(Player.CharacterType.Jason, skillJason);
		SkillTanya skillTanya = new SkillTanya();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillTanya");
		LoadHeroSkillBaseData(xmlElement, skillTanya);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillTanya.damageRadius = float.Parse(xmlElement2.GetAttribute("damageRadius"));
		attribute = xmlElement2.GetAttribute("repelDis");
		array = attribute.Split('-');
		skillTanya.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillTanya.upgradePhaseList = new List<SkillTanyaUpgrade>();
		foreach (XmlElement item13 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillTanyaUpgrade skillTanyaUpgrade = new SkillTanyaUpgrade();
			attribute = item13.GetAttribute("damage");
			array = attribute.Split('-');
			skillTanyaUpgrade.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillTanyaUpgrade.damageIncr = float.Parse(item13.GetAttribute("damageIncr"));
			skillTanyaUpgrade.fireCount = int.Parse(item13.GetAttribute("fireCount"));
			skillTanya.upgradePhaseList.Add(skillTanyaUpgrade);
		}
		m_characterSkills.Add(Player.CharacterType.Tanya, skillTanya);
		SkillBourne skillBourne = new SkillBourne();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillBourne");
		LoadHeroSkillBaseData(xmlElement, skillBourne);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillBourne.shootTime = float.Parse(xmlElement2.GetAttribute("shootTime"));
		skillBourne.damageCount = int.Parse(xmlElement2.GetAttribute("damageCount"));
		skillBourne.damageRadius = float.Parse(xmlElement2.GetAttribute("damageRadius"));
		attribute = xmlElement2.GetAttribute("repelDis");
		array = attribute.Split('-');
		skillBourne.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		skillBourne.debuffMoveSpeed = float.Parse(xmlElement2.GetAttribute("debuffMoveSpeed"));
		skillBourne.debuffTime = float.Parse(xmlElement2.GetAttribute("debuffTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillBourne.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item14 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade10 = new SkillDamageUpgrade();
			attribute = item14.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade10.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade10.damageIncr = float.Parse(item14.GetAttribute("damageIncr"));
			skillBourne.upgradePhaseList.Add(skillDamageUpgrade10);
		}
		m_characterSkills.Add(Player.CharacterType.Bourne, skillBourne);
		SkillRock skillRock = new SkillRock();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillRock");
		LoadHeroSkillBaseData(xmlElement, skillRock);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillRock.dashDistance = float.Parse(xmlElement2.GetAttribute("dashDistance"));
		skillRock.dashTime = float.Parse(xmlElement2.GetAttribute("dashTime"));
		attribute = xmlElement2.GetAttribute("repelDis");
		array = attribute.Split('-');
		skillRock.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillRock.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item15 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade11 = new SkillDamageUpgrade();
			attribute = item15.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade11.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade11.damageIncr = float.Parse(item15.GetAttribute("damageIncr"));
			skillRock.upgradePhaseList.Add(skillDamageUpgrade11);
		}
		m_characterSkills.Add(Player.CharacterType.Rock, skillRock);
		SkillWesker skillWesker = new SkillWesker();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillWesker");
		LoadHeroSkillBaseData(xmlElement, skillWesker);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillWesker.damageRadius = float.Parse(xmlElement2.GetAttribute("damageRadius"));
		attribute = xmlElement2.GetAttribute("repelDis");
		array = attribute.Split('-');
		skillWesker.repelDis = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
		skillWesker.limitTime = float.Parse(xmlElement2.GetAttribute("limitTime"));
		skillWesker.moveSpeed = float.Parse(xmlElement2.GetAttribute("moveSpeed"));
		skillWesker.count = int.Parse(xmlElement2.GetAttribute("count"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillWesker.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item16 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade12 = new SkillDamageUpgrade();
			attribute = item16.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade12.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade12.damageIncr = float.Parse(item16.GetAttribute("damageIncr"));
			skillWesker.upgradePhaseList.Add(skillDamageUpgrade12);
		}
		m_characterSkills.Add(Player.CharacterType.Wesker, skillWesker);
		SkillOppenheimer skillOppenheimer = new SkillOppenheimer();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillOppenheimer");
		LoadHeroSkillBaseData(xmlElement, skillOppenheimer);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillOppenheimer.frozeTime = float.Parse(xmlElement2.GetAttribute("frozeTime"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillOppenheimer.upgradePhaseList = new List<SkillOppenheimerUpgrade>();
		foreach (XmlElement item17 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillOppenheimerUpgrade skillOppenheimerUpgrade = new SkillOppenheimerUpgrade();
			attribute = item17.GetAttribute("damage");
			array = attribute.Split('-');
			skillOppenheimerUpgrade.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillOppenheimerUpgrade.damageIncr = float.Parse(item17.GetAttribute("damageIncr"));
			skillOppenheimerUpgrade.novaPlus = float.Parse(item17.GetAttribute("novaPlus"));
			skillOppenheimer.upgradePhaseList.Add(skillOppenheimerUpgrade);
		}
		m_characterSkills.Add(Player.CharacterType.Oppenheimer, skillOppenheimer);
		SkillShepard skillShepard = new SkillShepard();
		xmlElement = (XmlElement)documentElement.SelectSingleNode("SkillShepard");
		LoadHeroSkillBaseData(xmlElement, skillShepard);
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("FixedProperty");
		skillShepard.speed = float.Parse(xmlElement2.GetAttribute("speed"));
		skillShepard.radius = float.Parse(xmlElement2.GetAttribute("radius"));
		skillShepard.time = float.Parse(xmlElement2.GetAttribute("time"));
		skillShepard.damageInterval = float.Parse(xmlElement2.GetAttribute("damageInterval"));
		xmlElement2 = (XmlElement)xmlElement.SelectSingleNode("UpgradeProperty");
		skillShepard.upgradePhaseList = new List<SkillDamageUpgrade>();
		foreach (XmlElement item18 in xmlElement2.GetElementsByTagName("Phase"))
		{
			SkillDamageUpgrade skillDamageUpgrade13 = new SkillDamageUpgrade();
			attribute = item18.GetAttribute("damage");
			array = attribute.Split('-');
			skillDamageUpgrade13.damage = new NumberSection<float>(float.Parse(array[0]), float.Parse(array[1]));
			skillDamageUpgrade13.damageIncr = float.Parse(item18.GetAttribute("damageIncr"));
			skillShepard.upgradePhaseList.Add(skillDamageUpgrade13);
		}
		m_characterSkills.Add(Player.CharacterType.Shepard, skillShepard);
	}

	public StuffData GetStuffDataByIndex(int index)
	{
		if (m_stuffData.ContainsKey(index))
		{
			return m_stuffData[index];
		}
		return null;
	}

	private void LoadStuffDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Stuffs");
		LoadStuffData(text);
	}

	public void LoadStuffData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		int num = 0;
		string empty = string.Empty;
		m_stuffData = new Dictionary<int, StuffData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Stuff"))
		{
			StuffData stuffData = new StuffData();
			stuffData.index = int.Parse(item.GetAttribute("index"));
			stuffData.stuffType = (Defined.STUFF_TYPE)int.Parse(item.GetAttribute("stuffType"));
			stuffData.name = item.GetAttribute("name");
			stuffData.fileName = item.GetAttribute("fileName");
			stuffData.rank = (Defined.RANK_TYPE)int.Parse(item.GetAttribute("rank"));
			stuffData.cost = int.Parse(item.GetAttribute("cost"));
			stuffData.description = item.GetAttribute("description");
			if (stuffData.stuffType == Defined.STUFF_TYPE.Radar)
			{
				radarIndex = stuffData.index;
			}
			m_stuffData.Add(stuffData.index, stuffData);
		}
		foreach (XmlElement item2 in documentElement.GetElementsByTagName("Box"))
		{
			BoxData boxData = new BoxData();
			boxData.index = int.Parse(item2.GetAttribute("index"));
			boxData.stuffType = (Defined.STUFF_TYPE)int.Parse(item2.GetAttribute("stuffType"));
			boxData.name = item2.GetAttribute("name");
			boxData.fileName = item2.GetAttribute("fileName");
			boxData.boxRank = (Defined.BOX_RANK)int.Parse(item2.GetAttribute("boxRank"));
			boxData.needKey = int.Parse(item2.GetAttribute("needKey"));
			boxData.rank = (Defined.RANK_TYPE)int.Parse(item2.GetAttribute("rank"));
			boxData.cost = int.Parse(item2.GetAttribute("cost"));
			boxData.description = item2.GetAttribute("description");
			boxData.boxItemInfo = new List<BoxItemInfo>();
			foreach (XmlElement item3 in item2.GetElementsByTagName("BoxItem"))
			{
				BoxItemInfo boxItemInfo = new BoxItemInfo();
				boxItemInfo.type = item3.GetAttribute("type");
				empty = item3.GetAttribute("index");
				string[] array = empty.Split(',');
				boxItemInfo.index = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					boxItemInfo.index[i] = int.Parse(array[i]);
				}
				boxItemInfo.count = int.Parse(item3.GetAttribute("count"));
				empty = item3.GetAttribute("probability");
				array = empty.Split('-');
				boxItemInfo.probability = new NumberSection<int>(int.Parse(array[0]), int.Parse(array[1]));
				boxData.boxItemInfo.Add(boxItemInfo);
			}
			XmlElement xmlElement4 = item2.GetElementsByTagName("SystemEncourage").Item(0) as XmlElement;
			if (xmlElement4 != null)
			{
				boxData.systemEncourage = new BoxItemInfo();
				boxData.systemEncourage.type = xmlElement4.GetAttribute("type");
				empty = xmlElement4.GetAttribute("index");
				string[] array2 = empty.Split(',');
				boxData.systemEncourage.index = new int[array2.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					boxData.systemEncourage.index[j] = int.Parse(array2[j]);
				}
				boxData.systemEncourage.count = int.Parse(xmlElement4.GetAttribute("count"));
				empty = xmlElement4.GetAttribute("probability");
				array2 = empty.Split('-');
				boxData.systemEncourage.probability = new NumberSection<int>(int.Parse(array2[0]), int.Parse(array2[1]));
			}
			m_stuffData.Add(boxData.index, boxData);
		}
		foreach (XmlElement item4 in documentElement.GetElementsByTagName("ExpTome"))
		{
			ExpTomeData expTomeData = new ExpTomeData();
			expTomeData.index = int.Parse(item4.GetAttribute("index"));
			expTomeData.stuffType = (Defined.STUFF_TYPE)int.Parse(item4.GetAttribute("stuffType"));
			expTomeData.name = item4.GetAttribute("name");
			expTomeData.fileName = item4.GetAttribute("fileName");
			expTomeData.rank = (Defined.RANK_TYPE)int.Parse(item4.GetAttribute("rank"));
			expTomeData.cost = int.Parse(item4.GetAttribute("cost"));
			expTomeData.value = int.Parse(item4.GetAttribute("value"));
			expTomeData.calType = (Defined.CalType)int.Parse(item4.GetAttribute("calType"));
			expTomeData.description = item4.GetAttribute("description");
			m_stuffData.Add(expTomeData.index, expTomeData);
		}
	}

	public EquipData GetEquipDataByIndex(int index)
	{
		if (m_equipData.ContainsKey(index))
		{
			return m_equipData[index];
		}
		return null;
	}

	private void LoadEquipDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/Equips");
		LoadEquipData(text);
	}

	public void LoadEquipData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_equipData = new Dictionary<int, EquipData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Equip"))
		{
			EquipData equipData = new EquipData();
			equipData.index = int.Parse(item.GetAttribute("index"));
			equipData.name = item.GetAttribute("name");
			equipData.fileName = item.GetAttribute("fileName");
			equipData.equipType = (Defined.EQUIP_TYPE)int.Parse(item.GetAttribute("equipType"));
			equipData.rank = (Defined.RANK_TYPE)int.Parse(item.GetAttribute("rank"));
			string attribute = item.GetAttribute("hp");
			string[] array = attribute.Split(',');
			equipData.hp = new int[array.Length];
			for (int num = equipData.hp.Length - 1; num >= 0; num--)
			{
				equipData.hp[num] = int.Parse(array[num]);
			}
			equipData.def = float.Parse(item.GetAttribute("def"));
			equipData.proCrit = ushort.Parse(item.GetAttribute("proCrit"));
			equipData.critDamagePercent = float.Parse(item.GetAttribute("critDamagePercent"));
			equipData.proResilience = ushort.Parse(item.GetAttribute("proResilience"));
			equipData.proHit = ushort.Parse(item.GetAttribute("proHit"));
			equipData.hpPercent = float.Parse(item.GetAttribute("hpPercent"));
			equipData.proDodge = ushort.Parse(item.GetAttribute("proDodge"));
			equipData.proStab = ushort.Parse(item.GetAttribute("proStab"));
			equipData.atkPercent = float.Parse(item.GetAttribute("atkPercent"));
			equipData.atkFrequencyPercent = float.Parse(item.GetAttribute("atkFrequencyPercent"));
			equipData.reduceDamagePercent = float.Parse(item.GetAttribute("reduceDamagePercent"));
			equipData.moveSpeedPercent = float.Parse(item.GetAttribute("moveSpeedPercent"));
			equipData.atkRange = float.Parse(item.GetAttribute("atkRange"));
			equipData.specialAttr = ushort.Parse(item.GetAttribute("specialAttr"));
			equipData.description = item.GetAttribute("description");
			m_equipData.Add(equipData.index, equipData);
		}
	}

	public void SetCurrentGameLevel(Defined.LevelMode levelMode, int selectIndex)
	{
		if (selectIndex < 0 || !m_selectGameLevelNode.ContainsKey(levelMode) || selectIndex >= m_selectGameLevelNode[levelMode].Count)
		{
		}
		DataCenter.State().selectLevelMode = levelMode;
		DataCenter.State().selectAreaNode = selectIndex;
		if (selectIndex == -1)
		{
			m_selectedGameLevelData = new GameLevelData();
			m_selectedGameLevelData.level = 1;
			m_selectedGameLevelData.sBGM = "BGM_Channel";
			DataCenter.Save().selectLevelDropData = null;
			DataCenter.Save().selectLevelDropData = new LevelDropData();
			DataCenter.Save().selectLevelDropData.exp = 0;
			DataCenter.Save().selectLevelDropData.money = 1000;
			DataCenter.Save().selectLevelDropData.extraCrystal = 50;
		}
		else
		{
			m_selectedGameLevelData = m_selectGameLevelNode[levelMode][selectIndex];
		}
	}

	public GameLevelData GetCurrentGameLevelData()
	{
		return m_selectedGameLevelData;
	}

	public GameLevelNodeData[] GetGameLevelNodeList()
	{
		return m_gameLevelNodes.ToArray();
	}

	public GameLevelData[] GetCurrentGameLevelList(Defined.LevelMode levelMode)
	{
		if (m_selectGameLevelNode.ContainsKey(levelMode))
		{
			return m_selectGameLevelNode[levelMode].ToArray();
		}
		return null;
	}

	public void LoadGameLevelNodeDataFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/GameLevel/SelectGameLevel");
		LoadGameLevelNodeData(text);
	}

	private void LoadGameLevelNodeData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_gameLevelNodes = new List<GameLevelNodeData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("GameLevelNode"))
		{
			GameLevelNodeData gameLevelNodeData = new GameLevelNodeData();
			gameLevelNodeData.index = ushort.Parse(item.GetAttribute("index"));
			gameLevelNodeData.gameLevelID = item.GetAttribute("gameLevelID");
			gameLevelNodeData.name = item.GetAttribute("name");
			gameLevelNodeData.texIcon = item.GetAttribute("mapFileName");
			gameLevelNodeData.iconName = item.GetAttribute("mapIconName");
			gameLevelNodeData.mapNodePos = new Vector2(float.Parse(item.GetAttribute("mapNodePosX")), float.Parse(item.GetAttribute("mapNodePosY")));
			m_gameLevelNodes.Add(gameLevelNodeData);
		}
		if (Util.s_debug)
		{
			LoadSelectGameLevelDataFromDisk(m_gameLevelNodes[0].gameLevelID);
		}
	}

	public void LoadSelectGameLevelDataFromDisk(string gameLevelID)
	{
		if (m_selectedGameLevelData != null)
		{
			if (m_selectedGameLevelData.id != null && m_selectedGameLevelData.id.Equals(gameLevelID))
			{
				return;
			}
			m_selectedGameLevelData = null;
		}
		string text = FileUtil.LoadResourcesFile("Configs/GameLevel/" + gameLevelID);
		LoadSelectGameLevelData(text);
	}

	public void LoadSelectGameLevelData(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		int num = 0;
		m_selectGameLevelNode = new Dictionary<Defined.LevelMode, List<GameLevelData>>();
		foreach (XmlElement item3 in documentElement.GetElementsByTagName("LevelMode"))
		{
			Defined.LevelMode key = (Defined.LevelMode)int.Parse(item3.GetAttribute("mode"));
			string attribute = item3.GetAttribute("bgm");
			List<GameLevelData> list = new List<GameLevelData>();
			foreach (XmlElement item4 in item3.GetElementsByTagName("LevelNode"))
			{
				GameLevelData gameLevelData = new GameLevelData();
				gameLevelData.index = ushort.Parse(item4.GetAttribute("index"));
				gameLevelData.id = item4.GetAttribute("id");
				gameLevelData.name = item4.GetAttribute("name");
				if (item4.GetAttribute("depth") != string.Empty)
				{
					gameLevelData.depth = int.Parse(item4.GetAttribute("depth").ToString());
				}
				gameLevelData.mapNodePos = new Vector2(float.Parse(item4.GetAttribute("mapNodePosX")), float.Parse(item4.GetAttribute("mapNodePosY")));
				gameLevelData.mode = (Defined.LevelMode)ushort.Parse(item4.GetAttribute("mode"));
				gameLevelData.level = int.Parse(item4.GetAttribute("level"));
				gameLevelData.title = item4.GetAttribute("title");
				gameLevelData.description = item4.GetAttribute("description");
				gameLevelData.battleMode = (Defined.BattleMode)int.Parse(item4.GetAttribute("battleMode"));
				if (gameLevelData.isEncounter)
				{
					((GameLevelEncounterModeData)gameLevelData).enenyTeamID = item4.GetAttribute("enenyTeamID");
				}
				gameLevelData.sBGM = attribute;
				XmlElement xmlElement3 = item4.GetElementsByTagName("DialogStart").Item(0) as XmlElement;
				if (xmlElement3 != null)
				{
					gameLevelData.dialogStart = new List<GameLevelDialogData>();
					foreach (XmlElement item5 in xmlElement3.GetElementsByTagName("Part"))
					{
						GameLevelDialogData item = default(GameLevelDialogData);
						item.playerID = int.Parse(item5.GetAttribute("PlayerID"));
						item.dialog = item5.GetAttribute("dialog");
						gameLevelData.dialogStart.Add(item);
					}
				}
				XmlElement xmlElement5 = item4.GetElementsByTagName("DialogEnd").Item(0) as XmlElement;
				if (xmlElement5 != null)
				{
					gameLevelData.dialogEnd = new List<GameLevelDialogData>();
					foreach (XmlElement item6 in xmlElement5.GetElementsByTagName("Part"))
					{
						GameLevelDialogData item2 = default(GameLevelDialogData);
						item2.playerID = int.Parse(item6.GetAttribute("PlayerID"));
						item2.dialog = item6.GetAttribute("dialog");
						gameLevelData.dialogEnd.Add(item2);
					}
				}
				list.Add(gameLevelData);
			}
			m_selectGameLevelNode.Add(key, list);
		}
	}

	public void LoadEncounterEnemyTeam(string encounterEnemyTeamID)
	{
		string xml = FileUtil.LoadResourcesFile("Configs/GameLevel/EncounterEnemyTeam/" + encounterEnemyTeamID);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		int num = 0;
		DataCenter.User().PVP_SelectTarget.team.teamLevel = int.Parse(documentElement.GetAttribute("teamLevel"));
		XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("Talents").Item(0);
		foreach (XmlElement item in xmlElement.GetElementsByTagName("Talent"))
		{
			int key = int.Parse(item.GetAttribute("index"));
			int value = int.Parse(item.GetAttribute("level"));
			DataCenter.User().PVP_SelectTarget.team.talents.Add((TeamSpecialAttribute.TeamAttributeType)key, value);
		}
		XmlElement xmlElement3 = (XmlElement)documentElement.GetElementsByTagName("Evolves").Item(0);
		foreach (XmlElement item2 in xmlElement3.GetElementsByTagName("Evolve"))
		{
			int key2 = int.Parse(item2.GetAttribute("index"));
			int value2 = int.Parse(item2.GetAttribute("level"));
			DataCenter.User().PVP_SelectTarget.team.evolves.Add((TeamSpecialAttribute.TeamAttributeEvolveType)key2, value2);
		}
		XmlElement xmlElement5 = (XmlElement)documentElement.GetElementsByTagName("Heroes").Item(0);
		foreach (XmlElement item3 in xmlElement5.GetElementsByTagName("Hero"))
		{
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData = new PlayerData();
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.heroIndex = int.Parse(item3.GetAttribute("heroIndex"));
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.weaponLevel = int.Parse(item3.GetAttribute("weaponLevel"));
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.skillLevel = int.Parse(item3.GetAttribute("skillLevel"));
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.siteNum = int.Parse(item3.GetAttribute("siteNum"));
			UserEquipData userEquipData = new UserEquipData();
			userEquipData.currEquipIndex = int.Parse(item3.GetAttribute("equipHelmetIndex"));
			userEquipData.currEquipLevel = int.Parse(item3.GetAttribute("equipHelmetLevel"));
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.equips.Add(Defined.EQUIP_TYPE.Head, userEquipData);
			UserEquipData userEquipData2 = new UserEquipData();
			userEquipData2.currEquipIndex = int.Parse(item3.GetAttribute("equipArmorIndex"));
			userEquipData2.currEquipLevel = int.Parse(item3.GetAttribute("equipArmorLevel"));
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.equips.Add(Defined.EQUIP_TYPE.Body, userEquipData2);
			UserEquipData userEquipData3 = new UserEquipData();
			userEquipData3.currEquipIndex = int.Parse(item3.GetAttribute("equipOrnamentIndex"));
			userEquipData3.currEquipLevel = int.Parse(item3.GetAttribute("equipOrnamentLevel"));
			DataCenter.User().PVP_SelectTarget.team.teamSitesData[num].playerData.equips.Add(Defined.EQUIP_TYPE.Acc, userEquipData3);
			num++;
		}
	}

	public HeroRefreshData GetHeroRefreshDataByType(Defined.HERO_REFRESH_TYPE type)
	{
		if (m_heroRefreshData.ContainsKey(type))
		{
			return m_heroRefreshData[type];
		}
		return null;
	}

	public void LoadHeroRefreshData()
	{
		string xml = FileUtil.LoadResourcesFile("Configs/HeroRefresh");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		string empty = string.Empty;
		m_heroRefreshData = new Dictionary<Defined.HERO_REFRESH_TYPE, HeroRefreshData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Refresh"))
		{
			HeroRefreshData heroRefreshData = new HeroRefreshData();
			Defined.HERO_REFRESH_TYPE key = (Defined.HERO_REFRESH_TYPE)int.Parse(item.GetAttribute("type"));
			heroRefreshData.costType = (Defined.COST_TYPE)int.Parse(item.GetAttribute("costType"));
			heroRefreshData.cost = int.Parse(item.GetAttribute("cost"));
			heroRefreshData.limitTimeOneDay = int.Parse(item.GetAttribute("limitTimeOneDay"));
			heroRefreshData.refreshItemData = new Dictionary<Defined.RANK_TYPE, HeroRefreshItemData>();
			foreach (XmlElement item2 in item.GetElementsByTagName("Hero"))
			{
				HeroRefreshItemData heroRefreshItemData = new HeroRefreshItemData();
				heroRefreshItemData.rank = (Defined.RANK_TYPE)int.Parse(item2.GetAttribute("rank"));
				empty = item2.GetAttribute("index");
				string[] array = empty.Split(',');
				heroRefreshItemData.index = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					heroRefreshItemData.index[i] = int.Parse(array[i]);
				}
				empty = item2.GetAttribute("probability");
				array = empty.Split('-');
				heroRefreshItemData.probability = new NumberSection<int>(int.Parse(array[0]), int.Parse(array[1]));
				heroRefreshData.refreshItemData.Add(heroRefreshItemData.rank, heroRefreshItemData);
			}
			m_heroRefreshData.Add(key, heroRefreshData);
		}
	}

	public StoreItemData[] GetStoreListByType(Defined.STORE_ITEM_TYPE type)
	{
		if (m_storeData.ContainsKey(type))
		{
			return m_storeData[type].ToArray();
		}
		return null;
	}

	public StoreItemData[] GetStoreExchangeList()
	{
		return m_storeExchangeData.ToArray();
	}

	public void LoadStoreData()
	{
		string xml = FileUtil.LoadResourcesFile("Configs/Store");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_storeData = new Dictionary<Defined.STORE_ITEM_TYPE, List<StoreItemData>>();
		XmlElement xmlElement = (XmlElement)documentElement.GetElementsByTagName("Equips").Item(0);
		List<StoreItemData> list = new List<StoreItemData>();
		foreach (XmlElement item in xmlElement.GetElementsByTagName("Item"))
		{
			StoreItemData storeItemData = new StoreItemData();
			storeItemData.itemType = (Defined.ITEM_TYPE)int.Parse(item.GetAttribute("itemType"));
			storeItemData.index = int.Parse(item.GetAttribute("index"));
			storeItemData.costType = (Defined.COST_TYPE)int.Parse(item.GetAttribute("costType"));
			storeItemData.cost = int.Parse(item.GetAttribute("cost"));
			storeItemData.limitCountOneDay = int.Parse(item.GetAttribute("limitCountOneDay"));
			list.Add(storeItemData);
		}
		m_storeData.Add(Defined.STORE_ITEM_TYPE.Equip, list);
		XmlElement xmlElement3 = (XmlElement)documentElement.GetElementsByTagName("Stuffs").Item(0);
		List<StoreItemData> list2 = new List<StoreItemData>();
		foreach (XmlElement item2 in xmlElement3.GetElementsByTagName("Item"))
		{
			StoreItemData storeItemData2 = new StoreItemData();
			storeItemData2.itemType = (Defined.ITEM_TYPE)int.Parse(item2.GetAttribute("itemType"));
			storeItemData2.index = int.Parse(item2.GetAttribute("index"));
			storeItemData2.costType = (Defined.COST_TYPE)int.Parse(item2.GetAttribute("costType"));
			storeItemData2.cost = int.Parse(item2.GetAttribute("cost"));
			storeItemData2.limitCountOneDay = int.Parse(item2.GetAttribute("limitCountOneDay"));
			list2.Add(storeItemData2);
		}
		m_storeData.Add(Defined.STORE_ITEM_TYPE.Stuff, list2);
		XmlElement xmlElement5 = (XmlElement)documentElement.GetElementsByTagName("BoxAndKey").Item(0);
		List<StoreItemData> list3 = new List<StoreItemData>();
		foreach (XmlElement item3 in xmlElement5.GetElementsByTagName("Item"))
		{
			StoreItemData storeItemData3 = new StoreItemData();
			storeItemData3.itemType = (Defined.ITEM_TYPE)int.Parse(item3.GetAttribute("itemType"));
			storeItemData3.index = int.Parse(item3.GetAttribute("index"));
			storeItemData3.costType = (Defined.COST_TYPE)int.Parse(item3.GetAttribute("costType"));
			storeItemData3.cost = int.Parse(item3.GetAttribute("cost"));
			storeItemData3.limitCountOneDay = int.Parse(item3.GetAttribute("limitCountOneDay"));
			list3.Add(storeItemData3);
		}
		m_storeData.Add(Defined.STORE_ITEM_TYPE.BoxAndKey, list3);
	}

	public void LoadStoreExchangeData()
	{
		string xml = FileUtil.LoadResourcesFile("Configs/StoreExchange");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement documentElement = xmlDocument.DocumentElement;
		m_storeExchangeData = new List<StoreItemData>();
		foreach (XmlElement item in documentElement.GetElementsByTagName("Item"))
		{
			StoreItemData storeItemData = new StoreItemData();
			storeItemData.itemType = (Defined.ITEM_TYPE)int.Parse(item.GetAttribute("itemType"));
			storeItemData.index = int.Parse(item.GetAttribute("index"));
			storeItemData.costType = (Defined.COST_TYPE)int.Parse(item.GetAttribute("costType"));
			storeItemData.cost = int.Parse(item.GetAttribute("cost"));
			storeItemData.limitCountOneDay = int.Parse(item.GetAttribute("limitCountOneDay"));
			m_storeExchangeData.Add(storeItemData);
		}
	}

	public void LoadEnemySpawnInfoFromDisk()
	{
		if (Util.s_debugBuild)
		{
			string text = FileUtil.LoadResourcesFile("Configs/SpwanInfos/" + SceneLoadingManager.s_currSceneName + "_si");
			if (text != null && text.Length > 0)
			{
				LoadEnemySpawnInfo(text);
			}
		}
	}

	private void LoadEnemySpawnInfo(string text)
	{
		int num = 0;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		num = 0;
		GameObject gameObject = GameObject.Find("EnemySpawnPoint");
		Transform transform = gameObject.transform.Find("SpawnPointChain");
		if (transform != null)
		{
			Object.DestroyImmediate(transform.gameObject);
		}
		GameObject gameObject2 = new GameObject();
		gameObject2 = new GameObject();
		gameObject2.name = "SpawnPointChain";
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		foreach (XmlElement item in documentElement.GetElementsByTagName("SpawnPoint"))
		{
			GameObject gameObject3 = new GameObject();
			gameObject3.name = "SpawnPointChain";
			gameObject3.transform.parent = gameObject2.transform;
			EnemySpawnPointMain enemySpawnPointMain = gameObject3.AddComponent<EnemySpawnPointMain>();
			enemySpawnPointMain.transform.position = new Vector3(float.Parse(item.GetAttribute("PosX")), float.Parse(item.GetAttribute("PosY")), float.Parse(item.GetAttribute("PosZ")));
			enemySpawnPointMain.triggerDistance = float.Parse(item.GetAttribute("triggerDistance"));
			enemySpawnPointMain.frequency = float.Parse(item.GetAttribute("frequency"));
			enemySpawnPointMain.spawnOneTime = int.Parse(item.GetAttribute("spawnOneTime"));
			enemySpawnPointMain.storyPoint = int.Parse(item.GetAttribute("storyPoint"));
			if (item.GetAttribute("openDoor") != null)
			{
				Transform transform2 = GameObject.Find("Triggers").transform;
				num = int.Parse(item.GetAttribute("openDoor"));
				if (num != -1 && num <= transform2.childCount)
				{
					GameObject gameObject4 = transform2.GetChild(num).gameObject;
					enemySpawnPointMain.openDoor = gameObject4;
				}
			}
			if (enemySpawnPointMain.spawnWaves == null)
			{
				enemySpawnPointMain.spawnWaves = new List<SpawnEnemyWave>();
			}
			enemySpawnPointMain.spawnWaves.Clear();
			foreach (XmlElement item2 in item.GetElementsByTagName("SpawnWave"))
			{
				List<SpawnEnemyWave.SpawnInfo> list = new List<SpawnEnemyWave.SpawnInfo>();
				foreach (XmlElement item3 in item2.GetElementsByTagName("SpawnInfo"))
				{
					SpawnEnemyWave.SpawnInfo spawnInfo = new SpawnEnemyWave.SpawnInfo();
					spawnInfo.type = (Enemy.EnemyType)int.Parse(item3.GetAttribute("enemyType"));
					spawnInfo.count = int.Parse(item3.GetAttribute("count"));
					spawnInfo.eliteType = (Enemy.EnemyEliteType)int.Parse(item3.GetAttribute("eliteType"));
					num = int.Parse(item3.GetAttribute("isBoss"));
					spawnInfo.isBoss = num == 1;
					XmlElement xmlElement4 = item3.GetElementsByTagName("SpecialAttribute").Item(0) as XmlElement;
					spawnInfo.specialAttribute = new List<SpecialAttribute.AttributeType>();
					foreach (XmlElement item4 in xmlElement4.GetElementsByTagName("AttType"))
					{
						SpecialAttribute.AttributeType attributeType = (SpecialAttribute.AttributeType)int.Parse(item4.GetAttribute("type"));
						if (attributeType == SpecialAttribute.AttributeType.Random)
						{
							int num2 = Random.Range(1, 33);
							attributeType = (SpecialAttribute.AttributeType)num2;
						}
						spawnInfo.specialAttribute.Add(attributeType);
					}
					list.Add(spawnInfo);
				}
				SpawnEnemyWave spawnEnemyWave = new SpawnEnemyWave(list);
				spawnEnemyWave.waitSpawnTime = float.Parse(item2.GetAttribute("waitSpawnTime"));
				XmlElement xmlElement6 = (XmlElement)item2.GetElementsByTagName("SpawnPosition").Item(0);
				if (xmlElement6 != null)
				{
					if (spawnEnemyWave.spawnPosition == null)
					{
						GameObject gameObject5 = new GameObject();
						gameObject5.name = "EnemySpawnWavePoint";
						spawnEnemyWave.spawnPosition = gameObject5.AddComponent<EnemySpawnWavePoint>();
						spawnEnemyWave.spawnPosition.transform.parent = enemySpawnPointMain.transform;
					}
					spawnEnemyWave.spawnPosition.transform.position = new Vector3(float.Parse(xmlElement6.GetAttribute("PosX")), float.Parse(xmlElement6.GetAttribute("PosY")), float.Parse(xmlElement6.GetAttribute("PosZ")));
					spawnEnemyWave.spawnPosition.triggerDistance = float.Parse(xmlElement6.GetAttribute("triggerDistance"));
				}
				else if (spawnEnemyWave.spawnPosition != null)
				{
					Object.DestroyImmediate(spawnEnemyWave.spawnPosition);
				}
				enemySpawnPointMain.spawnWaves.Add(spawnEnemyWave);
			}
			num++;
		}
	}

	public string[] GetLoadingTips()
	{
		return m_loadingTips.ToArray();
	}

	public void LoadLoadingTipsFromDisk()
	{
		string text = FileUtil.LoadResourcesFile("Configs/LoadingTips");
		LoadLoadingTips(text);
	}

	public void LoadLoadingTips(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		if (m_loadingTips == null)
		{
			m_loadingTips = new List<string>();
		}
		foreach (XmlElement item in documentElement.GetElementsByTagName("tip"))
		{
			string attribute = item.GetAttribute("description");
			m_loadingTips.Add(attribute);
		}
	}
}

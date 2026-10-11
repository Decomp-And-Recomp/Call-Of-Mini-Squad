public class Defined
{
	public enum OBJECT_TYPE
	{
		OBJECT_TYPE_PLAYER = 0,
		OBJECT_TYPE_NPC = 1,
		OBJECT_TYPE_ALLY = 2,
		OBJECT_TYPE_ENEMY = 3,
		OBJECT_TYPE_ITEM = 4,
		OBJECT_TYPE_BULLET = 5,
		OBJECT_TYPE_OTHERS = 6,
		OBJECT_TYPE_SQUADCONTROLLER = 7
	}

	public enum EFFECT_TYPE
	{
		EFFECT_HIT_1 = 0,
		EFFECT_HIT_2 = 1,
		EFFECT_HIT_3 = 2,
		EFFECT_HIT_4 = 3,
		EFFECT_HIT_5 = 4,
		EFFECT_BOMB_1 = 5,
		EFFECT_GLITTER = 6,
		EFFECT_ENERGYSHIELD_BEGIN = 7,
		EFFECT_ENERGYSHIELD_COVER = 8,
		EFFECT_ENERGYSHIELD_END = 9,
		EFFECT_ADD_HP = 10,
		EFFECT_VOLLEY_FIRE = 11,
		EFFECT_VOLLEY_HIT = 12,
		EFFECT_SKILL_SHADOW_HIT_FIRE = 13,
		EFFECT_SKILL_SHADOW_HIT_ICE = 14,
		EFFECT_WEAPON_FIRE_LASER_01 = 15,
		EFFECT_WEAPON_FIRE_LASER_02 = 16,
		EFFECT_WEAPON_FIRE_LASER_03 = 17,
		EFFECT_WEAPON_FIRE_LASER_04 = 18,
		EFFECT_WEAPON_GRENADE_04 = 19,
		EFFECT_WEAPON_SHOTGUN_04 = 20,
		NINJIA_ATTACK_FIRE_SWING = 21,
		NINJIA_ATTACK_FIRE_BULLET = 22,
		NINJIA_01_ATTACK_BULLET = 23,
		NINJIA_01_ATTACK_ICE_SWING = 24,
		MACHINEGUN_01_ATTACK = 25,
		MACHINEGUN_02_ATTACK = 26,
		MACHINEGUN_03_ATTACK = 27,
		MACHINEGUN_04_ATTACK = 28,
		PISTOL_04_ATTACK = 29,
		VIRUS_01_ATTACK = 30,
		ZOMBIE_NURSE_DAMAGE_VENOM = 31,
		ZOMBIE_NURSE_VENOM_ATTACK = 32,
		POISON = 33,
		CURSE = 34,
		KNELL = 35,
		CONFUSE = 36,
		WEAKNESS = 37,
		DEADLIGHT_ATTACK_G = 38,
		BURST_B_01 = 39,
		BURST_D_01 = 40,
		pest_Attack_Attack = 41,
		Eva_Pollution = 42,
		BlackholeEnd = 43,
		Pistol_Love_Attack = 44,
		monsters_Vampire_0 = 45,
		gold = 46,
		Zombie_Pestilence_L_hit = 47,
		Leakage_Attack = 48,
		laceration_DOT = 49,
		Bloodthirst = 50,
		heores_Fake_Death = 51,
		heores_Aimed_Shot = 52,
		heores_Relentless_01 = 53,
		heores_Prayer = 54,
		heores_Focus = 55,
		heores_Splash_01 = 56,
		crystal = 57,
		COUNT = 58,
		NONE = -1
	}

	public enum SPECIAL_HIT_TYPE
	{
		NONE = 0,
		STUN = 1,
		FROZEN = 2,
		FROZEN_NOVA = 3,
		CAMERA_QUAKE = 4
	}

	public enum EQUIP_TYPE
	{
		Head = 0,
		Body = 1,
		Acc = 2
	}

	public enum EQUIP_SITE
	{
		Head = 0,
		Body = 1,
		Acc1 = 2,
		Acc2 = 3,
		Acc3 = 4
	}

	public enum TEAM_SITE
	{
		TEAM_LEADER = 0,
		TEAMMATE_1 = 1,
		TEAMMATE_2 = 2,
		TEAMMATE_3 = 3,
		TEAMMATE_4 = 4
	}

	public enum BAG_ITEM_TYPE
	{
		HEAD = 0,
		BODY = 1,
		ACC = 2,
		OTHERS = 3,
		PAPER = 4,
		STUFF = 5
	}

	public enum RANK_TYPE
	{
		WHITE = 0,
		GREEN = 1,
		BLUE = 2,
		PURPLE = 3
	}

	public enum STUFF_TYPE
	{
		Stuff = 0,
		Blueprints = 1,
		Box = 2,
		Key = 3,
		Radar = 4,
		Expbook = 5
	}

	public enum BOX_RANK
	{
		Copper = 0,
		Silver = 1,
		Gold = 2
	}

	public enum ITEM_TYPE
	{
		Money = 0,
		Stuff = 1,
		Equip = 2
	}

	public enum STORE_ITEM_TYPE
	{
		Equip = 0,
		Stuff = 1,
		BoxAndKey = 2
	}

	public enum HERO_REFRESH_TYPE
	{
		Normal = 0,
		Premium = 1,
		Super = 2
	}

	public enum COST_TYPE
	{
		Money = 0,
		Crystal = 1,
		Honor = 2,
		Radar = 3,
		Element = 4,
		Exp = 5
	}

	public enum ObjectLinkType
	{
		Cloned = 0,
		ShareLife = 1,
		Summon = 2
	}

	public enum LevelMode
	{
		Normal = 0,
		Hard = 1,
		Hell = 2
	}

	public enum BattleMode
	{
		Normal = 0,
		Boss = 1,
		Encounter = 2
	}

	public enum SceneType
	{
		Menu = 0,
		Battle = 1
	}

	public enum CalType
	{
		General = 0,
		Percentage = 1
	}

	public enum EffectAnimType
	{
		Particle = 0,
		Animation = 1,
		Animator = 2,
		Combie = 3
	}

	public enum ItemState
	{
		Locked = 0,
		Purchase = 1,
		Available = 2,
		FailByReasonOne = 11
	}

	public enum EquipAdditionType
	{
		High = 0,
		Middle = 1,
		Low = 2
	}

	public enum BattleResult
	{
		Win = 0,
		Failed = 1,
		Retreat = 2
	}

	public enum CameraView
	{
		Far = 0,
		Default = 1,
		Close = 2
	}

	public enum TutorialStep
	{
		None = 0,
		TutorialBattle = 1,
		CreateNameFialed = 2,
		CreateNameFinish = 3,
		UnlockedTeamSite = 4,
		SetTeam = 5,
		EnterBattle = 6,
		FinishStageOneWaveOne = 7,
		EquipUpgrade = 8,
		Finish = 9
	}

	public enum CameraQuakeType
	{
		Quake_A = 0,
		Quake_B = 1,
		Quake_C = 2,
		Quake_D = 3
	}

	public enum TeamEquipmentBreakType
	{
		Weapon = 0,
		Skill = 1
	}

	public const float CAMERA_DISTANCE_FAR_Y = 13f;

	public const float CAMERA_DISTANCE_FAR_Z = 13f;

	public const float CAMERA_DISTANCE_DEFAULT_Y = 10f;

	public const float CAMERA_DISTANCE_DEFAULT_Z = 10f;

	public const float CAMERA_DISTANCE_CLOSE_Y = 8.5f;

	public const float CAMERA_DISTANCE_CLOSE_Z = 8.5f;

	public const float PVP_CAMERA_DISTANCE_Y = 17f;

	public const float PVP_CAMERA_DISTANCE_Z = 12.7f;

	public const int CAMERA_FOCUS_OFFSE = 1;

	public const int CAMERA_ZOOMIN_OFFSE_DEATH = 5;

	public const float CAMERA_DISTANCE_UIExhibition_CLOSE_Y = 8f;

	public const float CAMERA_DISTANCE_UIExhibition_CLOSE_Z = 5f;

	public const string WEAPON_NAME_AK47 = "Ak47";

	public const string WEAPON_NAME_HK_MP5 = "HK MP5";

	public const string WEAPON_NAME_M1S90 = "M1S90";

	public const string WEAPON_NAME_DOUBLEGUN = "DoubleGun";

	public const string WEAPON_NAME_HANDGUN = "HandGun";

	public const string WEAPON_NAME_MACHINEGUN = "MachineGun";

	public const string WEAPON_NAME_RIFLE = "Rifle";

	public const string WEAPON_NAME_RPG = "RPG";

	public const string WEAPON_NAME_SHOTGUN = "ShotGun";

	public const string WEAPON_NAME_PISTOL = "Pistol";

	public const string WEAPON_NAME_SNIPER = "Sniper";

	public const string WEAPON_NAME_GRENADE = "Grenade";

	public const string WEAPON_NAME_NINJIA_FIRE = "Ninjia_Fire";

	public const string WEAPON_NAME_NINJIA_ICE = "Ninjia_Ice";

	public const string WEAPON_NAME_PISTOL_BUFF = "Virus";

	public const string WEAPON_NAME_LASER = "Laser";

	public const string WEAPON_TYPE_RIFLE = "Rifle";

	public const string RES_BUILDINGS_PATH = "Models/Buildings/X1/";

	public const string RES_CHARACTERS_PATH = "Models/Characters/";

	public const string RES_NEW_CHARACTERS_PATH = "Models/NewCharacters/";

	public const string RES_WEAPONS_PATH = "Models/Weapons/";

	public const string RES_BULLETS_PATH = "Models/Bullets/";

	public const string RES_EFFECTS_PATH = "Effects/";

	public const string RES_ACTIVEOBJ_PATH = "Models/ActiveObjects/";

	public const string RES_EQUIPMATERIAL_PATH = "EquipMaterial/";

	public const string RES_ITEM_PATH = "Models/Items/";

	public const string RES_SCENES_PATH = "Models/Scenes/";

	public const string RES_GAME_PATH = "Game/";

	public const string RES_LOADING_PATH = "UI/Loading/";

	public const int MAX_BULLET_TRAJECTORY_BUFFER_NUM = 100;

	public const int MAX_BULLET_BUFFER_NUM = 20;

	public const int MAX_EFFECT_BUFFER_NUM = 10;

	public const int MAX_EFFECT_BUFFER_COUNT = 2;

	public const int MAX_ENEMY_ONSCREEN = 30;

	public const int MAX_ENEMY_BUFFER_NUM = 30;

	public const int MAX_ENEMY_ONSCREEN_NORMAL = 0;

	public const int MAX_WAVE_ON_ENEMY_SPAWNPINT = 3;

	public const float FireReduceSpeed = 0f;

	public const float AllFollowSpeedUp = 0.5f;

	public const float SquadRadius = 1.8f;

	public const float TeammemberGuardDistance = 5f;

	public const float ENEMY_HURT_TIME_NORMAL_MIN = 0.3f;

	public const float ENEMY_HURT_TIME_NORMAL_MAX = 0.7f;

	public const float ENEMY_HURT_TIME_ELITE_MIN = 0.1f;

	public const float ENEMY_HURT_TIME_ELITE_MAX = 0.2f;

	public const float ENEMY_HURT_TIME_BOSS_MIN = 0f;

	public const float ENEMY_HURT_TIME_BOSS_MAX = 0.2f;

	public const float ENEMY_ELITE_RANK1_HP_INCREASE = 5f;

	public const float ENEMY_ELITE_RANK2_HP_INCREASE = 5f;

	public const float ENEMY_ELITE_RANK3_HP_INCREASE = 10f;

	public const string BIP_ARM_RIGHT = "Bip01/Spine_00/Bip01 Spine/Spine_0/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 Prop1";

	public const string BIP_ARM_LEFT = "Bip01/Spine_00/Bip01 Spine/Spine_0/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 Prop2";

	public const string BIP_BONE = "Bip01/Spine_00/Bip01 Spine";

	public const string BIP_PELVIS = "Bip01/Bip01 Spine";

	public const string BIP_PELVIS_2 = "Bip01/Bip01 Pelvis/Bip01 Spine";

	public const string BIP_PELVIS_3 = "Dummy_All/Bip01/Bip01 Pelvis/Bip01 Spine";

	public const string AI_NAME_MOVE = "Move";

	public const string AI_NAME_IDLE = "Idle";

	public const string AI_NAME_SHOOT = "Shoot";

	public const string AI_NAME_RELOAD = "Reload";

	public const string AI_NAME_DEATH = "Death";

	public const string AI_NAME_CHASE = "Chase";

	public const string AI_NAME_MELEE = "Melee";

	public const string AI_NAME_GUARD = "Guard";

	public const string AI_NAME_GUARD_PVP = "GuardPVP";

	public const string AI_NAME_MOVESHOOT = "MoveShoot";

	public const string AI_NAME_HURT = "Hurt";

	public const string AI_NAME_FOLLOW = "AllyFollow";

	public const string AI_NAME_REPEL = "Repel";

	public const string AI_NAME_BORN = "Born";

	public const string AI_NAME_SHIFT = "Shift";

	public const string AI_NAME_LOWER_MOVE = "LowerMove";

	public const string AI_NAME_FIREREADY = "FireReady";

	public const string AI_NAME_UPPERFIRE = "UpperFire";

	public const string AI_NAME_FINDSEAT = "FindSeat";

	public const string AI_NAME_SERIOUSINJURY = "SeriousInjury";

	public const string AI_NAME_SPECIALATTACK = "SpecialAttack";

	public const string AI_NAME_STUN = "Stun";

	public const string AI_NAME_FROZEN = "Frozen";

	public const string AI_NAME_UPPERRELOAD = "UpperReload";

	public const string AI_NAME_SKILL_DASH = "SkillDash";

	public const string AI_NAME_SKILL_GNAW = "SkillGnaw";

	public const string AI_NAME_SKILL = "Skill";

	public const string AI_NAME_SKILLREADY = "SkillReady";

	public const string AI_NAME_TIMER_SKILL = "TimerSkill";

	public const string AI_NAME_AUTOCONTRUL = "AutoControl";

	public const string AI_NAME_AVOID = "Avoid";

	public const string AI_NAME_STUCK = "Stuck";

	public const string AI_NAME_SQUADGUARD = "SquadGuard";

	public const string AI_NAME_SKILL_FINTARGET = "SkillFindTarget";

	public const string ENEMY_NAME_X1 = "MC";

	public const string ENEMY_NAME_ZOMBIE = "Zombie";

	public const string ENEMY_NAME_ZOMBIE_BOMB = "Zombie_Self_Destruchtion";

	public const string ENEMY_NAME_ZOMBIE_NURSE = "Zombie_Nurse";

	public const string ENEMY_NAME_HAOKE = "Haoke_B";

	public const string ENEMY_NAME_WRESTLER = "Zombie_Wrestler_Green";

	public const string ENEMY_NAME_NURSE = "Zombie_Nurse";

	public const string ENEMY_NAME_POLICEMAN_PISTOL = "Zombie_Policeman_Pistol";

	public const string ANIM_NAME_ATTACK_SPECIAL_01_A = "Attack_Special_01_A";

	public const string ANIM_NAME_ATTACK_SPECIAL_01_B = "Attack_Special_01_B";

	public const string ANIM_NAME_ATTACK_SPECIAL_01_C = "Attack_Special_01_C";

	public const string ANIM_NAME_ATTACK_SPECIAL_02_A = "Attack_Special_02_A";

	public const string ANIM_NAME_ATTACK_SPECIAL_02_B = "Attack_Special_02_B";

	public const string ANIM_NAME_ATTACK_SPECIAL_02_C = "Attack_Special_02_C";

	public const string ANIM_NAME_ATTACK_SPECIAL_03_A = "Attack_Special_03_A";

	public const string ANIM_NAME_ATTACK_SPECIAL_03_B = "Attack_Special_03_B";

	public const string ANIM_NAME_ATTACK_SPECIAL_03_C = "Attack_Special_03_C";

	public const string ANIMATION_NAME_DEFAULT_PREFIX = "Rifle";

	public const string ANIM_CH_BACKRETURN = "Back_Return";

	public const string ANIM_CH_MINE = "Mine";

	public const string ANIM_CH_BACK = "Back";

	public const string ANIM_CH_SKILL = "Skill";

	public const string ANIM_CH_CHARGE_LOWER = "Charge_Lower";

	public const string ANIM_CH_DEATH = "Death";

	public const string ANIM_CH_HURT = "Hurt";

	public const string ANIM_CH_IDLE = "Idle";

	public const string ANIM_CH_RELOAD = "Reload";

	public const string ANIM_CH_SHIFT = "Shift";

	public const string ANIM_CH_SHOTING = "Shoting";

	public const string ANIM_CH_UPPER_RUN = "Upper_Body_Run";

	public const string ANIM_CH_READYFIRE = "ReadyFire";

	public const string ANIM_CH_SHOW = "Show";

	public const string ANIM_NAME_SERIOUSINJURY = "SeriousInjury";

	public const string ANIM_CH_LOWER_RAN_F = "Lower_Body_Run_F";

	public const string ANIM_CH_LOWER_RAN_B = "Lower_Body_Run_B";

	public const string ANIM_CH_LOWER_RAN_L = "Lower_Body_Run_L";

	public const string ANIM_CH_LOWER_RAN_R = "Lower_Body_Run_R";

	public const string ANIM_CH_DEATH_VIOLENT = "DeathViolent";

	public const string ANIM_CH_GUARD = "Guard";

	public const string ANIM_CH_MOVE = "Move";

	public const string ANIM_CH_START_MOVE = "StartMove";

	public const string ANIM_CH_STOP_MOVE = "StopMove";

	public const string ANIM_CH_ATTACK = "Attack";

	public const string ANIM_CH_APPEAR = "Appear";

	public const string ANIM_CH_MOVEFAST = "MoveFast";

	public const string ANIM_CH_EXPLODE = "Explode";

	public const string ANIM_WEAPON_LOWER_IDLE = "Lower_Body_Idle";

	public const string ANIM_WEAPON_LOWER_RELOAD = "Lower_Body_Reload";

	public const string ANIM_WEAPON_LOWER_SHIFT = "Lower_Body_Shift";

	public const string ANIM_WEAPON_LOWER_SHOTING = "Lower_Body_Shoting";

	public const string ANIM_WEAPON_UPPER_IDLE = "Upper_Body_Idle";

	public const string ANIM_WEAPON_UPPER_RELOAD = "Upper_Body_Reload";

	public const string ANIM_WEAPON_UPPER_RUN = "Upper_Body_Run";

	public const string ANIM_WEAPON_UPPER_SHIFT = "Upper_Body_Shift";

	public const string ANIM_WEAPON_UPPER_SHOTING = "Upper_Body_Shoting";

	public const float ZOMBIES_ANIM_PLAY_BASE_SPEED = 4f;

	public const float HEROES_ANIM_PLAY_BASE_SPEED = 6f;

	public const string IMPLICATE_EVENT_TYPE_DEATH = "Death";

	public const int TEAM_SITE_MAX_COUNT = 5;

	public const int TEAM_SITE_EQUIP_MAX_COUNT = 5;

	public const string EFFECT_NAME_HIT = "Hit";

	public const string EFFECT_NAME_ATTACK = "Attack";

	public const string EFFECT_NAME_ATTACK_02 = "Attack_02";

	public const string EFFECT_NAME_BLOOD = "Blood";

	public const string EFFECT_NAME_DEADBLOOD = "DeadBlood";

	public const string EFFECT_NAME_THUD = "Thud";

	public const string EFFECT_NAME_THUDINDICATE = "ThudIndicate";

	public const string EFFECT_NAME_BURN = "Burn";

	public const string EFFECT_NAME_ATTACKINDICATE = "AttackIndicate";

	public const string EFFECT_NAME_FIRE = "Fire";

	public const string EFFECT_NAME_FIRE_02 = "Fire_02";

	public const string EFFECT_NAME_DASH = "Dash";

	public const string EFFECT_NAME_POWER = "Power";

	public const string EFFECT_NAME_FROZEN_APPEAR = "FrozenAppear";

	public const string EFFECT_NAME_FROZEN_CONTINUE = "FrozenContinue";

	public const string EFFECT_NAME_DEATH = "Death";

	public const string EFFECT_NAME_STUN = "Stun";

	public const string AUDIO_NAME_ATTACK = "Attack";

	public const string AUDIO_NAME_ATTACK_02 = "Attack_02";

	public const string AUDIO_NAME_ATTACK_03 = "Attack_03";

	public const string AUDIO_NAME_ATTACK_04 = "Attack_04";

	public const string AUDIO_NAME_SKILL = "Skill";

	public const string AUDIO_NAME_MOVE = "Move";

	public const string AUDIO_NAME_DEATH = "Death";

	public const int UI_ROOT_MAXIMUM_HEIGHT = 768;

	public static string[] SystemUserName = new string[]
	{
		"JohnnyHello"
		//"Aaron", "Abraham", "Angus", "Bert", "Brant", "Chris", "Cosmo", "Edison", "Ethan", "Francis",
		//"George", "Harrison", "Ignativs", "Jacob", "Jerry", "Joe", "Lawrence", "Mark", "Norman", "Peter",
		//"Randolph", "Robinson", "Steven", "Shawn", "Terry", "Warren"
	};

	public static string[] LoadingIcons = new string[17]
	{
		"pic_loading01", "pic_loading02", "pic_loading03", "pic_loading04", "pic_loading05", "pic_loading06", "pic_loading07", "pic_loading08", "pic_loading09", "pic_loading10",
		"pic_loading11", "pic_loading12", "pic_loading13", "pic_loading14", "pic_loading15", "pic_loading16", "pic_loading17"
	};

	public static int[,] TEAMMATE_POSTION_OFFSET = new int[5, 2]
	{
		{ 0, 0 },
		{ -2, 0 },
		{ 2, 0 },
		{ 0, -2 },
		{ 0, 2 }
	};

	public static int[,] TEAMMATE_POSTION_OFFSET_PVP = new int[5, 2]
	{
		{ 0, 0 },
		{ -3, 0 },
		{ 3, 0 },
		{ -6, 0 },
		{ 6, 0 }
	};
}

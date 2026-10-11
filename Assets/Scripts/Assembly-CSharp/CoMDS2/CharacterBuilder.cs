using UnityEngine;

namespace CoMDS2
{
	public class CharacterBuilder
	{
		public static Player CreatePlayer(string model, Vector3 position, Quaternion rotation, int layer)
		{
			Player player = new Player();
			GameObject prefab = Resources.Load<GameObject>("Models/Characters/" + model);
			player.Initialize(prefab, "Player", position, rotation, layer);
			player.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER;
			return player;
		}

		public static Player CreatePlayerByCharacterType(TeamData teamData, PlayerData playerData, Vector3 position, Quaternion rotation, int layer)
		{
			DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
			Player player;
			switch (heroDataByIndex.characterType)
			{
			case Player.CharacterType.Mike:
				player = new PlayerCharacterMike();
				break;
			case Player.CharacterType.Chris:
				player = new PlayerCharacterChris();
				break;
			case Player.CharacterType.Lili:
				player = new PlayerCharacterLili();
				break;
			case Player.CharacterType.Vasily:
				player = new PlayerCharacterVasily();
				break;
			case Player.CharacterType.Claire:
				player = new PlayerCharacterClaire();
				break;
			case Player.CharacterType.FireDragon:
				player = new PlayerCharacterFireDragon();
				break;
			case Player.CharacterType.Zero:
				player = new PlayerCharacterZero();
				break;
			case Player.CharacterType.Arnoud:
				player = new PlayerCharacterArnoud();
				break;
			case Player.CharacterType.XJohnX:
				player = new PlayerCharacterXJohnX();
				break;
			case Player.CharacterType.Clint:
				player = new PlayerCharacterClint();
				break;
			case Player.CharacterType.Eva:
				player = new PlayerCharacterEva();
				break;
			case Player.CharacterType.Jason:
				player = new PlayerCharacterJason();
				break;
			case Player.CharacterType.Tanya:
				player = new PlayerCharacterTanya();
				break;
			case Player.CharacterType.Bourne:
				player = new PlayerCharacterBourne();
				break;
			case Player.CharacterType.Rock:
				player = new PlayerCharacterRock();
				break;
			case Player.CharacterType.Wesker:
				player = new PlayerCharacterWesker();
				break;
			case Player.CharacterType.Oppenheimer:
				player = new PlayerCharacterOppenheimer();
				break;
			case Player.CharacterType.Shepard:
				player = new PlayerCharacterShepard();
				break;
			default:
				player = new Player();
				break;
			}
			player.name = heroDataByIndex.name;
			player.characterType = heroDataByIndex.characterType;
			player.SetGameData(playerData, teamData.teamLevel);
			player.SetTeamAttributeData(teamData);
			GameObject prefab = Resources.Load<GameObject>("Models/NewCharacters/" + heroDataByIndex.modelFileName);
			player.Initialize(prefab, heroDataByIndex.name, position, rotation, layer);
			player.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER;
			if (DataCenter.State().isPVPMode)
			{
				player.FillCDTime();
			}
			return player;
		}

		public static Player CreatePlayerByCharacterType(TeamData teamData, int siteNum, Vector3 position, Quaternion rotation, int layer)
		{
			PlayerData playerData = teamData.teamSitesData[siteNum].playerData;
			return CreatePlayerByCharacterType(teamData, playerData, position, rotation, layer);
		}

		public static Enemy CreateEnemy(string model, Vector3 position, Quaternion rotation, int layer)
		{
			Enemy enemy = new Enemy();
			enemy.Initialize(Resources.Load<GameObject>("Models/Characters/" + model), "Enemy", position, rotation, layer);
			enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
			return enemy;
		}

		public static Enemy CreateEnemy(string model, string skin, Vector3 position, Quaternion rotation, int layer)
		{
			if (skin.Length <= 0)
			{
				return CreateEnemy(model, position, rotation, layer);
			}
			Enemy enemy = new Enemy();
			enemy.Initialize(Resources.Load<GameObject>("Models/Characters/" + model + "_" + skin), "Enemy", position, rotation, layer);
			enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
			return enemy;
		}

		public static Enemy CreateEnemy(Enemy.EnemyType type, Vector3 position, Quaternion rotation, int layer)
		{
			Enemy enemy = null;
			DataConf.EnemyData enemyDataByType = DataCenter.Conf().GetEnemyDataByType(type);
			string empty = string.Empty;
			switch (type)
			{
			case Enemy.EnemyType.Zombie:
			case Enemy.EnemyType.Zombie_Purple:
			{
				enemy = new EnemyZombie();
				empty = ((type != Enemy.EnemyType.Zombie_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab10 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab10, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.ZombieBomb:
			case Enemy.EnemyType.ZombieBomb_Purple:
			{
				enemy = new EnemyZombieBomb();
				empty = ((type != Enemy.EnemyType.ZombieBomb_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab3 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab3, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.ZombieNurse:
			case Enemy.EnemyType.ZombieNurse_Purple:
			{
				enemy = new EnemyZombieNurse();
				empty = ((type != Enemy.EnemyType.ZombieNurse_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab11 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab11, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Haoke:
			case Enemy.EnemyType.Haoke_Purple:
			{
				enemy = new EnemyHaoke();
				empty = ((type != Enemy.EnemyType.Haoke_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab6 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab6, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Wrestler:
			{
				enemy = new EnemyWrestler();
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab16 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName);
				enemy.Initialize(prefab16, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.FatCook:
			case Enemy.EnemyType.FatCook_Purple:
			{
				enemy = new EnemyFatCook();
				empty = ((type != Enemy.EnemyType.FatCook_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab4 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab4, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.PolicemanPistol:
			case Enemy.EnemyType.PolicemanShotgun:
			{
				enemy = new EnemyZombiePoliceman();
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab8 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName);
				enemy.Initialize(prefab8, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Pestilence:
			case Enemy.EnemyType.Pestilence_Purple:
			{
				enemy = new EnemyPestilence();
				empty = ((type != Enemy.EnemyType.Pestilence_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab15 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab15, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Cowboy:
			case Enemy.EnemyType.Cowboy_Purple:
			{
				enemy = new EnemyCowboy();
				empty = ((type != Enemy.EnemyType.Cowboy_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab12 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab12, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Hook:
			{
				enemy = new EnemyHook();
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab7 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName);
				enemy.Initialize(prefab7, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.DeadLight:
			case Enemy.EnemyType.DeadLight_Purple:
			{
				enemy = new EnemyDeadLight();
				empty = ((type != Enemy.EnemyType.DeadLight_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab2 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab2, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Spore:
			case Enemy.EnemyType.Spore_Purple:
			{
				enemy = new EnemySpore();
				empty = ((type != Enemy.EnemyType.Spore_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab14 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab14, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.GuterTrennung:
			{
				enemy = new EnemyGuterTrennung();
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab13 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName);
				enemy.Initialize(prefab13, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Butcher:
			case Enemy.EnemyType.Butcher_Purple:
			{
				enemy = new EnemyButcher();
				empty = ((type != Enemy.EnemyType.Butcher_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab9 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab9, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.Blaze:
			case Enemy.EnemyType.Blaze_Purple:
			{
				enemy = new EnemyBlaze();
				empty = ((type != Enemy.EnemyType.Blaze_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab5 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab5, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.GoldenGoblin:
			{
				enemy = new EnemyGoldenGoblin();
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab11 = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName);
				enemy.Initialize(prefab11, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			case Enemy.EnemyType.PestilenceJar:
			case Enemy.EnemyType.PestilenceJar_Purple:
			{
				enemy = new EnemyPestilenceJar();
				empty = ((type != Enemy.EnemyType.PestilenceJar_Purple) ? string.Empty : "_Purple");
				enemy.enemyType = type;
				enemy.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY;
				enemy.enemyAnimTag = enemyDataByType.modelFileName;
				GameObject prefab = Resources.Load<GameObject>("Models/Characters/Enemies/" + enemyDataByType.modelFileName + empty);
				enemy.Initialize(prefab, enemyDataByType.name, position, rotation, layer);
				enemy.GetGameObject().SetActive(false);
				break;
			}
			}
			return enemy;
		}
	}
}

using CoMDS2;
using UnityEngine;

public static class GoldenGoblinSpawner
{
	private static bool s_spawned;

	public static void ResetForBattle()
	{
		s_spawned = false;
		LevelDropData drop = DataCenter.Save().selectLevelDropData;
		if (drop == null)
		{
			return;
		}
		EconomyConfig.EnsureLoaded();
		drop.bDropGoldenMonster = Random.Range(0, 100) < EconomyConfig.GoldenGoblinChance;
	}

	public static void OnChainTriggered(EnemySpawnPointMain chain)
	{
		if (s_spawned || !DataCenter.Save().BattleTutorialFinished || DataCenter.State().isPVPMode)
		{
			return;
		}
		LevelDropData drop = DataCenter.Save().selectLevelDropData;
		if (drop == null || !drop.bDropGoldenMonster)
		{
			return;
		}
		Player player = GameBattle.m_instance.GetPlayer();
		if (player == null)
		{
			return;
		}
		s_spawned = true;
		Vector3 position = chain.transform.position;
		if (chain.spawnWaves != null && chain.spawnWaves.Count > 0 && chain.spawnWaves[0].spawnPosition != null)
		{
			position = chain.spawnWaves[0].spawnPosition.transform.position;
		}
		Enemy enemy = CharacterBuilder.CreateEnemy(Enemy.EnemyType.GoldenGoblin, position, player.GetTransform().rotation, 11);
		enemy.isBoss = false;
		enemy.eliteType = Enemy.EnemyEliteType.None;
		enemy.GetGameObject().SetActive(true);
		enemy.Reset();
		GameBattle.m_instance.AddObjToComputerList(enemy);
		GameBattle.s_enemyCount++;
	}
}

using CoMDS2;
using UnityEngine;

public static class ChestSpawner
{
	private static float s_roundEndTime = -1f;

	public static void ResetForBattle()
	{
		s_roundEndTime = -1f;
		ChestBuffs.ResetForBattle();
		BattleChestUI.ResetForBattle();
	}

	private static bool Enabled
	{
		get
		{
			return DataCenter.Save().BattleTutorialFinished && !DataCenter.State().isPVPMode;
		}
	}

	public static void OnChainTriggered(EnemySpawnPointMain chain)
	{
		if (!Enabled)
		{
			return;
		}
		if (!LaterAreaLeft())
		{
			if (s_roundEndTime > 0f)
			{
				s_roundEndTime = -1f;
				BattleChestUI.HideTips();
			}
			return;
		}
		RewardBoxConfig.EnsureLoaded();
		if (s_roundEndTime > 0f || RewardBoxConfig.KillTime <= 0f)
		{
			return;
		}
		s_roundEndTime = Time.time + RewardBoxConfig.KillTime;
		BattleChestUI.ShowTips(RewardBoxConfig.KillTime);
	}

	private static bool LaterAreaLeft()
	{
		EnemySpawnPointMain[] chains = Object.FindObjectsByType<EnemySpawnPointMain>(FindObjectsSortMode.None);
		foreach (EnemySpawnPointMain chain in chains)
		{
			if (chain.storyPoint > GameBattle.s_storyPoint)
			{
				return true;
			}
		}
		return false;
	}

	public static void OnAreaCleared(EnemySpawnPointMain chain, bool lastArea)
	{
		if (!Enabled || chain.openDoor == null)
		{
			return;
		}
		bool inTime = s_roundEndTime > 0f && Time.time <= s_roundEndTime;
		s_roundEndTime = -1f;
		BattleChestUI.HideTips();
		if (inTime && !lastArea)
		{
			CreateChest();
		}
	}

	private static void CreateChest()
	{
		Player player = GameBattle.m_instance.GetPlayer();
		GameObject prefab = Resources.Load<GameObject>("Models/Items/Chest");
		if (player == null || prefab == null)
		{
			return;
		}
		Transform t = player.GetTransform();
		Vector3 position = t.position - t.forward * Random.Range(1.5f, 3f) + t.right * Random.Range(-1f, 1f);
		UnityEngine.AI.NavMeshHit hit;
		if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
		{
			position = hit.position;
		}
		else
		{
			position = t.position;
		}
		Object.Instantiate(prefab, position, Quaternion.LookRotation(-t.forward));
	}
}

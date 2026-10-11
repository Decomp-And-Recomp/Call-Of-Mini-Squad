using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

public static class RewardBoxConfig
{
	public enum RewardType
	{
		MoveSpeed = 0,
		AddDamage = 1,
		SkillDamagePercent = 2,
		DropGold = 3,
		DropCrystal = 4,
		HpMaxPercent = 5,
		SkillCoolDownCancel = 6,
		Invincible = 7,
		AddHP = 8
	}

	public class Reward
	{
		public RewardType type;

		public string name;

		public float probability;

		public float duration;

		public string icon;

		public float percentage;

		public int goldPieces = 10;

		public float goldStagePercent = 20f;

		public int[] crystalPerLocation = new int[] { 5, 5, 10, 10 };

		public int[] crystalDifficultyMultiple = new int[] { 1, 2, 3 };
	}

	public static float KillTime = 90f;

	public static float ChestLifeTime = 90f;

	private static bool s_loaded;

	private static List<Reward> s_rewards = new List<Reward>();

	public static List<Reward> Rewards
	{
		get
		{
			EnsureLoaded();
			return s_rewards;
		}
	}

	public static void EnsureLoaded()
	{
		if (s_loaded)
		{
			return;
		}
		s_loaded = true;
		string text = FileUtil.LoadResourcesFile("Configs/RewardBox");
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogWarning("RewardBoxConfig: Configs/RewardBox not found, chests will give nothing.");
			return;
		}
		try
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(text);
			Load(document.DocumentElement);
		}
		catch (Exception e)
		{
			Debug.LogError("RewardBoxConfig: could not parse Configs/RewardBox. " + e.Message);
		}
	}

	private static void Load(XmlElement root)
	{
		XmlElement chest = root.SelectSingleNode("Chest") as XmlElement;
		if (chest != null)
		{
			KillTime = Float(chest, "killTime", KillTime);
			ChestLifeTime = Float(chest, "lifeTime", ChestLifeTime);
		}
		foreach (XmlElement element in root.SelectNodes("Reward"))
		{
			Reward reward = new Reward();
			int index = (int)Float(element, "index", -1f);
			if (!Enum.IsDefined(typeof(RewardType), index))
			{
				Debug.LogWarning("RewardBoxConfig: unknown reward index " + index + ", skipped.");
				continue;
			}
			reward.type = (RewardType)index;
			reward.name = element.GetAttribute("name");
			reward.probability = Float(element, "probability", 0f);
			reward.duration = Float(element, "durcation", 0f);
			reward.icon = element.GetAttribute("icon");
			XmlElement property = element.SelectSingleNode("Property") as XmlElement;
			if (property != null)
			{
				reward.percentage = Float(property, "percentage", 0f);
				reward.goldPieces = (int)Float(property, "pieces", reward.goldPieces);
				reward.goldStagePercent = Float(property, "stageGoldPercent", reward.goldStagePercent);
			}
			XmlElement stage = element.SelectSingleNode("Stage") as XmlElement;
			if (stage != null)
			{
				reward.crystalPerLocation = IntList(stage.GetAttribute("dropCount"), reward.crystalPerLocation);
			}
			XmlElement difficult = element.SelectSingleNode("Difficult") as XmlElement;
			if (difficult != null)
			{
				reward.crystalDifficultyMultiple = IntList(difficult.GetAttribute("multiple"), reward.crystalDifficultyMultiple);
			}
			s_rewards.Add(reward);
		}
	}

	public static Reward Roll()
	{
		List<Reward> rewards = Rewards;
		float total = 0f;
		foreach (Reward reward in rewards)
		{
			total += reward.probability;
		}
		if (total <= 0f)
		{
			return null;
		}
		float pick = UnityEngine.Random.Range(0f, total);
		foreach (Reward reward in rewards)
		{
			if (pick < reward.probability)
			{
				return reward;
			}
			pick -= reward.probability;
		}
		return rewards[rewards.Count - 1];
	}

	private static float Float(XmlElement element, string attribute, float fallback)
	{
		float value;
		string text = element.GetAttribute(attribute);
		if (!string.IsNullOrEmpty(text) && float.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
		{
			return value;
		}
		return fallback;
	}

	private static int[] IntList(string text, int[] fallback)
	{
		if (string.IsNullOrEmpty(text))
		{
			return fallback;
		}
		string[] parts = text.Split(',');
		int[] values = new int[parts.Length];
		for (int i = 0; i < parts.Length; i++)
		{
			int value;
			if (!int.TryParse(parts[i].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
			{
				return fallback;
			}
			values[i] = value;
		}
		return values;
	}
}

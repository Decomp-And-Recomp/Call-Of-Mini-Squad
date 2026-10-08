using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

// Prices and unlock requirements read from Resources/Configs (Heroes.xml, Team.xml, Upgrades.xml, Rewards.xml). Every value falls back to the game's original number if its file or attribute is missing.
public static class EconomyConfig
{
	public class HeroUnlock
	{
		public int heroIndex;
		public int order;
		public int startSite = -1;
		public Defined.ItemState state = Defined.ItemState.Purchase;
		public int teamLevel;
		public Defined.COST_TYPE costType = Defined.COST_TYPE.Crystal;
		public int cost;
	}

	public class SiteConfig
	{
		public Defined.ItemState state;
		public int cost;
	}

	public class BreakthroughConfig
	{
		public int crystal = 10;
		public int teamLevel = 10;
	}

	public class EquipSlotConfig
	{
		public int teamLevel;
		public int crystal;
	}

	private static bool s_loaded;

	// Heroes.xml config values
	private static List<HeroUnlock> s_heroes = new List<HeroUnlock>();

	// Team.xml config values
	public static int TeamMaxLevel = 50;
	public static SiteConfig[] Sites;
	public static int TalentUnlockTeamLevel = 10;
	public static int TalentResetCostCrystal = 20;
	public static int ExtraPointCostCrystal = 10;
	public static int ExtraPointMax = 50;
	public static int EvolutionUnlockTeamLevel = 15;
	public static int EvolutionMaxLevel = 25;
	public static Defined.COST_TYPE EvolutionCostType = Defined.COST_TYPE.Money;
	public static int EvolutionCost = 30000;

	// Upgrades.xml config values
	public static int WeaponSkillRows = 30;
	public static int WeaponStarting = 1000, WeaponIncrease = 50, WeaponIncreaseGrowth = 20;
	public static int SkillStarting = 300, SkillIncrease = 10, SkillIncreaseGrowth = 10;
	public static int CombatStarting = 200, CombatIncrease = 70;
	private static Dictionary<int, BreakthroughConfig> s_breakthroughs = new Dictionary<int, BreakthroughConfig>();
	public static int HelmetStarting = 300, HelmetIncrease = 15, HelmetCombat = 60;
	public static int ArmorStarting = 500, ArmorIncrease = 25, ArmorCombat = 100;
	public static int OrnamentStarting = 200, OrnamentIncrease = 10, OrnamentCombat = 40;
	private static EquipSlotConfig[] s_equipSlots;

	// Rewards.xml config values
	public static int ThreeStarMultiplier = 2;
	public static int FirstThreeStarCrystals = 5;
	public static int TutorialRewardMoney = 1000;
	public static int TutorialRewardCrystals = 50;
	public static int StageRewardRows = 150;
	public static int CoinsStarting = 1500, CoinsStartIncreasing = 10, CoinsIncrease = 5;
	public static int StageXPStarting = 510, StageXPStartIncreasing = 280, StageXPIncrease = 20;
	public static int StageCombatStarting = 350, StageCombatStartIncreasing = 40, StageCombatIncrease = 20;
	public static int TeamXPRows = 100;
	public static int TeamXPStarting = 510, TeamXPStartIncreasing = 240, TeamXPIncrease = 240, TeamXPIncreaseGrowth = 30;

	static EconomyConfig()
	{
		EnsureLoaded();
	}

	public static void EnsureLoaded()
	{
		if (s_loaded)
		{
			return;
		}
		s_loaded = true;
		LoadHeroes();
		LoadTeam();
		LoadUpgrades();
		LoadRewards();
	}

	public static List<HeroUnlock> Heroes
	{
		get
		{
			EnsureLoaded();
			return s_heroes;
		}
	}

	public static HeroUnlock GetHeroUnlock(int heroIndex)
	{
		EnsureLoaded();
		return s_heroes.Find(h => h.heroIndex == heroIndex);
	}

	public static BreakthroughConfig GetBreakthrough(int star)
	{
		EnsureLoaded();
		BreakthroughConfig config;
		if (s_breakthroughs.TryGetValue(star, out config) || s_breakthroughs.TryGetValue(1, out config))
		{
			return config;
		}
		return new BreakthroughConfig();
	}

	public static EquipSlotConfig GetEquipSlot(int slotIndex)
	{
		EnsureLoaded();
		if (slotIndex >= 0 && slotIndex < s_equipSlots.Length)
		{
			return s_equipSlots[slotIndex];
		}
		return new EquipSlotConfig();
	}

	public static string TalentUnlockCondition
	{
		get { EnsureLoaded(); return "Team Level " + TalentUnlockTeamLevel + " Required"; }
	}

	public static string EvolutionUnlockCondition
	{
		get { EnsureLoaded(); return "Team Level " + EvolutionUnlockTeamLevel + " Required"; }
	}

	public static void ApplyToSave()
	{
		EnsureLoaded();
		DataSave save = DataCenter.Save();

		foreach (PlayerData hero in save.GetHeroList())
		{
			if (hero.state != Defined.ItemState.Available)
			{
				HeroUnlock unlock = GetHeroUnlock(hero.heroIndex);
				if (unlock != null && unlock.state == Defined.ItemState.Available)
				{
					hero.state = Defined.ItemState.Available;
				}
				else if (unlock != null)
				{
					hero.state = unlock.state;
					hero.unlockNeedTeamLevel = unlock.teamLevel;
					hero.costType = unlock.costType;
					hero.unlockCost = unlock.cost;
				}
			}
			ApplyEquipment(hero.upgradeData);
		}

		TeamData team = save.GetTeamData();
		if (team != null)
		{
			team.teamMaxLevel = TeamMaxLevel;
			if (team.teamSitesData != null)
			{
				for (int i = 0; i < team.teamSitesData.Length && i < Sites.Length; i++)
				{
					if (team.teamSitesData[i].state != Defined.ItemState.Available)
					{
						team.teamSitesData[i].state = Sites[i].state;
						team.teamSitesData[i].unlockSitePrice = Sites[i].cost;
					}
				}
			}
		}

		DataSave.TeamAttributeSaveData attributes = save.teamAttributeSaveData;
		if (attributes != null)
		{
			attributes.teamGeniusResetCostCrystalPerTimes = TalentResetCostCrystal;
			attributes.teamAttributeExtraPointCost = ExtraPointCostCrystal;
			attributes.teamAttributeExtraPointMax = ExtraPointMax;
			attributes.teamGeniusUnlockCondition = TalentUnlockCondition;
			attributes.teamEvolutionUnlockCondition = EvolutionUnlockCondition;
			if (attributes.teamAttributeEvolve != null)
			{
				foreach (TeamAttributeData evolve in attributes.teamAttributeEvolve)
				{
					evolve.unlockLevel = EvolutionUnlockTeamLevel;
					evolve.maxLevel = EvolutionMaxLevel;
					evolve.costType = EvolutionCostType;
					evolve.cost = EvolutionCost;
				}
			}
		}
	}

	public static int GetEquipUnlockMoney(List<int[]> table, int slotIndex)
	{
		return table[slotIndex][slotIndex == 0 ? 1 : 0];
	}

	private static void ApplyEquipment(UpgradeData upgradeData)
	{
		if (upgradeData == null)
		{
			return;
		}
		if (EquipUpgradeCalcTest.helmets == null || EquipUpgradeCalcTest.armors == null || EquipUpgradeCalcTest.ornaments == null)
		{
			EquipUpgradeCalcTest.Init();
		}
		ApplyEquipmentType(upgradeData.helmsUpgrade, EquipUpgradeCalcTest.helmets);
		ApplyEquipmentType(upgradeData.ArmorsUpgrade, EquipUpgradeCalcTest.armors);
		ApplyEquipmentType(upgradeData.ornamentsUpgrade, EquipUpgradeCalcTest.ornaments);
	}

	private static void ApplyEquipmentType(EquipUpgradeData[] equips, List<int[]> table)
	{
		if (equips == null)
		{
			return;
		}
		for (int i = 0; i < equips.Length && i < table.Count; i++)
		{
			EquipUpgradeData equip = equips[i];
			if (equip == null)
			{
				continue;
			}
			if (equip.state == Defined.ItemState.Available)
			{
				if (equip.level < equip.maxLevel && equip.level + 1 < 6)
				{
					equip.cost = table[i][equip.level + 1];
				}
			}
			else
			{
				EquipSlotConfig slot = GetEquipSlot(i);
				equip.unlockNeedTeamLevel = slot.teamLevel;
				equip.unlockCrystal = slot.crystal;
				equip.unlockMoney = GetEquipUnlockMoney(table, i);
				equip.cost = equip.unlockMoney;
			}
		}
	}

	private static void LoadHeroes()
	{
		s_heroes = new List<HeroUnlock>();
		XmlElement root = LoadRoot("Configs/Heroes");
		if (root == null)
		{
			return;
		}
		foreach (XmlElement heroElement in root.GetElementsByTagName("Hero"))
		{
			HeroUnlock unlock = new HeroUnlock();
			unlock.heroIndex = Int(heroElement, "index", 0);
			unlock.order = unlock.heroIndex;
			XmlElement unlockElement = heroElement.SelectSingleNode("Unlock") as XmlElement;
			if (unlockElement != null)
			{
				unlock.order = Int(unlockElement, "order", unlock.heroIndex);
				unlock.startSite = Int(unlockElement, "startSite", -1);
				unlock.state = Enum(unlockElement, "state", Defined.ItemState.Purchase);
				unlock.teamLevel = Int(unlockElement, "teamLevel", 0);
				unlock.costType = Enum(unlockElement, "costType", Defined.COST_TYPE.Crystal);
				unlock.cost = Int(unlockElement, "cost", 0);
			}
			s_heroes.Add(unlock);
		}
		s_heroes.Sort((a, b) => a.order != b.order ? a.order.CompareTo(b.order) : a.heroIndex.CompareTo(b.heroIndex));
	}

	private static void LoadTeam()
	{
		Sites = new SiteConfig[]
		{
			new SiteConfig { state = Defined.ItemState.Available },
			new SiteConfig { state = Defined.ItemState.Available },
			new SiteConfig { state = Defined.ItemState.Purchase, cost = 150 },
			new SiteConfig { state = Defined.ItemState.Purchase, cost = 300 },
			new SiteConfig { state = Defined.ItemState.Purchase, cost = 300 }
		};
		XmlElement root = LoadRoot("Configs/Team");
		if (root == null)
		{
			return;
		}
		TeamMaxLevel = Int(root, "maxLevel", TeamMaxLevel);
		foreach (XmlElement site in root.GetElementsByTagName("Site"))
		{
			int index = Int(site, "index", -1);
			if (index >= 0 && index < Sites.Length)
			{
				Sites[index].state = Enum(site, "state", Sites[index].state);
				Sites[index].cost = Int(site, "cost", Sites[index].cost);
			}
		}
		XmlElement talents = root.SelectSingleNode("Talents") as XmlElement;
		if (talents != null)
		{
			TalentUnlockTeamLevel = Int(talents, "unlockTeamLevel", TalentUnlockTeamLevel);
			TalentResetCostCrystal = Int(talents, "resetCostCrystal", TalentResetCostCrystal);
			ExtraPointCostCrystal = Int(talents, "extraPointCostCrystal", ExtraPointCostCrystal);
			ExtraPointMax = Int(talents, "extraPointMax", ExtraPointMax);
		}
		XmlElement evolution = root.SelectSingleNode("Evolution") as XmlElement;
		if (evolution != null)
		{
			EvolutionUnlockTeamLevel = Int(evolution, "unlockTeamLevel", EvolutionUnlockTeamLevel);
			EvolutionMaxLevel = Int(evolution, "maxLevel", EvolutionMaxLevel);
			EvolutionCostType = Enum(evolution, "costType", EvolutionCostType);
			EvolutionCost = Int(evolution, "cost", EvolutionCost);
		}
	}

	private static void LoadUpgrades()
	{
		s_breakthroughs = new Dictionary<int, BreakthroughConfig>
		{
			{ 1, new BreakthroughConfig { crystal = 10, teamLevel = 10 } },
			{ 2, new BreakthroughConfig { crystal = 20, teamLevel = 15 } },
			{ 3, new BreakthroughConfig { crystal = 30, teamLevel = 20 } },
			{ 4, new BreakthroughConfig { crystal = 40, teamLevel = 25 } }
		};
		s_equipSlots = new EquipSlotConfig[11];
		for (int i = 0; i < s_equipSlots.Length; i++)
		{
			s_equipSlots[i] = new EquipSlotConfig
			{
				teamLevel = i > 0 ? i > 2 ? i > 6 ? 24 : 16 : 8 : 0,
				crystal = i > 2 ? i > 6 ? 20 : 10 : 0
			};
		}
		XmlElement root = LoadRoot("Configs/Upgrades");
		if (root == null)
		{
			return;
		}
		XmlElement weaponSkill = root.SelectSingleNode("WeaponSkill") as XmlElement;
		if (weaponSkill != null)
		{
			WeaponSkillRows = Int(weaponSkill, "rows", WeaponSkillRows);
			WeaponStarting = Int(weaponSkill, "weaponStarting", WeaponStarting);
			WeaponIncrease = Int(weaponSkill, "weaponIncrease", WeaponIncrease);
			WeaponIncreaseGrowth = Int(weaponSkill, "weaponIncreaseGrowth", WeaponIncreaseGrowth);
			SkillStarting = Int(weaponSkill, "skillStarting", SkillStarting);
			SkillIncrease = Int(weaponSkill, "skillIncrease", SkillIncrease);
			SkillIncreaseGrowth = Int(weaponSkill, "skillIncreaseGrowth", SkillIncreaseGrowth);
			CombatStarting = Int(weaponSkill, "combatStarting", CombatStarting);
			CombatIncrease = Int(weaponSkill, "combatIncrease", CombatIncrease);
		}
		foreach (XmlElement breakthrough in root.GetElementsByTagName("Breakthrough"))
		{
			int star = Int(breakthrough, "star", 0);
			if (star > 0)
			{
				s_breakthroughs[star] = new BreakthroughConfig
				{
					crystal = Int(breakthrough, "crystal", 10),
					teamLevel = Int(breakthrough, "teamLevel", 10)
				};
			}
		}
		XmlElement equipment = root.SelectSingleNode("Equipment") as XmlElement;
		if (equipment != null)
		{
			HelmetStarting = Int(equipment, "helmetStarting", HelmetStarting);
			HelmetIncrease = Int(equipment, "helmetIncrease", HelmetIncrease);
			HelmetCombat = Int(equipment, "helmetCombat", HelmetCombat);
			ArmorStarting = Int(equipment, "armorStarting", ArmorStarting);
			ArmorIncrease = Int(equipment, "armorIncrease", ArmorIncrease);
			ArmorCombat = Int(equipment, "armorCombat", ArmorCombat);
			OrnamentStarting = Int(equipment, "ornamentStarting", OrnamentStarting);
			OrnamentIncrease = Int(equipment, "ornamentIncrease", OrnamentIncrease);
			OrnamentCombat = Int(equipment, "ornamentCombat", OrnamentCombat);
			foreach (XmlElement slot in equipment.GetElementsByTagName("Slot"))
			{
				int index = Int(slot, "index", -1);
				if (index >= 0 && index < s_equipSlots.Length)
				{
					s_equipSlots[index].teamLevel = Int(slot, "teamLevel", s_equipSlots[index].teamLevel);
					s_equipSlots[index].crystal = Int(slot, "crystal", s_equipSlots[index].crystal);
				}
			}
		}
	}

	private static void LoadRewards()
	{
		XmlElement root = LoadRoot("Configs/Rewards");
		if (root == null)
		{
			return;
		}
		XmlElement stars = root.SelectSingleNode("Stars") as XmlElement;
		if (stars != null)
		{
			ThreeStarMultiplier = Int(stars, "threeStarMultiplier", ThreeStarMultiplier);
			FirstThreeStarCrystals = Int(stars, "firstThreeStarCrystals", FirstThreeStarCrystals);
		}
		XmlElement tutorial = root.SelectSingleNode("Tutorial") as XmlElement;
		if (tutorial != null)
		{
			TutorialRewardMoney = Int(tutorial, "money", TutorialRewardMoney);
			TutorialRewardCrystals = Int(tutorial, "crystal", TutorialRewardCrystals);
		}
		XmlElement stage = root.SelectSingleNode("StageRewards") as XmlElement;
		if (stage != null)
		{
			StageRewardRows = Int(stage, "rows", StageRewardRows);
			CoinsStarting = Int(stage, "coinsStarting", CoinsStarting);
			CoinsStartIncreasing = Int(stage, "coinsStartIncreasing", CoinsStartIncreasing);
			CoinsIncrease = Int(stage, "coinsIncrease", CoinsIncrease);
			StageXPStarting = Int(stage, "xpStarting", StageXPStarting);
			StageXPStartIncreasing = Int(stage, "xpStartIncreasing", StageXPStartIncreasing);
			StageXPIncrease = Int(stage, "xpIncrease", StageXPIncrease);
			StageCombatStarting = Int(stage, "combatStarting", StageCombatStarting);
			StageCombatStartIncreasing = Int(stage, "combatStartIncreasing", StageCombatStartIncreasing);
			StageCombatIncrease = Int(stage, "combatIncrease", StageCombatIncrease);
		}
		XmlElement experience = root.SelectSingleNode("TeamExperience") as XmlElement;
		if (experience != null)
		{
			TeamXPRows = Int(experience, "rows", TeamXPRows);
			TeamXPStarting = Int(experience, "xpStarting", TeamXPStarting);
			TeamXPStartIncreasing = Int(experience, "xpStartIncreasing", TeamXPStartIncreasing);
			TeamXPIncrease = Int(experience, "xpIncrease", TeamXPIncrease);
			TeamXPIncreaseGrowth = Int(experience, "xpIncreaseGrowth", TeamXPIncreaseGrowth);
		}
	}

	private static XmlElement LoadRoot(string resourcePath)
	{
		string text = FileUtil.LoadResourcesFile(resourcePath);
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogWarning("EconomyConfig: " + resourcePath + " not found, using built-in defaults.");
			return null;
		}
		try
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(text);
			return document.DocumentElement;
		}
		catch (Exception e)
		{
			Debug.LogError("EconomyConfig: could not parse " + resourcePath + ", using built-in defaults. " + e.Message);
			return null;
		}
	}

	private static int Int(XmlElement element, string attribute, int fallback)
	{
		int value;
		string text = element.GetAttribute(attribute);
		if (!string.IsNullOrEmpty(text) && int.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
		{
			return value;
		}
		return fallback;
	}

	private static T Enum<T>(XmlElement element, string attribute, T fallback) where T : struct
	{
		T value;
		string text = element.GetAttribute(attribute);
		if (!string.IsNullOrEmpty(text) && System.Enum.TryParse(text.Trim(), true, out value))
		{
			return value;
		}
		return fallback;
	}
}

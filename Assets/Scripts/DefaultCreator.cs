using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zweronz.SavingSystem
{
	public static class Creator
	{
		public static SaveData Create()
		{
			return DefaultCreator.Create<SaveDefaultCreator>().saveData;
		}
	}

	public static class DefaultCreator
	{
		public static T Create<T>() where T : IDefaultCreator<T>
		{
			return Activator.CreateInstance<T>().Create();
		}
	}

	public interface IDefaultCreator<T>
	{
		T Create();
	}

	public class SaveDefaultCreator : IDefaultCreator<SaveDefaultCreator>
	{
		public SaveDefaultCreator Create()
		{
			return new SaveDefaultCreator
			{
				saveData = new SaveData
				{
					currency = DefaultCreator.Create<CurrencyDefaultCreator>().currency,
					teamSave = DefaultCreator.Create<TeamDefaultCreator>().teamSave,
					heroes = DefaultCreator.Create<HeroDefaultCreator>().playerData,
					worldNodes = DefaultCreator.Create<WorldNodeDefaultCreator>().gameProgressData,
					achievements = DefaultCreator.Create<AchievementDefaultCreator>().achievements,
					battleTutorialFinished = false,
					bNewUser = true,
					playMusic = true,
					playSound = true
				}
			};
		}

		public SaveData saveData;
	}

	public class AchievementDefaultCreator : IDefaultCreator<AchievementDefaultCreator>
	{
		public AchievementDefaultCreator Create()
		{
			return new AchievementDefaultCreator
			{
				achievements = new AchievementSave()
			};
		}

		public AchievementSave achievements;
	}

	public class TeamDefaultCreator : IDefaultCreator<TeamDefaultCreator>
	{
		public TeamDefaultCreator Create()
		{
            TeamDefaultCreator creator = new TeamDefaultCreator
            {
                teamSave = new TeamSave()
            };

            creator.teamSave.teamData = new TeamData
			{
				teamSitesData = System.Array.ConvertAll(EconomyConfig.Sites, site => new TeamSiteData { state = site.state, unlockSitePrice = site.cost }),

				teamExp = 0,
				teamLevel = 1,

				teamMaxLevel = EconomyConfig.TeamMaxLevel,
				teamMaxExp = ExperienceCalcTest.XPStarting,

				talents = new Dictionary<CoMDS2.TeamSpecialAttribute.TeamAttributeType, int>
				{
				},

				evolves = new Dictionary<CoMDS2.TeamSpecialAttribute.TeamAttributeEvolveType, int>
				{
				}
			};

			TeamAttributeData[] geniusList = new TeamAttributeData[25], evolutionList = new TeamAttributeData[10];

			for (int i = 0; i < 25; i++)
			{
				int tier = i / 5;
				int unlockPoint = tier * 5;
				Defined.ItemState startState = (tier == 0) ? Defined.ItemState.Purchase : Defined.ItemState.Locked;

				geniusList[i] = new TeamAttributeData()
				{
					index = i,
					level = 0,
					maxLevel = 5,
					state = startState,
					unlockPoint = unlockPoint
				};
			}

			for (int i = 0; i < 10; i++)
			{
				evolutionList[i] = new TeamAttributeData() { index = i, level = 0, maxLevel = EconomyConfig.EvolutionMaxLevel, state = Defined.ItemState.Locked, unlockLevel = EconomyConfig.EvolutionUnlockTeamLevel, costType = EconomyConfig.EvolutionCostType, cost = EconomyConfig.EvolutionCost };
			}

			creator.teamSave.teamAttributeSaveData = new DataSave.TeamAttributeSaveData
			{
				teamAttributeTalent = geniusList,
				teamAttributeEvolve = evolutionList,

				teamGeniusResetCostCrystalPerTimes = EconomyConfig.TalentResetCostCrystal,
				teamAttributeExtraPointCost = EconomyConfig.ExtraPointCostCrystal,
				teamAttributeExtraPointMax = EconomyConfig.ExtraPointMax,

				teamGeniusUnlockCondition = EconomyConfig.TalentUnlockCondition,
				teamEvolutionUnlockCondition = EconomyConfig.EvolutionUnlockCondition
			};

			return creator;
		}

		public TeamSave teamSave;
	}

	public class HeroDefaultCreator : IDefaultCreator<HeroDefaultCreator>
	{
		public HeroDefaultCreator Create()
		{
			HeroDefaultCreator creator = new HeroDefaultCreator
			{
				playerData = new List<PlayerData>()
			};

			foreach (EconomyConfig.HeroUnlock unlock in EconomyConfig.Heroes)
			{
				PlayerData hero = new PlayerData
				{
					heroIndex = unlock.heroIndex,
					state = unlock.state
				};
				if (unlock.startSite >= 0)
				{
					hero.siteNum = unlock.startSite;
				}
				if (unlock.state != Defined.ItemState.Available)
				{
					hero.unlockNeedTeamLevel = unlock.teamLevel;
					hero.costType = unlock.costType;
					hero.unlockCost = unlock.cost;
				}
				creator.playerData.Add(hero);
			}

			foreach (PlayerData hero in creator.playerData)
			{
				hero.equips = new System.Collections.Generic.Dictionary<Defined.EQUIP_TYPE, UserEquipData>
				{
					{Defined.EQUIP_TYPE.Acc, new UserEquipData { currEquipIndex = 22, currEquipLevel = 1 } },
					{Defined.EQUIP_TYPE.Body, new UserEquipData { currEquipIndex = 11, currEquipLevel = 1 } },
					{Defined.EQUIP_TYPE.Head, new UserEquipData { currEquipIndex = 0, currEquipLevel = 1 } }
				};

                hero.upgradeData = new UpgradeData
                {
                    helmsUpgrade = new EquipUpgradeData[11],
					ArmorsUpgrade = new EquipUpgradeData[11],
                    ornamentsUpgrade = new EquipUpgradeData[11]
                };

				hero.weaponLevel = 2;
				hero.skillLevel = 2;

				hero.weaponStar = 1;
				hero.skillStar = 1;

				//placeholder
				hero.weaponMaxLevel = 5;
				hero.skillMaxLevel = 5;

                for (int i = 0; i < 33; i++)
				{
					if (i < 11)
					{
						hero.upgradeData.helmsUpgrade[i] = new EquipUpgradeData() { index = i + 1, equipIndex = i, level = 0, state = Defined.ItemState.Locked, costType = Defined.COST_TYPE.Money, maxLevel = 5, unlockNeedTeamLevel = EconomyConfig.GetEquipSlot(i).teamLevel };
					}
					else if (i < 22)
					{
						hero.upgradeData.ArmorsUpgrade[i - 11] = new EquipUpgradeData() { index = i - 10, equipIndex = i, level = 0, state = Defined.ItemState.Locked, costType = Defined.COST_TYPE.Money, maxLevel = 5, unlockNeedTeamLevel = EconomyConfig.GetEquipSlot(i - 11).teamLevel };
					}
					else
					{
						hero.upgradeData.ornamentsUpgrade[i - 22] = new EquipUpgradeData() { index = i - 21, equipIndex = i, level = 0, state = Defined.ItemState.Locked, costType = Defined.COST_TYPE.Money, maxLevel = 5, unlockNeedTeamLevel = EconomyConfig.GetEquipSlot(i - 22).teamLevel };
					}
				}

				hero.upgradeData.helmsUpgrade[0].state = Defined.ItemState.Available;
				hero.upgradeData.ArmorsUpgrade[0].state = Defined.ItemState.Available;
				hero.upgradeData.ornamentsUpgrade[0].state = Defined.ItemState.Available;

				hero.upgradeData.helmsUpgrade[0].canUpgrade = true;
				hero.upgradeData.ArmorsUpgrade[0].canUpgrade = true;
				hero.upgradeData.ornamentsUpgrade[0].canUpgrade = true;

				if (EquipUpgradeCalcTest.helmets == null || EquipUpgradeCalcTest.armors == null || EquipUpgradeCalcTest.ornaments == null)
				{
					EquipUpgradeCalcTest.Init();
				}

				for (int i = 0; i < 11; i++)
				{
					hero.upgradeData.helmsUpgrade[i].cost = EconomyConfig.GetEquipUnlockMoney(EquipUpgradeCalcTest.helmets, i);
					hero.upgradeData.ArmorsUpgrade[i].cost = EconomyConfig.GetEquipUnlockMoney(EquipUpgradeCalcTest.armors, i);
					hero.upgradeData.ornamentsUpgrade[i].cost = EconomyConfig.GetEquipUnlockMoney(EquipUpgradeCalcTest.ornaments, i);

					hero.upgradeData.helmsUpgrade[i].unlockMoney = EconomyConfig.GetEquipUnlockMoney(EquipUpgradeCalcTest.helmets, i);
					hero.upgradeData.ArmorsUpgrade[i].unlockMoney = EconomyConfig.GetEquipUnlockMoney(EquipUpgradeCalcTest.armors, i);
					hero.upgradeData.ornamentsUpgrade[i].unlockMoney = EconomyConfig.GetEquipUnlockMoney(EquipUpgradeCalcTest.ornaments, i);

					hero.upgradeData.helmsUpgrade[i].unlockCrystal = EconomyConfig.GetEquipSlot(i).crystal;
					hero.upgradeData.ArmorsUpgrade[i].unlockCrystal = EconomyConfig.GetEquipSlot(i).crystal;
					hero.upgradeData.ornamentsUpgrade[i].unlockCrystal = EconomyConfig.GetEquipSlot(i).crystal;

					hero.upgradeData.helmsUpgrade[i].combat = EquipUpgradeCalcTest.helmets[i][6];
					hero.upgradeData.ArmorsUpgrade[i].combat = EquipUpgradeCalcTest.armors[i][6];
					hero.upgradeData.ornamentsUpgrade[i].combat = EquipUpgradeCalcTest.ornaments[i][6];
				}

				if (UpgradeCalcTest.upgrades == null)
				{
					UpgradeCalcTest.Init();
				}

				hero.upgradeData.weaponCombat = EconomyConfig.CombatStarting;
				hero.upgradeData.weaponCanUpgrade = true;

				hero.upgradeData.weaponUpgradeCost = UpgradeCalcTest.upgrades[hero.weaponLevel + (hero.weaponStar - 1)][0];

				hero.upgradeData.skillCanUpgrade = true;
				hero.upgradeData.skillUpgradeCost = UpgradeCalcTest.upgrades[hero.skillLevel + (hero.skillStar - 1)][1];
			}

			return creator;
		}

		public HeroDefaultCreator() {}

		public List<PlayerData> playerData;
	}

	public class CurrencyDefaultCreator : IDefaultCreator<CurrencyDefaultCreator>
	{
		public CurrencyDefaultCreator Create()
		{
			return new CurrencyDefaultCreator
			{
				currency = new Currency
				{
					money = 0,
					crystal = 0
				}
			};
		}

		public CurrencyDefaultCreator() {}

		public Currency currency;
	}

	public class WorldNodeDefaultCreator : IDefaultCreator<WorldNodeDefaultCreator>
	{
		public WorldNodeDefaultCreator Create()
		{
			return new WorldNodeDefaultCreator
			{
				gameProgressData = new List<GameProgressData>
				{
					GameProgressController.CreateProgress()
				}
			};
		}

		public List<GameProgressData> gameProgressData;
	}
}
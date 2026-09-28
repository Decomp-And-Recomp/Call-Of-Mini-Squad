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
				teamSitesData = new TeamSiteData[]
				{
					new TeamSiteData { state = Defined.ItemState.Available }, new TeamSiteData { state = Defined.ItemState.Available },
					new TeamSiteData { state = Defined.ItemState.Purchase, unlockSitePrice = 150 }, new TeamSiteData { state = Defined.ItemState.Purchase, unlockSitePrice = 300 }, new TeamSiteData { state = Defined.ItemState.Purchase, unlockSitePrice = 300 }
				},

				teamExp = 0,
				teamLevel = 1,

				teamMaxLevel = 50,
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
				evolutionList[i] = new TeamAttributeData() { index = i, level = 0, maxLevel = 25, state = Defined.ItemState.Locked, unlockLevel = 15, costType = Defined.COST_TYPE.Money, cost = 30000 };
			}

			creator.teamSave.teamAttributeSaveData = new DataSave.TeamAttributeSaveData
			{
				teamAttributeTalent = geniusList,
				teamAttributeEvolve = evolutionList,

				teamGeniusResetCostCrystalPerTimes = 20,
				teamAttributeExtraPointCost = 10,
				teamAttributeExtraPointMax = 50,

				teamGeniusUnlockCondition = "Team Level 10 Required",
				teamEvolutionUnlockCondition = "Team Level 15 Required"
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
				playerData = new List<PlayerData>
				{
					new PlayerData
					{
						heroIndex = 0,
						siteNum = 0,
						state = Defined.ItemState.Available,
					},
					new PlayerData
					{
						heroIndex = 1,
						siteNum = 1,
						state = Defined.ItemState.Available,
					},
					new PlayerData
					{
						heroIndex = 9,
						state = Defined.ItemState.Locked,
						unlockNeedTeamLevel = 10,
						costType = Defined.COST_TYPE.Money,
						unlockCost = 10000
					},
					new PlayerData
					{
						heroIndex = 7,
						state = Defined.ItemState.Locked,
						unlockNeedTeamLevel = 20,
						costType = Defined.COST_TYPE.Money,
						unlockCost = 20000
					},
					new PlayerData
					{
						heroIndex = 3,
						state = Defined.ItemState.Locked,
						unlockNeedTeamLevel = 30,
						costType = Defined.COST_TYPE.Money,
						unlockCost = 30000
					},
					new PlayerData
					{
						heroIndex = 13,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 11,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 10,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 149
					},
					new PlayerData
					{
						heroIndex = 14,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 2,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 6,
                        state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 399
                    },
					new PlayerData
					{
						heroIndex = 12,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 149
					},
					new PlayerData
					{
						heroIndex = 15,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 17,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 16,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Money,
						unlockCost = 75000
					},
					new PlayerData
					{
						heroIndex = 4,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
					new PlayerData
					{
						heroIndex = 5,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 299
					},
						new PlayerData
					{
						heroIndex = 8,
						state = Defined.ItemState.Purchase,
						costType = Defined.COST_TYPE.Crystal,
						unlockCost = 449
					},
				}
			};

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
						hero.upgradeData.helmsUpgrade[i] = new EquipUpgradeData() { index = i + 1, equipIndex = i, level = 0, state = Defined.ItemState.Locked, costType = Defined.COST_TYPE.Money, maxLevel = 5, unlockNeedTeamLevel = i > 0 ? i > 2 ? i > 6 ? 24 : 16 : 8 : 0 };
					}
					else if (i < 22)
					{
						hero.upgradeData.ArmorsUpgrade[i - 11] = new EquipUpgradeData() { index = i - 10, equipIndex = i, level = 0, state = Defined.ItemState.Locked, costType = Defined.COST_TYPE.Money, maxLevel = 5, unlockNeedTeamLevel = i - 11 > 0 ? i - 11 > 2 ? i - 11 > 6 ? 24 : 16 : 8 : 0  };
					}
					else
					{
						hero.upgradeData.ornamentsUpgrade[i - 22] = new EquipUpgradeData() { index = i - 21, equipIndex = i, level = 0, state = Defined.ItemState.Locked, costType = Defined.COST_TYPE.Money, maxLevel = 5, unlockNeedTeamLevel = i - 22 > 0 ? i - 22 > 2 ? i - 22 > 6 ? 24 : 16 : 8 : 0 };
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
					hero.upgradeData.helmsUpgrade[i].cost = EquipUpgradeCalcTest.helmets[i][i == 0 ? 1 : 0];
					hero.upgradeData.ArmorsUpgrade[i].cost = EquipUpgradeCalcTest.armors[i][i == 0 ? 1 : 0];
					hero.upgradeData.ornamentsUpgrade[i].cost = EquipUpgradeCalcTest.ornaments[i][i == 0 ? 1 : 0];

					hero.upgradeData.helmsUpgrade[i].unlockMoney = EquipUpgradeCalcTest.helmets[i][i == 0 ? 1 : 0];
					hero.upgradeData.ArmorsUpgrade[i].unlockMoney = EquipUpgradeCalcTest.armors[i][i == 0 ? 1 : 0];
					hero.upgradeData.ornamentsUpgrade[i].unlockMoney = EquipUpgradeCalcTest.ornaments[i][i == 0 ? 1 : 0];

					hero.upgradeData.helmsUpgrade[i].unlockCrystal = i > 2 ? i > 6 ? 20 : 10 : 0;
					hero.upgradeData.ArmorsUpgrade[i].unlockCrystal = i > 2 ? i > 6 ? 20 : 10 : 0;
					hero.upgradeData.ornamentsUpgrade[i].unlockCrystal = i > 2 ? i > 6 ? 20 : 10 : 0;

					hero.upgradeData.helmsUpgrade[i].combat = EquipUpgradeCalcTest.helmets[i][6];
					hero.upgradeData.ArmorsUpgrade[i].combat = EquipUpgradeCalcTest.armors[i][6];
					hero.upgradeData.ornamentsUpgrade[i].combat = EquipUpgradeCalcTest.ornaments[i][6];
				}

				hero.upgradeData.weaponCombat = UpgradeCalcTest.CombatStarting;
				hero.upgradeData.weaponCanUpgrade = true;

				hero.upgradeData.weaponUpgradeCost = UpgradeCalcTest.WeaponStarting + UpgradeCalcTest.WeaponIncrease;

				hero.upgradeData.skillCanUpgrade = true;
				hero.upgradeData.skillUpgradeCost = UpgradeCalcTest.SkillStarting + UpgradeCalcTest.SkillIncrease;
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
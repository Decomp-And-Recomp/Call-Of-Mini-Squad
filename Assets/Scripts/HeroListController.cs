using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HeroListController
{
	public static void Refresh()
	{
		EconomyConfig.ApplyToSave();
		UpgradeController.RefreshWeapons();
		UpgradeController.RefreshSkills();
		UpgradeController.RefreshEquipment();

		TeamBuffController.Refresh();

		foreach (PlayerData hero in DataCenter.Save().GetHeroList())
		{
			if (hero.state == Defined.ItemState.Locked && hero.unlockNeedTeamLevel > 0 && DataCenter.Save().GetTeamData().teamLevel >= hero.unlockNeedTeamLevel)
			{
				hero.state = Defined.ItemState.Purchase; 
			}

			hero.combat = hero.upgradeData.weaponCombat + hero.upgradeData.helmsUpgrade.ToList().Find(x => x.equipIndex == hero.equips[Defined.EQUIP_TYPE.Head].currEquipIndex).combat + hero.upgradeData.ArmorsUpgrade.ToList().Find(x => x.equipIndex == hero.equips[Defined.EQUIP_TYPE.Body].currEquipIndex).combat + hero.upgradeData.ornamentsUpgrade.ToList().Find(x => x.equipIndex == hero.equips[Defined.EQUIP_TYPE.Acc].currEquipIndex).combat;
		}

		TeamLevelController.OnReceivedXP();
	}

	public static int BuyHero()
	{
		PlayerData hero = DataCenter.Save().GetPlayerData(DataCenter.State().selectHeroIndex);

		if (hero.costType == Defined.COST_TYPE.Money && DataCenter.Save().Money >= hero.unlockCost)
		{
			DataCenter.Save().Money -= hero.unlockCost;
			hero.state = Defined.ItemState.Available;
			Save.RequestWrite();

			return 0;
		}
		if (hero.costType == Defined.COST_TYPE.Crystal && DataCenter.Save().Crystal >= hero.unlockCost)
		{
			DataCenter.Save().Crystal -= hero.unlockCost;
			hero.state = Defined.ItemState.Available;
			Save.RequestWrite();

			return 0;
		}

		return -1;
	}

	public static int BuySite()
	{
		if (DataCenter.Save().Crystal >= DataCenter.Save().GetTeamData().teamSitesData[DataCenter.State().selectTeamSiteIndex].unlockSitePrice)
		{
			DataCenter.Save().Crystal -= DataCenter.Save().GetTeamData().teamSitesData[DataCenter.State().selectTeamSiteIndex].unlockSitePrice;
			DataCenter.Save().GetTeamData().teamSitesData[DataCenter.State().selectTeamSiteIndex].state = Defined.ItemState.Available;
			Save.RequestWrite();

			return 0;
		}

		return -1;
	}

	public static void Equip()
	{
		PlayerData hero = DataCenter.Save().GetPlayerData(DataCenter.State().selectHeroIndex);
		EquipUpgradeData[] upgradeData = DataCenter.State().selectEquipType == Defined.EQUIP_TYPE.Head ? hero.upgradeData.helmsUpgrade : DataCenter.State().selectEquipType == Defined.EQUIP_TYPE.Body ? hero.upgradeData.ArmorsUpgrade : hero.upgradeData.ornamentsUpgrade;

		hero.equips[DataCenter.State().selectEquipType].currEquipIndex = DataCenter.State().selectEquipIndex - 1 + (DataCenter.State().selectEquipType == Defined.EQUIP_TYPE.Body ? 11 : DataCenter.State().selectEquipType == Defined.EQUIP_TYPE.Acc ? 22 : 0);
		hero.equips[DataCenter.State().selectEquipType].currEquipLevel = upgradeData[DataCenter.State().selectEquipIndex - 1].level;

		Refresh();
		Save.RequestWrite();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zweronz.SavingSystem
{
	public static class Saver
	{
		public static SaveData Save()
		{
			return TypeSaver.Save<SaveSaver>() as SaveData;
		}
	}
	
	public static class TypeSaver
	{
		public static object Save<T>() where T : ISaver
		{
			return Activator.CreateInstance<T>().Save();
		}
	}
	
	public interface ISaver
	{
		object Save();
	}
	
	public class SaveSaver : ISaver
	{
		public object Save()
		{
	        return new SaveData
	        {
	            heroes = TypeSaver.Save<HeroSaver>() as List<PlayerData>,
	            teamSave = TypeSaver.Save<TeamSaver>() as TeamSave,
	            currency = TypeSaver.Save<CurrencySaver>() as Currency,
	            worldNodes = TypeSaver.Save<WorldNodeSaver>() as List<GameProgressData>,
	            achievements = TypeSaver.Save<AchievementSaver>() as AchievementSave,
	            battleTutorialFinished = DataCenter.Save().BattleTutorialFinished,
	            bNewUser = DataCenter.Save().bNewUser,
	            playMusic = DataCenter.Save().PlayMusic,
	            playSound = DataCenter.Save().PlaySound,
	            cameraView = (int)DataCenter.Save().CameraView,
	            lastLoginTime = DataCenter.Save().lastLoginTime,
	            tutorialChangeMode = DataCenter.Save().tutorialChangeMode
	        };
	    }
	}

	public class AchievementSaver : ISaver
	{
		public object Save()
		{
			DataSave s = DataCenter.Save();
			return new AchievementSave
			{
				totalKills = s.achTotalKills,
				iapCrystals = s.achIapCrystals,
				dailyKills = s.achDailyKills,
				dailyStages = s.achDailyStages,
				dailyResetDate = s.achDailyResetDate ?? string.Empty,
				claimed = new List<string>(s.achClaimed)
			};
		}
	}
	
	public class HeroSaver : ISaver
	{
		public object Save()
		{
			return DataCenter.Save().GetHeroList().ToList();
		}
	}
	
	public class TeamSaver : ISaver
	{
		public object Save()
		{
			return new TeamSave
			{
				teamData = DataCenter.Save().GetTeamData(),
				teamAttributeSaveData = DataCenter.Save().teamAttributeSaveData
			};
		}
	}
	
	public class CurrencySaver : ISaver
	{
		public object Save()
		{
			return new Currency
			{
				money = DataCenter.Save().Money,
				crystal = DataCenter.Save().Crystal
			};
		}
	}
	
	public class WorldNodeSaver : ISaver
	{
		public object Save()
		{
			return DataCenter.Save().GetWorldNodeProgress();
		}
	}
}
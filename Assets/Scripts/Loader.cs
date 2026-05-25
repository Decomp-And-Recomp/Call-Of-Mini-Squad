using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zweronz.SavingSystem
{
	public static class Loader
	{
		public static void Load(SaveData save)
		{
			TypeLoader.Load<SaveLoader>(save);
		}
	}

	public static class TypeLoader
	{
		public static void Load<T>(object obj) where T : ILoader
		{
			Activator.CreateInstance<T>().Load(obj);
		}
	}

	public interface ILoader
	{
		void Load(object obj);
	}

	public class SaveLoader : ILoader
	{
		public void Load(object obj)
		{
			SaveData loadObject = obj as SaveData;

			TypeLoader.Load<TeamLoader>(loadObject.teamSave);
			TypeLoader.Load<HeroLoader>(loadObject.heroes);
			TypeLoader.Load<CurrencyLoader>(loadObject.currency);
			TypeLoader.Load<WorldNodeLoader>(loadObject.worldNodes);
			TypeLoader.Load<AchievementLoader>(loadObject.achievements);
			DataCenter.Save().BattleTutorialFinished = loadObject.battleTutorialFinished;
			DataCenter.Save().bNewUser = loadObject.bNewUser;
		}

		public SaveLoader() {}
	}

	public class AchievementLoader : ILoader
	{
		public void Load(object obj)
		{
			AchievementSave a = obj as AchievementSave;
			DataSave s = DataCenter.Save();
			if (a == null)
			{
				s.achTotalKills = 0;
				s.achIapCrystals = 0;
				s.achDailyKills = 0;
				s.achDailyStages = 0;
				s.achDailyResetDate = string.Empty;
				s.achClaimed = new System.Collections.Generic.HashSet<string>();
				return;
			}
			s.achTotalKills = a.totalKills;
			s.achIapCrystals = a.iapCrystals;
			s.achDailyKills = a.dailyKills;
			s.achDailyStages = a.dailyStages;
			s.achDailyResetDate = a.dailyResetDate ?? string.Empty;
			s.achClaimed = (a.claimed != null) ? new System.Collections.Generic.HashSet<string>(a.claimed) : new System.Collections.Generic.HashSet<string>();
		}
	}

	public class TeamLoader : ILoader
	{
		public void Load(object obj)
		{
			TeamSave loadObject = obj as TeamSave;

			DataCenter.Save().SetTeamData(loadObject.teamData);
			DataCenter.Save().teamAttributeSaveData = loadObject.teamAttributeSaveData;
		}

		public TeamLoader() {}
	}

	public class HeroLoader : ILoader
	{
		public void Load(object obj)
		{
			List<PlayerData> loadObject = obj as List<PlayerData>;

			DataCenter.Save().RemoveAllHeroes();

			foreach (PlayerData hero in loadObject)
			{
				DataCenter.Save().AddHero(hero);

				if (hero.siteNum != -1)
				{
					DataCenter.Save().SetHeroOnTeamSite(hero, (Defined.TEAM_SITE)hero.siteNum);
				}
			}

			MigrateMissingHeroes();
		}

		private void MigrateMissingHeroes()
		{
			HashSet<int> existing = new HashSet<int>();
			foreach (PlayerData h in DataCenter.Save().GetHeroList())
			{
				if (h != null) existing.Add(h.heroIndex);
			}

			HeroDefaultCreator defaults = DefaultCreator.Create<HeroDefaultCreator>();
			int added = 0;
			foreach (PlayerData defaultHero in defaults.playerData)
			{
				if (!existing.Contains(defaultHero.heroIndex))
				{
					DataCenter.Save().AddHero(defaultHero);
					added++;
				}
			}
		}
	}

	public class CurrencyLoader : ILoader
	{
		public void Load(object obj)
		{
			Currency loadObject = obj as Currency;

			DataCenter.Save().Money = loadObject.money;
			DataCenter.Save().Crystal = loadObject.crystal;
		}
	}

	public class WorldNodeLoader : ILoader
	{
		public void Load(object obj)
		{
			List<GameProgressData> loadObject = obj as List<GameProgressData>;

			DataCenter.Save().SetWorldNodeProgress(loadObject);
		}
	}
}

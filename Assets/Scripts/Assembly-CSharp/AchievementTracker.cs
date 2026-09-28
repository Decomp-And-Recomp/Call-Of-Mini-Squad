using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class AchievementTracker
{
    private static Dictionary<int, int[]> s_stageCountCache;

    private static int GetTotalStagesInWorld(int worldIndex, Defined.LevelMode mode)
    {
        if (s_stageCountCache == null)
        {
            s_stageCountCache = new Dictionary<int, int[]>();
        }
        if (!s_stageCountCache.ContainsKey(worldIndex))
        {
            int[] counts = new int[3];
            string text = FileUtil.LoadResourcesFile("Configs/gamelevel/GameLevel_" + (worldIndex + 1).ToString("D3"));
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(text);
                    foreach (XmlElement modeElem in doc.DocumentElement.GetElementsByTagName("LevelMode"))
                    {
                        int modeIdx = int.Parse(modeElem.GetAttribute("mode"));
                        if (modeIdx >= 0 && modeIdx < 3)
                        {
                            counts[modeIdx] = modeElem.GetElementsByTagName("LevelNode").Count;
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.Log("Idk why but failed to parse stage count for world " + worldIndex.ToString() + ", mode " + mode.ToString().ToLower() + ". Defaulting to 0.");
                }
            }
            s_stageCountCache[worldIndex] = counts;
        }
        return s_stageCountCache[worldIndex][(int)mode];
    }

    public static void EnsureDailyReset()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        DataSave save = DataCenter.Save();
        if (save.achDailyResetDate != today)
        {
            save.achDailyKills = 0;
            save.achDailyStages = 0;
            save.achDailyResetDate = today;
            List<string> toRemove = null;
            foreach (string id in save.achClaimed)
            {
                if (id != null && id.StartsWith("daily_"))
                {
                    if (toRemove == null) toRemove = new List<string>();
                    toRemove.Add(id);
                }
            }
            if (toRemove != null)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    save.achClaimed.Remove(toRemove[i]);
                }
            }
        }
    }

    public static void OnZombieKilled()
    {
        EnsureDailyReset();
        DataSave save = DataCenter.Save();
        save.achTotalKills++;
        save.achDailyKills++;
        Save.MarkDirty();
    }

    public static void OnStageCleared()
    {
        EnsureDailyReset();
        DataCenter.Save().achDailyStages++;
        Save.MarkDirty();
    }

    public static void OnIapCrystalPurchased(int amount)
    {
        DataCenter.Save().achIapCrystals += amount;
        Save.RequestWrite();
    }

    private static int GetTeamLevel()
    {
        TeamData td = DataCenter.Save().GetTeamData();
        return (td != null) ? td.teamLevel : 0;
    }

    private static int GetHeroCount()
    {
        PlayerData[] heroes = DataCenter.Save().GetHeroList();
        if (heroes == null)
        {
            return 0;
        }
        int count = 0;
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null && heroes[i].state == Defined.ItemState.Available)
            {
                count++;
            }
        }
        return count;
    }

    private static int GetBattlePoints()
    {
        TeamData td = DataCenter.Save().GetTeamData();
        return (td != null) ? td.GetTeamCombat() : 0;
    }

    private static int GetWeaponMaxedCount()
    {
        PlayerData[] heroes = DataCenter.Save().GetHeroList();
        if (heroes == null)
        {
            return 0;
        }
        int count = 0;
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null && heroes[i].weaponMaxLevel > 0 && heroes[i].weaponLevel >= heroes[i].weaponMaxLevel)
            {
                count++;
            }
        }
        return count;
    }

    private static int GetSkillMaxedCount()
    {
        PlayerData[] heroes = DataCenter.Save().GetHeroList();
        if (heroes == null)
        {
            return 0;
        }
        int count = 0;
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null && heroes[i].skillMaxLevel > 0 && heroes[i].skillLevel >= heroes[i].skillMaxLevel)
            {
                count++;
            }
        }
        return count;
    }

    private static int GetAnyEquipMaxed(int slot)
    {
        PlayerData[] heroes = DataCenter.Save().GetHeroList();
        if (heroes == null) return 0;
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] == null) continue;
            if (heroes[i].upgradeData == null) continue;
            EquipUpgradeData[] upgrades = null;
            switch (slot)
            {
                case 0: upgrades = heroes[i].upgradeData.helmsUpgrade; break;
                case 1: upgrades = heroes[i].upgradeData.ArmorsUpgrade; break;
                case 2: upgrades = heroes[i].upgradeData.ornamentsUpgrade; break;
            }
            if (upgrades == null) continue;
            for (int j = 0; j < upgrades.Length; j++)
            {
                EquipUpgradeData equip = upgrades[j];
                if (equip == null) continue;
                if (equip.maxLevel > 0 && equip.level >= equip.maxLevel)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    private static int GetChapterClearedCount(int worldIndex, Defined.LevelMode mode)
    {
        GameProgressData wpd = DataCenter.Save().GetWorldProgressData(worldIndex);
        if (wpd == null || wpd.levelStars == null || !wpd.levelStars.ContainsKey(mode))
        {
            return 0;
        }
        int total = GetTotalStagesInWorld(worldIndex, mode);
        if (total <= 0)
        {
            return 0;
        }
        ushort[] stars = wpd.levelStars[mode];
        int cleared = 0;
        for (int i = 0; i < total && i < stars.Length; i++)
        {
            if (stars[i] > 0) cleared++;
        }
        return (cleared >= total) ? 1 : 0;
    }

    private static int GetChapter3StarredCount(int worldIndex, Defined.LevelMode mode)
    {
        GameProgressData wpd = DataCenter.Save().GetWorldProgressData(worldIndex);
        if (wpd == null || wpd.levelStars == null || !wpd.levelStars.ContainsKey(mode))
        {
            return 0;
        }
        int total = GetTotalStagesInWorld(worldIndex, mode);
        if (total <= 0)
        {
            return 0;
        }
        ushort[] stars = wpd.levelStars[mode];
        int starred = 0;
        for (int i = 0; i < total && i < stars.Length; i++)
        {
            if (stars[i] >= 3) starred++;
        }
        return (starred >= total) ? 1 : 0;
    }

    private static int GetCounterValue(string counter, string counterArg)
    {
        DataSave save = DataCenter.Save();
        int worldArg;
        switch (counter)
        {
            case "total_kills": return save.achTotalKills;
            case "iap_crystals": return save.achIapCrystals;
            case "daily_kills": return save.achDailyKills;
            case "daily_stages": return save.achDailyStages;
            case "team_level": return GetTeamLevel();
            case "hero_count": return GetHeroCount();
            case "battle_points": return GetBattlePoints();
            case "weapon_maxed": return GetWeaponMaxedCount();
            case "skill_maxed": return GetSkillMaxedCount();
            case "helmet_maxed": return GetAnyEquipMaxed(0);
            case "armor_maxed": return GetAnyEquipMaxed(1);
            case "ornament_maxed": return GetAnyEquipMaxed(2);
            case "bind_account": return 1;
            case "clear_normal": return int.TryParse(counterArg, out worldArg) ? GetChapterClearedCount(worldArg, Defined.LevelMode.Normal) : 0;
            case "clear_hard": return int.TryParse(counterArg, out worldArg) ? GetChapterClearedCount(worldArg, Defined.LevelMode.Hard) : 0;
            case "clear_hell": return int.TryParse(counterArg, out worldArg) ? GetChapterClearedCount(worldArg, Defined.LevelMode.Hell) : 0;
            case "3star_normal": return int.TryParse(counterArg, out worldArg) ? GetChapter3StarredCount(worldArg, Defined.LevelMode.Normal) : 0;
            case "3star_hard": return int.TryParse(counterArg, out worldArg) ? GetChapter3StarredCount(worldArg, Defined.LevelMode.Hard) : 0;
            case "3star_hell": return int.TryParse(counterArg, out worldArg) ? GetChapter3StarredCount(worldArg, Defined.LevelMode.Hell) : 0;
            default: return 0;
        }
    }

    public static void RefreshAllProgress()
    {
        EnsureDailyReset();
        DataSave save = DataCenter.Save();
        Dictionary<string, AchievementData> map = DataCenter.Conf().GetAchievementDataMap();
        if (map.Count == 0)
        {
            DataCenter.Conf().LoadAchievementsFromDisk();
            map = DataCenter.Conf().GetAchievementDataMap();
        }
        foreach (KeyValuePair<string, AchievementData> kvp in map)
        {
            AchievementData data = kvp.Value;
            int progress = string.IsNullOrEmpty(data.counter) ? 0 : GetCounterValue(data.counter, data.counterArg);
            data.scheduleMin = Mathf.Min(progress, data.scheduleMax);
            if (save.achClaimed.Contains(data.id))
            {
                data.state = 2;
            }
            else
            {
                data.state = data.scheduleMax > 0 && data.scheduleMin >= data.scheduleMax ? 1 : 0;
            }
        }
    }

    public static void MarkClaimed(string questId)
    {
        DataCenter.Save().achClaimed.Add(questId);
        Save.RequestWrite();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Zweronz.SavingSystem;

public static class Save
{
	public static string Path
	{
		get
		{
			return Application.persistentDataPath + "/game.save";
		}
	}

	public static string BackupPath
	{
		get { return Path + ".bak"; }
	}

	public static string TempPath
	{
		get { return Path + ".tmp"; }
	}

	public static bool TestingCreate
	{
		get
		{
			return false;
		}
	}

	private static bool canWrite;

	public static void Write()
	{
		if (!canWrite)
		{
			Debug.LogWarning("Write skipped — save is in read-only emergency mode (load previously failed for both primary and backup).");
			return;
		}

		try
		{
			string json = JsonConvert.SerializeObject(saveInstance = Saver.Save());
			File.WriteAllText(TempPath, json);

			if (File.Exists(Path))
			{
				File.Copy(Path, BackupPath, true);
				File.Delete(Path);
			}
			File.Move(TempPath, Path);
		}
		catch (Exception e)
		{
			Debug.LogError("Write failed: " + e);
			try { if (File.Exists(TempPath)) File.Delete(TempPath); } catch { }
		}
	}

	public static void Load()
	{
		canWrite = false;

		if (TryLoadFromFile(Path))
		{
			canWrite = true;
			return;
		}

		if (File.Exists(BackupPath) && TryLoadFromFile(BackupPath))
		{
			Debug.LogWarning("Primary save unreadable — restored from backup.");
			try { File.Copy(BackupPath, Path, true); } catch (Exception e) { Debug.LogError("Could not copy backup over primary: " + e); }
			canWrite = true;
			return;
		}

		Debug.LogError("Both primary and backup save are unreadable. Running with defaults, autosave DISABLED. Restore a save manually to re-enable writes.");
		Loader.Load(saveInstance = Creator.Create());
	}

	private static bool TryLoadFromFile(string path)
	{
		try
		{
			string json = File.ReadAllText(path);
			SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
			if (data == null)
			{
				Debug.LogError("Deserialization of " + path + " returned null.");
				return false;
			}
			Loader.Load(saveInstance = data);
			return true;
		}
		catch (Exception e)
		{
			Debug.LogError("Load from " + path + " failed: " + e.Message);
			return false;
		}
	}

	public static void Create()
	{
		Loader.Load(saveInstance = Creator.Create());
		canWrite = true;
	}

	public static void Delete()
	{
		try { if (File.Exists(Path)) File.Delete(Path); } catch { }
		try { if (File.Exists(BackupPath)) File.Delete(BackupPath); } catch { }
		Application.Quit();
	}

	public static bool NeedsCreate
	{
		get
		{
			return !File.Exists(Path);
		}
	}

	private static SaveData saveInstance;

	public static SaveData SaveData
	{
		get
		{
			if (saveInstance == null)
			{
				Load();
			}

			return saveInstance;
		}
	}
}

public class SaveData
{
	public Currency currency;

	public TeamSave teamSave;

	public List<PlayerData> heroes;

	public List<GameProgressData> worldNodes;

	public AchievementSave achievements;

	public bool battleTutorialFinished;

	public bool bNewUser;
}

public class Currency
{
	public int money, crystal;
}

public class TeamSave
{
	public TeamData teamData;

	public DataSave.TeamAttributeSaveData teamAttributeSaveData;
}

public class AchievementSave
{
	public int totalKills;
	public int iapCrystals;
	public int dailyKills;
	public int dailyStages;
	public string dailyResetDate = string.Empty;
	public List<string> claimed = new List<string>();
}
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

	private static bool dirty;

	private static bool writeRequested;

	public static bool IsDirty
	{
		get { return dirty || writeRequested; }
	}

	public static bool IsWriteRequested
	{
		get { return writeRequested; }
	}

	public static void MarkDirty()
	{
		dirty = true;
	}

	public static void RequestWrite()
	{
		dirty = true;
		writeRequested = true;
	}

	public static void Flush()
	{
		if (IsDirty)
		{
			Write();
		}
	}

	public static void Write()
	{
		if (!canWrite)
		{
			dirty = false;
			writeRequested = false;
			Debug.LogWarning("Write skipped — save is in read-only emergency mode (load previously failed for both primary and backup).");
			return;
		}

		dirty = false;
		writeRequested = false;

		try
		{
			string json = JsonConvert.SerializeObject(saveInstance = Saver.Save());
			byte[] encrypted = SaveCrypto.Encrypt(json);
			WriteDurable(TempPath, encrypted);

			if (File.Exists(Path))
			{
				SwapInTemp();
			}
			else
			{
				File.Move(TempPath, Path);
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Write failed: " + e);
			dirty = true;
			try { if (File.Exists(Path) && File.Exists(TempPath)) File.Delete(TempPath); } catch { }
		}
	}

	private static void WriteDurable(string path, byte[] bytes)
	{
		using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
		{
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush(true);
		}
	}

	private static void SwapInTemp()
	{
		try
		{
			File.Replace(TempPath, Path, BackupPath);
			return;
		}
		catch (PlatformNotSupportedException) { }
		catch (NotSupportedException) { }
		File.Copy(Path, BackupPath, true);
		File.Delete(Path);
		File.Move(TempPath, Path);
	}

	public static void Load()
	{
		canWrite = false;

		if (File.Exists(Path) && TryLoadFromFile(Path))
		{
			canWrite = true;
			return;
		}
		if (TryRecoverFrom(TempPath, "temp") || TryRecoverFrom(BackupPath, "backup"))
		{
			canWrite = true;
			return;
		}

		Debug.LogError("Both primary and backup save are unreadable. Running with defaults, autosave DISABLED. Restore a save manually to re-enable writes.");
		Loader.Load(saveInstance = Creator.Create());
	}

	private static bool TryRecoverFrom(string path, string label)
	{
		if (!File.Exists(path) || !TryLoadFromFile(path))
		{
			return false;
		}

		Debug.LogWarning("Primary save missing or unreadable — restored from " + label + ".");
		try { File.Copy(path, Path, true); } catch (Exception e) { Debug.LogError("Could not copy " + label + " over primary: " + e); }
		return true;
	}

	private static bool TryLoadFromFile(string path)
	{
		try
		{
			byte[] bytes = File.ReadAllBytes(path);
			string json;
			try
			{
				json = SaveCrypto.Decrypt(bytes);
			}
			catch (Exception decryptException)
			{
				string plaintext = File.ReadAllText(path);
				SaveData plaintextData = JsonConvert.DeserializeObject<SaveData>(plaintext);
				if (plaintextData == null)
				{
					Debug.LogError("Decrypt of " + path + " failed (" + decryptException.Message + ") and plaintext fallback also returned null.");
					return false;
				}
				Loader.Load(saveInstance = plaintextData);
				return true;
			}

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
		try { if (File.Exists(TempPath)) File.Delete(TempPath); } catch { }
		Application.Quit();
	}

	public static bool NeedsCreate
	{
		get
		{
			return !File.Exists(Path) && !File.Exists(TempPath) && !File.Exists(BackupPath);
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

	public bool playMusic;

	public bool playSound;

	public int cameraView;

	public float lastLoginTime;

	public bool tutorialChangeMode;
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
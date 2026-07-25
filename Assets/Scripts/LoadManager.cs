using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		if (Save.TestingCreate || Save.NeedsCreate)
		{
			string oldSavePath = Application.persistentDataPath + "/GameData.dat";
			bool didMigrate = false;
			if (System.IO.File.Exists(oldSavePath))
			{
				DataCenter.Save().LoadGameData();
				bool oldTutorial = DataCenter.Save().BattleTutorialFinished;
				bool oldNewUser = DataCenter.Save().bNewUser;
				bool oldTutorialChangeMode = DataCenter.Save().tutorialChangeMode;
				bool oldPlayMusic = DataCenter.Save().PlayMusic;
				bool oldPlaySound = DataCenter.Save().PlaySound;
				Defined.CameraView oldCameraView = DataCenter.Save().CameraView;
				float oldLastLoginTime = DataCenter.Save().lastLoginTime;

				Save.Create();
				DataCenter.Save().BattleTutorialFinished = oldTutorial;
				DataCenter.Save().bNewUser = oldNewUser;
				DataCenter.Save().tutorialChangeMode = oldTutorialChangeMode;
				DataCenter.Save().PlayMusic = oldPlayMusic;
				DataCenter.Save().PlaySound = oldPlaySound;
				DataCenter.Save().CameraView = oldCameraView;
				DataCenter.Save().lastLoginTime = oldLastLoginTime;
				didMigrate = true;
			}
			else
			{
				Save.Create();
			}
			Save.Write();

			if (didMigrate)
			{
				try { System.IO.File.Delete(oldSavePath); }
				catch (System.Exception e) { Debug.LogWarning("Could not delete legacy GameData.dat: " + e.Message); }
			}
		}
		else
		{
			Save.Load();
		}
	}

	private void OnApplicationQuit()
	{
		Save.Write();
	}
}

using System.Collections.Generic;
using KamcordJSON;
using UnityEngine;

public class KamcordImplementationAndroid : Kamcord.Implementation
{
	private AndroidJavaClass kamcordJavaClass_;

	private static bool beginDrawErrorOnce;

	private bool frameCaptured;

	private static bool endDrawErrorOnce;

	private AndroidJavaClass kamcordJavaClass()
	{
		if (kamcordJavaClass_ == null)
		{
			kamcordJavaClass_ = new AndroidJavaClass("com.kamcord.android.Kamcord");
		}
		if (kamcordJavaClass_ == null)
		{
		}
		return kamcordJavaClass_;
	}

	public override void SetLoggingEnabled(bool value)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("setLoggingEnabled", value);
		}
	}

	public override bool IsEnabled()
	{
		if (kamcordJavaClass() != null)
		{
			return kamcordJavaClass().CallStatic<bool>("isEnabled", new object[0]);
		}
		return false;
	}

	public override string GetDisabledReason()
	{
		if (kamcordJavaClass() != null)
		{
			return kamcordJavaClass().CallStatic<string>("getDisabledReason", new object[0]);
		}
		return "Kamcord java class object not accessible from Unity script.";
	}

	public override void WhitelistBoard(string boardName)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("whitelistBoard", boardName);
		}
	}

	public override void BlacklistBoard(string boardName)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("blacklistBoard", boardName);
		}
	}

	public override void WhitelistDevice(string deviceName)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("whitelistDevice", deviceName);
		}
	}

	public override void BlacklistDevice(string deviceName)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("blacklistDevice", deviceName);
		}
	}

	public override void WhitelistBoard(string boardName, int sdkVersion)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("whitelistBoard", boardName, sdkVersion);
		}
	}

	public override void BlacklistBoard(string boardName, int sdkVersion)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("blacklistBoard", boardName, sdkVersion);
		}
	}

	public override void WhitelistDevice(string deviceName, int sdkVersion)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("whitelistDevice", deviceName, sdkVersion);
		}
	}

	public override void BlacklistDevice(string deviceName, int sdkVersion)
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("blacklistDevice", deviceName, sdkVersion);
		}
	}

	public override void WhitelistAllBoards()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("whitelistAllBoards");
		}
	}

	public override void BlacklistAllBoards()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("blacklistAllBoards");
		}
	}

	public override void WhitelistAll()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("whitelistAll");
		}
	}

	public override void BlacklistAll()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("blacklistAll");
		}
	}

	public override string GetBoard()
	{
		if (kamcordJavaClass() == null)
		{
			return string.Empty;
		}
		return kamcordJavaClass().CallStatic<string>("getBoard", new object[0]);
	}

	public override string GetDevice()
	{
		if (kamcordJavaClass() == null)
		{
			return string.Empty;
		}
		return kamcordJavaClass().CallStatic<string>("getDevice", new object[0]);
	}

	public override bool IsWhitelisted(string boardName)
	{
		if (kamcordJavaClass() == null)
		{
			return false;
		}
		return kamcordJavaClass().CallStatic<bool>("isWhitelisted", new object[1] { boardName });
	}

	public override bool IsWhitelisted()
	{
		if (kamcordJavaClass() == null)
		{
			return false;
		}
		return kamcordJavaClass().CallStatic<bool>("isWhitelisted", new object[0]);
	}

	public override void DoneChangingWhitelist()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("doneChangingWhitelist");
		}
	}

	public override void SetVideoTitle(string title)
	{
		kamcordJavaClass().CallStatic("setDefaultVideoTitle", title);
	}

	public override void SetYouTubeSettings(string description, string tags)
	{
		kamcordJavaClass().CallStatic("setDefaultYoutubeDescription", description);
		kamcordJavaClass().CallStatic("setDefaultYoutubeKeywords", tags);
	}

	public override void SetFacebookDescription(string facebookDescription)
	{
		kamcordJavaClass().CallStatic("setDefaultFacebookDescription", facebookDescription);
	}

	public override void SetDefaultTweet(string tweet)
	{
		kamcordJavaClass().CallStatic("setDefaultTweet", tweet);
	}

	public override void SetTwitterDescription(string twitterDescription)
	{
		kamcordJavaClass().CallStatic("setDefaultTwitterDescription", twitterDescription);
	}

	public override void SetDefaultEmailSubject(string subject)
	{
		kamcordJavaClass().CallStatic("setDefaultEmailSubject", subject);
	}

	public override void SetDefaultEmailBody(string body)
	{
		kamcordJavaClass().CallStatic("setDefaultEmailBody", body);
	}

	public override void SetShareTargets(Kamcord.ShareTarget target1, Kamcord.ShareTarget target2, Kamcord.ShareTarget target3, Kamcord.ShareTarget target4)
	{
		kamcordJavaClass().CallStatic("setShareTargets", new int[4]
		{
			(int)target1,
			(int)target2,
			(int)target3,
			(int)target4
		});
	}

	public override void SetVideoMetadata(Dictionary<string, object> metadata)
	{
		if (metadata != null && metadata.Count > 0)
		{
			kamcordJavaClass().CallStatic("setVideoMetadata", Json.Serialize(metadata));
		}
	}

	public override string Version()
	{
		return kamcordJavaClass().CallStatic<string>("version", new object[0]);
	}

	public override void SetLevelAndScore(string level, double score)
	{
		kamcordJavaClass().CallStatic("setLevel", level);
		kamcordJavaClass().CallStatic("setScore", score);
	}

	public override void SetDeveloperMetadata(Kamcord.MetadataType metadataType, string displayKey, string displayValue)
	{
		kamcordJavaClass().CallStatic("setDeveloperMetadata", (int)metadataType, displayKey, displayValue);
	}

	public override void SetDeveloperMetadataWithNumericValue(Kamcord.MetadataType metadataType, string displayKey, string displayValue, double numericValue)
	{
		kamcordJavaClass().CallStatic("setDeveloperMetadataWithNumericValue", (int)metadataType, displayKey, displayValue, numericValue);
	}

	public override void BeginDraw()
	{
		frameCaptured = false;
		if (kamcordJavaClass() == null)
		{
			if (!beginDrawErrorOnce)
			{
				beginDrawErrorOnce = true;
			}
		}
		else
		{
			kamcordJavaClass().CallStatic("beginDraw");
		}
	}

	public override void EndDraw()
	{
		if (kamcordJavaClass() == null)
		{
			if (!endDrawErrorOnce)
			{
				endDrawErrorOnce = true;
			}
		}
		else if (!frameCaptured)
		{
			kamcordJavaClass().CallStatic("endDraw");
		}
	}

	public override void StartRecording()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("startRecording");
		}
	}

	public override void StopRecording()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("stopRecording");
		}
	}

	public override void Pause()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("pauseRecording");
		}
	}

	public override void Resume()
	{
		if (kamcordJavaClass() != null)
		{
			kamcordJavaClass().CallStatic("resumeRecording");
		}
	}

	public override bool IsRecording()
	{
		if (kamcordJavaClass() == null)
		{
			return false;
		}
		return kamcordJavaClass().CallStatic<bool>("isRecording", new object[0]);
	}

	public override bool IsPaused()
	{
		if (kamcordJavaClass() == null)
		{
			return false;
		}
		return kamcordJavaClass().CallStatic<bool>("isPaused", new object[0]);
	}

	public override void SetVideoQuality(Kamcord.VideoQuality quality)
	{
		kamcordJavaClass().CallStatic("setVideoQuality", quality);
	}

	public override bool VoiceOverlayEnabled()
	{
		return kamcordJavaClass().CallStatic<bool>("voiceOverlayEnabled", new object[0]);
	}

	public override bool VoiceOverlayActivated()
	{
		return kamcordJavaClass().CallStatic<bool>("voiceOverlayActivated", new object[0]);
	}

	public override void SetVoiceOverlayEnabled(bool enabled)
	{
		kamcordJavaClass().CallStatic("setVoiceOverlayEnabled", enabled);
	}

	public override void ActivateVoiceOverlay(bool activate)
	{
		kamcordJavaClass().CallStatic("setVoiceOverlayActivated", activate);
	}

	public override void SetVideoIncompleteWarningEnabled(bool enabled)
	{
		kamcordJavaClass().CallStatic("setVideoIncompleteWarningEnabled", enabled);
	}

	public override bool IsVideoComplete()
	{
		return kamcordJavaClass().CallStatic<bool>("isVideoComplete", new object[0]);
	}

	public override void CaptureFrame()
	{
		EndDraw();
		frameCaptured = true;
	}

	public override void ShowView()
	{
		kamcordJavaClass().CallStatic("showView");
	}

	public override void ShowWatchView()
	{
		kamcordJavaClass().CallStatic("showWatchView");
	}

	public override void Init(string devKey, string devSecret, string appName, Kamcord.VideoQuality videoQuality)
	{
		if (kamcordJavaClass() != null)
		{
			int num = 1;
			num = ((videoQuality != 0 && videoQuality == Kamcord.VideoQuality.Trailer) ? 1 : 0);
			AndroidJavaObject @static = UnityEngine.Android.AndroidApplication.currentActivity;
			kamcordJavaClass().CallStatic("initActivity", @static);
			kamcordJavaClass().CallStatic("initKeyAndSecret", devKey, devSecret, appName, num);
		}
	}

	public override void Awake(Kamcord kamcordInstance)
	{
		if (kamcordJavaClass() != null)
		{
			InitializeRenderCamera("Pre");
			InitializeRenderCamera("Post");
		}
	}

	public override void Start(Kamcord kamcordInstance)
	{
		Kamcord.CallAdjustAndroidWhitelist();
		SetLoggingEnabled(Kamcord.loggingEnabled_);
		if (kamcordJavaClass() != null)
		{
			Init(kamcordInstance.developerKey, kamcordInstance.developerSecret, kamcordInstance.appName, kamcordInstance.videoQuality);
		}
	}

	public override void SetAudioSettings(int sampleRate, int numChannels)
	{
		kamcordJavaClass().CallStatic("setAudioSettings", sampleRate, numChannels);
	}

	public override void WriteAudioData(float[] data, int length)
	{
		kamcordJavaClass().CallStatic("writeAudioData", data, length);
	}

	private void InitializeRenderCamera(string type)
	{
		if ((type.Equals("Pre") || type.Equals("Post")) && GameObject.Find("kamcord" + type + "Camera") == null)
		{
			GameObject gameObject = new GameObject();
			Camera camera = (Camera)gameObject.AddComponent<Camera>();
			camera.name = "kamcord" + type + "Camera";
			camera.clearFlags = CameraClearFlags.Nothing;
			camera.cullingMask = 0;
			if (type.Equals("Pre"))
			{
				camera.depth = float.MinValue;
			}
			else
			{
				camera.depth = float.MaxValue;
			}
			if (type == "Pre")
			{
				camera.gameObject.AddComponent<KamcordAndroidPreRender>();
			}
			else
			{
				camera.gameObject.AddComponent<KamcordAndroidPostRender>();
			}
			gameObject.SetActive(true);
			Object.DontDestroyOnLoad(gameObject);
		}
	}

	public static void SetRenderCameraEnabled(string type, bool flag)
	{
		if (type.Equals("Pre") || type.Equals("Post"))
		{
			GameObject gameObject = GameObject.Find("kamcord" + type + "Camera");
			if (gameObject != null)
			{
				gameObject.SetActive(flag);
			}
		}
	}

	public static int getSDKVersion()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build$VERSION");
		return androidJavaClass.GetStatic<int>("SDK_INT");
	}
}

using System;
using System.Collections.Generic;
using KamcordJSON;
using UnityEngine;

public class Kamcord : MonoBehaviour
{
	public enum VideoQuality
	{
		Standard = 0,
		Trailer = 1
	}

	public enum MetadataType
	{
		level = 0,
		score = 1,
		list = 2,
		other = 1000
	}

	public enum ShareTarget
	{
		Facebook = 0,
		Twitter = 1,
		YouTube = 2,
		Email = 3,
		WeChat = 4,
		LINE = 5
	}

	public enum YouTubeVideoCategory
	{
		Comedy = 0,
		Education = 1,
		Entertainment = 2,
		Games = 3,
		Music = 4
	}

	[Serializable]
	public class KamcordBlacklist
	{
		public bool ipod4Gen;

		public bool ipod5Gen;

		public bool iphone3GS;

		public bool iphone4;

		public bool iphone4S;

		public bool iphone5;

		public bool iphone5c;

		public bool iphone5S;

		public bool ipad1;

		public bool ipad2;

		public bool ipadMini;

		public bool ipad3;

		public bool ipad4;

		public bool ipadAir;
	}

	public class Implementation
	{
		public virtual void SetLoggingEnabled(bool value)
		{
		}

		public virtual bool IsEnabled()
		{
			return false;
		}

		public virtual string GetDisabledReason()
		{
			return string.Empty;
		}

		public virtual void WhitelistBoard(string boardName)
		{
		}

		public virtual void BlacklistBoard(string boardName)
		{
		}

		public virtual void WhitelistDevice(string deviceName)
		{
		}

		public virtual void BlacklistDevice(string deviceName)
		{
		}

		public virtual void WhitelistBoard(string boardName, int sdkVersion)
		{
		}

		public virtual void BlacklistBoard(string boardName, int sdkVersion)
		{
		}

		public virtual void WhitelistDevice(string deviceName, int sdkVersion)
		{
		}

		public virtual void BlacklistDevice(string deviceName, int sdkVersion)
		{
		}

		public virtual void WhitelistAllBoards()
		{
		}

		public virtual void BlacklistAllBoards()
		{
		}

		public virtual void WhitelistAll()
		{
		}

		public virtual void BlacklistAll()
		{
		}

		public virtual string GetBoard()
		{
			return string.Empty;
		}

		public virtual string GetDevice()
		{
			return string.Empty;
		}

		public virtual bool IsWhitelisted(string boardName)
		{
			return false;
		}

		public virtual bool IsWhitelisted()
		{
			return false;
		}

		public virtual void DoneChangingWhitelist()
		{
		}

		public virtual void SetVideoTitle(string title)
		{
		}

		public virtual void SetYouTubeSettings(string description, string tags)
		{
		}

		public virtual void SetFacebookAppID(string facebookAppID)
		{
		}

		public virtual void SetFacebookAppIDAndShareAuth(string facebookAppID, bool useSharedAuth)
		{
		}

		public virtual void LogoutOfSharedFacebookAuth()
		{
		}

		public virtual void SetWeChatAppID(string weChatAppID)
		{
		}

		public virtual void SetFacebookDescription(string facebookDescription)
		{
		}

		public virtual void SetDefaultTweet(string tweet)
		{
		}

		public virtual void SetTwitterDescription(string twitterDescription)
		{
		}

		public virtual void SetDefaultEmailSubject(string subject)
		{
		}

		public virtual void SetDefaultEmailBody(string body)
		{
		}

		public virtual void SetShareTargets(ShareTarget target1, ShareTarget target2, ShareTarget target3, ShareTarget target4)
		{
		}

		public virtual void SetVideoMetadata(Dictionary<string, object> metadata)
		{
		}

		public virtual void SetMaxFreeDiskSpacePercentageUsage(double percentage)
		{
		}

		public virtual string Version()
		{
			return string.Empty;
		}

		public virtual void SetLevelAndScore(string level, double score)
		{
		}

		public virtual void SetDeveloperMetadata(MetadataType metadataType, string displayKey, string displayValue)
		{
		}

		public virtual void SetDeveloperMetadataWithNumericValue(MetadataType metadataType, string displayKey, string displayValue, double numericValue)
		{
		}

		public virtual bool VideoExistsWithMetadataConstraints(Dictionary<string, object> metadata)
		{
			return false;
		}

		public virtual void ShowVideoWithMetadataConstraints(Dictionary<string, object> metadata, string title)
		{
		}

		public virtual void ShowVideoWithVideoID(string videoID, string title)
		{
		}

		public virtual void BeginDraw()
		{
		}

		public virtual void EndDraw()
		{
		}

		public virtual void StartRecording()
		{
		}

		public virtual void StopRecording()
		{
		}

		public virtual void Pause()
		{
		}

		public virtual void Resume()
		{
		}

		public virtual bool IsRecording()
		{
			return false;
		}

		public virtual bool IsPaused()
		{
			return false;
		}

		public virtual bool IsViewShowing()
		{
			return false;
		}

		public virtual void Snapshot(string filename)
		{
		}

		public virtual void SetVideoQuality(VideoQuality quality)
		{
		}

		public virtual void SetUseFastRender(bool useFastRender)
		{
		}

		public virtual void SetVoiceOverlayEnabled(bool enabled)
		{
		}

		public virtual bool VoiceOverlayEnabled()
		{
			return false;
		}

		public virtual void ActivateVoiceOverlay(bool activate)
		{
		}

		public virtual bool VoiceOverlayActivated()
		{
			return false;
		}

		public virtual void CaptureFrame()
		{
		}

		public virtual void SetNotificationsEnabled(bool notificationsEnabled)
		{
		}

		public virtual void FireTestNotification()
		{
		}

		public virtual void ShowView()
		{
		}

		public virtual void ShowWatchView()
		{
		}

		public virtual void SetMaximumVideoLength(uint seconds)
		{
		}

		public virtual uint MaximumVideoLength()
		{
			return 0u;
		}

		public virtual void SetVideoFPS(uint videoFPS)
		{
		}

		public virtual uint VideoFPS()
		{
			return 0u;
		}

		public virtual void SetShouldPauseGameEngine(bool shouldPause)
		{
		}

		public virtual bool ShouldPauseGameEngine()
		{
			return false;
		}

		public virtual void UploadVideo(string title)
		{
		}

		public virtual void Disable()
		{
		}

		public virtual void TurnOffAutomaticAudioRecording(bool state)
		{
		}

		public virtual void PlayBackgroundSound(string fileName, bool loop)
		{
		}

		public virtual void Init(string devKey, string devSecret, string appName, VideoQuality videoQuality)
		{
		}

		public virtual void SetDeviceBlacklist(bool disableiPod4G, bool disableiPod5G, bool disableiPhone3GS, bool disableiPhone4, bool disableiPhone4S, bool disableiPhone5, bool disableiPhone5C, bool disableiPhone5S, bool disableiPad1, bool disableiPad2, bool disableiPadMini, bool disableiPad3, bool disableiPad4, bool disableiPadAir)
		{
		}

		public virtual void SetDefaultTitle(string title)
		{
		}

		public virtual void SetYouTubeVideoCategory(YouTubeVideoCategory category)
		{
		}

		public virtual void SetFacebookSettings(string title, string caption, string description)
		{
		}

		public virtual void SetDefaultEmailSubjectAndBody(string subject, string body)
		{
		}

		public virtual void Awake(Kamcord kamcordInstance)
		{
		}

		public virtual void Start(Kamcord kamcordInstance)
		{
		}

		public virtual void SetCrossPromoIcon(string localFileImageURL)
		{
		}

		public virtual void SetMode(int mode)
		{
		}

		public virtual void SetAgeRestrictionEnabled(bool restricted)
		{
		}

		public virtual bool IsAgeRestrictionEnabled()
		{
			return false;
		}

		public virtual void SetVideoIncompleteWarningEnabled(bool enabled)
		{
		}

		public virtual bool IsVideoComplete()
		{
			return true;
		}

		public virtual void SetAudioSettings(int sampleRate, int numChannels)
		{
		}

		public virtual void WriteAudioData(float[] data, int length)
		{
		}

		public virtual void SetFlushOnCopy(bool flush)
		{
		}
	}

	public delegate void KamcordViewDidAppear();

	public delegate void KamcordViewWillDisappear();

	public delegate void KamcordViewDidDisappear();

	public delegate void KamcordViewDidNotAppear();

	public delegate void KamcordWatchViewDidAppear();

	public delegate void KamcordWatchViewWillDisappear();

	public delegate void KamcordWatchViewDidDisappear();

	public delegate void VideoThumbnailReadyAtFilePath(string filepath);

	public delegate void ShareButtonPressed();

	public delegate void VideoWillBeginUploading(string videoID, string URL);

	public delegate void VideoUploadProgressed(string videoID, float progress);

	public delegate void VideoFinishedUploading(string videoID, bool success);

	public delegate void VideoSharedTo(string kamcordVideoID, string networkName, bool success);

	public delegate void VideoSharedToFacebook(string kamcordVideoID, bool success);

	public delegate void VideoSharedToTwitter(string kamcordVideoID, bool success);

	public delegate void VideoSharedToYoutube(string kamcordVideoID, bool success);

	public delegate void SnapshotReadyAtFilePath(string filepath);

	public delegate void PushNotifCallToActionButtonPressed();

	public delegate void AttributedKamcordInstall();

	public delegate void AdjustAndroidWhitelist();

	public delegate void IsEnabledChanged(bool isEnabled);

	public bool enableIOS;

	public bool enableAndroid = true;

	public bool enableLogging = true;

	public string developerKey = "Kamcord developer key";

	public string developerSecret = "Kamcord developer secret";

	public string appName = "Application name";

	public VideoQuality videoQuality;

	public bool enableVoiceOverlay = true;

	public bool useFastRender = true;

	public KamcordBlacklist deviceBlacklist;

	public static bool iOSEnabled_ = false;

	public static bool loggingEnabled_ = true;

	public static bool androidEnabled_ = true;

	private static Implementation implementation_;

	private static bool developerSetVoiceOverlay = false;

	public static Kamcord instance;

	protected static List<KamcordCallbackInterface> listeners = new List<KamcordCallbackInterface>();

	private static float timeScale;

	public static event KamcordViewDidAppear kamcordViewDidAppear;

	public static event KamcordViewWillDisappear kamcordViewWillDisappear;

	public static event KamcordViewDidDisappear kamcordViewDidDisappear;

	public static event KamcordViewDidNotAppear kamcordViewDidNotAppear;

	public static event KamcordWatchViewDidAppear kamcordWatchViewDidAppear;

	public static event KamcordWatchViewWillDisappear kamcordWatchViewWillDisappear;

	public static event KamcordWatchViewDidDisappear kamcordWatchViewDidDisappear;

	public static event VideoThumbnailReadyAtFilePath videoThumbnailReadyAtFilePath;

	public static event ShareButtonPressed shareButtonPressed;

	public static event VideoWillBeginUploading videoWillBeginUploading;

	public static event VideoUploadProgressed videoUploadProgressed;

	public static event VideoFinishedUploading videoFinishedUploading;

	public static event VideoSharedTo videoSharedTo;

	public static event VideoSharedToFacebook videoSharedToFacebook;

	public static event VideoSharedToTwitter videoSharedToTwitter;

	public static event VideoSharedToYoutube videoSharedToYoutube;

	public static event SnapshotReadyAtFilePath snapshotReadyAtFilePath;

	public static event PushNotifCallToActionButtonPressed pushNotifCallToActionButtonPressed;

	public static event AttributedKamcordInstall attributedKamcordInstall;

	public static event AdjustAndroidWhitelist adjustAndroidWhitelist;

	public static event IsEnabledChanged isEnabledChanged;

	public static void UnsubscribeFromAllCallbacks()
	{
		Kamcord.kamcordViewDidAppear = null;
		Kamcord.kamcordViewWillDisappear = null;
		Kamcord.kamcordViewDidDisappear = null;
		Kamcord.kamcordViewDidNotAppear = null;
		Kamcord.kamcordWatchViewDidAppear = null;
		Kamcord.kamcordWatchViewWillDisappear = null;
		Kamcord.kamcordWatchViewDidDisappear = null;
		Kamcord.videoThumbnailReadyAtFilePath = null;
		Kamcord.videoWillBeginUploading = null;
		Kamcord.videoUploadProgressed = null;
		Kamcord.videoFinishedUploading = null;
		Kamcord.videoSharedTo = null;
		Kamcord.videoSharedToFacebook = null;
		Kamcord.videoSharedToTwitter = null;
		Kamcord.videoSharedToYoutube = null;
		Kamcord.snapshotReadyAtFilePath = null;
		Kamcord.pushNotifCallToActionButtonPressed = null;
		Kamcord.adjustAndroidWhitelist = null;
		Kamcord.isEnabledChanged = null;
		ClearListeners();
	}

	protected static void CallKamcordViewDidAppear()
	{
		if (Kamcord.kamcordViewDidAppear != null)
		{
			Kamcord.kamcordViewDidAppear();
		}
	}

	protected static void CallKamcordViewDidDisappear()
	{
		if (Kamcord.kamcordViewDidDisappear != null)
		{
			Kamcord.kamcordViewDidDisappear();
		}
	}

	protected static void CallKamcordViewWillDisappear()
	{
		if (Kamcord.kamcordViewWillDisappear != null)
		{
			Kamcord.kamcordViewWillDisappear();
		}
	}

	protected static void CallKamcordViewDidNotAppear()
	{
		if (Kamcord.kamcordViewDidNotAppear != null)
		{
			Kamcord.kamcordViewDidNotAppear();
		}
	}

	protected static void CallKamcordWatchViewDidAppear()
	{
		if (Kamcord.kamcordWatchViewDidAppear != null)
		{
			Kamcord.kamcordWatchViewDidAppear();
		}
	}

	protected static void CallKamcordWatchViewDidDisappear()
	{
		if (Kamcord.kamcordWatchViewDidDisappear != null)
		{
			Kamcord.kamcordWatchViewDidDisappear();
		}
	}

	protected static void CallKamcordWatchViewWillDisappear()
	{
		if (Kamcord.kamcordWatchViewWillDisappear != null)
		{
			Kamcord.kamcordWatchViewWillDisappear();
		}
	}

	protected static void CallVideoWillBeginUploading(string videoID, string URL)
	{
		if (Kamcord.videoWillBeginUploading != null)
		{
			Kamcord.videoWillBeginUploading(videoID, URL);
		}
	}

	protected static void CallVideoUploadProgressed(string videoID, float progress)
	{
		if (Kamcord.videoUploadProgressed != null)
		{
			Kamcord.videoUploadProgressed(videoID, progress);
		}
	}

	protected static void CallVideoFinishedUploading(string videoID, bool success)
	{
		if (Kamcord.videoFinishedUploading != null)
		{
			Kamcord.videoFinishedUploading(videoID, success);
		}
	}

	protected static void CallVideoSharedTo(string videoID, string networkName, bool success)
	{
		if (Kamcord.videoSharedTo != null)
		{
			Kamcord.videoSharedTo(videoID, networkName, success);
		}
	}

	protected static void CallVideoSharedToFacebook(string videoID, bool success)
	{
		if (Kamcord.videoSharedToFacebook != null)
		{
			Kamcord.videoSharedToFacebook(videoID, success);
		}
	}

	protected static void CallVideoSharedToTwitter(string videoID, bool success)
	{
		if (Kamcord.videoSharedToTwitter != null)
		{
			Kamcord.videoSharedToTwitter(videoID, success);
		}
	}

	protected static void CallVideoSharedToYoutube(string videoID, bool success)
	{
		if (Kamcord.videoSharedToYoutube != null)
		{
			Kamcord.videoSharedToYoutube(videoID, success);
		}
	}

	protected static void CallShareButtonPressed()
	{
		if (Kamcord.shareButtonPressed != null)
		{
			Kamcord.shareButtonPressed();
		}
	}

	protected static void CallVideoThumbnailReadyAtFilePath(string url)
	{
		if (Kamcord.videoThumbnailReadyAtFilePath != null)
		{
			Kamcord.videoThumbnailReadyAtFilePath(url);
		}
	}

	protected static void CallPushNotifCallToActionButtonPressed()
	{
		if (Kamcord.pushNotifCallToActionButtonPressed != null)
		{
			Kamcord.pushNotifCallToActionButtonPressed();
		}
	}

	protected static void CallSnapshotReadyAtFilePath(string filepath)
	{
		if (Kamcord.snapshotReadyAtFilePath != null)
		{
			Kamcord.snapshotReadyAtFilePath(filepath);
		}
	}

	protected static void CallAttributedKamcordInstall()
	{
		if (Kamcord.attributedKamcordInstall != null)
		{
			Kamcord.attributedKamcordInstall();
		}
	}

	public static void CallAdjustAndroidWhitelist()
	{
		if (Kamcord.adjustAndroidWhitelist != null)
		{
			Kamcord.adjustAndroidWhitelist();
		}
	}

	public static void CallIsEnabledChanged(bool isEnabled)
	{
		if (Kamcord.isEnabledChanged != null)
		{
			Kamcord.isEnabledChanged(isEnabled);
		}
	}

	private static Implementation implementation()
	{
		//if (implementation_ == null && (iOSEnabled_ || androidEnabled_) && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) && androidEnabled_ && KamcordImplementationAndroid.getSDKVersion() >= 16)
		//{
		//	implementation_ = new KamcordImplementationAndroid();
		//}
		if (implementation_ == null)
		{
			implementation_ = new Implementation();
		}
		return implementation_;
	}

	public static void SetLoggingEnabled(bool value)
	{
		implementation().SetLoggingEnabled(value);
	}

	public static bool IsEnabled()
	{
		return implementation().IsEnabled();
	}

	public static string GetDisabledReason()
	{
		return implementation().GetDisabledReason();
	}

	public static void WhitelistBoard(string boardName)
	{
		implementation().WhitelistBoard(boardName);
	}

	public static void BlacklistBoard(string boardName)
	{
		implementation().BlacklistBoard(boardName);
	}

	public static void WhitelistDevice(string deviceName)
	{
		implementation().WhitelistDevice(deviceName);
	}

	public static void BlacklistDevice(string deviceName)
	{
		implementation().BlacklistDevice(deviceName);
	}

	public static void WhitelistBoard(string boardName, int sdkVersion)
	{
		implementation().WhitelistBoard(boardName, sdkVersion);
	}

	public static void BlacklistBoard(string boardName, int sdkVersion)
	{
		implementation().BlacklistBoard(boardName, sdkVersion);
	}

	public static void WhitelistDevice(string deviceName, int sdkVersion)
	{
		implementation().WhitelistDevice(deviceName, sdkVersion);
	}

	public static void BlacklistDevice(string deviceName, int sdkVersion)
	{
		implementation().BlacklistDevice(deviceName, sdkVersion);
	}

	public static void WhitelistAllBoards()
	{
		implementation().WhitelistAllBoards();
	}

	public static void BlacklistAllBoards()
	{
		implementation().BlacklistAllBoards();
	}

	public static void WhitelistAll()
	{
		implementation().WhitelistAll();
	}

	public static void BlacklistAll()
	{
		implementation().BlacklistAll();
	}

	public static string GetBoard()
	{
		return implementation().GetBoard();
	}

	public static bool IsWhitelisted(string boardName)
	{
		return implementation().IsWhitelisted(boardName);
	}

	public static void DoneChangingWhitelist()
	{
		implementation().DoneChangingWhitelist();
	}

	public static void SetVideoTitle(string title)
	{
		implementation().SetVideoTitle(title);
	}

	public static void SetYouTubeSettings(string description, string tags)
	{
		implementation().SetYouTubeSettings(description, tags);
	}

	public static void SetFacebookAppID(string facebookAppID)
	{
		implementation().SetFacebookAppID(facebookAppID);
	}

	public static void SetFacebookAppIDAndShareAuth(string facebookAppID, bool useSharedAuth)
	{
		implementation().SetFacebookAppIDAndShareAuth(facebookAppID, useSharedAuth);
	}

	public static void LogoutOfSharedFacebookAuth()
	{
		implementation().LogoutOfSharedFacebookAuth();
	}

	public static void SetWeChatAppID(string weChatAppID)
	{
		implementation().SetWeChatAppID(weChatAppID);
	}

	public static void SetFacebookDescription(string facebookDescription)
	{
		implementation().SetFacebookDescription(facebookDescription);
	}

	public static void SetDefaultTweet(string tweet)
	{
		implementation().SetDefaultTweet(tweet);
	}

	public static void SetTwitterDescription(string twitterDescription)
	{
		implementation().SetTwitterDescription(twitterDescription);
	}

	public static void SetDefaultEmailSubject(string subject)
	{
		implementation().SetDefaultEmailSubject(subject);
	}

	public static void SetDefaultEmailBody(string body)
	{
		implementation().SetDefaultEmailBody(body);
	}

	public static void SetShareTargets(ShareTarget target1, ShareTarget target2, ShareTarget target3, ShareTarget target4)
	{
		implementation().SetShareTargets(target1, target2, target3, target4);
	}

	public static void SetVideoMetadata(Dictionary<string, object> metadata)
	{
		implementation().SetVideoMetadata(metadata);
	}

	public static void SetDeveloperMetadata(MetadataType metadataType, string displayKey, string displayValue)
	{
		implementation().SetDeveloperMetadata(metadataType, displayKey, displayValue);
	}

	public static void SetDeveloperMetadataWithNumericValue(MetadataType metadataType, string displayKey, string displayValue, double numericValue)
	{
		implementation().SetDeveloperMetadataWithNumericValue(metadataType, displayKey, displayValue, numericValue);
	}

	public static bool VideoExistsWithMetadataConstraints(Dictionary<string, object> metadata)
	{
		return implementation().VideoExistsWithMetadataConstraints(metadata);
	}

	public static void ShowVideoWithMetadataConstraints(Dictionary<string, object> metadata, string title)
	{
		implementation().ShowVideoWithMetadataConstraints(metadata, title);
	}

	public static void ShowVideoWithVideoID(string videoID, string title)
	{
		implementation().ShowVideoWithVideoID(videoID, title);
	}

	public static void SetMaxFreeDiskSpacePercentageUsage(double percentage)
	{
		implementation().SetMaxFreeDiskSpacePercentageUsage(percentage);
	}

	public static string Version()
	{
		return implementation().Version();
	}

	public static void SetLevelAndScore(string level, double score)
	{
		implementation().SetLevelAndScore(level, score);
	}

	public static void BeginDraw()
	{
		implementation().BeginDraw();
	}

	public static void EndDraw()
	{
		implementation().EndDraw();
	}

	public static void StartRecording()
	{
		implementation().StartRecording();
	}

	public static void StopRecording()
	{
		implementation().StopRecording();
	}

	public static void Pause()
	{
		implementation().Pause();
	}

	public static void Resume()
	{
		implementation().Resume();
	}

	public static bool IsRecording()
	{
		return implementation().IsRecording();
	}

	public static bool IsPaused()
	{
		return implementation().IsPaused();
	}

	public static bool IsViewShowing()
	{
		return implementation().IsViewShowing();
	}

	public static void Snapshot(string filename)
	{
		implementation().Snapshot(filename);
	}

	public static void SetVideoQuality(VideoQuality quality)
	{
		implementation().SetVideoQuality(quality);
	}

	public static void SetVoiceOverlayEnabled(bool enabled)
	{
		implementation().SetVoiceOverlayEnabled(enabled);
		developerSetVoiceOverlay = true;
	}

	public static bool VoiceOverlayEnabled()
	{
		return implementation().VoiceOverlayEnabled();
	}

	public static void ActivateVoiceOverlay(bool activate)
	{
		implementation().ActivateVoiceOverlay(activate);
	}

	public static bool VoiceOverlayActivated()
	{
		return implementation().VoiceOverlayActivated();
	}

	public static void CaptureFrame()
	{
		implementation().CaptureFrame();
	}

	public static void SetNotificationsEnabled(bool notificationsEnabled)
	{
		implementation().SetNotificationsEnabled(notificationsEnabled);
	}

	public static void FireTestNotification()
	{
		implementation().FireTestNotification();
	}

	public static void ShowView()
	{
		implementation().ShowView();
	}

	public static void ShowWatchView()
	{
		implementation().ShowWatchView();
	}

	public static void SetMaximumVideoLength(uint seconds)
	{
		implementation().SetMaximumVideoLength(seconds);
	}

	public static uint MaximumVideoLength()
	{
		return implementation().MaximumVideoLength();
	}

	public static void SetVideoFPS(uint videoFPS)
	{
		implementation().SetVideoFPS(videoFPS);
	}

	public static uint VideoFPS()
	{
		return implementation().VideoFPS();
	}

	public static void SetShouldPauseGameEngine(bool shouldPause)
	{
		implementation().SetShouldPauseGameEngine(shouldPause);
	}

	public static bool ShouldPauseGameEngine()
	{
		return implementation().ShouldPauseGameEngine();
	}

	public static void SetAgeRestrictionEnabled(bool restricted)
	{
		implementation().SetAgeRestrictionEnabled(restricted);
	}

	public static bool IsAgeRestrictionEnabled()
	{
		return implementation().IsAgeRestrictionEnabled();
	}

	public static void SetVideoIncompleteWarningEnabled(bool enabled)
	{
		implementation().SetVideoIncompleteWarningEnabled(enabled);
	}

	public static bool IsVideoComplete()
	{
		return implementation().IsVideoComplete();
	}

	public static void Disable()
	{
		implementation().Disable();
	}

	public static void SetAudioListener(AudioListener audioListener)
	{
		GameObject gameObject = audioListener.gameObject;
		bool flag = true;
		Component[] components = gameObject.GetComponents(typeof(MonoBehaviour));
		Component[] array = components;
		foreach (Component component in array)
		{
			if (typeof(KamcordAudioRecorder).Equals(component.GetType()))
			{
				flag = false;
			}
		}
		int numChannels = ((AudioSettings.speakerMode == AudioSpeakerMode.Mono) ? 1 : 2);
		implementation().SetAudioSettings(AudioSettings.outputSampleRate, numChannels);
		if (flag)
		{
			audioListener.enabled = false;
			gameObject.AddComponent<KamcordAudioRecorder>();
			audioListener.enabled = true;
		}
	}

	public static void WriteAudioData(float[] data, int numSamples)
	{
		implementation().WriteAudioData(data, numSamples);
	}

	public static void Init(string devKey, string devSecret, string appName, VideoQuality videoQuality)
	{
		implementation().Init(devKey, devSecret, appName, videoQuality);
	}

	public static void SetDeviceBlacklist(bool disableiPod4G, bool disableiPod5G, bool disableiPhone3GS, bool disableiPhone4, bool disableiPhone4S, bool disableiPhone5, bool disableiPhone5C, bool disableiPhone5S, bool disableiPad1, bool disableiPad2, bool disableiPadMini, bool disableiPad3, bool disableiPad4, bool disableiPadAir)
	{
		implementation().SetDeviceBlacklist(disableiPod4G, disableiPod5G, disableiPhone3GS, disableiPhone4, disableiPhone4S, disableiPhone5, disableiPhone5C, disableiPhone5S, disableiPad1, disableiPad2, disableiPadMini, disableiPad3, disableiPad4, disableiPadAir);
	}

	public static void SetDefaultTitle(string title)
	{
		implementation().SetDefaultTitle(title);
	}

	public static void SetYouTubeVideoCategory(YouTubeVideoCategory category)
	{
		implementation().SetYouTubeVideoCategory(category);
	}

	public static void SetFacebookSettings(string title, string caption, string description)
	{
		implementation().SetFacebookSettings(title, caption, description);
	}

	public static void SetDefaultEmailSubjectAndBody(string subject, string body)
	{
		implementation().SetDefaultEmailSubjectAndBody(subject, body);
	}

	public static void SetCrossPromoIcon(string localImageFileURL)
	{
		implementation().SetCrossPromoIcon(localImageFileURL);
	}

	public static void SetMode(int mode)
	{
		implementation().SetMode(mode);
	}

	public static void SetFlushOnCopy(bool flush)
	{
		implementation().SetFlushOnCopy(flush);
	}

	private void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.gameObject.name = "KamcordPrefab";
		UnityEngine.Object.DontDestroyOnLoad(this);
		instance = this;
		iOSEnabled_ = enableIOS;
		androidEnabled_ = enableAndroid;
		loggingEnabled_ = enableLogging;
		implementation().Awake(this);
		SetMode(0);
	}

	private void Start()
	{
		implementation().Start(this);
		if (!developerSetVoiceOverlay)
		{
			implementation().SetVoiceOverlayEnabled(enableVoiceOverlay);
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Pause();
		}
		else
		{
			Resume();
		}
	}

	public static void AddListener(KamcordCallbackInterface listener)
	{
		if (!listeners.Contains(listener))
		{
			listeners.Add(listener);
		}
	}

	public static void RemoveListener(KamcordCallbackInterface listener)
	{
		listeners.Remove(listener);
	}

	public static void ClearListeners()
	{
		listeners.Clear();
	}

	private void _KamcordViewDidAppear(string empty)
	{
		timeScale = Time.timeScale;
		Time.timeScale = 0f;
		CallKamcordViewDidAppear();
	}

	private void _KamcordViewDidDisappear(string empty)
	{
		Time.timeScale = timeScale;
		CallKamcordViewDidDisappear();
	}

	private void _KamcordViewDidNotAppear(string empty)
	{
		CallKamcordViewDidNotAppear();
	}

	private void _KamcordVideoWillBeginUploading(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoWillBeginUploading((string)dictionary["videoID"], (string)dictionary["URL"]);
	}

	private void _KamcordVideoUploadProgressed(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoUploadProgressed((string)dictionary["videoID"], Convert.ToSingle(dictionary["progress"]));
	}

	private void _KamcordVideoFinishedUploading(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoFinishedUploading((string)dictionary["videoID"], (bool)dictionary["success"]);
	}

	private void _KamcordVideoSharedTo(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoSharedTo((string)dictionary["videoID"], (string)dictionary["networkName"], (bool)dictionary["success"]);
	}

	private void _KamcordVideoSharedToFacebook(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoSharedToFacebook((string)dictionary["videoID"], (bool)dictionary["success"]);
	}

	private void _KamcordVideoSharedToTwitter(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoSharedToTwitter((string)dictionary["videoID"], (bool)dictionary["success"]);
	}

	private void _KamcordVideoSharedToYoutube(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoSharedToYoutube((string)dictionary["videoID"], (bool)dictionary["success"]);
	}

	private void _KamcordVideoThumbnailReadyAtFilePath(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallVideoThumbnailReadyAtFilePath((string)dictionary["url"]);
	}

	private void _KamcordIsEnabledChanged(string jsonString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		CallIsEnabledChanged((bool)dictionary["isEnabled"]);
	}
}

using System.Collections.Generic;
using UnityEngine;

public class TapjoyPluginAndroid : MonoBehaviour
{
	private static string CONNECT_FLAG_DICTIONARY_NAME = "connectFlags";

	private static string SEGMENTS_DICTIONARY_NAME = "segmentationParams";

	private static AndroidJavaObject currentActivity;

	private static AndroidJavaClass tapjoyConnect;

	private static AndroidJavaObject tapjoyConnectInstance;

	private static AndroidJavaClass TapjoyConnect
	{
		get
		{
			if (tapjoyConnect == null)
			{
				Debug.Log("C#: Loading TapjoyPlugin");
				currentActivity = UnityEngine.Android.AndroidApplication.currentActivity;
				tapjoyConnect = new AndroidJavaClass("com.tapjoy.TapjoyConnectUnity");
			}
			return tapjoyConnect;
		}
	}

	private static AndroidJavaObject TapjoyConnectInstance
	{
		get
		{
			if (tapjoyConnectInstance == null)
			{
				tapjoyConnectInstance = TapjoyConnect.CallStatic<AndroidJavaObject>("getTapjoyConnectInstance", new object[0]);
			}
			return tapjoyConnectInstance;
		}
	}

	public static void SetCallbackHandler(string handlerName)
	{
		TapjoyConnect.CallStatic("setHandlerClass", handlerName);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
		RequestTapjoyConnect(appID, secretKey, null);
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, object> flags)
	{
		if (flags != null)
		{
			foreach (KeyValuePair<string, object> flag in flags)
			{
				if (flag.Value.GetType().IsGenericType)
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)flag.Value;
					string key = flag.Key;
					transferDictionaryToJavaWithName(dictionary, key);
					TapjoyConnect.CallStatic("setDictionaryInDictionary", flag.Key, key, CONNECT_FLAG_DICTIONARY_NAME);
				}
				else
				{
					TapjoyConnect.CallStatic("setKeyValueInDictionary", flag.Key, flag.Value, CONNECT_FLAG_DICTIONARY_NAME);
				}
			}
		}
		TapjoyConnect.CallStatic("requestTapjoyConnect", currentActivity, appID, secretKey);
	}

	public static void transferDictionaryToJavaWithName(Dictionary<string, object> dictionary, string dictionaryName)
	{
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			TapjoyConnect.CallStatic("setKeyValueInDictionary", item.Key, item.Value, dictionaryName);
		}
	}

	public static void EnableLogging(bool enable)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tapjoy.TapjoyLog");
		androidJavaClass.CallStatic("enableLogging", enable);
	}

	public static void SendSegmentationParams(Dictionary<string, object> segmentationParams)
	{
		transferDictionaryToJavaWithName(segmentationParams, SEGMENTS_DICTIONARY_NAME);
		TapjoyConnectInstance.Call("sendSegmentationParams", SEGMENTS_DICTIONARY_NAME);
	}

	public static void AppPause()
	{
		TapjoyConnectInstance.Call("appPause");
	}

	public static void AppResume()
	{
		TapjoyConnectInstance.Call("appResume");
	}

	public static void ActionComplete(string actionID)
	{
		TapjoyConnectInstance.Call("actionComplete", actionID);
	}

	public static void SetUserID(string userID)
	{
		TapjoyConnectInstance.Call("setUserID", userID);
	}

	public static void ShowOffers()
	{
		TapjoyConnectInstance.Call("showOffers");
	}

	public static void GetTapPoints()
	{
		TapjoyConnectInstance.Call("getTapPoints");
	}

	public static void SpendTapPoints(int points)
	{
		TapjoyConnectInstance.Call("spendTapPoints", points);
	}

	public static void AwardTapPoints(int points)
	{
		TapjoyConnectInstance.Call("awardTapPoints", points);
	}

	public static int QueryTapPoints()
	{
		return TapjoyConnectInstance.Call<int>("getTapPointsTotal", new object[0]);
	}

	public static void SetEarnedPointsNotifier()
	{
		TapjoyConnectInstance.Call("setEarnedPointsNotifier");
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
		TapjoyConnectInstance.Call("showDefaultEarnedCurrencyAlert");
	}

	public static void GetDisplayAd()
	{
		TapjoyConnectInstance.Call("getDisplayAd");
	}

	public static void ShowDisplayAd()
	{
		TapjoyConnectInstance.Call("showDisplayAd");
	}

	public static void HideDisplayAd()
	{
		TapjoyConnectInstance.Call("hideDisplayAd");
	}

	public static void SetDisplayAdSize(string size)
	{
		TapjoyConnectInstance.Call("setDisplayAdSize", size);
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
		TapjoyConnectInstance.Call("enableDisplayAdAutoRefresh", enable);
	}

	public static void RefreshDisplayAd()
	{
		TapjoyConnectInstance.Call("getDisplayAd");
	}

	public static void MoveDisplayAd(int x, int y)
	{
		TapjoyConnectInstance.Call("setDisplayAdPosition", x, y);
	}

	public static void SetTransitionEffect(int transition)
	{
	}

	public static void GetFullScreenAd()
	{
		TapjoyConnectInstance.Call("getFullScreenAd");
	}

	public static void ShowFullScreenAd()
	{
		TapjoyConnectInstance.Call("showFullScreenAd");
	}

	public static void SendShutDownEvent()
	{
		TapjoyConnectInstance.Call("sendShutDownEvent");
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
		TapjoyConnectInstance.Call("sendIAPEvent", name, price, quantity, currencyCode);
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
		TapjoyConnectInstance.Call("showOffersWithCurrencyID", currencyID, selector);
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
		TapjoyConnectInstance.Call("getDisplayAdWithCurrencyID", currencyID);
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
		TapjoyConnectInstance.Call("getFullScreenAdWithCurrencyID", currencyID);
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
		TapjoyConnectInstance.Call("setCurrencyMultiplier", multiplier);
	}

	public static void CreateEvent(string eventGuid, string eventName, string eventParameter)
	{
		TapjoyConnectInstance.Call("createEventWithGuid", eventGuid, eventName, eventParameter);
	}

	public static void SendEvent(string eventGuid)
	{
		TapjoyConnectInstance.Call("sendEventWithGuid", eventGuid);
	}

	public static void ShowEvent(string eventGuid)
	{
		TapjoyConnectInstance.Call("showEventWithGuid", eventGuid);
	}

	public static void EnableEventAutoPresent(string eventGuid, bool autoPresent)
	{
		TapjoyConnectInstance.Call("enableEventAutoPresent", eventGuid, autoPresent);
	}

	public static void EnableEventPreload(string eventGuid, bool preload)
	{
		TapjoyConnectInstance.Call("enableEventPreload", eventGuid, preload);
	}

	public static void EventRequestCompleted(string eventGuid)
	{
		TapjoyConnectInstance.Call("eventRequestCompleted", eventGuid);
	}

	public static void EventRequestCancelled(string eventGuid)
	{
		TapjoyConnectInstance.Call("eventRequestCancelled", eventGuid);
	}
}

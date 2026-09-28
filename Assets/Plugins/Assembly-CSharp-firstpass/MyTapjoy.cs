using UnityEngine;

public class MyTapjoy : MonoBehaviour
{
	private const string methodName = "GotTapPoints";

	private static MyTapjoy sInstance;

	public string androidAppId = "b6c91c3e-7fb3-4246-a59a-71b1b84fe952";

	public string androidSecretKey = "JiB7hxPUdRViH4d3Ddwz";

	public string iphoneAppId = "93e78102-cbd7-4ebf-85cc-315ba83ef2d5";

	public string iphoneSecretKey = "JWxgS26URM0XotaghqGn";

	private int tapPoints;

	public static MyTapjoy Instance()
	{
		if (sInstance == null)
		{
			GameObject gameObject = new GameObject("_MyTapjoy");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			Object.DontDestroyOnLoad(gameObject);
			sInstance = gameObject.AddComponent<MyTapjoy>();
		}
		return sInstance;
	}

	public static void Show()
	{
		TapjoyPlugin.ShowOffers();
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		//if (Application.platform == RuntimePlatform.Android)
		//{
		//	AndroidJNI.AttachCurrentThread();
		//}
		//TapjoyPlugin.EnableLogging(true);
		//TapjoyPlugin.SetCallbackHandler(base.gameObject.name);
		//if (Application.platform == RuntimePlatform.Android)
		//{
		//	TapjoyPlugin.RequestTapjoyConnect(androidAppId, androidSecretKey);
		//}
		//else if (Application.platform == RuntimePlatform.IPhonePlayer)
		//{
		//	TapjoyPlugin.RequestTapjoyConnect(iphoneAppId, iphoneSecretKey);
		//}
		//TapjoyPlugin.GetTapPoints();
	}

	private void OnEnable()
	{
		TapjoyPlugin.getTapPointsSucceeded += TapPointsLoaded;
		TapjoyPlugin.spendTapPointsSucceeded += TapPointsSpent;
		TapjoyPlugin.spendTapPointsFailed += TapPointsSpendError;
		TapjoyPlugin.awardTapPointsSucceeded += TapPointsAwarded;
		TapjoyPlugin.tapPointsEarned += CurrencyEarned;
	}

	private void OnDisable()
	{
		TapjoyPlugin.getTapPointsSucceeded -= TapPointsLoaded;
		TapjoyPlugin.spendTapPointsSucceeded -= TapPointsSpent;
		TapjoyPlugin.spendTapPointsFailed -= TapPointsSpendError;
		TapjoyPlugin.awardTapPointsSucceeded -= TapPointsAwarded;
		TapjoyPlugin.tapPointsEarned -= CurrencyEarned;
	}

	public void TapPointsLoaded(int message)
	{
		MonoBehaviour.print("TapPointsLoaded: " + message);
		UsedAllTapPoints();
	}

	public void TapPointsSpent(int message)
	{
		MonoBehaviour.print("TapPointsSpent: " + message);
		SpendSuccessful();
	}

	public void TapPointsSpendError()
	{
		MonoBehaviour.print("TapPointsSpendError");
		tapPoints = 0;
	}

	public void TapPointsAwarded()
	{
		MonoBehaviour.print("TapPointsAwarded");
		UsedAllTapPoints();
	}

	public void CurrencyEarned(int message)
	{
		MonoBehaviour.print("CurrencyEarned: " + message);
		TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
		TapjoyPlugin.GetTapPoints();
	}

	public void TapjoyConnectSuccess(string message)
	{
		MonoBehaviour.print(message);
	}

	public void TapjoyConnectFail(string message)
	{
		MonoBehaviour.print(message);
	}

	public void TapPointsLoaded(string message)
	{
		MonoBehaviour.print("TapPointsLoaded: " + message);
		UsedAllTapPoints();
	}

	public void TapPointsLoadedError(string message)
	{
		MonoBehaviour.print("TapPointsLoadedError: " + message);
	}

	public void TapPointsSpent(string message)
	{
		MonoBehaviour.print("TapPointsSpent: " + message);
		SpendSuccessful();
	}

	public void TapPointsSpendError(string message)
	{
		MonoBehaviour.print("TapPointsSpendError: " + message);
		tapPoints = 0;
	}

	public void TapPointsAwarded(string message)
	{
		MonoBehaviour.print("TapPointsAwarded: " + message);
		UsedAllTapPoints();
	}

	public void TapPointsAwardError(string message)
	{
		MonoBehaviour.print("TapPointsAwardError: " + message);
	}

	public void CurrencyEarned(string message)
	{
		MonoBehaviour.print("CurrencyEarned: " + message);
		TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
		TapjoyPlugin.GetTapPoints();
	}

	public void FullScreenAdLoaded(string message)
	{
		MonoBehaviour.print("FullScreenAdLoaded: " + message);
		TapjoyPlugin.ShowFullScreenAd();
	}

	public void FullScreenAdError(string message)
	{
		MonoBehaviour.print("FullScreenAdError: " + message);
	}

	public void DailyRewardAdError(string message)
	{
		MonoBehaviour.print("DailyRewardAd: " + message);
	}

	public void DisplayAdLoaded(string message)
	{
		MonoBehaviour.print("DisplayAdLoaded: " + message);
		TapjoyPlugin.ShowDisplayAd();
	}

	public void DisplayAdError(string message)
	{
		MonoBehaviour.print("DisplayAdError: " + message);
	}

	public void VideoAdStart(string message)
	{
		MonoBehaviour.print("VideoAdStart: " + message);
	}

	public void VideoAdError(string message)
	{
		MonoBehaviour.print("VideoAdError: " + message);
	}

	public void VideoAdComplete(string message)
	{
		MonoBehaviour.print("VideoAdComplete: " + message);
	}

	private void UsedAllTapPoints()
	{
		tapPoints = TapjoyPlugin.QueryTapPoints();
		if (tapPoints > 0)
		{
			TapjoyPlugin.SpendTapPoints(tapPoints);
		}
	}

	private void SpendSuccessful()
	{
		Debug.Log("SpendSuccessful");
		if (tapPoints > 0)
		{
			SendMessage("GotTapPoints", tapPoints);
		}
	}
}

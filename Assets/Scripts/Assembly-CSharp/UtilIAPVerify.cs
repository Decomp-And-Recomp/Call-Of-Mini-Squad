using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class UtilIAPVerify : MonoBehaviour
{
	protected static UtilIAPVerify m_Instance;

	private string gIdKey = "IAP_Id";

	private string gIdentifierKey = "IAP_TransactionId";

	private string gReceiptKey = "IAP_Receipt";

	private string gSignatureKey = "IAP_Signature";

	private string gReasonKey = "IAP_Reason";

	public string sIAPKey = string.Empty;

	public string sIdentifier = string.Empty;

	public string sReceipt = string.Empty;

	public string sSignature = string.Empty;

	private string m_amazon_userid = string.Empty;

	private string defaultFileName = "payinfo.dat";

	public string m_sCurIAPKey = string.Empty;

	private bool itemOwned;

	private UUtilIAPVerify_OnEvent onPurchaseIAP_SelfFinishedEvent;

	private string sepSign = "zheshiyigechangfengefu";

	private string sepSignSub = "zheshiyigezifengefu";

	private static string CHECK = "COMA";

	private List<string> identities = new List<string>();

	private bool _NeedIAPCertificate;

	private int _CertificateNum;

	private int _curCertiNum;

	public bool _CanIAPCertificate;

	private float fpreVerifyTime = -200f;

	public string content
	{
		get
		{
			string text = string.Empty;
			foreach (string identity in identities)
			{
				text = text + sepSign + identity;
			}
			if (text != string.Empty)
			{
				text = text.Substring(sepSign.Length);
			}
			return text;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				identities.Clear();
				string[] array = value.Split(new string[1] { sepSign }, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					identities.Add(array[i]);
				}
			}
		}
	}

	public static UtilIAPVerify Instance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_UtilIAPVerify");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<UtilIAPVerify>();
		}
		return m_Instance;
	}

	private void Awake()
	{
		if (GameObject.Find("_AndroidPlatform") == null)
		{
			GameObject gameObject = new GameObject("_AndroidPlatform");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			DevicePlugin.InitAndroidPlatform();
			gameObject.AddComponent<TrinitiAdAndroidPlugin>();
			gameObject.AddComponent<AndroidQuit>();
		}
	}

	private void Start()
	{
		Load();
		int checkIAPCount = GetCheckIAPCount();
		if (checkIAPCount > 0)
		{
			_CertificateNum = checkIAPCount;
			_curCertiNum = 0;
			_NeedIAPCertificate = true;
			_CanIAPCertificate = true;
		}
	}

	private void OnEnable()
	{
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAzruYqZA3hyFNdNplzYVsNMsX3/LebQOeuXhDE4M2B7PVxqdOwPheSQDcDoY3vd9YsUi9+eHTwS1WRtOYF30CSNmoduuXdxK6YklCZBh2vcEItA7w3HAbfMUkKMpz9bWcKgvxkMjLhZ0xSRMR6fjtmRySX5eOZQgkZG/vgJqnQucsvxbJsi6Ypr4CrLQzl6Yn9AdwNfxLw62DjrVTvGWWX4V8/MKyOsp2KvxRqXWR/7FJ0dfWbYeoF1WNdyB4waog+S4QPiNXhVBug2xIQ4Hr8P+Ve0TnQCaW2z6lnj2zAZJCG04HZ/3WFygeK5hY7dl4L5Q+FNVxkVmKKBqLVvw26wIDAQAB";
		GoogleIAB.init(publicKey);
		GoogleIAB.setAutoVerifySignatures(true);
	}

	private void OnDisable()
	{
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		GoogleIAB.unbindService();
	}

	private void billingSupportedEvent()
	{
	}

	private void billingNotSupportedEvent(string error)
	{
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (itemOwned)
		{
			GoogleIAB.consumeProduct(m_sCurIAPKey);
			itemOwned = false;
		}
		Utils.logObject(purchases);
		Utils.logObject(skus);
	}

	private void queryInventoryFailedEvent(string error)
	{
		if (itemOwned)
		{
			itemOwned = false;
		}
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		GoogleIAB.consumeProduct(m_sCurIAPKey);
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		sIAPKey = purchase.productId;
		sIdentifier = purchase.orderId;
		sReceipt = purchase.originalJson;
		sSignature = purchase.signature;
		AddToLocal(sIAPKey, sIdentifier, sReceipt, sSignature);
		SaveIAPInfo(sIAPKey, sIdentifier, sReceipt, sSignature, "Success");
		DataCenter.State().selectIAPID = sIAPKey;
		DataCenter.State().selectIAPReceipt = sReceipt;
		DataCenter.State().selectIAPTransactionId = sIdentifier;
		DataCenter.State().selectIAPSignature = sSignature;
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Store_PurchaseIAP, OnPurchaseIAP_SelfFinished);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		if (error.Substring(0, 14) == "Item not owned" || error.Substring(0, 14) == "Unable to buy ")
		{
			GoogleIAB.consumeProduct(m_sCurIAPKey);
		}
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 8);
		UIDialogManager.Instance.HideBlock(8);
	}

	private void purchaseFailedEvent(string reason)
	{
		int num = reason.IndexOf("response: ");
		string text = reason.Substring(num + 10);
		num = text.IndexOf(':');
		switch (text.Substring(0, num))
		{
		case "7":
			itemOwned = true;
			GoogleIAB.queryInventory(new string[1] { m_sCurIAPKey });
			break;
		case "1":
		case "2":
		case "3":
		case "4":
		case "5":
		case "6":
		case "8":
		case "-1000":
		case "-1001":
		case "-1002":
		case "-1003":
		case "-1004":
		case "-1005":
		case "-1006":
		case "-1007":
		case "-1008":
		case "-1009":
		case "-1010":
			UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 8);
			UIDialogManager.Instance.HideBlock(8);
			UIDialogManager.Instance.ShowPopupA("Purchase failed.", UIWidget.Pivot.Center, true);
			break;
		}
	}

	public bool VerifyLastShoppingEvidenceIsFinished()
	{
		bool result = true;
		if (PlayerPrefs.HasKey(gIdentifierKey))
		{
			result = ((PlayerPrefs.GetString(gIdentifierKey) == string.Empty) ? true : false);
		}
		return result;
	}

	public void SetPurchaseIAP_SelfFinishedSuccessedEventDelegate(UUtilIAPVerify_OnEvent _event)
	{
		onPurchaseIAP_SelfFinishedEvent = _event;
	}

	protected void ResetIAPInfo()
	{
		PlayerPrefs.SetString(gIdKey, string.Empty);
		PlayerPrefs.SetString(gIdentifierKey, string.Empty);
		PlayerPrefs.SetString(gReceiptKey, string.Empty);
		PlayerPrefs.SetString(gSignatureKey, string.Empty);
		PlayerPrefs.SetString(gReasonKey, string.Empty);
		PlayerPrefs.Save();
	}

	protected void SaveIAPInfo(string sId, string sIdentifier, string sReceipt, string sSignature, string sReason)
	{
		PlayerPrefs.SetString(gIdKey, sId);
		PlayerPrefs.SetString(gIdentifierKey, sIdentifier);
		PlayerPrefs.SetString(gReceiptKey, sReceipt);
		PlayerPrefs.SetString(gSignatureKey, sSignature);
		PlayerPrefs.SetString(gReasonKey, sReason);
		PlayerPrefs.Save();
	}

	public void RequesetPurchaseIAP(string sIAPKey)
	{
		m_sCurIAPKey = sIAPKey;
		GoogleIAB.purchaseProduct(sIAPKey);
	}

	protected void RequestPurchaseIAP_Apple(string sIAPKey)
	{
		IAPManager.Instance().Purchase(sIAPKey, OnPurchaseApplePartFinished_Success, OnPurchaseApplePartFinished_Failed, OnPurchaseApplePartFinished_Cancel, OnPurchaseApplePartFinished_NetError);
	}

	protected void RequestPurchaseIAP_Self(string sIAPKey, string sIdentifier, string sReceipt)
	{
		DataCenter.State().selectIAPID = sIAPKey;
		DataCenter.State().selectIAPReceipt = sReceipt;
		DataCenter.State().selectIAPTransactionId = sIdentifier;
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Store_PurchaseIAP, OnPurchaseIAP_SelfFinished);
	}

	protected void RequestVerifyIAP_Self(string sIAPKey, string sIdentifier, string sReceipt)
	{
		DataCenter.State().selectIAPID = sIAPKey;
		DataCenter.State().selectIAPReceipt = sReceipt;
		DataCenter.State().selectIAPTransactionId = sIdentifier;
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Store_VerifyIAP, OnVerifyIAP_SelfFinsihed);
	}

	protected void RequestGoogleVerifyIAP_Self(string sIAPKey, string sIdentifier, string sReceipt, string sSignature)
	{
		DataCenter.State().selectIAPID = sIAPKey;
		DataCenter.State().selectIAPReceipt = sReceipt;
		DataCenter.State().selectIAPTransactionId = sIdentifier;
		DataCenter.State().selectIAPSignature = sSignature;
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Store_VerifyIAP, OnVerifyIAP_SelfFinsihed);
	}

	public void OnPurchaseApplePartFinished_Success(string sIAPKey, string sIdentifier, string sReceipt)
	{
		SaveIAPInfo(sIAPKey, sIdentifier, sReceipt, string.Empty, "Success");
		RequestPurchaseIAP_Self(sIAPKey, sIdentifier, sReceipt);
	}

	public void OnPurchaseApplePartFinished_Failed()
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 8);
		UIDialogManager.Instance.HideBlock(8);
		UIDialogManager.Instance.ShowPopupA("Purchase failed.", UIWidget.Pivot.Center, true);
		SaveIAPInfo(string.Empty, string.Empty, string.Empty, string.Empty, "Failed");
	}

	public void OnPurchaseApplePartFinished_Cancel()
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 8);
		UIDialogManager.Instance.HideBlock(8);
		SaveIAPInfo(string.Empty, string.Empty, string.Empty, string.Empty, "Cancel");
	}

	public void OnPurchaseApplePartFinished_NetError()
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 8);
		UIDialogManager.Instance.HideBlock(8);
		UIDialogManager.Instance.ShowPopupA("Server error.", UIWidget.Pivot.Center, true);
		SaveIAPInfo(string.Empty, string.Empty, string.Empty, string.Empty, "NetError");
	}

	protected void OnPurchaseIAP_SelfFinished(int code)
	{
		if (onPurchaseIAP_SelfFinishedEvent != null)
		{
			onPurchaseIAP_SelfFinishedEvent(code, PlayerPrefs.GetString(gIdKey));
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
		if (code == 0 || code == 1045 || code == 1047 || code == 1048 || code == 1049)
		{
			ResetIAPInfo();
			RemoveToLocal(sIAPKey, sIdentifier, sReceipt, sSignature);
		}
	}

	protected void OnVerifyIAP_SelfFinsihed(int code)
	{
		if (code == 0 || code == 1045 || code == 1047 || code == 1048 || code == 1049 || code == 1052)
		{
			ResetIAPInfo();
			RemoveToLocal(sIAPKey, sIdentifier, sReceipt, sSignature);
		}
	}

	public void AddToLocal(string a, string b, string c, string d)
	{
		AddToLocal(a, b, c, d, "0", "0", "0", "0");
	}

	public void AddToLocal(string a, string b, string c, string d, string c1, string c2, string c3, string c4)
	{
		d = d + sepSignSub + c1 + sepSignSub + c2 + sepSignSub + c3 + sepSignSub + c4;
		identities.Add(a);
		identities.Add(b);
		identities.Add(c);
		identities.Add(d);
		Save(true);
	}

	public void RemoveToLocal(string a, string b, string c, string d)
	{
		RemoveToLocal(a, b, c, d, "0", "0", "0", "0");
	}

	public void RemoveToLocal(string a, string b, string c, string d, string c1, string c2, string c3, string c4)
	{
		for (int i = 0; i < identities.Count; i += 4)
		{
			if (identities[i] == a && identities[i + 1] == b && identities[i + 2] == c && identities[i + 3].Contains(d))
			{
				identities.RemoveAt(i + 3);
				identities.RemoveAt(i + 2);
				identities.RemoveAt(i + 1);
				identities.RemoveAt(i);
				break;
			}
		}
		Save(true);
	}

	public int GetCheckIAPCount()
	{
		return identities.Count / 4;
	}

	public string GetSubInfo0(int index)
	{
		return identities[index * 4];
	}

	public string GetSubInfo1(int index)
	{
		return identities[index * 4 + 1];
	}

	public string GetSubInfo2(int index)
	{
		return identities[index * 4 + 2];
	}

	public string GetSubInfo3(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		return array[0];
	}

	public string GetSubInfo4(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 1)
		{
			return array[1];
		}
		return "0";
	}

	public string GetSubInfo5(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 2)
		{
			return array[2];
		}
		return "0";
	}

	public string GetSubInfo6(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 3)
		{
			return array[3];
		}
		return "0";
	}

	public string GetSubInfo7(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 4)
		{
			return array[4];
		}
		return "0";
	}

	public void Load()
	{
		string text = FileUtil.LoadFile(defaultFileName);
		if (text == string.Empty)
		{
			content = string.Empty;
			return;
		}
		int num = text.IndexOf('&');
		if (num < 0)
		{
			content = text.Substring(num + 1);
		}
	}

	public static void SaveFile(string fileName, string content)
	{
		content = CHECK + "&" + content;
		FileUtil.WriteFile(fileName, content);
	}

	public void Save(bool bNeedUpload)
	{
		SaveFile(defaultFileName, "&" + content);
	}

	private void IAPVerify()
	{
		if (_NeedIAPCertificate && _CanIAPCertificate)
		{
			int index = 0;
			string subInfo = GetSubInfo0(index);
			string subInfo2 = GetSubInfo1(index);
			string subInfo3 = GetSubInfo2(index);
			string subInfo4 = GetSubInfo3(index);
			_CanIAPCertificate = false;
			RequestGoogleVerifyIAP_Self(subInfo, subInfo2, subInfo3, subInfo4);
		}
	}

	private void Update()
	{
		if (Time.time - fpreVerifyTime >= 3f)
		{
			fpreVerifyTime = Time.time;
			IAPVerify();
		}
	}
}

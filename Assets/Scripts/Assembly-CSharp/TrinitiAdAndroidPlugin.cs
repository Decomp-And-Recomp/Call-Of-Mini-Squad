using System.Collections;
using Boomlagoon.JSON;
using ChartboostSDK;
using UnityEngine;

public class TrinitiAdAndroidPlugin : MonoBehaviour
{
	private static TrinitiAdAndroidPlugin sInstance;

	private Chartboost m_chartboost;

	private bool isready;

	private bool admob;

	private bool chartboost;

	private bool openclik;

	private string id = string.Empty;

	private string key = string.Empty;

	public static TrinitiAdAndroidPlugin Instance()
	{
		return sInstance;
	}

	public bool IsReady()
	{
		return isready;
	}

	public bool CanAdmob()
	{
		return admob;
	}

	public bool CanChartboost()
	{
		return chartboost;
	}

	public bool CanOpenclik()
	{
		return openclik;
	}

	public string GetID()
	{
		return id;
	}

	public string GetKey()
	{
		return key;
	}

	private void Awake()
	{
		m_chartboost = base.gameObject.AddComponent<Chartboost>();
	}

	private void Start()
	{
		sInstance = this;
		//StartCoroutine(init());
	}

	public void InitAdd()
	{
		if (Instance().IsReady())
		{
			if (Instance().CanAdmob())
			{
				OpenClikPlugin.Initialize(Instance().GetKey());
			}
			else if (Instance().CanChartboost())
			{
				Chartboost.Instance().OnInit(Instance().GetID(), Instance().GetKey());
			}
			else if (Instance().CanOpenclik())
			{
				DevicePlugin.OpenclikShow(Instance().GetID(), Instance().GetKey());
			}
		}
	}

	private IEnumerator init()
	{
		string url = "http://184.168.67.133/trinitiadconfig_android.txt";
		WWW www = new WWW(url);
		yield return www;
		JSONObject jsonData = JSONObject.Parse(www.text);
		try
		{
			JSONObject data = jsonData.GetObject("CoMSquad");
			switch (data.GetString("type"))
			{
			case "admob":
				admob = true;
				id = string.Empty;
				key = data.GetString("admobkey");
				break;
			case "chartboost":
				chartboost = true;
				id = data.GetString("chartboostid");
				key = data.GetString("chartboostkey");
				break;
			case "openclik":
				openclik = true;
				id = data.GetString("openclikurl");
				key = data.GetString("openclikkey");
				break;
			}
		}
		catch
		{
		}
		isready = true;
		InitAdd();
	}
}

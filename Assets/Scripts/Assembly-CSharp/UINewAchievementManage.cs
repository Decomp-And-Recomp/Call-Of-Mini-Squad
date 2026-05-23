using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UINewAchievementManage : MonoBehaviour
{
	public class ACHIEVEMENTITEMINFO
	{
		public UtilUIAchievementInfo ui;

		public AchievementData data;

		public GameObject go;

		public ACHIEVEMENTITEMINFO(GameObject go, AchievementData data)
		{
			this.go = go;
			this.data = data;
			ui = go.GetComponent<UtilUIAchievementInfo>();
		}
	}

	[SerializeField]
	private UtilUIPropertyInfo m_scriptUIPropertyInfo;

	[SerializeField]
	private UIToggle[] toggles;

	[SerializeField]
	private UIGrid[] grid;

	[SerializeField]
	private float rowHeight = 200f;

	[SerializeField]
	private AutoCreatByPrefab[] autoCreate;

	protected Dictionary<string, ACHIEVEMENTITEMINFO> dictAchievementInfo = new Dictionary<string, ACHIEVEMENTITEMINFO>();

	protected Dictionary<GameObject, string> dictMapAchievementInfo = new Dictionary<GameObject, string>();

	public UtilUIPropertyInfo UIPROPERTYINFO
	{
		get
		{
			return m_scriptUIPropertyInfo;
		}
	}

	private void Start()
	{
		UIDialogManager.Instance.SetPropertyScript(UIPROPERTYINFO);
		UIPROPERTYINFO.SetBackBtnClickDelegate(HandleBackBtnClickEvent);
		UIPROPERTYINFO.SetIAPBtnClickDelegate(HandleOpenShopBtnClickEvent);
		UpdatePropertyInfoPart("ACHIEVEMENTS", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
		LoadAchievements();
		//RequestGetAchievementList();
	}

	public void UpdatePropertyInfoPart(string name, int rank, int gold, int crystal)
	{
		if (name != "null#")
		{
			UIPROPERTYINFO.UpdateName(name);
		}
		if (rank > 0)
		{
			UIPROPERTYINFO.UpdateRank(rank.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFO.UpdateRank(0 + string.Empty);
		}
		if (gold > 0)
		{
			UIPROPERTYINFO.UpdateGold(gold.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFO.UpdateGold(0 + string.Empty);
		}
		if (crystal > 0)
		{
			UIPROPERTYINFO.UpdateCrystal(crystal.ToString("###, ###"));
		}
		else
		{
			UIPROPERTYINFO.UpdateCrystal(0 + string.Empty);
		}
	}

	public void InitAchievement(Dictionary<string, AchievementData> dicts)
	{
		foreach (KeyValuePair<GameObject, string> item in dictMapAchievementInfo)
		{
			Object.DestroyImmediate(item.Key);
		}
		for (int gi = 0; gi < grid.Length; gi++)
		{
			if (grid[gi] == null) continue;
			Transform gridT = grid[gi].transform;
			for (int c = gridT.childCount - 1; c >= 0; c--)
			{
				Transform child = gridT.GetChild(c);
				if (child != null && child.name == "ScrollPadSpacer")
				{
					Object.DestroyImmediate(child.gameObject);
				}
			}
		}
		dictAchievementInfo.Clear();
		dictMapAchievementInfo.Clear();

		List<AchievementData> ordered = new List<AchievementData>(dicts.Values);
		ordered.Sort(delegate(AchievementData a, AchievementData b) { return a.site.CompareTo(b.site); });

		HashSet<string> familyShown = new HashSet<string>();
		int dailyIndex = 0;
		int achIndex = 0;
		foreach (AchievementData data in ordered)
		{
			if (data.bDaily)
			{
				SerializeItem(dailyIndex++, data.id, data, autoCreate[0]);
				UpdateItemUI(data.id);
				continue;
			}
			if (data.state == 2)
			{
				continue;
			}
			string familyKey = data.title + "|" + data.counter + "|" + data.counterArg;
			if (familyShown.Contains(familyKey))
			{
				continue;
			}
			familyShown.Add(familyKey);
			SerializeItem(achIndex++, data.id, data, autoCreate[1]);
			UpdateItemUI(data.id);
		}
		for (int gi = 0; gi < grid.Length; gi++)
		{
			if (grid[gi] == null) continue;
			Transform gridT = grid[gi].transform;
			foreach (KeyValuePair<GameObject, string> kvp in dictMapAchievementInfo)
			{
				if (kvp.Key != null && kvp.Key.transform.parent != gridT)
				{
					AchievementData d = dictAchievementInfo[kvp.Value].data;
					bool isDaily = d.bDaily;
					if ((gi == 0 && isDaily) || (gi == 1 && !isDaily))
					{
						kvp.Key.transform.parent = gridT;
						kvp.Key.transform.localScale = Vector3.one;
						kvp.Key.transform.localPosition = Vector3.zero;
					}
				}
			}
			GameObject spacer = new GameObject("ScrollPadSpacer");
			spacer.layer = gridT.gameObject.layer;
			spacer.transform.SetParent(gridT, false);
			spacer.transform.localScale = Vector3.one;
			spacer.transform.localPosition = Vector3.zero;
			UIWidget spacerWidget = spacer.AddComponent<UIWidget>();
			spacerWidget.width = 1;
			spacerWidget.height = (int)rowHeight;
			spacerWidget.alpha = 0f;
			grid[gi].cellHeight = rowHeight;
			grid[gi].enabled = true;
			grid[gi].repositionNow = true;
			grid[gi].Reposition();
		}
	}

    private void LoadAchievements()
    {
        AchievementTracker.RefreshAllProgress();
        UIConstant.gDictAchievementData.Clear();
        foreach (var kvp in DataCenter.Conf().GetAchievementDataMap())
        {
            UIConstant.gDictAchievementData.Add(kvp.Key, kvp.Value);
        }
        InitAchievement(UIConstant.gDictAchievementData);
    }

    protected void SerializeItem(int index, string _id, AchievementData data, AutoCreatByPrefab ac)
	{
		GameObject gameObject = ac.CreatePefab(index);
		ACHIEVEMENTITEMINFO aCHIEVEMENTITEMINFO = new ACHIEVEMENTITEMINFO(gameObject, data);
		dictAchievementInfo.Add(_id, aCHIEVEMENTITEMINFO);
		dictMapAchievementInfo.Add(gameObject, _id);
		aCHIEVEMENTITEMINFO.ui.BlindFunction(HandleClaimRewardBtnClickEvent);
	}

	protected void UpdateItemUI(string _id)
	{
		ACHIEVEMENTITEMINFO aCHIEVEMENTITEMINFO = dictAchievementInfo[_id];
		aCHIEVEMENTITEMINFO.ui.UpdateTitle(aCHIEVEMENTITEMINFO.data.title);
		aCHIEVEMENTITEMINFO.ui.UpdateContent(aCHIEVEMENTITEMINFO.data.des);
		aCHIEVEMENTITEMINFO.ui.UpdateSlider(aCHIEVEMENTITEMINFO.data.scheduleMin, aCHIEVEMENTITEMINFO.data.scheduleMax);
		aCHIEVEMENTITEMINFO.ui.UpdateClaimBtnState(aCHIEVEMENTITEMINFO.data.state);
		aCHIEVEMENTITEMINFO.ui.UpdateReward(aCHIEVEMENTITEMINFO.data.money, aCHIEVEMENTITEMINFO.data.crystal, aCHIEVEMENTITEMINFO.data.honor, aCHIEVEMENTITEMINFO.data.hero);
	}

	public void RequestGetAchievementList()
	{
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 33);
		UIDialogManager.Instance.ShowBlock(33);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Achievement_Get, OnGetAchievementListFinished);
	}

	public void OnGetAchievementListFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 33);
		UIDialogManager.Instance.HideBlock(33);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
		}
		else
		{
			InitAchievement(UIConstant.gDictAchievementData);
		}
	}

	public void RequestClaimReward(string _id)
	{
		DataCenter.State().selectAchievementId = _id;
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 34);
		UIDialogManager.Instance.ShowBlock(34);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Achievement_ClaimReward, OnClaimRewardFinished);
	}

	public void OnClaimRewardFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 34);
		UIDialogManager.Instance.HideBlock(34);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		InitAchievement(UIConstant.gDictAchievementData);
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}

	public void HandleOpenShopBtnClickEvent()
	{
		UIDialogManager.Instance.ShowShopDialogUI(HandleBuyIAPFinishedEvent);
	}

	public void HandleBuyIAPFinishedEvent(int code)
	{
		UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
	}

	public void HandleBackBtnClickEvent()
	{
		SceneManager.Instance.SwitchScene("UIBase");
	}

	public void HandleToggle1ValueChanged()
	{
	}

	public void HandleToggle2ValueChanged()
	{
	}

	public void HandleClaimRewardBtnClickEvent(GameObject go)
	{
		if (dictMapAchievementInfo.ContainsKey(go))
		{
			string text = dictMapAchievementInfo[go];
			ACHIEVEMENTITEMINFO aCHIEVEMENTITEMINFO = dictAchievementInfo[text];
            if (UIConstant.gDictAchievementData.ContainsKey(text))
            {
                AchievementData data = UIConstant.gDictAchievementData[text];
                if (data.state != 1)
                {
                    UIDialogManager.Instance.ShowDriftMsgInfoUI("Unable to claim.");
                    return;
                }
                DataCenter.Save().Money += data.money;
                DataCenter.Save().Crystal += data.crystal;
                DataCenter.Save().Honor += data.honor;
                data.state = 2;
                AchievementTracker.MarkClaimed(data.id);
                Save.Write();
                InitAchievement(UIConstant.gDictAchievementData);
                UpdatePropertyInfoPart("null#", DataCenter.Save().Honor, DataCenter.Save().Money, DataCenter.Save().Crystal);
            }
            /*if (aCHIEVEMENTITEMINFO.data.state == 0)
			{
				UIDialogManager.Instance.ShowDriftMsgInfoUI("Unable to claim.");
			}
			else
			{
				RequestClaimReward(text);
			}*/
        }
	}
}

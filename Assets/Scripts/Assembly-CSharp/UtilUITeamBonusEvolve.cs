using System;
using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class UtilUITeamBonusEvolve : MonoBehaviour
{
	[Serializable]
	public class ITEMINFO
	{
		public TeamAttributeData teamAttributeData;

		public TeamSpecialAttribute.TeamAttributeEvolveData teamAttributeConf;

		public UtilUITeamBonusItem ui;

		public GameObject go;

		public ITEMINFO(GameObject go)
		{
			this.go = go;
			ui = go.GetComponent<UtilUITeamBonusItem>();
		}
	}

	[SerializeField]
	public UILabel detailName;

	[SerializeField]
	public UITexture detailIcon;

	[SerializeField]
	public UILabel detailIntro;

	[SerializeField]
	public GameObject detailUnlockPartGO;

	[SerializeField]
	public GameObject detailUpgradePartGO;

	[SerializeField]
	public UIToggle titleToggle;

	[SerializeField]
	public UIImageButton titleCopyBtn;

	[SerializeField]
	public UIScrollBar detailScrollBar;

	private bool bTeamEvolveUnlocked;

	public List<ITEMINFO> lsEvolveItems;

	protected ITEMINFO nowSelectEvolveItem;

	private DataSave.TeamAttributeSaveData attributeSaveData;

	private UtilUITeamBonusEvolve_OnChanged_Delegate _onChangedEvent;

	public ITEMINFO GetItemInfoByGO(GameObject go)
	{
		ITEMINFO result = null;
		for (int i = 0; i < lsEvolveItems.Count; i++)
		{
			if (lsEvolveItems[i].go == go)
			{
				result = lsEvolveItems[i];
				break;
			}
		}
		return result;
	}

	public void SetTeamEnvolBUnlocked(bool bUnlocked)
	{
		bTeamEvolveUnlocked = bUnlocked;
		SetTitleEnable(bTeamEvolveUnlocked);
		if (!bTeamEvolveUnlocked)
		{
			titleCopyBtn.gameObject.SetActive(true);
		}
		else
		{
			titleCopyBtn.gameObject.SetActive(false);
		}
	}

	protected void SetTitleEnable(bool bEnable)
	{
		titleToggle.gameObject.GetComponent<UIImageButton>().isEnabled = bEnable;
	}

	public void Init(DataSave.TeamAttributeSaveData data, UtilUITeamBonusEvolve_OnChanged_Delegate _dele)
	{
		EvolutionController.RefreshStates();
		attributeSaveData = data;
		_onChangedEvent = _dele;
		for (int i = 0; i < lsEvolveItems.Count; i++)
		{
			lsEvolveItems[i].teamAttributeData = data.teamAttributeEvolve[i];
			lsEvolveItems[i].teamAttributeConf = DataCenter.Conf().GetTeamAttributeEvolveData((TeamSpecialAttribute.TeamAttributeEvolveType)lsEvolveItems[i].teamAttributeData.index);
			lsEvolveItems[i].ui.SetItemToggleStateChangeDelegate(HandleItemStateChangedEvent);
			UpdateItemUI(lsEvolveItems[i]);
		}
		HandleItemStateChangedEvent(lsEvolveItems[0].go, true);
	}

	public void UpdateItemUI(ITEMINFO info)
	{
		UpdateItemState(info);
		info.ui.UpdateLV(info.teamAttributeData.level + "/" + info.teamAttributeData.maxLevel);
		info.ui.UpdateIcon(UIUtil.GetEquipTextureMaterial(info.teamAttributeConf.iconFileName).mainTexture);
	}

	public void UpdateItemState(ITEMINFO info)
	{
		info.ui.UpdateBackground("pic_decal122");
		info.ui.UpdateLVBackground("pic_decal123");
		info.ui.UpdateIconColorfulStyle(false, Color.white);
		info.ui.SetLVPartVisable(true);
		if (info.teamAttributeData.state == Defined.ItemState.Available)
		{
			if (info.teamAttributeData.level <= 0)
			{
				info.ui.UpdateBackground("pic_decal122");
				info.ui.UpdateLVBackground("pic_decal123");
			}
			else if (info.teamAttributeData.level >= info.teamAttributeData.maxLevel)
			{
				info.ui.UpdateBackground("pic_decal012");
				info.ui.UpdateLVBackground("pic_decal026");
			}
			else
			{
				info.ui.UpdateBackground("pic_decal011");
				info.ui.UpdateLVBackground("pic_decal025");
			}
		}
		else if (info.teamAttributeData.state == Defined.ItemState.Purchase)
		{
			info.ui.UpdateBackground("pic_decal10");
			info.ui.UpdateIconColorfulStyle(true, new Color(16f / 51f, 14f / 51f, 0.23529412f));
			info.ui.SetLVPartVisable(false);
		}
		else if (info.teamAttributeData.state == Defined.ItemState.Locked)
		{
			info.ui.UpdateBackground("pic_decal10");
			info.ui.UpdateIconColorfulStyle(true, new Color(16f / 51f, 14f / 51f, 0.23529412f));
			info.ui.SetLVPartVisable(false);
		}
	}

	public void UpdateItemDetailInfo(ITEMINFO info)
	{
		if (!detailScrollBar.enabled)
		{
			detailScrollBar.enabled = true;
		}
		detailScrollBar.value = (float)info.teamAttributeData.level / (float)info.teamAttributeData.maxLevel;
		detailName.text = info.teamAttributeConf.name;
		detailIcon.mainTexture = UIUtil.GetEquipTextureMaterial(info.teamAttributeConf.iconFileName).mainTexture;
		string[] array = info.teamAttributeConf.description.Split(new string[1] { "{*}" }, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array[1].Split(new string[1] { "{#}" }, StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 0; i < array2.Length; i++)
		{
			text = ((info.teamAttributeData.level - 1 != i) ? (text + "[d2d2d2]" + array2[i] + "[-]\n") : (text + "[00ff00]" + array2[i] + "[-]\n"));
		}
		detailIntro.text = array[0] + "\n" + text;
		detailUnlockPartGO.SetActive(false);
		detailUpgradePartGO.SetActive(false);
		if (info.teamAttributeData.state == Defined.ItemState.Available)
		{
			detailUpgradePartGO.SetActive(info.teamAttributeData.level < info.teamAttributeData.maxLevel);
			UILabel component = detailUpgradePartGO.transform.Find("Gold Container").Find("Label").GetComponent<UILabel>();
			UISprite component2 = detailUpgradePartGO.transform.Find("Gold Container").Find("Sprite").GetComponent<UISprite>();
			component2.spriteName = UIUtil.GetCurrencyIconSpriteNameByCurrencyType(info.teamAttributeData.costType);
			component.text = ((info.teamAttributeData.cost <= 0) ? "0" : info.teamAttributeData.cost.ToString("###,###"));
		}
		else if (info.teamAttributeData.state == Defined.ItemState.Purchase)
		{
			detailUnlockPartGO.SetActive(true);
			UIImageButton component3 = detailUnlockPartGO.transform.Find("Break Button").GetComponent<UIImageButton>();
			UILabel component4 = detailUnlockPartGO.transform.Find("Gold Container").Find("Label").GetComponent<UILabel>();
			UISprite component5 = detailUnlockPartGO.transform.Find("Gold Container").Find("Sprite").GetComponent<UISprite>();
			component5.spriteName = UIUtil.GetCurrencyIconSpriteNameByCurrencyType(info.teamAttributeData.costType);
			component4.text = ((info.teamAttributeData.cost <= 0) ? "0" : info.teamAttributeData.cost.ToString("###,###"));
			UILabel component6 = detailUnlockPartGO.transform.Find("Item Container").Find("Label").GetComponent<UILabel>();
			UISprite component7 = detailUnlockPartGO.transform.Find("Item Container").Find("Sprite").GetComponent<UISprite>();
			component7.spriteName = UIUtil.GetCurrencyIconSpriteNameByCurrencyType(info.teamAttributeData.costType);
			component6.text = ((info.teamAttributeData.cost <= 0) ? "0" : info.teamAttributeData.cost.ToString("###,###"));
			component7.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			detailIntro.text = UIUtil.GetCombinationString(UIUtil._UIRedColor, "Unlock at team level " + EconomyConfig.EvolutionUnlockTeamLevel + ". \n") + array[0] + "\n" + text;
		}
	}

	protected void RequestLevelupEvolveUseCrystal()
	{
		RequestLevelupEvolve(1);
	}

	protected void RequestLevelupEvolve(int useCystal)
	{
		DataCenter.State().protocoUseCrystal = useCystal;
		DataCenter.State().selectEvolveIndex = nowSelectEvolveItem.teamAttributeData.index;
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 15);
		UIDialogManager.Instance.ShowBlock(15);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_LevelUpEvolution, OnLevelupEvolutionFinished);
	}

	protected void RequestUnlockEvolveUseCrystal()
	{
		RequestUnlockEvolve(1);
	}

	protected void RequestUnlockEvolve(int useCystal)
	{
		DataCenter.State().protocoUseCrystal = useCystal;
		DataCenter.State().selectEvolveIndex = nowSelectEvolveItem.teamAttributeData.index;
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 16);
		UIDialogManager.Instance.ShowBlock(16);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_UnlockEvolution, OnUnlockEvolutionFinished);
	}

	protected void HandleItemStateChangedEvent(GameObject go, bool bChecked)
	{
		if (bChecked)
		{
			ITEMINFO itemInfoByGO = GetItemInfoByGO(go);
			if (itemInfoByGO != null)
			{
				nowSelectEvolveItem = itemInfoByGO;
				UpdateItemDetailInfo(itemInfoByGO);
			}
		}
	}

	protected void HandleItemUpgradedBtnClickEvent()
	{
		if (nowSelectEvolveItem.teamAttributeData.level < nowSelectEvolveItem.teamAttributeData.maxLevel)
		{
			RequestLevelupEvolve(0);
		}
	}

	public void HandleItemUnlockBtnClickEvent()
	{
		if (true)
		{
			RequestUnlockEvolve(0);
		}
	}

	public void OnLevelupEvolutionFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 15);
		UIDialogManager.Instance.HideBlock(15);
		if (code == EvolutionController.CODE_NOT_ENOUGH || code == EvolutionController.CODE_NO_CRYSTALS)
		{
			UIDialogManager.Instance.ShowDriftMsgInfoUI(code == EvolutionController.CODE_NO_CRYSTALS ? "Not enough tCrystals." : "Not enough gold.");
			return;
		}
		if (code != 0)
		{
			string strInput = "update " + nowSelectEvolveItem.teamAttributeConf.name + "?";
			int num = Mathf.CeilToInt((float)(UIConstant.TradeMoneyNotEnough + UIConstant.TradeHornorNotEnough) / (float)UIConstant.MoneyExchangRate);
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code, strInput, RequestLevelupEvolveUseCrystal, Defined.COST_TYPE.Money, UIConstant.TradeMoneyNotEnough, Defined.COST_TYPE.Honor, UIConstant.TradeHornorNotEnough, " " + num);
			return;
		}
		UpdateItemUI(nowSelectEvolveItem);
		UpdateItemDetailInfo(nowSelectEvolveItem);
		UISoundManager.Instance.PlayLevelUpSound();
		UIEffectManager.Instance.ShowEffectParticle(detailIcon.transform, UIEffectManager.EffectType.E_Team_LevelUp);
		if (_onChangedEvent != null)
		{
			_onChangedEvent();
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void OnUnlockEvolutionFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 16);
		UIDialogManager.Instance.HideBlock(16);
		if (code == EvolutionController.CODE_NOT_ENOUGH || code == EvolutionController.CODE_NO_CRYSTALS)
		{
			UIDialogManager.Instance.ShowDriftMsgInfoUI(code == EvolutionController.CODE_NO_CRYSTALS ? "Not enough tCrystals." : "Not enough gold.");
			return;
		}
		if (code != 0)
		{
			string strInput = "unlock " + nowSelectEvolveItem.teamAttributeConf.name + "?";
			int num = Mathf.CeilToInt((float)(UIConstant.TradeMoneyNotEnough + UIConstant.TradeHornorNotEnough) / (float)UIConstant.MoneyExchangRate);
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code, strInput, RequestUnlockEvolveUseCrystal, Defined.COST_TYPE.Money, UIConstant.TradeMoneyNotEnough, Defined.COST_TYPE.Honor, UIConstant.TradeHornorNotEnough, " " + num);
			return;
		}
		UpdateItemUI(nowSelectEvolveItem);
		UpdateItemDetailInfo(nowSelectEvolveItem);
		UIEffectManager.Instance.ShowEffectParticle(nowSelectEvolveItem.ui.GetIcon().transform, UIEffectManager.EffectType.E_Team_Unlock);
		if (_onChangedEvent != null)
		{
			_onChangedEvent();
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}
}

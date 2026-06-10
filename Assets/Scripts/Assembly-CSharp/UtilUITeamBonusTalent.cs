using System;
using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class UtilUITeamBonusTalent : MonoBehaviour
{
	[Serializable]
	public class ITEMINFO
	{
		public TeamAttributeData teamAttributeData;

		public TeamSpecialAttribute.TeamAttributeData teamAttributeConf;

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
	public UILabel treeRemainingPointsLabel;

	[SerializeField]
	public UILabel treeExternPointsLabel;

	[SerializeField]
	private GameObject[] m_arrTreeLinesLockedStateGO;

	[SerializeField]
	private UIImageButton talentResetBtn;

	[SerializeField]
	private UILabel talentResetBtnLabel;

	[SerializeField]
	private GameObject resetConfirmDialogGO;

	[SerializeField]
	private UILabel resetConfirmDialogPriceLabel;

	private string strDialogSureUnlockRestFunctionMsg = string.Empty;

	public List<ITEMINFO> lsTalentItems;

	protected ITEMINFO nowSelectTalentItem;

	private DataSave.TeamAttributeSaveData attributeSaveData;

	private UtilUITeamBonusTalent_OnChanged_Delegate _onChangedEvent;

	public ITEMINFO GetItemInfoByGO(GameObject go)
	{
		ITEMINFO result = null;
		for (int i = 0; i < lsTalentItems.Count; i++)
		{
			if (lsTalentItems[i].go == go)
			{
				result = lsTalentItems[i];
				break;
			}
		}
		return result;
	}

	public void Init(DataSave.TeamAttributeSaveData data, UtilUITeamBonusTalent_OnChanged_Delegate _dele)
	{
		attributeSaveData = data;
		if (_dele != null)
		{
			_onChangedEvent = _dele;
		}
		for (int i = 0; i < lsTalentItems.Count; i++)
		{
			lsTalentItems[i].teamAttributeData = data.teamAttributeTalent[i];
			lsTalentItems[i].teamAttributeConf = DataCenter.Conf().GetTeamAttributeData((TeamSpecialAttribute.TeamAttributeType)lsTalentItems[i].teamAttributeData.index);
			lsTalentItems[i].ui.SetItemToggleStateChangeDelegate(HandleItemStateChangedEvent);
			UpdateItemUI(lsTalentItems[i]);
			if (i == 0)
			{
				lsTalentItems[i].ui.Select();
			}
		}
		HandleItemStateChangedEvent(lsTalentItems[0].go, true);
		UpdateTreeItemsInfo();
		UpdateResetBtnInfo(attributeSaveData);
		resetConfirmDialogPriceLabel.text = data.teamGeniusResetCostCrystalPerTimes + string.Empty;
		UtilIAPVerify.Instance().SetPurchaseIAP_SelfFinishedSuccessedEventDelegate(OnStorePurchaseIAPFinished);
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
			info.ui.UpdateIconGrayStyle(true);
			info.ui.SetLVPartVisable(false);
		}
		else if (info.teamAttributeData.state == Defined.ItemState.Locked)
		{
			info.ui.UpdateBackground("pic_decal10");
			info.ui.UpdateIconGrayStyle(true);
			info.ui.SetLVPartVisable(false);
		}
	}

	public void UpdateItemDetailInfo(ITEMINFO info)
	{
		detailName.text = info.teamAttributeConf.name;
		detailIcon.mainTexture = UIUtil.GetEquipTextureMaterial(info.teamAttributeConf.iconFileName).mainTexture;
		string[] array = info.teamAttributeConf.description.Split(new string[1] { "{*}" }, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array[1].Split(new string[1] { "{#}" }, StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 0; i < array2.Length; i++)
		{
			text = ((info.teamAttributeData.level - 1 != i) ? (text + "[d2d2d2]" + array2[i] + "[-]\n") : (text + "[00ff00]" + array2[i] + "[-]\n"));
		}
		detailUnlockPartGO.SetActive(false);
		detailUpgradePartGO.SetActive(false);
		if (info.teamAttributeData.state == Defined.ItemState.Available)
		{
			detailUpgradePartGO.SetActive(true);
			detailIntro.text = array[0] + "\n" + text;
			UILabel component = detailUpgradePartGO.transform.Find("Gold Container").Find("Label").GetComponent<UILabel>();
			UISprite component2 = detailUpgradePartGO.transform.Find("Gold Container").Find("Sprite").GetComponent<UISprite>();
			component2.spriteName = UIUtil.GetCurrencyIconSpriteNameByCurrencyType(info.teamAttributeData.costType);
			component.text = ((info.teamAttributeData.cost <= 0) ? "0" : info.teamAttributeData.cost.ToString("###,###"));
			component2.transform.parent.gameObject.SetActive(false);
		}
		else if (info.teamAttributeData.state != Defined.ItemState.Purchase)
		{
			detailIntro.text = UIUtil.GetCombinationString(UIUtil._UIRedColor, "Requirements Assign " + info.teamAttributeData.unlockPoint + " points on Talent. \n") + array[0] + "\n" + text;
		}
	}

	public void UpdateTreeItemsInfo()
	{
		treeRemainingPointsLabel.text = attributeSaveData.teamAttributeRemainingPoints + string.Empty;
		treeExternPointsLabel.text = "[" + attributeSaveData.teamAttributeExtraPoint + "/" + attributeSaveData.teamAttributeExtraPointMax + "]";
	}

	public void UpdateLineLockedTag(ITEMINFO info)
	{
		if (info.teamAttributeData.index % 5 == 0)
		{
			SetTreeLockedStateVisable(info.teamAttributeData.index / 5, info.teamAttributeData.state == Defined.ItemState.Locked, info.teamAttributeData.unlockLevel);
		}
	}

	public void SetTreeLockedStateVisable(int index, bool bShow, int level)
	{
		m_arrTreeLinesLockedStateGO[index].SetActive(bShow);
		m_arrTreeLinesLockedStateGO[index].transform.Find("Label").GetComponent<UILabel>().text = "UNLOCK AT TEAM LEVEL " + level;
	}

	public void UpdateResetBtnInfo(DataSave.TeamAttributeSaveData data)
	{
		if (data.teamGeniusfreeResetTimes > 0)
		{
			UpdateTalentBtnWord("Reset - free");
		}
		else
		{
			UpdateTalentBtnWord("Reset");
		}
		if (data.teamAttributeAssignedPoint > 0)
		{
			talentResetBtn.isEnabled = true;
		}
		else
		{
			talentResetBtn.isEnabled = false;
		}
	}

	protected void UpdateTalentBtnWord(string str)
	{
		talentResetBtnLabel.text = str;
	}

	protected void RequestUnlockResetFunction()
	{
		if (!UtilIAPVerify.Instance().VerifyLastShoppingEvidenceIsFinished())
		{
			string str = "You must wait until your last purchase is completed.";
			UIDialogManager.Instance.ShowPopupA(str, UIWidget.Pivot.Center, true);
		}
	}

	public void RequsetBuyExGeniusPoint()
	{
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 12);
		UIDialogManager.Instance.ShowBlock(12);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_BuyGeniusPoint, OnBuyGeniusPointFinished);
	}

	protected void HandleItemStateChangedEvent(GameObject go, bool bChecked)
	{
		if (bChecked)
		{
			ITEMINFO itemInfoByGO = GetItemInfoByGO(go);
			if (itemInfoByGO != null)
			{
				nowSelectTalentItem = itemInfoByGO;
				UpdateItemDetailInfo(itemInfoByGO);
			}
		}
	}

	protected void HandleItemUpgradedBtnClickEvent()
	{
		if (attributeSaveData.teamAttributeRemainingPoints > 0)
		{
			DataCenter.State().selectTalentIndex = nowSelectTalentItem.teamAttributeData.index;
			UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 11);
			UIDialogManager.Instance.ShowBlock(11);
			HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_LevelUpGenius, OnLevelupGeniusFinished);
		}
		else
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(1005);
		}
	}

	protected void HandleItemUnlockBtnClickEvent()
	{
		if (true)
		{
			DataCenter.State().selectTalentIndex = nowSelectTalentItem.teamAttributeData.index;
			UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 10);
			UIDialogManager.Instance.ShowBlock(10);
			HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_UnlockGenius, OnUnlockGeniusFinished);
		}
	}

	protected void HandleBuyPointBtnClickEvent()
	{
		if (DataCenter.Save().teamAttributeSaveData.teamAttributeExtraPoint < DataCenter.Save().teamAttributeSaveData.teamAttributeExtraPointMax)
		{
			int teamAttributeExtraPointCost = DataCenter.Save().teamAttributeSaveData.teamAttributeExtraPointCost;
			string str = "Spend " + teamAttributeExtraPointCost + " tCrystals to get 1 talent points?";
			UIDialogManager.Instance.ShowPopupA(str, UIWidget.Pivot.Center, UIUtil.GetCurrencyIconSpriteNameByCurrencyType(Defined.COST_TYPE.Crystal), teamAttributeExtraPointCost + string.Empty, true, RequsetBuyExGeniusPoint);
		}
		else
		{
			UIDialogManager.Instance.ShowDriftMsgInfoUI("You already bought the max points.");
		}
	}

	protected void HandleResetPointsBtnClickEvent()
	{
		if (attributeSaveData.teamGeniusfreeResetTimes > 0)
		{
			HandleResetPointsDialogOKBtnClickEvent();
			return;
		}
		resetConfirmDialogGO.SetActive(true);
		UIDialogManager.Instance.ShowBlock(13);
	}

	private void HandleResetPointsDialogOKBtnClickEvent()
	{
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 13);
		UIDialogManager.Instance.ShowBlock(13);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_ResetGenius, OnResetGeniusPointFinished);
	}

	private void HandleResetPointsDialogCloseBtnClickEvent()
	{
		resetConfirmDialogGO.SetActive(false);
		UIDialogManager.Instance.HideBlock(13);
	}

	public void OnBuyGeniusPointFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 12);
		UIDialogManager.Instance.HideBlock(12);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		UpdateTreeItemsInfo();
		if (_onChangedEvent != null)
		{
			_onChangedEvent();
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void OnResetGeniusPointFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 13);
		UIDialogManager.Instance.HideBlock(13);
		resetConfirmDialogGO.SetActive(false);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		Init(DataCenter.Save().teamAttributeSaveData, null);
		if (_onChangedEvent != null)
		{
			_onChangedEvent();
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void OnStorePurchaseIAPFinished(int code, string IAPId)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 14);
		UIDialogManager.Instance.HideBlock(14);
		string empty = string.Empty;
		if (code != 0)
		{
			if (code < 0)
			{
				empty = "System Error. Code: " + code;
				UIDialogManager.Instance.ShowPopupA(empty, UIWidget.Pivot.Center, true);
				return;
			}
			switch (code)
			{
			case 1045:
				empty = "Purchase failed.";
				UIDialogManager.Instance.ShowPopupA(empty, UIWidget.Pivot.Center, true);
				break;
			case 1046:
				empty = "Verification timed out! The request will continue in the background.";
				UIDialogManager.Instance.ShowPopupA(empty, UIWidget.Pivot.Center, true);
				break;
			case 1047:
				empty = "You have purchased.";
				UIDialogManager.Instance.ShowPopupA(empty, UIWidget.Pivot.Center, true);
				break;
			case 1048:
				empty = "You have purchased.";
				UIDialogManager.Instance.ShowPopupA(empty, UIWidget.Pivot.Center, true);
				break;
			case 1049:
				empty = "Purchase failed.";
				UIDialogManager.Instance.ShowPopupA(empty, UIWidget.Pivot.Center, true);
				break;
			default:
				UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
				break;
			}
		}
		else
		{
			UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 13);
			UIDialogManager.Instance.ShowBlock(13);
			HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Team_ResetGenius, OnResetGeniusPointFinished);
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

	public void OnLevelupGeniusFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 11);
		UIDialogManager.Instance.HideBlock(11);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		UpdateItemUI(nowSelectTalentItem);
		UpdateItemDetailInfo(nowSelectTalentItem);
		UpdateTreeItemsInfo();
		UpdateResetBtnInfo(attributeSaveData);
		UISoundManager.Instance.PlayLevelUpSound();
		UIEffectManager.Instance.ShowEffectParticle(detailIcon.transform, UIEffectManager.EffectType.E_Team_LevelUp);
		for (int i = 0; i < DataCenter.State().lsUpdatedTalentIndexs.Count; i++)
		{
			ITEMINFO info = lsTalentItems[DataCenter.State().lsUpdatedTalentIndexs[i]];
			UpdateItemUI(info);
		}
	}

	public void OnUnlockGeniusFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 10);
		UIDialogManager.Instance.HideBlock(10);
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			return;
		}
		UpdateItemUI(nowSelectTalentItem);
		UpdateItemDetailInfo(nowSelectTalentItem);
		UpdateTreeItemsInfo();
		UIEffectManager.Instance.ShowEffectParticle(nowSelectTalentItem.ui.GetIcon().transform, UIEffectManager.EffectType.E_Team_Unlock);
	}
}

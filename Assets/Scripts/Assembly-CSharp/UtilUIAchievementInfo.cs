using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilUIAchievementInfo : MonoBehaviour
{
	[Serializable]
	public class Rewards
	{
		public GameObject go;

		public GameObject H1;

		public UISprite H1Icon;

		public UILabel H1Label;

		public GameObject R1;

		public UISprite R1Icon;

		public UILabel R1Label;

		public GameObject R2;

		public UISprite R2Icon;

		public UILabel R2Label;
	}

	[SerializeField]
	private UILabel title;

	[SerializeField]
	private UILabel content;

	[SerializeField]
	private UISlider slider;

	[SerializeField]
	private UILabel sliderSchedule;

	[SerializeField]
	private UIImageButton claimBtn;

	[SerializeField]
	private Rewards rewards;

	private UtilUIAchievementInfo_ClaimBtnClicked_Delegate _event;

	public void UpdateTitle(string _str)
	{
		title.text = _str;
	}

	public void UpdateContent(string _str)
	{
		content.text = _str;
	}

	public void UpdateSlider(int _min, int _max)
	{
		if (_max > 0)
		{
			slider.gameObject.SetActive(true);
			float num = (float)_min / (float)_max;
			slider.value = num;
			sliderSchedule.text = _min + " / " + _max;
			if (num > 0f)
			{
				slider.alpha = 1f;
			}
			else
			{
				slider.alpha = 1E-05f;
			}
		}
		else
		{
			slider.gameObject.SetActive(false);
		}
	}

	protected void SetClaimBtnEnable(bool bEnable)
	{
		if (!bEnable)
		{
			claimBtn.normalSprite = claimBtn.disabledSprite;
			claimBtn.hoverSprite = claimBtn.disabledSprite;
			claimBtn.pressedSprite = claimBtn.disabledSprite;
			claimBtn.isEnabled = false;
			claimBtn.isEnabled = true;
		}
		else
		{
			claimBtn.normalSprite = claimBtn.hoverSprite;
			claimBtn.pressedSprite = claimBtn.normalSprite;
			claimBtn.isEnabled = false;
			claimBtn.isEnabled = true;
		}
	}

	protected void SetClaimBtnVisable(bool bShow)
	{
		claimBtn.gameObject.SetActive(bShow);
	}

	public void UpdateClaimBtnState(int state)
	{
		SetClaimBtnEnable(true);
		SetClaimBtnVisable(true);
		switch (state)
		{
		case 2:
		case 3:
			SetClaimBtnVisable(false);
			break;
		case 1:
			break;
		default:
			SetClaimBtnEnable(false);
			break;
		}
	}

	protected void HideRewardsPart()
	{
		rewards.H1.SetActive(false);
		rewards.R1.SetActive(false);
		rewards.R2.SetActive(false);
	}

	protected void UpdateRewardHero(string heroIcon, string heroName)
	{
		rewards.H1.SetActive(true);
		rewards.H1Icon.spriteName = heroIcon;
		rewards.H1Label.text = heroName;
	}

	protected void UpdateRewardCurrency(Defined.COST_TYPE r1, int r1Count, Defined.COST_TYPE r2, int r2Count)
	{
		if (r2Count > 0)
		{
			rewards.R2.SetActive(true);
			rewards.R2Icon.spriteName = UIUtil.GetCurrencyIconSpriteNameByCurrencyType(r2);
			rewards.R2Label.text = string.Empty + r2Count;
			rewards.R1.transform.localPosition = new Vector3(-170f, rewards.R1.transform.localPosition.y, rewards.R1.transform.localPosition.z);
			rewards.R2.transform.localPosition = new Vector3(70f, rewards.R2.transform.localPosition.y, rewards.R2.transform.localPosition.z);
		}
		else
		{
			rewards.R1.transform.localPosition = new Vector3(-110f, rewards.R1.transform.localPosition.y, rewards.R1.transform.localPosition.z);
		}
		rewards.R1.SetActive(true);
		rewards.R1Icon.spriteName = UIUtil.GetCurrencyIconSpriteNameByCurrencyType(r1);
		rewards.R1Label.text = string.Empty + r1Count;
	}

	public void UpdateReward(int money, int crystal, int hornor, int hero)
	{
		HideRewardsPart();
		if (hero >= 0)
		{
			DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(hero);
			UpdateRewardHero(heroDataByIndex.iconFileName, heroDataByIndex.name);
			return;
		}
		List<KeyValuePair<Defined.COST_TYPE, int>> list = new List<KeyValuePair<Defined.COST_TYPE, int>>();
		if (money > 0)
		{
			list.Add(new KeyValuePair<Defined.COST_TYPE, int>(Defined.COST_TYPE.Money, money));
		}
		if (crystal > 0)
		{
			list.Add(new KeyValuePair<Defined.COST_TYPE, int>(Defined.COST_TYPE.Crystal, crystal));
		}
		if (hornor > 0)
		{
			list.Add(new KeyValuePair<Defined.COST_TYPE, int>(Defined.COST_TYPE.Honor, hornor));
		}
		if (list.Count < 2)
		{
			list.Add(new KeyValuePair<Defined.COST_TYPE, int>(Defined.COST_TYPE.Honor, 0));
		}
		UpdateRewardCurrency(list[0].Key, list[0].Value, list[1].Key, list[1].Value);
	}

	public void HandleClaimBtnClickEvent()
	{
		if (_event != null)
		{
			_event(base.gameObject);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void BlindFunction(UtilUIAchievementInfo_ClaimBtnClicked_Delegate _e)
	{
		_event = _e;
	}
}

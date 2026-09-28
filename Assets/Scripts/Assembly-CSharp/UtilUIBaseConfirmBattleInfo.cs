using System.Collections.Generic;
using UnityEngine;

public class UtilUIBaseConfirmBattleInfo : MonoBehaviour
{
	[SerializeField]
	private GameObject mGO;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UITexture scenceIconTexture;

	[SerializeField]
	private UILabel battleCombatLabel;

	[SerializeField]
	private UISprite difficultBGSprite;

	[SerializeField]
	private UILabel difficultLabel;

	[SerializeField]
	private UILabel vitalityLabel;

	[SerializeField]
	private UILabel showCaseTitleLabel;

	[SerializeField]
	private UILabel showCaseConentLabel;

	[SerializeField]
	private GameObject teamPlayersGO;

	[SerializeField]
	private List<GameObject> teamPlayerGO;

	[SerializeField]
	private UILabel myTeamCombatLabel;

	[SerializeField]
	private UILabel expRewardLabel;

	[SerializeField]
	private UILabel goldRewardLabel;

	[SerializeField]
	private UISprite itemRewardIcon;

	[SerializeField]
	private UILabel itemRewardLabel;

	[SerializeField]
	private UIImageButton cancelBtn;

	[SerializeField]
	private UIImageButton battleBtn;

	private UtilUIBaseConfirmBattleInfo_CancelBtnClicked_Delegate CancelBtnClickedEvent;

	private UtilUIBaseConfirmBattleInfo_BattleBtnClicked_Delegate BattleBtnClickedEvent;

	protected void HandleEvent_CancelBtnClicked()
	{
		if (CancelBtnClickedEvent != null)
		{
			CancelBtnClickedEvent();
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	protected void HandleEvent_BattleBtnClicked()
	{
		if (BattleBtnClickedEvent != null)
		{
			BattleBtnClickedEvent();
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void SetVisable(bool bShow)
	{
		mGO.SetActive(bShow);
	}

	public void SetBattleBtnEnable(bool bEnable)
	{
		battleBtn.isEnabled = bEnable;
	}

	public void UpdateTitel(string str)
	{
		titleLabel.text = str;
	}

	public void UpdateScenceIconTexture(Texture iconTex)
	{
		scenceIconTexture.mainTexture = iconTex;
	}

	public void UpdateScenceCombat(Color col, string str)
	{
		battleCombatLabel.text = str;
		battleCombatLabel.color = col;
	}

	public void UpdateDifficultWarningColorAndScenceCombat(int scenceCombat, int teamCombat)
	{
		if ((float)teamCombat < (float)scenceCombat * 0.5f)
		{
			Color color = new Color(1f, 0f, 0f);
			difficultBGSprite.color = color;
			difficultLabel.color = color;
			difficultLabel.text = "[All Dead]";
			UpdateScenceCombat(UIUtil._UIWhiteColor, scenceCombat + string.Empty);
		}
		else if ((float)teamCombat >= (float)scenceCombat * 0.5f && (float)teamCombat < (float)scenceCombat * 0.75f)
		{
			Color color2 = new Color(50f / 51f, 0.5882353f, 0f);
			difficultBGSprite.color = color2;
			difficultLabel.color = color2;
			difficultLabel.text = "[Dangerous]";
			UpdateScenceCombat(UIUtil._UIWhiteColor, scenceCombat + string.Empty);
		}
		else if ((float)teamCombat >= (float)scenceCombat * 0.75f && (float)teamCombat < (float)scenceCombat * 1f)
		{
			Color color3 = new Color(50f / 51f, 50f / 51f, 0f);
			difficultBGSprite.color = color3;
			difficultLabel.color = color3;
			difficultLabel.text = "[Insecure]";
			UpdateScenceCombat(UIUtil._UIWhiteColor, scenceCombat + string.Empty);
		}
		else
		{
			Color color4 = new Color(4f / 51f, 46f / 51f, 0f);
			difficultBGSprite.color = color4;
			difficultLabel.color = color4;
			difficultLabel.text = "[Normal]";
			UpdateScenceCombat(UIUtil._UIWhiteColor, scenceCombat + string.Empty);
		}
		difficultBGSprite.transform.localPosition = new Vector3(battleCombatLabel.transform.parent.localPosition.x + battleCombatLabel.localSize.x + (float)difficultBGSprite.width / 2f, difficultBGSprite.transform.localPosition.y, difficultBGSprite.transform.localPosition.z);
	}

	public void UpdateConsumeVitality(string str)
	{
		vitalityLabel.text = str;
	}

	public void UpdateShowPartTitle(string str)
	{
		showCaseTitleLabel.text = str;
	}

	public void UpdateShowPartConent(string str)
	{
		showCaseConentLabel.text = str;
	}

	public void UpdateTeamCombat(string str)
	{
		myTeamCombatLabel.text = str;
	}

	public void UpdateTeamPlayerInfo(List<string> teamPlayerIcons)
	{
		for (int i = 0; i < teamPlayerGO.Count; i++)
		{
			if (i < teamPlayerIcons.Count)
			{
				teamPlayerGO[i].SetActive(true);
				GameObject icon = teamPlayerGO[i].transform.Find("Icon").gameObject;
				if (!string.IsNullOrEmpty(teamPlayerIcons[i]))
				{
					icon.SetActive(true);
					icon.GetComponent<UISprite>().spriteName = teamPlayerIcons[i];
				}
				else
				{
					icon.SetActive(false);
				}
			}
			else
			{
				teamPlayerGO[i].SetActive(false);
			}
		}
	}

	public void UpdateEXPReward(string str)
	{
		expRewardLabel.text = str;
	}

	public void UpdateGoldReward(string str)
	{
		goldRewardLabel.text = str;
	}

	public void UpdateItemRewardIcon(string str)
	{
		itemRewardIcon.spriteName = str;
	}

	public void UpdateItemReward(string str)
	{
		if (int.Parse(str) > 0)
		{
			itemRewardLabel.transform.parent.gameObject.SetActive(true);
			itemRewardLabel.text = str;
		}
		else
		{
			itemRewardLabel.transform.parent.gameObject.SetActive(false);
		}
	}

	public void SetCancelBtnClickDelegate(UtilUIBaseConfirmBattleInfo_CancelBtnClicked_Delegate dele)
	{
		CancelBtnClickedEvent = dele;
	}

	public void SetBattleBtnClickDelegate(UtilUIBaseConfirmBattleInfo_BattleBtnClicked_Delegate dele)
	{
		BattleBtnClickedEvent = dele;
	}
}

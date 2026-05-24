using UnityEngine;

public class UtilUIStandbyPlayerAppearance : MonoBehaviour
{
	[SerializeField]
	private UITexture m_iconTexture;

	[SerializeField]
	public UIWidget m_widget;

	[SerializeField]
	private GameObject m_inTeamTagGO;

	[SerializeField]
	private GameObject m_inActiveTagGO;

	[SerializeField]
	private UISprite m_inActiveCostIcon;

	[SerializeField]
	private UILabel m_inActiveCostLabel;

	[SerializeField]
	private UIImageButton m_inActiveUnlockBtn;

	[SerializeField]
	private GameObject m_lockedTagGO;

	[SerializeField]
	private UILabel m_lockedLabel;

	[SerializeField]
	private GameObject m_combatGO;

	[SerializeField]
	private UILabel m_combatLabel;

	[SerializeField]
	private GameObject m_defaultIconGO;

	private UtilUIStandbyPlayerAppearance_ItemClicked_Delegate ItemClickedEvent;

	private UtilUIStandbyPlayerAppearance_UnlockBtnClicked_Delegate UnlockBtnClickedEvent;

	private UtilUIStandbyPlayerAppearance_GOPressed_Delegate GOPressedEvent;

	private void OnPress(bool pressed)
	{
		if (GOPressedEvent != null)
		{
			GOPressedEvent(base.gameObject, pressed);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void OnClick()
	{
		if (ItemClickedEvent != null)
		{
			ItemClickedEvent(base.gameObject);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	private void HandleEvent_UnlockBtnClicked()
	{
		if (UnlockBtnClickedEvent != null)
		{
			UnlockBtnClickedEvent(base.gameObject);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void SetItemVisable(bool bShow)
	{
		if (bShow)
		{
			base.gameObject.GetComponent<UIWidget>().alpha = 1f;
		}
		else
		{
			base.gameObject.GetComponent<UIWidget>().alpha = 0f;
		}
	}

	public void UpdateIcon(Texture iconTex)
	{
		m_iconTexture.mainTexture = iconTex;
	}

	public void UpdateIconUV(Rect rect)
	{
		m_iconTexture.uvRect = rect;
	}

	public void HideDefaultModelIcon()
	{
		m_defaultIconGO.SetActive(false);
	}

	public void UpdateIconGrayType(bool bGray)
	{
		if (bGray)
		{
			m_iconTexture.shader = Shader.Find("Triniti/Extra/GrayStyleUI");
		}
		else
		{
			m_iconTexture.shader = Shader.Find("Unlit/Colored");
		}
		m_iconTexture.enabled = false;
		m_iconTexture.enabled = true;
	}

	public void SetInTeamTagVisable(bool bShow)
	{
		m_inTeamTagGO.SetActive(bShow);
	}

	public void SetInActiveTagVisable(bool bShow)
	{
		m_inActiveTagGO.SetActive(bShow);
	}

	public void UpdateInActiveCost(string str)
	{
		m_inActiveCostLabel.text = str;
	}

	public void UpdateInActiveCostIcon(string str)
	{
		m_inActiveCostIcon.spriteName = str;
	}

	public void SetLockedTagVisable(bool bShow)
	{
		m_lockedTagGO.SetActive(bShow);
	}

	public void UpdateUnlockedLevel(string str)
	{
		m_lockedLabel.text = str;
	}

	public void SetCombatTagVisable(bool bShow)
	{
		m_combatGO.SetActive(bShow);
	}

	public void UpdateCombat(string str)
	{
		m_combatLabel.text = str;
	}

	public void UpdatePlayerAppearanceState(Defined.ItemState state)
	{
		SetLockedTagVisable(false);
		SetInActiveTagVisable(false);
		UpdateIconGrayType(false);
		SetCombatTagVisable(false);
		switch (state)
		{
		case Defined.ItemState.Available:
			SetCombatTagVisable(true);
			break;
		case Defined.ItemState.Purchase:
			SetInActiveTagVisable(true);
			break;
		case Defined.ItemState.Locked:
			SetLockedTagVisable(true);
			UpdateIconGrayType(true);
			break;
		}
	}

	public void SetPlayerAppearanceItemBtnClickedDelegate(UtilUIStandbyPlayerAppearance_ItemClicked_Delegate dele)
	{
		ItemClickedEvent = dele;
	}

	public void SetPlayerAppearanceUnlockBtnClickedDelegate(UtilUIStandbyPlayerAppearance_UnlockBtnClicked_Delegate dele)
	{
		UnlockBtnClickedEvent = dele;
	}

	public void SetPlayerAppearanceGOPressedDelegate(UtilUIStandbyPlayerAppearance_GOPressed_Delegate dele)
	{
		GOPressedEvent = dele;
	}
}

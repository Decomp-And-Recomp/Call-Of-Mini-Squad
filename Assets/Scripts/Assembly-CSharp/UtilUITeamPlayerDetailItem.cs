using UnityEngine;

public class UtilUITeamPlayerDetailItem : MonoBehaviour
{
	[SerializeField]
	private UIToggle m_toggle;

	[SerializeField]
	private UIImageButton m_imageBtn;

	[SerializeField]
	private UISprite m_backgroundSprite;

	[SerializeField]
	private UISprite m_equipTagSprite;

	[SerializeField]
	private UITexture m_iconTexture;

	[SerializeField]
	private GameObject m_StarsGO;

	private UtilUITeamPlayerDetailItem_ItemToggleStateChange_Delegate ItemToggleStateChangeEvent;

	public void HandleEvent_ItemToggleStateChange()
	{
		if (ItemToggleStateChangeEvent != null)
		{
			ItemToggleStateChangeEvent(base.gameObject, m_toggle.value);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	public void Selected(bool bShow)
	{
		m_toggle.value = bShow;
	}

	public void SetItemEnable(bool bEnable)
	{
		m_imageBtn.isEnabled = bEnable;
	}

	public void UpdateIcon(Texture iconTex)
	{
		m_iconTexture.mainTexture = iconTex;
	}

	public UITexture GetIcon()
	{
		return m_iconTexture;
	}

	public void UpdateIconGrayStyle(bool bGray)
	{
		if (bGray)
		{
			m_iconTexture.shader = Shader.Find("Triniti/Extra/GrayStyleUI");
		}
		else
		{
			m_iconTexture.shader = Shader.Find("Unlit/Transparent Colored");
		}
	}

	public void UpdateIconColorfulStyle(bool bColorful, Color col)
	{
		if (bColorful)
		{
			m_iconTexture.shader = Shader.Find("Triniti/Extra/GrayStyleUI");
			m_iconTexture.color = col;
		}
		else
		{
			m_iconTexture.shader = Shader.Find("Unlit/Transparent Colored");
			m_iconTexture.color = Color.white;
		}
	}

	public void UpdateStars(int showCount, int maxCount)
	{
		UIUtil.ShowStars(m_StarsGO.transform, showCount);
		if (showCount >= maxCount)
		{
			UpdateBackground("pic_decal012");
		}
	}

	public void SetEquipTagVisable(bool bShow)
	{
		m_equipTagSprite.gameObject.SetActive(bShow);
	}

	public void UpdateBackground(string str)
	{
		m_backgroundSprite.spriteName = str;
		m_imageBtn.normalSprite = str;
		m_imageBtn.hoverSprite = str;
	}

	public void UpdateTreeItemState(Defined.ItemState state)
	{
		SetItemEnable(true);
		UpdateBackground("pic_decal011");
		UpdateIconGrayStyle(false);
		switch (state)
		{
		case Defined.ItemState.Purchase:
			UpdateBackground("pic_decal10");
			break;
		case Defined.ItemState.FailByReasonOne:
			UpdateBackground("pic_decal10");
			UpdateIconGrayStyle(true);
			break;
		case Defined.ItemState.Locked:
			SetItemEnable(false);
			UpdateIconGrayStyle(true);
			break;
		}
	}

	public void SetItemToggleStateChangeDelegate(UtilUITeamPlayerDetailItem_ItemToggleStateChange_Delegate dele)
	{
		ItemToggleStateChangeEvent = dele;
	}
}

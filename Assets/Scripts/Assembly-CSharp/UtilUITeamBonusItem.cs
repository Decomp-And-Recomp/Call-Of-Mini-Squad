using UnityEngine;

public class UtilUITeamBonusItem : MonoBehaviour
{
	[SerializeField]
	private UIToggle m_toggle;

	[SerializeField]
	private UIImageButton m_imageBtn;

	[SerializeField]
	private UISprite m_backgroundSprite;

	[SerializeField]
	private UITexture m_iconTexture;

	[SerializeField]
	private UILabel m_levelInfo;

	private UtilUITeamBonusItem_ItemToggleStateChange_Delegate ItemToggleStateChangeEvent;

	public void Select()
	{
		m_toggle.value = true;
	}

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

	public void UpdateBackground(string str)
	{
		m_backgroundSprite.spriteName = str;
		m_imageBtn.normalSprite = str;
		m_imageBtn.hoverSprite = str;
	}

	public void UpdateLV(string str)
	{
		m_levelInfo.text = str;
	}

	public void SetLVPartVisable(bool bShow)
	{
		m_levelInfo.transform.parent.gameObject.SetActive(bShow);
	}

	public void UpdateLVBackground(string str)
	{
		m_levelInfo.transform.parent.gameObject.GetComponent<UISprite>().spriteName = str;
	}

	public void UpdateTreeItemState(Defined.ItemState state)
	{
		SetItemEnable(true);
		UpdateBackground("pic_decal011");
		UpdateIconGrayStyle(false);
		SetLVPartVisable(true);
		switch (state)
		{
		case Defined.ItemState.Purchase:
			UpdateIconGrayStyle(true);
			break;
		case Defined.ItemState.Locked:
			UpdateIconGrayStyle(true);
			break;
		}
	}

	public void SetItemToggleStateChangeDelegate(UtilUITeamBonusItem_ItemToggleStateChange_Delegate dele)
	{
		ItemToggleStateChangeEvent = dele;
	}
}

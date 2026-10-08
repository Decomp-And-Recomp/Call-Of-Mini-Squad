using UnityEngine;

public class UtilUIAccountUILogoutInfo : MonoBehaviour
{
	private void Start()
	{
	}

	public void Init()
	{
	}

	public void SetVisable(bool bShow)
	{
		base.gameObject.SetActive(bShow);
	}

	public void OnBackBtnClickedEvent()
	{
		UtilUIAccountManager.mInstance.HideScene(UtilUIAccountManager.UIScene.E_LOGOUT);
	}

	public void OnLogoutBtnClickedEvent()
	{
		//DataCenter.State().ResetData(true);
		//Application.LoadLevel("UICheckUpdate");
		UtilUIAccountManager.mInstance.HideScene(UtilUIAccountManager.UIScene.E_LOGOUT);
	}
}

using UnityEngine;

public class UtilUIAccountUIAccountInfomationInfo : MonoBehaviour
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
		UtilUIAccountManager.mInstance.HideScene(UtilUIAccountManager.UIScene.E_ACCOUNT);
	}

	public void OnCreateAccountBtnClickedEvent()
	{
		UtilUIAccountManager.mInstance.HideAll();
		UtilUIAccountManager.mInstance.ShowScene(UtilUIAccountManager.UIScene.E_REGISTER);
		UtilUIAccountManager.mInstance.accountRegisterInfo.Init(true);
	}

	public void OnLoginBtnClickedEvent()
	{
		UtilUIAccountManager.mInstance.HideScene(UtilUIAccountManager.UIScene.E_ACCOUNT);
		//DataCenter.State().ResetData(true);
		//Application.LoadLevel("UICheckUpdate");
	}
}

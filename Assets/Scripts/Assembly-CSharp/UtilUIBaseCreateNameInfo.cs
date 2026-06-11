using System.Text.RegularExpressions;
using UnityEngine;

public class UtilUIBaseCreateNameInfo : MonoBehaviour
{
	public GameObject go;

	public UIInput mInput;

	protected Regex myRex;

	private void Start()
	{
		myRex = new Regex("^[A-Za-z0-9]+$");
		mInput.label.maxLineCount = 1;
		mInput.keyboardType = UIInput.KeyboardType.NamePhonePad;
	}

	public void SetVisable(bool bShow)
	{
		go.SetActive(bShow);
	}

	public void OnSubmit()
	{
		RequestCreatePlayer();
	}

	public void OnOKBtnClickEvent()
	{
		RequestCreatePlayer();
	}

	public void RequestCreatePlayer()
	{
		string text = NGUIText.StripSymbols(mInput.value);
		if (text.Length >= 4 && text.Length <= 15)
		{
			Match match = myRex.Match(text);
			if (match.Success)
			{
				if (!string.IsNullOrEmpty(text))
				{
					UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 38);
					UIDialogManager.Instance.ShowBlock(38);
					DataCenter.Save().userName = text;
					HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.CreatePlayerData, OnCreatePlayerDateFinished);
				}
			}
			else
			{
				UIDialogManager.Instance.ShowHttpFeedBackMsg(1053);
			}
		}
		else
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(1053);
		}
	}

	public void OnCreatePlayerDateFinished(int code)
	{
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 38);
		UIDialogManager.Instance.HideBlock(38);
		mInput.value = string.Empty;
		mInput.isSelected = false;
		if (code != 0)
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
			DataCenter.Save().tutorialStep = Defined.TutorialStep.CreateNameFialed;
			return;
		}
		DataCenter.Save().tutorialStep = Defined.TutorialStep.CreateNameFinish;
		DataCenter.Save().bNewUser = false;
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 39);
		UIDialogManager.Instance.ShowBlock(39);
		HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Get_WorldNodeList, CreateWorldNodeListCallBack);
	}

	public void CreateWorldNodeListCallBack(int code)
	{
		if (code == 0)
		{
			UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 39);
			UIDialogManager.Instance.HideBlock(39);
			SetVisable(false);
			Application.LoadLevel("UIBase");
		}
		else
		{
			UIDialogManager.Instance.ShowHttpFeedBackMsg(code);
		}
	}
}

using CoMDS2;
using UnityEngine;

public class UtilUIBaseOpitionInfo : MonoBehaviour
{
	[SerializeField]
	private GameObject m_go;

	[SerializeField]
	private GameObject m_staffContentGO;

	[SerializeField]
	private GameObject m_musicBtnGO;

	[SerializeField]
	private GameObject m_soundBtnGO;

	private GameObject[] m_gameObjectsMusic;

	private UILabel m_labelTextMusic;

	private GameObject[] m_gameObjectsSound;

	private UILabel m_labelTextSound;

	public void Init()
	{
		SetVisable(true);
		m_labelTextMusic = m_musicBtnGO.transform.Find("Label").gameObject.GetComponent<UILabel>();
		m_gameObjectsMusic = new GameObject[1];
		m_gameObjectsMusic[0] = m_musicBtnGO.transform.Find("cusor").gameObject;
		if (DataCenter.Save().PlayMusic)
		{
			m_labelTextMusic.text = "OFF";
			m_labelTextMusic.transform.localPosition = new Vector3(17f, m_labelTextMusic.transform.localPosition.y, m_labelTextMusic.transform.localPosition.z);
			m_gameObjectsMusic[0].transform.localPosition = new Vector3(-17f, m_gameObjectsMusic[0].transform.localPosition.y, m_gameObjectsMusic[0].transform.localPosition.z);
		}
		else
		{
			m_labelTextMusic.text = "ON";
			m_labelTextMusic.transform.localPosition = new Vector3(-17f, m_labelTextMusic.transform.localPosition.y, m_labelTextMusic.transform.localPosition.z);
			m_gameObjectsMusic[0].transform.localPosition = new Vector3(17f, m_gameObjectsMusic[0].transform.localPosition.y, m_gameObjectsMusic[0].transform.localPosition.z);
		}
		m_labelTextSound = m_soundBtnGO.transform.Find("Label").gameObject.GetComponent<UILabel>();
		m_gameObjectsSound = new GameObject[1];
		m_gameObjectsSound[0] = m_soundBtnGO.transform.Find("cusor").gameObject;
		if (DataCenter.Save().PlaySound)
		{
			m_labelTextSound.text = "OFF";
			m_labelTextSound.transform.localPosition = new Vector3(17f, m_labelTextSound.transform.localPosition.y, m_labelTextSound.transform.localPosition.z);
			m_gameObjectsSound[0].transform.localPosition = new Vector3(-17f, m_gameObjectsSound[0].transform.localPosition.y, m_gameObjectsSound[0].transform.localPosition.z);
		}
		else
		{
			m_labelTextSound.text = "ON";
			m_labelTextSound.transform.localPosition = new Vector3(-17f, m_labelTextSound.transform.localPosition.y, m_labelTextSound.transform.localPosition.z);
			m_gameObjectsSound[0].transform.localPosition = new Vector3(17f, m_gameObjectsSound[0].transform.localPosition.y, m_gameObjectsSound[0].transform.localPosition.z);
		}
	}

	public void SetVisable(bool bShow)
	{
		m_go.SetActive(bShow);
	}

	public void HandleForumBtnEvent()
	{
		UIConstant.bNeedLoseConnect = false;
		Application.OpenURL("http://forum.trinitigame.com/forum/viewforum.php?f=140");
	}

	public void HandleSupportBtnEvent()
	{
		UIConstant.bNeedLoseConnect = false;
		string url = "http://www.trinitigame.com/support/support.html?country=" + DevicePlugin.GetCountryCode() + "&device=Android%20google&os=" + DevicePlugin.GetSysVersion() + "&game=CoM:%20Squad&gamever=1.0.1&code=" + UtilUIAccountManager.mInstance.accountData.email;
		Application.OpenURL(url);
	}

	public void HandleStaffBtnEvent()
	{
		m_staffContentGO.SetActive(true);
	}

	public void HandelReviewBtnEvent()
	{
		UIConstant.bNeedLoseConnect = false;
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.trinitigame.android.callofminidoubleshot2");
	}

	public void HandelCloseBtnEvent()
	{
		UIConstant.bNeedLoseConnect = true;
		SetVisable(false);
	}

	public void HandleMusicPress()
	{
		DataCenter.Save().PlayMusic = !DataCenter.Save().PlayMusic;
		if (DataCenter.Save().PlayMusic)
		{
			BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.UI_BG);
		}
		else
		{
			BackgroundMusicManager.Instance().StopBG();
		}
		Save.Write();
	}

	public void HandelAccountBtnEvent()
	{
		UIConstant.bNeedLoseConnect = true;
		if ((UtilUIAccountManager.mInstance.accountData.email != string.Empty && UtilUIAccountManager.mInstance.accountData.password != string.Empty) ? true : false)
		{
			UtilUIAccountManager.mInstance.HideAll();
			UtilUIAccountManager.mInstance.ShowScene(UtilUIAccountManager.UIScene.E_LOGOUT);
		}
		else
		{
			UtilUIAccountManager.mInstance.HideAll();
			UtilUIAccountManager.mInstance.ShowScene(UtilUIAccountManager.UIScene.E_ACCOUNT);
		}
	}

	public void HandleAnimationBtnEvent()
	{
		UIConstant.bNeedLoseConnect = false;
		Util.PlayCG(false);
	}

	public void Update()
	{
		if (m_gameObjectsMusic != null)
		{
			if (DataCenter.Save().PlayMusic)
			{
				if (m_gameObjectsMusic[0].transform.localPosition.x > -17f)
				{
					float num = 5f;
					m_gameObjectsMusic[0].transform.localPosition = new Vector3(m_gameObjectsMusic[0].transform.localPosition.x - num, m_gameObjectsMusic[0].transform.localPosition.y, m_gameObjectsMusic[0].transform.localPosition.z);
					if (m_gameObjectsMusic[0].transform.localPosition.x <= -17f)
					{
						m_gameObjectsMusic[0].transform.localPosition = new Vector3(-17f, m_gameObjectsMusic[0].transform.localPosition.y, m_gameObjectsMusic[0].transform.localPosition.z);
						m_labelTextMusic.text = "OFF";
						m_labelTextMusic.transform.localPosition = new Vector3(17f, m_labelTextMusic.transform.localPosition.y, m_labelTextMusic.transform.localPosition.z);
					}
				}
			}
			else if (m_gameObjectsMusic[0].transform.localPosition.x < 17f)
			{
				float num2 = 5f;
				m_gameObjectsMusic[0].transform.localPosition = new Vector3(m_gameObjectsMusic[0].transform.localPosition.x + num2, m_gameObjectsMusic[0].transform.localPosition.y, m_gameObjectsMusic[0].transform.localPosition.z);
				if (m_gameObjectsMusic[0].transform.localPosition.x >= 17f)
				{
					m_gameObjectsMusic[0].transform.localPosition = new Vector3(17f, m_gameObjectsMusic[0].transform.localPosition.y, m_gameObjectsMusic[0].transform.localPosition.z);
					m_labelTextMusic.text = "ON";
					m_labelTextMusic.transform.localPosition = new Vector3(-17f, m_labelTextMusic.transform.localPosition.y, m_labelTextMusic.transform.localPosition.z);
				}
			}
		}
		if (m_gameObjectsSound == null)
		{
			return;
		}
		if (DataCenter.Save().PlaySound)
		{
			if (m_gameObjectsSound[0].transform.localPosition.x > -17f)
			{
				float num3 = 5f;
				m_gameObjectsSound[0].transform.localPosition = new Vector3(m_gameObjectsSound[0].transform.localPosition.x - num3, m_gameObjectsSound[0].transform.localPosition.y, m_gameObjectsSound[0].transform.localPosition.z);
				if (m_gameObjectsSound[0].transform.localPosition.x <= -17f)
				{
					m_gameObjectsSound[0].transform.localPosition = new Vector3(-17f, m_gameObjectsSound[0].transform.localPosition.y, m_gameObjectsSound[0].transform.localPosition.z);
					m_labelTextSound.text = "OFF";
					m_labelTextSound.transform.localPosition = new Vector3(17f, m_labelTextSound.transform.localPosition.y, m_labelTextSound.transform.localPosition.z);
				}
			}
		}
		else if (m_gameObjectsSound[0].transform.localPosition.x < 17f)
		{
			float num4 = 5f;
			m_gameObjectsSound[0].transform.localPosition = new Vector3(m_gameObjectsSound[0].transform.localPosition.x + num4, m_gameObjectsSound[0].transform.localPosition.y, m_gameObjectsSound[0].transform.localPosition.z);
			if (m_gameObjectsSound[0].transform.localPosition.x >= 17f)
			{
				m_gameObjectsSound[0].transform.localPosition = new Vector3(17f, m_gameObjectsSound[0].transform.localPosition.y, m_gameObjectsSound[0].transform.localPosition.z);
				m_labelTextSound.text = "ON";
				m_labelTextSound.transform.localPosition = new Vector3(-17f, m_labelTextSound.transform.localPosition.y, m_labelTextSound.transform.localPosition.z);
			}
		}
	}

	public void HandleSoundPress()
	{
		DataCenter.Save().PlaySound = !DataCenter.Save().PlaySound;
		Save.Write();
	}
}

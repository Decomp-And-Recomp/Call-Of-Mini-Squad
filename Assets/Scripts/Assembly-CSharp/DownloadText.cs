using System.Collections;
using UnityEngine;

public class DownloadText : MonoBehaviour
{
	private string url = "http://account.trinitigame.com/game/CoMsquadAndroid/CoMSquadEncrypt.txt?time=";

	public HandlerEvent_VesionDownloadError m_DownLoadErrorEvent;

	public HandlerEvent_VesionDownloadOK m_DownLoadOKEvent;

	public HandlerEvent_DeafultEvent m_defaultEvent;

	private IEnumerator Start()
	{
		/*WWW www = new WWW(url + Random.Range(1000, 1000000));
		yield return www;
		if (www.error != null)
		{
			if (m_DownLoadErrorEvent != null)
			{
				m_DownLoadErrorEvent(www.error);
			}
		}
		else if (m_DownLoadOKEvent != null)
		{
			m_DownLoadOKEvent(www.text);
		}
		if (m_defaultEvent != null)
		{
			m_defaultEvent();
		}
		www.Dispose();*/
		yield return null;
	}
}

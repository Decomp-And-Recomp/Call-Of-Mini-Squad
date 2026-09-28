using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
	private static SceneManager mInstance;

	private static string strSceneName = string.Empty;

	private Stack<string> stackScenes = new Stack<string>();

	public static SceneManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = GameObject.Find("Fade Anchor").GetComponent<SceneManager>();
			}
			return mInstance;
		}
	}

	protected void Clear()
	{
		stackScenes.Clear();
	}

	protected void Push(string sceneName)
	{
		stackScenes.Push(sceneName);
	}

	protected string Pop()
	{
		if (stackScenes.Count > 0)
		{
			return stackScenes.Pop();
		}
		return "UIBase";
	}

	public void SwitchScene(string str = "")
	{
		string empty = string.Empty;
		if (str == "UIBase")
		{
			Clear();
		}
		if (str != string.Empty)
		{
			Push(str);
			empty = str;
		}
		else
		{
			Pop();
			empty = GetCurrentScene();
		}
		strSceneName = empty;
		FadeInfoScript.Instance.FadeOut();
		FadeInfoScript.Instance.SubFinishEvent(base.gameObject, "LoadScene");
	}

	public string GetCurrentScene()
	{
		if (stackScenes.Count > 0)
		{
			return stackScenes.Peek();
		}
		return "UIBase";
	}

	private void LoadScene()
	{
		FadeInfoScript.Instance.SubFinishEvent(null, string.Empty);
		Application.LoadLevel(strSceneName);
	}

	private void Start()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDestroy()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		if (!isActiveAndEnabled)
		{
			return;
		}
		int level = scene.buildIndex;
		if (level != 0 && (Application.loadedLevelName.StartsWith("UI") || Application.loadedLevelName.StartsWith("Load")))
		{
			if (Application.loadedLevelName.StartsWith("Load"))
			{
				FadeInfoScript.Instance.FadeIn(0.1f);
			}
			else
			{
				FadeInfoScript.Instance.FadeIn();
			}
			FadeInfoScript.Instance.SubFinishEvent(null, string.Empty);
		}
	}
}

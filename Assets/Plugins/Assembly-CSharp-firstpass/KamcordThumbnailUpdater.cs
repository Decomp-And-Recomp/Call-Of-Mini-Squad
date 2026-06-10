using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class KamcordThumbnailUpdater : MonoBehaviour
{
	public Texture2D playButtonTexture;

	public float thumbnailRelativeX = 0.25f;

	public float thumbnailRelativeY = 0.25f;

	public float thumbnailToScreenRatio = 0.4f;

	//private GUITexture theGuiTexture;

	private float playButtonToThumbnailRatio = 0.5f;

	private Rect playButtonLocationAndSize;

	public void EnableThumbnail(bool enable)
	{
		//if (theGuiTexture != null)
		{
			//theGuiTexture.enabled = enable;
		}
	}

	private void Start()
	{
        /*base.gameObject.AddComponent<GUITexture>();
		GUITexture[] components = base.gameObject.GetComponents<GUITexture>();
		if (components.Length == 0)
		{
			throw new Exception("Kamcord script " + base.name + " needs to have at least one GUITexture component on the attached game object named: " + base.gameObject.name);
		}
		theGuiTexture = components[0];*/
        Kamcord.videoThumbnailReadyAtFilePath += VideoThumbnailReadyAtFilePath;
		EnableThumbnail(false);
	}

	private void OnDestroy()
	{
		Kamcord.videoThumbnailReadyAtFilePath -= VideoThumbnailReadyAtFilePath;
	}

	private void Update()
	{
		//if (!(theGuiTexture != null))
		//{
		//	return;
		//}
		Touch[] touches = Input2.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			//if (touch.phase == TouchPhase.Began && theGuiTexture.HitTest(touch.position))
			{
				Kamcord.ShowView();
				break;
			}
		}
	}

	private void OnGUI()
	{
		//if (theGuiTexture != null && theGuiTexture.enabled)
		{
			GUI.Label(playButtonLocationAndSize, playButtonTexture);
		}
	}

	public void VideoThumbnailReadyAtFilePath(string filepath)
	{
		if (File.Exists(filepath))
		{
			SetThumbnailTextureToFilepath(filepath);
		}
	}

	private IEnumerator WaitForLoadToFinishAndThenSetThumbnail(WWW loader)
	{
		yield return loader;
		if (loader.error == null)
		{
			if (thumbnailToScreenRatio < 0.2f)
			{
				thumbnailToScreenRatio = 0.2f;
			}
			float absoluteX = thumbnailRelativeX * (float)Screen.width;
			float absoluteY = thumbnailRelativeY * (float)Screen.height;
			float absoluteWidth = thumbnailToScreenRatio * (float)Screen.width;
			float absoluteHeight = thumbnailToScreenRatio * (float)Screen.height;
			float playButtonWidth = Mathf.Min(playButtonTexture.width, playButtonToThumbnailRatio * absoluteWidth);
			float playButtonHeight = playButtonWidth;
			float playButtonAbsoluteX = absoluteX + 0.5f * (absoluteWidth - playButtonWidth);
			float playButtonAbsoluteY = (float)Screen.height - absoluteY - 0.5f * (absoluteHeight + playButtonHeight);
			playButtonLocationAndSize = new Rect(playButtonAbsoluteX, playButtonAbsoluteY, playButtonWidth, playButtonHeight);
			base.transform.position = Vector3.zero;
			base.transform.localScale = Vector3.zero;
			//theGuiTexture.pixelInset = new Rect(absoluteX, absoluteY, absoluteWidth, absoluteHeight);
			//theGuiTexture.texture = loader.texture;
			EnableThumbnail(true);
		}
	}

	private void SetThumbnailTextureToFilepath(string filepath)
	{
		WWW loader = new WWW("file://" + filepath);
		StartCoroutine(WaitForLoadToFinishAndThenSetThumbnail(loader));
	}
}

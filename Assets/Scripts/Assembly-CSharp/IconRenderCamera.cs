using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class IconRenderCamera : MonoBehaviour
{
	[SerializeField]
	private RenderTexture texture;

	[SerializeField]
	private Vector2 iconSize;

	public int paddingX = 2;

	public int paddingY = 2;

	private Camera camr;

	private UIGrid grid;

	private List<Transform> anchors = new List<Transform>();

	public Texture Texture
	{
		get
		{
			return texture;
		}
	}

	public int AnchorNum
	{
		get
		{
			return anchors.Count;
		}
	}

	public void Render(bool autoClose = false)
	{
		camr.enabled = true;
		StopAllCoroutines();
		if (autoClose)
		{
			StartCoroutine(CloseRenderCamera());
		}
	}

	public int GetEmptyIndex()
	{
		int result = -1;
		for (int i = 0; i < anchors.Count; i++)
		{
			if (IsEmpty(i))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public bool IsEmpty(int index)
	{
		return (IndexIsVaild(index) && anchors[index].childCount == 0) || 0 == anchors[index].GetChild(0).childCount;
	}

	public void Set(int anchorIndex, GameObject obj)
	{
		if (!IndexIsVaild(anchorIndex))
		{
			return;
		}
		obj.transform.parent = ClearObj(anchors[anchorIndex]);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		if (componentsInChildren != null)
		{
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.gameObject.layer = camr.gameObject.layer;
			}
		}
		obj.layer = camr.gameObject.layer;
	}

	public GameObject Get(int anchorIndex)
	{
		GameObject result = null;
		if (IndexIsVaild(anchorIndex))
		{
			result = GetObj(anchors[anchorIndex]);
		}
		return result;
	}

	public bool GetUV(int anchorIndex, ref Rect uv)
	{
		bool flag = IndexIsVaild(anchorIndex);
		if (flag)
		{
			Vector2 vector = camr.WorldToScreenPoint(anchors[anchorIndex].position);
			Rect rect = new Rect(vector.x - iconSize.x * 0.5f, vector.y - iconSize.y * 0.5f, iconSize.x, iconSize.y);
			uv = new Rect(rect.x / (float)texture.width, rect.y / (float)texture.height, rect.width / (float)texture.width, rect.height / (float)texture.height);
		}
		return flag;
	}

	public void Remove(int anchorIndex)
	{
		if (IndexIsVaild(anchorIndex))
		{
			ClearObj(anchors[anchorIndex]);
		}
	}

	public void Clear(int except = -1)
	{
		for (int i = 0; i < anchors.Count; i++)
		{
			if (except != i)
			{
				ClearObj(anchors[i]);
			}
		}
	}

	private void Awake()
	{
		camr = GetComponent<Camera>();
		GameObject gameObject = new GameObject("Grid");
		grid = gameObject.AddComponent<UIGrid>();
		grid.transform.parent = base.transform;
		grid.cellWidth = iconSize.x + (float)paddingX;
		grid.cellHeight = iconSize.y + (float)paddingY;
		grid.maxPerLine = Mathf.FloorToInt((float)texture.width / grid.cellWidth);
		int num = Mathf.FloorToInt((float)texture.height / grid.cellHeight);
		int num2 = grid.maxPerLine * num;
		for (int i = 0; i < num2; i++)
		{
			gameObject = new GameObject("Anchor" + i);
			anchors.Add(gameObject.transform);
			anchors[i].parent = grid.transform;
		}
		grid.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
		grid.repositionNow = true;
		camr.aspect = (float)texture.width * 1f / (float)texture.height;
		camr.orthographicSize = (float)texture.height * 0.5f;
		camr.targetTexture = texture;
		if (null != grid)
		{
			float x = ((float)(-texture.width) + grid.cellWidth) * 0.5f;
			float y = ((float)texture.height - grid.cellHeight) * 0.5f;
			grid.transform.localPosition = new Vector2(x, y);
		}
	}

	private bool IndexIsVaild(int anchorIndex)
	{
		return anchorIndex > -1 && anchorIndex < AnchorNum;
	}

	private GameObject GetObj(Transform trans)
	{
		GameObject result = null;
		if (trans.childCount > 0)
		{
			result = trans.GetChild(0).gameObject;
		}
		return result;
	}

	private Transform ClearObj(Transform trans)
	{
		if (trans.childCount != 0)
		{
			Transform child = trans.GetChild(0);
			child.parent = null;
			Object.Destroy(child.gameObject);
		}
		GameObject gameObject = new GameObject("anchor");
		Transform transform = gameObject.transform;
		transform.parent = trans;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		return transform;
	}

	private IEnumerator CloseRenderCamera()
	{
		yield return null;
		camr.enabled = false;
	}
}

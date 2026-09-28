using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class MinimapNGUI : MonoBehaviour
{
	public struct WalkwayInfo
	{
		public Rect bound;

		public float rotationY;
	}

	private List<GameObject> m_rooms;

	private Rect m_sceneSize;

	public Rect m_viewSize;

	private Vector2 m_viewPos;

	private Vector2 m_playerPos;

	private bool m_bOverall;

	private DS2ActiveObject[] m_allysObj;

	private GameObject m_targetObj;

	private float m_scale = 1f;

	private Rect m_oringinalView;

	private GameObject m_palyer;

	private UISprite m_targetMarkTex;

	private UISprite[] m_allyMarksTex;

	private Dictionary<int, UISprite> m_spawnPointTex;

	private Dictionary<int, Vector3> m_mainPointPosition;

	private List<WalkwayInfo> m_walkwayMarks;

	private int m_allyBuffNum = 7;

	private Texture m_texturePack;

	public static MinimapNGUI instance;

	private float m_rutorialTimer;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		m_rooms = new List<GameObject>(GameObject.FindGameObjectsWithTag("NodeGroup"));
		m_walkwayMarks = new List<WalkwayInfo>();
		float b = (float)Screen.width / 960f;
		b = Mathf.Min(1.5f, b);
		m_scale = Mathf.Max(1f, b) * 1.4f / m_rooms[0].transform.root.localScale.x;
		float num = 9999.9f;
		float num2 = -9999.9f;
		float num3 = 9999.9f;
		float num4 = -9999.9f;
		foreach (GameObject room in m_rooms)
		{
			Renderer component = room.GetComponent<Renderer>();
			Bounds bounds = component.bounds;
			if (num > bounds.min.x)
			{
				num = bounds.min.x;
			}
			if (num2 < bounds.max.x)
			{
				num2 = bounds.max.x;
			}
			if (num3 > bounds.min.z)
			{
				num3 = bounds.min.z;
			}
			if (num4 < bounds.max.z)
			{
				num4 = bounds.max.z;
			}
			Transform transform = room.transform.parent.Find("WalkWay");
			if (transform != null)
			{
				GameObject gameObject = transform.gameObject;
				BoxCollider[] componentsInChildren = gameObject.GetComponentsInChildren<BoxCollider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					WalkwayInfo item = default(WalkwayInfo);
					item.bound = new Rect(componentsInChildren[i].transform.position.x, componentsInChildren[i].transform.position.z, componentsInChildren[i].size.x * gameObject.transform.root.localScale.x, componentsInChildren[i].size.z * gameObject.transform.root.localScale.z);
					item.rotationY = componentsInChildren[i].transform.rotation.eulerAngles.y;
					m_walkwayMarks.Add(item);
				}
				Object.DestroyImmediate(transform.gameObject);
			}
		}
		m_oringinalView = new Rect(num, num3, Mathf.Abs(num2 - num), Mathf.Abs(num4 - num3));
		float left = m_oringinalView.xMin * m_scale;
		float top = m_oringinalView.yMin * m_scale;
		float width = m_oringinalView.width * m_scale;
		float height = m_oringinalView.height * m_scale;
		m_sceneSize = new Rect(left, top, width, height);
		m_viewSize = new Rect(Screen.width - 160, 20f, 120f, 120f);
		m_viewPos = default(Vector2);
		m_targetObj = GameObject.FindGameObjectWithTag("Target");
		if (m_bOverall)
		{
			StateToOverall();
		}
		else
		{
			StateToScroll();
		}
		GameObject gameObject2 = base.transform.Find("bg").gameObject;
		GameObject gameObject3 = base.transform.Find("room").gameObject;
		GameObject palyer = base.transform.Find("player").gameObject;
		GameObject gameObject4 = base.transform.Find("ally").gameObject;
		GameObject gameObject5 = base.transform.Find("spawnPoint").gameObject;
		UISprite component2 = gameObject2.GetComponent<UISprite>();
		m_palyer = palyer;
		m_allyMarksTex = new UISprite[m_allyBuffNum];
		for (int j = 0; j < m_allyMarksTex.Length; j++)
		{
			GameObject gameObject6 = Object.Instantiate(gameObject4) as GameObject;
			gameObject6.name = "allyMark";
			gameObject6.transform.parent = gameObject4.transform.parent;
			gameObject6.transform.localScale = Vector3.one;
			m_allyMarksTex[j] = gameObject6.GetComponent<UISprite>();
			gameObject6.SetActive(false);
		}
		Object.DestroyImmediate(gameObject4);
		m_viewSize = new Rect(0f - m_sceneSize.width - 10f, 0f - m_sceneSize.height - 10f, m_sceneSize.width, m_sceneSize.height);
		gameObject2.transform.localPosition = new Vector3(m_viewSize.x + m_sceneSize.width / 2f, m_viewSize.y + m_sceneSize.height / 2f, 0f);
		component2.width = (int)(m_sceneSize.width + 10f);
		component2.height = (int)(m_sceneSize.height + 10f);
		foreach (WalkwayInfo walkwayMark in m_walkwayMarks)
		{
			GameObject gameObject7 = Object.Instantiate(gameObject3) as GameObject;
			gameObject7.name = "room";
			gameObject7.transform.parent = base.transform;
			gameObject7.transform.localScale = Vector3.one;
			gameObject7.transform.localRotation = Quaternion.Euler(0f, 0f, walkwayMark.rotationY);
			left = walkwayMark.bound.x * m_scale;
			top = walkwayMark.bound.y * m_scale;
			width = walkwayMark.bound.width * m_scale;
			height = walkwayMark.bound.height * m_scale;
			Rect rect = new Rect(left - m_sceneSize.x, top - m_sceneSize.y, width, height);
			gameObject7.transform.localPosition = new Vector3(m_viewSize.x + rect.x, m_viewSize.y + rect.y, 0f);
			UISprite component3 = gameObject7.GetComponent<UISprite>();
			component3.width = Mathf.FloorToInt(rect.width);
			component3.height = Mathf.FloorToInt(rect.height);
		}
		Object.DestroyImmediate(gameObject3);
		GameObject gameObject8 = GameObject.Find("EnemySpawnPoint/SpawnPointChain");
		EnemySpawnPointMain[] componentsInChildren2 = gameObject8.GetComponentsInChildren<EnemySpawnPointMain>();
		m_spawnPointTex = new Dictionary<int, UISprite>();
		m_mainPointPosition = new Dictionary<int, Vector3>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			GameObject gameObject9 = Object.Instantiate(gameObject5) as GameObject;
			gameObject9.name = "spawnPoint";
			gameObject9.transform.parent = gameObject5.transform.parent;
			gameObject9.transform.localScale = Vector3.one;
			UISprite component4 = gameObject9.GetComponent<UISprite>();
			component4.width = component4.width;
			component4.height = component4.height;
			gameObject9.transform.localPosition = new Vector3(m_viewSize.x + componentsInChildren2[k].transform.position.x * m_scale - m_sceneSize.x, m_viewSize.y + componentsInChildren2[k].transform.position.z * m_scale - m_sceneSize.y);
			m_spawnPointTex.Add(componentsInChildren2[k].gameObject.GetInstanceID(), component4);
			m_mainPointPosition.Add(componentsInChildren2[k].storyPoint, componentsInChildren2[k].transform.position);
		}
		Object.DestroyImmediate(gameObject5);
	}

	private void Update()
	{
		m_allysObj = GameBattle.m_instance.GetTeammateList();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		m_playerPos = new Vector2(GameBattle.m_instance.GetPlayer().GetTransform().position.x, GameBattle.m_instance.GetPlayer().GetTransform().position.z);
		num = m_playerPos.x * m_scale;
		num2 = m_playerPos.y * m_scale;
		m_palyer.transform.localPosition = new Vector3(m_viewSize.x + num - m_sceneSize.x, m_viewSize.y + (num2 - m_sceneSize.y));
		m_palyer.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - GameBattle.m_instance.GetPlayer().GetTransform().eulerAngles.y);
		int num5 = 0;
		for (int i = 0; i < m_allysObj.Length; i++)
		{
			Player player2 = m_allysObj[i] as Player;
			if (player2 != null && !player2.CurrentController)
			{
				if (num5 < m_allyMarksTex.Length)
				{
					m_allyMarksTex[num5].gameObject.SetActive(true);
					Vector2 vector = new Vector2(m_allysObj[i].GetTransform().position.x, m_allysObj[i].GetTransform().position.z);
					num = vector.x * m_scale;
					num2 = vector.y * m_scale;
					num3 = m_allyMarksTex[num5].width;
					num4 = m_allyMarksTex[num5].height;
					m_allyMarksTex[num5].transform.localPosition = new Vector3(m_viewSize.x + num - m_sceneSize.x - num3 * 0.5f, m_viewSize.y + (num2 - m_sceneSize.y) - num4 * 0.5f);
					num5++;
				}
			}
		}
		for (int j = num5; j < m_allyMarksTex.Length; j++)
		{
			if (m_allyMarksTex[j].gameObject.activeSelf)
			{
				m_allyMarksTex[j].gameObject.SetActive(false);
			}
		}
	}

	private void OnGUI()
	{
	}

	private void StateToOverall()
	{
		m_bOverall = true;
		m_viewPos.x = 0f;
		m_viewPos.y = 0f;
		float left = m_oringinalView.xMin * m_scale;
		float top = m_oringinalView.yMin * m_scale;
		float width = m_oringinalView.width * m_scale;
		float height = m_oringinalView.height * m_scale;
		m_sceneSize = new Rect(left, top, width, height);
		m_viewSize = new Rect((float)Screen.width - m_sceneSize.width - 40f, 20f, m_sceneSize.width, m_sceneSize.height);
	}

	private void StateToScroll()
	{
		m_bOverall = false;
		m_viewSize = new Rect(Screen.width - 160, 20f, 120f, 120f);
	}

	private void DrawTexture(Rect destinationPixel, Rect sourcePixel, ref Texture tex, Color c)
	{
		int width = tex.width;
		int height = tex.height;
		sourcePixel.x /= width;
		sourcePixel.y = 1f - (sourcePixel.y + sourcePixel.height) / (float)height;
		sourcePixel.width /= width;
		sourcePixel.height /= height;
		GUI.color = c;
		GUI.DrawTextureWithTexCoords(destinationPixel, tex, sourcePixel, false);
		GUI.color = Color.white;
	}

	public void SetSpawnPointEnable(int gameObjectID, bool enable)
	{
		if (m_spawnPointTex.ContainsKey(gameObjectID))
		{
			m_spawnPointTex[gameObjectID].gameObject.SetActive(enable);
			m_spawnPointTex.Remove(gameObjectID);
		}
	}

	public void SetSpawnPointInBattle(int gameObjectID)
	{
		if (m_spawnPointTex.ContainsKey(gameObjectID))
		{
			m_spawnPointTex[gameObjectID].spriteName = "pic_decal006";
		}
	}

	public int GetSpawnPointLeftCount()
	{
		return m_spawnPointTex.Count;
	}

	public Vector3 GetCurrentMainPointPosition()
	{
		if (m_mainPointPosition.ContainsKey(GameBattle.s_storyPoint))
		{
			return m_mainPointPosition[GameBattle.s_storyPoint];
		}
		return Vector3.zero;
	}
}

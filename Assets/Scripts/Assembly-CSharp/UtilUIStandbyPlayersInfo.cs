using System.Collections.Generic;
using UnityEngine;

public class UtilUIStandbyPlayersInfo : MonoBehaviour
{
	public class PLAYERINFO
	{
		public UtilUIStandbyPlayerAppearance ui;

		public UtilUIStandbyPlayerAppearanceData data;

		public UtilUIStandbyPlayerFormationControl formationControl;

		public GameObject go;

		public int indexOfList = -1;

		public PLAYERINFO(GameObject go, int indexOfList)
		{
			this.go = go;
			this.indexOfList = indexOfList;
			ui = go.GetComponent<UtilUIStandbyPlayerAppearance>();
			data = go.GetComponent<UtilUIStandbyPlayerAppearanceData>();
			formationControl = go.transform.Find("Arrow").GetComponent<UtilUIStandbyPlayerFormationControl>();
		}
	}

	public UITeamModelManager m_modelManagerScript;

	[SerializeField]
	private UIScrollView scrollView;

	[SerializeField]
	private UIGrid grid;

	[SerializeField]
	private AutoCreatByPrefab autoCreat;

	[SerializeField]
	private GameObject selectPlayerEffectPrefab;

	[SerializeField]
	private UIToggle[] m_teamPlayersToggle;

	[SerializeField]
	private UIScrollBar scrollBar;

	private bool bInit;

	protected Dictionary<int, PLAYERINFO> dictPlayersInfo = new Dictionary<int, PLAYERINFO>();

	protected Dictionary<GameObject, int> dictMapPlayerInfo = new Dictionary<GameObject, int>();

	protected List<int> lsSortOrder = new List<int>();

	private int needSelectStoreItemIndex = -1;

	protected int nowSelectStoreItemIndex = -1;

	protected List<int> lsVisablPlayersAppearance = new List<int>();

	protected Vector3 lastScrollViewPosition = new Vector3(99999f, 999f, 999f);

	private UtilUIStandbyPlayersInfo_AppearanceItemBtnClicked_Delegate ItemClickedEvent;

	private UtilUIStandbyPlayersInfo_AppearanceItemUnlockBtnClicked_Delegate ItemUnlockClickedEvent;

	private UtilUIStandbyPlayersInfo_FormationControlOnPressTouchGO FormationControlPressTouchEvent;

	private UtilUIStandbyPlayersInfo_FormationControlOnDragDropStartTouchGO FormationControlDropStartTouchEvent;

	private UtilUIStandbyPlayersInfo_FormationControlOnDragDropMovedTouchGO FormationControlDropMovedTouchEvent;

	private UtilUIStandbyPlayersInfo_FormationControlOnDragDropReleasedTouchGO FormationControlDropReleasedTouchEvent;

	public int GetNowSelectItemIndex()
	{
		return nowSelectStoreItemIndex;
	}

	public PLAYERINFO GetPlayerInfoById(int id)
	{
		return dictPlayersInfo[id];
	}

	private void Start()
	{
		if (!scrollBar.enabled)
		{
			scrollBar.enabled = true;
		}
	}

	public void Init(PlayerData[] lsAllNotUseHero, TeamData td, int selectID)
	{
		UIWidget component = autoCreat.Prefab.GetComponent<UIWidget>();
		autoCreat.Prefab.SetActive(true);
		grid.cellWidth = component.width;
		grid.cellHeight = component.height;
		autoCreat.Prefab.SetActive(false);
		for (int i = 0; i < lsAllNotUseHero.Length; i++)
		{
			PlayerData playerData = lsAllNotUseHero[i];
			if (!dictPlayersInfo.ContainsKey(playerData.heroIndex) && playerData.unlockNeedTeamLevel != -4)
			{
				DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerData.heroIndex);
				CreateItem(playerData.heroIndex, i);
				SerializeItem(playerData, heroDataByIndex, new Vector3(component.width, component.height, 0f));
				UpdateBaseUI(playerData.heroIndex, false);
			}
		}
		grid.repositionNow = true;
		scrollView.onDragFinished = HandleScrollViewDragFinishEvent;
		needSelectStoreItemIndex = selectID;
		grid.onReposition = InitOnGridFinished;
		UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 41);
		UIDialogManager.Instance.ShowBlock(41);
		bInit = true;
	}

	public void InitOnGridFinished()
	{
		GameObject targetGO = null;
		PLAYERINFO pLAYERINFO = dictPlayersInfo[needSelectStoreItemIndex];
		if (pLAYERINFO.data.SITEINDEX != -1)
		{
			targetGO = m_teamPlayersToggle[pLAYERINFO.data.SITEINDEX].gameObject;
		}
		SetPlayerInTheTargetGOCenterOfTheView(needSelectStoreItemIndex, targetGO);
		grid.onReposition = null;
		HandleScrollViewDragFinishEvent();
	}

	public void SelectPlayerInTheCenterOfTheView(int index)
	{
		needSelectStoreItemIndex = index;
		GameObject targetGO = null;
		PLAYERINFO pLAYERINFO = dictPlayersInfo[needSelectStoreItemIndex];
		if (pLAYERINFO.data.SITEINDEX != -1)
		{
			targetGO = m_teamPlayersToggle[pLAYERINFO.data.SITEINDEX].gameObject;
		}
		SetPlayerInTheTargetGOCenterOfTheView(index, targetGO);
	}

	public void SetStandbyPlayerAppearanceItemBtnClickedDelegate(UtilUIStandbyPlayersInfo_AppearanceItemBtnClicked_Delegate dele)
	{
		ItemClickedEvent = dele;
	}

	public void SetStandbyPlayerAppearanceItemUnlockBtnClickedDelegate(UtilUIStandbyPlayersInfo_AppearanceItemUnlockBtnClicked_Delegate dele)
	{
		ItemUnlockClickedEvent = dele;
	}

	public void BlindFunction(UtilUIStandbyPlayersInfo_FormationControlOnPressTouchGO press, UtilUIStandbyPlayersInfo_FormationControlOnDragDropStartTouchGO start, UtilUIStandbyPlayersInfo_FormationControlOnDragDropMovedTouchGO move, UtilUIStandbyPlayersInfo_FormationControlOnDragDropReleasedTouchGO release)
	{
		FormationControlPressTouchEvent = press;
		FormationControlDropStartTouchEvent = start;
		FormationControlDropMovedTouchEvent = move;
		FormationControlDropReleasedTouchEvent = release;
	}

	protected void SelectPlayer(int index)
	{
		if (index == nowSelectStoreItemIndex)
		{
			return;
		}
		if (nowSelectStoreItemIndex != -1)
		{
			GameObject mODELGO = dictPlayersInfo[nowSelectStoreItemIndex].data.MODELGO;
			if (mODELGO != null)
			{
				SetPlayerModelHaloEffectVisable(false, mODELGO, selectPlayerEffectPrefab);
				SetPlayerModelOutLineEffectVisable(false, mODELGO);
			}
		}
		nowSelectStoreItemIndex = index;
		needSelectStoreItemIndex = index;
		if (dictPlayersInfo[nowSelectStoreItemIndex].data.MODELGO == null)
		{
			CreatePlayersModel(nowSelectStoreItemIndex, nowSelectStoreItemIndex);
		}
		GameObject mODELGO2 = dictPlayersInfo[nowSelectStoreItemIndex].data.MODELGO;
		if (mODELGO2 == null)
		{
			return;
		}
		SetPlayerModelHaloEffectVisable(true, mODELGO2, selectPlayerEffectPrefab);
		SetPlayerModelOutLineEffectVisable(true, mODELGO2);
		AudioTalkManager componentInChildren = mODELGO2.GetComponentInChildren<AudioTalkManager>();
		if (componentInChildren != null)
		{
			componentInChildren.PlaySelect();
		}
		UITeamModelAnimationManager componentInChildren2 = mODELGO2.GetComponentInChildren<UITeamModelAnimationManager>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.PlayShowAnim(false);
		}
	}

	protected void SetPlayerInTheTargetGOCenterOfTheView(int index, GameObject targetGO)
	{
		if (!dictPlayersInfo.ContainsKey(index))
		{
			return;
		}
		PLAYERINFO pLAYERINFO = dictPlayersInfo[index];
		UIPanel panel = scrollView.panel;
		if (panel != null && panel.clipping != 0)
		{
			UIScrollView uIScrollView = scrollView;
			Vector3 vector = -panel.cachedTransform.InverseTransformPoint(pLAYERINFO.go.transform.position);
			Vector3 vector2 = NGUITools.GetRoot(base.gameObject).transform.InverseTransformPoint(targetGO.transform.position);
			Vector3 pos = new Vector3(vector.x + vector2.x, vector.y, vector.z);
			if (!uIScrollView.canMoveHorizontally)
			{
				pos.x = panel.cachedTransform.localPosition.x;
			}
			if (!uIScrollView.canMoveVertically)
			{
				pos.y = panel.cachedTransform.localPosition.y;
			}
			SpringPanel.Begin(panel.cachedGameObject, pos, 6f);
			SpringPanel component = uIScrollView.gameObject.GetComponent<SpringPanel>();
			uIScrollView.gameObject.GetComponent<SpringPanel>().onFinished = HandleScrollViewDragFinishEvent;
		}
	}

	protected void SetPlayerInTheCenterOfTheView(int index, List<int> _ls)
	{
		if (!dictPlayersInfo.ContainsKey(index))
		{
			return;
		}
		PLAYERINFO pLAYERINFO = dictPlayersInfo[index];
		UIPanel panel = scrollView.panel;
		if (panel != null && panel.clipping != 0)
		{
			UIScrollView uIScrollView = scrollView;
			Vector3 pos = -panel.cachedTransform.InverseTransformPoint(pLAYERINFO.go.transform.position);
			float num = pLAYERINFO.ui.m_widget.width;
			int num2 = Mathf.CeilToInt((float)Screen.width / 2f / num);
			if (pLAYERINFO.indexOfList + 1 < num2)
			{
				PLAYERINFO pLAYERINFO2 = dictPlayersInfo[_ls[0]];
				pos = -panel.cachedTransform.InverseTransformPoint(pLAYERINFO2.go.transform.position);
				float num3 = (float)Screen.width / 2f - num / 2f;
				pos = new Vector3(pos.x - num3, pos.y, pos.z);
				lastScrollViewPosition = new Vector3(99999f, 999f, 999f);
			}
			else if (pLAYERINFO.indexOfList + 1 > _ls.Count - num2)
			{
				PLAYERINFO pLAYERINFO3 = dictPlayersInfo[_ls[_ls.Count - 1]];
				pos = -panel.cachedTransform.InverseTransformPoint(pLAYERINFO3.go.transform.position);
				float num4 = (float)Screen.width / 2f - num / 2f;
				pos = new Vector3(pos.x + num4, pos.y, pos.z);
				lastScrollViewPosition = new Vector3(99999f, 999f, 999f);
			}
			if (!uIScrollView.canMoveHorizontally)
			{
				pos.x = panel.cachedTransform.localPosition.x;
			}
			if (!uIScrollView.canMoveVertically)
			{
				pos.y = panel.cachedTransform.localPosition.y;
			}
			SpringPanel.Begin(panel.cachedGameObject, pos, 6f);
			SpringPanel component = uIScrollView.gameObject.GetComponent<SpringPanel>();
			uIScrollView.gameObject.GetComponent<SpringPanel>().onFinished = HandleScrollViewDragFinishEvent;
		}
	}

	public static void SetPlayerModelHaloEffectVisable(bool bShow, GameObject go, GameObject _selectPlayerEffectPrefab)
	{
		if (go != null)
		{
			Transform transform = go.transform.Find(_selectPlayerEffectPrefab.name + "(Clone)");
			if (transform != null)
			{
				Object.DestroyImmediate(transform.gameObject);
				if (bShow)
				{
					UIUtil.PDebug("Create more effect go in model");
				}
			}
		}
		if (bShow && go != null)
		{
			GameObject gameObject = Object.Instantiate(_selectPlayerEffectPrefab) as GameObject;
			gameObject.transform.parent = go.transform;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
		}
	}

	public static void SetPlayerModelOutLineEffectVisable(bool bShow, GameObject go, float outLine = 5f)
	{
		if (go == null) return;
		SkinnedMeshRenderer[] componentsInChildren = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		MeshRenderer[] componentsInChildren2 = go.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (bShow)
			{
				Material material = componentsInChildren[i].materials[0];
				Material material2 = new Material(material);
				material2.shader = Shader.Find("Triniti/Model/CartoonRendering");
				material2.SetColor("_OutlineColor", new Color(1f, 0.79607844f, 6f / 85f, 1f));
				material2.SetFloat("_Outline", outLine);
				Material[] materials = new Material[2] { material, material2 };
				componentsInChildren[i].materials = materials;
			}
			else
			{
				Material material3 = componentsInChildren[i].materials[0];
				Material[] materials2 = new Material[1] { material3 };
				componentsInChildren[i].materials = materials2;
			}
		}
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (bShow)
			{
				Material material4 = componentsInChildren2[j].materials[0];
				Material material5 = new Material(material4);
				material5.shader = Shader.Find("Triniti/Model/CartoonRendering");
				material5.SetColor("_OutlineColor", new Color(1f, 0.79607844f, 6f / 85f, 1f));
				material5.SetFloat("_Outline", outLine);
				Material[] materials3 = new Material[2] { material4, material5 };
				componentsInChildren2[j].materials = materials3;
			}
			else
			{
				Material material6 = componentsInChildren2[j].materials[0];
				Material[] materials4 = new Material[1] { material6 };
				componentsInChildren2[j].materials = materials4;
			}
		}
	}

	protected void CreateItem(int id, int indexOfList)
	{
		GameObject gameObject = autoCreat.CreatePefab(indexOfList);
		PLAYERINFO value = new PLAYERINFO(gameObject, indexOfList);
		gameObject.SetActive(true);
		lsSortOrder.Add(id);
		dictPlayersInfo.Add(id, value);
		dictMapPlayerInfo.Add(gameObject, id);
	}

	protected void SerializeItem(PlayerData pd, DataConf.HeroData heroConf, Vector3 arrowBoxCollider)
	{
		PLAYERINFO pLAYERINFO = dictPlayersInfo[pd.heroIndex];
		pLAYERINFO.data.Init(pd, heroConf);
		pLAYERINFO.ui.SetPlayerAppearanceItemBtnClickedDelegate(HandlePlayerAppearanceItemBtnClickedEvent);
		pLAYERINFO.ui.SetPlayerAppearanceUnlockBtnClickedDelegate(HandlePlayerAppearanceItemUnlockBtnClickedEvent);
		pLAYERINFO.formationControl.BlindFuntion(HandlePlayerFormationControlPressEvent, HandlePlayerFormationControlDragStartEvent, HandlePlayerFormationControlDragMovedEvent, HandlePlayerFormationControlDragReleasedEvent);
		UIWidget component = autoCreat.Prefab.GetComponent<UIWidget>();
		pLAYERINFO.formationControl.gameObject.GetComponent<BoxCollider>().size = arrowBoxCollider;
	}

	public void UpdateAllStandbyPlayersUI()
	{
		foreach (KeyValuePair<int, PLAYERINFO> item in dictPlayersInfo)
		{
			UpdateBaseUI(item.Key, false);
		}
	}

	public void UpdateBaseUI(int index, bool bAutoHide = true)
	{
		PLAYERINFO pLAYERINFO = dictPlayersInfo[index];
		pLAYERINFO.ui.SetInTeamTagVisable(pLAYERINFO.data.SITEINDEX != -1);
		pLAYERINFO.ui.UpdatePlayerAppearanceState(pLAYERINFO.data.STATE);
		pLAYERINFO.ui.UpdateInActiveCostIcon(UIUtil.GetCurrencyIconSpriteNameByCurrencyType(pLAYERINFO.data.PLAYERDATA.costType));
		pLAYERINFO.ui.UpdateInActiveCost(pLAYERINFO.data.INACTIVEMONEY.ToString("###,###"));
		pLAYERINFO.ui.UpdateCombat(pLAYERINFO.data.PLAYERDATA.combat.ToString("###,###"));
		if (pLAYERINFO.data.UNLOCKLEVEL == -1)
		{
			pLAYERINFO.ui.UpdateUnlockedLevel("Unlock by completing \"Tcrystal Collector\"");
		}
		else if (pLAYERINFO.data.UNLOCKLEVEL == -2)
		{
			pLAYERINFO.ui.UpdateUnlockedLevel("Unlock by completing \"Pro Pack\"");
		}
		else if (pLAYERINFO.data.UNLOCKLEVEL == -3)
		{
			pLAYERINFO.ui.UpdateUnlockedLevel("coming soon");
		}
		else
		{
			pLAYERINFO.ui.UpdateUnlockedLevel("UNLOCK AT TEAM LEVEL " + pLAYERINFO.data.UNLOCKLEVEL);
		}
		if (bAutoHide)
		{
			pLAYERINFO.ui.SetItemVisable(false);
		}
	}

	protected void CreatePlayersModel(int i, int heroIndex)
	{
		PLAYERINFO pLAYERINFO = dictPlayersInfo[heroIndex];
		bool flag = m_modelManagerScript.AddModelInfo_Force(i, pLAYERINFO.data.NAME, pLAYERINFO.data.MODELFILENAME, pLAYERINFO.data.CHARACTERTYPE, pLAYERINFO.data.WEAPONTYPE, Defined.RANK_TYPE.WHITE, true, 10f);
		GameObject model = m_modelManagerScript.GetModel(i);
		pLAYERINFO.data.MODELGO = model;
	}

	protected List<int> CheckIsVisablPlayersAppearance()
	{
		List<int> list = new List<int>();
		int num = 0;
		if (scrollView.transform.localPosition.x <= 0f)
		{
			num = (int)(Mathf.Abs(scrollView.transform.localPosition.x) / grid.cellWidth);
		}
		int num2 = (int)((Mathf.Abs(scrollView.transform.localPosition.x) + scrollView.panel.GetViewSize().x) / grid.cellWidth) + 1;
		UIUtil.PDebug("{" + num + "," + num2 + ")", "1-4");
		for (int i = num; i < num2; i++)
		{
			if (i < lsSortOrder.Count)
			{
				list.Add(lsSortOrder[i]);
			}
		}
		return list;
	}

	protected void ShowPlayersAppearanceItemUI()
	{
		for (int i = 0; i < lsVisablPlayersAppearance.Count; i++)
		{
			dictPlayersInfo[lsVisablPlayersAppearance[i]].ui.SetItemVisable(true);
			Rect texUV = m_modelManagerScript.GetTexUV(lsVisablPlayersAppearance[i]);
			PLAYERINFO pLAYERINFO = dictPlayersInfo[lsVisablPlayersAppearance[i]];
			pLAYERINFO.ui.UpdateIconUV(texUV);
			pLAYERINFO.ui.HideDefaultModelIcon();
		}
		UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 0);
		UIDialogManager.Instance.HideBlock(0);
		if (bInit)
		{
			UIEffectManager.Instance.HideEffect(UIEffectManager.EffectType.E_Loading, 41);
			UIDialogManager.Instance.HideBlock(41);
			bInit = false;
		}
	}

	protected void HandleScrollViewDragFinishEvent()
	{
		bool flag = false;
		if (Mathf.Abs(lastScrollViewPosition.x - scrollView.gameObject.transform.localPosition.x) <= 3f)
		{
			return;
		}
		lastScrollViewPosition = scrollView.gameObject.transform.localPosition;
		lsVisablPlayersAppearance = CheckIsVisablPlayersAppearance();
		for (int i = 0; i < lsVisablPlayersAppearance.Count; i++)
		{
			int num = lsVisablPlayersAppearance[i];
			if (dictPlayersInfo.ContainsKey(num))
			{
				if (dictPlayersInfo[num].data.MODELGO == null)
				{
					CreatePlayersModel(num, num);
					flag = true;
				}
				if (num == needSelectStoreItemIndex)
				{
					SelectPlayer(num);
				}
			}
		}
		if (flag)
		{
			UIEffectManager.Instance.ShowEffect(UIEffectManager.EffectType.E_Loading, 0);
			UIDialogManager.Instance.ShowBlock(0);
			Invoke("ShowPlayersAppearanceItemUI", 1f);
		}
		else
		{
			ShowPlayersAppearanceItemUI();
		}
	}

	protected void HandlePlayerAppearanceItemBtnClickedEvent(GameObject go)
	{
		if (dictMapPlayerInfo.ContainsKey(go))
		{
			int num = dictMapPlayerInfo[go];
			PLAYERINFO pi = dictPlayersInfo[num];
			SelectPlayer(num);
			if (ItemClickedEvent != null)
			{
				ItemClickedEvent(pi);
			}
			else
			{
				UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
			}
		}
	}

	protected void HandlePlayerAppearanceItemUnlockBtnClickedEvent(GameObject go)
	{
		if (dictMapPlayerInfo.ContainsKey(go))
		{
			int key = dictMapPlayerInfo[go];
			PLAYERINFO pi = dictPlayersInfo[key];
			if (ItemUnlockClickedEvent != null)
			{
				ItemUnlockClickedEvent(pi);
			}
			else
			{
				UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
			}
		}
	}

	protected void HandlePlayerFormationControlPressEvent(GameObject go, bool isPressed)
	{
		if (FormationControlDropStartTouchEvent != null)
		{
			if (dictMapPlayerInfo.ContainsKey(go))
			{
				int key = dictMapPlayerInfo[go];
				PLAYERINFO pi = dictPlayersInfo[key];
				FormationControlPressTouchEvent(pi, isPressed);
			}
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	protected void HandlePlayerFormationControlDragStartEvent(GameObject go)
	{
		HandlePlayerAppearanceItemBtnClickedEvent(go);
		if (FormationControlDropStartTouchEvent != null)
		{
			if (dictMapPlayerInfo.ContainsKey(go))
			{
				int key = dictMapPlayerInfo[go];
				PLAYERINFO pi = dictPlayersInfo[key];
				FormationControlDropStartTouchEvent(pi);
			}
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	protected void HandlePlayerFormationControlDragMovedEvent(GameObject go, Vector3 detal)
	{
		if (FormationControlDropMovedTouchEvent != null)
		{
			FormationControlDropMovedTouchEvent(go, detal);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	protected void HandlePlayerFormationControlDragReleasedEvent(GameObject go)
	{
		if (FormationControlDropReleasedTouchEvent != null)
		{
			FormationControlDropReleasedTouchEvent(go);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoCreatePage : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal = 0,
		Vertical = 1
	}

	[Serializable]
	public class PageInfo
	{
		public GameObject pageGo;

		public int maxCountConents;

		public List<GameObject> lsGOs = new List<GameObject>();
	}

	public Arrangement arrangement;

	public GameObject Prefab;

	public Vector3 sizePage = Vector3.zero;

	public int totalCount;

	public int eachLineCount = 1;

	public int maxLineCount = 1;

	public int cellWidth;

	public int cellHeight;

	public string GOEntryWord = string.Empty;

	public bool PlayOnAwake;

	public bool bNeedBoxCollider;

	public Vector3 boxColliderCenter = Vector3.zero;

	public bool bNeedUIDragPanelContents;

	public Dictionary<int, PageInfo> dictPages = new Dictionary<int, PageInfo>();

	private int totalPage;

	private int totalCountPerPage;

	public bool AddOne;

	public bool RemoveOne;

	private void Awake()
	{
		if (PlayOnAwake)
		{
			DoAutoCreate();
		}
	}

	private void Start()
	{
	}

	public void DoAutoCreate()
	{
		dictPages.Clear();
		totalCountPerPage = maxLineCount * eachLineCount;
		if (totalCount <= totalCountPerPage)
		{
			totalPage = 1;
		}
		else
		{
			totalPage = ((totalCount / totalCountPerPage != 0) ? (totalCount / totalCountPerPage + 1) : (totalCount / totalCountPerPage));
		}
		int num = totalCount;
		for (int i = 0; i < totalPage; i++)
		{
			if (num > 0)
			{
				PageInfo pageInfo = new PageInfo();
				if (num > totalCountPerPage)
				{
					pageInfo.pageGo = CreatePage(i, totalCountPerPage, ref pageInfo.lsGOs);
					pageInfo.maxCountConents = totalCountPerPage;
					num -= totalCountPerPage;
				}
				else
				{
					pageInfo.pageGo = CreatePage(i, num, ref pageInfo.lsGOs);
					pageInfo.maxCountConents = num;
					num -= num;
				}
				dictPages.Add(i, pageInfo);
			}
		}
		GetComponent<UIGrid>().repositionNow = true;
	}

	private GameObject CreatePage(int index, int maxConentsCount, ref List<GameObject> ls)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Page" + index.ToString("D0" + totalPage.ToString().Length);
		gameObject.layer = LayerMask.NameToLayer("UI");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UIGrid uIGrid = gameObject.AddComponent<UIGrid>();
		if (arrangement == Arrangement.Horizontal)
		{
			uIGrid.arrangement = UIGrid.Arrangement.Horizontal;
		}
		else if (arrangement == Arrangement.Vertical)
		{
			uIGrid.arrangement = UIGrid.Arrangement.Vertical;
		}
		uIGrid.maxPerLine = eachLineCount;
		uIGrid.sorting = UIGrid.Sorting.Horizontal;
		uIGrid.cellWidth = cellWidth;
		uIGrid.cellHeight = cellHeight;
		if (bNeedBoxCollider)
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			boxCollider.center = boxColliderCenter;
			boxCollider.size = sizePage;
		}
		if (bNeedUIDragPanelContents)
		{
			gameObject.AddComponent<UIScrollView>();
		}
		ls = CreateContent(gameObject, Prefab, maxConentsCount, GOEntryWord);
		uIGrid.repositionNow = true;
		return gameObject;
	}

	private List<GameObject> CreateContent(GameObject goTarget, GameObject prefab, int maxCount, string entryWord)
	{
		AutoCreatByPrefab autoCreatByPrefab = goTarget.AddComponent<AutoCreatByPrefab>();
		autoCreatByPrefab.Prefab = prefab;
		autoCreatByPrefab.MaxCount = maxCount;
		autoCreatByPrefab.GOEntryWord = entryWord;
		autoCreatByPrefab.PlayOnAwake = false;
		autoCreatByPrefab.ResetCopyGOTrasform = true;
		return autoCreatByPrefab.DoAutoCreate();
	}

	public void AddContent()
	{
		if (dictPages.Count > 0)
		{
			totalCountPerPage = maxLineCount * eachLineCount;
			PageInfo pageInfo = dictPages[dictPages.Count - 1];
			if (pageInfo.maxCountConents + 1 > totalCountPerPage)
			{
				totalPage++;
				pageInfo = new PageInfo();
				pageInfo.pageGo = CreatePage(dictPages.Count, 1, ref pageInfo.lsGOs);
				pageInfo.maxCountConents = 1;
				dictPages.Add(dictPages.Count, pageInfo);
				GetComponent<UIGrid>().repositionNow = true;
			}
			else
			{
				AutoCreatByPrefab component = pageInfo.pageGo.GetComponent<AutoCreatByPrefab>();
				GameObject item = component.CreatePefab(pageInfo.maxCountConents, Prefab, pageInfo.maxCountConents + 1, GOEntryWord, true);
				pageInfo.maxCountConents++;
				pageInfo.lsGOs.Add(item);
				pageInfo.pageGo.GetComponent<UIGrid>().repositionNow = true;
			}
			totalCount++;
		}
		else
		{
			totalPage++;
			PageInfo pageInfo2 = new PageInfo();
			pageInfo2.pageGo = CreatePage(dictPages.Count, 1, ref pageInfo2.lsGOs);
			pageInfo2.maxCountConents = 1;
			dictPages.Add(dictPages.Count, pageInfo2);
			GetComponent<UIGrid>().repositionNow = true;
		}
	}

	public void RemoveContent()
	{
		if (dictPages.Count > 0)
		{
			PageInfo pageInfo = dictPages[dictPages.Count - 1];
			if (pageInfo.maxCountConents - 1 <= 0)
			{
				totalPage--;
				GameObject pageGo = pageInfo.pageGo;
				dictPages.Remove(dictPages.Count - 1);
				UnityEngine.Object.Destroy(pageGo);
				GetComponent<UIGrid>().repositionNow = true;
			}
			else
			{
				GameObject gameObject = pageInfo.lsGOs[pageInfo.maxCountConents - 1];
				pageInfo.maxCountConents--;
				pageInfo.lsGOs.Remove(gameObject);
				UnityEngine.Object.Destroy(gameObject);
				pageInfo.pageGo.GetComponent<UIGrid>().repositionNow = true;
			}
			totalCount--;
		}
	}

	public KeyValuePair<int, PageInfo> GetDictInfoFromPageGO(GameObject go)
	{
		KeyValuePair<int, PageInfo> result = new KeyValuePair<int, PageInfo>(-999, null);
		foreach (KeyValuePair<int, PageInfo> dictPage in dictPages)
		{
			if (dictPage.Value.pageGo == go)
			{
				result = dictPage;
				return result;
			}
		}
		return result;
	}

	public void ResetPage()
	{
		base.transform.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void Update()
	{
		if (AddOne)
		{
			AddOne = false;
			AddContent();
			ResetPage();
		}
		if (RemoveOne)
		{
			RemoveOne = false;
			RemoveContent();
			ResetPage();
		}
	}
}

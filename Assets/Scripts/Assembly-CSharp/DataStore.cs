using System;
using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class DataStore
{
	public bool HeroRefresh(Defined.HERO_REFRESH_TYPE type, ref List<int> getHero)
	{
		DataConf.HeroRefreshData heroRefreshDataByType = DataCenter.Conf().GetHeroRefreshDataByType(type);
		switch (type)
		{
		case Defined.HERO_REFRESH_TYPE.Normal:
		{
			int num3 = -1;
			List<int> list2 = new List<int>();
			for (int j = 0; j < 2; j++)
			{
				int num4 = UnityEngine.Random.Range(0, 100);
				foreach (DataConf.HeroRefreshItemData value in heroRefreshDataByType.refreshItemData.Values)
				{
					if (num4 >= value.probability.left && num4 <= value.probability.right)
					{
						if (num3 == (int)value.rank)
						{
							int index6 = UnityEngine.Random.Range(0, list2.Count);
							int item6 = list2[index6];
							getHero.Add(item6);
							list2.RemoveAt(index6);
						}
						else
						{
							num3 = (int)value.rank;
							list2.Clear();
							list2.AddRange(value.index);
							int index7 = UnityEngine.Random.Range(0, list2.Count);
							int item7 = list2[index7];
							getHero.Add(item7);
							list2.RemoveAt(index7);
						}
					}
				}
			}
			break;
		}
		case Defined.HERO_REFRESH_TYPE.Premium:
		{
			int num5 = -1;
			List<int> list3 = new List<int>();
			for (int k = 0; k < 2; k++)
			{
				int num6 = UnityEngine.Random.Range(0, 100);
				foreach (DataConf.HeroRefreshItemData value2 in heroRefreshDataByType.refreshItemData.Values)
				{
					if (DataCenter.Save().PremiumRefreshMaker == 0)
					{
						DataCenter.Save().PremiumRefreshMaker++;
						list3.Clear();
						list3.AddRange(heroRefreshDataByType.refreshItemData[Defined.RANK_TYPE.BLUE].index);
						num5 = 2;
						int index8 = UnityEngine.Random.Range(0, list3.Count);
						int item8 = list3[index8];
						getHero.Add(item8);
						list3.RemoveAt(index8);
						break;
					}
					if (num6 < value2.probability.left || num6 > value2.probability.right)
					{
						continue;
					}
					if (value2.rank != Defined.RANK_TYPE.PURPLE)
					{
						if (DataCenter.Save().PremiumRefreshMaker == 30 || DataCenter.Save().PremiumRefreshMaker % 60 == 0)
						{
							list3.Clear();
							list3.AddRange(heroRefreshDataByType.refreshItemData[Defined.RANK_TYPE.PURPLE].index);
							num5 = 3;
							int index9 = UnityEngine.Random.Range(0, list3.Count);
							int item9 = list3[index9];
							getHero.Add(item9);
							list3.RemoveAt(index9);
						}
						else if (num5 == (int)value2.rank)
						{
							int index10 = UnityEngine.Random.Range(0, list3.Count);
							int item10 = list3[index10];
							getHero.Add(item10);
							list3.RemoveAt(index10);
						}
						else
						{
							num5 = (int)value2.rank;
							list3.Clear();
							list3.AddRange(value2.index);
							int index11 = UnityEngine.Random.Range(0, list3.Count);
							int item11 = list3[index11];
							getHero.Add(item11);
							list3.RemoveAt(index11);
						}
						DataCenter.Save().PremiumRefreshMaker++;
					}
					else
					{
						DataCenter.Save().PremiumRefreshMaker = 61;
						if (num5 == (int)value2.rank)
						{
							int index12 = UnityEngine.Random.Range(0, list3.Count);
							int item12 = list3[index12];
							getHero.Add(item12);
							list3.RemoveAt(index12);
						}
						else
						{
							num5 = (int)value2.rank;
							list3.Clear();
							list3.AddRange(value2.index);
							int index13 = UnityEngine.Random.Range(0, list3.Count);
							int item13 = list3[index13];
							getHero.Add(item13);
							list3.RemoveAt(index13);
						}
					}
					break;
				}
			}
			break;
		}
		case Defined.HERO_REFRESH_TYPE.Super:
		{
			int num = -1;
			List<int> list = new List<int>();
			for (int i = 0; i < 2; i++)
			{
				int num2 = UnityEngine.Random.Range(0, 100);
				foreach (DataConf.HeroRefreshItemData value3 in heroRefreshDataByType.refreshItemData.Values)
				{
					if (num2 < value3.probability.left || num2 > value3.probability.right)
					{
						continue;
					}
					if (value3.rank != Defined.RANK_TYPE.PURPLE)
					{
						if (DataCenter.Save().SuperRefreshMaker == 0 || DataCenter.Save().SuperRefreshMaker == 6 || DataCenter.Save().SuperRefreshMaker % 12 == 0)
						{
							list.Clear();
							list.AddRange(heroRefreshDataByType.refreshItemData[Defined.RANK_TYPE.PURPLE].index);
							num = 3;
							int index = UnityEngine.Random.Range(0, list.Count);
							int item = list[index];
							getHero.Add(item);
							list.RemoveAt(index);
						}
						else if (num == (int)value3.rank)
						{
							int index2 = UnityEngine.Random.Range(0, list.Count);
							int item2 = list[index2];
							getHero.Add(item2);
							list.RemoveAt(index2);
						}
						else
						{
							num = (int)value3.rank;
							list.Clear();
							list.AddRange(value3.index);
							int index3 = UnityEngine.Random.Range(0, list.Count);
							int item3 = list[index3];
							getHero.Add(item3);
							list.RemoveAt(index3);
						}
						DataCenter.Save().SuperRefreshMaker++;
					}
					else
					{
						DataCenter.Save().SuperRefreshMaker = 13;
						if (num == (int)value3.rank)
						{
							int index4 = UnityEngine.Random.Range(0, list.Count);
							int item4 = list[index4];
							getHero.Add(item4);
							list.RemoveAt(index4);
						}
						else
						{
							num = (int)value3.rank;
							list.Clear();
							list.AddRange(value3.index);
							int index5 = UnityEngine.Random.Range(0, list.Count);
							int item5 = list[index5];
							getHero.Add(item5);
							list.RemoveAt(index5);
						}
					}
					break;
				}
			}
			break;
		}
		}
		if (getHero.Count > 0 && DoCost(heroRefreshDataByType.costType, heroRefreshDataByType.cost))
		{
			if (DataCenter.Save().heroRefreshSaveData.heroRefreshCount.ContainsKey(type))
			{
				Dictionary<Defined.HERO_REFRESH_TYPE, int> heroRefreshCount;
				Dictionary<Defined.HERO_REFRESH_TYPE, int> dictionary = (heroRefreshCount = DataCenter.Save().heroRefreshSaveData.heroRefreshCount);
				Defined.HERO_REFRESH_TYPE key;
				Defined.HERO_REFRESH_TYPE key2 = (key = type);
				int num7 = heroRefreshCount[key];
				dictionary[key2] = num7 + 1;
			}
			else
			{
				DataCenter.Save().heroRefreshSaveData.heroRefreshCount.Add(type, 1);
			}
			return true;
		}
		return false;
	}

	public int GetHeroRefreshCount(Defined.HERO_REFRESH_TYPE type)
	{
		if (DataCenter.Save().heroRefreshSaveData.heroRefreshCount.ContainsKey(type))
		{
			return DataCenter.Save().heroRefreshSaveData.heroRefreshCount[type];
		}
		return 0;
	}

	public bool BuyItem(Defined.STORE_ITEM_TYPE type, int itemIndexAtStoreList)
	{
		DataConf.StoreItemData[] storeListByType = DataCenter.Conf().GetStoreListByType(type);
		if (itemIndexAtStoreList < 0 || itemIndexAtStoreList >= storeListByType.Length)
		{
			return false;
		}
		DataConf.StoreItemData storeItemData = storeListByType[itemIndexAtStoreList];
		int cost = storeItemData.cost;
		if (!DoCost(storeItemData.costType, cost))
		{
			return false;
		}
		Defined.ITEM_TYPE itemType = storeItemData.itemType;
		if (itemType == Defined.ITEM_TYPE.Stuff)
		{
			StuffData stuffData = new StuffData();
			stuffData.index = storeItemData.index;
			stuffData.count = 1;
			DataCenter.Save().AddStuffToBag(stuffData);
		}
		if (DataCenter.Save().storeSaveData.buyCount.ContainsKey(storeItemData.itemType))
		{
			if (DataCenter.Save().storeSaveData.buyCount[storeItemData.itemType].ContainsKey(storeItemData.index))
			{
				Dictionary<int, int> dictionary;
				Dictionary<int, int> dictionary2 = (dictionary = DataCenter.Save().storeSaveData.buyCount[storeItemData.itemType]);
				int index;
				int key = (index = storeItemData.index);
				index = dictionary[index];
				dictionary2[key] = index + 1;
			}
			else
			{
				DataCenter.Save().storeSaveData.buyCount[storeItemData.itemType] = new Dictionary<int, int>();
				DataCenter.Save().storeSaveData.buyCount[storeItemData.itemType].Add(storeItemData.index, 1);
			}
		}
		else
		{
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			dictionary3.Add(storeItemData.index, 1);
			DataCenter.Save().storeSaveData.buyCount.Add(storeItemData.itemType, dictionary3);
		}
		return true;
	}

	public bool ExchangeItemByElement(int itemIndexAtStoreExchangeList)
	{
		DataConf.StoreItemData[] storeExchangeList = DataCenter.Conf().GetStoreExchangeList();
		if (itemIndexAtStoreExchangeList < 0 || itemIndexAtStoreExchangeList >= storeExchangeList.Length)
		{
			return false;
		}
		DataConf.StoreItemData storeItemData = storeExchangeList[itemIndexAtStoreExchangeList];
		int cost = storeItemData.cost;
		if (DataCenter.Save().Element < cost)
		{
			return false;
		}
		DataCenter.Save().Money -= cost;
		return true;
	}

	public int GetItemBuyCount(Defined.ITEM_TYPE itemType, int itemIndex)
	{
		if (DataCenter.Save().storeSaveData.buyCount.ContainsKey(itemType))
		{
			Dictionary<int, int> dictionary = DataCenter.Save().storeSaveData.buyCount[itemType];
			if (dictionary.ContainsKey(itemIndex))
			{
				return dictionary[itemIndex];
			}
		}
		return 0;
	}

	public void CheckStoreListBuyCount()
	{
		DateTime dateTime = DateTime.Parse(DataCenter.Save().storeSaveData.lastOpenStoreDateTime);
		DateTime serverTime = Util.GetServerTime();
		if (serverTime.Year != dateTime.Year || serverTime.Month != dateTime.Month || serverTime.Day != dateTime.Day)
		{
			DataCenter.Save().storeSaveData.buyCount.Clear();
			DataCenter.Save().heroRefreshSaveData.heroRefreshCount.Clear();
		}
		DataCenter.Save().storeSaveData.lastOpenStoreDateTime = serverTime.ToString();
	}

	private bool DoCost(Defined.COST_TYPE costType, int cost)
	{
		bool paid = TryPay(costType, cost);
		if (paid)
		{
			Save.RequestWrite();
		}
		return paid;
	}

	private bool TryPay(Defined.COST_TYPE costType, int cost)
	{
		switch (costType)
		{
		case Defined.COST_TYPE.Money:
			if (DataCenter.Save().Money < cost)
			{
				return false;
			}
			DataCenter.Save().Money -= cost;
			return true;
		case Defined.COST_TYPE.Crystal:
			if (DataCenter.Save().Crystal < cost)
			{
				return false;
			}
			DataCenter.Save().Crystal -= cost;
			return true;
		case Defined.COST_TYPE.Element:
			if (DataCenter.Save().Element < cost)
			{
				return false;
			}
			DataCenter.Save().Element -= cost;
			return true;
		case Defined.COST_TYPE.Radar:
			if (DataCenter.Save().Radar < cost)
			{
				return false;
			}
			DataCenter.Save().Radar -= cost;
			return true;
		default:
			return false;
		}
	}
}

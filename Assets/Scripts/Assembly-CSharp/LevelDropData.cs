using System.Collections.Generic;

public class LevelDropData
{
	public int money;

	public int exp;

	public int crystal;

	public int honor;

	public int extraHonor;

	public int extraMoney;

	public int extraExp;

	public int extraCrystal;

	public int recommendCombat;

	public bool bDropGoldenMonster;

	public List<StuffData> stuffs;

	public LevelDropData()
	{
		money = 0;
		exp = 0;
		honor = 0;
		extraHonor = 0;
		extraMoney = 0;
		extraExp = 0;
		extraCrystal = 0;
		recommendCombat = 0;
		stuffs = new List<StuffData>();
	}
}

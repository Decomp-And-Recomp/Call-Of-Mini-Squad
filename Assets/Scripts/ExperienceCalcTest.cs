using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExperienceCalcTest
{
	public static int[] levelTest;

	public static int XPStarting
	{
		get { EconomyConfig.EnsureLoaded(); return EconomyConfig.TeamXPStarting; }
	}

	public static void Init()
	{
		EconomyConfig.EnsureLoaded();
		int rows = Mathf.Max(EconomyConfig.TeamXPRows, EconomyConfig.TeamMaxLevel + 1);
		int xpIncrease = EconomyConfig.TeamXPIncrease;
		int currentXPAdd = 0;
		levelTest = new int[rows];

		for (int i = 0; i < rows; i++)
		{
			if (i == 0)
			{
				levelTest[i] = EconomyConfig.TeamXPStarting;
				continue;
			}

			currentXPAdd += EconomyConfig.TeamXPStartIncreasing + (xpIncrease * (i - 1));
			xpIncrease += EconomyConfig.TeamXPIncreaseGrowth;

			levelTest[i] = EconomyConfig.TeamXPStarting + currentXPAdd;
		}
	}
}

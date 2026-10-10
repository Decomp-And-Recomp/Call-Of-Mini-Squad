using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgressController
{
	public static int GetRewardIndex()
	{
		int location = DataCenter.State().selectWorldNode;
		int area = DataCenter.State().selectAreaNode;
		switch (DataCenter.State().selectLevelMode)
		{
		case Defined.LevelMode.Hard:
			return 80 + location * 15 + area;
		case Defined.LevelMode.Hell:
			return 140 + location * 15 + area;
		default:
			return location * 20 + area;
		}
	}

	public static void Progress()
	{
		GameProgressData gameProgressData = DataCenter.Save().GetWorldProgressData(DataCenter.State().selectWorldNode);

		if (DataCenter.State().selectAreaNode + 1 < gameProgressData.levelProgress[(int)DataCenter.State().selectLevelMode])
		{
			return;
		}

		if (DataCenter.State().selectAreaNode == 19)
		{
			if (DataCenter.State().selectLevelMode != Defined.LevelMode.Hell)
			{
				gameProgressData.levelProgress[(int)DataCenter.State().selectLevelMode + 1] = 1;
				gameProgressData.modeProgress++;

				if (DataCenter.State().selectLevelMode == Defined.LevelMode.Normal)
				{
					DataCenter.Save().AddWorldNodeProgress(CreateProgress());
				}
			}
		}

		gameProgressData.levelProgress[(int)DataCenter.State().selectLevelMode]++;
	}

	public static GameProgressData CreateProgress()
	{
		return new GameProgressData
		{
			levelProgress = new ushort[3]
			{
				1,0,0
			},

			modeProgress = 0,

			levelStars = new Dictionary<Defined.LevelMode, ushort[]>
			{
				{Defined.LevelMode.Normal, new ushort[20]},
				{Defined.LevelMode.Hard, new ushort[20]},
				{Defined.LevelMode.Hell, new ushort[20]}
			}
		};
	}
}

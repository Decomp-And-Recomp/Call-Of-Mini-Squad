using UnityEngine;

public class UtilUITeamCursorInfo : MonoBehaviour
{
	public GameObject[] targets;

	private Bounds[] targetBounds;

	private Bounds mBounds;

	private void Awake()
	{
		mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(base.transform);
		targetBounds = new Bounds[targets.Length];
		for (int i = 0; i < targets.Length; i++)
		{
			targetBounds[i] = NGUIMath.CalculateAbsoluteWidgetBounds(targets[i].transform);
		}
	}

	public int CrashDetectionIndex()
	{
		int result = -1;
		mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(base.transform);
		for (int i = 0; i < targetBounds.Length; i++)
		{
			targetBounds[i] = NGUIMath.CalculateAbsoluteWidgetBounds(targets[i].transform);
			if (mBounds.Intersects(targetBounds[i]))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public GameObject CrashDetection()
	{
		int num = CrashDetectionIndex();
		return targets[num];
	}
}

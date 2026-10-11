using UnityEngine;

public class StarReasonLabel : MonoBehaviour
{
	private void OnEnable()
	{
		UILabel label = GetComponent<UILabel>();
		if (label != null)
		{
			label.text = BattleResultController.GetStarReason();
		}
	}
}

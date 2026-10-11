using UnityEngine;

public class TransMessage : MonoBehaviour
{
	public GameObject target;

	public void TranMessage(string functionName)
	{
		if (target != null)
		{
			target.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
		}
	}
}

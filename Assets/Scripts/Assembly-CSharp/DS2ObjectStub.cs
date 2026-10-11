using CoMDS2;
using UnityEngine;

public class DS2ObjectStub : MonoBehaviour
{
	private DS2Object m_object;

	public string currentState = string.Empty;

	public int buffCount;

	public string currentBuffName = string.Empty;

	public string currentHp = string.Empty;

	public bool isRage;

	public bool isGod;

	public bool isStuck;

	public static void BindObject(GameObject gameObject, DS2Object obj)
	{
		DS2ObjectStub dS2ObjectStub = gameObject.GetComponent<DS2ObjectStub>();
		if (dS2ObjectStub == null)
		{
			dS2ObjectStub = gameObject.AddComponent<DS2ObjectStub>();
		}
		dS2ObjectStub.m_object = obj;
	}

	// Editor only: fills the fields above so a character can be inspected while the game runs.
	public void RefreshDebug(string state, int hp, int hpMax, bool rage, bool god, bool stuck, int buffs, string buffNames)
	{
		currentState = state;
		currentHp = hp + "/" + hpMax;
		isRage = rage;
		isGod = god;
		isStuck = stuck;
		buffCount = buffs;
		currentBuffName = buffNames;
	}

	public static T GetObject<T>(GameObject gameObject) where T : DS2Object
	{
		DS2ObjectStub component = gameObject.GetComponent<DS2ObjectStub>();
		return (T)component.m_object;
	}
}

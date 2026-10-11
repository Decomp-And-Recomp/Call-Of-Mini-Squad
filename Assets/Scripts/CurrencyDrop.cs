using System;
using CoMDS2;
using UnityEngine;

public static class CurrencyDrop
{
	public static void Gold(Vector3 position, int pieces, int moneyPerPiece)
	{
		for (int i = 0; i < pieces; i++)
		{
			DS2StaticObject obj = BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.Gold) as DS2StaticObject;
			if (obj == null)
			{
				continue;
			}
			obj.GetTransform().position = position;
			obj.GetTransform().localScale = Vector3.one;
			obj.GetGameObject().SetActive(true);
			Gold gold = obj.GetGameObject().GetComponent<Gold>();
			gold.money = moneyPerPiece;
			float angle = 360f / (float)pieces * (float)i * ((float)Math.PI / 180f);
			Vector3 direction = new Vector3(Mathf.Cos(angle), 300f, Mathf.Sin(angle));
			int spread = UnityEngine.Random.Range(20, 50);
			int lift = UnityEngine.Random.Range(300, 500);
			gold.IdleTime = 5.3333f;
			gold.Emit(new Vector3(direction.x * (float)spread, lift, direction.z * (float)spread));
			GameBattle.m_instance.getMoneyInBattle += moneyPerPiece;
		}
	}

	public static void Crystals(Vector3 position, int count)
	{
		for (int i = 0; i < count; i++)
		{
			DS2StaticObject obj = BattleBufferManager.Instance.GetObjectFromBuffer(BattleBufferManager.PreLoadObjectBufferType.Crystal) as DS2StaticObject;
			if (obj == null)
			{
				continue;
			}
			obj.GetTransform().position = position;
			obj.GetTransform().localScale = Vector3.one;
			obj.GetGameObject().SetActive(true);
			Gold crystal = obj.GetGameObject().GetComponent<Gold>();
			crystal.money = 1;
			GameBattle.m_instance.getCrystalInBattle++;
			float angle = 360f / (float)count * (float)i * ((float)Math.PI / 180f);
			Vector3 direction = new Vector3(Mathf.Cos(angle), 300f, Mathf.Sin(angle));
			int spread = UnityEngine.Random.Range(20, 50);
			crystal.IdleTime = 5f * (1f + (float)i * 10f / 300f);
			crystal.Emit(new Vector3(direction.x * (float)spread, 300 + i * 10, direction.z * (float)spread));
		}
	}
}

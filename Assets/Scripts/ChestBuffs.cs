using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public static class ChestBuffs
{
	private class Timed
	{
		public RewardBoxConfig.RewardType type;

		public float remaining;

		public float percentage;

		public float duration;

		public float sinceTick;

		public List<KeyValuePair<Character, int>> maxHpBonuses = new List<KeyValuePair<Character, int>>();
	}

	public static float SkillDamageBonus;

	private static List<Timed> s_active = new List<Timed>();

	private static GameObject s_runner;

	public static void ResetForBattle()
	{
		SkillDamageBonus = 0f;
		s_active.Clear();
		if (s_runner != null)
		{
			Object.Destroy(s_runner);
			s_runner = null;
		}
	}

	public static bool Apply(RewardBoxConfig.Reward reward)
	{
		switch (reward.type)
		{
		case RewardBoxConfig.RewardType.MoveSpeed:
			ForEachMember(c => c.GetBuffManager().AddBuff(new Buff(Buff.AffectType.MoveSpeed, reward.percentage, 0f, reward.duration, Buff.CalcType.Percentage, 0f)));
			break;
		case RewardBoxConfig.RewardType.AddDamage:
			ForEachMember(c => c.GetBuffManager().AddBuff(new Buff(Buff.AffectType.AddATK, reward.percentage, 0f, reward.duration, Buff.CalcType.Percentage, 0f)));
			break;
		case RewardBoxConfig.RewardType.HpMaxPercent:
			StartTimed(reward);
			break;
		case RewardBoxConfig.RewardType.AddHP:
			StartTimed(reward);
			break;
		case RewardBoxConfig.RewardType.SkillDamagePercent:
			SkillDamageBonus = reward.percentage;
			StartTimed(reward);
			break;
		case RewardBoxConfig.RewardType.SkillCoolDownCancel:
		case RewardBoxConfig.RewardType.Invincible:
			StartTimed(reward);
			break;
		default:
			return false;
		}
		return true;
	}

	private static void ForEachMember(System.Action<Character> action)
	{
		DS2ActiveObject[] players = GameBattle.m_instance.GetPlayerList();
		foreach (DS2ActiveObject obj in players)
		{
			if (obj == null || !obj.Alive())
			{
				continue;
			}
			if (obj.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || obj.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
			{
				action((Character)obj);
			}
		}
	}

	private static void Heal(Character character, float percent)
	{
		int amount = (int)((float)character.hpMax * percent);
		int before = character.hp;
		character.hp = before + amount;
		if (character.hp <= before)
		{
			return;
		}
		if (EffectNumManager.instance != null)
		{
			EffectNumManager.instance.GenerageEffectNum(EffectNumber.EffectNumType.Heal, character.hp - before, character.m_effectPoint.position);
		}
		BattleBufferManager.Instance.GenerateEffectFromBuffer(Defined.EFFECT_TYPE.EFFECT_ADD_HP, character.GetTransform().position, 0f, character.GetTransform());
	}

	private static void StartTimed(RewardBoxConfig.Reward reward)
	{
		foreach (Timed existing in s_active)
		{
			if (existing.type == reward.type)
			{
				existing.remaining = reward.duration;
				existing.percentage = reward.percentage;
				existing.duration = reward.duration;
				return;
			}
		}
		Timed timed = new Timed();
		timed.type = reward.type;
		timed.remaining = reward.duration;
		timed.percentage = reward.percentage;
		timed.duration = reward.duration;
		s_active.Add(timed);
		if (reward.type == RewardBoxConfig.RewardType.HpMaxPercent)
		{
			ForEachMember(delegate(Character c)
			{
				int extra = (int)((float)c.hpMax * reward.percentage);
				c.hpMax += extra;
				c.hp += extra;
				timed.maxHpBonuses.Add(new KeyValuePair<Character, int>(c, extra));
			});
		}
		if (s_runner == null)
		{
			s_runner = new GameObject("ChestBuffs");
			s_runner.AddComponent<ChestBuffRunner>();
		}
	}

	private static bool Has(RewardBoxConfig.RewardType type)
	{
		foreach (Timed timed in s_active)
		{
			if (timed.type == type)
			{
				return true;
			}
		}
		return false;
	}

	private class ChestBuffRunner : MonoBehaviour
	{
		private void Update()
		{
			if (GameBattle.m_instance == null)
			{
				return;
			}
			float delta = Time.deltaTime;
			for (int i = s_active.Count - 1; i >= 0; i--)
			{
				s_active[i].remaining -= delta;
				if (s_active[i].type == RewardBoxConfig.RewardType.AddHP)
				{
					s_active[i].sinceTick += delta;
					if (s_active[i].sinceTick >= 1f || s_active[i].remaining <= 0f)
					{
						float share = s_active[i].duration > 0f ? s_active[i].percentage * s_active[i].sinceTick / s_active[i].duration : s_active[i].percentage;
						s_active[i].sinceTick = 0f;
						ForEachMember(c => Heal(c, share));
					}
				}
				if (s_active[i].remaining <= 0f)
				{
					if (s_active[i].type == RewardBoxConfig.RewardType.SkillDamagePercent)
					{
						SkillDamageBonus = 0f;
					}
					if (s_active[i].type == RewardBoxConfig.RewardType.HpMaxPercent)
					{
						foreach (KeyValuePair<Character, int> bonus in s_active[i].maxHpBonuses)
						{
							if (bonus.Key != null && bonus.Key.Alive())
							{
								float hpPercent = (float)bonus.Key.hp / (float)bonus.Key.hpMax;
								bonus.Key.hpMax -= bonus.Value;
								bonus.Key.hp = Mathf.Max(1, Mathf.RoundToInt(hpPercent * (float)bonus.Key.hpMax));
							}
						}
					}
					s_active.RemoveAt(i);
				}
			}
			bool cancelCooldown = Has(RewardBoxConfig.RewardType.SkillCoolDownCancel);
			bool invincible = Has(RewardBoxConfig.RewardType.Invincible);
			if (!cancelCooldown && !invincible)
			{
				return;
			}
			if (cancelCooldown)
			{
				GameBattle.m_instance.SkillCommonnalityCDTime = 0f;
			}
			ForEachMember(delegate(Character c)
			{
				if (cancelCooldown)
				{
					c.ClearSkillCDTime();
				}
				if (invincible && c.m_godTime < 0.3f)
				{
					c.SetGodTime(0.3f);
				}
			});
		}
	}
}

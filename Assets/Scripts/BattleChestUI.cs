using System.Collections.Generic;
using UnityEngine;

public class BattleChestUI : MonoBehaviour
{
	private class ActiveBuff
	{
		public GameObject item;

		public UITexture icon;

		public float remaining;
	}

	private const float SlideTime = 1.2f;

	private const float FlashTime = 5f;

	private static BattleChestUI s_instance;

	public GameObject tipsRoot;

	public UILabel tipsLabel;

	public UIGrid buffGrid;

	public GameObject buffTemplate;

	private float m_countdown;

	private List<ActiveBuff> m_buffs = new List<ActiveBuff>();

	private void Awake()
	{
		s_instance = this;
		if (tipsRoot != null)
		{
			tipsRoot.SetActive(false);
		}
		if (buffTemplate != null)
		{
			buffTemplate.SetActive(false);
		}
		if (buffGrid != null)
		{
			buffGrid.hideInactive = true;
		}
	}

	private void OnDestroy()
	{
		if (s_instance == this)
		{
			s_instance = null;
		}
	}

	public static void ResetForBattle()
	{
		if (s_instance != null)
		{
			s_instance.Clear();
		}
	}

	public static void ShowTips(float seconds)
	{
		if (s_instance != null)
		{
			s_instance.m_countdown = seconds;
			s_instance.Refresh();
		}
	}

	public static void HideTips()
	{
		if (s_instance != null)
		{
			s_instance.m_countdown = 0f;
			s_instance.Refresh();
		}
	}

	public static void AddBuff(RewardBoxConfig.Reward reward, Vector3 chestPosition)
	{
		if (s_instance == null || s_instance.buffTemplate == null || s_instance.buffGrid == null)
		{
			return;
		}
		s_instance.CreateBuffItem(reward, chestPosition);
	}

	private void CreateBuffItem(RewardBoxConfig.Reward reward, Vector3 chestPosition)
	{
		float duration = Mathf.Max(reward.duration, FlashTime);
		foreach (ActiveBuff existing in m_buffs)
		{
			if (existing.item.name == reward.name)
			{
				existing.remaining = duration;
				return;
			}
		}
		GameObject item = (GameObject)Object.Instantiate(buffTemplate);
		item.name = reward.name;
		item.transform.parent = buffGrid.transform;
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		item.layer = buffGrid.gameObject.layer;
		UITexture icon = item.GetComponentInChildren<UITexture>();
		if (icon != null && !string.IsNullOrEmpty(reward.icon))
		{
			icon.mainTexture = Resources.Load<Texture>("ui/battle/textures/buffs/" + reward.icon);
		}
		item.SetActive(true);
		ActiveBuff buff = new ActiveBuff();
		buff.item = item;
		buff.icon = icon;
		buff.remaining = duration;
		m_buffs.Add(buff);
		buffGrid.Reposition();
		SlideFromChest(item, chestPosition);
	}

	private void SlideFromChest(GameObject item, Vector3 chestPosition)
	{
		Camera worldCamera = GameBattle.m_instance != null ? GameBattle.m_instance.GetCamera() : null;
		Camera uiCamera = buffGrid.GetComponentInParent<Camera>();
		if (worldCamera == null || uiCamera == null)
		{
			return;
		}
		Vector3 screen = worldCamera.WorldToScreenPoint(chestPosition);
		if (screen.z < 0f)
		{
			return;
		}
		Vector3 end = item.transform.localPosition;
		Vector3 start = buffGrid.transform.InverseTransformPoint(uiCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, 0f)));
		start.z = end.z;
		item.transform.localPosition = start;
		TweenPosition.Begin(item, SlideTime, end);
	}

	private void Clear()
	{
		m_countdown = 0f;
		foreach (ActiveBuff buff in m_buffs)
		{
			Object.Destroy(buff.item);
		}
		m_buffs.Clear();
		Refresh();
	}

	private void Refresh()
	{
		if (tipsRoot != null)
		{
			tipsRoot.SetActive(m_countdown > 0f);
		}
	}

	private void Update()
	{
		float delta = Time.deltaTime;
		if (m_countdown > 0f)
		{
			m_countdown -= delta;
			if (tipsLabel != null)
			{
				int seconds = Mathf.CeilToInt(Mathf.Max(m_countdown, 0f));
				tipsLabel.text = (seconds / 60).ToString("00") + ":" + (seconds % 60).ToString("00");
			}
			if (m_countdown <= 0f)
			{
				Refresh();
			}
		}
		bool removed = false;
		for (int i = m_buffs.Count - 1; i >= 0; i--)
		{
			m_buffs[i].remaining -= delta;
			if (m_buffs[i].remaining <= 0f)
			{
				Object.Destroy(m_buffs[i].item);
				m_buffs.RemoveAt(i);
				removed = true;
			}
			else if (m_buffs[i].icon != null)
			{
				m_buffs[i].icon.alpha = m_buffs[i].remaining > FlashTime ? 1f : (Mathf.Sin(Time.time * 14f) > 0f ? 1f : 0.35f);
			}
		}
		if (removed && buffGrid != null)
		{
			buffGrid.repositionNow = true;
		}
	}
}

using CoMDS2;
using UnityEngine;

public class ChestInfo : MonoBehaviour
{
	private bool bOpened;

	private float lifeTime = 60f;

	public new Animation animation;

	public GameObject effect;

	public Transform[] currencyPos;

	private void Awake()
	{
		if (animation == null)
		{
			animation = GetComponentInChildren<Animation>();
		}
		if (effect == null)
		{
			Transform found = transform.Find("Model/Effect");
			if (found == null)
			{
				found = FindDeep(transform, "Effect");
			}
			effect = found != null ? found.gameObject : null;
		}
		if (currencyPos == null || currencyPos.Length == 0)
		{
			Transform pos = FindDeep(transform, "CurrencyPos");
			if (pos != null)
			{
				currencyPos = new Transform[pos.childCount];
				for (int i = 0; i < pos.childCount; i++)
				{
					currencyPos[i] = pos.GetChild(i);
				}
			}
		}
		Collider box = GetComponent<Collider>();
		if (box != null)
		{
			box.isTrigger = true;
		}
		if (GetComponent<Rigidbody>() == null)
		{
			Rigidbody body = gameObject.AddComponent<Rigidbody>();
			body.isKinematic = true;
			body.useGravity = false;
		}
		TransMessage message = GetComponentInChildren<TransMessage>();
		if (message != null && message.target == null)
		{
			message.target = gameObject;
		}
		if (effect != null)
		{
			effect.SetActive(false);
		}
	}

	private void Start()
	{
		lifeTime = RewardBoxConfig.ChestLifeTime;
		Invoke("Destroy", lifeTime);
	}

	private void Destroy()
	{
		Object.Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (bOpened || other == null)
		{
			return;
		}
		int layer = other.gameObject.layer;
		if (layer != 10 && layer != 9)
		{
			return;
		}
		Open();
	}

	public void Open()
	{
		if (bOpened)
		{
			return;
		}
		bOpened = true;
		CancelInvoke("Destroy");
		if (animation != null && animation["Open01"] != null)
		{
			animation["Open01"].wrapMode = WrapMode.Once;
			animation.Play("Open01");
		}
		else
		{
			ShowEffect();
		}
		Invoke("GetRewardRandom", 0.5f);
		Invoke("Destroy", 5f);
	}

	public void GetRewardRandom()
	{
		RewardBoxConfig.Reward reward = RewardBoxConfig.Roll();
		if (reward != null)
		{
			GetReward(reward);
		}
	}

	public void GetReward(RewardBoxConfig.Reward reward)
	{
		Vector3 origin = (currencyPos != null && currencyPos.Length > 0 && currencyPos[0] != null) ? currencyPos[0].position : transform.position + Vector3.up;
		switch (reward.type)
		{
		case RewardBoxConfig.RewardType.DropGold:
		{
			int rewardIndex = GameProgressController.GetRewardIndex();
			int stageGold = (rewardIndex >= 0 && rewardIndex < LevelCalcTest.LevelRewards.Count) ? LevelCalcTest.LevelRewards[rewardIndex][0] : 0;
			int pieces = Mathf.Max(1, reward.goldPieces);
			int money = Mathf.Max(1, (int)((float)stageGold * reward.goldStagePercent / 100f / (float)pieces));
			CurrencyDrop.Gold(origin, pieces, money);
			break;
		}
		case RewardBoxConfig.RewardType.DropCrystal:
		{
			int location = Mathf.Clamp(DataCenter.State().selectWorldNode, 0, reward.crystalPerLocation.Length - 1);
			int mode = Mathf.Clamp((int)DataCenter.State().selectLevelMode, 0, reward.crystalDifficultyMultiple.Length - 1);
			int count = reward.crystalPerLocation[location] * reward.crystalDifficultyMultiple[mode];
			CurrencyDrop.Crystals(origin, count);
			break;
		}
		default:
			if (ChestBuffs.Apply(reward))
			{
				BattleChestUI.AddBuff(reward, transform.position);
			}
			break;
		}
	}

	public void ShowEffect()
	{
		if (effect != null)
		{
			effect.SetActive(true);
		}
	}

	private static Transform FindDeep(Transform root, string name)
	{
		foreach (Transform child in root)
		{
			if (child.name == name)
			{
				return child;
			}
			Transform found = FindDeep(child, name);
			if (found != null)
			{
				return found;
			}
		}
		return null;
	}
}

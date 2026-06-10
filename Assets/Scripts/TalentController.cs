public static class TalentController
{
	public const int CODE_OK = 0;
	public const int CODE_NO_POINTS = 1005;
	public const int CODE_NO_CRYSTALS = 1006;
	public const int CODE_INVALID = -1;

	public static void SyncDictionaries()
	{
		TeamData td = DataCenter.Save().GetTeamData();
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		if (td == null || s == null) return;

		if (td.talents == null)
		{
			td.talents = new System.Collections.Generic.Dictionary<CoMDS2.TeamSpecialAttribute.TeamAttributeType, int>();
		}
		td.talents.Clear();
		if (s.teamAttributeTalent != null)
		{
			for (int i = 0; i < s.teamAttributeTalent.Length; i++)
			{
				TeamAttributeData node = s.teamAttributeTalent[i];
				if (node != null && node.level > 0)
				{
					td.talents[(CoMDS2.TeamSpecialAttribute.TeamAttributeType)node.index] = node.level;
				}
			}
		}

		if (td.evolves == null)
		{
			td.evolves = new System.Collections.Generic.Dictionary<CoMDS2.TeamSpecialAttribute.TeamAttributeEvolveType, int>();
		}
		td.evolves.Clear();
		if (s.teamAttributeEvolve != null)
		{
			for (int i = 0; i < s.teamAttributeEvolve.Length; i++)
			{
				TeamAttributeData node = s.teamAttributeEvolve[i];
				if (node != null && node.level > 0)
				{
					td.evolves[(CoMDS2.TeamSpecialAttribute.TeamAttributeEvolveType)node.index] = node.level;
				}
			}
		}
	}

	public static int TryUnlock(int index)
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		if (index < 0 || index >= s.teamAttributeTalent.Length)
		{
			return CODE_INVALID;
		}

		TeamAttributeData node = s.teamAttributeTalent[index];
		if (node.state != Defined.ItemState.Purchase)
		{
			return CODE_INVALID;
		}

		if (node.cost > 0)
		{
			if (!TrySpend(node.costType, node.cost))
			{
				return CodeForCostType(node.costType);
			}
		}

		node.state = Defined.ItemState.Available;
		SyncDictionaries();
		Save.Write();
		return CODE_OK;
	}

	public static int TryLevelUp(int index)
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		if (index < 0 || index >= s.teamAttributeTalent.Length)
		{
			return CODE_INVALID;
		}

		TeamAttributeData node = s.teamAttributeTalent[index];
		if (node.state != Defined.ItemState.Available)
		{
			return CODE_INVALID;
		}
		if (node.level >= node.maxLevel)
		{
			return CODE_INVALID;
		}
		if (s.teamAttributeRemainingPoints <= 0)
		{
			return CODE_NO_POINTS;
		}

		node.level++;
		s.teamAttributeAssignedPoint++;
		s.teamAttributeRemainingPoints--;

		RecomputeNodeStates();
		SyncDictionaries();
		Save.Write();
		return CODE_OK;
	}

	public static int TryBuyExtraPoint()
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		if (s.teamAttributeExtraPoint >= s.teamAttributeExtraPointMax)
		{
			return CODE_INVALID;
		}

		if (!TrySpend(Defined.COST_TYPE.Crystal, s.teamAttributeExtraPointCost))
		{
			return CODE_NO_CRYSTALS;
		}

		s.teamAttributeExtraPoint++;
		s.teamAttributeRemainingPoints++;
		Save.Write();
		return CODE_OK;
	}

	public static int TryReset()
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;

		if (s.teamGeniusfreeResetTimes > 0)
		{
			s.teamGeniusfreeResetTimes--;
		}
		else
		{
			if (!TrySpend(Defined.COST_TYPE.Crystal, s.teamGeniusResetCostCrystalPerTimes))
			{
				return CODE_NO_CRYSTALS;
			}
		}

		int refund = 0;
		for (int i = 0; i < s.teamAttributeTalent.Length; i++)
		{
			refund += s.teamAttributeTalent[i].level;
			s.teamAttributeTalent[i].level = 0;
		}
		s.teamAttributeRemainingPoints += refund;
		s.teamAttributeAssignedPoint = 0;

		for (int i = 0; i < s.teamAttributeTalent.Length; i++)
		{
			TeamAttributeData node = s.teamAttributeTalent[i];
			node.state = (node.unlockPoint == 0 || s.teamAttributeAssignedPoint >= node.unlockPoint)
				? Defined.ItemState.Purchase
				: Defined.ItemState.Locked;
		}

		SyncDictionaries();
		Save.Write();
		return CODE_OK;
	}

	private static void RecomputeNodeStates()
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		DataCenter.State().lsUpdatedTalentIndexs.Clear();

		for (int i = 0; i < s.teamAttributeTalent.Length; i++)
		{
			TeamAttributeData node = s.teamAttributeTalent[i];
			if (node.state == Defined.ItemState.Locked &&
				s.teamAttributeAssignedPoint >= node.unlockPoint)
			{
				node.state = Defined.ItemState.Purchase;
				DataCenter.State().lsUpdatedTalentIndexs.Add(i);
			}
		}
	}

	private static bool TrySpend(Defined.COST_TYPE type, int amount)
	{
		if (amount <= 0) return true;
		switch (type)
		{
			case Defined.COST_TYPE.Money:
				if (DataCenter.Save().Money < amount) return false;
				DataCenter.Save().Money -= amount;
				return true;
			case Defined.COST_TYPE.Crystal:
				if (DataCenter.Save().Crystal < amount) return false;
				DataCenter.Save().Crystal -= amount;
				return true;
			case Defined.COST_TYPE.Honor:
				if (DataCenter.Save().Honor < amount) return false;
				DataCenter.Save().Honor -= amount;
				return true;
			default:
				return false;
		}
	}

	private static int CodeForCostType(Defined.COST_TYPE type)
	{
		return (type == Defined.COST_TYPE.Crystal) ? CODE_NO_CRYSTALS : CODE_NO_POINTS;
	}
}

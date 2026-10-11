public static class EvolutionController
{
	public const int CODE_OK = 0;
	public const int CODE_NO_CRYSTALS = 1000;
	public const int CODE_NOT_ENOUGH = 1001;
	public const int CODE_MAX_LEVEL = 1007;
	public const int CODE_INVALID = -1;

	public static void RefreshStates()
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		if (s == null || s.teamAttributeEvolve == null)
		{
			return;
		}
		if (!s.teamEvolutionUnLocked && DataCenter.Save().GetTeamData().teamLevel >= EconomyConfig.EvolutionUnlockTeamLevel)
		{
			s.teamEvolutionUnLocked = true;
		}
		if (!s.teamEvolutionUnLocked)
		{
			return;
		}
		foreach (TeamAttributeData node in s.teamAttributeEvolve)
		{
			if (node != null && node.state == Defined.ItemState.Locked)
			{
				node.state = Defined.ItemState.Purchase;
			}
		}
	}

	public static int TryUnlock(int index)
	{
		RefreshStates();
		TeamAttributeData node = GetNode(index);
		if (node == null || node.state != Defined.ItemState.Purchase)
		{
			return CODE_INVALID;
		}
		int code = Pay(node);
		if (code != CODE_OK)
		{
			return code;
		}
		node.state = Defined.ItemState.Available;
		Finish();
		return CODE_OK;
	}

	public static int TryLevelUp(int index)
	{
		TeamAttributeData node = GetNode(index);
		if (node == null || node.state != Defined.ItemState.Available)
		{
			return CODE_INVALID;
		}
		if (node.level >= node.maxLevel)
		{
			return CODE_MAX_LEVEL;
		}
		int code = Pay(node);
		if (code != CODE_OK)
		{
			return code;
		}
		node.level++;
		Finish();
		return CODE_OK;
	}

	private static TeamAttributeData GetNode(int index)
	{
		DataSave.TeamAttributeSaveData s = DataCenter.Save().teamAttributeSaveData;
		if (s == null || s.teamAttributeEvolve == null || index < 0 || index >= s.teamAttributeEvolve.Length)
		{
			return null;
		}
		return s.teamAttributeEvolve[index];
	}

	private static int Pay(TeamAttributeData node)
	{
		int cost = node.cost;
		if (cost <= 0)
		{
			return CODE_OK;
		}
		if (!TalentController.TrySpend(node.costType, cost))
		{
			return node.costType == Defined.COST_TYPE.Crystal ? CODE_NO_CRYSTALS : CODE_NOT_ENOUGH;
		}
		return CODE_OK;
	}

	private static void Finish()
	{
		TalentController.SyncDictionaries();
		Save.Write();
	}
}

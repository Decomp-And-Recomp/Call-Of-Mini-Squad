using System.Collections.Generic;
using System.Diagnostics;

public class DataState
{
	public List<int> lsUpdatedEquipmentEquipIndexs = new List<int>();

	public List<int> lsUpdatedTalentIndexs = new List<int>();

	public string[] selectMailMessageIDS;

	public bool isPVPMode { get; set; }

	public int selectWorldNode { get; set; }

	public int selectAreaNode { get; set; }

	public Defined.LevelMode selectLevelMode { get; set; }

	public Defined.SceneType lastSceneType { get; set; }

	public int selectHeroIndex { get; set; }

	public Defined.TeamEquipmentBreakType selectEquipBreakType { get; set; }

	public Defined.EQUIP_TYPE selectEquipType { get; set; }

	public int selectTeamSiteIndex { get; set; }

	public int selectEquipIndex { get; set; }

	public int selectTalentIndex { get; set; }

	public int selectEvolveIndex { get; set; }

	public Defined.BattleResult battleResult { get; set; }

	public int battleStars { get; set; }

	public bool battleSquadMemberFell { get; set; }

	public float battleTime { get; set; }

	public string selectIAPID { get; set; }

	public string selectIAPReceipt { get; set; }

	public string selectIAPTransactionId { get; set; }

	public string selectIAPSignature { get; set; }

	public string selectArenaTargetUUId { get; set; }

	public int selectArenaTargetRank { get; set; }

	public bool isEncounterLevel { get; set; }

	public string selectArenaRankTypeByLanguage { get; set; }

	public string selectAchievementId { get; set; }

	public int protocoUseCrystal { get; set; }

	public string selectNeedFileName { get; set; }

	public string needSendMsgContent { get; set; }

	public float lastRightKeyDownTime { get; set; }

	public float lastLeftKeyDownTime { get; set; }

	public bool gameLoaded { get; set; }

	public DataState()
	{
		battleStars = 0;
		isPVPMode = false;
		selectWorldNode = 0;
		lastSceneType = Defined.SceneType.Menu;
	}

	public void ResetData(bool bCleanUUID)
	{
		UnityEngine.Debug.LogError("askmdskad");
		if (bCleanUUID)
		{
			UtilUIAccountManager.mInstance.accountData.uuid = string.Empty;
		}
		UtilUIAccountManager.mInstance.accountData.password = string.Empty;
		UIUtil.bStageMapChangedUI = true;
		DataCenter.State().lastSceneType = Defined.SceneType.Menu;
		DataCenter.State().selectLevelMode = Defined.LevelMode.Normal;
		DataCenter.Save().ResetTeamData();
		DataCenter.Save().InitGameProgress();
		DataCenter.Save().ResetTutorial();
		DataCenter.Save().Crystal = 0;
		DataCenter.Save().Money = 0;
		UIUtil.HideOpenClik();
	}
}

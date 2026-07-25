using System;
using CoMDS2;
using UnityEngine;

public class PhenixDebug : MonoBehaviour
{
	private bool bStartRemember;

	private string gInputString = string.Empty;

	private Rect windowRect = new Rect((float)Screen.width / 2f - (float)Screen.width / 8f, 20f, (float)Screen.width / 4f, (float)Screen.width / 4f);

	public UIScrollView scrollView;

	public bool bMoveRelative;

	public bool bMoveAbsolute;

	public bool bReset;

	public bool bRestrictWithinBounds;

	public Vector3 scrollViewVec3 = Vector3.zero;

	public Transform targetT;

	public GameObject targetUIGPrefab;

	public GameObject targetUIGO;

	public Transform targetUIParent;

	private void Update()
	{
		if (targetT != null)
		{
			Vector3 vector = SolidMapCameraControl.mInstance.WorldToScreenViewPort(targetT.position);
			Vector3 vector2 = new Vector3((float)Screen.width * vector.x, (float)Screen.height * vector.y, vector.z);
			if (targetUIGO == null)
			{
				targetUIGO = UnityEngine.Object.Instantiate(targetUIGPrefab) as GameObject;
				targetUIGO.name = "UI-" + targetT.name;
				targetUIGO.transform.parent = targetUIParent;
				targetUIGO.transform.localPosition = Vector3.zero;
				targetUIGO.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			Vector3 localPosition = new Vector3(vector2.x - (float)Screen.width / 2f, vector2.y - (float)Screen.height / 2f, 0f);
			targetUIGO.transform.localPosition = localPosition;
		}
		if (bMoveRelative)
		{
			scrollView.MoveRelative(scrollViewVec3);
			bMoveRelative = false;
		}
		if (bMoveAbsolute)
		{
			scrollView.MoveAbsolute(scrollViewVec3);
			bMoveAbsolute = false;
		}
		if (bReset)
		{
			scrollView.ResetPosition();
			bReset = false;
		}
		if (bRestrictWithinBounds)
		{
			scrollView.RestrictWithinBounds(true, true, true);
			bRestrictWithinBounds = false;
		}
	}

	private string ShowKeyCodePrefixMean()
	{
		return "  [1] PlayerCautionAreaRadius: when the target in this area, it ll auto attack it, and the team member ll attack to the same direction.\n  [2] PlayerCautionSectorRadius: The sector domain radius of the player.\n  [3] PlayerCautionSectorAngle: The sector domain angle of the player.\n  [4] LockRJoystickMaxTime: The timeout for the right joystick lock.\n  [5] SkillCommonnalityTotalCDTime: The time about the Total CD time.\n  [6] freshEnemyByTime_TimeRate: The time about how long ll recreate enemy by time.\n  [7] freshEnemyByTime_EnemyCount: The count of spawn enemy once.\n  [8] freshEnemyByTime_EnemyType: The enemy type.\n  [999] money crystal hornor exp added(Ex:[999]m999,c99,e999,h999)";
	}

	private void ProcessKeyString(string keyString)
	{
		if (keyString.StartsWith("[1]"))
		{
			keyString = keyString.Replace("[1]", string.Empty);
			DataCenter.Save().m_fPlayerCautionAreaRadius = float.Parse(keyString);
			UIUtil.PDebug("PlayerCautionAreaRadius is " + DataCenter.Save().m_fPlayerCautionAreaRadius, "1-4");
		}
		else if (keyString.StartsWith("[2]"))
		{
			keyString = keyString.Replace("[2]", string.Empty);
			DataCenter.Save().m_fPlayerCautionSectorRadius = float.Parse(keyString);
			UIUtil.PDebug("PlayerCautionSectorRadius is " + DataCenter.Save().m_fPlayerCautionSectorRadius, "1-4");
		}
		else if (keyString.StartsWith("[3]"))
		{
			keyString = keyString.Replace("[3]", string.Empty);
			DataCenter.Save().m_fPlayerCautionSectorAngle = float.Parse(keyString);
			UIUtil.PDebug("PlayerCautionSectorAngle is " + DataCenter.Save().m_fPlayerCautionSectorAngle, "1-4");
		}
		else if (keyString.StartsWith("[4]"))
		{
			keyString = keyString.Replace("[4]", string.Empty);
			DataCenter.Save().m_fLockRJoystickMaxTime = float.Parse(keyString);
			UIUtil.PDebug("LockRJoystickMaxTime is " + DataCenter.Save().m_fLockRJoystickMaxTime, "1-4");
		}
		else if (keyString.StartsWith("[5]"))
		{
			keyString = keyString.Replace("[5]", string.Empty);
			GameBattle.m_instance.SkillCommonnalityTotalCDTime = float.Parse(keyString);
			UIUtil.PDebug("SkillCommonnalityTotalCDTime is " + GameBattle.m_instance.SkillCommonnalityTotalCDTime, "1-4");
		}
		else if (keyString.StartsWith("[6]"))
		{
			keyString = keyString.Replace("[6]", string.Empty);
			DataCenter.Save().m_ffreshEnemyByTime_TimeRate = float.Parse(keyString);
			UIUtil.PDebug("freshEnemyByTime_TimeRate is " + DataCenter.Save().m_ffreshEnemyByTime_TimeRate, "1-4");
		}
		else if (keyString.StartsWith("[7]"))
		{
			keyString = keyString.Replace("[7]", string.Empty);
			DataCenter.Save().m_ffreshEnemyByTime_EnemyCount = float.Parse(keyString);
			UIUtil.PDebug("freshEnemyByTime_EnemyCount is " + DataCenter.Save().m_ffreshEnemyByTime_EnemyCount, "1-4");
		}
		else if (keyString.StartsWith("[8]"))
		{
			keyString = keyString.Replace("[8]", string.Empty);
			DataCenter.Save().m_ifreshEnemyByTime_EnemyType = (Enemy.EnemyType)int.Parse(keyString);
			UIUtil.PDebug("freshEnemyByTime_EnemyType is " + DataCenter.Save().m_ifreshEnemyByTime_EnemyType, "1-4");
		}
		else
		{
			if (!keyString.StartsWith("[999]"))
			{
				return;
			}
			keyString = keyString.Replace("[999]", string.Empty);
			string[] array = keyString.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].StartsWith("m"))
				{
					string s = array[i].Replace("m", string.Empty);
					UIConstant.MONEY = int.Parse(s);
				}
				else if (array[i].StartsWith("c"))
				{
					string s2 = array[i].Replace("c", string.Empty);
					UIConstant.CRYSTAL = int.Parse(s2);
				}
				else if (array[i].StartsWith("h"))
				{
					string s3 = array[i].Replace("h", string.Empty);
					UIConstant.HORNOR = int.Parse(s3);
				}
				else if (array[i].StartsWith("e"))
				{
					string s4 = array[i].Replace("e", string.Empty);
					UIConstant.EXP = int.Parse(s4);
				}
			}
			HttpRequestHandle.instance.SendRequest(HttpRequestHandle.RequestType.Phenix_Test, null);
		}
	}

	private void OnGUI()
	{
	}

	private void DebugWindow(int windowID)
	{
		GUI.color = Color.red;
		GUILayout.Label(Screen.width + " x " + Screen.height);
		GUI.color = Color.white;
		GUILayout.Space(30f);
		if (GUILayout.Button("Save GameData"))
		{
			Save.Write();
		}
		if (GUILayout.Button("BaseMap Point LayOut") && UINewBaseManager.Instance.gameObject.GetComponent<BaseMapPositionEdior>() == null)
		{
			UINewBaseManager.Instance.gameObject.AddComponent<BaseMapPositionEdior>();
		}
		if (DataCenter.State().lastSceneType != Defined.SceneType.Battle && GUILayout.Button("AutoControl"))
		{
			Util.s_autoControl = !Util.s_autoControl;
			UIUtil.PDebug("AutoControl is " + Util.s_autoControl, "1-4");
		}
		if (GUILayout.Button("ManualUseSkill"))
		{
			DataCenter.Save().m_bManualUseSkill = !DataCenter.Save().m_bManualUseSkill;
			GameObject gameObject = GameObject.Find("SkillEx");
			if (gameObject != null)
			{
				gameObject.SetActiveRecursively(true);
			}
			UIUtil.PDebug("ManualUseSkill is " + DataCenter.Save().m_bManualUseSkill, "1-4");
		}
		if (DataCenter.State().lastSceneType != Defined.SceneType.Battle)
		{
			if (GUILayout.Button("TeamMemberAutoAttack"))
			{
				DataCenter.Save().m_bTeamMemberAutoAttack = !DataCenter.Save().m_bTeamMemberAutoAttack;
				UIUtil.PDebug("TeamMemberAutoAttack is " + DataCenter.Save().m_bTeamMemberAutoAttack, "1-4");
			}
			if (GUILayout.Button("RefreshEnemyByTime"))
			{
				DataCenter.Save().m_bRefreshEnemyByTime = !DataCenter.Save().m_bRefreshEnemyByTime;
				UIUtil.PDebug("RefreshEnemyByTime is " + DataCenter.Save().m_bRefreshEnemyByTime, "1-4");
			}
		}
		if (!bStartRemember)
		{
			if (GUILayout.Button("Begin Dynamic Input"))
			{
				bStartRemember = true;
				gInputString = string.Empty;
				UIUtil.PDebug("KeyCodePrefixMean:\n" + ShowKeyCodePrefixMean(), "1-4");
			}
		}
		else
		{
			gInputString = GUILayout.TextField(gInputString);
			if (GUILayout.Button("Finish Dynamic Input"))
			{
				bStartRemember = false;
				UIUtil.PDebug("Input String:" + gInputString, "1-4");
				ProcessKeyString(gInputString);
				gInputString = string.Empty;
			}
		}
		GUI.DragWindow();
	}

	public static void ShowDebugNotifaction()
	{
		UIUtil.PDebug("Push (Save GameData) to Save GameData", "4");
		UIUtil.PDebug("Push (AutoControl(NotInGame)) to change the Control Player can auto attack the enemy or not. Remember if change it in the battle, it ll be effective next time. Now it is " + Util.s_autoControl, "4");
		UIUtil.PDebug("Push (ManualUseSkill) to change the player use the skill auto/manual. Now it is " + DataCenter.Save().m_bManualUseSkill, "4");
		UIUtil.PDebug("Push (CanChangeTeamMember) to control the team member when u in game can change it to leader. Now it is " + DataCenter.Save().m_bCanChangeTeamMember, "4");
		UIUtil.PDebug("Push (TeamMemberAutoAttack(NotInGame)) to control the team member auto attack the enemy. Now it is " + DataCenter.Save().m_bTeamMemberAutoAttack, "4");
		UIUtil.PDebug("Push (RefreshEnemyByTime(NotInGame)) to control the enemy is refresh with the time. Now it is " + DataCenter.Save().m_bRefreshEnemyByTime, "4");
		UIUtil.PDebug("Push (Begin Dynamic Input) to Start the Remember KeyString. Push (Finish Dynamic Input) to end of it", "4");
	}
}

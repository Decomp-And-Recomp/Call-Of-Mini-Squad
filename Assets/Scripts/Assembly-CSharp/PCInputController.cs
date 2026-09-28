using CoMDS2;
using UnityEngine;

public class PCInputController : MonoBehaviour
{
	private float m_lastRightLparam;
	private float m_lastRightWparam;
	private Camera m_mainCamera;
	private int m_uiLayerGui;
	private int m_uiLayerUi;

	private void Start()
	{
		if (Application.isMobilePlatform)
		{
			enabled = false;
			return;
		}
		m_mainCamera = Camera.main;
		m_uiLayerGui = LayerMask.NameToLayer("GUI");
		m_uiLayerUi = LayerMask.NameToLayer("UI");
	}

	private void ComputeLeftStick(out float wparam, out float lparam)
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		Vector2 v = new Vector2(x, z);
		if (v.sqrMagnitude < 0.0001f)
		{
			wparam = 0f;
			lparam = 0f;
			return;
		}
		if (v.sqrMagnitude > 1f)
		{
			v = v.normalized;
		}
		wparam = v.magnitude;
		lparam = Mathf.Atan2(v.y, v.x);
	}

	private bool IsCursorOverUI()
	{
		GameObject hovered = UICamera.hoveredObject;
		if (hovered == null)
		{
			return false;
		}
		int layer = hovered.layer;
		return layer == m_uiLayerGui || layer == m_uiLayerUi;
	}

	private void ComputeRightStick(Player owner, out float wparam, out float lparam)
	{
		if (!Input.GetMouseButton(0))
		{
			wparam = 0f;
			lparam = 0f;
			m_lastRightWparam = 0f;
			m_lastRightLparam = 0f;
			return;
		}
		if (IsCursorOverUI())
		{
			wparam = 0f;
			lparam = 0f;
			return;
		}
		Camera cam = m_mainCamera;
		if (cam == null || owner == null)
		{
			wparam = m_lastRightWparam;
			lparam = m_lastRightLparam;
			return;
		}
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 ownerPos = owner.GetTransform().position;
		Plane plane = new Plane(Vector3.up, ownerPos);
		float enter;
		if (!plane.Raycast(ray, out enter))
		{
			wparam = m_lastRightWparam;
			lparam = m_lastRightLparam;
			return;
		}
		Vector3 hit = ray.GetPoint(enter);
		Vector3 dir = hit - ownerPos;
		dir.y = 0f;
		if (dir.sqrMagnitude < 0.0001f)
		{
			wparam = m_lastRightWparam;
			lparam = m_lastRightLparam;
			return;
		}
		wparam = 1f;
		lparam = Mathf.Atan2(dir.z, dir.x);
		m_lastRightWparam = wparam;
		m_lastRightLparam = lparam;
	}

	private void HandleDiscreteKeys(Player player)
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameBattle.IsPauseBlockedByTutorial())
			{
				return;
			}
			GameBattle.State state = GameBattle.m_instance.GameState;
			if (state == GameBattle.State.Game)
			{
				GameBattle.m_instance.GameState = GameBattle.State.Pause;
				UIUtil.ShowOpenClik(false);
			}
			else if (state == GameBattle.State.Pause)
			{
				GameObject pausePanel = UIControlManager.Instance.GetControl((int)BattleUIEvent.UIControlID.Panel_GamePause);
				if (pausePanel != null)
				{
					pausePanel.SetActive(false);
				}
				UIUtil.HideOpenClik();
				GameBattle.m_instance.GameState = GameBattle.State.Game;
			}
			return;
		}

		if (GameBattle.m_instance.GameState != GameBattle.State.Game)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Space))
		{
			TryUseSkill(-1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) 
		{ 
			TryUseSkill(0); 
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3)) 
		{ 
			TryUseSkill(1); 
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4)) 
		{ 
			TryUseSkill(2); 
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5)) 
		{ 
			TryUseSkill(3); 
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			ToggleSquadMode();
		}
		if (!DataCenter.Save().m_bCanChangeTeamMember)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			CycleSquad(-1);
			return;
		}
		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))
		{
			CycleSquad(1);
			return;
		}
	}

	private void TryUseSkill(int memberIndex)
	{
		Player target = GameBattle.m_instance.GetPlayer(memberIndex);
		if (target == null) return;
		if (target.clique != DS2ActiveObject.Clique.Player) return;
		if (!target.Alive()) return;
		if (target.SkillInCDTime()) return;
		if (GameBattle.m_instance.SkillCommonnalityCDTime > 0f) return;
		target.UseSkill(0);
	}

	private void ToggleSquadMode()
	{
		if (GameBattle.m_instance == null) return;
		if (Time.realtimeSinceStartup - GameBattle.m_instance.changeModeLimitTime <= 0.3f) return;

		bool currentlySquad = DataCenter.Save().squadMode;
		bool newSquad = !currentlySquad;
		if (newSquad && GameBattle.m_instance.GetPlayerAliveList().Length <= 1) return;

		GameBattle.m_instance.changeModeLimitTime = Time.realtimeSinceStartup;
		DataCenter.Save().squadMode = newSquad;
		GameBattle.m_instance.SetSquadMode(newSquad);
		GameObject rpgBtn = UIControlManager.Instance.GetControl((int)BattleUIEvent.UIControlID.BT_SwitchToRPG);
		GameObject squadBtn = UIControlManager.Instance.GetControl((int)BattleUIEvent.UIControlID.BT_SwitchToSquad);
		if (rpgBtn != null) rpgBtn.SetActive(newSquad);
		if (squadBtn != null) squadBtn.SetActive(!newSquad);

		if (Tutorial.Instance != null && Tutorial.Instance.TutorialPahseChangeMode == Tutorial.TutorialPhaseState.InProgress)
		{
			Tutorial.Instance.TutorialPahseChangeMode = Tutorial.TutorialPhaseState.Done;
		}
	}

	private void CycleSquad(int direction)
	{
		Player[] team = GameBattle.m_instance.GetTeammateList();
		if (team == null || team.Length == 0) return;

		int currentIdx = -1;
		for (int i = 0; i < team.Length; i++)
		{
			if (team[i] != null && team[i].CurrentController)
			{
				currentIdx = i;
				break;
			}
		}
		if (currentIdx == -1)
		{
			currentIdx = (direction > 0) ? -1 : 0;
		}

		for (int offset = 1; offset <= team.Length; offset++)
		{
			int candidate = ((currentIdx + direction * offset) % team.Length + team.Length) % team.Length;
			if (team[candidate] != null && team[candidate].Alive() && !team[candidate].CurrentController)
			{
				TrySwapToIndex(team, candidate);
				return;
			}
		}
	}

	private void TrySwapToIndex(Player[] team, int index)
	{
		if (team == null || index < 0 || index >= team.Length) return;
		if (team[index] == null || !team[index].Alive()) return;
		if (team[index].CurrentController) return;
		GameBattle.m_instance.SetTeammateToCurrentControlPlayer(index);
	}

	private void UpdateCursorForGameState()
	{
		bool inGame = GameBattle.m_instance.GameState == GameBattle.State.Game;
		CursorLockMode wantedLock = inGame ? CursorLockMode.Confined : CursorLockMode.None;
		bool wantedVisible = !inGame;
		if (Cursor.lockState != wantedLock)
		{
			Cursor.lockState = wantedLock;
		}
	}

	private void Update()
	{
		if (GameBattle.m_instance == null)
		{
			return;
		}

		UpdateCursorForGameState();

		if (GameBattle.s_bInputLocked || BattleUIEvent.s_anyButtonDown || TUIHandleManager.s_anyUIButtonDown)
		{
			return;
		}
		Player player = GameBattle.m_instance.GetPlayer();
		if (player == null)
		{
			return;
		}

		bool tutorialActive = Tutorial.Instance != null && Tutorial.Instance.TutorialInProgress;

		float leftW = 0f, leftL = 0f, rightW = 0f, rightL = 0f;

		if (tutorialActive)
		{
			HandleTutorialPhaseInput(player, out leftW, out leftL, out rightW, out rightL);
		}
		else
		{
			HandleDiscreteKeys(player);
			ComputeLeftStick(out leftW, out leftL);
			ComputeRightStick(player, out rightW, out rightL);
		}

		if (DataCenter.Save().squadMode)
		{
			SquadController squad = GameBattle.m_instance.GetSquadController();
			if (squad != null)
			{
				squad.UpdateInput(leftW, leftL, rightW, rightL);
			}
		}
		else
		{
			player.UpdateInput(leftW, leftL, rightW, rightL);
		}
	}

	private void HandleTutorialPhaseInput(Player player, out float leftW, out float leftL, out float rightW, out float rightL)
	{
		leftW = 0f;
		leftL = 0f;
		rightW = 0f;
		rightL = 0f;

		if (Tutorial.Instance.TutorialPahseMove == Tutorial.TutorialPhaseState.InProgress)
		{
			ComputeLeftStick(out leftW, out leftL);
		}
		else if (Tutorial.Instance.TutorialPahseFire == Tutorial.TutorialPhaseState.InProgress)
		{
			ComputeRightStick(player, out rightW, out rightL);
		}
		else if (Tutorial.Instance.TutorialPahseSkill == Tutorial.TutorialPhaseState.InProgress)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Space))
			{
				TryUseSkill(-1);
			}
		}
		else if (Tutorial.Instance.TutorialPahseChangeMode == Tutorial.TutorialPhaseState.InProgress)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				ToggleSquadMode();
			}
		}
		else if (Tutorial.Instance.TutorialPahseChangePlayer == Tutorial.TutorialPhaseState.InProgress)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				CycleSquad(-1);
			}
			else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))
			{
				CycleSquad(1);
			}
		}
	}
}

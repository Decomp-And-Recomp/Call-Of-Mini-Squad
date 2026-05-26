using CoMDS2;
using UnityEngine;

public class DS2UIBattle : MonoBehaviour
{
	private float left_wparam;

	private float left_lparam;

	private float right_wparam;

	private float right_lparam;

	public void HandleEventJoystickLeft(int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			left_wparam = 0f;
			left_lparam = 0f;
		}
		else
		{
			left_wparam = wparam;
			left_lparam = lparam;
		}
	}

	public void HandleEventJoystickRight(int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			right_wparam = 0f;
			right_lparam = 0f;
		}
		else
		{
			right_wparam = wparam;
			right_lparam = lparam;
		}
	}

	public void HandleEventMove(int eventType, float wparam, float lparam, object data)
	{
	}

	public void Start()
	{
		if (!Application.isMobilePlatform)
		{
			TUIButtonJoystick[] leftSticks = UnityEngine.Object.FindObjectsOfType<TUIButtonJoystick>();
			for (int i = 0; i < leftSticks.Length; i++)
			{
				if (leftSticks[i] != null)
				{
					leftSticks[i].gameObject.SetActive(false);
				}
			}
			TUIButtonJoystickEx[] rightSticks = UnityEngine.Object.FindObjectsOfType<TUIButtonJoystickEx>();
			for (int i = 0; i < rightSticks.Length; i++)
			{
				if (rightSticks[i] != null)
				{
					rightSticks[i].gameObject.SetActive(false);
				}
			}
			if (gameObject.GetComponent<PCInputController>() == null)
			{
				gameObject.AddComponent<PCInputController>();
			}
			UICamera[] uiCameras = UnityEngine.Object.FindObjectsOfType<UICamera>();
			for (int i = 0; i < uiCameras.Length; i++)
			{
				if (uiCameras[i] != null)
				{
					uiCameras[i].autoHideCursor = false;
				}
			}
		}
	}

	public void Update()
	{
		if (!Application.isMobilePlatform)
		{
			return;
		}
		if (GameBattle.s_bInputLocked || BattleUIEvent.s_anyButtonDown)
		{
			return;
		}
		if (DataCenter.Save().squadMode)
		{
			SquadController squadController = GameBattle.m_instance.GetSquadController();
			if (squadController != null)
			{
				squadController.UpdateInput(left_wparam, left_lparam, right_wparam, right_lparam);
			}
		}
		else
		{
			Player player2 = GameBattle.m_instance.GetPlayer();
			if (player2 != null)
			{
				player2.UpdateInput(left_wparam, left_lparam, right_wparam, right_lparam);
			}
		}
	}
}

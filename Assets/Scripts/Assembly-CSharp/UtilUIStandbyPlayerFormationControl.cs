using UnityEngine;

public class UtilUIStandbyPlayerFormationControl : UIDragDropItem
{
	public UISprite icon;

	public Transform formationTrans;

	public Vector3 orignalPos = Vector3.zero;

	public Vector3 nowMousePos = Vector3.zero;

	private UtilUIStandbyPlayerFormationControl_OnPressTouchGO _pressEvent;

	private UtilUIStandbyPlayerFormationControl_OnDragDropStartTouchGO _startEvent;

	private UtilUIStandbyPlayerFormationControl_OnDragDropMovedTouchGO _movedEvent;

	private UtilUIStandbyPlayerFormationControl_OnDragDropReleasedTouchGO _releasedEvent;

	public void BlindFuntion(UtilUIStandbyPlayerFormationControl_OnPressTouchGO _press, UtilUIStandbyPlayerFormationControl_OnDragDropStartTouchGO _start, UtilUIStandbyPlayerFormationControl_OnDragDropMovedTouchGO _move, UtilUIStandbyPlayerFormationControl_OnDragDropReleasedTouchGO _release)
	{
		_pressEvent = _press;
		_startEvent = _start;
		_movedEvent = _move;
		_releasedEvent = _release;
	}

	protected override void Start()
	{
		base.Start();
		orignalPos = base.transform.localPosition;
		nowMousePos = orignalPos;
	}

	protected override void OnDragDropStart()
	{
		base.OnDragDropStart();
		if (_startEvent != null)
		{
			_startEvent(base.transform.parent.gameObject);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}

	protected override void OnDragDropMove(Vector2 delta)
	{
		GameObject hoveredObject = UICamera.hoveredObject;
		if (hoveredObject != null)
		{
			if (_movedEvent != null)
			{
				_movedEvent(hoveredObject, delta);
			}
			else
			{
				UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
			}
		}
	}

	protected override void OnDragDropRelease(GameObject surface)
	{
		if (_releasedEvent != null)
		{
			_releasedEvent(surface);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
		base.OnDragDropRelease(surface);
	}

	protected new void OnPress(bool isPressed)
	{
		base.OnPress(isPressed);
		if (_startEvent != null)
		{
			_pressEvent(base.transform.parent.gameObject, isPressed);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
	}
}

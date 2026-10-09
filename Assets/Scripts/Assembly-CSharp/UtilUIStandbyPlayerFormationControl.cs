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

	private bool mScrollGesture;

	protected override void Start()
	{
		base.Start();
		orignalPos = base.transform.localPosition;
		nowMousePos = orignalPos;
		pressAndHoldDelay = 0.25f;
	}

	protected override void Update()
	{
		if (!mScrollGesture)
		{
			base.Update();
		}
	}

	protected override void OnDragStart()
	{
		if (!interactable || !canBeDragged || !mPressed || mDragging || !enabled || mTouch != UICamera.currentTouch)
		{
			return;
		}
		Vector2 delta = mTouch.totalDelta;
		if (Mathf.Abs(delta.y) >= Mathf.Abs(delta.x))
		{
			StartDragging();
		}
		else
		{
			mScrollGesture = true;
		}
	}

	protected override void OnDragDropStart()
	{
		if (mDragScrollView != null && mDragScrollView.scrollView != null)
		{
			mDragScrollView.scrollView.Press(false);
		}
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
		if (_movedEvent != null)
		{
			_movedEvent(UICamera.hoveredObject, delta);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
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

	protected override void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			mScrollGesture = false;
		}
		base.OnPress(isPressed);
		if (_startEvent != null)
		{
			_pressEvent(base.transform.parent.gameObject, isPressed);
		}
		else
		{
			UIUtil.PDebug("Delegate Event Is NULL!!!", "1-4");
		}
		if (!isPressed && mDragging)
		{
			StopDragging(UICamera.hoveredObject);
		}
	}
}

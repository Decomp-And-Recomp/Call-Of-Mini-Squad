using System.Collections.Generic;
using UnityEngine;

public class UtilUISoliderMapDrag : UIDragDropItem
{
	private List<Vector3> lsMoveDelta = new List<Vector3>();

	protected override void OnDragDropStart()
	{
		base.OnDragDropStart();
		lsMoveDelta.Clear();
	}

	protected override void OnDragDropMove(Vector2 delta)
	{
		SolidMapCameraControl.mInstance.MoveCamera(delta);
	}

	protected override void OnDragDropRelease(GameObject surface)
	{
		base.OnDragDropRelease(surface);
		if (!SolidMapCameraControl.mInstance.ExploreCamera(base.gameObject, "OnExploreFinished"))
		{
			OnExploreFinished();
		}
	}

	public void OnExploreFinished()
	{
	}

	private void OnDoubleClick()
	{
		Vector2 pos = UICamera.currentTouch.pos;
		Vector3 vector = SolidMapCameraControl.mInstance.ScreenToWorldPosition(new Vector3(pos.x, pos.y, 0f));
	}
}

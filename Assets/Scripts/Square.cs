using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square : MonoBehaviour {

	public int x;
	public int y;
	public BuildingBlock block;

	public bool occupied { get { return block != null; } }

#if UNITY_EDITOR
	void OnDrawGizmos() {
		float cellSize = Grid.instance.cellSize;
		Gizmos.color = Color.clear;

		if (MenuController.instance.holdingBlock) {
			if (CanTake(MenuController.instance.previewItem)) {
				if (MenuController.instance.hover == this)
					Gizmos.color = Color.green;
				else
					Gizmos.color = Color.yellow;
			} else if (MenuController.instance.hover == this)
				Gizmos.color = Color.red;
		} else if (MenuController.instance.hover == this && occupied && !block.isCore) {
			Gizmos.color = Color.cyan;
		}
		
		Gizmos.DrawWireCube(transform.position, new Vector3(cellSize, cellSize));

		Vector3 pos = transform.position;
		Gizmos.DrawLine(pos + new Vector3(-cellSize / 2, -cellSize / 2), pos + new Vector3(cellSize / 2, cellSize / 2));
		Gizmos.DrawLine(pos + new Vector3(-cellSize / 2, cellSize / 2), pos + new Vector3(cellSize / 2, -cellSize / 2));
	}
#endif

	public bool CanTake(BuildingBlock block) {
		// Check if this squares neightbor got any blocks
		return !occupied && block.CanBePlacedAt(this);
	}

	public void ClearBlock() {
		Destroy(block.gameObject);
		block = null;
		Grid.instance.SendBlockUpdate(this);
	}

	// Called by the gridraycast
	public void OnLeftClick() {
		if (block != null)
			print("Connected to core: " + Grid.instance.ConnectedToCore(block));
	}

	// Called by the gridraycast
	public void OnRightClick() {
		if (block != null && !block.isCore)
			ClearBlock();
	}

}

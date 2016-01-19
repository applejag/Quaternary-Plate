using UnityEngine;
using System.Collections;

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
				if (GridRaycast.instance.hover == this)
					Gizmos.color = Color.green;
				else
					Gizmos.color = Color.yellow;
			} else if (GridRaycast.instance.hover == this)
				Gizmos.color = Color.red;
		}
		
		Gizmos.DrawWireCube(transform.position, new Vector3(cellSize, cellSize));

		Vector3 pos = transform.position;
		Gizmos.DrawLine(pos + new Vector3(-cellSize / 2, -cellSize / 2), pos + new Vector3(cellSize / 2, cellSize / 2));
		Gizmos.DrawLine(pos + new Vector3(-cellSize / 2, cellSize / 2), pos + new Vector3(cellSize / 2, -cellSize / 2));
	}
#endif

	public bool CanTake(BuildingBlock block) {
		return !occupied && Grid.instance.GetSurroundingBlocks(this).Count > 0;
	}

}

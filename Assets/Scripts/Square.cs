using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

	public int x;
	public int y;
	public BuildingBlock item;

	public bool occupied { get { return item != null; } }

#if UNITY_EDITOR
	void OnDrawGizmos() {
		float cellSize = Grid.instance.cellSize;

		Gizmos.color = GridRaycast.instance.down == this ? Color.white : GridRaycast.instance.hover == this ? Color.yellow : Color.clear;
		Gizmos.DrawWireCube(transform.position, new Vector3(cellSize, cellSize));

		Vector3 pos = transform.position;
		Gizmos.DrawLine(pos + new Vector3(-cellSize / 2, -cellSize / 2), pos + new Vector3(cellSize / 2, cellSize / 2));
		Gizmos.DrawLine(pos + new Vector3(-cellSize / 2, cellSize / 2), pos + new Vector3(cellSize / 2, -cellSize / 2));
	}
#endif

}

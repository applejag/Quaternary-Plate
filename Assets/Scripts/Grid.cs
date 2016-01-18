using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class Grid : SingletonBaseScript<Grid> {

	public float cellSize = 1;
	public int gridSize = 5;

	public List<Square> grid;

#if UNITY_EDITOR
	void OnValidate() {
		cellSize = Mathf.Max(cellSize, 0);
		gridSize = Mathf.Max(gridSize, 0);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(1, 1) * cellSize * gridSize);
	}
#endif

	protected override void Awake() {
		base.Awake();

		ResetGrid();
	}

	public Square GetObjectAt(Vector2 worldPos) {
		Vector2 pos = worldPos - transform.position.ToVector2() + Vector2.one * gridSize * cellSize / 2;
		return GetObjectAt(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
	}

	public Square GetObjectAt(int x, int y) {
		return grid.Find(delegate (Square obj) {
			return obj.x == x && obj.y == y;
		});
	}

	public void ResetGrid() {
		// First clear it
		foreach(Square square in grid) {
			Destroy(square.gameObject);
		}
		grid.Clear();

		// Then recreate it
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				GameObject obj = new GameObject("Square (x"+x+" y"+y+")");

				obj.transform.SetParent(transform);
				obj.transform.localPosition = new Vector3((x + .5f) * cellSize, (y + .5f) * cellSize) - new Vector3(1, 1) * gridSize * cellSize / 2;

				Square square = obj.AddComponent<Square>();
				square.x = x;
				square.y = y;

				grid.Add(square);
			}
		}
	}

}

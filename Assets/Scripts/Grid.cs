using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class Grid : SingletonBaseScript<Grid> {

	public float cellSize = 1;
	public int gridRadius = 3;
	public int gridSize { get { return gridRadius * 2 + 1; } }

	public List<Square> grid;

#if UNITY_EDITOR
	void OnValidate() {
		cellSize = Mathf.Max(cellSize, 0);
		gridRadius = Mathf.Max(gridRadius, 0);
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

	public Square GetSquareAt(Vector2 worldPos) {
		Vector2 pos = worldPos - transform.position.ToVector2() + Vector2.one * gridSize * cellSize / 2;
		return GetSquareAt(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
	}

	public Square GetSquareAt(int x, int y) {
		return grid.Find(delegate (Square obj) {
			return obj.x == x && obj.y == y;
		});
	}

	public Square GetCenterSquare() {
		return GetSquareAt(gridSize / 2, gridSize / 2);
	}

	public List<Square> GetSurroundingSquares(Square square) {
		List<Square> list = new List<Square>();

		list.Add(GetSquareAt(square.x, square.y-1));
		list.Add(GetSquareAt(square.x, square.y+1));
		list.Add(GetSquareAt(square.x-1, square.y));
		list.Add(GetSquareAt(square.x+1, square.y));

		// Trim
		list.RemoveAll(delegate (Square s) { return s == null; });

		return list;
	}

	public List<BuildingBlock> GetSurroundingBlocks(Square square) {
		List<BuildingBlock> list = new List<BuildingBlock>();

		foreach (Square s in GetSurroundingSquares(square)) {
			if (s.block != null) list.Add(s.block);
		}

		return list;
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

	public void SendBlockUpdate(Square from) {
		if (from.block != null)
			from.block.BlockUpdate();

		foreach(BuildingBlock block in GetSurroundingBlocks(from)) {
			block.BlockUpdate();
		}
	}

	// Search algorithm
	public bool ConnectedToCore(BuildingBlock block) {
		return ConnectedToCore(block, new List<BuildingBlock>());
	}

	public bool ConnectedToCore(BuildingBlock block, List<BuildingBlock> searched) {
		if (block.isCore)
			return true;

		searched.Add(block);

		foreach(BuildingBlock b in GetSurroundingBlocks(block.square)) {
			if (searched.Contains(b))
				continue;

			searched.Add(b);
			if (ConnectedToCore(b, searched))
				return true;
		}

		return false;
	}

}

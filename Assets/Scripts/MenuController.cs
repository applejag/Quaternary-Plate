using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : SingletonBaseScript<MenuController> {

	public GameObject startingPiece;

	[HideInInspector]
	public BuildingBlock previewItem;
	private MenuItem selectedItem;

	public bool holdingBlock { get { return previewItem != null; } }
	public Vector3 mousePos { get {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = transform.position.z;
			return pos;
		} }

	void Start() {
		Select(null);
		PlaceOnGrid(Grid.instance.GetCenterSquare(), prefab:startingPiece, forcePlace:true);
	}

	void Update() {
		if (selectedItem != null && Input.mousePresent) {
			// Move the preview
			previewItem.transform.position = mousePos;

			// Drop it
			if (Input.GetMouseButtonUp(0))
				PlaceOnGrid(Grid.instance.GetSquareAt(mousePos));
		}
	}

	void ClearSelection() {
		selectedItem = null;
		if (previewItem != null)
			Destroy(previewItem.gameObject);
	}

	public void Select(MenuItem item) {
		if (selectedItem != null) ClearSelection();

		if (item == selectedItem || item == null) {
			// Deselect
			ClearSelection();
		} else if (item != null) {
			// Select
			selectedItem = item;

			// Create
			if (item.prefab != null) {
				previewItem = CreateBuildingBlock(item.prefab);

				if (previewItem == null) {
					Debug.LogError("Please have a buildingblock component attached, it's kinda required.");
				}
			} else {
				Debug.LogError("Please assign a prefab for the menuitem \"" + item.name + "\"!");
			}
		}
	}

	BuildingBlock CreateBuildingBlock(GameObject prefab) {
		GameObject clone = Instantiate(prefab, mousePos, prefab.transform.rotation) as GameObject;
		clone.transform.SetParent(transform);
		return clone.GetComponent<BuildingBlock>();
	}

	/// <summary>
	/// Create a block from the given prefab and place it on the grid.
	/// </summary>
	void PlaceOnGrid(Square square, GameObject prefab, bool forcePlace = false) {
		if (square != null && prefab != null) {
			PlaceOnGrid(square, CreateBuildingBlock(prefab), forcePlace);
		}
	}

	/// <summary>
	/// Place the currently selected block on the grid.
	/// </summary>
	void PlaceOnGrid(Square square, bool forcePlace = false) {
		if (selectedItem != null && square != null) {
			PlaceOnGrid(square, previewItem, forcePlace);
			previewItem = null;

			Select(null);
		}
	}

	/// <summary>
	/// Place the given block on the grid.
	/// If forcePlace is true, it will place it on the square, even if the square doesn't allow it.
	/// </summary>
	void PlaceOnGrid(Square square, BuildingBlock block, bool forcePlace = false) {
		// This is where all PlaceOnGrid methods ends up, this is the final check
		if (square != null && block != null) {
			if (forcePlace || square.CanTake(block)) {
				block.transform.SetParent(square.transform);
				block.transform.localPosition = Vector3.zero;
				square.block = block;
			} else {
				// Can't take it; don't need it.
				Destroy(block.gameObject);
			}
		}
	}
}

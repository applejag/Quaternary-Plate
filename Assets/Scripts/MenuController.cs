using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExtensionMethods;

public class MenuController : SingletonBaseScript<MenuController> {

	public GameObject startingPiece;

	[HideInInspector]
	public BuildingBlock previewItem;
	private MenuItem selectedItem;

	private Square leftDown;
	private Square rightDown;

	[HideInInspector]
	public Square hover;
	
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
		// MenuController
		if (selectedItem != null && Input.mousePresent) {
			// Move the preview
			previewItem.transform.position = mousePos;

			// Duplicate it
			if (Input.GetMouseButtonDown(1)) {
				PlaceOnGrid(Grid.instance.GetSquareAt(mousePos), selectedItem.prefab, false);
			}

			// Drop it
			if (Input.GetMouseButtonUp(0)) {
                PlaceOnGrid(Grid.instance.GetSquareAt(mousePos));
				Select(null);
			}
		}

		// GridRaycast
		if (Input.mousePresent) {
			
			Square square = Grid.instance.GetSquareAt(mousePos.ToVector2());

			if (square != hover) {
				// Just hovered over a new one
				hover = square;
			} else if (square == null) {
				// Mouse left the current one
				hover = null;
			}

			if (hover != null && !holdingBlock) {
				// Left clicking
				if (Input.GetMouseButtonDown(0)) {
					leftDown = hover;
				} else if (hover == leftDown && Input.GetMouseButtonUp(0)) {
					// Clicked it
					//print("leftclicked " + leftDown.name);
					leftDown.OnLeftClick();
				}

				// Right clicking
				if (Input.GetMouseButtonDown(1)) {
					rightDown = hover;
				} else if (hover == rightDown && Input.GetMouseButtonUp(1)) {
					// Clicked it
					//print("rightclicked " + rightDown.name);
					rightDown.OnRightClick();
				}
			}

		} else {
			// Mouse left the current one
			hover = null;
		}

		// Failsafe
		if (!Input.GetMouseButton(0)) {
			leftDown = null;
		}
		if (!Input.GetMouseButton(1)) {
			rightDown = null;
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

				block.OnPlace(square);
				Grid.instance.SendBlockUpdate(square);
			} else {
				// Can't take it; don't need it.
				Destroy(block.gameObject);
			}
		}
	}
}

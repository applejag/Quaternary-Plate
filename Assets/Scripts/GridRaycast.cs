using UnityEngine;
using System.Collections;
using ExtensionMethods;

[RequireComponent(typeof(Grid))]
public class GridRaycast : SingletonBaseScript<GridRaycast> {

	private Grid grid;
	
	public Square down;
	public Square hover;
	public Square lastHover;

	protected override void Awake() {
		base.Awake();
		grid = GetComponent<Grid>();
	}
	
	void Update () {
		if (Input.mousePresent) {

			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Square square = grid.GetObjectAt(pos.ToVector2());

			if (square != hover) {
				// Just hovered over a new one
				SetCurrent(square);
			} else if (square == null) {
				// Mouse left the current one
				SetCurrent(null);
			}

			if (hover != null) {
				if (Input.GetMouseButtonDown(0)) {
					down = hover;
				} else if (hover == down && Input.GetMouseButtonUp(0)) {
					// Clicked it
					print("clicked " + down.name);
					if (!square.occupied)
						MenuController.instance.PlaceOnGrid(down);
				}
            }

		} else {
			// Mouse left the current one
			SetCurrent(null);
		}

		if (Input.GetMouseButtonUp(0)) {
			down = null;
		}
	}

	void SetCurrent(Square square) {
		lastHover = hover;
		hover = square;
	}
}

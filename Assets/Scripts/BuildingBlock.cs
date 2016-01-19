using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingBlock : MonoBehaviour {

	[Header("Placement filters")]
	public Filter filter;

	[Header("Other fields")]
	public bool isCore = false;
	public Object[] activateOnPlace;

	[HideInInspector]
	public Square square;

	public virtual void OnPlace(Square square) {
		this.square = square;
		foreach (Object obj in activateOnPlace) {
			GameObject go = obj as GameObject;
			ParticleSystem ps = obj as ParticleSystem;
			MonoBehaviour mb = obj as MonoBehaviour;

			if (go != null)
				go.SetActive(true);
			else if (ps != null)
				ps.Play();
			else if (mb != null)
				mb.enabled = true;
		}
	}

	// A nearby block got placed/removed
	public virtual void BlockUpdate() {
		if (!Grid.instance.ConnectedToCore(this)) {
			square.ClearBlock();
		}
	}

	public bool CanBePlacedAt(Square square) {
		List<BuildingBlock> list = Grid.instance.GetSurroundingBlocks(square);
		if (list.Count == 0) return false;

		foreach(BuildingBlock block in list) {
			int deltaX = block.square.x - square.x;
			int deltaY = block.square.y - square.y;

			// Check if this can have a block next to it
			if (!filter.GetFilterBool(deltaX, deltaY))
				return false;

			// Check if the other block can have this next to it
			if (!block.filter.GetFilterBool(-deltaX, -deltaY))
				return false;
		}

		return true;
	}

	
	[System.Serializable]
	public class Filter {
		public bool above = true;
		public bool below = true;
		public bool right = true;
		public bool left = true;

		public bool GetFilterBool(int deltaX, int deltaY) {
			if (deltaX == 0 && deltaY == 1) return above;
			if (deltaX == 0 && deltaY == -1) return below;
			if (deltaX == 1 && deltaY == 0) return right;
			if (deltaX == -1 && deltaY == 0) return left;
			return true;
		}
	}

}

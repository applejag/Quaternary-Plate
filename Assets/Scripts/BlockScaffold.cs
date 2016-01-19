using UnityEngine;
using System.Collections;

public class BlockScaffold : BuildingBlock {

	[Header("Scaffold fields")]
	public GameObject activeOnAbove;
	public GameObject activeOnBelow;
	public GameObject activeOnRight;
	public GameObject activeOnLeft;

	public override void BlockUpdate() {
		base.BlockUpdate();

		SetScaffoldActive(activeOnAbove, 0, 1);
		SetScaffoldActive(activeOnBelow, 0, -1);
		SetScaffoldActive(activeOnRight, 1, 0);
		SetScaffoldActive(activeOnLeft, -1, 0);
	}

	void SetScaffoldActive(GameObject obj, int dx, int dy) {
		Square square = Grid.instance.GetSquareAt(this.square.x + dx, this.square.y + dy);
		if (obj != null)
			obj.SetActive(square != null && square.occupied);
	}
}

using UnityEngine;
using System.Collections;

public class RotatorScript : MonoBehaviour {

	public Vector3 eulerMotion;
	public Space relativeTo;
	public MoveVia moveVia;

	public Rigidbody rbody;
	public Rigidbody2D rbody2D;

	void Update() {
		switch (moveVia) {
			case MoveVia.transform:
				transform.Rotate(eulerMotion * Time.deltaTime, relativeTo);
				break;

			case MoveVia.rigidbody:

				break;

			case MoveVia.rigidbody2D:

				break;
		}
	}

	public enum MoveVia {
		transform, rigidbody, rigidbody2D
	}
}

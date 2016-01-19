using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuItem : MonoBehaviour {

	public Image image;
	public GameObject prefab;

	public void OnPointerDown() {
		if (Input.GetMouseButtonDown(0))
			MenuController.instance.Select(this);
	}

}

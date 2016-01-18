using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuItem : MonoBehaviour {

	public Image image;
	public GameObject prefab;

	public void OnClick() {
		MenuController.instance.Select(this);
	}

}

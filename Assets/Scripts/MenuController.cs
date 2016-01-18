using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : SingletonBaseScript<MenuController> {

	public Image selectedImage;
	public MenuItem selectedItem;

	void Start() {
		Select(null);
	}

	void Update() {
		if (selectedImage && Input.mousePresent) {
			selectedImage.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			selectedImage.transform.localPosition = new Vector3(selectedImage.transform.localPosition.x, selectedImage.transform.localPosition.y);
		}
	}

	public void Select(MenuItem item) {
		if (item == selectedItem || item == null) {
			// Deselect
			selectedItem = null;
			selectedImage.color = Color.clear;
		} else {
			// Select
			selectedImage.sprite = item.image != null ? item.image.sprite : null;
			selectedImage.color = item.image != null ? item.image.color : Color.green;
			selectedItem = item;
		}
	}

	public void PlaceOnGrid(Square square) {
		if (selectedItem) {
			GameObject clone = Instantiate(selectedItem.prefab, square.transform.position, selectedItem.transform.rotation) as GameObject;
			clone.transform.SetParent(square.transform);
			square.item = clone.GetComponent<BuildingBlock>();
			Select(null);
		}
	}
}

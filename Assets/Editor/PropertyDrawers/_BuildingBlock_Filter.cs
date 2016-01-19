using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(BuildingBlock.Filter))]
public class _BuildingBlock_Filter : PropertyDrawer {

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return base.GetPropertyHeight(property, label) * 3 + 6;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, label);

		float height = EditorGUIUtility.singleLineHeight;

		GUI.Box(position, "");
		position.width -= 6;
		position.height -= 6;
		position.x += 3;
		position.y += 3;

		Rect leftRect = new Rect(position.x, position.y + height, position.width / 2, height);
		Rect rightRect = new Rect(position.x + position.width / 2, position.y + height, position.width / 2, height);
		Rect aboveRect = new Rect(position.x + position.width / 4, position.y, position.width / 2, height);
		Rect belowRect = new Rect(position.x + position.width / 4, position.y + height*2, position.width / 2, height);

		SerializedProperty left = property.FindPropertyRelative("left");
		SerializedProperty right = property.FindPropertyRelative("right");
		SerializedProperty above = property.FindPropertyRelative("above");
		SerializedProperty below = property.FindPropertyRelative("below");

		GUIContent leftLabel = new GUIContent(left.displayName);
		GUIContent rightLabel = new GUIContent(right.displayName);
		GUIContent aboveLabel = new GUIContent(above.displayName);
		GUIContent belowLabel = new GUIContent(below.displayName);

		EditorGUI.BeginProperty(leftRect, leftLabel, left);
			left.boolValue = EditorGUI.ToggleLeft(leftRect, "Left", left.boolValue);
		EditorGUI.EndProperty();
		EditorGUI.BeginProperty(rightRect, rightLabel, right);
			right.boolValue = EditorGUI.ToggleLeft(rightRect, "Right", right.boolValue);
		EditorGUI.EndProperty();
		EditorGUI.BeginProperty(aboveRect, aboveLabel, above);
			above.boolValue = EditorGUI.ToggleLeft(aboveRect, "Above", above.boolValue);
		EditorGUI.EndProperty();
		EditorGUI.BeginProperty(belowRect, belowLabel, below);
			below.boolValue = EditorGUI.ToggleLeft(belowRect, "Below", below.boolValue);
		EditorGUI.EndProperty();

		EditorGUI.EndProperty();
	}

}

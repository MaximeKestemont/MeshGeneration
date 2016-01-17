using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {

	float v = .5f;

	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();
		DrawDefaultInspector();

		//  Currently useless, could be useful to parametrize the BuildMesh functon to fill it the slider value
		EditorGUILayout.BeginVertical();
		v = EditorGUILayout.Slider(v, 0, 2.0f);
		EditorGUILayout.EndVertical();

		if (GUILayout.Button("Regenerate")) {
			Debug.Log("Regenerate");
			TileMap tileMap = (TileMap)target;
			tileMap.BuildMesh();
		}
	}
}

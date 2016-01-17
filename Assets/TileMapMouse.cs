using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {

	TileMap _tileMap;

	Vector3 currentTileCoord;

	public Transform selectionCube;

	void Start() {
		_tileMap = GetComponent<TileMap>();
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if ( GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity)) {
			int x = Mathf.FloorToInt(hitInfo.point.x / _tileMap.tileSize);
			int z = Mathf.FloorToInt(hitInfo.point.z / _tileMap.tileSize);

			currentTileCoord.x = x;
			currentTileCoord.z = z;
			
			selectionCube.transform.position = currentTileCoord * 5f;

			//GetComponent<Renderer>().material.color = Color.red;
		} else {
			//GetComponent<Renderer>().material.color = Color.green;
		}


/*		if (Input.GetMouseButtonDown(1) && selectedUnit != null) {
			selectedUnit.MoveToCoord(currentTileCoord.x, currentTileCoord.z);
		}*/	
	}
}

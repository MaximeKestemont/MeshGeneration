using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class TileMap : MonoBehaviour {

	public int size_x = 100;
	public int size_z = 50;
	public float tileSize = 1.0f;

	public Texture2D terrainTiles;
	public int tileResolution = 16;

	// Use this for initialization
	void Start () {
		BuildMesh();
	}

	Color[][] ChopUpTiles() {
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;

		Color[][] tiles = new Color[numTilesPerRow * numRows][]; // second dimension is the colors per tile, as a texture is by itself an array of color

		for ( int i = 0 ; i < numRows ; i++ ) {
			for ( int x = 0 ; x < numTilesPerRow ; x++ ) {
				tiles[i*numTilesPerRow + x] = terrainTiles.GetPixels( x * tileResolution, i * tileResolution, tileResolution, tileResolution);
			}
		}

		return tiles;
	}

	void BuildTexture() {



		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);

		Color[][] tiles = ChopUpTiles();

		for (int y = 0; y < size_z; y++) {
			for (int x = 0; x < size_x; x++) {
				Color[] p = tiles[Random.Range(0, 4)];
				texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}

		}

		texture.filterMode = FilterMode.Point; // Make the passing from one pixel to the other not blurry (blending)
		texture.Apply();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;

		Debug.Log("Done Texture");
	}

	public void BuildMesh() {
		
		int numTiles = size_x * size_z;
		int numTriangles = numTiles * 2;
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_z * vsize_x;

		// Generate the mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		int[] triangles = new int[numTriangles * 3];

		int x, z;

		// height
		for ( z = 0 ; z < vsize_z ; z++ ) {
			// row
			for ( x = 0 ; x < vsize_x ; x++ ) {
				vertices[ z * vsize_x + x ] = new Vector3(x * tileSize, 0, z * tileSize); //new Vector3(x * tileSize, Random.Range(-1f, 1f), z * tileSize);
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / vsize_x , (float)z / vsize_z );
			}
		}

		Debug.Log("test");
		// height
		for ( z = 0 ; z < size_z ; z++ ) {
			// row
			for ( x = 0 ; x < size_x ; x++ ) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;

				triangles[triOffset + 0] = z * vsize_x + x + 		   0; 
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;

				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = z * vsize_x + x + 		   1;
			} 
		}

		// Create a new mesh
		Mesh mesh = new Mesh();

		// Populate the mesh with the data
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		// Assign the mesh to the renderer/filter/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();

		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log("Done mesh");

		BuildTexture();
	}
	
}

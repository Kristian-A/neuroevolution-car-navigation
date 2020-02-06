using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public GameObject tilePrefab;

	private static Tile[,] tileMap;

	void Start () {
		float tileScale = tilePrefab.GetComponent<Transform>().localScale.x;
		
		float xDiff = endPos.x - startPos.x;
		float zDiff = endPos.z - startPos.z;
		int xDir = xDiff > 0 ? 1 : -1;
		int zDir = zDiff > 0 ? 1 : -1;
		int tileCountX = (int)( Mathf.Abs(xDiff)/tileScale ) + 1;
		int tileCountZ = (int)( Mathf.Abs(zDiff)/tileScale ) + 1;

		TileController.tileMap = new Tile[tileCountZ, tileCountX];

		for (int z = 0; z < tileCountZ; z++) {
			for (int x = 0; x < tileCountX; x++) {
				GameObject tileObject = Instantiate(tilePrefab, startPos + new Vector3(x * tileScale * xDir, 0, z * tileScale * zDir), Quaternion.identity);
				Tile tile = tileObject.GetComponent<Tile>();
				tile.setPos(new Vector2(z, x));
				TileController.tileMap[z, x] = tile;
			}
		}
	}	
	
	public static Tile[,] getMap() {
		return tileMap;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public GameObject tile;

	private List<List<Tile>> tileMap = new List<List<Tile>>();
	void Start () {
		float tileScale = tile.GetComponent<Transform>().localScale.x;
		
		float xDiff = endPos.x - startPos.x;
		float zDiff = endPos.z - startPos.z;
		int xDir = xDiff > 0 ? 1 : -1;
		int zDir = zDiff > 0 ? 1 : -1;
		int tileNumX = (int)( Mathf.Abs(xDiff)/tileScale ) + 1;
		int tileNumZ = (int)( Mathf.Abs(zDiff)/tileScale ) + 1;

		for (int z = 0; z < tileNumZ; z++) {
			List<Tile> row = new List<Tile>();
			for (int x = 0; x < tileNumX; x++) {
				GameObject tileGO = Instantiate(tile, startPos + new Vector3(x * tileScale * xDir, 0, z * tileScale * zDir), Quaternion.identity);
				row.Add(tileGO.GetComponent<Tile>());
			}
			tileMap.Add(row);
		}

		print(tileMap[0][0].IsWall());
	}
	
	void Update () {
		
	}
}

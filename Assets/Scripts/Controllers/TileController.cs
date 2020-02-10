using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public GameObject tilePrefab;
	public Vector2 start;
	public Vector2 end;

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
				tile.SetPos(new Vector2(x, z));
				TileController.tileMap[z, x] = tile;
			}
		}
	}	
	
	void Update() {
		Reset();
		
		Tile startTile = TileController.tileMap[(int)start.y, (int)start.x];
		Tile endTile = TileController.tileMap[(int)end.y, (int)end.x];

		PathFinding.FindPath(startTile, endTile);	

	}

	public static void Reset() {
		foreach (Tile tile in tileMap) {
			tile.Reset();
		}
	}

	public static List<Tile> GetNeighbours(Tile tile) {
		Vector2 pos = tile.GetPos();
		int x = (int)pos.x;
		int y = (int)pos.y;

		List<Tile> neighbours = new List<Tile>();

		for (int j = -1; j <= 1; j++) {
			for (int i = -1; i <= 1; i++) {
				int nX = x+i;
				int nY = y+j;

				if (( nX == x && nY == y ) ||
					( nX < 0 || nX >= tileMap.GetLength(1) ) ||
					( nY < 0 || nY >= tileMap.GetLength(0) )) {
					continue;
				}

				neighbours.Add(tileMap[nY, nX]);
			}
		}

		return neighbours;
	}
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathFinding : MonoBehaviour {
	
	public static List<Tile> FindPath(Tile startTile, Tile endTile) {
		List<Tile> openSet = new List<Tile>();
		HashSet<Tile> closedSet = new HashSet<Tile>();

		openSet.Add(startTile);

		while (openSet.Count > 0) {
			Tile currentTile = openSet[0];
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet[i].Fcost() < currentTile.Fcost() || openSet[i].Fcost() == currentTile.Fcost() &&
					openSet[i].Hcost() < currentTile.Hcost() ) {
					currentTile = openSet[i];
				}
			}

			openSet.Remove(currentTile);
			closedSet.Add(currentTile);

			if (currentTile.Equals(endTile)) {
				return Retrace(startTile, endTile);
			}

			foreach (Tile neighbour in TileController.GetNeighbours(currentTile)) {
			
				if (neighbour.IsWall() || closedSet.Contains(neighbour)) {
					continue;
				}
				
				int newCostToNeighbour = currentTile.Gcost() + GetDistance(currentTile, neighbour);			
				if (newCostToNeighbour < neighbour.Gcost() || !openSet.Contains(neighbour)) {
					neighbour.SetG(newCostToNeighbour);
					neighbour.SetH(GetDistance(neighbour, endTile));
					neighbour.SetPrevious(currentTile);

					if (!openSet.Contains(neighbour)) {
						openSet.Add(neighbour);
					}
				}
			}
		}

		return null;
	}

	private static int GetDistance(Tile a, Tile b) {
		Vector2 aPos = a.GetPos();
		Vector2 bPos = b.GetPos();
		int xDist = (int)Math.Abs(aPos.x - bPos.x);
		int yDist = (int)Math.Abs(aPos.y - bPos.y);

		if (xDist > yDist) {
			return 14*yDist + 10*(xDist - yDist);
		}
		return 14*xDist + 10*(yDist - xDist);
	}

	private static List<Tile> Retrace(Tile startTile, Tile endTile) {
		List<Tile> path = new List<Tile>();
		Tile currentTile = endTile;

		while (currentTile != startTile) {
			path.Add(currentTile);
			currentTile = currentTile.Previous();
		}

		path.Reverse();
		foreach (Tile tile in path) {
			tile.SetPath();
		}

		return path;
	}
}

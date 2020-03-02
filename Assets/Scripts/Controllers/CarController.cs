using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public int carCount = 2;
	public GameObject carPrefab;

	private static List<GameObject> cars;
	private List<Tile> spawnpoints;
	void Start() {
		spawnpoints = TileController.GetSpawnpoints();
		
		CarController.cars = new List<GameObject>();
		for (int i = 0; i < carCount; i++) {
			Tile currSpawnpoint = spawnpoints[i % spawnpoints.Count];
			GameObject car = Instantiate(carPrefab, currSpawnpoint.GetWorldPos(), Quaternion.identity);
			car.GetComponent<CarMovement>().SetSpawnpoint(currSpawnpoint);

			CarController.cars.Add(car);
			List<Collider> colls = car.GetComponent<CarMovement>().GetColliders();
			
			foreach (GameObject other in CarController.cars) {
				if (car == other) {
					continue;
				}
				DisableCollisions(colls, other.GetComponent<CarMovement>().GetColliders());
			}
		}
	}

	void FixedUpdate() {
		foreach (GameObject car in CarController.cars) {
			// print(car.GetComponent<CarMovement>().GetCompletedTiles());
		}
	}
	
	public static List<GameObject> GetCars() {
		return cars;
	}

	private void DisableCollisions(List<Collider> first, List<Collider> second) {
		foreach (Collider fCol in first) {
			foreach (Collider sCol in second) {
				Physics.IgnoreCollision(fCol, sCol);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public int carCount = 2;
	public GameObject carPrefab;

	private static List<CarMovement> cars;
	private List<Tile> spawnpoints;
	private static Timer restartTimer = new Timer(5000);
	private static bool generationDone = false;
	void Start() {
		spawnpoints = TileController.GetSpawnpoints();
		
		CarController.cars = new List<CarMovement>();
		for (int i = 0; i < carCount; i++) {
			Tile currSpawnpoint = spawnpoints[i % spawnpoints.Count];
			GameObject carObject = Instantiate(carPrefab, currSpawnpoint.GetWorldPos(), Quaternion.identity);
			CarMovement car = carObject.GetComponent<CarMovement>();
			car.GetComponent<CarMovement>().SetSpawnpoint(currSpawnpoint);

			CarController.cars.Add(car);
			List<Collider> colls = car.GetColliders();
			
			foreach (CarMovement other in CarController.cars) {
				if (car == other) {
					continue;
				}
				DisableCollisions(colls, other.GetColliders());
			}
		}
	}

	void Update() {
		if (CarController.restartTimer.IsElapsed()) {
			CarController.generationDone = true;
		}
	}
	
	public static bool GenerationDone() {
		bool ret = CarController.generationDone;
		if (CarController.generationDone) {
			CarController.generationDone = false;
		}
		return ret;
	}

	public static List<CarMovement> GetCars() {
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

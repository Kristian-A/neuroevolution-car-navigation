using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public int carCount = 2;
	public GameObject carPrefab;
	private static bool demonstrating = false;

	private static List<CarMovement> cars;
	private List<Tile> spawnpoints;
	private static Timer restartTimer = new Timer(20000);
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
		if (CarController.demonstrating) {
			return;
		} 
		
		if (CarController.restartTimer.IsElapsed()) {	
			CarController.generationDone = true;
			TileController.Reset();
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

	public static void Demonstrate(List<double> DNA) {
		foreach (var car in cars) {
			car.gameObject.SetActive(false);
		}

		var simCar = cars[0];

		simCar.Reset();

		var nnWeights = AIController.nnSize;
		var brain = new NeuralNetwork(nnWeights[0], nnWeights[1], nnWeights[2]);

		brain.SetWeights(DNA);

		simCar.SetBrain(brain);
		simCar.gameObject.SetActive(true);

		demonstrating = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	private static bool createdBrains = false;

    public void Start() {
		List<GameObject> cars = CarController.GetCars();
		print(cars.Count);

		foreach (GameObject car in cars) {
		    car.GetComponent<CarMovement>().SetBrain(new NeuralNetwork(8, 15, 2));
		}
	}

	public void Update() {

	}
}
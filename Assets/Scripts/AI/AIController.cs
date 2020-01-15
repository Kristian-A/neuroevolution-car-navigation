using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	private static bool createdBrains = false;
    public void Start() {
	}

	public void Update() {
		if (!CarController.Ready() || createdBrains) {
			return;
		}
		
		List<GameObject> cars = CarController.GetCars();
        
		foreach (GameObject car in cars) {
		    car.GetComponent<CarMovement>().SetBrain(new NeuralNetwork(6, 15, 2));
		}
		
		createdBrains = true;
	}

	public static bool Ready() {
		return createdBrains;
	}
}
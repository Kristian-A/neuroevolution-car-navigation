using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    public void Start() {
    }

	public void Update() {
		if (!CarController.Ready()) {
			print("hui");
			return;
		}

		List<GameObject> cars = CarController.GetCars();

        foreach (GameObject car in cars) {
            car.GetComponent<CarMovement>().SetBrain(new NeuralNetwork(6, 15, 3));
        }
	}
}
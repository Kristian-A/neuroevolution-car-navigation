using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	private static bool createdBrains = false;

    public void Start() {
		NeuralNetwork m = new NeuralNetwork(1, 2, 2);
		List<double> a = new List<double>();
		int size = m.DNA().Count;
		for (int i = 0; i < size; i++) {
			a.Add(i);
		}

		m.SetWeights(a);

		foreach (double num in m.DNA()) {
			
			print(num);
		}
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	private static bool createdBrains = false;

    public void Start() {
		// NeuralNetwork m = new NeuralNetwork(1, 2, 2);
		// List<double> a = new List<double>();
		// int size = m.DNA().Count;
		// for (int i = 0; i < size; i++) {
		// 	a.Add(i);
		// }

		// m.SetWeights(a);

		
	
		List<double> a = new List<double>();
		List<double> b = new List<double>();
		List<double> c = new List<double>();
		

		a.Add(0);
		a.Add(1);
		a.Add(2);

		b.Add(-1);
		b.Add(-2);
		b.Add(-3);

		c.Add(4);
		c.Add(4);
		c.Add(4);

		GeneticAlgorithm.Entry e1 = new GeneticAlgorithm.Entry(a, 0.2);
		GeneticAlgorithm.Entry e2 = new GeneticAlgorithm.Entry(b, 0.3);
		GeneticAlgorithm.Entry e3 = new GeneticAlgorithm.Entry(c, 0.5);
		
		List<GeneticAlgorithm.Entry> l = new List<GeneticAlgorithm.Entry>();

		l.Add(e1);
		l.Add(e2);
		l.Add(e3);

		GeneticAlgorithm.Entry res = GeneticAlgorithm.Pick(l);

		foreach (double num in res.GetDna()) {
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
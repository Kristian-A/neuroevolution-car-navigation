using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	private List<CarMovement> cars;
	private int[] nnSize = new int[] {8, 30, 2};

    public void Start() {

		cars = CarController.GetCars();
		foreach (CarMovement car in cars) {		    
			car.SetBrain(new NeuralNetwork(nnSize[0], nnSize[1], nnSize[2]));
		}
	}

	public void Update() {
		if (CarController.GenerationDone()) {
			double maxFitness = 0;
			double sumFitness = 0;
			var entries = new List<GeneticAlgorithm.Entry>(); 
			foreach (CarMovement car in cars) {
				var dna = car.GetBrain().DNA();
				var fitness = car.Score();
				entries.Add(new GeneticAlgorithm.Entry(dna, fitness));
			
				if (maxFitness < fitness) {
					maxFitness = fitness;
				}

				sumFitness += fitness;
			}

			print("Max:" + maxFitness);
			print("Average:" + sumFitness/entries.Count);

			var DNAs = NextGeneration(entries);

			for (int i = 0; i < DNAs.Count; i++) {
				var brain = new NeuralNetwork(nnSize[0], nnSize[1], nnSize[2]); 
				brain.SetWeights(DNAs[i]);
				cars[i].SetBrain(brain);
			}
		}
	}

	List<List<double>> NextGeneration(List<GeneticAlgorithm.Entry> entries) {
		int half = cars.Count / 2;
		var pool = new List<List<double>>(); 
		for (int i = 0; i < half; i++) {
			var entry = GeneticAlgorithm.Pick(entries);
			pool.Add(entry.GetDna());
		}

		for (int i = 0; i < half/2; i++) {
			var parent1 = GeneticAlgorithm.Pick(entries);
			var parent2 = GeneticAlgorithm.Pick(entries);
			var children = GeneticAlgorithm.Crossover(parent1.GetDna(), parent2.GetDna());
			
			pool.Add(children[0]);
			pool.Add(children[1]);
		}

		return pool;
	}

	private List<T> Shuffle<T>(List<T> l) {
		for (int j = 0; j < 100; j++) {
			for (int i = 0; i < l.Count; i++) {
				T temp = l[i];
				int randomIndex = Random.Range(i, l.Count);
				l[i] = l[randomIndex];
				l[randomIndex] = temp;
			}
		}

		return l;
	}
}
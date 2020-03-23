using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour {

	private static List<Agent> agents = new List<Agent>();
	private static int generationCount = 0;
	public static int[] nnSize = new int[] {8, 30, 2};
	public GameObject textfield;

    public void Start() {
		GeneticAlgorithm.Seed(0);
		var cars = CarController.GetCars();
		foreach (var car in cars) {
			agents.Add(car.GetAgent());
		}
		WriteInfo(0, 0, 0);
	}

	public void Update() {
		if (CarController.GenerationDone()) {

			Average avgFitness = new Average();
			var entries = GetEntries(); 
			
			foreach (var entry in entries) {
				avgFitness.Add((float)entry.GetFitness());
			}
			
			WriteInfo((float)GetBestEntry().GetFitness(), avgFitness.Get(), ++generationCount);

			var DNAs = NextGeneration(entries);

			for (int i = 0; i < DNAs.Count; i++) {
				var brain = new NeuralNetwork(nnSize[0], nnSize[1], nnSize[2]); 
				brain.SetWeights(DNAs[i]);
				agents[i].SetBrain(brain);
			}
		}
	}

	private void WriteInfo(float max, float avg, int generation) {
		var text = "Max: " + max + "\n" +
					"Average: " + avg + "\n" +
					"Generation: " + generation;					
				
		textfield.GetComponent<Text>().text = text;
	}

	private static List<GeneticAlgorithm.Entry> GetEntries() {
		var entries = new List<GeneticAlgorithm.Entry>();

		foreach (Agent agent in agents) {
			var dna = agent.GetBrain().DNA();
			var fitness = agent.Score();
			entries.Add(new GeneticAlgorithm.Entry(dna, fitness));
		}

		return entries;
	}

	public static GeneticAlgorithm.Entry GetBestEntry() {
		var entries = GetEntries();

		var bestEntry = entries[0];

		foreach (var entry in entries) {

			if (entry.GetFitness() > bestEntry.GetFitness()) {
				bestEntry = entry;
			}
		}

		return bestEntry;
	}

	private static List<List<double>> NextGeneration(List<GeneticAlgorithm.Entry> entries) {
		entries.Sort((GeneticAlgorithm.Entry a, GeneticAlgorithm.Entry b) => {
            return (int)(Math.Ceiling(b.GetFitness()) - Math.Ceiling(a.GetFitness()));
        });
		
		int half = agents.Count / 2;
		var pool = new List<List<double>>();
		
		pool.Add(GetBestEntry().GetDna());

		for (int i = 0; i < half - 1; i++) {
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

		return Shuffle(pool);
	}

	private static List<T> Shuffle<T>(List<T> l) {
		for (int j = 0; j < 100; j++) {
			for (int i = 0; i < l.Count; i++) {
				T temp = l[i];
				int randomIndex = UnityEngine.Random.Range(i, l.Count);
				l[i] = l[randomIndex];
				l[randomIndex] = temp;
			}
		}

		return l;
	}

	public static int GenerationCount() {
		return generationCount;
	}
}
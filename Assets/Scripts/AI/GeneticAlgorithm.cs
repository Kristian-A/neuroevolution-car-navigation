using System;
using System.Collections.Generic;

public class GeneticAlgorithm {

    public class Entry {

        private List<Double> dna;
        private double fitness;

        public Entry(List<Double> dna, double fitness) {
            this.dna = dna;
            this.fitness = fitness;
        }

        public double GetProbability(double sumFitness) {
            return fitness/sumFitness;
        }

        public double GetFitness() {
            return fitness;
        }

        public List<Double> GetDna() {
            return dna;
        }
    }

    private static Random generator = new Random(0);
    
    public static List<double>[] Crossover(List<double> dna1, List<double> dna2) {
        List<double> child1 = new List<double>();
        List<double> child2 = new List<double>();

        int intersection = generator.Next(dna1.Count);
        List<double> parent1 = dna1;
        List<double> parent2 = dna2;

        for (int i = 0; i < dna1.Count; i++) {
            if (i > intersection) {
                parent1 = dna2;
                parent2 = dna1;
            }

            child1.Add(parent1[i]);
            child2.Add(parent2[i]);
        }

        return new[] { Mutate(child1), Mutate(child2) };
    }   

    private static List<Double> Mutate(List<Double> dna) {
        const double mutationRate = 0.01;

        if (generator.NextDouble() < mutationRate) {
            int toSwap1 = generator.Next(dna.Count);
            int toSwap2 = generator.Next(dna.Count);

            double gene = dna[toSwap1];
            dna[toSwap1] = dna[toSwap2];
            dna[toSwap2] = gene;
        }

        return dna;
    }

    public static Entry Pick(List<Entry> entries) {

        double sumFitness = 0;
        foreach (Entry entry in entries) {
            sumFitness += entry.GetFitness();
        }

        double previousProb = 0; 

        foreach (Entry entry in entries) {
            double currentProb = entry.GetProbability(sumFitness); 

            if (generator.NextDouble() < currentProb + previousProb) {
                return entry;
            }
            previousProb += currentProb;
        }

        return null;
    }

    public static void Seed(int seed) {
        generator = new Random(seed);
    }
}
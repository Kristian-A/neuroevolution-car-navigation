using System;
using System.Collections.Generic;
public class NeuralNetwork {

    private Matrix w_ih;
    private Matrix w_ho;
    private Func<double, double> tanh = x => Math.Tanh(x);
    private Func<double, double> LReLU = x => x > 0 ? x : x*0.01;
    public NeuralNetwork(int inputs, int hidden, int outputs) {
        w_ih = new Matrix(hidden, inputs+1);
        w_ih.Randomize();
        w_ho = new Matrix(outputs, hidden);
        w_ho.Randomize();
    }

    public Matrix FeedForward(Matrix inputs) {
        Matrix result = Biased(inputs)*w_ih;
        result.Activate(LReLU);
        result = result*w_ho;
        result.Activate(tanh);
        return result;
    }

    private Matrix Biased(Matrix inputs) {
        int width = inputs.GetWidth();
        Matrix newInputs = new Matrix(width+1, 1);
        for (int i = 0; i < width; i++) {
            newInputs.Set(i, 0, inputs.Get(i, 0));
        }

        newInputs.Set(width, 0, 1);
        return newInputs;
    }

    public List<double> DNA() {
        List<double> dna = new List<double>();
        foreach (Matrix m in new Matrix[] {w_ih, w_ho}) {
            foreach (double weight in m) {
                dna.Add(weight);
            }
        }

        return dna;
    }

    public void SetWeights(List<double> dna) {
        int dnaIndex = 0;
        foreach (Matrix m in new Matrix[] {w_ih, w_ho}) {
            for (int y = 0; y < m.GetHeight(); y++) {
                for (int x = 0; x < m.GetWidth(); x++) {
                    m.Set(x, y, dna[dnaIndex++]);
                }
            }
        }
    }
}
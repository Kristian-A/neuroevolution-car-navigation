using System;
public class NeuralNetwork {

    private Matrix w_ih;
    private Matrix w_ho;
    private Func<double, double> activation = x => Math.Tanh(x);
    public NeuralNetwork(int inputs, int hidden, int outputs) {
        w_ih = new Matrix(hidden, inputs);
        w_ih.Randomize();
        w_ho = new Matrix(outputs, hidden);
        w_ho.Randomize();
    }

    public void SetWeights(Matrix ih, Matrix ho) {
        w_ih = ih;
        w_ho = ho;
    }
    public Matrix FeedForward(Matrix inputs) {
        Matrix result = inputs*w_ih;
        result.Activate(activation);
        result = result*w_ho;
        result.Activate(activation);
        return result;
    }
}
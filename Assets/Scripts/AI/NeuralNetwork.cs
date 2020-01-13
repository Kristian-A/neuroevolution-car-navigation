using System;
public class NeuralNetwork {

    private Matrix w_ih;
    private Matrix w_ho;
    private Func<double, double> activation = x => Math.Tanh(x);
    public NeuralNetwork(int inputs, int hidden, int outputs) {
        w_ih = new Matrix(hidden, inputs);
        w_ih.randomize();
        w_ho = new Matrix(outputs, hidden);
        w_ho.randomize();
    }

    public void setWeights(Matrix ih, Matrix ho) {
        w_ih = ih;
        w_ho = ho;
    }
    public Matrix feedforward(Matrix inputs) {
        Matrix result = inputs*w_ih;
        result.activate(activation);
        result = result*w_ho;
        result.activate(activation);
        return result;
    }
}
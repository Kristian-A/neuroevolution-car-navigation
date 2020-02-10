using System;
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

    public void SetWeights(Matrix ih, Matrix ho) {
        w_ih = ih;
        w_ho = ho;
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
}
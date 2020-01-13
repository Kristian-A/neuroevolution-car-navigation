using System;

public class Matrix {

    private double[][] numbers;
    private int width;
    private int height;
    
    public Matrix(int width, int height) {
        numbers = new double[height][];
        for (var y = 0; y < height; y++) {
            numbers[y] = new double[width];
        }

        this.width = width;
        this.height = height;
    }

    public void set(int x, int y, double value) {
        numbers[y][x] = value;
    }
    public double get(int x, int y) {
        return numbers[y][x];
    }

    public string print() {
        var ret = "";
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                ret += numbers[y][x];
                ret += " ";
            }
            ret += "\n";
        }
        return ret;
    }

    public Matrix transpolate() {
        var ret = new Matrix(height, width);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                ret.set(y, ret.height-x-1, get(x, y));
            }
        }
        return ret;
    }

    public void randomize(int seed = 0) {
        Random generator = new Random(seed);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                set(x, y, (float)generator.NextDouble());
            }
        }
    }
    public static Matrix operator* (Matrix a, Matrix b) {
        if (a.width != b.height) {
            return null;
        }
        Matrix res = new Matrix(b.width, a.height);
        b = b.transpolate();
        for (int ah = 0; ah < a.height; ah++) {
            for (int bh = b.height-1; bh >= 0; bh--) {
                double sum = 0;
                for (int i = 0; i < a.width; i++) {
                    sum += a.get(i, ah) * b.get(i, bh);
                }
                res.set(b.height-bh-1, ah, sum);
            }
        }
        return res;
    }

    public void activate(Func<double, double> activation) {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                set(x, y, activation(get(x, y)));
            }
        }
    }
}
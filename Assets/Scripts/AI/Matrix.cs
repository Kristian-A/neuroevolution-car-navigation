using System;

public class Matrix {

    private double[][] numbers;
    private int width;
    private int height;
        
    private static Random generator = new Random();
    
    public Matrix(int width, int height) {
        numbers = new double[height][];
        for (var y = 0; y < height; y++) {
            numbers[y] = new double[width];
        }

        this.width = width;
        this.height = height;
    }

    public void Set(int x, int y, double value) {
        numbers[y][x] = value;
    }
    public double Get(int x, int y) {
        return numbers[y][x];
    }

    public string Print() {
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

    public Matrix Transpolate() {
        var ret = new Matrix(height, width);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                ret.Set(y, ret.height-x-1, Get(x, y));
            }
        }
        return ret;
    }

    public void Randomize(int seed = 130) {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Set(x, y, ((double)Matrix.generator.NextDouble())*2 - 1);
            }
        }
    }
    public static Matrix operator* (Matrix a, Matrix b) {
        if (a.width != b.height) {
            return new Matrix(1, 1);
        }
        Matrix res = new Matrix(b.width, a.height);
        b = b.Transpolate();
        for (int ah = 0; ah < a.height; ah++) {
            for (int bh = b.height-1; bh >= 0; bh--) {
                double sum = 0;
                for (int i = 0; i < a.width; i++) {
                    sum += a.Get(i, ah) * b.Get(i, bh);
                }
                res.Set(b.height-bh-1, ah, sum);
            }
        }
        return res;
    }

    public void Activate(Func<double, double> activation) {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Set(x, y, activation(Get(x, y)));
            }
        }
    }

    public static void setSeed(int seed) {
        generator = new Random(seed);
    }
}
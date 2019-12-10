public class Matrix {
    private float[][] numbers;
    private int width;
    private int height;
    public Matrix(int width, int height) {
        numbers = new float[height][];
        for (var y = 0; y < height; y++) {
            numbers[y] = new float[width];
        }

        this.width = width;
        this.height = height;
    }

    public void set(int x, int y, float value) {
        numbers[y][x] = value;
    }
    public float get(int x, int y) {
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

    public static Matrix operator* (Matrix m1, Matrix m2) {
        if (m1.width != m2.height) {
            return null;
        }

        for (int i = 0; i < m1.width; i++) {
            for (int j = 0; j < m2.height; j++) {
                
            }
        }
    }
}
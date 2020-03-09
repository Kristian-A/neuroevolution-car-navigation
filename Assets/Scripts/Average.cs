class Average {

    private double sum = 0;
    private int count = 0;
    public void Add(double num) {
        sum += num;
        count++;
    } 

    public double Get() {
        if (count == 0) {
            return 0;
        }
        return sum / count;
    }

    public void Reset() {
        sum = 0;
        count = 0;
    }
}
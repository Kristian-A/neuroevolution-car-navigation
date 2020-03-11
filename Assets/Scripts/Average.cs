class Average {

    private float sum = 0;
    private int count = 0;
    public void Add(float num) {
        sum += num;
        count++;
    } 

    public float Get() {
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
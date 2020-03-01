using System;
using System.Diagnostics;
using System.Threading;
class Timer {
    private Stopwatch stopwatch = new Stopwatch();
    private int resetTime;
    private int prevTime;

    public Timer(int resetTime) {
        this.resetTime = resetTime;
        stopwatch.Start();
    }

    public bool IsElapsed() {
        int currTime = CurrentTime(); 
        if (currTime - prevTime >= resetTime) {
            prevTime = currTime;
            return true;
        }
        return false;
    }

    private int CurrentTime() {
        return stopwatch.Elapsed.Seconds * 1000 + stopwatch.Elapsed.Milliseconds; 
    }

    public void Reset() {
        prevTime = CurrentTime();
    }
}
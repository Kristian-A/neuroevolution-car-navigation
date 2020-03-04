using System;
using System.Diagnostics;
using System.Threading;

class Timer {
    private Stopwatch stopwatch = new Stopwatch();
    private int resetTime;
    private long prevTime;

    public Timer(int resetTime) {
        this.resetTime = resetTime;
        stopwatch.Start();
    }

    public bool IsElapsed() {
        long currTime = CurrentTime(); 
        if (currTime - prevTime >= resetTime) {
            prevTime = currTime;
            return true;
        }
        return false;
    }

    private long CurrentTime() {
        return ( (stopwatch.Elapsed.Hours * 1000 + 
                stopwatch.Elapsed.Minutes) * 1000 +
            stopwatch.Elapsed.Seconds) * 1000 +
            stopwatch.Elapsed.Milliseconds; 
    }

    public void Reset() {
        prevTime = CurrentTime();
    }
}
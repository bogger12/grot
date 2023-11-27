using System;

public class Timing {

    public class Timer {
        private DateTime startTime;
        public float timeElapsed;
        public void Start() {
            timeElapsed = 0;
            startTime = DateTime.Now;
        }
        public float Stop() {
            timeElapsed = (float)(DateTime.Now - startTime).TotalSeconds;
            return timeElapsed;
        }

    }
    public Timer DeltaTime;
    public Timer UpdateTime;
    public Timer DrawTime;
    public Timing() {
        DeltaTime = new Timer();
        UpdateTime = new Timer();
        DrawTime = new Timer();
    }
}
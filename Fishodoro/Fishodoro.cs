namespace Fishodoro
{
    public class Fishodoro
    {
        // Fields
        private System.Timers.Timer timer;
        private int time;   // in seconds
        private bool done;
        private bool isStudyTimer; // Tracks whether this is a study or break timer

        // Timer settings
        private int studyTime = 25; // Default study time in minutes
        private int breakTime = 5;  // Default break time in minutes

        // Properties
        /// <summary>
        /// Displays time remaining in mm:ss format
        /// </summary>
        public string Display
        {
            get => $"{time / 60:D2}:{time % 60:D2}"; // Ensures two-digit formatting
        }

        /// <summary>
        /// Returns whether the timer has finished
        /// </summary>
        public bool Done { get => done; }

        /// <summary>
        /// Returns whether the current timer is a study timer
        /// </summary>
        public bool IsStudyTimer { get => isStudyTimer; }

        // Constructors
        /// <summary>
        /// Creates a default timer (25m study or 5m break)
        /// </summary>
        /// <param name="study">True for study timer, false for break timer</param>
        public Fishodoro(bool study)
        {
            timer = new System.Timers.Timer(1000);
            isStudyTimer = study;
            ResetTimer(); // Initialize the timer with the correct time
            done = false;

            // Event hookup
            timer.Elapsed += Tick!;
        }

        // Methods
        /// <summary>
        /// Enables the timer
        /// </summary>
        public void Start()
        {
            timer.Enabled = true;
        }

        /// <summary>
        /// Disables the timer
        /// </summary>
        public void Pause()
        {
            timer.Enabled = false;
        }

        /// <summary>
        /// Restarts the timer with the current settings
        /// </summary>
        public void Restart()
        {
            ResetTimer();
            Start();
        }

        /// <summary>
        /// Updates the study and break time settings
        /// </summary>
        /// <param name="newStudyTime">New study time in minutes</param>
        /// <param name="newBreakTime">New break time in minutes</param>
        public void UpdateSettings(int newStudyTime, int newBreakTime)
        {
            studyTime = newStudyTime;
            breakTime = newBreakTime;
            ResetTimer(); // Reset the timer with the new settings
        }

        /// <summary>
        /// Resets the timer based on whether it's a study or break timer
        /// </summary>
        private void ResetTimer()
        {
            time = isStudyTimer ? studyTime * 60 : breakTime * 60;
            done = false;
        }

        // Events
        private void Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            // Decrement the time
            time--;

            // Stop the timer if it reaches 0
            if (time <= 0)
            {
                time = 0;
                Pause();
                done = true;

                // Switch to the other timer (study <-> break)
                isStudyTimer = !isStudyTimer;
                ResetTimer(); // Reset the timer with the new mode
                Start();      // Start the new timer
            }
        }
    }
}
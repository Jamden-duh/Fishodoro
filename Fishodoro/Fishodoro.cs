namespace Fishodoro
{
    public class Fishodoro
    {
        // fields
        private System.Timers.Timer timer;
        private int time;   // in seconds
        private bool done; 

        // properties
        /// <summary>
        /// displays time remaining mm:ss
        /// </summary>
        public string Display { get => $"{time / 60}: {time % 60}"; }
        /// <summary>
        /// returns bool whether or not timer has finished
        /// </summary>
        public bool Done { get => done; }

        // constructors
        /// <summary>
        /// creates default timer (25m study, 5m break)
        /// </summary>
        /// <param name="study"> true for study timer, false for break </param>
        public Fishodoro(bool study)
        {
            timer = new System.Timers.Timer(1000);

            if (study)
                time = 25 * 60; // study
            else
                time = 5 * 60;  // break

            done = false;

            // event hookup
            timer.Elapsed += Tick!;
        }
        /// <summary>
        /// custom timer
        /// </summary>
        /// <param name="minutes"> minutes timer should run for </param>
        public Fishodoro(int minutes)
        {
            timer = new System.Timers.Timer(1000);
            time = minutes * 60;
            done = false;

            // event hookup
            timer.Elapsed += Tick!;
        }

        // methods
        /// <summary>
        /// enables timer
        /// </summary>
        public void Start()
        {
            timer.Enabled = true;
        }
        /// <summary>
        /// disables timer
        /// </summary>
        public void Pause()
        {
            timer.Enabled = false;
        }

        // events
        private static void Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            Fishodoro t;
            if (source is Fishodoro)
            {
                t = (Fishodoro)source;
                
                // updates timer
                t.time--;

                // stops timer after it runs out
                if (t.time == 0)
                {
                    t.Pause();
                    t.done = true;
                }
            }    
        }
    }
}

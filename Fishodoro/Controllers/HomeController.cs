using System.Diagnostics;
using Fishodoro.Models;
using Microsoft.AspNetCore.Mvc;


namespace Fishodoro.Controllers
{
    public class HomeController : Controller
    { 
        private readonly ILogger<HomeController> _logger;
        private int coinAmount = 100;

        /// <summary>
        /// creates coinAmount as a read and write public property
        /// </summary>
        public int CoinAmount
        {
            get { return coinAmount; }
            set { coinAmount = value; }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public class TimerController : ControllerBase
        {
            private static Fishodoro timer = new Fishodoro(true); // Study timer as default

            // Get the current timer display (mm:ss)
            [HttpGet("get")]
            public IActionResult GetTimer()
            {
                return Ok(new { time = timer.Display });
            }

            // Start the timer
            [HttpPost("start")]
            public IActionResult StartTimer()
            {
                // Start the timer (if it's already finished, reset it)
                if (timer.Done)
                {
                    timer = new Fishodoro(true); // Restart with a new 25-minute study timer
                }
                timer.Start();  // Start the timer
                return Ok(new { message = "Timer started" });
            }

            // Pause the timer
            [HttpPost("pause")]
            public IActionResult PauseTimer()
            {
                timer.Pause();  // Pause the timer
                return Ok(new { message = "Timer paused" });
            }
        }
    }

    [ApiController]
    [Route("api/timer")]
    public class TimerController : ControllerBase
    {
        private static Fishodoro timer = new Fishodoro(true); // Study timer as default

        // Get the current timer display (mm:ss) and state (study/break)
        [HttpGet("get")]
        public IActionResult GetTimer()
        {
            return Ok(new
            {
                time = timer.Display,
                isStudyTimer = timer.IsStudyTimer // Include the current timer state
            });
        }

        // Start the timer
        [HttpPost("start")]
        public IActionResult StartTimer()
        {
            // Start the timer (if it's already finished, reset it)
            if (timer.Done)
            {
                timer = new Fishodoro(true); // Restart with a new study timer
            }
            timer.Start();  // Start the timer
            return Ok(new { message = "Timer started" });
        }

        // Pause the timer
        [HttpPost("pause")]
        public IActionResult PauseTimer()
        {
            timer.Pause();  // Pause the timer
            return Ok(new { message = "Timer paused" });
        }

        // Restart the timer
        [HttpPost("restart")]
        public IActionResult RestartTimer()
        {
            timer.Restart(); // Restart the timer
            return Ok(new { message = "Timer restarted" });
        }

        // Update timer settings
        [HttpPost("updateSettings")]
        public IActionResult UpdateTimerSettings([FromBody] TimerSettings settings)
        {
            timer.UpdateSettings(settings.StudyTime, settings.BreakTime);
            return Ok(new { message = "Timer settings updated" });
        }

        // Timer settings model
        public class TimerSettings
        {
            public int StudyTime { get; set; }
            public int BreakTime { get; set; }
        }
    }
}
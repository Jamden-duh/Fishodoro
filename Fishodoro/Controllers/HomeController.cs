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
        /// creates coinAmount as a read-only public int to be accessed by the user
        /// </summary>
        public int CoinAmount
        {
            get { return coinAmount; }
            //no set line: property is read-only
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
    }
}

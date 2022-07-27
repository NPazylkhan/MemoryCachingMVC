using MemoryCachingMVC.Models;
using MemoryCachingMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MemoryCachingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly UserService userService;

        public HomeController(ILogger<HomeController> logger, UserService service)
        {
            _logger = logger;
            userService = service;
        }

        public async Task<IActionResult> Index(int id)
        {
            User user = await userService.GetUser(id);

            if (user != null)
                return Content($"User: {user.Name}");

            return Content("User not found");
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
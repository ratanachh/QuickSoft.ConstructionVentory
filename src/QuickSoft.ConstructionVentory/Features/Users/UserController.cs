using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ConstructionVentory.Domain;

namespace QuickSoft.ConstructionVentory.Features.Users
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorView() {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
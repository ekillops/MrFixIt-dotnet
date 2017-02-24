using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;

namespace MrFixIt.Controllers
{
    public class HomeController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var thisWorker = db.Workers.FirstOrDefault(item => item.UserName == User.Identity.Name);
                return View(thisWorker);
            } else
            {
                return View();
            }
        }
    }
}

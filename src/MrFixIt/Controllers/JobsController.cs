using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

namespace MrFixIt.Controllers
{
    public class JobsController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        public IActionResult Index()
        {
            return View(db.Jobs.Include(i => i.Worker).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            db.Jobs.Add(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Claim(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        [ActionName("Claim")]
        public IActionResult ClaimJob(int jobId)
        {
            Job job = db.Jobs.Include(j => j.Worker).FirstOrDefault(j => j.JobId == jobId);
            job.Worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int jobId, string changeValue)
        {
            Job job = db.Jobs.FirstOrDefault(j => j.JobId == jobId);

            switch (changeValue)
            {
                case "pending":
                    job.MarkPending();
                    break;
                case "complete":
                    job.MarkCompleted();
                    break;
                default:
                    break;
            }

            db.Entry(job).State = EntityState.Modified;
            db.SaveChangesAsync();

            return View(job);
        }
    }
}

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        [HttpPost]
        public void Delete(int jobId)
        {
            Job job = db.Jobs.FirstOrDefault(j => j.JobId == jobId);
            db.Remove(job);
            db.SaveChanges();
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
            Worker applyingWorker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);

            if (applyingWorker.Available)
            {
                applyingWorker.Available = false;
                job.Worker = applyingWorker;
            }

            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int jobId, string changeValue)
        {
            List<Job> workerJobs = db.Jobs.Include(j => j.Worker).Where(j => j.JobId == jobId).ToList();

            if(changeValue == "pending")
            {
                foreach(Job job in workerJobs)
                {
                    if(job.JobId == jobId && job.Completed == false)
                    {
                        job.MarkPending();
                        job.Worker.Available = false;
                        db.Entry(job).State = EntityState.Modified;
                        db.Entry(job.Worker).State = EntityState.Modified;
                    }
                    else if (job.Completed == false)
                    {
                        job.MarkInactive();
                        db.Entry(job).State = EntityState.Modified;
                    }
                }
            }
            else if (changeValue == "complete")
            {
                foreach (Job job in workerJobs)
                {
                    if (job.JobId == jobId)
                    {
                        job.MarkCompleted();
                        job.Worker.Available = true;
                        db.Entry(job).State = EntityState.Modified;
                        db.Entry(job.Worker).State = EntityState.Modified;
                    }
                }
            }

            db.SaveChangesAsync();

            return View(workerJobs);
        }
    }
}

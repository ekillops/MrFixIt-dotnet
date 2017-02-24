using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MrFixIt.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; private set; }
        public bool Pending { get; private set; }
        public virtual Worker Worker { get; set; }

        public Worker FindWorker(string UserName)
        {
            Worker thisWorker = new MrFixItContext().Workers.FirstOrDefault(i => i.UserName == UserName);
            return thisWorker;
        }

        public void MarkCompleted()
        {
            Completed = true;
            Pending = false;
        }

        public void MarkPending()
        {
            Completed = false;
            Pending = true;
        }

        // Checks task completion status and returns bootstrap panel coloring based on results
        public string GetPanelClass()
        {
            if(Completed)
            {
                return "panel-default";
            }
            else if(Pending)
            {
                return "panel-primary";
            }
            else
            {
                return "panel-info";
            }
        }
    }
}

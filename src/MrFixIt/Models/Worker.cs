using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MrFixIt.Models
{
    public class Worker
    {
        [Key]
        public int WorkerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Avaliable { get; set; }
        public string UserName { get; set; }
        //this comes from Identity.User
        public virtual ICollection<Job> Jobs { get; set; }

        public Worker()
        {
            Avaliable = true;
        }

    }
}
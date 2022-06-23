using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Author { get; set; }
        public string Performer { get; set; }
        public DateTime Created { get; set; }
    }
}

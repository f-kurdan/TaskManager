using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Tag
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
    }
}

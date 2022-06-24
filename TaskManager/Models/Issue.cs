﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace TaskManager.Models
{//this class is called 'Issue' in order to avoid confusion with System.Threading.Tasks.Task class
    public class Issue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<Tag> Tags { get; set; }
        public string Author { get; set; }
        public string Performer { get; set; }
        public DateTime Created { get; set; }
    }
}

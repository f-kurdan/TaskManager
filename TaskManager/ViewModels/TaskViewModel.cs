﻿using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskViewModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Performer { get; set; }
        [Required]
        public DateTime Created { get; set; }

        public List<User> Performers { get; set; }

        public List<Tag> Tags { get; set; }


    }
}

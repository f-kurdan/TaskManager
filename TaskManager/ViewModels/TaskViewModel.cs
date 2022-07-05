using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskViewModel
    {
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{3,50}$",
            ErrorMessage = "Title must be from 3 to 20 characters long and contain latin letters and numbers")]
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public List<Status> Statuses { get; set; }

        public List<Models.Task> Tasks { get; set; }

        public List<string> Performers { get; set; }

        public IEnumerable<SelectListItem> TagSelectList { get; set; }

        public List<int> TagIDs { get; set; }

        public List<Tag> Tags { get; set; }
    }
}

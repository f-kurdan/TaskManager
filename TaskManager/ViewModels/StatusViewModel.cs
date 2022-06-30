using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class StatusViewModel
    {
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{3,50}$",
            ErrorMessage = "Title must be from 3 to 20 characters long and contain latin letters and numbers")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        public List<Status> Statuses { get; set; }
    }
}

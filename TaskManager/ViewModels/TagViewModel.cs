using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
	public class TagViewModel
	{
		public int ID { get; set; }

		[Required]
		[RegularExpression(@"^[a-zA-Z''-'\s]{3,50}$",
			ErrorMessage = "Title must be from 3 to 20 characters long and contain latin letters and numbers")]
		public string Title { get; set; }

		public DateTime? Created { get; set; }

		public List<Tag> Tags { get; set; }
	}
}

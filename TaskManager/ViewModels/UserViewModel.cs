using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.ViewModels
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public int FullName { get; set; }
        public int Email { get; set; }
        public DateTime Created { get; set; }
        public bool WantsToDeleteUser { get; set; }
        public List<IdentityUser> Users { get; set; }
        public bool TriedToDeleteUser { get; set; }
    }
}

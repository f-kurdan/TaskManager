using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public int? TaskID { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;

namespace Taskify.API.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}

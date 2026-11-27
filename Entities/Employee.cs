using System.ComponentModel.DataAnnotations;

namespace WebAPI_tutorial.Entities
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateOnly? DOB { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set; } = DateTime.Now;


    }
}

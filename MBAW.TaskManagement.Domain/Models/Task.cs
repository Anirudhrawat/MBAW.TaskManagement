using MBAW.TaskManagement.Infrastructure.Model;

namespace MBAW.TaskManagement.Domain.Models
{
    public class Task: BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }
}

using MBAW.TaskManagement.Domain.Models;

namespace MBAW.TaskManagement.Application.DTOs.Task
{
    public class UpdateTaskDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
    }
}

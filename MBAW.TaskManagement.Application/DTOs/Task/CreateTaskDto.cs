using MBAW.TaskManagement.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace MBAW.TaskManagement.Application.DTOs.Task
{
    public class CreateTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [FutureDate(ErrorMessage = "Due date cannot be in the past.")]
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Range(0, 2, ErrorMessage = "Value must be 0, 1, or 2.")]
        public int Priority { get; set; }
        [Range(0, 2, ErrorMessage = "Value must be 0, 1, or 2.")]
        public int Status { get; set; }
    }
}

using MBAW.TaskManagement.Infrastructure.Model;

namespace MBAW.TaskManagement.Domain.Models.StoredProcedures
{
    public class HighPriorityTasks: BaseModel
    {
        public int TaskCount { get; set; }
    }
}

using MBAW.TaskManagement.Application.DTOs.Task;

namespace MBAW.TaskManagement.Application.Services.Task
{
    public interface ITaskService
    {
        public Task<string> AddTask(CreateTaskDto createTaskDto);
        public Task<bool> UpdateTask(UpdateTaskDto updateTaskDto);
    }
}

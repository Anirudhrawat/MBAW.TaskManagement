using AutoMapper;
using MBAW.TaskManagement.Application.DTOs.Task;
using TaskModel = MBAW.TaskManagement.Domain.Models.Task;
namespace MBAW.TaskManagement.Mapping.Mappings
{
    public class TaskProfile: Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskDto, TaskModel>();
            CreateMap<UpdateTaskDto, TaskModel>();
        }
    }
}

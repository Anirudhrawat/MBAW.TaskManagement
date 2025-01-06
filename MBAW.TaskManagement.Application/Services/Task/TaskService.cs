using AutoMapper;
using MBAW.TaskManagement.Application.DTOs.Task;
using MBAW.TaskManagement.Application.Validation;
using MBAW.TaskManagement.Domain.Models.StoredProcedures;
using MBAW.TaskManagement.Infrastructure.Services.Execution;
using MBAW.TaskManagement.Shared;
using TaskModel = MBAW.TaskManagement.Domain.Models.Task;

namespace MBAW.TaskManagement.Application.Services.Task
{
    public class TaskService: ITaskService
    {
        private readonly IExecutionService _executionService;
        private readonly IMapper _mapper;

        public TaskService(IExecutionService executionService, IMapper mapper)
        {
            _executionService = executionService;
            _mapper = mapper;
        }

        private async Task<int> GetHighPriorityTasks(DateTime dueDate, string? id = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@DueDate", dueDate }
            };
            if(!string.IsNullOrEmpty(id))
                parameters.Add("@Id", id);
            var response = await _executionService.ExecuteStoredProcedureQueryAsync<HighPriorityTasks>(Constants.StoredProcedures.GET_HIGH_PRIORITY_TASK, parameters);
            var highPriorityTasks = response.FirstOrDefault();
            if (highPriorityTasks != null)
                return highPriorityTasks.TaskCount;
            throw new Exception(Constants.ValidationMessages.EXCEPTION_EXECUTING_QUERY);

        }

        public async Task<string> AddTask(CreateTaskDto createTaskDto)
        {
            if (createTaskDto.DueDate < DateTime.Now)
                throw new Exception(Constants.ValidationMessages.PAST_DUE_DATE);
           
            if (!HolidayValidation.IsValidDueDate(createTaskDto.DueDate))
                throw new Exception(Constants.ValidationMessages.INVALID_DUE_DATE);

            var highPriorityTasks = await GetHighPriorityTasks(createTaskDto.DueDate);
            if (highPriorityTasks >= 100)
                throw new Exception(Constants.ValidationMessages.DUE_TASKS_THRESHOLD);
            var task = _mapper.Map<TaskModel>(createTaskDto);
            task.Id = Guid.NewGuid().ToString();

            var parameters = new Dictionary<string, object>()
            {
                { "@Id", task.Id },
                { "@Name", task.Name },
                { "@Description", task.Description },
                { "@DueDate", task.DueDate },
                { "@StartDate", task.StartDate },
                { "@EndDate", task.EndDate },
                { "@Priority", task.Priority },
                { "@Status", task.Status }
            };
            await _executionService.ExecuteStoredProcedureNonQueryAsync(Constants.StoredProcedures.INSERT_TASK, parameters);
            return task.Id;
        }

        public async Task<bool> UpdateTask(UpdateTaskDto updateTaskDto)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@Id", updateTaskDto.Id }
            };
            var response = await _executionService.ExecuteStoredProcedureQueryAsync<TaskModel>(Constants.StoredProcedures.GET_BY_ID, parameters);
            var existingTask = response.FirstOrDefault();
            if (existingTask == null)
                throw new Exception(Constants.ValidationMessages.NO_TASK_FOUND_WITH_ID);
            if (existingTask.DueDate != updateTaskDto.DueDate)
                if (updateTaskDto.DueDate < DateTime.Now)
                    throw new Exception(Constants.ValidationMessages.PAST_DUE_DATE);
                if (!HolidayValidation.IsValidDueDate(updateTaskDto.DueDate))
                    throw new Exception(Constants.ValidationMessages.INVALID_DUE_DATE);

            var highPriorityTasks = await GetHighPriorityTasks(updateTaskDto.DueDate, updateTaskDto.Id);
            if (highPriorityTasks >= 100)
                throw new Exception(Constants.ValidationMessages.DUE_TASKS_THRESHOLD);
            var task = _mapper.Map<TaskModel>(updateTaskDto);

            parameters = new Dictionary<string, object>()
            {
                { "@Id", task.Id },
                { "@Name", task.Name },
                { "@Description", task.Description },
                { "@DueDate", task.DueDate },
                { "@StartDate", task.StartDate },
                { "@EndDate", task.EndDate },
                { "@Priority", task.Priority },
                { "@Status", task.Status }
            };
            await _executionService.ExecuteStoredProcedureNonQueryAsync(Constants.StoredProcedures.UPDATE_TASK, parameters);
            return true;
        }


    }
}

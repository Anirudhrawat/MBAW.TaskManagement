using AutoMapper;
using Moq;
using MBAW.TaskManagement.Application.DTOs.Task;
using MBAW.TaskManagement.Application.Services.Task;
using MBAW.TaskManagement.Infrastructure.Services.Execution;
using MBAW.TaskManagement.Domain.Models.StoredProcedures;
using MBAW.TaskManagement.Shared;
using MBAW.TaskManagement.Domain.Models;
using MBAW.TaskManagement.Infrastructure.Model;

namespace MBAW.TaskManagement.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<IExecutionService> _mockExecutionService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockExecutionService = new Mock<IExecutionService>();
            _mockMapper = new Mock<IMapper>();
            _taskService = new TaskService(_mockExecutionService.Object, _mockMapper.Object);
        }

        [Fact]
        public async void AddTask_ShouldThrowDueDateException_WhenDueDateIsInThePast()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Name = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(-1), // Past date
                StartDate = DateTime.Now,
                Priority = 0, // High priority
                Status = 0 // New
            };

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _taskService.AddTask(createTaskDto));

            // Assert
            Assert.Equal(Constants.ValidationMessages.PAST_DUE_DATE, exception.Message);
        }

        [Fact]
        public async void AddTask_ShouldThrowException_WhenDueDateIsHolidayOrWeekend()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Name = "Test Task",
                Description = "Test Description",
                DueDate = new DateTime(2027, 11, 11), // Example holiday
                StartDate = DateTime.Now,
                Priority = 0, // High priority
                Status = 0 // New
            };


            
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.AddTask(createTaskDto));

            // Assert
            Assert.Equal(Constants.ValidationMessages.INVALID_DUE_DATE, exception.Message);
        }

        [Fact]
        public async void AddTask_ShouldThrowException_WhenHighPriorityTaskLimitExceeded()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Name = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1), // Valid due date
                StartDate = DateTime.Now,
                Priority = 0, // High priority
                Status = 0 // New
            };

            _mockExecutionService
                .Setup(s => s.ExecuteStoredProcedureQueryAsync<HighPriorityTasks>(
                    Constants.StoredProcedures.GET_HIGH_PRIORITY_TASK, It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<HighPriorityTasks> { new HighPriorityTasks { TaskCount = 100 } }); // Mock 100 tasks

            
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.AddTask(createTaskDto));

            // Assert
            Assert.Equal(Constants.ValidationMessages.DUE_TASKS_THRESHOLD, exception.Message);
        }

        [Fact]
        public async void UpdateTask_ShouldThrowException_WhenDueDateIsInThePast()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDto
            {
                Id = "123",
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(-1), // Invalid due date
                StartDate = DateTime.Now,
                Priority = 0,
                Status = 0
            };

            var existingTask = new Domain.Models.Task
            {
                Id = "123",
                Name = "Existing Task",
                Description = "Existing Description",
                DueDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now,
                Priority = Priority.HIGH,
                Status = Status.NEW
            };

            _mockExecutionService
                .Setup(e => e.ExecuteStoredProcedureQueryAsync<Domain.Models.Task>(
                    Constants.StoredProcedures.GET_BY_ID,
                    It.Is<Dictionary<string, object>>(d => (string)d["@Id"] == "123")))
                .ReturnsAsync(new List<Domain.Models.Task> { existingTask }); // Mock database response

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.UpdateTask(updateTaskDto));
            Assert.Equal(Constants.ValidationMessages.PAST_DUE_DATE, exception.Message);
        }


        [Fact]
        public async void UpdateTask_ShouldThrowException_WhenDueDateIsHolidayOrWeekend()
        {
            
            // Arrange
            var updateTaskDto = new UpdateTaskDto
            {
                Id = "123",
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = new DateTime(2025, 11, 11), // Invalid due date
                StartDate = DateTime.Now,
                Priority = 0,
                Status = 0
            };

            var existingTask = new Domain.Models.Task
            {
                Id = "123",
                Name = "Existing Task",
                Description = "Existing Description",
                DueDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now,
                Priority = Priority.HIGH,
                Status = Status.NEW
            };

            _mockExecutionService
                .Setup(e => e.ExecuteStoredProcedureQueryAsync<Domain.Models.Task>(
                    Constants.StoredProcedures.GET_BY_ID,
                    It.Is<Dictionary<string, object>>(d => (string)d["@Id"] == "123")))
                .ReturnsAsync(new List<Domain.Models.Task> { existingTask }); // Mock database response

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.UpdateTask(updateTaskDto));
            Assert.Equal(Constants.ValidationMessages.INVALID_DUE_DATE, exception.Message);
        }

        [Fact]
        public async void UpdateTask_ShouldThrowException_WhenHighPriorityTaskLimitExceeded()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDto
            {
                Id = "123",
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now,
                Priority = 0,
                Status = 1
            };
            var existingTask = new Domain.Models.Task
            {
                Id = "123",
                Name = "Existing Task",
                Description = "Existing Description",
                DueDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now,
                Priority = Priority.HIGH,
                Status = Status.NEW
            };

            _mockExecutionService
                .Setup(e => e.ExecuteStoredProcedureQueryAsync<Domain.Models.Task>(
                    Constants.StoredProcedures.GET_BY_ID,
                    It.Is<Dictionary<string, object>>(d => (string)d["@Id"] == "123")))
                .ReturnsAsync(new List<Domain.Models.Task> { existingTask }); // Mock database response

            _mockExecutionService
                .Setup(s => s.ExecuteStoredProcedureQueryAsync<HighPriorityTasks>(
                    Constants.StoredProcedures.GET_HIGH_PRIORITY_TASK, It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<HighPriorityTasks> { new HighPriorityTasks { TaskCount = 100 } }); // Mock 100 tasks

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.UpdateTask(updateTaskDto));
            Assert.Equal(Constants.ValidationMessages.DUE_TASKS_THRESHOLD, exception.Message);
        }

        [Fact]
        public async void UpdateTask_ShouldUpdateTaskSuccessfully_WhenAllConditionsAreMet()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDto
            {
                Id = "123",
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = new DateTime(2027, 01, 08),
                StartDate = new DateTime(2027, 01, 07),
                Priority = 1,
                Status = 1
            };

            var existingTask = new Domain.Models.Task
            {
                Id = "123",
                Name = "Existing Task",
                Description = "Existing Description",
                DueDate = DateTime.Now.AddDays(1),
                StartDate = DateTime.Now,
                Priority = Priority.HIGH,
                Status = Status.NEW
            };

            var mappedTask = new Domain.Models.Task
            {
                Id = updateTaskDto.Id,
                Name = updateTaskDto.Name,
                Description = updateTaskDto.Description,
                DueDate = updateTaskDto.DueDate,
                StartDate = updateTaskDto.StartDate,
                EndDate = updateTaskDto.EndDate,
                Priority = (Priority)updateTaskDto.Priority,
                Status = (Status)updateTaskDto.Status
            };

            _mockExecutionService
                .Setup(s => s.ExecuteStoredProcedureQueryAsync<HighPriorityTasks>(
                    Constants.StoredProcedures.GET_HIGH_PRIORITY_TASK, It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<HighPriorityTasks> { new HighPriorityTasks { TaskCount = 90 } }); // Mock 90 tasks

            _mockExecutionService
                .Setup(e => e.ExecuteStoredProcedureQueryAsync<Domain.Models.Task>(
                    Constants.StoredProcedures.GET_BY_ID,
                    It.Is<Dictionary<string, object>>(d => (string)d["@Id"] == "123")))
                .ReturnsAsync(new List<Domain.Models.Task> { existingTask }); // Mock database response

            _mockMapper
                .Setup(m => m.Map<Domain.Models.Task>(updateTaskDto))
                .Returns(mappedTask); // Mock mapper to return a mapped TaskModel

            // Act
            await _taskService.UpdateTask(updateTaskDto);

            // Assert
            _mockExecutionService.Verify(e => e.ExecuteStoredProcedureNonQueryAsync(Constants.StoredProcedures.UPDATE_TASK,
                It.Is<Dictionary<string, object>>(d =>
                    (string)d["@Id"] == updateTaskDto.Id &&
                    (string)d["@Name"] == updateTaskDto.Name &&
                    (string)d["@Description"] == updateTaskDto.Description &&
                    (DateTime)d["@DueDate"] == updateTaskDto.DueDate &&
                    (DateTime)d["@StartDate"] == updateTaskDto.StartDate &&
                    (DateTime?)d["@EndDate"] == updateTaskDto.EndDate &&
                    (int)d["@Priority"] == (int)updateTaskDto.Priority &&
                    (int)d["@Status"] == (int)updateTaskDto.Status)),
                Times.Once);
        }


    }
}

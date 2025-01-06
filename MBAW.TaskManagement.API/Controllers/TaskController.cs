using MBAW.TaskManagement.Application.DTOs.Task;
using MBAW.TaskManagement.Application.Services.Task;
using MBAW.TaskManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace MBAW.TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Response<string>>> Create(CreateTaskDto createTaskDto)
        {
            return Ok(new Response<string>(await _service.AddTask(createTaskDto)));
        }

        [HttpPut]
        public async Task<ActionResult<Response<bool>>> Update(UpdateTaskDto updateTaskDto)
        {
            return Ok(new Response<bool>(await _service.UpdateTask(updateTaskDto)));
        }
    }
}

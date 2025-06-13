using Clarity.Api.Contracts.AppTasks;
using Clarity.Api.Extensions;
using Clarity.Application.Abstractions;
using Clarity.Application.AppTasks.Create;
using Clarity.Application.AppTasks.Delete;
using Clarity.Application.AppTasks.GetAll;
using Clarity.Application.AppTasks.GetById;
using Clarity.Application.AppTasks.Update;
using Clarity.Core.Enums;
using Clarity.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppTasksController: ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AppTasksController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(List<AppTaskResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppTaskResponse>> GetAppTasks(CancellationToken cancellationToken = default)
    {
        var getAllQuery = new GetAllTasksQuery(this.GetCurrentUserId());
        var getResult = await _queryDispatcher.DispatchAsync<GetAllTasksQuery, List<AppTask>>(
            getAllQuery,
            cancellationToken);
        if (!getResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(getResult.Errors);
            return BadRequest(problemDetails);
        }

        var responseTasks = getResult.Value
            .Select(at => new AppTaskResponse(
                at.Id, at.Title,
                at.Description, at.CreationDate,
                at.Deadline, at.Tags, 
                at.Status))
            .ToList();

        return Ok(responseTasks);
    }
    
    
    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppTaskResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppTaskResponse>> GetAppTask(Guid id,
        CancellationToken cancellationToken = default)
    {
        var getByIdQuery = new GetTaskByIdQuery(id);
        var getResult = await _queryDispatcher.DispatchAsync<GetTaskByIdQuery, AppTask>(
            getByIdQuery,
            cancellationToken);
        if (!getResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(getResult.Errors);
            return BadRequest(problemDetails);
        }

        var responseTask = new AppTaskResponse(
            getResult.Value.Id, getResult.Value.Title,
            getResult.Value.Description, getResult.Value.CreationDate,
            getResult.Value.Deadline, getResult.Value.Tags,
            getResult.Value.Status);

        return Ok(responseTask);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateTask(AppTaskCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateTaskCommand(
            this.GetCurrentUserId(), request.Title,
            request.Description, DateTime.Now,
            request.Deadline, [],
            AppTaskStatus.NotStarted);
        var createResult = await _commandDispatcher.DispatchAsync<CreateTaskCommand, Guid>(
            createCommand,
            cancellationToken);
        if (!createResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(createResult.Errors);
            return BadRequest(problemDetails);
        }
    
        return CreatedAtAction(nameof(GetAppTask), new { id = createResult.Value }, createResult.Value);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateTask(
        Guid id,
        AppTaskUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return BadRequest("Task id cannot be empty");

        var updateCommand = new UpdateTaskCommand(
            id, this.GetCurrentUserId(),
            new AppTaskUpdate(request.Title,
                request.Description,
                request.Deadline,
                request.Status));
        var updateResult = await _commandDispatcher.DispatchAsync(
            updateCommand,
            cancellationToken);
        if (!updateResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(updateResult.Errors);
            return BadRequest(problemDetails);
        }

        return Ok();
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteTask(Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return BadRequest("Task id cannot be empty");

        var deleteCommand = new DeleteTaskCommand(id);
        var deleteResult = await _commandDispatcher.DispatchAsync(
            deleteCommand,
            cancellationToken);
        if (!deleteResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(deleteResult.Errors);
            return BadRequest(problemDetails);
        }
        
        return NoContent();
    }
}
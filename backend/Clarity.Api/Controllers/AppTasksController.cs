using AutoMapper;
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
using Clarity.Infrastructure.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppTasksController: ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMapper _mapper;

    public AppTasksController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IMapper mapper)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _mapper = mapper;
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

        return Ok(_mapper.Map<List<AppTaskResponse>>(getResult.Value));
    }
    
    
    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppTaskResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppTaskResponse>> GetAppTask(Guid id,
        CancellationToken cancellationToken = default)
    {
        var getByIdQuery = new GetTaskByIdQuery(id, this.GetCurrentUserId());
        var getResult = await _queryDispatcher.DispatchAsync<GetTaskByIdQuery, AppTask>(
            getByIdQuery,
            cancellationToken);
        if (!getResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(getResult.Errors);
            return BadRequest(problemDetails);
        }

        return Ok(_mapper.Map<AppTaskResponse>(getResult.Value));
    }

    [Authorize]
    [HttpPost]
    [ValidateModel<AppTaskCreateRequest>]
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
    [ValidateModel<AppTaskUpdateRequest>]
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

        var deleteCommand = new DeleteTaskCommand(id, this.GetCurrentUserId());
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
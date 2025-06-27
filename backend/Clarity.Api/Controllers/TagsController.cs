using Asp.Versioning;
using AutoMapper;
using Clarity.Api.Contracts.Tags;
using Clarity.Api.Extensions;
using Clarity.Application.Abstractions;
using Clarity.Application.Tags.Create;
using Clarity.Application.Tags.Delete;
using Clarity.Application.Tags.GetAllByTask;
using Clarity.Core.Models;
using Clarity.Infrastructure.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TagsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMapper _mapper;

    public TagsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IMapper mapper)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(List<TagResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TagResponse>> GetTags(Guid appTaskId, CancellationToken cancellationToken = default)
    {
        var getAllQuery = new GetAllTagsByTaskQuery(appTaskId, this.GetCurrentUserId());
        var getResult = await _queryDispatcher.DispatchAsync<GetAllTagsByTaskQuery, List<Tag>>(
            getAllQuery,
            cancellationToken);
        if (!getResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(getResult.Errors));

        return Ok(_mapper.Map<List<TagResponse>>(getResult.Value));
    }
    
    [Authorize]
    [HttpPost]
    [ValidateModel<TagCreateRequest>]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create(TagCreateRequest request, CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateTagCommand(
            request.Name,
            request.AppTaskId,
            this.GetCurrentUserId());
        var createResult = await _commandDispatcher.DispatchAsync<CreateTagCommand, int>(
            createCommand,
            cancellationToken);
        if (!createResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(createResult.Errors));

        return Created("", new {id = createResult.Value});
    }
    
    [Authorize]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        if (id == 0)
            return BadRequest("Tag id cannot be empty");

        var deleteCommand = new DeleteTagCommand(id);
        var deleteResult = await _commandDispatcher.DispatchAsync<DeleteTagCommand>(
            deleteCommand,
            cancellationToken);
        if (!deleteResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(deleteResult.Errors));

        return NoContent();
    }
}
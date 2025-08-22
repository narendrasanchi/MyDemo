using Application.SelectionTypes.Commands;
using Application.SelectionTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/selection-types")]
public class SelectionTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SelectionTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<GetSelectionTypesQueryResult>> Get()
    {
        var result = await _mediator.Send(new GetSelectionTypesQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProcessSelectionTypesCommandResult>> Post([FromBody] string[] selectedValues)
    {
        var command = new ProcessSelectionTypesCommand { SelectedValues = selectedValues };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
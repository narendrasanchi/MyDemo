using MediatR;

namespace Application.SelectionTypes.Commands;

public class ProcessSelectionTypesCommand : IRequest<ProcessSelectionTypesCommandResult>
{
    public string[] SelectedValues { get; set; } = Array.Empty<string>();
}
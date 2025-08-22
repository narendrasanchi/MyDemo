using Domain.Enums;
using MediatR;

namespace Application.SelectionTypes.Commands;

public class ProcessSelectionTypesCommandHandler : IRequestHandler<ProcessSelectionTypesCommand, ProcessSelectionTypesCommandResult>
{
    public Task<ProcessSelectionTypesCommandResult> Handle(ProcessSelectionTypesCommand request, CancellationToken cancellationToken)
    {
        // Validate the selected values
        var validEnumValues = Enum.GetNames(typeof(SelectionType));
        var invalidValues = request.SelectedValues.Where(v => !validEnumValues.Contains(v)).ToArray();

        if (invalidValues.Any())
        {
            return Task.FromResult(new ProcessSelectionTypesCommandResult
            {
                IsSuccess = false,
                Message = $"Invalid selection values: {string.Join(", ", invalidValues)}"
            });
        }

        // Process the selection (for now, just return success)
        return Task.FromResult(new ProcessSelectionTypesCommandResult
        {
            IsSuccess = true,
            Message = $"Successfully processed selection: {string.Join(", ", request.SelectedValues)}"
        });
    }
}
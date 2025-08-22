using Domain.Enums;
using MediatR;

namespace Application.SelectionTypes.Queries;

public class GetSelectionTypesQueryHandler : IRequestHandler<GetSelectionTypesQuery, GetSelectionTypesQueryResult>
{
    public Task<GetSelectionTypesQueryResult> Handle(GetSelectionTypesQuery request, CancellationToken cancellationToken)
    {
        var enumValues = Enum.GetNames(typeof(SelectionType));
        
        var result = new GetSelectionTypesQueryResult
        {
            Items = new ItemsSchema
            {
                Enum = enumValues
            }
        };

        return Task.FromResult(result);
    }
}
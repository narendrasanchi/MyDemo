namespace Application.SelectionTypes.Queries;

public class GetSelectionTypesQueryResult
{
    public string Type { get; set; } = "array";
    public ItemsSchema Items { get; set; } = new();
}

public class ItemsSchema
{
    public string Type { get; set; } = "string";
    public string[] Enum { get; set; } = Array.Empty<string>();
}
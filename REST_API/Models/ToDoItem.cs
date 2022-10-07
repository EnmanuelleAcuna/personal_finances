namespace REST_API.Models;

public class ToDoItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public string Secret { get; set; } = string.Empty;
}
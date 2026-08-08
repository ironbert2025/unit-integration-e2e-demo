namespace SimpleTaskApp.Core;

// Represents a task. Simple model, no external dependencies.
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; }
}

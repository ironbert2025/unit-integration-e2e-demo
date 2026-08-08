namespace SimpleTaskApp.Core;

// Representa una tarea. Modelo simple, sin dependencias externas.
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; }
}

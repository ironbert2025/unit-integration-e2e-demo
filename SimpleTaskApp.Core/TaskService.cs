namespace SimpleTaskApp.Core;

// Pure business logic: validation rules + orchestration.
// Knows nothing about SQLite, WinForms, or how data is stored.
public class TaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public TaskItem AddTask(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (title.Length > 100)
            throw new ArgumentException("Title cannot exceed 100 characters.");

        var task = new TaskItem { Title = title.Trim(), IsDone = false };
        return _repository.Save(task);
    }

    public List<TaskItem> GetAll() => _repository.GetAll();
}

namespace SimpleTaskApp.Core;

// Logica de negocio pura: reglas de validacion + orquestacion.
// No sabe nada de SQLite, WinForms, ni de como se guardan los datos.
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
            throw new ArgumentException("El titulo no puede estar vacio.");

        if (title.Length > 100)
            throw new ArgumentException("El titulo no puede superar 100 caracteres.");

        var task = new TaskItem { Title = title.Trim(), IsDone = false };
        return _repository.Save(task);
    }

    public List<TaskItem> GetAll() => _repository.GetAll();
}

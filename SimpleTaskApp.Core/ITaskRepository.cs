namespace SimpleTaskApp.Core;

// Persistence contract. The business logic (TaskService) depends on
// this interface, NOT on a concrete database. This is what allows
// unit tests to use a mock/fake instead of real SQLite.
public interface ITaskRepository
{
    TaskItem Save(TaskItem item);
    List<TaskItem> GetAll();
}

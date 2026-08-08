namespace SimpleTaskApp.Core;

// Contrato de persistencia. La logica de negocio (TaskService) depende de
// esta interfaz, NO de una base de datos concreta. Esto es lo que permite
// que los tests unitarios usen un mock/fake en vez de SQLite real.
public interface ITaskRepository
{
    TaskItem Save(TaskItem item);
    List<TaskItem> GetAll();
}

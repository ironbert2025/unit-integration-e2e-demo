using SimpleTaskApp.Core;
using SimpleTaskApp.Data;
using Xunit;

namespace SimpleTaskApp.IntegrationTests;

// TEST DE INTEGRACION: prueba TaskService + SqliteTaskRepository JUNTOS,
// contra una base de datos SQLite REAL (en memoria, pero real: SQL de verdad,
// tabla de verdad, roundtrip de verdad). Aqui SI detectas errores de que
// tu SQL este mal escrito o que el mapeo de columnas no coincida.
public class SqliteTaskRepositoryTests : IDisposable
{
    private readonly string _connectionString;

    public SqliteTaskRepositoryTests()
    {
        // Cada test usa su propia BD SQLite en memoria, aislada de los demas
        _connectionString = $"Data Source=file:{Guid.NewGuid()}?mode=memory&cache=shared";
    }

    [Fact]
    public void AddTask_GuardaYRecupera_DesdeSqliteReal()
    {
        var repository = new SqliteTaskRepository(_connectionString);
        var service = new TaskService(repository);

        service.AddTask("Comprar leche");
        service.AddTask("Revisar TradeSignal");

        var tareas = service.GetAll();

        Assert.Equal(2, tareas.Count);
        Assert.Contains(tareas, t => t.Title == "Comprar leche");
        Assert.Contains(tareas, t => t.Title == "Revisar TradeSignal");
        // El Id lo asigna SQLite (AUTOINCREMENT) -- si esto es > 0, el
        // roundtrip real con la BD funciono de principio a fin.
        Assert.All(tareas, t => Assert.True(t.Id > 0));
    }

    [Fact]
    public void GetAll_ConBaseDeDatosVacia_RetornaListaVacia()
    {
        var repository = new SqliteTaskRepository(_connectionString);

        var tareas = repository.GetAll();

        Assert.Empty(tareas);
    }

    public void Dispose() { /* la BD en memoria muere sola al cerrar la conexion */ }
}

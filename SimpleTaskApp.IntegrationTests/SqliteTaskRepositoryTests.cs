using SimpleTaskApp.Core;
using SimpleTaskApp.Data;
using Xunit;

namespace SimpleTaskApp.IntegrationTests;

// INTEGRATION TEST: tests TaskService + SqliteTaskRepository TOGETHER,
// against a REAL SQLite database (in memory, but real: real SQL,
// real table, real roundtrip). This IS where you catch errors from
// badly written SQL or mismatched column mapping.
public class SqliteTaskRepositoryTests : IDisposable
{
    private readonly string _connectionString;

    public SqliteTaskRepositoryTests()
    {
        // Each test uses its own in-memory SQLite DB, isolated from the others
        _connectionString = $"Data Source=file:{Guid.NewGuid()}?mode=memory&cache=shared";
    }

    [Fact]
    public void AddTask_SavesAndRetrieves_FromRealSqlite()
    {
        var repository = new SqliteTaskRepository(_connectionString);
        var service = new TaskService(repository);

        service.AddTask("Buy milk");
        service.AddTask("Review TradeSignal");

        var tasks = service.GetAll();

        Assert.Equal(2, tasks.Count);
        Assert.Contains(tasks, t => t.Title == "Buy milk");
        Assert.Contains(tasks, t => t.Title == "Review TradeSignal");
        // The Id is assigned by SQLite (AUTOINCREMENT) -- if this is > 0, the
        // real roundtrip with the DB worked end to end.
        Assert.All(tasks, t => Assert.True(t.Id > 0));
    }

    [Fact]
    public void GetAll_WithEmptyDatabase_ReturnsEmptyList()
    {
        var repository = new SqliteTaskRepository(_connectionString);

        var tasks = repository.GetAll();

        Assert.Empty(tasks);
    }

    public void Dispose() { /* the in-memory DB dies on its own when the connection closes */ }
}

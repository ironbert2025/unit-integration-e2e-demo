using Microsoft.Data.Sqlite;
using SimpleTaskApp.Core;

namespace SimpleTaskApp.Data;

// REAL implementation of ITaskRepository using SQLite.
// This is the piece a unit test NEVER touches (it gets mocked),
// but an INTEGRATION test does exercise for real.
public class SqliteTaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public SqliteTaskRepository(string connectionString)
    {
        _connectionString = connectionString;
        CreateTableIfNotExists();
    }

    private void CreateTableIfNotExists()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Tasks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                IsDone INTEGER NOT NULL DEFAULT 0
            );
            """;
        cmd.ExecuteNonQuery();
    }

    public TaskItem Save(TaskItem item)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Tasks (Title, IsDone) VALUES ($title, $isDone); SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$title", item.Title);
        cmd.Parameters.AddWithValue("$isDone", item.IsDone ? 1 : 0);
        item.Id = Convert.ToInt32((long)cmd.ExecuteScalar()!);
        return item;
    }

    public List<TaskItem> GetAll()
    {
        var result = new List<TaskItem>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Title, IsDone FROM Tasks ORDER BY Id;";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new TaskItem
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                IsDone = reader.GetInt32(2) == 1
            });
        }
        return result;
    }
}

using SimpleTaskApp.Data;

namespace SimpleTaskApp.App;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Real database in the user's folder. For production you'd replace
        // this with your real connection string (or dependency injection).
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SimpleTaskApp", "tasks.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        var repository = new SqliteTaskRepository($"Data Source={dbPath}");
        var service = new Core.TaskService(repository);

        Application.Run(new MainForm(service));
    }
}

using SimpleTaskApp.Data;

namespace SimpleTaskApp.App;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // BD real en la carpeta del usuario. Para produccion cambiarias
        // esto por tu cadena de conexion real (o inyeccion de dependencias).
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SimpleTaskApp", "tasks.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        var repository = new SqliteTaskRepository($"Data Source={dbPath}");
        var service = new Core.TaskService(repository);

        Application.Run(new MainForm(service));
    }
}

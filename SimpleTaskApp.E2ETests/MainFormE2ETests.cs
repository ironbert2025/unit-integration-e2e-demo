using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Xunit;

namespace SimpleTaskApp.E2ETests;

// TEST END-TO-END: NO llama codigo C# directamente. Abre el .exe REAL,
// como lo abririas tu, y simula clics de un usuario real sobre la ventana.
// Es el mas lento y el mas fragil (depende de que la UI no cambie), pero
// es el UNICO que prueba que la app completa funciona de punta a punta,
// tal como la usara la persona final.
//
// IMPORTANTE: antes de correr esto hay que compilar SimpleTaskApp.App
// (dotnet build) y ajustar RutaExe abajo a la ruta del .exe generado.
public class MainFormE2ETests : IDisposable
{
    private const string RutaExe = @"..\..\..\..\SimpleTaskApp.App\bin\Debug\net8.0-windows\SimpleTaskApp.App.exe";

    private readonly Application _app;
    private readonly UIA3Automation _automation;
    private readonly Window _ventana;

    public MainFormE2ETests()
    {
        _app = Application.Launch(RutaExe);
        _automation = new UIA3Automation();
        _ventana = _app.GetMainWindow(_automation);
    }

    [Fact]
    public void AgregarTarea_ConTituloValido_ApareceEnLaLista()
    {
        var txtTitle = _ventana.FindFirstDescendant(cf => cf.ByAutomationId("txtTitle")).AsTextBox();
        var btnAdd = _ventana.FindFirstDescendant(cf => cf.ByAutomationId("btnAdd")).AsButton();
        var lstTasks = _ventana.FindFirstDescendant(cf => cf.ByAutomationId("lstTasks")).AsListBox();

        txtTitle.Enter("Comprar leche");
        btnAdd.Click();

        Assert.Contains(lstTasks.Items, item => item.Text == "Comprar leche");
    }

    [Fact]
    public void AgregarTarea_ConTituloVacio_MuestraError()
    {
        var btnAdd = _ventana.FindFirstDescendant(cf => cf.ByAutomationId("btnAdd")).AsButton();
        var lblError = _ventana.FindFirstDescendant(cf => cf.ByAutomationId("lblError")).AsLabel();

        btnAdd.Click(); // sin escribir nada en txtTitle

        Assert.False(string.IsNullOrEmpty(lblError.Text));
    }

    public void Dispose()
    {
        _app.Close();
        _automation.Dispose();
    }
}

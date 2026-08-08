using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Xunit;

namespace SimpleTaskApp.E2ETests;

// END-TO-END TEST: does NOT call C# code directly. Opens the REAL .exe,
// just like you would yourself, and simulates a real user's clicks on the
// window. It's the slowest and most fragile (depends on the UI not
// changing), but it's the ONLY one that proves the whole app works
// end to end, exactly as the end user will use it.
//
// IMPORTANT: before running this you need to build SimpleTaskApp.App
// (dotnet build) and adjust ExePath below to the generated .exe's path.
public class MainFormE2ETests : IDisposable
{
    private const string ExePath = @"..\..\..\..\SimpleTaskApp.App\bin\Debug\net8.0-windows\SimpleTaskApp.App.exe";

    private readonly Application _app;
    private readonly UIA3Automation _automation;
    private readonly Window _window;

    public MainFormE2ETests()
    {
        _app = Application.Launch(ExePath);
        _automation = new UIA3Automation();
        _window = _app.GetMainWindow(_automation);
    }

    [Fact]
    public void AddTask_WithValidTitle_AppearsInList()
    {
        var txtTitle = _window.FindFirstDescendant(cf => cf.ByAutomationId("txtTitle")).AsTextBox();
        var btnAdd = _window.FindFirstDescendant(cf => cf.ByAutomationId("btnAdd")).AsButton();
        var lstTasks = _window.FindFirstDescendant(cf => cf.ByAutomationId("lstTasks")).AsListBox();

        txtTitle.Enter("Buy milk");
        btnAdd.Click();

        Assert.Contains(lstTasks.Items, item => item.Text == "Buy milk");
    }

    [Fact]
    public void AddTask_WithEmptyTitle_ShowsError()
    {
        var btnAdd = _window.FindFirstDescendant(cf => cf.ByAutomationId("btnAdd")).AsButton();
        var lblError = _window.FindFirstDescendant(cf => cf.ByAutomationId("lblError")).AsLabel();

        btnAdd.Click(); // without typing anything into txtTitle

        Assert.False(string.IsNullOrEmpty(lblError.Text));
    }

    public void Dispose()
    {
        _app.Close();
        _automation.Dispose();
    }
}

using SimpleTaskApp.Core;

namespace SimpleTaskApp.App;

public partial class MainForm : Form
{
    private readonly TaskService _taskService;

    public MainForm(TaskService taskService)
    {
        _taskService = taskService;
        InitializeComponent();
        CargarTareas();
    }

    private void CargarTareas()
    {
        lstTasks.Items.Clear();
        foreach (var t in _taskService.GetAll())
            lstTasks.Items.Add(t.Title);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        try
        {
            _taskService.AddTask(txtTitle.Text);
            txtTitle.Clear();
            CargarTareas();
        }
        catch (ArgumentException ex)
        {
            lblError.Text = ex.Message;
        }
    }
}

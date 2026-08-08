namespace SimpleTaskApp.App;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtTitle;
    private Button btnAdd;
    private ListBox lstTasks;
    private Label lblError;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        txtTitle = new TextBox();
        btnAdd = new Button();
        lstTasks = new ListBox();
        lblError = new Label();
        SuspendLayout();

        // txtTitle
        txtTitle.Location = new Point(12, 12);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(250, 23);
        txtTitle.AccessibleName = "txtTitle"; // AutomationId para FlaUI

        // btnAdd
        btnAdd.Location = new Point(270, 11);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(90, 25);
        btnAdd.Text = "Agregar";
        btnAdd.AccessibleName = "btnAdd"; // AutomationId para FlaUI
        btnAdd.Click += btnAdd_Click;

        // lblError
        lblError.Location = new Point(12, 40);
        lblError.Name = "lblError";
        lblError.Size = new Size(348, 23);
        lblError.ForeColor = Color.Red;
        lblError.AccessibleName = "lblError";

        // lstTasks
        lstTasks.Location = new Point(12, 70);
        lstTasks.Name = "lstTasks";
        lstTasks.Size = new Size(348, 200);
        lstTasks.AccessibleName = "lstTasks"; // AutomationId para FlaUI

        // MainForm
        ClientSize = new Size(380, 290);
        Controls.Add(lstTasks);
        Controls.Add(lblError);
        Controls.Add(btnAdd);
        Controls.Add(txtTitle);
        Name = "MainForm";
        Text = "Simple Task App";
        ResumeLayout(false);
        PerformLayout();
    }
}

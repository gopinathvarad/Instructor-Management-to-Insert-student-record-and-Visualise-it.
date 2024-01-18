using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace instructorManager
{
    public partial class instructor : Form
    {
        // DataTable to store student information
        DataTable instruct = new DataTable();

        public instructor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize columns in the DataTable
            instruct.Columns.Add("Student Name");
            instruct.Columns.Add("University Number");
            instruct.Columns.Add("Field of Study");
            instruct.Columns.Add("Score in Subject 1 ");
            instruct.Columns.Add("Score in Subject 2");
            instruct.Columns.Add("Score in Subject 3");
            instruct.Columns.Add("Average");
            instruct.Columns.Add("Result");

            // Set DataTable as the data source for the DataGridView
            dataGridView1.DataSource = instruct;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            // Clear input fields
            nameTextBox.Text = "";
            UninumTextBox.Text = "";
            sub1TextBox.Text = "";
            sub2TextBox.Text = "";
            sub3TextBox.Text = "";
            courseBox.SelectedIndex = 1;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Extract input values
            String studentName = nameTextBox.Text;
            String universityNumber = UninumTextBox.Text;
            String subject1 = sub1TextBox.Text;
            String subject2 = sub2TextBox.Text;
            String subject3 = sub3TextBox.Text;

            // Ensure that the entered values are numeric
            if (double.TryParse(subject1, out double subject1Score) &&
                double.TryParse(subject2, out double subject2Score) &&
                double.TryParse(subject3, out double subject3Score))
            {
                // Calculate total and average
                double totalScore = subject1Score + subject2Score + subject3Score;
                double average = totalScore / 3;
                string result = (average > 50) ? "Pass" : "Fail";

                // Get selected field of study from the ComboBox
                String field = (string)courseBox.SelectedItem;

                // Add values to the DataTable
                instruct.Rows.Add(studentName, universityNumber, field, subject1, subject2, subject3, average, result);

                // Clear input fields after saving
                newButton_Click(sender, e);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Delete the selected row from the DataTable
            try
            {
                instruct.Rows[dataGridView1.CurrentCell.RowIndex].Delete();
            }
            catch (Exception err)
            {
                Console.WriteLine("Error: " + err);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Load values from the selected row into input fields for editing
            try
            {
                nameTextBox.Text = instruct.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[0].ToString();
                UninumTextBox.Text = instruct.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[1].ToString();
                sub1TextBox.Text = instruct.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[3].ToString();
                sub2TextBox.Text = instruct.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[4].ToString();
                sub3TextBox.Text = instruct.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[5].ToString();

                // Set the selected field of study in the ComboBox
                String itemToLookFor = instruct.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[2].ToString();
                courseBox.SelectedIndex = courseBox.Items.IndexOf(itemToLookFor);
            }
            catch (Exception err)
            {
                Console.WriteLine("There has been an error: " + err);
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            // Display a message box with the submitted data
            ShowVisualization();
        }

        private void ShowVisualization()
        {
            // Check if there is any data to visualize
            if (instruct.Rows.Count == 0)
            {
                MessageBox.Show("No data to visualize.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Build a message with student names and averages
            StringBuilder visualizationMessage = new StringBuilder();
            foreach (DataRow row in instruct.Rows)
            {
                string studentName = row["Student Name"].ToString();
                string average = row["Average"].ToString();
                visualizationMessage.AppendLine($"Student: {studentName}, Average: {average}");
            }

            // Display the visualization using MessageBox
            MessageBox.Show(visualizationMessage.ToString(), "Final Marks Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowChart_Click(object sender, EventArgs e)
        {
            // Check if there is any data to visualize
            if (instruct.Rows.Count == 0)
            {
                MessageBox.Show("No data to visualize.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a new chart control
            Chart chart = new Chart();
            chart.Size = new Size(600, 400);
            chart.Dock = DockStyle.Fill;

            // Create a new chart area
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            // Create a series for the chart
            Series series = new Series();
            series.ChartType = SeriesChartType.Column;

            // Iterate through each row in the DataTable
            foreach (DataRow row in instruct.Rows)
            {
                // Get the student name and average
                string studentName = row["Student Name"].ToString();
                double average = Convert.ToDouble(row["Average"]);

                // Add data points to the series
                series.Points.AddXY(studentName, average);
            }

            // Add the series to the chart
            chart.Series.Add(series);

            // Add the chart control to a new form
            Form chartForm = new Form();
            chartForm.Text = "Student Marks Chart";
            chartForm.Size = new Size(800, 600);
            chartForm.Controls.Add(chart);

            // Show the form with the chart
            chartForm.ShowDialog();
        }
    }
}

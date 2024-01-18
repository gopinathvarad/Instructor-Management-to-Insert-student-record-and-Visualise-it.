using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace instructorManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            // Set the PasswordChar property to '*' for security and privacy reasons
            textBox2.PasswordChar = '*';
        }

        // Clear the textboxes
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        // Exit the application
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Validate username and password
        private bool isValid()
        {
            if (textBox1.Text.TrimStart() == string.Empty)
            {
                MessageBox.Show("Enter Valid Username");
                return false;
            }
            else if (textBox2.Text.TrimStart() == string.Empty)
            {
                MessageBox.Show("Enter Valid Password");
                return false;
            }
            return true;
        }

        // Attempting to log in into the dashboard
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                // Establish connection to the database
                using (SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=\\uol.le.ac.uk\root\student\home\g\gv58\Desktop Files\instructorManager\instructorManager\Database1.mdf;Integrated Security=True"))
                {
                    // SQL query to check if the entered username and password exist in the LOGIN table
                    string query = "SELECT * FROM LOGIN WHERE username = '" + textBox1.Text.Trim() + "' AND password = '" + textBox2.Text.Trim() + "'";

                    // Use SqlDataAdapter to execute the query and fill the DataTable
                    SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
                    DataTable DTA = new DataTable();
                    SDA.Fill(DTA);

                    // If a match is found, open the instructor form
                    if (DTA.Rows.Count == 1)
                    {
                        instructor form1 = new instructor();
                        this.Hide();
                        form1.Show();
                    }
                }
            }
        }
    }
}

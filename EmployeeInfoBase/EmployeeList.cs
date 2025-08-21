using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EmployeeInfoBase
{
    //enum RowState
    //{
    //    Existed,
    //    New,
    //    Modified,
    //    ModifiedNew,
    //    Deleted
    //}

    public partial class EmployeeList : Form
    {
        DataBase dataBase = new DataBase();

        public EmployeeList()
        {
            InitializeComponent();
        }

        public void CreateColumns()
        {
            dataGridView1.Columns.Add("full_name", "ФИО");

            DataGridViewTextBoxColumn dateEmployColumn = new DataGridViewTextBoxColumn
            {
                Name = "date_employ",
                HeaderText = "Дата приёма",
                DefaultCellStyle = { NullValue = string.Empty }
            };

            DataGridViewTextBoxColumn dateUneployColumn = new DataGridViewTextBoxColumn
            {
                Name = "date_uneploy",
                HeaderText = "Дата увольнения",
                DefaultCellStyle = { NullValue = string.Empty }
            };

            dataGridView1.Columns.Add(dateEmployColumn);
            dataGridView1.Columns.Add(dateUneployColumn);
            dataGridView1.Columns.Add("status", "Статус");
            dataGridView1.Columns.Add("id_dep", "Отдел");
            dataGridView1.Columns.Add("id_post", "Должность");
        }

        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {
            object dateEmploy = record.IsDBNull(1) ? (object)DBNull.Value : record.GetDateTime(1).ToString("dd.MM.yyyy");
            object dateUneploy = record.IsDBNull(2) ? (object)DBNull.Value : record.GetDateTime(2).ToString("dd.MM.yyyy");

            dgv.Rows.Add(
                record.GetString(0),
                dateEmploy,
                dateUneploy,
                record.GetString(3),
                record.GetString(4),
                record.GetString(5));
        }

        public void FillDataGrid(DataGridView dgv)
        {
            dgv.Rows.Clear();

            SqlCommand cmd = new SqlCommand("GetEmployees", dataBase.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            dataBase.openConnection();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgv, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid(DataGridView dgv, string Status_F, string Department_F, 
            string Post_F, string Second_Name_F)
        {
            dgv.Rows.Clear();

            SqlCommand cmd = new SqlCommand("GetFilteredEmployees", dataBase.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Status_Filter", Status_F);
            cmd.Parameters.AddWithValue("@Department_Filter", Department_F);
            cmd.Parameters.AddWithValue("@Post_Filter", Post_F);
            cmd.Parameters.AddWithValue("@Second_Name_Filter", Second_Name_F);

            dataBase.openConnection();

            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                ReadSingleRow(dgv, reader);
            }
            reader.Close();
        }

        private void LoadFilterList()
        {
            SqlCommand cmd = new SqlCommand("GetEmployees", dataBase.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.NextResult())
            {
                cb_statuses.Items.Clear();
                while (reader.Read())
                {
                    cb_statuses.Items.Add(reader["name"].ToString());
                }
            }

            if (reader.NextResult())
            {
                cb_deps.Items.Clear();
                while (reader.Read())
                {
                    cb_deps.Items.Add(reader["name"].ToString());
                }
            }

            if (reader.NextResult())
            {
                cb_posts.Items.Clear();
                while (reader.Read())
                {
                    cb_posts.Items.Add(reader["name"].ToString());
                }
            }

            reader.Close();
        }

        private void EmployeeList_Load(object sender, EventArgs e)
        {
            CreateColumns();
            FillDataGrid(dataGridView1);
            LoadFilterList();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void btn_clearFilters_Click(object sender, EventArgs e)
        {
            cb_statuses.SelectedItem = null;
            cb_deps.SelectedItem = null;
            cb_posts.SelectedItem = null;
            tb_secondName.Text = null;
            FillDataGrid(dataGridView1);
        }

        private void btn_sort_Click(object sender, EventArgs e)
        {
            string Status_F = null;
            string Department_F = null;
            string Post_F = null;
            string Second_Name_F = null;

            if (cb_statuses.SelectedItem != null)
            {
                Status_F = cb_statuses.SelectedItem.ToString();
            }

            if (cb_deps.SelectedItem != null)
            {
                Department_F = cb_deps.SelectedItem.ToString();
            }

            if (cb_posts.SelectedItem != null)
            {
                Post_F = cb_posts.SelectedItem.ToString();
            }

            if (tb_secondName != null)
            {
                Second_Name_F = tb_secondName.Text;
            }
            RefreshDataGrid(dataGridView1, Status_F, Department_F, Post_F, Second_Name_F);
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics statisticsForm = new Statistics();
            statisticsForm.Show();
        }
    }
}

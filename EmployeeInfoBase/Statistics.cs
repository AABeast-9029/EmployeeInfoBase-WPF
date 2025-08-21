using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace EmployeeInfoBase
{
    public partial class Statistics : Form
    {
        DataBase dataBase = new DataBase();

        public Statistics()
        {
            InitializeComponent();
        }

        public void CreateColumns()
        {
            dgv_Stat.Columns.Add("date", "Дата");
            dgv_Stat.Columns.Add("employee_count", "Количество");
        }

        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {
            dgv.Rows.Add(
                record.GetDateTime(0).ToString("dd.MM.yyyy"),
                record.GetInt32(1));
        }

        private void LoadStatusList()
        {
            string query = @"SELECT name FROM status";

            SqlCommand cmd = new SqlCommand(query, dataBase.getConnection());
            cmd.CommandType = CommandType.Text;
            dataBase.openConnection();

            SqlDataReader reader = cmd.ExecuteReader();

            cb_statuses.Items.Clear();
            while (reader.Read())
            {
                cb_statuses.Items.Add(reader["name"].ToString());
            }

            reader.Close();
        }

        private void RefreshDataGrid(DataGridView dgv, string Status_F, DateTime? startDate, DateTime? endDate, bool isHired)
        {
            dgv.Rows.Clear();

            SqlCommand cmd = new SqlCommand("GetEmployeeStatistics", dataBase.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Status_Filter", Status_F);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@IsHired", isHired ? false : true);

            dataBase.openConnection();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgv, reader);
            }

            reader.Close();
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            CreateColumns();
            LoadStatusList();
        }

        private void btn_showStat_Click(object sender, EventArgs e)
        {
            string Status_F = "";
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (cb_statuses.SelectedItem != null)
            {
                Status_F = cb_statuses.SelectedItem.ToString();
            }

            if (dtp_start.Checked)
            {
                startDate = dtp_start.Value;
            }

            if (dtp_end.Checked)
            {
                endDate = dtp_end.Value;
            }

            bool isHired = checkb_uneploy.Checked;

            RefreshDataGrid(dgv_Stat, Status_F, startDate, endDate, isHired);
        }
    }
}

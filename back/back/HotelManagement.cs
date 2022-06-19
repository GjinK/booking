using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CS_CRUD_SQLServer
{
    public partial class HotelManagement : Form
    {
        public HotelManagement()
        {
            InitializeComponent();
        }

        public void DGV_GetHotels()
        {
            //to refresh content
            dgv_Hotels.DataSource = null;

            var conn = new SqlConnection(CSTool.sqlConnString());
            var cmd = new SqlCommand("SP_GetHotels", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            var dataAdapter = new SqlDataAdapter(cmd);

            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();

            dataAdapter.Fill(ds);
            dgv_Hotels.DataSource = ds.Tables[0];

            conn.Close();

            //Make Uneditable
            dgv_Hotels.ReadOnly = true;

            //Auto size specific columns

            dgv_Hotels.Columns[0].Width = 40;
            dgv_Hotels.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Hotels.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Hotels.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Hotels.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv_Hotels.Columns[5].Width = 70;

            //hide row header
            dgv_Hotels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_Hotels.RowHeadersVisible = false;

            //Disable row resize
            dgv_Hotels.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv_Hotels.AllowHotelToResizeRows = false;

            //Remove extra row
            dgv_Hotels.AllowHotelToAddRows = false;

        }

        private void HotelManagement_Load(object sender, EventArgs e)
        {
            DGV_GetHotels();

            //RESTRICT ACCESS
            if (CSULevel.Admin()) btn_add.Visible = true;
            if (CSULevel.Hotel() || CSULevel.Support() || CSULevel.Admin()) btn_view.Visible = true;
            if (CSULevel.Support() || CSULevel.Admin()) btn_edit.Visible = true;
            if (CSULevel.Admin()) btn_delete.Visible = true;
        }

        private void HotelManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            var CPanelInstance = Application.OpenForms.OfType<CPanel>().Single();
            CPanelInstance.Show();
        }

        private void dgv_Hotels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btn_edit.Enabled = true;
                btn_view.Enabled = true;
                btn_delete.Enabled = true;

                //DATA FROM DGV
                CSVar.selected_id = (int)dgv_Hotels.Rows[e.RowIndex].Cells[0].Value;
                CSVar.selected_uname = dgv_Hotels.Rows[e.RowIndex].Cells[1].Value.ToString();
                CSVar.selected_fname = dgv_Hotels.Rows[e.RowIndex].Cells[2].Value.ToString();
                CSVar.selected_lname = dgv_Hotels.Rows[e.RowIndex].Cells[3].Value.ToString();
                CSVar.selected_email = dgv_Hotels.Rows[e.RowIndex].Cells[4].Value.ToString();
                CSVar.selected_Hotellevel = (int)dgv_Hotels.Rows[e.RowIndex].Cells[5].Value;
            }
            else
            {
                btn_edit.Enabled = false;
                btn_view.Enabled = false;
                btn_delete.Enabled = false;
            }
        }

        private void dgv_Hotels_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                new ViewHotel().ShowDialog();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            new EditHotel().ShowDialog();
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            new ViewHotel().ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to permanently delete the selected Hotel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                deleteHotel();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void deleteHotel()
        {
            SqlConnection conn = new SqlConnection(CSTool.sqlConnString());
            SqlCommand cmd = new SqlCommand("SP_DeleteHotel", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", CSVar.selected_id);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            //refresh table content
            DGV_GetHotels();

            //notif
            CSTool.infoMessage("Record has been deleted.");
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            new AddHotel().ShowDialog();
        }
    }
}

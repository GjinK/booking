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
    public partial class RoomManagement : Form
    {
        public RoomManagement()
        {
            InitializeComponent();
        }

        public void DGV_GetRooms()
        {
            //to refresh content
            dgv_Rooms.DataSource = null;

            var conn = new SqlConnection(CSTool.sqlConnString());
            var cmd = new SqlCommand("SP_GetRooms", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            var dataAdapter = new SqlDataAdapter(cmd);

            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();

            dataAdapter.Fill(ds);
            dgv_Rooms.DataSource = ds.Tables[0];

            conn.Close();

            //Make Uneditable
            dgv_Rooms.ReadOnly = true;

            //Auto size specific columns

            dgv_Rooms.Columns[0].Width = 40;
            dgv_Rooms.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Rooms.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Rooms.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Rooms.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv_Rooms.Columns[5].Width = 70;

            //hide row header
            dgv_Rooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_Rooms.RowHeadersVisible = false;

            //Disable row resize
            dgv_Rooms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv_Rooms.AllowRoomToResizeRows = false;

            //Remove extra row
            dgv_Rooms.AllowRoomToAddRows = false;

        }

        private void RoomManagement_Load(object sender, EventArgs e)
        {
            DGV_GetRooms();

            //RESTRICT ACCESS
            if (CSULevel.Admin()) btn_add.Visible = true;
            if (CSULevel.Room() || CSULevel.Support() || CSULevel.Admin()) btn_view.Visible = true;
            if (CSULevel.Support() || CSULevel.Admin()) btn_edit.Visible = true;
            if (CSULevel.Admin()) btn_delete.Visible = true;
        }

        private void RoomManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            var CPanelInstance = Application.OpenForms.OfType<CPanel>().Single();
            CPanelInstance.Show();
        }

        private void dgv_Rooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btn_edit.Enabled = true;
                btn_view.Enabled = true;
                btn_delete.Enabled = true;

                //DATA FROM DGV
                CSVar.selected_id = (int)dgv_Rooms.Rows[e.RowIndex].Cells[0].Value;
                CSVar.selected_uname = dgv_Rooms.Rows[e.RowIndex].Cells[1].Value.ToString();
                CSVar.selected_fname = dgv_Rooms.Rows[e.RowIndex].Cells[2].Value.ToString();
                CSVar.selected_lname = dgv_Rooms.Rows[e.RowIndex].Cells[3].Value.ToString();
                CSVar.selected_email = dgv_Rooms.Rows[e.RowIndex].Cells[4].Value.ToString();
                CSVar.selected_Roomlevel = (int)dgv_Rooms.Rows[e.RowIndex].Cells[5].Value;
            }
            else
            {
                btn_edit.Enabled = false;
                btn_view.Enabled = false;
                btn_delete.Enabled = false;
            }
        }

        private void dgv_Rooms_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                new ViewRoom().ShowDialog();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            new EditRoom().ShowDialog();
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            new ViewRoom().ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to permanently delete the selected Room?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                deleteRoom();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void deleteRoom()
        {
            SqlConnection conn = new SqlConnection(CSTool.sqlConnString());
            SqlCommand cmd = new SqlCommand("SP_DeleteRoom", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", CSVar.selected_id);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            //refresh table content
            DGV_GetRooms();

            //notif
            CSTool.infoMessage("Record has been deleted.");
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            new AddRoom().ShowDialog();
        }
    }
}

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
    public partial class EditHotel : Form
    {
        public EditHotel()
        {
            InitializeComponent();
        }

        private void LoadSelectedRow()
        {
            txt_id.Text = CSVar.selected_id.ToString();
            txt_uname.Text = CSVar.selected_uname;
            txt_fname.Text = CSVar.selected_fname;
            txt_lname.Text = CSVar.selected_lname;
            txt_email.Text = CSVar.selected_email;
            txt_ulevel.Text = CSVar.selected_Hotellevel.ToString();
           
        }

        private void EditHotel_Load(object sender, EventArgs e)
        {
            LoadSelectedRow();

            
            if (CSULevel.Admin()) txt_ulevel.ReadOnly = false;

        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to update this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                UpdateHotel();
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }
        }

        private void UpdateHotel()
        {
            SqlConnection conn = new SqlConnection(CSTool.sqlConnString());
            SqlCommand cmd = new SqlCommand("SP_UpdateHotel", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            txt_uname.Text = txt_uname.Text.Trim();

            cmd.Parameters.AddWithValue("@id", txt_id.Text);
            cmd.Parameters.AddWithValue("@uname", txt_uname.Text);
            cmd.Parameters.AddWithValue("@fname", txt_fname.Text);
            cmd.Parameters.AddWithValue("@lname", txt_lname.Text);
            cmd.Parameters.AddWithValue("@email", txt_email.Text);
            cmd.Parameters.AddWithValue("@ulevel", txt_ulevel.Text);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            
            refreshHotelsTable();

        
            CSTool.infoMessage("Record has been updated.");
        }

        private void refreshHotelsTable()
        {
            var UMInstance = Application.OpenForms.OfType<HotelManagement>().Single();
            UMInstance.DGV_GetHotels();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_CRUD_SQLServer
{
    public partial class ViewRoom : Form
    {
        public ViewRoom()
        {
            InitializeComponent();
        }

        private void ViewRoom_Load(object sender, EventArgs e)
        {
            LoadSelectedRow();
        }

        private void LoadSelectedRow()
        {
            lbl_id.Text = CSVar.selected_id.ToString();
            lbl_uname.Text = CSVar.selected_uname;
            lbl_fname.Text = CSVar.selected_fname;
            lbl_lname.Text = CSVar.selected_lname;
            lbl_email.Text = CSVar.selected_email;
            lbl_ulevel.Text = CSVar.selected_Roomlevel.ToString();
            //CSVar.Clear();//clear data after loading
        }
    }
}

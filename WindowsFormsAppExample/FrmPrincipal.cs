using System;
using System.Windows.Forms;
using Models;
namespace WindowsFormsAppExample
{
    public partial class FrmPrincipal : Form
    {
        private ConnectionSqlServer ConnectionSqlServer { get; set; }

        public FrmPrincipal()
        {
            InitializeComponent();
            ConnectionSqlServer = new ConnectionSqlServer();
        }

        private void pessoasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPeople frm = new FrmPeople(ConnectionSqlServer);
            frm.ShowDialog();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConnectionSqlServer?.Dispose();
            ConnectionSqlServer = null;
        }
    }
}

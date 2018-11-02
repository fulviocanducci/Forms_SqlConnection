using System;
using System.Windows.Forms;
using Models;
namespace WindowsFormsAppExample
{
    public partial class FrmPeople : Form
    {        
        public DalPeople DalPeople { get; set; }

        public FrmPeople(ConnectionSqlServer connectionSqlServer)
        {
            InitializeComponent();        
            DalPeople = new DalPeople(connectionSqlServer);
        }       

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            FrmPeopleCreateOrEdit frm = new FrmPeopleCreateOrEdit(DalPeople);
            frm.ShowDialog();
            GridLoad();
        }

        private void FrmPeople_Load(object sender, EventArgs e)
        {
            GridLoad();
        }

        private void GridLoad(string filter = null)
        {
            GridPeople.AutoGenerateColumns = false;
            GridPeople.DataSource = DalPeople.Get(filter, "Id", "Name");
            GridPeople.Update();
        }

        private void GridPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (int.TryParse(GridPeople.CurrentRow.Cells[0].Value.ToString(), out var id))
                {
                    FrmPeopleCreateOrEdit frm = new FrmPeopleCreateOrEdit(DalPeople, id);
                    frm.ShowDialog();
                    GridLoad();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (GridPeople.Rows.Count > 0)
            {
                if (GridPeople.CurrentRow != null)
                {
                    if (int.TryParse(GridPeople.CurrentRow.Cells[0].Value.ToString(), out var id))
                    {
                        FrmPeopleCreateOrEdit frm = new FrmPeopleCreateOrEdit(DalPeople, id);
                        frm.ShowDialog();
                        GridLoad();
                    }
                }
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            GridLoad(!string.IsNullOrEmpty(TxtFilter.Text) ? TxtFilter.Text : null);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (GridPeople.Rows.Count > 0)
            {
                if (GridPeople.CurrentRow != null)
                {
                    if (int.TryParse(GridPeople.CurrentRow.Cells[0].Value.ToString(), out var id))
                    {
                        if (MessageBox.Show("Deseja excluir?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DalPeople.Delete(id);
                            GridLoad();
                        } 
                    }
                }
            }
        }
    }
}

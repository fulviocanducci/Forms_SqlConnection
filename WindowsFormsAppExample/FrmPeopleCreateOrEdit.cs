using System;
using System.Windows.Forms;
using Models;
namespace WindowsFormsAppExample
{
    public partial class FrmPeopleCreateOrEdit : Form
    {
        public Operation Operation { get; private set; } = Operation.Insert;
        public int Id { get; private set; } = 0;        
        public DalPeople DalPeople { get; set; }        

        public FrmPeopleCreateOrEdit(DalPeople dalPeople)
        {
            InitializeComponent();
            DalPeople = dalPeople;
        }
        
        public FrmPeopleCreateOrEdit(DalPeople dalPeople, int id)
        {
            InitializeComponent();
            DalPeople = dalPeople;
            Id = id;
            Operation = Operation.Edit;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                People people = new People();
                people.Name = TxtName.Text;
                people.Created = DateTime.Now;
                if (DateTime.TryParse(TxtBirthday.Text, out var dateBirthday))
                {
                    people.DateBirthday = dateBirthday;
                }
                people.Active = ChkActive.Checked;
                people.Salary = 0m;
                if (decimal.TryParse(TxtSalary.Text, out var salary))
                {
                    people.Salary = salary;
                }
                if (Operation == Operation.Edit)
                {
                    people.Id = Id;
                    DalPeople.Edit(people);
                }
                else if (Operation == Operation.Insert)
                {
                    DalPeople.Insert(people);
                }

                Close();
            }
            else
            {
                MessageBox.Show("Dados inválidos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FrmPeopleCreateOrEdit_Load(object sender, EventArgs e)
        {
            if (Operation == Operation.Edit)
            {
                People people = DalPeople.Find(Id);
                TxtName.Text = people.Name;
                TxtSalary.Text = people.Salary.ToString("N2");
                if (people.DateBirthday.HasValue)
                {
                    TxtBirthday.Text = people.DateBirthday.Value.ToString("dd/MM/yyyy");
                }
                ChkActive.Checked = people.Active;
            }
        }

        private void TxtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TxtName.Text.Trim().Length == 0)
            {
                e.Cancel = true;
                ErrorProviderCollection.SetError(TxtName, "Digite o nome");
            }
            else
            {
                ErrorProviderCollection.SetError(TxtName, "");
            }
        }

        private void TxtBirthday_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DateTime.TryParse(TxtBirthday.Text, out _))
            {
                e.Cancel = true;
                ErrorProviderCollection.SetError(TxtBirthday, "Digite o aniversário");
            }
            else
            {
                ErrorProviderCollection.SetError(TxtBirthday, "");
            }
        }

        private void TxtSalary_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!decimal.TryParse(TxtSalary.Text, out _))
            {
                e.Cancel = true;
                ErrorProviderCollection.SetError(TxtSalary, "Valor salarial errado");
            }
            else
            {
                ErrorProviderCollection.SetError(TxtSalary, "");
            }
        }
    }
}

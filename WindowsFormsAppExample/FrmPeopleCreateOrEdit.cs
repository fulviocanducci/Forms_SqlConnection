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

    }
}

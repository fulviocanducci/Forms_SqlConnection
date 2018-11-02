using System;
namespace Models
{
    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DateBirthday { get; set; }
        public bool Active { get; set; }
        public decimal Salary { get; set; }
    }
}

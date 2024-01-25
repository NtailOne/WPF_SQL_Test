namespace WPF_SQL_Test
{
    public class Employee
    {
        public int idEmp { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public Department department { get; set; }

        public Employee() { }
        public Employee(Employee other)
        {
            idEmp = other.idEmp;
            lastName = other.lastName;
            firstName = other.firstName;
            department = other.department;
        }
    }
}

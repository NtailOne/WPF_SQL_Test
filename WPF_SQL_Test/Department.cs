namespace WPF_SQL_Test
{
    public class Department
    {
        public int idDep { get; set; }
        public string depName { get; set; }

        public Department() { }
        public Department(Department other)
        {
            idDep = other.idDep;
            depName = other.depName;
        }
    }
}

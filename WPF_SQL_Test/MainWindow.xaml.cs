using System.Collections.Generic;
using System.Windows;

namespace WPF_SQL_Test
{
    public partial class MainWindow : Window
    {
        private Data data;

        public List<Department> departments { get; set; }
        public List<Employee> employees { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            data = new Data();

            departments = data.GetDepartments();
            employees = data.GetEmployees();

            DataContext = this;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeCreateWindow employeeCreateWindow = new EmployeeCreateWindow();
            employeeCreateWindow.ShowDialog();
            if (employeeCreateWindow.DialogResult == true)
            {
                
            }
        }
    }
}

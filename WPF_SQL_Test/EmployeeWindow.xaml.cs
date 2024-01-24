using System.Collections.Generic;
using System.Windows;

namespace WPF_SQL_Test
{
    public partial class EmployeeWindow : Window
    {
        public Employee newEmployee { get; private set; }

        public EmployeeWindow(List<Department> departments)
        {
            InitializeComponent();
            departmentComboBox.ItemsSource = departments;
            newEmployee = new Employee();

            DataContext = this;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (lastNameBox.Text.Replace(" ", "") == "" || firstNameBox.Text.Replace(" ", "") == "" || departmentComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            newEmployee.lastName = lastNameBox.Text;
            newEmployee.firstName = firstNameBox.Text;
            newEmployee.department = departmentComboBox.SelectedItem as Department;

            DialogResult = true;
            Close();
        }
    }
}

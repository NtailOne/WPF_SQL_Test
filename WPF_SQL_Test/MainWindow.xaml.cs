using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_SQL_Test
{
    public partial class MainWindow : Window
    {
        private Data data;

        public List<Department> departments { get; set; }
        public List<Employee> employees { get; set; }

        private bool employeeHasChanged = false;
        private Employee oldEmployee;

        public MainWindow()
        {
            InitializeComponent();

            data = new Data();

            UpdateGrid();

            DataContext = this;
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Employee employeeToDelete = grid.SelectedItem as Employee;
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить сотрудника " + employeeToDelete.lastName + " " + employeeToDelete.firstName, "Внимание", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Handled = true;
                    return;
                }
                else if (result == MessageBoxResult.OK)
                {
                    data.DeleteEmployeeById(employeeToDelete.idEmp);
                    UpdateGrid();
                }
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow employeeCreateWindow = new EmployeeWindow(departments);
            employeeCreateWindow.ShowDialog();
            if (employeeCreateWindow.DialogResult == true)
            {
                data.AddEmployee(employeeCreateWindow.newEmployee);
                UpdateGrid();
            }
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            DepartmentWindow departmentWindow = new DepartmentWindow();
            departmentWindow.ShowDialog();
            if (departmentWindow.DialogResult == true)
            {
                data.AddDepartment(departmentWindow.newDepartment);
                departments = data.GetDepartments();
            }
        }

        private void EditDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Department department = button.DataContext as Department;
            DepartmentWindow departmentWindow = new DepartmentWindow(department);
            departmentWindow.ShowDialog();
            if (departmentWindow.DialogResult == true)
            {
                data.UpdateDepartment(departmentWindow.newDepartment);
                UpdateGrid();
            }
        }

        private void DeleteDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Department departmentToDelete = button.DataContext as Department;
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить подразделение \"" + departmentToDelete.depName + "\"", "Внимание", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                e.Handled = true;
            }
            else if (result == MessageBoxResult.OK)
            {
                data.DeleteDepartmentFromEmployees(departmentToDelete.idDep);
                data.DeleteDepartmentById(departmentToDelete.idDep);
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            departments = data.GetDepartments();
            employees = data.GetEmployees();
            grid.ItemsSource = employees;
            grid.Items.Refresh();
        }

        private void grid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && employeeHasChanged)
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите изменить данные сотрудника?", "Внимание", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    employeeHasChanged = false;
                }
                else if (result == MessageBoxResult.OK)
                {
                    Employee employeeToEdit = e.Row.Item as Employee;
                    data.UpdateEmployee(employeeToEdit);
                }
                UpdateGrid();
            }
        }

        private void DepartmentComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Employee emp = grid.SelectedItem as Employee;
            ComboBox cb = sender as ComboBox;
            emp.department = cb.SelectedItem as Department;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            employeeHasChanged = true;
            oldEmployee = grid.SelectedItem as Employee;
        }
    }
}

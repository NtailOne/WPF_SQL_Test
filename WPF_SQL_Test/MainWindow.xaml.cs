using System;
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
                    data.ExecutableQuery($"DELETE FROM employees WHERE id_emp = {employeeToDelete.idEmp}");
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
                data.ExecutableQuery($"INSERT INTO employees (last_name, first_name, department_id) VALUES ({employeeCreateWindow.newEmployee.lastName}, {employeeCreateWindow.newEmployee.firstName}, {employeeCreateWindow.newEmployee.department.idDep})");
                UpdateGrid();
            }
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            DepartmentWindow departmentWindow = new DepartmentWindow();
            departmentWindow.ShowDialog();
            if (departmentWindow.DialogResult == true)
            {
                data.ExecutableQuery($"INSERT INTO departments(dep_name) VALUES({departmentWindow.newDepartment.depName})");
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
                data.ExecutableQuery($"UPDATE departments SET dep_name = {departmentWindow.newDepartment.depName} WHERE id_dep = {departmentWindow.newDepartment.idDep}");
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
                data.ExecutableQuery($"UPDATE employees SET department_id = {DBNull.Value} WHERE department_id = {departmentToDelete.idDep}");
                data.ExecutableQuery($"DELETE FROM departments WHERE id_dep = {departmentToDelete.idDep}");
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

        private void grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            oldEmployee = new Employee(grid.SelectedItem as Employee);
        }

        private void grid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Employee employeeToEdit = e.Row.Item as Employee;
            if (e.EditAction == DataGridEditAction.Commit && oldEmployee != employeeToEdit)
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите изменить данные сотрудника?", "Внимание", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    employeeToEdit = oldEmployee; /*тут не работает*/
                }
                else if (result == MessageBoxResult.OK)
                {
                    data.ExecutableQuery($"UPDATE employees SET last_name = {employeeToEdit.lastName}, first_name = {employeeToEdit.firstName}, department_id = {employeeToEdit.department.idDep} WHERE id_emp = {employeeToEdit.idEmp}");
                    UpdateGrid();
                }
            }
        }

        private void DepartmentComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Employee emp = grid.SelectedItem as Employee;
            ComboBox cb = sender as ComboBox;
            emp.department = cb.SelectedItem as Department;
        }
    }
}

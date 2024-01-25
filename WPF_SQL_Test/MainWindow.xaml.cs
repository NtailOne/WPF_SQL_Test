using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@EmployeeId", employeeToDelete.idEmp)
                    };
                    data.ExecutableQuery("DELETE FROM employees WHERE id_emp = @EmployeeId", parameters);
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
                SqlParameter[] parameters =
                {
                    new SqlParameter("@LastName", employeeCreateWindow.newEmployee.lastName),
                    new SqlParameter("@FirstName", employeeCreateWindow.newEmployee.firstName),
                    new SqlParameter("@DepartmentId", employeeCreateWindow.newEmployee.department.idDep)
                };
                data.ExecutableQuery("INSERT INTO employees (last_name, first_name, department_id) VALUES (@LastName, @FirstName, @DepartmentId)", parameters);
                UpdateGrid();
            }
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            DepartmentWindow departmentWindow = new DepartmentWindow();
            departmentWindow.ShowDialog();
            if (departmentWindow.DialogResult == true)
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@DepartmentName", departmentWindow.newDepartment.depName)
                };
                data.ExecutableQuery("INSERT INTO departments(dep_name) VALUES(@DepartmentName)", parameters);
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
                SqlParameter[] parameters =
                {
                    new SqlParameter("@DepartmentName", departmentWindow.newDepartment.depName),
                    new SqlParameter("@DepartmentId", departmentWindow.newDepartment.idDep)
                };
                data.ExecutableQuery("UPDATE departments SET dep_name = @DepartmentName WHERE id_dep = @DepartmentId", parameters);
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
                SqlParameter[] updateEmployeesParameters =
                {
                    new SqlParameter("@DBNull", DBNull.Value),
                    new SqlParameter("@DepartmentId", departmentToDelete.idDep)
                };
                SqlParameter[] deleteDepartmentParameters =
                {
                    new SqlParameter("@DepartmentId", departmentToDelete.idDep)
                };
                data.ExecutableQuery("UPDATE employees SET department_id = @DBNull WHERE department_id = @DepartmentId", updateEmployeesParameters);
                data.ExecutableQuery("DELETE FROM departments WHERE id_dep = @DepartmentId", deleteDepartmentParameters);
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

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            oldEmployee = new Employee(grid.SelectedItem as Employee);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Employee employeeToEdit = e.Row.Item as Employee;
            if (e.EditAction == DataGridEditAction.Commit && oldEmployee != employeeToEdit)
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите изменить данные сотрудника?", "Внимание", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    UpdateGrid();
                }
                else if (result == MessageBoxResult.OK)
                {
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@EmployeeId", employeeToEdit.idEmp),
                        new SqlParameter("@LastName", employeeToEdit.lastName),
                        new SqlParameter("@FirstName", employeeToEdit.firstName),
                        new SqlParameter("@DepartmentId", employeeToEdit.department.idDep)
                    };
                    data.ExecutableQuery("UPDATE employees SET last_name = @LastName, first_name = @FirstName, department_id = @DepartmentId WHERE id_emp = @EmployeeId", parameters);
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WPF_SQL_Test
{
    public class Data
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public DataTable GetQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        public void ExecutableQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddRange(parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public List<Department> GetDepartments()
        {
            DataTable dataTable = GetQuery("SELECT * FROM departments");
            List<Department> departments = new List<Department>();

            foreach (DataRow row in dataTable.Rows)
            {
                int departmentId = Convert.ToInt32(row["id_dep"]);
                string departmentName = row["dep_name"].ToString();

                Department department = new Department()
                {
                    idDep = departmentId,
                    depName = departmentName
                };

                departments.Add(department);
            }

            return departments;
        }

        private Department GetDepartmentById(int departmentId)
        {
            DataTable dataTable = GetQuery($"SELECT * FROM departments WHERE id_dep = {departmentId}");

            if (dataTable.Rows.Count > 0)
            {
                string departmentName = dataTable.Rows[0]["dep_name"].ToString();

                Department department = new Department()
                {
                    idDep = departmentId,
                    depName = departmentName
                };

                return department;
            }

            return null;
        }

        public List<Employee> GetEmployees()
        {
            DataTable dataTable = GetQuery("SELECT * FROM employees");
            List<Employee> employees = new List<Employee>();

            foreach (DataRow row in dataTable.Rows)
            {
                int employeeId = Convert.ToInt32(row["id_emp"]);
                string lastName = row["last_name"].ToString();
                string firstName = row["first_name"].ToString();
                int departmentId;
                object departmentIdObj = row["department_id"];
                if (departmentIdObj != DBNull.Value) departmentId = Convert.ToInt32(row["department_id"]);
                else departmentId = -1;

                Department department = GetDepartmentById(departmentId);

                Employee employee = new Employee()
                {
                    idEmp = employeeId,
                    lastName = lastName,
                    firstName = firstName,
                    department = department
                };

                employees.Add(employee);
            }

            return employees;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WPF_SQL_Test
{
    public class Data
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM departments";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int departmentId = Convert.ToInt32(reader["id_dep"]);
                    string departmentName = reader["dep_name"].ToString();

                    Department department = new Department()
                    {
                        idDep = departmentId,
                        depName = departmentName
                    };

                    departments.Add(department);
                }

                reader.Close();
            }

            return departments;
        }

        public void AddDepartment(Department department)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO departments (dep_name) VALUES (@DepartmentName)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentName", department.depName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE departments SET dep_name = @DepartmentName WHERE id_dep = @DepartmentId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentId", department.idDep);
                command.Parameters.AddWithValue("@DepartmentName", department.depName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteDepartmentById(int depId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM departments WHERE id_dep = @DepartmentId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentId", depId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Department GetDepartmentById(int departmentId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM departments WHERE id_dep = @DepartmentId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentId", departmentId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string departmentName = reader["dep_name"].ToString();

                    Department department = new Department()
                    {
                        idDep = departmentId,
                        depName = departmentName
                    };

                    return department;
                }

                reader.Close();
            }

            return null;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM employees";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int employeeId = Convert.ToInt32(reader["id_emp"]);
                    string lastName = reader["last_name"].ToString();
                    string firstName = reader["first_name"].ToString();
                    int departmentId = Convert.ToInt32(reader["department_id"]);

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

                reader.Close();
            }

            return employees;
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO employees (last_name, first_name, department_id) VALUES (@EmployeeId, @LastName, @FirstName, @DepartmentId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employee.idEmp);
                command.Parameters.AddWithValue("@LastName", employee.lastName);
                command.Parameters.AddWithValue("@FirstName", employee.firstName);
                command.Parameters.AddWithValue("@DepartmentId", employee.department.idDep);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE employees SET last_name = @LastName, first_name = @FirstName, department_id = @DepartmentId WHERE id_emp = @EmployeeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employee.idEmp);
                command.Parameters.AddWithValue("@LastName", employee.lastName);
                command.Parameters.AddWithValue("@FirstName", employee.firstName);
                command.Parameters.AddWithValue("@DepartmentId", employee.department.idDep);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEmployeeById(int empId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM employees WHERE id_emp = @EmployeeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", empId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}

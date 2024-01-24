using System.Windows;

namespace WPF_SQL_Test
{
    public partial class DepartmentWindow : Window
    {
        public Department newDepartment { get; private set; }

        public DepartmentWindow()
        {
            InitializeComponent();

            newDepartment = new Department();
            button.Content = "Добавить";
        }

        public DepartmentWindow(Department department)
        {
            InitializeComponent();

            newDepartment = department;
            departmentNameBox.Text = department.depName;
            button.Content = "Изменить";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (departmentNameBox.Text.Replace(" ", "") == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            newDepartment.depName = departmentNameBox.Text;

            DialogResult = true;
            Close();
        }
    }
}

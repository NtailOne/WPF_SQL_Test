using System.Collections.Generic;
using System.Windows;

namespace WPF_SQL_Test
{
    public partial class EmployeeCreateWindow : Window
    {
        public Employee newEmployee { get; private set; }
        public EmployeeCreateWindow()
        {
            InitializeComponent();
        }
    }
}

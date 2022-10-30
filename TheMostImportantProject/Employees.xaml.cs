using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TheMostImportantProject
{
    public partial class Employees : Window
    {
        public Employees()
        {
            
            InitializeComponent();
            ListViewLoad();
        }
        
        public void ListViewLoad()
        {
            using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
            {
                var employees = db.Employee.ToList();
                List<Emp> employeesList = new List<Emp>();
                foreach (var employee in employees)
                {
                    employeesList.Add(new Emp(employee.EmployeeID, employee.Login, employee.Password, employee.LastName, employee.FirstName, employee.DateOfBirth.ToString("dd-MM-yyyy"), employee.PhoneNumber));
                }
                dgEmp.ItemsSource = employeesList;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmp.SelectedIndex >= 0) {
                var result = MessageBox.Show("Вы точно хотите удалить этого сотрудника?", "Удалить", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var item = dgEmp.SelectedItem as Emp;
                    int id = item.EmployeeID;
                    using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
                    {
                        Employee employee = db.Employee.Where(x => x.EmployeeID == id).FirstOrDefault();

                        db.Employee.Remove(employee);
                        db.SaveChanges();
                    }
                    ListViewLoad();
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAdd add = new EmployeeAdd();
            add.Show();
            this.Close();
        }

        private void ToMenu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Close();
        }
    }
}

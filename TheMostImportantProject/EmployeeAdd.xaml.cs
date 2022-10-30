using System;
using System.Collections.Generic;
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
    public partial class EmployeeAdd : Window
    {
        public EmployeeAdd()
        {
            InitializeComponent();
        }

        private void PhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                e.Handled = true;
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (LastName.Text == "" || FirstName.Text == "" || PhoneNumber.Text == "" || Password.Text == "" || Login.Text == "" || DateOfBirth.SelectedDate != null)
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
                {
                    Employee employee = new Employee();
                    employee.FirstName = FirstName.Text;
                    employee.LastName = LastName.Text;
                    employee.PhoneNumber = PhoneNumber.Text;
                    employee.Password = Password.Text;
                    employee.Login = Login.Text;
                    employee.DateOfBirth = (DateTime)DateOfBirth.SelectedDate;

                    db.Employee.Add(employee);
                    db.SaveChanges();

                    MessageBox.Show("Сотрудник успешно добавлен");
                }
                LastName.Text = "";
                FirstName.Text = "";
                PhoneNumber.Text = "";
                Password.Text = "";
                Login.Text = "";
                DateOfBirth.SelectedDate = DateTime.Now;
            }
        }

        private void ToEmployees_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Employees employees = new Employees();
            employees.Show();
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

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

    public partial class Profile : Window
    {
        bool pasChange = false;
        public Profile()
        {
            InitializeComponent();
        }
        private void Label_MouseUpGoToMenu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MenuScreen menuScreen = new MenuScreen();
            menuScreen.Show();
            this.Close();
        }

        private void Label_MouseUpGoToBasket(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Basket basket = new Basket();
            basket.Show();
            this.Close();
        }

        private void Label_MouseUpGoToOrders(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Orders orders = new Orders(CurrentUser.Id);
            orders.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
            {
                Client client = db.Client.Where(x => x.ClientID == CurrentUser.Id).FirstOrDefault();
                LastName.Text = client.LastName;
                FirstName.Text = client.FirstName;
                PhoneNumber.Text = client.PhoneNumber;
                Password.Password = client.Password;
                DateOfBirth.SelectedDate = client.DateOfBirth;
            }
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            pasChange = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
            {
                Client client = db.Client.Where(x => x.ClientID == CurrentUser.Id).FirstOrDefault();
                if (!pasChange || Password.Password == RepeatPassword.Password)
                {
                    client.LastName = LastName.Text;
                    client.FirstName = FirstName.Text;
                    client.PhoneNumber = PhoneNumber.Text;
                    client.Password = Password.Password;
                    client.DateOfBirth = DateOfBirth.SelectedDate;
                    MessageBox.Show("Изменения успешно сохранены");
                    db.SaveChanges();
                }                
                else
                {
                    MessageBox.Show("Повторите новый пароль");
                }
            }
        }
    }
}

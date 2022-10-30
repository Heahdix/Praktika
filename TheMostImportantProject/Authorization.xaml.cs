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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheMostImportantProject
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            bool clientFound = false;
            using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
            {
                foreach(var entity in db.Client)
                {
                    if (PhoneNumber.Text == entity.PhoneNumber)
                    {
                        clientFound = true;
                        if (Password.Password == entity.Password)
                        {
                            CurrentUser.Id = entity.ClientID;
                            MenuScreen menuScreen = new MenuScreen();
                            menuScreen.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Введён неверный пароль");
                        }
                    }                 
                }
            }
            if (!clientFound) 
            {
                MessageBox.Show("Такой номер телефона не зарегестрирован");
            }
        }

        private void PhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                e.Handled = true;
            }
        }
    }
}

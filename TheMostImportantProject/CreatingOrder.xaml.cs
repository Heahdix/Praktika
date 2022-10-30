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
    /// <summary>
    /// Логика взаимодействия для CreatingOrder.xaml
    /// </summary>
    public partial class CreatingOrder : Window
    {
        List<string> hours = new List<string>();
        List<string> minutes = new List<string>();
        public CreatingOrder()
        {
            InitializeComponent();

            for (int i = 0; i < 14; i++)
            {
                hours.Add((i+8).ToString());
            }
            for (int i = 0; i < 60; i++)
            {
                minutes.Add(i.ToString());
            }
            hoursList.ItemsSource = hours;
            minutesList.ItemsSource = minutes;
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

        private void Label_MouseUpGoToProfile(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            decimal sum = BasketItems.Lines.Sum(x => x.Position.Price * x.Quantity);
            SumLabel.Content = "Стоимость заказа: " + String.Format("{0:0.00}", sum) + " Рублей";
        }

        private void RadioButtonTimeSelect_Checked(object sender, RoutedEventArgs e)
        {
            Hiddenable.Visibility = Visibility.Visible;
        }

        private void AsSoon_Checked(object sender, RoutedEventArgs e)
        {
            Hiddenable.Visibility = Visibility.Hidden;
        }

        private void ButtonOrderDone_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new DateTime();
            decimal sum = BasketItems.Lines.Sum(x => x.Position.Price * x.Quantity);
            string status = "Выполняется";
            string address = Address.Text;
            if ((bool)AsSoon.IsChecked)
            {
                date = DateTime.Now.AddHours(1);
                status = "Выполняется";
            }
            else if ((bool)SelectedTime.IsChecked)
            {
                date = (DateTime)DateSelector.SelectedDate;
                date = date.AddHours(hoursList.SelectedIndex + 8);
                date = date.AddMinutes(minutesList.SelectedIndex);
                status = "Ожидает выполнения";
            }
            if (address == "")
            {
                MessageBox.Show("Введите адрес");
            }
            else if (date < DateTime.Now.AddMinutes(50))
            {
                MessageBox.Show("Введите действительное время");
            }
            else
            {
                bool addressPossibilty = false;
                using (Pizza_MozzarellaEntities db = new Pizza_MozzarellaEntities())
                {
                    foreach (var possibleAddress in db.PossibleAddress)
                    {
                        if (address.ToLower().Contains(possibleAddress.Location))
                        {
                            addressPossibilty = true;
                            break;
                        }
                        
                    }
                    if (!addressPossibilty)
                    {
                        MessageBox.Show("На этот адрес не осуществляется доставка");
                    }
                    else
                    {
                        Order order = new Order();
                        order.Sum = sum;
                        order.Date = date;
                        order.Status = status;
                        order.Address = address;
                        order.ClientID = CurrentUser.Id;
                        db.Order.Add(order);
                        db.SaveChanges();

                        int orderId = db.Order.Where(x => x.ClientID == CurrentUser.Id).Max(x => x.OrderID);
                        foreach (var line in BasketItems.Lines)
                        {
                            PositionOrder positionOrder = new PositionOrder();
                            positionOrder.OrderID = orderId;
                            positionOrder.PositionID = line.Position.PositionID;
                            positionOrder.Amount = line.Quantity;
                            db.PositionOrder.Add(positionOrder);
                            db.SaveChanges();
                        }
                        BasketItems.Lines.Clear();
                        Orders orders = new Orders(CurrentUser.Id);
                        orders.Show();
                        this.Close();
                    }
                }
            }
        }

        
    }
}

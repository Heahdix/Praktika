using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        public Basket()
        {
            InitializeComponent();
        }

        private void Label_MouseUpGoToMenu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MenuScreen menuScreen = new MenuScreen();
            menuScreen.Show();
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
            StackPanelLoad();
            UpdateSum();
        }

        public void StackPanelLoad()
        {
            MainPlace.Children.Clear();
            foreach (var position in BasketItems.Lines)

            {
                FillBasket(position);
            }
        }
        public void FillBasket(BasketLine basketLine)
        {
            Border border = new Border();
            DockPanel dockPanel = new DockPanel();
            StackPanel stackPanel = new StackPanel();
            StackPanel stackPanelInner = new StackPanel();
            Image image = new Image();
            Label labelName = new Label();
            Label labelPrice = new Label();
            Button buttonMinus = new Button();
            Button buttonPlus = new Button();
            TextBox TextBoxQuantity = new TextBox();

            DockPanel.SetDock(image, Dock.Left);
            DockPanel.SetDock(labelName, Dock.Left);
            DockPanel.SetDock(stackPanel, Dock.Right);

            dockPanel.LastChildFill = false;

            border.BorderBrush = Brushes.DarkOrange;
            border.Background = Brushes.White;
            border.BorderThickness = new Thickness(2);
            border.Width = 940;
            border.Height = 175;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.Margin = new Thickness(0, 65, 0, 0);

            buttonMinus.Background = Brushes.DarkOrange;
            buttonPlus.Background = Brushes.DarkOrange;

            stackPanel.Orientation = Orientation.Vertical;
            stackPanelInner.Orientation = Orientation.Horizontal;

            image.Width = 175;
            image.Height = 175;

            buttonMinus.Width = 35;
            buttonMinus.Height = 35;
            buttonMinus.Content = "-";

            buttonPlus.Width = 35;
            buttonPlus.Height = 35;
            buttonPlus.Content = "+";

            labelName.FontSize = 32;
            labelPrice.FontSize = 32;

            buttonMinus.FontSize = 24;
            buttonPlus.FontSize = 24;
            TextBoxQuantity.FontSize = 24;
            TextBoxQuantity.Width = 66;
            TextBoxQuantity.Height = 35;

            labelName.Margin = new Thickness(34, 25, 0, 0);
            labelPrice.Margin = new Thickness(0, 25, 0, 0);

            stackPanelInner.Margin = new Thickness(0, 56, 48, 5);

            dockPanel.MinWidth = 940;
            dockPanel.Height = 175;
            dockPanel.HorizontalAlignment = HorizontalAlignment.Center;

            MemoryStream ms = new MemoryStream(basketLine.Position.Image);
            image.Source = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            labelName.Content = basketLine.Position.Name;
            labelPrice.Content = String.Format("{0:0.00}", basketLine.Position.Price) + " Руб";
            TextBoxQuantity.Text = basketLine.Quantity.ToString();
            TextBoxQuantity.HorizontalContentAlignment = HorizontalAlignment.Center;


            stackPanelInner.Children.Add(buttonMinus);
            stackPanelInner.Children.Add(TextBoxQuantity);
            stackPanelInner.Children.Add(buttonPlus);

            stackPanel.Children.Add(labelPrice);
            stackPanel.Children.Add(stackPanelInner);

            dockPanel.Children.Add(image);
            dockPanel.Children.Add(labelName);
            dockPanel.Children.Add(stackPanel);

            border.Child = dockPanel;

            MainPlace.Children.Add(border);

            buttonPlus.Name = $"plus{basketLine.Position.PositionID}";
            buttonMinus.Name = $"minus{basketLine.Position.PositionID}";
            TextBoxQuantity.Name = $"textBox{basketLine.Position.PositionID}";

            buttonPlus.Click += new RoutedEventHandler(QuantityPlus);
            buttonMinus.Click += new RoutedEventHandler(QuantityMinus);
            TextBoxQuantity.LostFocus += new RoutedEventHandler(ChangeQuantity);
        }

        public void UpdateSum()
        {
            decimal sum = BasketItems.Lines.Sum(x => x.Position.Price * x.Quantity);
            Sum.Content = String.Format("{0:0.00}", sum) + " Рублей";
        }

        public void QuantityPlus(object sender, RoutedEventArgs e)
        {
            string idString = (sender as Button).Name.Remove(0, 4);
            int Id = Convert.ToInt32(idString);

            int quant = ++BasketItems.Lines.Where(x => x.Position.PositionID == Id).First().Quantity;
            UpdateSum();
            StackPanelLoad();
            
        }
        public void QuantityMinus(object sender, RoutedEventArgs e)
        {
            string idString = (sender as Button).Name.Remove(0, 5);
            int Id = Convert.ToInt32(idString);
            
            int quant = --BasketItems.Lines.Where(x => x.Position.PositionID == Id).First().Quantity;
            if (quant > 0)
            {
                StackPanelLoad();
            }
            else
            {
                BasketItems.Lines.Remove(BasketItems.Lines.Where(x => x.Position.PositionID == Id).First());
                StackPanelLoad();
            }
            UpdateSum();
            
        }

        public void ChangeQuantity(object sender, RoutedEventArgs e)
        {
            int quantity = Convert.ToInt32((sender as TextBox).Text);
            int Id = Convert.ToInt32((sender as TextBox).Name.Remove(0, 7));
            if (quantity > 0)
            {          
                BasketItems.Lines.Where(x => x.Position.PositionID == Id).First().Quantity = quantity;
            }
            else
            {
                BasketItems.Lines.Remove(BasketItems.Lines.Where(x => x.Position.PositionID == Id).First());
                StackPanelLoad();
            }
            UpdateSum();
        }

        private void ButtonOrderCreate_Click(object sender, RoutedEventArgs e)
        {
            if (BasketItems.Lines.Count > 0) 
            { 
                CreatingOrder creatingOrder = new CreatingOrder();
                creatingOrder.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Добавьте позиции в корзину");
            }
        }
    }
}

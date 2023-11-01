using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MidtermPizzaOrders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new PizzaOrderDbContext(); // Exceptions
                LvOrders.ItemsSource = Globals.dbContext.PizzaOrders.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                // Close();
                Environment.Exit(1);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnuNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var newOrderdlg = new NewOrderDialog();
            newOrderdlg.ShowDialog();
            LvOrders.ItemsSource = Globals.dbContext.PizzaOrders.ToList();

        }

        private void MnuPlaced_Click(object sender, RoutedEventArgs e)
        {
            PizzaOrder pSelected = LvOrders.SelectedItem as PizzaOrder; 
            if (pSelected == null)
            {
                return;
            }
            try
            {
                pSelected.OrderStatus = PizzaOrder.OrderStatusEnum.Placed;
                Globals.dbContext.SaveChanges();
                LvOrders.ItemsSource = Globals.dbContext.PizzaOrders.ToList();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MnuFulFilled_Click(object sender, RoutedEventArgs e)
        {
            PizzaOrder pSelected = LvOrders.SelectedItem as PizzaOrder;
            if (pSelected == null)
            {
                return;
            }
            try
            {
                pSelected.OrderStatus = PizzaOrder.OrderStatusEnum.Fulfilled;
                Globals.dbContext.SaveChanges(); 
                LvOrders.ItemsSource = Globals.dbContext.PizzaOrders.ToList(); 
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PizzaOrder pSelected = LvOrders.SelectedItem as PizzaOrder;
            if (pSelected == null)
            {
                MnuFulFilled.IsEnabled = false;
                MnuPlaced.IsEnabled = false;
            } else
            {
                MnuFulFilled.IsEnabled = true;
                MnuPlaced.IsEnabled = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
           Close();
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
  
            var result = MessageBox.Show("Are you sure you want to quit?", "Confirm quit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);

            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}

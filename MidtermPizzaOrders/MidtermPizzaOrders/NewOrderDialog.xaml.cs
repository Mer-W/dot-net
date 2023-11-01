using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MidtermPizzaOrders
{
    /// <summary>
    /// Interaction logic for NewOrderDialog.xaml
    /// </summary>
    public partial class NewOrderDialog : Window
    {
        public NewOrderDialog()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new PizzaOrderDbContext(); // Exceptions
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        public string NowPlus1hr()
        {
     
            DateTime now = DateTime.Now;
            now.AddHours(1);
            return now.ToString(format: "t");
        }
        private void BtnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TbxName.Text == null || TbxName.Text.Length > 100)
                {
                    throw new ArgumentException("Please enter a name for delivery");

                }
                if (!Regex.IsMatch(TbxPostalCode.Text, @"[A-Za-z]\d[A-Za-z]\s\d[A-Za-z]\d$"))
                {
                    throw new ArgumentException("Please enter a valid postal code (e.g. 'H1H 1A1')");

                }
                if (DateDelivery.SelectedDate == null)
                {
                    throw new ArgumentException("Please select a delivery date");
                }

                List<string> extrasList = new List<string>();
                if (CbxCheese.IsChecked == true)
                {
                    extrasList.Add("ExtraCheese");
                }
                if (CbxDeepDish.IsChecked == true)
                {
                    extrasList.Add("DeepDish");
                }
                if (CbxWholeWheat.IsChecked == true)
                {
                    extrasList.Add("WholeWheat");
                }

                string extras = string.Join<string>(",", extrasList);

                DateTime orderDate = (DateTime)DateDelivery.SelectedDate; //FIXME
                int.TryParse(TimeDeliveryHr.Text, out int hrs);
                int.TryParse(TimeDeliveryMin.Text, out int min);
               orderDate = orderDate.AddHours(hrs);
               orderDate = orderDate.AddMinutes(min);

                if (CmbSize.SelectedIndex == 0) {
                    throw new ArgumentException("Please select a size");
                  
                }
                string sizeTag = ((ComboBoxItem)CmbSize.SelectedItem).Tag.ToString();
                Console.WriteLine(sizeTag);

                PizzaOrder newOrder = new PizzaOrder(TbxName.Text, TbxPostalCode.Text, orderDate,
                    (PizzaOrder.SizeEnum)Int16.Parse(sizeTag),
                                        (int)SldrMinutes.Value,

                    extras);  // ArgumentException
                Globals.dbContext.PizzaOrders.Add(newOrder);
                Globals.dbContext.SaveChanges(); // SystemException

                DialogResult = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Database error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        private void TbxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TbxName.Text == null || TbxName.Text.Length < 1 || TbxName.Text.Length > 100)
            {
                LblErrName.Visibility = Visibility.Visible;

            } else
            {
                LblErrName.Visibility = Visibility.Hidden;

            }
        }

        private void TbxPostalCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TbxPostalCode.Text == null || !Regex.IsMatch(TbxPostalCode.Text, @"[A-Za-z]\d[A-Za-z]\s\d[A-Za-z]\d$"))
            {
                LblErrPostalCode.Visibility = Visibility.Visible;

            }
            else
            {
                LblErrPostalCode.Visibility = Visibility.Hidden;

            }
        }

        private void DateDelivery_LostFocus(object sender, RoutedEventArgs e)
        {

           var now = DateTime.Now;
            var validDate = now.AddMinutes(45);
            
            if (DateDelivery.SelectedDate == null || DateTime.Compare((DateTime)DateDelivery.SelectedDate, validDate) < 0)
            {
                LblErrDelivery.Visibility = Visibility.Visible;

            }
            else
            {
                LblErrDelivery.Visibility = Visibility.Hidden;

            }
        }

        private void TimeDelivery_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TimeDeliveryHr.Text == null || TimeDeliveryMin.Text == null )
            {
                LblErrDelivery.Visibility = Visibility.Visible;

            }
            else
            {
                LblErrDelivery.Visibility = Visibility.Hidden;

            }
        }

        private void CmbSize_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((ComboBoxItem)CmbSize.SelectedItem).Tag  == null)
            {
                LblErrSize.Visibility = Visibility.Visible;

            }
            else
            {
                LblErrSize.Visibility = Visibility.Hidden;

            }
        }
    }
}

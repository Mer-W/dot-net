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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Day03AllInputs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = TbxName.Text;
            if (name == "")
            {
                MessageBox.Show(this, "Name must not be empty", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            LblResult.Content = $"Hello {name}!";

            string age ="";

            var checkedBtn = GridMain.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true && r.GroupName == "RbnAge");
            if (checkedBtn == null)
            {
                MessageBox.Show(this, "Name must not be empty", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (RbnUnder18.IsChecked == true)
            {
                age = "Under 18";
            }

           else if (Rbn18to35.IsChecked == true)
            {
                age = "18-35";
            }
           else if (RbnOver35.IsChecked == true)
            {
                age = "Over 35";
            }
            else
            {
                MessageBox.Show(this, "Please select an age range", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string pets = "";

            if (CbxCat.IsChecked == true)
            {
                pets += "cat; ";
            }
            if (CbxDog.IsChecked == true)
            {
                pets += "dog; ";
            }
            if (CbxOther.IsChecked == true)
            {
                pets += "other; ";
            }

            string continent = CmbContinent.SelectedValue?.ToString();
            if (continent == null) {
                MessageBox.Show(this, "Please select a continent", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            continent = CmbContinent.Text;

            double temp = SldrTemp.Value;

            SaveToFile(new Registrant(name, age, continent, pets, 25));
        }

        static void SaveToFile(Registrant registrant)
        {

            try
            {
                using (StreamWriter file = new StreamWriter(@"..\..\registrant.txt"))
                {
                
                        file.WriteLine(
                            "Name: " + registrant.Name + 
                            "\nAge: " + registrant.Age + 
                            "\nContinent: " + registrant.Continent +
                            "\nPets: " + registrant.Pets +
                            "\nTemperature: " + registrant.Temperature +
                            "\n");
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("File writing exception: " + ex.Message); // + " : " + ex.GetType().FullName);
            }
        }
    }
}

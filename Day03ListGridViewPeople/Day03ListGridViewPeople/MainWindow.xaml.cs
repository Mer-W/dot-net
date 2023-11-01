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
using System.Xml.Linq;

namespace Day03ListGridViewPeople
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string fileName = @"..\..\output.txt";
        List<Person> peopleList = new List<Person>();
        
        public MainWindow()
        {
            InitializeComponent();
            LvPeople.ItemsSource = peopleList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromFile();

        }

        private void BtnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            // if (!ArePersonInputsValid()) return;
            string name = TbxName.Text;
            int.TryParse(TbxAge.Text, out int age); // okay: no need to validate again
            peopleList.Add(new Person(name, age));
            LvPeople.Items.Refresh(); // tell ListView data has changed
            SaveDataToFile();
            ClearInput();

        }

        private void ClearInput()
        {
            TbxName.Text = "";
            TbxAge.Text = "";
        }

        private void BtnDeletePerson_Click(object sender, RoutedEventArgs e)
        {
            Person pSelected = LvPeople.SelectedItem as Person;
            if (pSelected == null) return;
            var result = MessageBox.Show(this, "Are you sure you want to delete this item?\n" + pSelected, "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            peopleList.Remove(pSelected);
            LvPeople.Items.Refresh();
            LvPeople.SelectedIndex = -1;
            ClearInput();

        }
        private void BtnUpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            Person pSelected = LvPeople.SelectedItem as Person;
            if (pSelected == null) return;
            string name = TbxName.Text;
            int.TryParse(TbxAge.Text, out int age);
            pSelected.Age = age;
            pSelected.Name = name;
            LvPeople.Items.Refresh();
            LvPeople.SelectedIndex = -1;
            ClearInput();
        }

        private void LvPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person pSelected = LvPeople.SelectedItem as Person;
            if (pSelected == null)
            {
                ClearInput();
            } else
            {
                TbxAge.Text = pSelected.Age.ToString();
                TbxName.Text = pSelected.Name;
            }
        }



        private void LoadDataFromFile() // call when window is loaded
        {
            // do your best with validation
            // data stored semicolon-separated, one per line:  Jerry;33
            List<string> errList = new List<string>();
            try
            {
                if (!File.Exists(fileName))
                {
                    return; // it's okay if the file does not exist yet
                }
                string[] linesArray = File.ReadAllLines(fileName); // ex IOException, SystemException
                foreach (string line in linesArray)
                {
                    try
                    {
                        string[] data = line.Split(';');
                        if (data.Length != 2)
                        {
                            errList.Add("Invalid number of items");
                            continue;
                        }
                        string name = data[0];
                        int age = int.Parse(data[1]); // ex FormatException
                        Person person = new Person(name, age); // ex ArgumentException
                        peopleList.Add(person);
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentException)
                    {
                        Console.WriteLine($"Error (skipping line): {ex.Message} in:\n  {line}");
                    }
                }
                LvPeople.Items.Refresh();
            }
            // catch (Exception ex) when (ex is IOException || ex is SystemException)
            catch (SystemException ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }

        }

        private void SaveDataToFile() // call when window is closing
        {
            List<string> lines = new List<string>();
            foreach (Person p in peopleList)
            {
                lines.Add($"{p.Name};{p.Age}");
            }
            try
            {
                File.WriteAllLines(fileName, lines); // ex
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error writing to file\n" + ex.Message, "File access error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDataToFile();
        }
    }

}

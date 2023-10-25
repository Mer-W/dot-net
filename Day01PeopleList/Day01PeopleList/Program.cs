using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01PeopleList
{
    internal class Program
    {
        static List<Person> people = new List<Person>();
        static void AddPersonInfo()
        {
            try
            {
                Console.WriteLine("Enter new person name: ");

                string name = Console.ReadLine();

                Console.WriteLine("Enter new person age: ");

                string ageStr = Console.ReadLine();
                int age = int.Parse(ageStr);

                Console.WriteLine("Enter new person city: ");

                string city = Console.ReadLine();

                people.Add(new Person(name, age, city));


            }
            catch (SystemException ex)
            {
                Console.WriteLine("File writing exception: " + ex.Message); // + " : " + ex.GetType().FullName);
            }
        }
        static void ListAllPersonsInfo()
        {
            try
            {
                SaveAllPeopleToFile();

                string[] linesArray = File.ReadAllLines(@"..\..\people.txt");
                foreach (string line in linesArray)
                {
                    Console.WriteLine(line);
                }
            }
            catch (SystemException ex) // (IOException ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
        static void FindPersonByName()
        {
            try

            {
                Console.WriteLine("Enter person name: ");
                string name = Console.ReadLine();
                int i = 0;

                foreach (Person person in people)
                {
                    if (person.Name == name)
                    {
                        i++;
                        Console.WriteLine("Name: " + person.Name + "\nAge: " + person.Age + "\nCity: " + person.City + "\n");
                    }
                }
                if (i == 0)
                {
                    Console.WriteLine("No matches found");
                }
            }
            catch (SystemException ex) // (IOException ex)
            {
                Console.WriteLine("Error reading input: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
        static void FindPersonYoungerThan()
        {
            try

            {
                Console.WriteLine("Enter max age: ");
                string ageStr = Console.ReadLine();
                int age = int.Parse(ageStr);

                int i = 0;

                foreach (Person person in people)
                {
                    if (person.Age < age)
                    {
                        i++;
                        Console.WriteLine("Name: " + person.Name + "\nAge: " + person.Age + "\nCity: " + person.City + "\n");
                    }
                }
                if (i == 0)
                {
                    Console.WriteLine("No matches found");
                }
            }
            catch (SystemException ex) // (IOException ex)
            {
                Console.WriteLine("Error reading input: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        static void ReadAllPeopleFromFile()
        {
            try
            {
                string[] lines = File.ReadAllLines(@"..\..\people.txt");
                for (int i = 0; i < lines.Length-3; i += 4)
                {
                    string name = lines[i].Substring(6);
                    string ageStr = lines[i + 1].Substring(5);
                    int age = int.Parse(ageStr);
                    string city = lines[i + 2].Substring(6);
                    people.Add(new Person(name, age, city));

                }
            }
            catch (SystemException ex) // (IOException ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }

        }
        static void SaveAllPeopleToFile()
        {
            try
            {
                using (StreamWriter file = new StreamWriter(@"..\..\people.txt"))
                {
                    foreach (Person person in people)
                    {
                        file.WriteLine("Name: " + person.Name + "\nAge: " + person.Age + "\nCity: " + person.City + "\n");
                    }
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("File writing exception: " + ex.Message); // + " : " + ex.GetType().FullName);
            }
        }

        private static int getMenuChoice()
        {
            try
            {
                Console.Write("What do you want to do?\n" +
                        "1.Add person info\n" +
                        "2.List persons info\n" +
                        "3.Find and list a person by name\n" +
                        "4.Find and list persons younger than age\n" +
                        "0.Exit\n" + "Choice: ");
                string line = Console.ReadLine();
                int choice = int.Parse(line); // ex
                return choice;
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                return -1;
            }
            // return (int.TryParse(Console.ReadLine(), out int choice)) ? choice : -1;
        }

        //main
        static void Main(string[] args)
      
        {
            ReadAllPeopleFromFile();
            while (true)
            {
                int choice = getMenuChoice();
                switch (choice)
                {
                    case 1:
                        AddPersonInfo();
                        break;
                    case 2:
                        ListAllPersonsInfo();
                        break;
                    case 3:
                        FindPersonByName();
                        break;
                    case 4:
                        FindPersonYoungerThan();
                        break;
                    case 0:
                        SaveAllPeopleToFile();
                        return; // exit the program
                    default:
                        Console.WriteLine("No such option, try again");
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}

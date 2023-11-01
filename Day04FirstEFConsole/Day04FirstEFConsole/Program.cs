using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04FirstEFConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SocietyDBContext db = new SocietyDBContext();
                Random random = new Random();
                Person p1 = new Person { Name = "Jerry " + random.Next(1000), Age = random.Next(100) };
                db.People.Add(p1); // insert operation is scheduled but NOT executed yet
                db.SaveChanges(); // synchronize objects in memory with database
                Console.WriteLine("record added");

                // UPDATE: equivalent of update - fetch then modify, save changes
                // FirstOrDefault will either return Person or null
                Person p2 = (from p in db.People where p.Id == 2 select p).FirstOrDefault<Person>();
                if (p2 != null)
                {
                    p2.Name = "Alabama " + (random.Next(10000) + 10000); // entity framework is watching and notices the modification
                    db.SaveChanges(); // update the database to synchronize entities in memory with the database
                    Console.WriteLine("Record updated");
                }
                else
                {
                    Console.WriteLine("record to update not found");
                }

                // delete - fetch then schedule for deletion, then save changes
                Person p3 = (from p in db.People where p.Id == 3 select p).FirstOrDefault<Person>();
                if (p3 != null)
                { // found the record to delete
                    db.People.Remove(p3); // schedule for deletion in the database
                    db.SaveChanges(); // update the database to synchronize entities in memory with the database
                    Console.WriteLine("Record deleted");
                }
                else
                {
                    Console.WriteLine("record to delete not found");
                }

                // fetch all records
                List<Person> peopleList = (from p in db.People select p).ToList<Person>();
                foreach (Person p in peopleList)
                {
                    Console.WriteLine($"{p.Id}: {p.Name} is {p.Age} y/o");
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine("Database operation failed: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Press any key");
                Console.ReadKey();
            }
        }
    }
}


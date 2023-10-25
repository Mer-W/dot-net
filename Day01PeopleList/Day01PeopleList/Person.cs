using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01PeopleList
{
    public class Person
    {
        public string Name // Name 2-100 characters long, not containing semicolons
        { get; set; }

        public int Age // Age 0-150
        { get; set; }

        public string City // City 2-100 characters long, not containing semicolons
        { get; set; }

        public Person(string name, int age, string city)
        {
            Name = name;
            Age = age;
            City = city;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03AllInputs
{
    public class Registrant
    {
        public string Name 
        { get; set; }

        public string Age 
        { get; set; }

        public string Continent
        { get; set; }

        public string Pets { get; set; }

        public int Temperature { get; set; }

        public Registrant(string name, string age, string continent, string pets, int temperature)
        {
            Name = name;
            Age = age;
            Continent = continent;
            Pets = pets;
            Temperature = temperature;
        }
    }
}

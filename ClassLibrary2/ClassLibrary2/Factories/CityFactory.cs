using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public static class CityFactory
    {
        private static int idCounter = 0;

        private static void IncrementIdCounter() { idCounter++; }

        public static City MakeCity()
        {
            City city = new City(idCounter);
            IncrementIdCounter();
            return city;
        }
    }
}

﻿using System;
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

        public static City MakeCity(int X, int Y)
        {
            City city = new City(X, Y, idCounter);
            IncrementIdCounter();
            return city;
        }
    }
}

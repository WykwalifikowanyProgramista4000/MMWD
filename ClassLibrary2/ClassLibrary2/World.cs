using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public static class World
    {
        

        public static List<City> cities = new List<City>();

        public static double[,] distanceCityToCityMatrix;

        //public static void EvaluateDistancesMatrix()
        //{
        //    distanceCityToCityMatrix = new double[cities.Count, cities.Count];
        //    FillWithNaN();

        //    foreach (City city in cities)
        //    {
        //        foreach(City connectedCity in city.GetConnectedCities())
        //        {
        //            distanceCityToCityMatrix[city.GetID(), connectedCity.GetID()] = distanceCityToCityMatrix[connectedCity.GetID(), city.GetID()] = EvaluateDistance(city, connectedCity);
        //        }
        //    }
        //}

        private static void FillWithNaN()
        {
            for (int j = 0; j < World.cities.Count; j++)
            {
                for (int i = 0; i < World.cities.Count; i++)
                {
                    distanceCityToCityMatrix[j, i] = Double.NaN;
                }
            }
        }

        private static double EvaluateDistance(City cityA, City cityB)
        {
            return Math.Sqrt(
                Math.Pow(cityA.X - cityB.X, 2) +
                Math.Pow(cityA.Y - cityB.Y, 2)
                );
        }

        public static void AddCity(int X, int Y)    // temporary
        {
            cities.Add(CityFactory.MakeCity(X, Y));
            //EvaluateDistancesMatrix();
        }
    }
}

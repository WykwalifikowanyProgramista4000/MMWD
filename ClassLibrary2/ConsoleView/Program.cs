using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm;

namespace ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {

            World.AddCity(0, 0);
            World.AddCity(2, 2);
            World.AddCity(4, 4);

            World.cities[0].AddConnectedCities(new List<int>() {1,1,2});

            //World.EvaluateDistancesMatrix(); //Temporary disabled - the way connected cities are stored has been changed

            //PrintMatrix(World.distanceCityToCityMatrix);

            Console.ReadLine();





        }

        public static void PrintMatrix(double[,] Matrix)
        {
            for (int j = 0; j < World.cities.Count; j++)
            {
                Console.Write("[");
                for (int i = 0; i < World.cities.Count; i++)
                {
                    Console.Write(" " + Matrix[j,i] + " ");
                }
                Console.WriteLine("]");
            }
                
        }
    }
}

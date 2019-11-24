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
            World.AddCities(5);

            #region Connections

            World.AddConnection(0, 1, 11);
            World.AddConnection(0, 2, 11);
            World.AddConnection(0, 3, 11);
            World.AddConnection(0, 4, 11);

            World.AddConnection(1, 0, 22);
            World.AddConnection(1, 2, 22);
            World.AddConnection(1, 3, 22);
            World.AddConnection(1, 4, 22);

            World.AddConnection(2, 0, 33);
            World.AddConnection(2, 1, 33);
            World.AddConnection(2, 3, 33);
            World.AddConnection(2, 4, 33);

            World.AddConnection(3, 0, 44);
            World.AddConnection(3, 1, 44);
            World.AddConnection(3, 2, 44);
            World.AddConnection(3, 4, 44);

            World.AddConnection(4, 0, 55);
            World.AddConnection(4, 1, 55);
            World.AddConnection(4, 2, 55);
            World.AddConnection(4, 3, 55);

            #endregion

            #region Contracts

            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 200, 20, 2000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 300, 30, 3000));

            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(0, 110, 10, 1011));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(3, 330, 33, 3033));

            #endregion

            #region Locomotive start parameters

            Locomotive.TargetLocationID = 4;
            Locomotive.CurrentLocationID = 0;

            Locomotive.SignContractByID(0);
            Locomotive.SignContractByID(1);

            #endregion

            ShowMatrix();
            ShowContracts();
            ShowLocomotive();

            Console.ReadLine();
        }

        public static void ShowMatrix()
        {
            for (int j = 0; j < World.Cities.Count; j++)
            {
                Console.Write("[");
                for (int i = 0; i < World.Cities.Count; i++)
                {
                    Console.Write(" " + World.CTCMatrix[j,i] + " ");
                }
                Console.WriteLine("]");
            }
                
        }

        public static void ShowContracts()
        {
            Console.WriteLine("Contracts:");

            foreach(City city in World.Cities)
            {
                foreach(DelieveryContract contract in city.DelieveryContracts)
                {
                    Console.WriteLine("\t" +
                        "ID: " + contract.ID +
                        "Source city: " + city.ID +
                        ", target city: " + contract.TargetCityID +
                        ", payment: " + contract.Payment +
                        ", waggon count: " + contract.WaggonCount +
                        ", total delievery weight: " + contract.TotalWeight);
                }

                Console.WriteLine();
            }
        }

        public static void ShowLocomotive()
        {
            Console.WriteLine("Locomotive status:");
            Console.WriteLine("\t" + "Current location id: " + Locomotive.CurrentLocationID);
            Console.WriteLine("\t" + "Ultimate target location id: " + Locomotive.TargetLocationID);
            Console.WriteLine("\t" + "Cash: " + Locomotive.Cash);

            Console.WriteLine("Signed contracts: ");
            foreach(DelieveryContract contract in Locomotive.DelieveryContracts)
            {
                Console.WriteLine("\t" +
                    "ID: " + contract.ID +
                    ", target city: " + contract.TargetCityID +
                    ", payment: " + contract.Payment +
                    ", waggon count: " + contract.WaggonCount +
                    ", total delievery weight: " + contract.TotalWeight);
            }

        }
    }
}

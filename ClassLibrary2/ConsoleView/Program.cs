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
            #region Cities

            World.AddCities(5);

            #endregion

            #region Connections

            World.AddConnection(0, 1, 10);
            World.AddConnection(0, 2, 14);
            World.AddConnection(0, 3, 55);    
            World.AddConnection(0, 4, 40);

            World.AddConnection(1, 0, 10);
            World.AddConnection(1, 2, 20);
            World.AddConnection(1, 3, 30);
            World.AddConnection(1, 4, 47);

            World.AddConnection(2, 1, 22);
            World.AddConnection(2, 3, 30);
            World.AddConnection(2, 4, 44);

            World.AddConnection(3, 1, 30);
            World.AddConnection(3, 2, 22);
            World.AddConnection(3, 4, 40);

            World.AddConnection(4, 0, 50);
            World.AddConnection(4, 2, 33);

            #endregion

            #region Contracts

            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 200, 20, 2000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 300, 30, 3000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 200, 20, 2000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 300, 30, 3000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 200, 20, 2000));
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 300, 30, 3000));

            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));

            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(1, 330, 44, 2242));
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(0, 320, 14, 1242));
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(0, 330, 44, 2242));
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(2, 320, 14, 1242));
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(2, 330, 44, 2242));
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(2, 320, 14, 1242));

            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));

            World.AddContractToCity(4, DelieveryContractFactory.CreateContract(0, 242, 15, 9349));
            World.AddContractToCity(4, DelieveryContractFactory.CreateContract(2, 782, 73, 9949));

            #endregion

            #region Locomotive start parameters

            Locomotive.TargetLocationID = 4;
            Locomotive.CurrentLocationID = 0;

            Locomotive.SignContractByID(0);

            #endregion

            #region Algorythm parameters

            AlgorithmImplementation.MaxCityVisited = 4;
            AlgorithmImplementation.Seed = 3;

            AlgorithmImplementation.MainLoopTemperature = 20;
            AlgorithmImplementation.ContractLoopTemperature = 20;

            AlgorithmImplementation.MainLoopAlpha = 0.98;
            AlgorithmImplementation.ContractLoopAlpha = 0.98;

            #endregion

            ShowMatrix();
            ShowContracts();
            ShowLocomotive();

            AlgorithmImplementation.Solve();

            #region Sandbox

            //List<KeyValuePair<int, double>> somekindofnigger = new List<KeyValuePair<int, double>>() { new KeyValuePair<int, double>(1, 12.3) };

            //Console.WriteLine("\nklucz: " + somekindofnigger[0].Key + ", wartość: " + somekindofnigger[0].Value);


        #endregion

        Console.ReadLine();
        }

        #region Methods

        public static void ShowMatrix()
        {
            Console.WriteLine("\n Adjacency matrix: ");
            for (int j = 0; j < World.Cities.Count; j++)
            {
                Console.Write("\t[");
                for (int i = 0; i < World.Cities.Count; i++)
                {
                    Console.Write(" " + World.CTCMatrix[j,i] + " ");
                }
                Console.Write("]\n");
            }
                
        }

        public static void ShowContracts()
        {
            Console.WriteLine("\n Contracts:");

            foreach(City city in World.Cities)
            {
                foreach(DelieveryContract contract in city.DelieveryContracts)
                {
                    Console.WriteLine("\t" +
                        "ID: " + contract.ID +
                        " Source city: " + city.ID +
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
            Console.WriteLine("\n Locomotive status:");
            Console.WriteLine("\t" + "Current location id: " + Locomotive.CurrentLocationID);
            Console.WriteLine("\t" + "Ultimate target location id: " + Locomotive.TargetLocationID);
            Console.WriteLine("\t" + "Cash: " + Locomotive.Cash);

            Console.WriteLine(" Signed contracts: ");
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

        #endregion
    }
}

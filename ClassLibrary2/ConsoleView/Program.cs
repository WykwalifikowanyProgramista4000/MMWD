using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm;
using Algorithm.DataIO;

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

            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));  // 0
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 100, 20, 2000));  // 1
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 100, 30, 3000));  // 2
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));  // 3
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 100, 20, 2000));  // 4
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 300, 30, 3000));  // 5
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(1, 100, 10, 1000));  // 6
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(2, 100, 20, 2000));  // 7
            World.AddContractToCity(0, DelieveryContractFactory.CreateContract(3, 300, 30, 3000));  // 8

            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));  // 9
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));  // 10
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));  // 11
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 100, 23, 2022));  // 12
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));  // 13
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 100, 23, 2022));  // 14
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(4, 110, 10, 1011));  // 15
            World.AddContractToCity(1, DelieveryContractFactory.CreateContract(2, 220, 23, 2022));  // 16

            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(1, 330, 44, 2242));  // 17
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(0, 320, 14, 1242));  // 18 
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(0, 330, 44, 2242));  // 19
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(2, 320, 14, 1242));  // 20
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(2, 330, 44, 2242));  // 21
            World.AddContractToCity(2, DelieveryContractFactory.CreateContract(2, 320, 14, 1242));  // 22
                                                                                                       
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 23
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));  // 24
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 25
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 26
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 27
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 28
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 29
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(4, 222, 12, 1234));  // 30
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));  // 31
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));  // 32
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));  // 33
            World.AddContractToCity(3, DelieveryContractFactory.CreateContract(2, 212, 33, 1554));  // 34
                                                                                                       
            World.AddContractToCity(4, DelieveryContractFactory.CreateContract(0, 242, 15, 9349));  // 35
            World.AddContractToCity(4, DelieveryContractFactory.CreateContract(2, 782, 73, 9949));  // 36

            #endregion

            #region Locomotive start parameters

            Locomotive.CurrentLocationID = 0;

            Locomotive.MaxWeight = 15_000;
            Locomotive.MaxWaggonCount = 112;

            #endregion

            #region Data output 

            DataOutput.OutputPath = "C:\\Users\\wojte\\Documents\\dupa.txt";

            #endregion

            #region Algorythm parameters

            AlgorithmImplementation.MaxCityVisited = 4;
            AlgorithmImplementation.Seed = 69;

            AlgorithmImplementation.MainLoopTemperature = 100;
            AlgorithmImplementation.ContractLoopTemperature = 100;

            AlgorithmImplementation.MainLoopAlpha = 0.98;
            AlgorithmImplementation.ContractLoopAlpha = 0.98;

            #endregion

            ShowMatrix();
            ShowContracts();
            ShowLocomotive();

            // ============================== The M A G I C

            AlgorithmImplementation.Solve();

            // ==============================

            #region Sandbox




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
            Console.WriteLine("\t" + "Cash: " + Locomotive.NewCash);

            Console.WriteLine(" Signed contracts: ");


        }

        #endregion
    }
}

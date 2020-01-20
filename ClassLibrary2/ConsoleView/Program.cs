using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm;
using Algorithm.DataIO;
using System.Windows.Forms;

namespace ConsoleView
{
    public static class StringExtensions
    {
        public static string CenterString(this string stringToCenter, int totalLength)
        {
            return stringToCenter.PadLeft(((totalLength - stringToCenter.Length) / 2)
                                + stringToCenter.Length)
                       .PadRight(totalLength);
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Press 'Enter' to skip test case selection, and start default test case\n" +
                              "Press 'F' to start test case selection");
            ConsoleKeyInfo key = Console.ReadKey();
            if(key.Key == ConsoleKey.F)
            {
                //LOADING
                Console.Write("\nPress 'Enter' to select test scenario file");
                Console.ReadLine();

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataInput.testScenarioFilePath = openFileDialog.FileName;
                }
                Console.Write("Selected test scenario: " + DataInput.testScenarioFilePath + "\n\n");
                DataInput.LoadTestScenario();

                //SAVEING
                Console.Write("Press 'Enter' to select save location for current test run");
                Console.ReadLine();

                FolderBrowserDialog b = new FolderBrowserDialog();
                if (b.ShowDialog() == DialogResult.OK)
                {
                    DataOutput.OutputPath = b.SelectedPath;
                }
                Console.Write("Selected save location: " + DataOutput.OutputPath + "\n\n");

                ShowMatrix();
            }
            else if(key.Key == ConsoleKey.S || key.Key == ConsoleKey.Enter)
            {
                #region preparationes

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
                /*
                0;1;10;
                0;2;14;
                0;3;55;
                0;4;40;
                1;0;10;
                1;2;20;
                1;3;30;
                1;4;47;
                2;1;22;
                2;3;30;
                2;4;44;
                3;1;30;
                3;2;22;
                3;4;40;
                4;0;50;
                4;2;33;
                */
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
                World.AddContractToCity(2, DelieveryContractFactory.CreateContract(4, 320, 14, 1242));  // 20
                World.AddContractToCity(2, DelieveryContractFactory.CreateContract(3, 330, 44, 2242));  // 21
                World.AddContractToCity(2, DelieveryContractFactory.CreateContract(3, 320, 14, 1242));  // 22

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

                /*
                0;1;100;10;1000;
                0;2;100;20;2000;
                0;3;100;30;3000;
                0;1;100;10;1000;
                0;2;100;20;2000;
                0;3;300;30;3000;
                0;1;100;10;1000;
                0;2;100;20;2000;
                0;3;300;30;3000;
                1;4;110;10;1011;
                1;2;220;23;2022;
                1;4;110;10;1011;
                1;2;100;23;2022;
                1;4;110;10;1011;
                1;2;100;23;2022;
                1;4;110;10;1011;
                1;2;220;23;2022;
                2;1;330;44;2242;
                2;0;320;14;1242;
                2;0;330;44;2242;
                2;2;320;14;1242;
                2;2;330;44;2242;
                2;2;320;14;1242;
                3;4;222;12;1234;
                3;2;212;33;1554;
                3;4;222;12;1234;
                3;4;222;12;1234;
                3;4;222;12;1234;
                3;4;222;12;1234;
                3;4;222;12;1234;
                3;4;222;12;1234;
                3;2;212;33;1554;
                3;2;212;33;1554;
                3;2;212;33;1554;
                3;2;212;33;1554;
                4;0;242;15;9349;
                4;2;782;73;9949;
                */

                #endregion

                DataInput.PrepCityAndContractCounters();

                #region Locomotive start parameters

                Locomotive.CurrentLocationID = 0;

                Locomotive.MaxWeight = 15_000;
                Locomotive.MaxWaggonCount = 112;

                #endregion

                #region Data output 

                DataOutput.OutputPath = AppDomain.CurrentDomain.BaseDirectory;
                DataOutput.SaveFileName = "default_test_plan";

                #endregion

                #region Algorythm parameters

                AlgorithmImplementation.MaxCityVisited = 4;
                AlgorithmImplementation.Seed = 69;

                AlgorithmImplementation.MainLoopTemperature = 100;
                AlgorithmImplementation.ContractLoopTemperature = 100;

                AlgorithmImplementation.MainLoopAlpha = 0.8;
                AlgorithmImplementation.ContractLoopAlpha = 0.8;

                #endregion

                #endregion

                ShowContracts();
                ShowMatrix();

                Console.WriteLine("Test plan data will be saved at " + DataOutput.OutputPath + "\n");

                
            }

            

            Console.WriteLine("Press 'Enter' to start the algorithm");
            Console.ReadLine();

            // ============================== The M A G I C

            AlgorithmImplementation.Solve();

            // ==============================

            Console.ReadLine();
        }

        #region Methods

        public static void ShowMatrix()
        {
            Console.WriteLine("\n Adjacency matrix: ");
            Console.Write("\t    ");
            for (int i = 0; i < World.Cities.Count; i++)
            {
                Console.Write(String.Format("{0}", i).CenterString(5));
            }
            Console.Write("\n");

            for (int j = 0; j < World.Cities.Count; j++)
            {
                Console.Write("\t{0}[", String.Format("{0}",j).CenterString(3));
                for (int i = 0; i < World.Cities.Count; i++)
                {
                    Console.Write(String.Format(" " + World.CTCMatrix[j,i] + " ").CenterString(5));
                }
                Console.Write("]\n");
            }
            Console.WriteLine("");   
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

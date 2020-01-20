using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DataIO
{
    public static class DataOutput
    {
        #region Params

        private static List<int> _cityTimesVisited = new List<int>();

        private static List<int> _contractTimesTaken = new List<int>();

        private static DateTime testDate = DateTime.Now;

        private static string _line = "";

        #region Gaj request 
        private static List<double> _solutionVector = new List<double>();
        #endregion

        private static System.IO.StreamWriter _streamWriter;

        #endregion // Params

        #region Get/Set

        public static List<int> CityTimesVisited
        {
            get { return _cityTimesVisited; }
            set { _cityTimesVisited = value; }
        }
        public static List<int> ContractTimesTaken
        {
            get { return _contractTimesTaken; }
            set { _contractTimesTaken = value; }
        }
        public static string OutputPath { get; set; }
        public static string SaveFileName { get; set; }

        #endregion // Get/Set

        #region Methods

        public static void SaveData(int iteration, List<int> path, List<List<DelieveryContract>> contractFlowingList, 
                                    double currentBestValue, double currentValue, double temperature)
        { 
            #region Prepering iteration log string

            //iteration
            _line = String.Format("iteration:{0};\n", iteration);

            //path
            _line += String.Format("path");
            foreach(int cityID in path) { _line += String.Format(":{0}", cityID); }
            _line += ";\n";

            //flowinglist
            _line += "flowing_list:\n";
            for ( int i = 0; i < path.Count; i++)
            {
                _line += String.Format("{0}:", path[i]);
                foreach( DelieveryContract contract in contractFlowingList[i])
                {
                    _line += String.Format(":{0}", contract.ID);
                }
                _line += ";\n";
            }

            //best value
            _line += String.Format("best_value:{0};\n", currentBestValue);

            //current value
            _line += String.Format("current_value:{0};\n", currentValue);
            _solutionVector.Add(currentValue);  //Gaju 

            //temperature
            _line += String.Format("temperature:{0};\n", temperature.ToString("F"));

            _line += "======================================\n";

            #endregion

            using (_streamWriter = new System.IO.StreamWriter(OutputPath + "//" + SaveFileName + String.Format("_{0}{1}{2}", testDate.Hour, testDate.Minute, testDate.Second) + ".txt", true))
            {
                _streamWriter.WriteLine(_line);
            }

            IncrementCounters();
        }

        public static void SaveTheBestSolution(int theBestIteration, double theBestCash, List<int> bestCompleatedContractsIDs, List<int> bestCityRoute,
                                               List<List<DelieveryContract>> theBestFlowingContractsList, List<Status> theBestFlowingStatusList)
        {
            //iteration
            _line = String.Format("iteration:{0};\n", theBestIteration);

            //best value
            _line += String.Format("best_value:{0};\n", theBestCash);

            //path
            _line += String.Format("path");
            foreach (int cityID in bestCityRoute) { _line += String.Format(":{0}", cityID); }
            _line += ";\n";

            //compleated contracts
            _line += String.Format("contracts");
            foreach (int contractID in bestCompleatedContractsIDs) { _line += String.Format(":{0}", contractID); }
            _line += ";\n";

            Console.WriteLine("\nFINAL SOLUTION:\n\n" + _line);

            //flowinglist
            _line += "flowing_contract_list:\n";
            for (int i = 0; i < bestCityRoute.Count; i++)
            {
                _line += String.Format("{0}:", i);
                foreach (DelieveryContract contract in theBestFlowingContractsList[i])
                {
                    _line += String.Format(":{0}", contract.ID);
                }
                _line += ";\n";
            }

            _line += "flowing_status_list:\n";
            for (int i = 0; i < theBestFlowingStatusList.Count; i++)
            {
                _line += String.Format("{0}:-> weight:{0}, wagon_count:{1}\n", bestCityRoute[i], theBestFlowingStatusList[i].Weight, theBestFlowingStatusList[i].WaggonCount);
            }

            using (_streamWriter = new System.IO.StreamWriter(OutputPath + "//" + SaveFileName + String.Format("_{0}{1}{2}", testDate.Hour, testDate.Minute, testDate.Second) + ".txt", true))
            {
                _streamWriter.WriteLine("Optimal solution:");
                _streamWriter.WriteLine(_line + "\n");
            }

            using (_streamWriter = new System.IO.StreamWriter(OutputPath + "//" + SaveFileName + "_solutionVector" + String.Format("_{0}{1}{2}", testDate.Hour, testDate.Minute, testDate.Second) + ".txt", true)) // Gaju 
            {
                foreach (double value in _solutionVector)
                {
                    _streamWriter.WriteLine(value);
                }
            }
        }

        public static void SaveSigmaBest(double sigmaBestSolution, int sigmaStuff)
        {
            //  sigmaStuff = 1  ->  found truely better solution 
            //  sigmaStuff = 0  ->  the solution stays the same, sigma did nothing 
            //  sigmaStuff = -1 ->  sigma did its magic, best solution is now worst then before 

            using (_streamWriter = new System.IO.StreamWriter(OutputPath + "//" + SaveFileName  + "_sigmaBestVector" + String.Format("_{0}{1}{2}", testDate.Hour, testDate.Minute, testDate.Second)  + ".txt", true)) // Gaju 
            {
                _streamWriter.WriteLine(sigmaBestSolution + " ; " + sigmaStuff);
            }
        }

        public static void SaveCounters(long elapsedTime)
        {
            using (_streamWriter = new System.IO.StreamWriter(OutputPath + "//" + SaveFileName + String.Format("_{0}{1}{2}", testDate.Hour, testDate.Minute, testDate.Second) + ".txt", true))
            {
                _streamWriter.WriteLine(String.Format("Finding optimal solution took: {0}ms\n", elapsedTime));

                _streamWriter.WriteLine("Number of times city was taken into solution:");
                for (int i = 0; i < CityTimesVisited.Count; i++)
                {
                    _streamWriter.WriteLine(String.Format("{0}: {1}", i, CityTimesVisited[i]));
                }

                _streamWriter.WriteLine("\n\nNumber of times contract was taken into solution:");
                for (int i = 0; i < ContractTimesTaken.Count; i++)
                {
                    _streamWriter.WriteLine(String.Format("{0}: {1}", i, ContractTimesTaken[i]));
                }
            }
        }

        private static void IncrementCounters()
        {
            foreach (int cityID in Locomotive.NewCityRoute)
            {
                CityTimesVisited[cityID]++;
            }

            foreach (int contractID in Locomotive.NewCompleatedContractsIDs)
            {
                ContractTimesTaken[contractID]++;
            }
        }

        #endregion //Methods
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithm.DataIO
{
    public static class DataInput
    {
        public static string testScenarioFilePath;

        private static StreamReader _file;

        public static void LoadTestScenario()
        {
            if (testScenarioFilePath == null) return;

            using (StreamReader file = new StreamReader(testScenarioFilePath))
            {
                string line = file.ReadLine();
                if(line != "[test_scenario_start]")
                {
                    throw new FileNotFoundException("This is not a test scenario file", testScenarioFilePath);
                }

                while ((line = file.ReadLine()) != "[test_scenario_end]")
                {
                    Console.WriteLine(line);

                    if(line != null)
                    {
                        string[] data = line.Split(':');

                        switch (data[0])
                        {
                            case "test_name":
                                DataOutput.SaveFileName = data[1];
                                break;

                            case "start_locomotive_location":
                                Locomotive.CurrentLocationID = Convert.ToInt32(data[1]);
                                break;

                            case "max_carried_weight":
                                Locomotive.MaxWeight = Convert.ToDouble(data[1]);
                                break;

                            case "max_waggon_count":
                                Locomotive.MaxWaggonCount = Convert.ToInt32(data[1]);
                                break;

                            case "max_city_jumps":
                                AlgorithmImplementation.MaxCityVisited = Convert.ToInt32(data[1]);
                                break;

                            case "algorithm_seed":
                                AlgorithmImplementation.Seed = Convert.ToInt32(data[1]);
                                break;

                            case "city_route_loop_temperature":
                                AlgorithmImplementation.MainLoopTemperature = Convert.ToDouble(data[1]);
                                break;

                            case "city_route_loop_alpha":
                                AlgorithmImplementation.MainLoopAlpha = Convert.ToDouble(data[1]);
                                break;

                            case "contract_loop_temperature":
                                AlgorithmImplementation.ContractLoopTemperature = Convert.ToDouble(data[1]);
                                break;

                            case "contract_loop_alpha":
                                AlgorithmImplementation.ContractLoopAlpha = Convert.ToDouble(data[1]);
                                break;

                            case "number_of_cities":
                                World.AddCities(Convert.ToInt32(data[1]));
                                break;

                            case "cities_connections_start":
                                while((line = file.ReadLine()) != "[start]") { if (line == "[test_scenario_end]") return; }
                                while((line = file.ReadLine()) != "[end]")
                                {
                                    if (line == "[test_scenario_end]") return;
                                    if (line == null || line == "" || line.Contains("|")) continue;

                                    string[] values = line.Split(';');
                                    World.AddConnection(
                                        Convert.ToInt32(values[0]),
                                        Convert.ToInt32(values[1]),
                                        Convert.ToDouble(values[2]));
                                }
                                break;

                            case "contracts":
                                while ((line = file.ReadLine()) != "[start]") { if (line == "[test_scenario_end]") return; }
                                while ((line = file.ReadLine()) != "[end]")
                                {
                                    if (line == "[test_scenario_end]") return;
                                    if (line == null || line == "" || line.Contains("|")) continue;

                                    string[] values = line.Split(';');
                                    World.AddContractToCity(
                                        Convert.ToInt32(values[0]),
                                        DelieveryContractFactory.CreateContract(
                                            Convert.ToInt32(values[1]),
                                            Convert.ToDouble(values[2]),
                                            Convert.ToInt32(values[3]),
                                            Convert.ToDouble(values[4])));
                                }
                                break;
                        }
                    }

                }

                file.Close();
            }

            PrepCityAndContractCounters();
        }

        public static void PrepCityAndContractCounters()
        {
            foreach (var city in World.Cities)
            {
                DataOutput.CityTimesVisited.Add(0);
            }

            foreach (var contract in World.Contracts)
            {
                DataOutput.ContractTimesTaken.Add(0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.DataIO;

namespace Algorithm
{
    public static class AlgorithmImplementation
    {
        #region Parameters

        private static int _maxCityJumps;
        private static bool _maxCityJumpsSetFlag = false;

        private static double _mainLoopTemperature;
        private static bool _mainLoopTemperatureSetFlag = false;

        private static double _contractLoopTemperature;
        private static bool _contractLoopTemperatureSetFlag = false;

        private static double _mainLoopAlpha;
        private static bool _mainLoopAlphaSetFlag = false;

        private static double _contractLoopAlpha;
        private static bool _contractLoopAlphaSetFlag = false;

        private static double _bestSolutionValue = -1;

        private static List<int> _citiesToVisitID = new List<int>();
        private static List<List<int>> _contractsToSignID = new List<List<int>>();

        private static Random _random = new Random();

        #endregion

        #region Get/Set

        public static int MaxCityVisited
        {
            get { return _maxCityJumps; }
            set
            {
                if(_maxCityJumpsSetFlag == false)
                {
                    _maxCityJumps = value;
                    _maxCityJumpsSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Max cities to visit bound has been already set");
                }
            } 
        }


        public static double MainLoopTemperature
        {
            get { return _mainLoopTemperature; }
            set
            {
                if(_mainLoopTemperatureSetFlag == false)
                {
                    _mainLoopTemperature = value;
                    _mainLoopTemperatureSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Start main temperature has been already set");
                }
            }
        }

        public static double ContractLoopTemperature
        {
            get { return _contractLoopTemperature; }
            set
            {
                if(_contractLoopTemperatureSetFlag == false)
                {
                    _contractLoopTemperature = value;
                    _contractLoopTemperatureSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Start contract temperature has been already set");
                }
            }
        }

        public static double MainLoopAlpha
        {
            get { return _mainLoopAlpha; }
            set
            {
                if (_mainLoopAlphaSetFlag == false)
                {
                    _mainLoopAlpha = value;
                    _mainLoopAlphaSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Main Alpha has been already set");
                }
            }
        }

        public static double ContractLoopAlpha
        {
            get { return _contractLoopAlpha; }
            set
            {
                if (_contractLoopAlphaSetFlag == false)
                {
                    _contractLoopAlpha = value;
                    _contractLoopAlphaSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Contract Alpha has been already set");
                }
            }
        }

        public static List<int> CitiesToVisitIDVector
        {
            get { return CitiesToVisitIDVector; }
        }

        public static int Seed
        {
            set { _random = new Random(value); }
        }


        #endregion

        #region Methods

        #region Main Solve

        public static void Solve()
        {
            Stopwatch algorithmTimer = Stopwatch.StartNew();
            double temperature = _mainLoopTemperature;

            Locomotive.BetterFlowingContractsList = new List<List<DelieveryContract>>();
            Locomotive.BetterFlowingStatusList = new List<Status>();

            for( int i = 0; i <= MaxCityVisited; i++)
            {
                Locomotive.BetterFlowingContractsList.Add(new List<DelieveryContract>());
                Locomotive.BetterFlowingStatusList.Add(new Status(0,0));
            }

            List<int> bestCityRoute = GenerateTemplateCityRoute();
            List<int> newCityRoute;

            List<List<int>> bestContractSet = FindBestContractsSet(bestCityRoute);
            List<List<int>> newContractSet;

            double bestSolutionValue = CalculateSolutionValue(bestContractSet);
            double newSolutionValue;

            while(temperature > 0.001)
            {
                newCityRoute = GenerateNextCityRoute(bestCityRoute);
                newContractSet = FindBestContractsSet(newCityRoute);
                newSolutionValue = CalculateSolutionValue(newContractSet);

                if(newSolutionValue > bestSolutionValue)
                {
                    bestSolutionValue = newSolutionValue;
                    bestContractSet = newContractSet;
                    bestSolutionValue = newSolutionValue;
                    Locomotive.TheBestCityRoute = new List<int>(Locomotive.NewCityRoute);
                    Locomotive.TheBestCompleatedContractsIDs = new List<int>(Locomotive.TheBestCompleatedContractsIDs);
                    Locomotive.TheBestFlowingContractsList = new List<List<DelieveryContract>>(Locomotive.TheBestFlowingContractsList);
                    Locomotive.TheBestFlowingStatusList = new List<Status>(Locomotive.TheBestFlowingStatusList);
                }
                else if(_random.NextDouble() > Math.Pow(Math.E, (bestSolutionValue - newSolutionValue) / temperature))
                {
                    bestSolutionValue = newSolutionValue;
                    bestContractSet = newContractSet;
                    bestSolutionValue = newSolutionValue;
                    Locomotive.TheBestCityRoute = new List<int>(Locomotive.NewCityRoute);
                    Locomotive.TheBestCompleatedContractsIDs = new List<int>(Locomotive.TheBestCompleatedContractsIDs);
                    Locomotive.TheBestFlowingContractsList = new List<List<DelieveryContract>>(Locomotive.TheBestFlowingContractsList);
                    Locomotive.TheBestFlowingStatusList = new List<Status>(Locomotive.TheBestFlowingStatusList);
                }

                temperature *= _mainLoopAlpha;
                DataIO.DataOutput.SaveData(temperature, newSolutionValue, bestSolutionValue);
            }

            _bestSolutionValue = bestSolutionValue;
            _citiesToVisitID = bestCityRoute;
            _contractsToSignID = bestContractSet;

            algorithmTimer.Stop();
            Console.WriteLine("\nFinal solution value: " + _bestSolutionValue);
            Console.WriteLine("\nSolution took: " + algorithmTimer.ElapsedMilliseconds + " miliseconds");
        }

        #endregion

        #region Utilities

        private static double CalculateSolutionValue(List<List<int>> solution)
        {
            //double cashEarned = 0;

            //foreach(List<int> contractsIDs in solution)
            //{
            //    foreach(int contractID in contractsIDs)
            //    {
            //        foreach(City city in World.Cities)
            //        {
            //            DelieveryContract contract = city.GetDelieveryContractByID(contractID);
            //            if(contract != null)
            //            {
            //                cashEarned += contract.Payment;
            //            }
            //        }
            //    }
            //}

            Locomotive.EvalCash(SolutionType.New);

            Console.WriteLine("\n\tThis solution value is: " + Locomotive.NewCash);

            return Locomotive.NewCash;
        } 

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static T Last<T>(this IList<T> lst)
        {
                return lst[lst.Count - 1];
        }

        public static int LastIndex<T>(this IList<T> lst)
        {   
            if(lst.Count > 0)
            {
                return lst.Count - 1;
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region City specific steps

        private static List<int> GenerateTemplateCityRoute()
        {
            List<int> templateCityRoute = new List<int>() { Locomotive.StartLocationID };
            while (templateCityRoute.Count <= _maxCityJumps)   // generating list of the cities to visit
            {
                Locomotive.CurrentLocationID = templateCityRoute.Last();
                City currentCity = World.Cities[Locomotive.CurrentLocationID];

                templateCityRoute.Add(currentCity.Connections[_random.Next(0, currentCity.Connections.LastIndex())].Key); // pseudorandomly adding next city to current solution
            }

            Console.WriteLine("\n\n Cities to visit in first template solution: " + string.Join(", ", templateCityRoute.ToArray()));

            Locomotive.TheBestCityRoute = templateCityRoute;
            return templateCityRoute;
        }

        private static List<int> GenerateNextCityRoute(List<int> cityRoute)
        {
            List<int> newCitiyRoute = new List<int>(cityRoute);
            bool firstLoop = true;

            // we decide what city we want to swap. We will always change te next city hop, from city which index we randomly get
            // after the first loop, we will prioritize not changeing the currently established root, and not changeing the next hopcity if possible
            for (int cityToSwapIndex = _random.Next(0, newCitiyRoute.LastIndex() - 1); cityToSwapIndex < newCitiyRoute.LastIndex(); cityToSwapIndex++) // we do it for the each remaining city in the loop
            {
                City currentCity = World.GetCityByID(newCitiyRoute[cityToSwapIndex]);

                if(firstLoop == false)   // sprawdzamy czy jest to pierwsza pętla, ponieważ w niej priorytet ma zmiana
                {
                    if(currentCity.Connections.Exists(connection => connection.Key == newCitiyRoute[cityToSwapIndex + 1]))   // jeśli nie pierwsza to sprawdzamy czy w liście dostępnych następnych skoków znajduje się to samo miasto co na odpowiadającym indeksie w obecnej trasie
                    {
                        continue;
                    }
                }
                else
                {
                    int nextHopIndex = _random.Next(0, currentCity.Connections.LastIndex());    // z listy następnych skoków wybieramy jeden

                    if (currentCity.Connections.Count > 1) // jeśli jest więcej niż 1 możliwy wybór to losujemy tak długo aż uda nam się w końcu zmienić na nowy :)
                    {
                        while (currentCity.Connections[nextHopIndex].Key == newCitiyRoute[cityToSwapIndex + 1])
                        {
                            nextHopIndex = _random.Next(0, currentCity.Connections.LastIndex());
                        }
                    }
                    newCitiyRoute[cityToSwapIndex + 1] = currentCity.Connections[nextHopIndex].Key;  // ustawiamy nowe miasto jako kolejne
                }

                firstLoop = false;
            }

            Locomotive.NewCityRoute = newCitiyRoute;
            return newCitiyRoute;
        }

        #endregion

        #region Contracts Signed specific steps

        private static List<List<int>> FindBestContractsSet(List<int> cityRoute)
        {
            double temperature = _contractLoopTemperature;
            double bestValue = -1;
            double newValue;
            
            List<List<int>> bestContractsSet = new List<List<int>>(GenerateTemplateContractSet(cityRoute));  // first template solution
            List<List<int>> newContractsSet = new List<List<int>>();


            while( temperature > 0.01)  // end statement
            {
                newContractsSet = GenerateNextContractSet(bestContractsSet, cityRoute);    // generating the neighbouring solution

                bestValue = CalculateSolutionValue(bestContractsSet);   // calculating the value for the currently best solution
                newValue = CalculateSolutionValue(newContractsSet); // calculating the value of neighbouring solution

                if (bestValue <= newValue)  // in neighbouring better than current than current = neighbouring
                {
                    bestContractsSet = new List<List<int>>(newContractsSet);
                    Locomotive.BetterCompleatedContractsIDs = new List<int>(Locomotive.NewCompleatedContractsIDs);
                    Locomotive.BetterFlowingContractsList = new List<List<DelieveryContract>>(Locomotive.NewFlowingContractsList);
                    Locomotive.BetterFlowingStatusList = new List<Status>(Locomotive.NewFlowingStatusList);
                }
                else if (_random.NextDouble() > Math.Pow( Math.E, (bestValue-newValue) / temperature))  // the sigma part of algorithm
                {
                    bestContractsSet = new List<List<int>>(newContractsSet);
                }

                temperature *= _contractLoopAlpha;  // lowering the temperature with Alpha
            }

            return bestContractsSet;
        }

        private static List<List<int>> GenerateTemplateContractSet(List<int> cityRoute)
        {
            #region Reseting lists

            List<List<int>> templateContractSet = new List<List<int>>();

            Locomotive.NewCompleatedContractsIDs = new List<int>();
            Locomotive.NewFlowingContractsList = new List<List<DelieveryContract>>();
            Locomotive.NewFlowingStatusList = new List<Status>();

            for (int i = 0; i <= MaxCityVisited; i++)
            {
                Locomotive.NewFlowingContractsList.Add(new List<DelieveryContract>());
                Locomotive.NewFlowingStatusList.Add(new Status(0, 0));
            }

            #endregion

            #region Adding Contracts

            // generating lists of contracts that will be signed up in each city
            for (int currentCityIndex = 0; currentCityIndex < cityRoute.LastIndex(); currentCityIndex++)   // iterating through cities in current city route
            {
                templateContractSet.Add(new List<int>());

                Locomotive.CompleateContractsInCityIndex(cityIndex: currentCityIndex, cityID: cityRoute[currentCityIndex]); // compleating contracts

                #region Finding contracts that are possible to realize

                City currentCity = World.GetCityByID(cityRoute[currentCityIndex]);  // current city
                List<DelieveryContract> possibleContracts = new List<DelieveryContract>();

                foreach (DelieveryContract contract in currentCity.DelieveryContracts)  // iterating through the contracts of each city in current route
                {
                    if (cityRoute.
                        GetRange(currentCityIndex + 1, cityRoute.Count - currentCityIndex - 1).    //todo I feelike it might go out of bounds, but it does not >:[
                        Exists(cityID => cityID == contract.TargetCityID))    // generating the list of contracts that have their target city futher in the cities to visit list
                    {
                        possibleContracts.Add(contract);
                    }
                }

                #endregion

                #region Adding conntracts to Lists

                int maxContractsWeWantToAdd = _random.Next(0, possibleContracts.Count); // we decide for the max number of contracts we want to add
                for (int i = 0; i < maxContractsWeWantToAdd; i++)  // and here we try to add contracts //todo need to add bounds with max weight and max waggon count
                {
                    if (possibleContracts.Count > 0)   
                    {
                        int chosenContractIndex = _random.Next(0, possibleContracts.Count - 1); // randomly choosing contract from available ones

                        if( Locomotive.BetterFlowingContractsList.Exists(hop => hop.Exists(contract => contract.ID == possibleContracts[chosenContractIndex].ID)) == false &&   // we check if we havent taken that contract already
                            Locomotive.MaxWeight >= (Locomotive.BetterFlowingStatusList[currentCityIndex].Weight + possibleContracts[chosenContractIndex].TotalWeight) &&              // we check if we wont exceed the max weight!
                            Locomotive.MaxWaggonCount >= (Locomotive.BetterFlowingStatusList[currentCityIndex].WaggonCount + possibleContracts[chosenContractIndex].WaggonCount)       // we check if we wont exceed the max waggon count :)
                            ) // checking the bounds, if the contract we possibly want to add meets the requirements
                        {
                            Locomotive.SignContract(possibleContracts[chosenContractIndex], currentCityIndex); // adding contracts to list of contracts we take

                            templateContractSet[currentCityIndex].Add(possibleContracts[chosenContractIndex].ID); // adding contracts to list of contracts we take in station
                            possibleContracts.Remove(possibleContracts[chosenContractIndex]);   // removing from possibleContracts so we wont try to add the same contract twice
                        }
                    }
                }

                #endregion

                Locomotive.BetterCompleatedContractsIDs = new List<int>(Locomotive.NewCompleatedContractsIDs);
                Locomotive.BetterFlowingContractsList = new List<List<DelieveryContract>>(Locomotive.NewFlowingContractsList);
                Locomotive.BetterFlowingStatusList = new List<Status>(Locomotive.NewFlowingStatusList);

                Locomotive.ShowFlowingContractsList();
                Locomotive.MoveContractsForward(currentCityIndex);
                Locomotive.EvalStatusList();
            }

            #endregion

            Locomotive.CompleateContractsInCityIndex(cityIndex: cityRoute.LastIndex(), cityID: cityRoute.Last()); // we complete contracts in last city
            Locomotive.EvalStatusList();
            Locomotive.ShowFlowingContractsList();

            return templateContractSet;
        }

        private static List<List<int>> GenerateNextContractSet(List<List<int>> contractSet, List<int> cityRoute)
        {
            List<List<int>> newContractsSet = new List<List<int>>(contractSet);
            Locomotive.NewFlowingStatusList = new List<Status>(Locomotive.BetterFlowingStatusList);
            Locomotive.NewFlowingContractsList = new List<List<DelieveryContract>>(Locomotive.BetterFlowingContractsList);
            Locomotive.NewCompleatedContractsIDs = new List<int>();

            #region Removing Contracts

            foreach (List<int> contractsList in newContractsSet) // we decide if we want ro remove any currently taken contracts
            {
                if (contractsList.Count < 1) { continue; }  // checking if any contracts are available

                int contractToRemoveIndex = _random.Next(0, (contractsList.Count - 1) * 2);   // we decide if we want to remove anything
                                                                                    // and if yes then what is the index of it
                if (contractToRemoveIndex > contractsList.Count - 1) { continue; }

                foreach(List<DelieveryContract> contracts in Locomotive.NewFlowingContractsList) { contracts.RemoveAll(contract => contract.ID == contractsList[contractToRemoveIndex]);  } // wywalamy z flowing lista
                Locomotive.NewCompleatedContractsIDs.RemoveAll(id => id == contractsList[contractToRemoveIndex]);
                contractsList.Remove(contractsList[contractToRemoveIndex]);   // we remove chosen element
                Locomotive.EvalStatusList();
            }

            #endregion

            #region Adding Contracts

            List<DelieveryContract> possibleContracts = new List<DelieveryContract>();

            for (int currentCityIndex = 0; currentCityIndex < cityRoute.LastIndex(); currentCityIndex++) // przechodzimy przez wszystkie miasta w wektorze
            {
                Locomotive.CompleateContractsInCityIndex(cityIndex: currentCityIndex, cityID: cityRoute[currentCityIndex]);

                #region Finding possible contracts

                possibleContracts = new List<DelieveryContract>();

                foreach (DelieveryContract contract in World.GetCityByID(cityRoute[currentCityIndex]).DelieveryContracts) // przechodzimy przez wszystkie kontrakty w każdym mieście
                {
                    //if(newContractsSet[currentCityIndex].Count < 1) { continue; } // this might speed up algorithm by a bit

                    if( newContractsSet[currentCityIndex].Exists(id => id == contract.ID) == false &&   // sprawdzamy czy kontrakt jest już podpisany w rozwiązaniu
                        Locomotive.NewFlowingContractsList.Exists(cityIndex => cityIndex.Exists(cont => cont.ID == contract.ID)) == false &&   // sprawdzamy czy kontrakt jest już podpisany w flowing contracts list
                        cityRoute.FindIndex(cityId => cityId == contract.TargetCityID) > currentCityIndex   // sprawdzamy czy dotrzemy do celu tego kontraktu jeszcze kiedyś
                        )
                    {
                        possibleContracts.Add(contract); // jeśli nie jest już na liście i będziemy w kolejnych krokach przejeżdżali przez target city to dodajemy do listy potencalnych kontraktów
                    }
                }

                #endregion

                if (possibleContracts.Count < 1) { continue; }    // checking if there is at least one contract to sign in list of potential contracts

                int contractToAddIndex = _random.Next(0, possibleContracts.Count - 1); // randomly choosing contract to add from the list of available ones

                if (Locomotive.MaxWeight >= (Locomotive.NewFlowingStatusList[currentCityIndex].Weight + possibleContracts[contractToAddIndex].TotalWeight) &&              // we check if we wont exceed the max weight!
                    Locomotive.MaxWaggonCount >= (Locomotive.NewFlowingStatusList[currentCityIndex].WaggonCount + possibleContracts[contractToAddIndex].WaggonCount))       // we check if we wont exceed the max waggon count :)
                {
                    newContractsSet[currentCityIndex].Add(possibleContracts[contractToAddIndex].ID);
                    Locomotive.SignContract(possibleContracts[contractToAddIndex], currentCityIndex);
                    //Locomotive.ShowFlowingContractsList();
                }
                Locomotive.ShowFlowingContractsList();
                Locomotive.MoveContractsForward(currentCityIndex);
                Locomotive.EvalStatusList();
            }
            Locomotive.CompleateContractsInCityIndex(cityIndex: cityRoute.LastIndex(), cityID: cityRoute.Last()); // we complete contracts in last city
            Locomotive.EvalStatusList();
            Locomotive.ShowFlowingContractsList();

            #endregion

            return newContractsSet;
        }

        #endregion

        #endregion
    }
}

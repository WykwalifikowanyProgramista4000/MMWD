using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static List<int> _citiesToVisitID = new List<int>() { Locomotive.CurrentLocationID };
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

        public static void Solve()
        {
            Stopwatch algorithmTimer = Stopwatch.StartNew();
            double temperature = _mainLoopTemperature;

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
                }
                else if(_random.NextDouble() > Math.Pow(Math.E, (bestSolutionValue - newSolutionValue) / temperature))
                {
                    bestSolutionValue = newSolutionValue;
                    bestContractSet = newContractSet;
                    bestSolutionValue = newSolutionValue;
                }

                temperature *= _mainLoopAlpha;
            }

            _bestSolutionValue = bestSolutionValue;
            _citiesToVisitID = bestCityRoute;
            _contractsToSignID = bestContractSet;

            algorithmTimer.Stop();
            Console.WriteLine("\nFinal solution value: " + _bestSolutionValue);
            Console.WriteLine("\nSolution took: " + algorithmTimer.ElapsedMilliseconds + " miliseconds");
        }


        private static double CalculateSolutionValue(List<List<int>> solution)
        {
            double cashEarned = 0;

            foreach(List<int> contractsIDs in solution)
            {
                foreach(int contractID in contractsIDs)
                {
                    foreach(City city in World.Cities)
                    {
                        DelieveryContract contract = city.GetDelieveryContractByID(contractID);
                        if(contract != null)
                        {
                            cashEarned += contract.Payment;
                        }
                    }
                }
            }

            Console.WriteLine("\n\tThis solution value is: " + cashEarned);

            return cashEarned;
        } 

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        #region City specific steps

        private static List<int> GenerateTemplateCityRoute()
        {
            List<int> templateCityRoute = new List<int>(_citiesToVisitID);
            while (templateCityRoute.Count <= _maxCityJumps)   // generating list of the cities to visit
            {
                List<int> availableConnections = new List<int>();   // temporary list to store available connections from the city in Locomotive current location
                int locomotiveLocation = templateCityRoute[templateCityRoute.Count - 1];

                for (int i = 0; i < World.CTCMatrix.GetLength(0); i++)
                {
                    if (!Double.IsNaN(World.CTCMatrix[locomotiveLocation, i]))
                    {
                        availableConnections.Add(i);
                    }

                }

                templateCityRoute.Add(availableConnections[_random.Next(0, availableConnections.Count - 1)]); // pseudorandomly adding next city to current solution
            }

            Console.WriteLine("\n\n Cities to visit in first template solution: " + string.Join(", ", templateCityRoute.ToArray()));

            return templateCityRoute;
        }

        private static List<int> GenerateNextCityRoute(List<int> cityRoute)
        {
            List<int> newCitiesRoute = new List<int>(cityRoute);
            bool firstLoop = true;

            // we decide what city we want to swap. We will always change te next city hop, from city which index we randomly get
            // after the first loop, we will prioritize not changeing the currently established root, and not changeing the next hopcity if possible
            for (int cityToSwapIndex = _random.Next(0, newCitiesRoute.Count - 2); cityToSwapIndex < newCitiesRoute.Count - 2; cityToSwapIndex++) // we do it for the each remaining city in the loop
            {
                List<int> availableHops = new List<int>();

                for (int nextHopCityID = 0; nextHopCityID < World.CTCMatrix.GetLength(0); nextHopCityID++) // robimy listę kolejnych miast do których możemy sobie skoczyć
                {
                    if (Double.IsNaN(World.CTCMatrix[newCitiesRoute[cityToSwapIndex], nextHopCityID]) == false)
                    {
                        availableHops.Add(nextHopCityID);
                    }
                }

                if(firstLoop == false)   // sprawdzamy czy jest to pierwsza pętla, ponieważ w niej priorytet ma zmiana
                {
                    if(availableHops.Exists(id => id == newCitiesRoute[cityToSwapIndex + 1]))   // jeśli nie pierwsza to sprawdzamy czy w liście dostępnych następnych skoków znajduje się to samo miasto co na odpowiadającym indeksie w obecnej trasie
                    {
                        continue;
                    }
                }

                int nextHopIndex = _random.Next(0, availableHops.Count - 1);    // z listy następnych skoków wybieramy jeden

                if (availableHops.Count > 1) // jeśli jest więcej niż 1 możliwy wybór to losujemy tak długo aż uda nam się w końcu zmienić na nowy :)
                {
                    while (availableHops[nextHopIndex] == newCitiesRoute[cityToSwapIndex + 1])
                    {
                        nextHopIndex = _random.Next(0, availableHops.Count - 1);
                    }
                }

                newCitiesRoute[cityToSwapIndex + 1] = availableHops[nextHopIndex];  // ustawiamy nowe miasto jako kolejne
                firstLoop = false;
            }

            return newCitiesRoute;
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
                }
                else if (_random.NextDouble() > Math.Pow( Math.E, (bestValue-newValue) / temperature))  // the sigma part of algorithm
                {
                    bestContractsSet = newContractsSet;
                }

                temperature *= _contractLoopAlpha;  // lowering the temperature with Alpha
            }

            return bestContractsSet;
        }

        private static List<List<int>> GenerateTemplateContractSet(List<int> cityRoute)
        {
            List<List<int>> templateContractSet = new List<List<int>>();
            // generating lists of contracts that will be signed up in each city
            for (int currentCityIndex = 0; currentCityIndex < cityRoute.Count; currentCityIndex++)   // iterating through cities to visit
            {
                int cityToVisitID = cityRoute[currentCityIndex];
                int contractsInCityCount = World.GetCityByID(cityToVisitID).DelieveryContracts.Count;

                templateContractSet.Add(new List<int>());

                List<int> possibleContractsID = new List<int>();

                foreach (DelieveryContract contract in World.GetCityByID(cityToVisitID).DelieveryContracts)  // iterating through the each city
                {
                    if (cityRoute.
                        GetRange(currentCityIndex + 1, cityRoute.Count - currentCityIndex - 1).    //todo I feelike it might go out of bounds, but it does not >:[
                        Exists(cityID => cityID == contract.TargetCityID))    // generating the list of contracts that have their target city futher in the cities to visit list
                    {
                        possibleContractsID.Add(contract.ID);
                    }
                }

                int maxContractsWeWantToAdd = _random.Next(0, possibleContractsID.Count); // we decide for the max number of contracts we want to add

                for (int i = 0; i < maxContractsWeWantToAdd; i++)  // and here we try to add contracts //todo need to add bounds with max weight and max waggon count
                {
                    if (possibleContractsID.Count > 0)   // randomly choosing contract from available ones
                    {
                        int chosenContractIndex = _random.Next(0, possibleContractsID.Count - 1);
                        templateContractSet[currentCityIndex].Add(possibleContractsID[chosenContractIndex]);
                        possibleContractsID.Remove(possibleContractsID[chosenContractIndex]);
                    }
                }
            }

            Console.WriteLine("\n Chosen contracts:");
            for (int i = 0; i < cityRoute.Count; i++)
            {
                Console.WriteLine("\tIn city: '" + cityRoute[i] + "' we take contracts with ID: " + string.Join(",", templateContractSet[i].ToArray()));
            }
            return templateContractSet;
        }

        private static List<List<int>> GenerateNextContractSet(List<List<int>> contractSet, List<int> cityRoute)
        {
            List<List<int>> newContractsSet = new List<List<int>>(contractSet);

            #region Removing Contracts
            foreach (List<int> contractsList in newContractsSet) // we decide if we want ro remove any currently taken contracts
            {
                if (contractsList.Count < 1) { continue; }  // checking if any contracts are available

                int decisionInt = _random.Next(0, (contractsList.Count - 1) * 2);   // we decide if we want to remove anything
                                                                                    // and if yes then what is the index of it
                if (decisionInt > contractsList.Count - 1) { continue; }

                contractsList.Remove(contractsList[decisionInt]);   // we remove chosen element
            }
            #endregion

            #region Adding Contracts
            List<int> contrIMightWntToTake = new List<int>();

            for (int i=0; i < cityRoute.Count; i++) // przechodzimy przez wszystkie miasta w wektorze
            {
                contrIMightWntToTake = new List<int>();

                foreach (DelieveryContract contract in World.GetCityByID(cityRoute[i]).DelieveryContracts) // przechodzimy przez wszystkie contrakty w każdym mieście
                {
                    if(newContractsSet[i].Count < 1) { continue; }
                    if(newContractsSet[i].Exists(id => id == contract.ID)) // sprawdzamy czy kontrakt jest już podpisany w rozwiązaniu
                    {
                        continue; // jeśli tak to sprawdzamy kolejny kappa
                    }
                    else if(cityRoute.FindIndex(cityId => cityId == contract.TargetCityID) > i) // sprawdzamy czy dla tego kontraktu będziemy przejeżdżali w kolejnych krokach przez jego miasto docelowe
                    {
                        contrIMightWntToTake.Add(contract.ID); // jeśli nie jest już na liście i będziemy w kolejnych krokach przejeżdżali przez target city to dodajemy do listy potencalnych kontraktów
                    }
                }

                if(contrIMightWntToTake.Count < 1) { continue; }    // checking if there is at least one contract to sign in list of potential contracts

                int contractToAddIndex = _random.Next(0, contrIMightWntToTake.Count - 1); // randomly choosing contract to add from the list of available ones

                newContractsSet[i].Add(contrIMightWntToTake[contractToAddIndex]);
            }
            #endregion

            return newContractsSet;
        }

        #endregion

        #endregion
    }
}

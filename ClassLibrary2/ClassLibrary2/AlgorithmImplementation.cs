using System;
using System.Collections.Generic;
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

        private static double _temperature;
        private static bool _temperatureSetFlag = false;

        private static double _oldSolutionValue;
        private static double _newSolutionValue;

        private static List<int> _citiesToVisitIDVector = new List<int>() { Locomotive.CurrentLocationID };
        private static List<List<int>> _contractsToSignIDVector2D = new List<List<int>>();

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


        public static double Temperature
        {
            get { return _temperature; }
            set
            {
                if(_temperatureSetFlag == false)
                {
                    _temperature = value;
                    _temperatureSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Start temperature has been already set");
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
            GenerateFirstTemplateSolution();
            _oldSolutionValue = CalculateCurrentSolutionValue();

            GenerateNextSolution();
        }

        private static void GenerateFirstTemplateSolution()
        {
            while(_citiesToVisitIDVector.Count <= _maxCityJumps)   // generating list of the cities to visit
            {
                List<int> availableConnections = new List<int>();   // temporary list to store available connections from the city in Locomotive current location
                int locomotiveLocation = _citiesToVisitIDVector[_citiesToVisitIDVector.Count - 1];

                for (int i=0; i < World.CTCMatrix.GetLength(0); i++)
                {
                    if (!Double.IsNaN(World.CTCMatrix[locomotiveLocation, i])) 
                    {
                        availableConnections.Add(i);
                    }

                }

                _citiesToVisitIDVector.Add(availableConnections[_random.Next(0, availableConnections.Count - 1)]); // pseudorandomly adding next city to current solution
            }

            Console.WriteLine("\nCities to visit in first template solution: " + string.Join(", ", _citiesToVisitIDVector.ToArray()) );

            // generating lists of contracts that will be signed up in each city
            for (int currentCityIndex=0; currentCityIndex < _citiesToVisitIDVector.Count; currentCityIndex++)   // iterating through cities to visit
            {
                int cityToVisitID = _citiesToVisitIDVector[currentCityIndex];
                int contractsInCityCount = World.GetCityByID(cityToVisitID).DelieveryContracts.Count;

                _contractsToSignIDVector2D.Add(new List<int>());

                List<int> possibleContractsID = new List<int>();

                foreach(DelieveryContract contract in World.GetCityByID(cityToVisitID).DelieveryContracts)  // iterating through the each city
                {
                    if(_citiesToVisitIDVector.
                        GetRange(currentCityIndex + 1, _citiesToVisitIDVector.Count-currentCityIndex-1).    //todo I feelike it might go out of bounds, but it does not >:[
                        Exists(cityID => cityID == contract.TargetCityID))    // generating the list of contracts that have their target city futher in the cities to visit list
                    {
                        possibleContractsID.Add(contract.ID);
                    }
                }

                int maxContractsWeWantToAdd = _random.Next(0, possibleContractsID.Count); // we decide for the max number of contracts we want to add

                for(int i=0; i < maxContractsWeWantToAdd; i++)  // and here we try to add contracts //todo need to add bounds with max weight and max waggon count
                {
                    if(possibleContractsID.Count > 0)   // randomly choosing contract from available ones
                    {
                        int chosenContractIndex = _random.Next(0, possibleContractsID.Count - 1);
                        _contractsToSignIDVector2D[currentCityIndex].Add(possibleContractsID[chosenContractIndex]);
                        possibleContractsID.Remove(possibleContractsID[chosenContractIndex]);
                    }
                }

            }

            Console.WriteLine("\nChosen contracts:");
            for(int i=0; i < _citiesToVisitIDVector.Count; i++)
            {
                Console.WriteLine("In city: '" + _citiesToVisitIDVector[i] + "' we take contracts with ID: " + string.Join(",", _contractsToSignIDVector2D[i].ToArray()));
            }

        }

        private static void GenerateNextSolution()
        {
            int firstCityToSwapIndex = _random.Next(0, _citiesToVisitIDVector.Count - 1);
            int secondCityToSwapIndex = _random.Next(0, _citiesToVisitIDVector.Count - 1);
            while(firstCityToSwapIndex == secondCityToSwapIndex)
            {
                secondCityToSwapIndex = _random.Next(0, _citiesToVisitIDVector.Count - 1);
            }

            _citiesToVisitIDVector.Swap(firstCityToSwapIndex, secondCityToSwapIndex);
            int pivot;
            if(firstCityToSwapIndex < secondCityToSwapIndex) { pivot = firstCityToSwapIndex; } else { pivot = secondCityToSwapIndex; }

            for(int i=firstCityToSwapIndex; i < _citiesToVisitIDVector.GetRange(pivot, _citiesToVisitIDVector.Count-pivot-1).Count; i++)
            {
                //todo dorobić XDDD
            }
        }

        private static double CalculateCurrentSolutionValue()
        {
            double cashEarned = 0;

            foreach(List<int> contractsIDs in _contractsToSignIDVector2D)
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

            Console.WriteLine("This solution value is: " + cashEarned);

            return cashEarned;
        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace Algorithm
{
    public enum SolutionType
    {
        New,
        Better,
        TheBest
    }

    public struct Status
    {
        public double Weight;
        public int WaggonCount;

        public Status(double weight, int waggonCount)
        {
            Weight = weight;
            WaggonCount = waggonCount;
        }
    }

    public static class Locomotive
    {
        #region Parameters

        private static double _maxWeight;
        private static bool _maxWeightSetFlag = false;

        private static int _maxWaggonCount;
        private static bool _maxWaggonCountSetFlag = false;

        private static List<int> _theBestCityRoute = new List<int>();
        private static List<List<DelieveryContract>> _theBestFlowingContractsList = new List<List<DelieveryContract>>();
        private static List<Status> _theBestFlowingStatus = new List<Status>();
        private static List<int> _theBestCompleatedContractsIDs = new List<int>();
        private static double _theBestCash = 0;

        //private static List<int> _betterCityRoute = new List<int>();
        private static List<List<DelieveryContract>> _betterFlowingContractsList = new List<List<DelieveryContract>>();
        private static List<Status> _betterFlowingStatus = new List<Status>();
        private static List<int> _betterCompleatedContractsIDs = new List<int>();
        private static double _betterCash = 0;

        private static List<int> _newCityRoute = new List<int>();
        private static List<List<DelieveryContract>> _newFlowingContractsList = new List<List<DelieveryContract>>();
        private static List<Status> _newFlowingStatusList = new List<Status>();
        private static List<int> _newCompleatedContractsIDs = new List<int>();
        private static double _newCash = 0;

        private static int _startLocationID;    //todo add setflag
        private static int _currentLocationID;

        #endregion

        #region Get/Set

        public static double MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                if (_maxWeightSetFlag == false)
                {
                    _maxWeight = value;
                    _maxWeightSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Max weight has been already set to '" + _maxWeight + "' and cannot be set to '" + value + "'");
                }
            }
        }
        public static int MaxWaggonCount
        {
            get { return _maxWaggonCount; }
            set
            {
                if (_maxWaggonCountSetFlag == false)
                {
                    _maxWaggonCount = value;
                    _maxWaggonCountSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Max waggon count has been already set to '" + _maxWaggonCount + "' and cannot be set to '" + value + "'");
                }
            }
        }

        public static List<int> TheBestCityRoute
        {
            get { return _theBestCityRoute; }
            set { _theBestCityRoute = value; }
        }
        public static List<List<DelieveryContract>> TheBestFlowingContractsList
        {
            get { return _theBestFlowingContractsList; }
            set { _theBestFlowingContractsList = value; }
        }
        public static List<Status> TheBestFlowingStatusList
        {
            get { return _theBestFlowingStatus; }
            set { _theBestFlowingStatus = value; }
        }
        public static List<int> TheBestCompleatedContractsIDs
        {
            get { return _theBestCompleatedContractsIDs; }
            set { _theBestCompleatedContractsIDs = value; }
        }
        public static double TheBestCash
        {
            get { return _theBestCash; }
            set { _theBestCash = value; }
        }

        public static List<List<DelieveryContract>> BetterFlowingContractsList
        {
            get { return _betterFlowingContractsList; }
            set { _betterFlowingContractsList = value; }
        }
        public static List<Status> BetterFlowingStatusList
        {
            get { return _betterFlowingStatus; }
            set { _betterFlowingStatus = value; }
        }
        public static List<int> BetterCompleatedContractsIDs
        {
            get { return _betterCompleatedContractsIDs; }
            set { _betterCompleatedContractsIDs = value; }
        }
        public static double BetterCash
        {
            get { return _betterCash; }
            set { _betterCash = value; }
        }

        public static List<int> NewCityRoute
        {
            get { return _newCityRoute; }
            set { _newCityRoute = value; }
        }
        public static List<List<DelieveryContract>> NewFlowingContractsList
        {
            get { return _newFlowingContractsList; }
            set { _newFlowingContractsList = value; }
        }
        public static List<Status> NewFlowingStatusList
        {
            get { return _newFlowingStatusList; }
            set { _newFlowingStatusList = value; }
        }
        public static List<int> NewCompleatedContractsIDs
        {
            get { return _newCompleatedContractsIDs; }
            set { _newCompleatedContractsIDs = value; }
        }
        public static double NewCash
        {
            get { return _newCash; }
            set { _newCash = value; }
        }

        public static int StartLocationID
        {
            get { return _startLocationID; }
            set { _startLocationID = value; }
        }

        public static int CurrentLocationID
        {
            get { return _currentLocationID; }
            set { _currentLocationID = value; }
        }

        #endregion

        #region Methods

        public static void SignContract(DelieveryContract contract, int cityIndex)
        {
            _newFlowingContractsList[cityIndex].Add(contract);

            _newFlowingStatusList[cityIndex] = new Status(_newFlowingStatusList[cityIndex].Weight + contract.TotalWeight,
                                                          _newFlowingStatusList[cityIndex].WaggonCount + contract.WaggonCount);
        }

        public static void CompleateContractsInCityIndex(int cityIndex, int cityID)
        {
            if(_newFlowingContractsList.Count < 1) { return; }
            List<DelieveryContract> contractsToRemove = new List<DelieveryContract>();
            foreach(DelieveryContract contract in _newFlowingContractsList[cityIndex])
            {
                if(contract.TargetCityID == cityID)
                {
                    _newCompleatedContractsIDs.Add(contract.ID);
                    contractsToRemove.Add(contract);
                }
            }

            foreach( DelieveryContract contractToRemove in contractsToRemove)
            {
                _newFlowingContractsList[cityIndex].Remove(contractToRemove);
            }
            EvalStatusList();
        }

        public static void EvalStatusList()
        {
            double weight;
            int waggonCount;

            for( int cityIndex = 0; cityIndex <= _newFlowingContractsList.LastIndex(); cityIndex++)
            {
                weight = 0;
                waggonCount = 0;

                foreach(DelieveryContract contract in _newFlowingContractsList[cityIndex])
                {
                    weight += contract.TotalWeight;
                    waggonCount += contract.WaggonCount;
                }

                _newFlowingStatusList[cityIndex] = new Status(weight, waggonCount);
            }
        }

        public static void EvalCash(SolutionType solution)
        {
            if(solution == SolutionType.TheBest)
            {
                _theBestCash = 0;
                foreach(int contractID in _theBestCompleatedContractsIDs) { _theBestCash += World.Contracts.Find(contract => contract.ID == contractID).Payment; }
            }
            else if(solution == SolutionType.Better)
            {
                _betterCash = 0;
                foreach (int contractID in _betterCompleatedContractsIDs) { _betterCash += World.Contracts.Find(contract => contract.ID == contractID).Payment; }
            }
            else if (solution == SolutionType.New)
            {
                _newCash = 0;
                foreach (int contractID in _newCompleatedContractsIDs) { _newCash += World.Contracts.Find(contract => contract.ID == contractID).Payment; }
            }
        }
        
        public static void ShowFlowingContractsList()
        {
            for (int cityIndex = 0; cityIndex <= _newFlowingContractsList.LastIndex(); cityIndex++)
            {
                Console.Write("\nIn CityIndex: " + cityIndex + "  we have: ");

                foreach( DelieveryContract contract in _newFlowingContractsList[cityIndex])
                {
                    Console.Write(contract.ID + ", ");
                }
            }
            Console.Write("\n");
        }

        public static void MoveContractsForward(int currnetCityIndex)
        {
            foreach(DelieveryContract contract in _newFlowingContractsList[currnetCityIndex])
            {
                if (_newFlowingContractsList[currnetCityIndex + 1].Exists(possibleTwin => possibleTwin.ID == contract.ID) == false)
                {
                    _newFlowingContractsList[currnetCityIndex + 1].Add(contract);
                }
            }
        }

        #endregion
    }
}

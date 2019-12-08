using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public struct Status
    {
        public double Weight { get; set; }
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

        private static double _cash;

        private static double _weight;
        private static double _maxWeight;
        private static bool _maxWeightSetFlag = false;

        private static int _waggonCount;
        private static int _maxWaggonCount;
        private static bool _maxWaggonCountSetFlag = false;

        private static List<List<DelieveryContract>> _flowingContractsList = new List<List<DelieveryContract>>();
        private static List<Status> _flowingStatus = new List<Status>();

        private static List<int> _compleatedContractsIDs = new List<int>();

        private static int _startLocationID;    //todo add setflag
        private static int _currentLocationID;

        private static int _targetLocationID;
        private static bool _targetLocationSetFlag = false;


        #endregion

        #region Get/Set

        public static double Cash
        {
            get { return _cash; }
        }
        public static double Weight
        {
            get { return _weight; }
        }
        public static double MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                if (_maxWaggonCountSetFlag == false)
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
        public static int WaggonCount
        {
            get { return _waggonCount; }

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

        public static List<List<DelieveryContract>> FlowingContractsList
        {
            get { return _flowingContractsList; }
            set { _flowingContractsList = value; }
        }
        public static List<Status> FlowingStatusList
        {
            get { return _flowingStatus; }
            set { _flowingStatus = value; }
        }
        public static List<int> CompleatedContractsIDs
        {
            get { return _compleatedContractsIDs; }
            set { _compleatedContractsIDs = value; }
        }
        
        public static int TargetLocationID
        {
            get { return _targetLocationID; }
            set
            {
                
            }
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
            _flowingContractsList[cityIndex].Add(contract);

            FlowingStatusList[cityIndex] = new Status(_flowingStatus[cityIndex].Weight + contract.TotalWeight,
                                                      _flowingStatus[cityIndex].WaggonCount + contract.WaggonCount);
        }

        public static void CompleateContractsInCityIndex(int cityIndex, int cityID)
        {
            if(FlowingContractsList.Count < 1) { return; }
            List<DelieveryContract> contractsToRemove = new List<DelieveryContract>();
            foreach(DelieveryContract contract in FlowingContractsList[cityIndex])
            {
                if(contract.TargetCityID == cityID)
                {
                    _compleatedContractsIDs.Add(contract.ID);
                    contractsToRemove.Add(contract);
                }
            }

            foreach( DelieveryContract contractToRemove in contractsToRemove)
            {
                FlowingContractsList[cityIndex].Remove(contractToRemove);
            }
            EvalStatusList();
        }

        public static void EvalStatusList()
        {
            double weight;
            int waggonCount;

            for( int cityIndex = 0; cityIndex <= _flowingContractsList.LastIndex(); cityIndex++)
            {
                weight = 0;
                waggonCount = 0;

                foreach(DelieveryContract contract in _flowingContractsList[cityIndex])
                {
                    weight += contract.TotalWeight;
                    waggonCount += contract.WaggonCount;
                }

                FlowingStatusList[cityIndex] = new Status(weight, waggonCount);
            }
        }

        public static void EvalCash()
        {
            _cash = 0;

            foreach(int contractID in _compleatedContractsIDs)
            {
                _cash += World.Contracts.Find(contract => contract.ID == contractID).Payment;
            }
        }
        
        public static void ShowFlowingContractsList()
        {
            for (int cityIndex = 0; cityIndex <= FlowingContractsList.LastIndex(); cityIndex++)
            {
                Console.Write("\nIn CityIndex: " + cityIndex + "  we have: ");

                foreach( DelieveryContract contract in FlowingContractsList[cityIndex])
                {
                    Console.Write(contract.ID + ", ");
                }
            }
            Console.Write("\n");
        }

        public static void MoveContractsForward(int currnetCityIndex)
        {
            foreach(DelieveryContract contract in FlowingContractsList[currnetCityIndex])
            {
                if (FlowingContractsList[currnetCityIndex + 1].Exists(possibleTwin => possibleTwin.ID == contract.ID) == false)
                {
                    FlowingContractsList[currnetCityIndex + 1].Add(contract);
                }
            }
        }

        #endregion
    }
}

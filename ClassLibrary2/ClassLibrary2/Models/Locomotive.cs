using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
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

        private static double _cash;

        private static double _weight;
        private static double _maxWeight;
        private static bool _maxWeightSetFlag = false;

        private static int _waggonCount;
        private static int _maxWaggonCount;
        private static bool _maxWaggonCountSetFlag = false;

        private static List<List<DelieveryContract>> _bestFlowingContractsList = new List<List<DelieveryContract>>();
        private static List<Status> _bestFlowingStatus = new List<Status>();
        private static List<int> _bestCompleatedContractsIDs = new List<int>();

        private static List<List<DelieveryContract>> _newFlowingContractsList = new List<List<DelieveryContract>>();
        private static List<Status> _newFlowingStatus = new List<Status>();
        private static List<int> _newCompleatedContractsIDs = new List<int>();

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
            get { return _bestFlowingContractsList; }
            set { _bestFlowingContractsList = value; }
        }
        public static List<Status> FlowingStatusList
        {
            get { return _bestFlowingStatus; }
            set { _bestFlowingStatus = value; }
        }
        public static List<int> CompleatedContractsIDs
        {
            get { return _bestCompleatedContractsIDs; }
            set { _bestCompleatedContractsIDs = value; }
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
            _bestFlowingContractsList[cityIndex].Add(contract);

            FlowingStatusList[cityIndex] = new Status(_bestFlowingStatus[cityIndex].Weight + contract.TotalWeight,
                                                      _bestFlowingStatus[cityIndex].WaggonCount + contract.WaggonCount);
        }

        public static void CompleateContractsInCityIndex(int cityIndex, int cityID)
        {
            if(FlowingContractsList.Count < 1) { return; }
            List<DelieveryContract> contractsToRemove = new List<DelieveryContract>();
            foreach(DelieveryContract contract in FlowingContractsList[cityIndex])
            {
                if(contract.TargetCityID == cityID)
                {
                    _bestCompleatedContractsIDs.Add(contract.ID);
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

            for( int cityIndex = 0; cityIndex <= _bestFlowingContractsList.LastIndex(); cityIndex++)
            {
                weight = 0;
                waggonCount = 0;

                foreach(DelieveryContract contract in _bestFlowingContractsList[cityIndex])
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

            foreach(int contractID in _bestCompleatedContractsIDs)
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

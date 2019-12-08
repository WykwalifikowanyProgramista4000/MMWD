using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
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

        private static List<DelieveryContract> _delieveryContracts = new List<DelieveryContract>();

        private static int _startLocationID;
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
                if(_maxWaggonCountSetFlag == false)
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

        public static List<DelieveryContract> DelieveryContracts
        {
            get { return _delieveryContracts; }
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

        public static DelieveryContract GetDelieveryContractByID(int contractID)
        {
            return _delieveryContracts.Find(contract => contract.ID == contractID);
        }


        public static void SignContractByID(int contractID)
        {
            DelieveryContract signedContract = World.GetCityByID(CurrentLocationID).DelieveryContracts.Find(contract => contract.ID == contractID);
            _delieveryContracts.Add(signedContract);

            World.GetCityByID(CurrentLocationID).RemoveDelieveryContract(contractID);
            
            _weight += signedContract.TotalWeight;
            _waggonCount += signedContract.WaggonCount;
        }

        public static void CompleateContractByID(int contractID)
        {
            DelieveryContract compleatedContract = _delieveryContracts.Find(contract => contract.ID == contractID);
            _delieveryContracts.Remove(GetDelieveryContractByID(contractID));

            _cash += compleatedContract.Payment;

            _weight -= compleatedContract.TotalWeight;
            _waggonCount -= compleatedContract.WaggonCount;
        }

        #endregion
    }
}

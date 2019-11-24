using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public static class Locomotive
    {
        private static double _cash;
        private static double _weight;
        private static int _waggonCount;

        private static List<DelieveryContract> _delieveryContracts = new List<DelieveryContract>();

        private static int _currentLocationID;

        private static int _targetLocationID;

        private static bool _targetLocationSetFlag = false;


        public static double Cash
        {
            get { return _cash; }
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
                if(_targetLocationSetFlag == false)
                {
                    _targetLocationID = value;
                    _targetLocationSetFlag = true;
                }
                else
                {
                    Console.WriteLine("Target location has been already set to '" + _targetLocationID + "' and cannot be set to '" + value + "'");
                }
            }
        }

        public static int CurrentLocationID
        {
            get { return _currentLocationID; }
            set { _currentLocationID = value; }
        }

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
    }
}

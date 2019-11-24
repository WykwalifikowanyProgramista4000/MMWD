using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class DelieveryContract
    {
        private readonly int _id;
        private readonly int _targetCityID;
        private readonly double _payment;
        private readonly int _waggonCount;
        private readonly double _totalWeight;

        public double TotalWeight
        {
            get { return _totalWeight; }
        }

        public int WaggonCount
        {
            get { return _waggonCount; }
        }
        
        public double Payment
        {
            get { return _payment; }
        }
        
        public int TargetCityID
        {
            get { return _targetCityID; }
        }

        public int ID
        {
            get { return _id; }
        }

        public DelieveryContract(int targetCityID, double payment, int waggonCount, double totalWeight, int contractID)
        {
            _targetCityID = targetCityID;
            _payment = payment;
            _waggonCount = waggonCount;
            _totalWeight = totalWeight;
            _id = contractID;
        }
    }
}

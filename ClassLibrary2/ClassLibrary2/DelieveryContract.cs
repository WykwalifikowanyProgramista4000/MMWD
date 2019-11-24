using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class DelieveryContract
    {
        private readonly City _targetCity1;
        private readonly float _payment;
        private readonly int _waggonCount;
        private readonly float _totalWeight;

        public float GetTotalWeight() { return _totalWeight; }

        public int GetWaggonCount() { return _waggonCount; }
        
        public float GetPayment() { return _payment; }
        
        public City GetTargetCity() { return _targetCity1; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public static class DelieveryContractFactory
    {
        private static int _idCounter = 0;

        private static void IncrementIdCounter() { _idCounter++; }

        public static DelieveryContract CreateContract(int targetCityID, double payment, int waggonCount, double totalWeight)
        {
            DelieveryContract contract = new DelieveryContract(targetCityID, payment, waggonCount, totalWeight, _idCounter);
            IncrementIdCounter();
            return contract;
        }
    }
}

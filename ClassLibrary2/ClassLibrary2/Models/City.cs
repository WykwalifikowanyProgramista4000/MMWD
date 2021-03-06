﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class City
    {
        #region Parameters

        private readonly int _id;
        private List<DelieveryContract> _delieveryContracts = new List<DelieveryContract>();
        private List<KeyValuePair<int, double>> _connections = new List<KeyValuePair<int, double>>();

        #endregion 

        #region Constructors

        public City(int id)
        {
            _id = id;
        }

        #endregion

        #region Get/Set

        public int ID
        {
            get { return _id; }
        }

        public List<DelieveryContract> DelieveryContracts
        {
            get { return _delieveryContracts; }
        }

        public List<KeyValuePair<int, double>> Connections
        {
            get { return _connections; }
        }
        #endregion

        #region Public Methods

        public DelieveryContract GetDelieveryContractByID(int contractID)
        {
            return _delieveryContracts.Find(contract => contract.ID == contractID);
        }

        public void AddDelieveryContract(DelieveryContract delieveryContract)
        {
            if (delieveryContract.TargetCityID == _id)
            {
                Console.WriteLine("Cannot add contract with target set to source city");
                return;
            }

            _delieveryContracts.Add(delieveryContract);
        }

        public void AddConnection(int destinationCityID, double distance)
        {
            _connections.Add(new KeyValuePair<int, double>(destinationCityID, distance));
        }

        public void RemoveDelieveryContract(int contractID)
        {
            _delieveryContracts.Remove(GetDelieveryContractByID(contractID));
        }

        #endregion
    }
}

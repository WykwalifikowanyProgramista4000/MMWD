using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class City
    {
        #region Parameters

        private double _xPosition;
        private double _yPosition;
        private readonly int _id;
        private List<DelieveryContract> _availableContracts = new List<DelieveryContract>();

        private List<int> _connectedCitiesID;

        #endregion 

        public double X // todo dodać delegate do reewaluacji dystansów
        {
            get { return _xPosition; }
            set { _xPosition = value; }
        }

        public double Y // todo dodać delegatę do rewaluacji dystansów
        {
            get { return _yPosition; }
            set { _yPosition = value; }
        }

        #region Constructors

        public City(float X, float Y, int ID)
        {
            _xPosition = X;
            _yPosition = Y;

            _id = ID;
        }

        #endregion

        #region Getters

        public int GetID() { return _id; }

        public List<int> GetConnectedCities() { return _connectedCitiesID; }

        #endregion

        #region Public Methods

        public void AddConnectedCity(int cityID) { _connectedCitiesID.Add(cityID); }

        public void AddConnectedCities(List<int> cityIDs)
        {
            foreach(int id in cityIDs)
            {
                if(_connectedCitiesID.FindIndex(x => x == id) == -1)
                {
                    _connectedCitiesID.Add(id);
                }
                else
                {
                    Console.WriteLine("City with ID matching ", id, " already found in connected cities. Skipping to next ID.");
                }
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class City
    {
        private List<DelieveryContract> _availableContracts = new List<DelieveryContract>();
        private List<City> _connectedCities = new List<City>();

        private double _xPosition;
        private double _yPosition;

        private readonly int _id;

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

        public City(float X, float Y, int ID)
        {
            _xPosition = X;
            _yPosition = Y;

            _id = ID;
        }


        public int GetID() { return _id; }

        public List<City> GetConnectedCities() { return _connectedCities; }

        public void AddConnectedCity(City city) { _connectedCities.Add(city); }
    }
}

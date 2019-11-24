using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public static class World
    {
        #region Params

        private static List<City> _cities = new List<City>();

        private static double[,] _distanceCityToCityMatrix = new double[1, 1] { { Double.NaN } };

        #endregion

        #region Get/Set

        public static List<City> Cities
        {
            get { return _cities; }
        }
        
        public static double[,] CTCMatrix
        {
            get { return _distanceCityToCityMatrix; }
        }

        #endregion

        #region Methods

        public static City GetCityByID(int cityID) { return _cities.Find(city => city.ID == cityID); }

        public static void AddCities(int numberOfCities)
        {
            for(int i=0; i < numberOfCities; i++)
            {
                _cities.Add(CityFactory.MakeCity());
            }
            
            RefreshCTCMatrix();
        }

        public static void AddConnection(int sourceCityID, int targetCityID, double distance)
        {
            if(sourceCityID == targetCityID)
            {
                Console.WriteLine("Cannot establish connection between city: " + sourceCityID + " and city: " + targetCityID);
                return;
            }

            if(_cities.Find(x => x.ID == sourceCityID) == null || _cities.Find(x => x.ID == targetCityID) == null)
            {
                Console.WriteLine("Connection between city: " + sourceCityID + "and city: " + targetCityID + "cannot be established");
                return;
            }
            else
            {
                _distanceCityToCityMatrix[sourceCityID, targetCityID] = distance;
            }
        }

        public static void AddContractToCity(int sourceCityID, DelieveryContract delieveryContract)
        {
            if (delieveryContract.TargetCityID == sourceCityID)
            {
                Console.WriteLine("Cannot add contract with target set to source city");
                return;
            }

            _cities.Find(city => city.ID == sourceCityID).AddDelieveryContract(delieveryContract);
        }

        public static void RemoveContractFromCityByID(int cityID, int contractID)
        {

        }

        private static void RefreshCTCMatrix()
        {
            double[,] newCTCMatrix = new double[_cities.Count, _cities.Count];
            FillWithNaN(newCTCMatrix);

            for (int j = 0; j < _distanceCityToCityMatrix.GetLength(0); j++)
                for (int i = 0; i < _distanceCityToCityMatrix.GetLength(0); i++)
                {
                    newCTCMatrix[i, j] = _distanceCityToCityMatrix[i, j];
                }

            _distanceCityToCityMatrix = newCTCMatrix;
        }

        private static void FillWithNaN(double[,] matrix)
        {
            for(int j=0; j < matrix.GetLength(0); j++)
                for(int i=0; i < matrix.GetLength(0); i++)
                {
                    matrix[i, j] = Double.NaN;
                }
        }

        #endregion

    }
}

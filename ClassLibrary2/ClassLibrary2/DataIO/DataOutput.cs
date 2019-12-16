using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DataIO
{
    public static class DataOutput
    {
        #region Params

        private static string _line = "";

        private static System.IO.StreamWriter _streamWriter;

        #endregion // Params

        #region Get/Set

        public static string OutputPath { get; set; }

        #endregion // Get/Set

        #region Methods

        public static void SaveData(double temperature, double currentValue, double currentBestValue)
        {
            _line = String.Format("{0};{1};{2};", temperature.ToString("F"), currentValue, currentBestValue);

            using (_streamWriter = new System.IO.StreamWriter(OutputPath, true))
            {
                _streamWriter.WriteLine(_line);
            }
        }

        #endregion //Methods
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Each instance of CanonicalModel is a single row of a CSV in the correct datatype.
    /// </summary>
    public class CanonicalModel
    {
        //have all the values that i will need in here in the correct type
        public int EAN { get; set; }
        public string Size { get; set; } // Size stays as a string, as the sorting takes strings as input.
        public decimal GrossProfit { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal BoughtPrice { get; set; } 
        public int QuantitySold { get; set; }
        public string ItemgroupName { get; set; }
        public List<string> Unknown { get; set; } = new();//to catch any values that for some reason doesn't have a header name that's not registered
        //SortingIndex is only for sorting. it doesn't come from the file, but is derrived from the Size in step 2.
        public double SortingIndex { get; set; } = 0;

        public override string ToString()
        {
            return $"EAN: {EAN}, Size: {Size}, SalesPrice: {SalesPrice}, BoughtPrice: {BoughtPrice}, unkowns: {Unknown}";
        }

    }
}

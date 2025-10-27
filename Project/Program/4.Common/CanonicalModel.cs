using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CanonicalModel
    {
        //have all the values that i will need in here in the correct type
        public int EAN { get; set; }
        public string Size { get; set; } // Size stays as a string, as the sorting takes strings as input.
        public decimal Turnover { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal VAT { get; set; }
        public string Unknown { get; set; } //to catch any values that for some reason doesn't have a header name that's not registered


    }
}

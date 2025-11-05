using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandling
{
    internal class CombineItems
    {
        public Dictionary<int,CanonicalModel> UniqueProducts { get; set; } = new Dictionary<int, CanonicalModel>();
        public void Combine(InhouseData FullParsedCSV)
        {
            foreach (CanonicalModel CM in FullParsedCSV.Row)
            {
                //If price is different on the same product, then this will miss it.
                //I'll probably need to make sure the price 
                //doesn't change, and if it does, make a case for that.
                if (UniqueProducts.ContainsKey(CM.EAN))
                {
                    UniqueProducts[CM.EAN].QuantitySold += CM.QuantitySold;
                }
                else
                {
                    UniqueProducts.Add(CM.EAN, CM);
                }
            }
            //optimization. Map original CM to new one

            InhouseData combinedData = new();
            CanonicalModel combinedCM = new();
            foreach (var item in UniqueProducts) 
            {
                combinedCM.EAN = item.Key;
                //combinedCM.QuantitySold = item.Value;
            }

         
        }

    }
}

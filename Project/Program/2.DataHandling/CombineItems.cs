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


        //TODO: Theres a bug where the item category doesn't get added, or something like that. "Top was not found"
        public Dictionary<int,CanonicalModel> UniqueProducts { get; set; } = new Dictionary<int, CanonicalModel>();
        /// <summary>
        /// Compressed a full parsed CSV file into a smaller chunk, so when i sort, it doesn't have to sort x amount of the same size
        /// </summary>
        /// <param name="FullParsedCSV"></param>
        /// <returns>new InhouseData type, that contains the compressed version of the input InhouseData</returns>
        public InhouseData Combine(InhouseData FullParsedCSV)
        {
            //each row of the CSV
            foreach (CanonicalModel CM in FullParsedCSV.Row)
            {
                if (UniqueProducts.ContainsKey(CM.EAN))
                {
                    UniqueProducts[CM.EAN].QuantitySold += CM.QuantitySold;
                }
                else
                {
                    UniqueProducts.Add(CM.EAN, CM);
                }
            }
            
            InhouseData combinedData = new();
            //TODO: Add each field that needs to be multiplied in the foreach.
            foreach (var item in UniqueProducts) 
            {
                CanonicalModel combinedCM = item.Value;
                combinedCM.GrossProfit = (item.Value.SalesPrice-item.Value.BoughtPrice) * item.Value.QuantitySold;
                combinedData.Row.Add(combinedCM);
            }
            return combinedData;
         
        }

    }
}

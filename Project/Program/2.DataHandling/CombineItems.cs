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

            foreach (var item in UniqueProducts) 
            {
                CanonicalModel combinedCM = item.Value;
                combinedCM.GrossProfit = (item.Value.SalesPrice-item.Value.BoughtPrice) * item.Value.QuantitySold;
                combinedData.Row.Add(combinedCM);
            }

            //using linq to combine data to see if my own methods is losing data
            var allItemGroups = combinedData.Row.Select(x => x.ItemgroupName).Distinct();
            foreach (string itemGroupName in allItemGroups) 
            {
                Summary sum = new();

                var bar = combinedData.Row.Where(x => x.ItemgroupName == itemGroupName).ToList();

                sum.SummaryName = itemGroupName;
                sum.TotalSalesLinq = bar.Sum(x => x.QuantitySold);
                sum.GrossProfitLinq = bar.Sum(x => x.GrossProfit);

                SummariesStaticList.Summaries.Add(sum);
            }

            return combinedData;
         
        }

    }
}

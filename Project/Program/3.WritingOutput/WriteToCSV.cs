using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WritingOutput
{
    internal class WriteToCSV
    {

        public void ToCSV(OutputFormatter outputFormatter, char delimeter = ';')
        {
            //get output file location
            string docPathSum = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/outputSummery.csv";
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/output.csv";

            //summary
            string headersForSummary = $"SummaryName{delimeter}GrossProfit{delimeter}TotalSales\r\n";
            File.WriteAllText(docPathSum, headersForSummary);
            List<string> summeryOutput = new();
            foreach (Summary x in outputFormatter.summaries) 
            {
                summeryOutput.Add(x.SummaryName + delimeter + x.GrossProfitLinq + delimeter + x.TotalSalesLinq);
            }
            File.AppendAllLines(docPathSum, summeryOutput);
            
            
            // full details
            string headersForFullData = $"Category{delimeter}EAN{delimeter}Size{delimeter}GrossProfit{delimeter}SalesPrice{delimeter}BoughtPrice{delimeter}QuantitySold\r\n";
            File.WriteAllText(docPath, headersForFullData);

            foreach (var category in outputFormatter.FullDetails.Keys)
            {
                List<string> fullData = new();
                foreach (var item in outputFormatter.FullDetails[category])
                {
                   fullData.Add(category + delimeter +
                        item.EAN + delimeter +
                        item.Size + delimeter +
                        item.GrossProfit + delimeter +
                        item.SalesPrice + delimeter +
                        item.BoughtPrice + delimeter +
                        item.QuantitySold);
                }
                
                File.AppendAllLines(docPath, fullData);
            }
        }


    }
}

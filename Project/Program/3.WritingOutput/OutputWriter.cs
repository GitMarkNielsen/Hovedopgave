using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WritingOutput
{
    internal class OutputWriter
    {
        public static void ToJSON(OutputFormatter stuffToSerialize, string fileName)
        {
            fileName = CleanupFileName(fileName);
            string docPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output", fileName + "SummaryForIT.json");


            string objToJSON = JsonSerializer.Serialize(stuffToSerialize, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(docPath, objToJSON);
        }

        public static void ToCSV(OutputFormatter outputFormatter, string fileName, char delimeter = ';')
        {
            fileName = CleanupFileName(fileName);
            //get output file location
            string CurrentFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Output");
            string docPathSum = Path.Combine(CurrentFolder, fileName + "ShortSummary.csv");
            
           
            string docPath = Path.Combine(CurrentFolder, fileName + "Summary.csv"); ;
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

        private static string CleanupFileName(string fileName)
        {
            string[] splitString = fileName.Split('\\');
            string outString = splitString[splitString.Length - 1];
            
            return outString.Split('.')[0];
        }
    }
}

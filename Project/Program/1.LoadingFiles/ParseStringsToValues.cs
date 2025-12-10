using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadingFiles
{
    public class ParseStringsToValues : IDisposable
    {
        public void Dispose()
        {
            
        }

        public InhouseData ParseValues(CSV_DBO data)
        {
            //CanonicalModel is the data from a CSV file in the correct data types
            InhouseData ParsedCSV = new();
            for (int i = 0; i < data.AllRows.Count; i++)
            {
            CanonicalModel CM = new();
                for (int j = 0; j < data.HeaderValues.Count; j++)
                {
                    switch (data.HeaderValues[j].ToUpper())
                    {
                        case "EAN":
                            CM.EAN = int.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "SIZE":
                            CM.Size = data.AllRows[i].Columns[j];
                            break;
                        case "SOLDPRICE":
                            CM.SalesPrice = Decimal.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "BOUGHTPRICE":
                            CM.BoughtPrice=Decimal.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "SALES":
                            CM.QuantitySold = int.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "ITEMGROUPNAME":
                            CM.ItemgroupName = data.AllRows[i].Columns[j];
                            break;
                        default:
                            CM.Unknown = data.AllRows[i].Columns[j];
                            break;
                    }
                }
                ParsedCSV.Row.Add(CM);
            }
            return ParsedCSV;


        }

       

    }

}

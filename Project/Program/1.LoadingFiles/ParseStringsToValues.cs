using Program._4.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace LoadingFiles
{
    public class ParseStringsToValues
    {
        public void ParseValues(CSV_DBO data)
        {
            //CanonicalModel is the data from a CSV file in the correct data types
            CanonicalModel CM = new();
            InhouseData inhouseData = new();
            for (int i = 0; i < data.AllRows.Count; i++)
            {
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
                        case "TURNOVER":
                            CM.Turnover = Decimal.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "GROSSPROFIT":
                            CM.GrossProfit=Decimal.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "COSTPRICE":
                            CM.CostPrice = Decimal.Parse(data.AllRows[i].Columns[j]);
                            break;
                        case "VAT":
                            CM.VAT = Decimal.Parse(data.AllRows[i].Columns[j]);
                            break;
                        default:
                            CM.Unknown = data.AllRows[i].Columns[j];
                            break;
                    }
                }
                inhouseData.Row.Add(CM);
            }

            //send inhouseData to next step in pipeline


        }

    }

}

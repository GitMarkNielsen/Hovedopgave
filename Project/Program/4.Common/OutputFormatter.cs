using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common
{
    internal static class OutputFormatter
    {

        public static List<Summary> summaries { get; set; } = new();
        
        public static Dictionary<string, List<CanonicalModel>> FullDetails { get; set; } = new();
        public static string ToJson
        {
            get
            {
                var x = Summary();
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            }
        }

        internal class Summary
        {
            public string SummaryName { get; set; }
            public decimal GrossProfitLinq { get; set; }
            public decimal TotalSalesLinq { get; set; }
            public Dictionary<string, List<CanonicalModel>> Fulldetails { get; set; } = OutputFormatter.FullDetails;


        }

    }

   


}

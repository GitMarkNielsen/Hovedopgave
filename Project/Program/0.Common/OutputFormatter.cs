using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common
{
    internal class OutputFormatter
    {
        public  List<Summary> summaries { get; set; } = new();
        public  Dictionary<string, List<CanonicalModel>> FullDetails { get; set; } = new();

        public OutputFormatter() 
        {
            summaries = SummariesStaticList.Summaries;
        }

    }

  


}

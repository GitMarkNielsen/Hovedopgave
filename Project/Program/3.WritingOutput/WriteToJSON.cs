using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WritingOutput
{
    internal class WriteToJSON
    {
        public static void ObjectToJSON(Dictionary<string, List<CanonicalModel>> sortedValues)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            OutputFormatter.FullDetails = sortedValues;
              var x  = JsonSerializer.Serialize(OutputFormatter.FullDetails, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(docPath + "/output.json", OutputFormatter);
        }

    }
}

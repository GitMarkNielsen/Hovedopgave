using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WritingOutput
{
    internal class WriteToJSON
    {
        public static void ObjectToJSON(Dictionary<string, List<CanonicalModel>> sortedValues)
        {
            string objToJSON = JsonSerializer.Serialize(sortedValues);
            File.WriteAllText("../../../5.Output/output.json", objToJSON);
        }

    }
}

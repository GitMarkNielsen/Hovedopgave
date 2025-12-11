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
        public static void ObjectToJSON(OutputFormatter stuffToSerialize)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            string objToJSON = JsonSerializer.Serialize(stuffToSerialize, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(docPath + "/output.json", objToJSON);
        }

    }
}

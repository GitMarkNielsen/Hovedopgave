using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Each Instance of InhouseData is the entire contents of a CSV file represented in CanonicalModels, which is a single row of a CSV
    /// </summary>
    public class InhouseData
    {
        public List<CanonicalModel> Row{ get; set; } = new List<CanonicalModel>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Tests
{
    internal class CategoryDBO
    {
        public string ItemGroupName { get; set; }
        public int PriceMin { get; set; }
        public int PriceMax { get; set; }
        public string[] Sizes { get; set; }

        public CategoryDBO(string ItemGroupName, int PriceMin, int PriceMax, string[] Sizes) {
            this.ItemGroupName = ItemGroupName;
            this.PriceMin = PriceMin;
            this.PriceMax = PriceMax;
            this.Sizes = Sizes;
        }
    }
    
}

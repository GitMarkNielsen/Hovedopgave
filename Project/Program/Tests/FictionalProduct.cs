using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Tests
{
    internal class FictionalProduct
    {
        public int EAN { get; set; }
        public string ItemGroupName { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }

        public FictionalProduct(string ItemGroupName, int Price, string Size) {
            this.ItemGroupName = ItemGroupName;
            this.Price = Price;
            this.Size = Size;
        }

        public FictionalProduct() { }
    }
}

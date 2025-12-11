using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests
{
    internal class GenerateInputFiles
    {



        //Setup
        //↓ length of file
        int finalLineCount = 2500;

        //↓Fine tune possible data
        List<CategoryDBO> categoryOptions = new()
        { new CategoryDBO("Top", 20, 80, ["3XS","2XS","XXS","X242S", "S", "M", "L", "XL", "2XL","3XL","4XL"]),
          new CategoryDBO("Pants", 40, 120, ["28W/30L", "30W/32L", "32W/32L", "34W/34L", "36W/34L", "38W/36L"]),
          new CategoryDBO("Socks", 5, 15, ["One Size","25-30" ,"30-36", "36-45"]),
          new CategoryDBO("Bra", 30, 70, ["32A", "32BB", "32B", "32C", "36C", "36DD", "36D", "40D"]),
          new CategoryDBO("Jacket", 100,450,["S", "M", "L", "XL", "2XL"]),
        };


        //setup list of the products so we can use it in another method
        List<FictionalProduct> availableProducts = new();
        Random rand = new Random();

        public void MakeData()
        {
            MakeFictionalProducts();
            GenerateCSV();
        }

        private void MakeFictionalProducts()
        {
            // modify ↓ to change total amount of items
            int differentItems = 250;
            for (int i = 0; i < differentItems; i++)
            {
                FictionalProduct nextProduct = new FictionalProduct();

                int currentCategory = rand.Next(0, categoryOptions.Count - 1);
                int minPrice = categoryOptions[currentCategory].PriceMin;
                int maxPrice = categoryOptions[currentCategory].PriceMax;
                int sizeCount = categoryOptions[currentCategory].Sizes.Count() - 1;
                int EAN = rand.Next(10000000, 99999999);

                nextProduct.EAN = EAN;
                nextProduct.ItemGroupName = categoryOptions[currentCategory].ItemGroupName;
                nextProduct.SoldPrice = rand.Next(minPrice, maxPrice);

                nextProduct.BoughtPrice = Math.Round(nextProduct.SoldPrice * double.Parse("0," + rand.Next(70, 90)), 2);
                nextProduct.Size = categoryOptions[currentCategory].Sizes[rand.Next(0, sizeCount)];

                availableProducts.Add(nextProduct);
            }
        }




        private void GenerateCSV()
        {
            string headers = "EAN;Sales;Size;SoldPrice;BoughtPrice;ItemGroupName\r\n";


            //writingData
            string filePath = "../../../0.InputFiles/DummyData.csv";
            File.WriteAllText(filePath, headers);
            List<string> lines = new List<string>();
            for (int i = 0; i < finalLineCount; i++)
            {
                int productNumber = rand.Next(0, availableProducts.Count);

                int salesQuantity = rand.Next(0, 10);
                int EAN = availableProducts[productNumber].EAN;
                string Size = availableProducts[productNumber].Size;
                double soldprice = availableProducts[productNumber].SoldPrice;
                double boughtPrice = availableProducts[productNumber].BoughtPrice;
                string itemGroupName = availableProducts[productNumber].ItemGroupName;


                lines.Add($"{EAN};{salesQuantity};{Size};{soldprice};{boughtPrice};{itemGroupName}");
            }
            File.AppendAllLines(filePath, lines);
        }
    }
}
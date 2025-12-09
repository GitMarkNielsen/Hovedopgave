using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Program.Tests
{
    internal class GenerateInputFiles
    {



    //variables to make the file
        string FileName = "clothing_sales_data.csv";
        int finalLineCount = 2500;
        int Year = 2025;
        int Month= 12;

        List<CategoryDBO> categoryOptions = new()
        { new CategoryDBO("Top", 20, 80, ["3XS","2XS","XXS","XS", "S", "M", "L", "XL", "2XL","3XL","4XL"]),
          new CategoryDBO("Pants", 40, 120, ["28W/30L", "30W/32L", "32W/32L", "34W/34L", "36W/34L", "38W/36L"]),
          new CategoryDBO("Socks", 5, 15, ["One Size","25-30" ,"30-36", "36-45"]),
          new CategoryDBO("Bra", 30, 70, ["32A", "32BB", "34B", "34C", "36C", "36DD", "38D"]),
          new CategoryDBO("Jacket", 100,450,["S", "M", "L", "XL", "2XL"]),
        };



        List<FictionalProduct> availableProducts = new();
            Random rand = new Random();

        public void GenerateCSV()
        {

            FictionalProduct nextProduct = new FictionalProduct();

            int currentCategory = rand.Next(0, categoryOptions.Count - 1);
            int minPrice = categoryOptions[currentCategory].PriceMin;
            int maxPrice = categoryOptions[currentCategory].PriceMax;
            int sizeCount = categoryOptions[currentCategory].Sizes.Count() - 1;
            int EAN = rand.Next(10000000, 99999999);
            
            nextProduct.EAN = EAN;
            nextProduct.ItemGroupName = categoryOptions[currentCategory].ItemGroupName;
            nextProduct.Price = rand.Next(minPrice, maxPrice);
            nextProduct.Size = categoryOptions[currentCategory].Sizes[rand.Next(0, sizeCount)];
            
            availableProducts.Add(nextProduct);
        }




        public void generate_csv() {
            string headers = "EAN;Sales;Size;SoldPrice;BoughtPrice;ItemGroupName;";


            //writingData
            string Filepath = "C:\\Projects\\Hovedopgave\\Hovedopgave\\Project\\Program\\0.InputFiles\\DummyData.csv";
            File.WriteAllText(Filepath, headers);

           for (int i = 0; i < finalLineCount; i++)
            {
                int productNumber = rand.Next(0,availableProducts.Count);

                int salesQuantity = rand.Next(0,10);


            }

        for _ in range(NUM_ENTRIES):
            # Pick a random product from our catalog (simulating scanning an item)
            item = random.choice(catalog)

            # Logic: Sales (Quantity)
            sales_qty = random.randint(1, 10)

            # Logic: Sold Price
# We add small variance to sold price to simulate minor discounts/fluctuations
            variance = random.uniform(0.95, 1.05)
            sold_price = round(item["base_sold_price"] * variance, 2)

            # Logic: Bought Price (Margin 5% - 30%)
# Margin calculation: SoldPrice - (SoldPrice * Margin%) = BoughtPrice
# Wait, BoughtPrice needs to be lower. 
# If Margin is 30%, BoughtPrice is 70% of SoldPrice.
# If Margin is 5%, BoughtPrice is 95% of SoldPrice.
            margin_percent = random.uniform(0.05, 0.30)
            bought_price = round(sold_price * (1 - margin_percent), 2)

            # Logic: Date
            date_str = generate_random_date(YEAR, MONTH)


            writer.writerow([
                item["ean"],
                sales_qty,
                item["size"],
                sold_price,
                bought_price,
                item["group_name"],
                date_str
            ])
            }
    print(f"Successfully generated {FILENAME} with {NUM_ENTRIES} entries.")

if __name__ == "__main__":
    generate_csv()import csv
import random
import datetime

# Configuration
FILENAME = "clothing_sales_data.csv"
NUM_ENTRIES = 2500
YEAR = 2023
MONTH = 10  # October

# Product Categories and their logic
# Structure: Name: {Sizes, PriceRange(min, max)}
CATEGORIES = {
    "Top": {
        "sizes": ["XS", "S", "M", "L", "XL", "XXL"],
        "price_min": 20, "price_max": 80
    },
    "Pants": {
        "sizes": ["28W/30L", "30W/32L", "32W/32L", "34W/34L", "36W/34L", "38W/36L"],
        "price_min": 40, "price_max": 120
    },
    "Socks": {
        "sizes": ["One Size", "S-M", "M-L"],
        "price_min": 5, "price_max": 15
    },
    "Bra": {
        "sizes": ["32A", "32B", "34B", "34C", "36C", "36D", "38D"],
        "price_min": 30, "price_max": 70
    },
    "Jacket": {
        "sizes": ["S", "M", "L", "XL"],
        "price_min": 100, "price_max": 300
    },
    "Dress": {
        "sizes": ["2", "4", "6", "8", "10", "12"],
        "price_min": 50, "price_max": 150
    }
}

# 1. Generate a catalog of "Products" first
# This ensures that if an EAN appears twice, it refers to the same Item/Size combo
# We will create 50 unique products to simulate a store inventory
catalog = []
for _ in range(50):
    category_name = random.choice(list(CATEGORIES.keys()))
    cat_data = CATEGORIES[category_name]
    
    product = {
        "ean": random.randint(10000000, 99999999),
        "group_name": category_name,
        "size": random.choice(cat_data["sizes"]),
        # Base sold price for this specific product
        "base_sold_price": round(random.uniform(cat_data["price_min"], cat_data["price_max"]), 2)
    }
    catalog.append(product)

def generate_random_date(year, month):
    """Generates a random datetime within the specified month."""
    # Days in month logic (simplified)
    if month == 2:
        max_days = 28
    elif month in [4, 6, 9, 11]:
        max_days = 30
    else:
        max_days = 31
        
    day = random.randint(1, max_days)
    hour = random.randint(9, 21) # Store hours 9am to 9pm
    minute = random.randint(0, 59)
    second = random.randint(0, 59)
    
    return datetime.datetime(year, month, day, hour, minute, second)

def generate_csv():
    headers = ["EAN", "Sales", "Size", "SoldPrice", "BoughtPrice", "ItemGroupName", "Date"]
    
    with open(FILENAME, mode='w', newline='') as file:
        writer = csv.writer(file)
        writer.writerow(headers)
        
        for _ in range(NUM_ENTRIES):
            # Pick a random product from our catalog (simulating scanning an item)
            item = random.choice(catalog)
            
            # Logic: Sales (Quantity)
            sales_qty = random.randint(1, 10)
            
            # Logic: Sold Price
            # We add small variance to sold price to simulate minor discounts/fluctuations
            variance = random.uniform(0.95, 1.05) 
            sold_price = round(item["base_sold_price"] * variance, 2)
            
            # Logic: Bought Price (Margin 5% - 30%)
            # Margin calculation: SoldPrice - (SoldPrice * Margin%) = BoughtPrice
            # Wait, BoughtPrice needs to be lower. 
            # If Margin is 30%, BoughtPrice is 70% of SoldPrice.
            # If Margin is 5%, BoughtPrice is 95% of SoldPrice.
            margin_percent = random.uniform(0.05, 0.30)
            bought_price = round(sold_price * (1 - margin_percent), 2)
            
            # Logic: Date
            date_str = generate_random_date(YEAR, MONTH)
            
            writer.writerow([
                item["ean"],
                sales_qty,
                item["size"],
                sold_price,
                bought_price,
                item["group_name"],
                date_str
            ])

    print(f"Successfully generated {FILENAME} with {NUM_ENTRIES} entries.")

if __name__ == "__main__":
    generate_csv()



    }
}

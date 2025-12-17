using Common;
using DataHandling;
using LoadingFiles;
using System.Net.Http.Headers;
using Tests;
using WritingOutput;



public class Program
{
    public static string FolderToSearch { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/GoFactLight/Input";

    public static void Main(string[] args)
    {
        //-------------------------
        //0.Setup synth data
        //-------------------------
        GenerateInputFiles testmaker = new();
        testmaker.MakeData(FolderToSearch);

        Console.WriteLine($"Searching folder: {FolderToSearch}");

        //______________________________
        //1. Loading Files
        //______________________________
        LoadAllFiiles loadAllFiles = new();
        List<CSV_DBO> AllFiles = loadAllFiles.LoadCSVFiles(FolderToSearch);

        List<InhouseData> parsedData = new();
        //using statement, so it will get cleaned up as soon as the parser isn't needed anymore.
        //probably not needed, but i wanted to do this ¯\_(ツ)_/¯
        using (ParseStringsToValues parser = new())
        {
            foreach (CSV_DBO LoadedFile in AllFiles)
            {
                parsedData.Add(parser.ParseValues(LoadedFile));
            }
        }


        //-------------------------------------
        // 2. Data Handling
        //-------------------------------------

        //Compressing data for optimization
        CombineItems CombineFactory = new();
        List<InhouseData> compressedData = new();
        foreach (InhouseData parsedFile in parsedData)
        {
            InhouseData compressedFile = CombineFactory.Combine(parsedFile);
            compressedData.Add(compressedFile);
        }

        List<Dictionary<string, List<CanonicalModel>>> finishedData = new();
        //string is the category, list<CM> is all the entries within that category
        Dictionary<string, List<CanonicalModel>> sortedValues = new();
        //sorting into categories so the sorting makes sense
        foreach (InhouseData compressedFile in compressedData)
        {
            Dictionary<string, List<CanonicalModel>> categories = new();
            //each file
            //We add each type in the file to a dictionary
            foreach (CanonicalModel row in compressedFile.Row)
            {
                if (categories.ContainsKey(row.ItemgroupName))
                    categories[row.ItemgroupName].Add(row);
                else
                {
                    categories[row.ItemgroupName] = new() { row };
                }
            }
            //then sort within each category.

            foreach (string category in categories.Keys)
            {
                List<CanonicalModel> sortedByCategory = sortingBySize.SmartSorter(categories[category]);
                sortedValues[category] = sortedByCategory;
            }


            //__________________________________________________________
            //SORTING BY SIZE IS NOW DONE
            //__________________________________________________________

            //after each category is sorted, we sort within each size, so the highest sales is at the top
            
            //We go through each category, which is already sorted by size.
            foreach (string category in sortedValues.Keys)
            {
                //SalesSortedList will contain the different sizes
                Dictionary<string, List<CanonicalModel>> SalesSortedList = new();
                //FinalSortedListISwear is the category, sorted within Size and QuantSold
                List<CanonicalModel> FinalSortedListISwear = new();

                //We go through each item in the category
                foreach (CanonicalModel listItem in sortedValues[category])
                {
                    //if we haven't seen the size before, we make a new entry in the size dictionary
                    if (!SalesSortedList.ContainsKey(listItem.Size))
                    {
                        //we set the sortingIndex to be "-listItem.QuantitySold", as the sorting goes low to high.
                        //Where we want this output to be high to low. So we "reverse" the ordering behind the scenes.
                        listItem.SortingIndex = -listItem.QuantitySold;
                        SalesSortedList.Add(listItem.Size, new List<CanonicalModel>() { listItem });
                    }
                    else
                    {
                        //same as above, but we dont make a new entry, we just add to the existing size list.
                        listItem.SortingIndex = -listItem.QuantitySold;
                        SalesSortedList[listItem.Size].Add(listItem);
                    }
                }
                //Here we have a category, with updated sorting index based on the sales numbers
                //And they are in seperate lists based on their size.
                foreach (string size in SalesSortedList.Keys)
                { //in this loop, we're going over all entires in each size in the specefic category

                    //we make a new list<CM> as the method we're calling needs a ref. 
                    List<CanonicalModel> TempList = SalesSortedList[size];
                    sortingBySize.InsersionSortByIndex(ref TempList);

                    foreach (CanonicalModel sortedListItems in TempList)
                    { 
                        //Kinda ugly, but we go over each item that's been sorted now, and add it to a running list. 
                        //the list will over time contain each item sorted by size within the category.
                        FinalSortedListISwear.Add(sortedListItems);
                    }
                }
                //when we're done with this category, we update the final dictionary with the new "double" sorted list.
                sortedValues[category] = FinalSortedListISwear;

            }

            //_______________________________
            //3. Writing Output
            //-------------------------------
            OutputFormatter outputFormatter = new OutputFormatter();
            outputFormatter.FullDetails = sortedValues;

            OutputWriter.ToJSON(outputFormatter);
            OutputWriter.ToCSV(outputFormatter);

        }
        //End Of Main
    }
}
using Common;
using DataHandling;
using LoadingFiles;
using Tests;
using WritingOutput;



public class Program
{
    public static string FolderToSearch { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/GoFactLight";

    public static void Main(string[] args)
    {

        GenerateInputFiles testmaker = new();
        testmaker.MakeData(FolderToSearch);

        Console.WriteLine($"Searching folder: {FolderToSearch}");
        LoadAllFiiles loadAllFiiles = new();
        List<CSV_DBO> AllFiles = loadAllFiiles.LoadCSVFiles(FolderToSearch);

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

        //Compressing data for optimization
        CombineItems CombineFactory = new();
        List<InhouseData> compressedData = new();
        foreach (InhouseData parsedFile in parsedData)
        {
            InhouseData compressedFile = new();
            compressedFile = CombineFactory.Combine(parsedFile);
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
                    categories[row.ItemgroupName] = new(){row};
                } 
            }
            //then sort within each category.
            
            foreach (string category in categories.Keys)
            {
                List<CanonicalModel> sortedByCategory = sortingBySize.SmartSorter(categories[category]);
                sortedValues[category] = sortedByCategory;
            }

            //after each category is sorted, we sort within each size, so the highest sales is at the top
            foreach (var item in sortedValues.Keys)
            {
                foreach(var listItem in sortedValues[item])
                {
                    //we set the sortingIndex to be -quantsold, as the sorting goes low to high. Where we want this output to be high to low.
                    listItem.SortingIndex = -listItem.QuantitySold;
                }
                List<CanonicalModel> sortedBySalesInCategory = sortedValues[item];
                //the method should be private, but this way i can go to it directly
                sortingBySize.InsersionSortByIndex(ref sortedBySalesInCategory);
                sortedValues[item] = sortedBySalesInCategory;
            }
        }

        //_______________________________
        //3. Writing Output
        //-------------------------------
        OutputFormatter outputFormatter = new OutputFormatter();
        outputFormatter.FullDetails = sortedValues;

        WriteToJSON.ObjectToJSON(outputFormatter);

        WriteToCSV toCSV = new WriteToCSV();
        toCSV.ToCSV(outputFormatter);


    }
}
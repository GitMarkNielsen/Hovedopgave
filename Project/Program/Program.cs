using Common;
using DataHandling;
using LoadingFiles;
using Tests;
using WritingOutput;



public class Program
{
    public static string FolderToSearch { get; set; } = "../../../0.InputFiles";

    public static void Main(string[] args)
    {

        GenerateInputFiles testmaker = new();
        testmaker.MakeData();

        Console.WriteLine($"Searching folder: {FolderToSearch}");
        LoadAllFiiles loadAllFiiles = new();
        List<CSV_DBO> AllFiles = loadAllFiiles.LoadCSVFiles(FolderToSearch);

        List<InhouseData> parsedData = new();
        //using statement, so it will get cleaned up as soon as the parser isn't needed anymore.
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
        }

        //_______________________________
        //3. Writing Output
        //-------------------------------
        OutputFormatter outputFormatter = new OutputFormatter();
        outputFormatter.FullDetails = sortedValues;

        WriteToJSON.ObjectToJSON(outputFormatter);




    }
}
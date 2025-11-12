using Common;
using DataHandling;
using LoadingFiles;


public class Program
{
    public static string FolderToSearch { get; set; } = "../../../0.InputFiles";

    public static void Main(string[] args)
    {
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
            //Debug writing contents to console ↓
            #region Debug
            for (int i = 0; i < parsedData.Count; i++)
            {
                foreach (CanonicalModel canonicalModel in parsedData[i].Row)
                {
                    Console.WriteLine(canonicalModel.ToString());
                }
            }
            #endregion
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

        //sorting into categories so the sorting makes sense
        Dictionary<string, List<CanonicalModel>> categories = new();
        sortingBySize sorter = new();
        foreach (InhouseData compressedFile in compressedData)
        {
            //each file
            foreach (CanonicalModel row in compressedFile.Row)
            {
                //each row
                categories[row.ItemgroupName].Add(row);
            }
            sorter.Sorter();

        }

        /*
         * I've compressed the data, now i just have to Sort all the T-shirts together, shoes together ect. 
         * In the input-file, There's a column for "ItemgroupName" which will be what i need for this.
         * 
         * I'll sort them into lists seperating the categories, and call my smartSort on each list.
         * 
         * After that, they should be ready to output to a file.
         * I'll just take each list and write the contents 1-1 into a JSON or CSV file
         * it will be 1 file with all the data in it.
         * I Can output to console how many of each category there is to make sure the data is correct
         * 
         * 
         */


    }
}
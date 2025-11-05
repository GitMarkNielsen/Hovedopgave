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
        //testing
        CombineItems foobar = new();
        foobar.Combine(parsedData[0]);

        /*
         * from here on out, i have all the data from a CSV loaded into memory. I should now combine as much data as possible.
         * I'll do this by reading each EAN or other identifier, checking a running list of what EANS we've seen so far
         * If it's a match, i'll increase a count by 1. 
         * Maybe using a dictionary. Adding a new key every time i see a new identifier.
         * Where the value is the amount of times it's been seen.
         * 
         * The result will be a dictionary that's potentially many thousands lines long, that contains the identifier and the times it's shown up.
         * i'll use that data to find the combined cost to use in my final output.
         * When i have a list of all the unique products, i'll do another loop over each "type". So all types of the same clothing will be sorted the same.
         * to make this happen, i'll make a list of all the potential categories it could be,
         * and when looping over the unique products, i'll add the EAN to the category it fits in.
         * 
         * To make that happen, i'll need a "Category" column in the CSV, or i'll find it out based on the size.
         * Using the size would mean i would re-use what i've written, but change it to serve a different purpose. 
         * When i have a list of all the products in the category, i'll sort them within each category. 
         * Then pass the list of those items along to the next step, where it will combine the size, amount, costs ect. 
         * Then write it to a new CSV or Json
         * And that's the end of the program, for now.
         */


    }
}
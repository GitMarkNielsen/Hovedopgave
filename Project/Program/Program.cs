using Common;
using LoadingFiles;

public class Program
{
    public static string FolderToSearch { get; set; } = "../../../0.InputFiles";

    public static void Main(string[] args)
    {
        Console.WriteLine($"Searching folder: {FolderToSearch}");
        LoadAllFiiles loadAllFiiles = new LoadAllFiiles();
        List<CSV_DBO> AllFiles = loadAllFiiles.LoadCSVFiles(FolderToSearch);

        List<InhouseData> parsedData = new();
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
    }
}
using System;
using System.IO;
using System.Collections;
public class LoadAllFiiles
{
    public string FolderToSearch { get; set; } = "../0.FilesToSearch";
    public string[] AllFilePaths { get; set; }


    public LoadAllFiiles()
	{
        AllFilePaths = Directory.GetFiles(FolderToSearch);
        foreach (string file in AllFilePaths)
        {   
        using(var reader = new StreamReader(file))
            {
            MapCSVToCs CSVData = new ();
                reader.ReadLine();
            }
            //send each file down the pipeline
        }
	}
}

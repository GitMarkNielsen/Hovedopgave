using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
public class LoadAllFiiles
{
    public string FolderToSearch { get; set; } = "../../../0.InputFiles";
    public string[] AllFilePaths { get; set; }


    public LoadAllFiiles()
    {
        AllFilePaths = Directory.GetFiles(FolderToSearch);
        foreach (string file in AllFilePaths)
        {
            char delimeter = ';';
            using (var reader = new StreamReader(file))
            {
                //if the file has no data
                if (reader.EndOfStream)
                {
                    return;
                }

                CSV_DBO CSVData = new();
                //Header from CSV
                string line = reader.ReadLine();
                delimeter = FindSeperationChar(line);
                foreach (string item in line.Split(delimeter))
                {
                    CSVData.HeaderValues.Add(item);
                }

                //data from CSV
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    Rows newRow = new();
                    foreach (string column in line.Split(delimeter))
                    {
                        newRow.Columns.Add(column);
                    }
                    CSVData.AllRows.Add(newRow);
                }
                //from here, the CSVData variable should contain all the data from the CSV.
                Console.Write(CSVData.ToString());
            }
            //send each file down the pipeline
        
        }
    }

    private char FindSeperationChar(string RowOfCSV)
    {
        //have the most likely at first, as a fix for if every field has a decimal
        //will break if there's a reading dot in big numbers. as it will outnumber the actual delimeter.
        //update this↓ char[] with new seperator if a file uses a delimeter not in this array.
        char[] possibleSeperators = [
            ';',
            ':',
            ',',
            '.',
            '|',
            '\\',
            '/'];
        int highest = 0;
        char seperatorCharInTheCSV = ';';
        foreach (char c in possibleSeperators)
        {
            string Pattern = $@"(\{c})";
            int count = Regex.Count(RowOfCSV, Pattern);
            if (count > highest)
            {
                highest = count;
                seperatorCharInTheCSV = c;
            }
        }
        return seperatorCharInTheCSV;
    }



}

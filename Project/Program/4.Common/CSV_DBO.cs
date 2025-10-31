using System;
using System.Text;

public class CSV_DBO
{
    public List<string> HeaderValues { get; set; } = new List<string>();
    public List<Rows> AllRows { get; set; } = new List<Rows>();

    /// <summary>
    /// Debug the CSV_DBO. 
    /// </summary>
    /// <returns>header plus 5 lines</returns>
    public override string ToString()
    {
        StringBuilder returnstring = new();
        foreach (string headerValue in HeaderValues)
        {
            returnstring.Append(headerValue + "; ");

        }
        returnstring.Append('\n');
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < AllRows[0].Columns.Count; j++)
            {
                returnstring.Append(AllRows[i].Columns[j] + "; ");
            }
            returnstring.Append('\n');
        }

        return returnstring.ToString();
    }

}

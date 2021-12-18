using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using System.Linq;

public class CSVLoader
{
    TextAsset csvFile;
    string filename = "";
    static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

    public CSVLoader(string filename)
    {
        this.filename = filename;
        Load();
    }


    public void Load()
    {
        csvFile = Resources.Load<TextAsset>(filename);
    }


    public Dictionary<string, string> GetDictionaryColumn(string key)
    {
        string[] lines = csvFile.text.Split('\n');
        string[] headers = CSVParser.Split(lines[0]);

        // Get column
        int columnIndex = -1;
        for (int i = 0; i < headers.Length; i++)
            if (headers[i].Contains(key.ToLower()))
            {
                columnIndex = i;
                break;
            }

        return GetDictionaryColumn(columnIndex);
    }


    public Dictionary<string, string> GetDictionaryColumn(int index)
    {
        //Debug.Log("CSV text: " + csvFile.text);
        Dictionary<string, string> dict = new Dictionary<string, string>();
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = CSVParser.Split(lines[i]);
            if (fields.Length <= index)
                continue;

            fields[0] = fields[0].Replace("\\n", "\n").TrimStart(' ', '"', '\"').TrimEnd('"', '\"');
            fields[index] = fields[index].Replace("\r", "").Replace("\\n", "\n").TrimStart(' ', '"', '\"').TrimEnd('"', '\"', '\n');
            //Debug.Log("Criando: " + fields[0] + ", " + fields[index] + "|");
            dict.Add(fields[0], fields[index]);
        }

        Debug.Assert(dict.Count > 0);
        return dict;
    }


    public bool LineExists(string key)
    {
        string[] lines = csvFile.text.Split('\n');
        foreach (string line in lines)
            if (CSVParser.Split(line)[0].Contains(key.ToLower()))
                return true;
        
        return false;
    }

#if UNITY_EDITOR
    public void AddLine(string key, string[] values)
    {
        string[] parsed_lines = csvFile.text.Split('\n');
        string[] lines = new string[parsed_lines.Length+1];
        for (int i = 0; i < parsed_lines.Length; i++)
            lines[i] = parsed_lines[i];

        lines[parsed_lines.Length] = "\"" + key + "\"";
        foreach(string value in values)
            lines[parsed_lines.Length] += ", \"" + value + "\"";

        WriteToFile(lines);
    }

    public void AddColumn(string key, string[] values)
    {
        string[] lines = csvFile.text.Split('\n');

        lines[0] += ", " + key;
        for (int i = 1; i < values.Length; i++)
        {
            lines[i] += ", " + values[i];
        }

        WriteToFile(lines);
    }

    public void SetCell(string lineKey, string columnKey, string value)
    {
        string[] lines = csvFile.text.Split('\n');
        string[] headers = CSVParser.Split(lines[0]);

        // Get column
        int columnIndex = -1;
        for (int i = 0; i < headers.Length; i++)
            if (headers[i].Contains(columnKey.ToLower()))
            {
                columnIndex = i;
                break;
            }

        SetCell(lineKey, columnIndex, value);
    }

    public void SetCell(string lineKey, int columnIndex, string value)
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parsed_fields = CSVParser.Split(lines[i]);
            string[] fields = parsed_fields;

            // After this point, only the desired line will run
            if (!fields[0].Contains(lineKey.ToLower()))
                continue;

            // In case the chosen line doesn't have enough columns, populate them with empty strings until the
            // desired column is reached
            if (columnIndex >= fields.Length)
            {
                fields = new string[columnIndex+1];
                for (int f = 0; f < parsed_fields.Length; f++)  // Populate known columns
                    fields[f] = parsed_fields[f];
                for (int f = parsed_fields.Length; f <= columnIndex; f++)    // Populate unknown columns
                    fields[f] = "";
            }

            // Add field
            for (int f = 0; f < fields.Length; f++)
                fields[f] = fields[f].TrimStart(' ');
            fields[columnIndex] = '"' + value + '"';
            lines[i] = string.Join(", ", fields);
            break;
        }

        WriteToFile(lines);
    }

    void WriteToFile(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
            lines[i] = lines[i].TrimEnd(' ', '\r', '\n') + '\n';
        File.WriteAllLines("Assets/Resources/" + filename + ".csv", lines.Where(w => w.Length > 2));
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}

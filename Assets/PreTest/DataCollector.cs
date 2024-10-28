using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    public static List<Data> DataList = new();

    [ContextMenu("ExportCSV")]
    public static string ExportCSV()
    {
        var sb = new StringBuilder(Data.COLUMNS);
        foreach(var data in DataList) 
        {
            sb.Append('\n').Append(data.ToString());
        }
        var folder = Application.persistentDataPath;
        var filePath = Path.Combine(folder, $"{DateTime.Now:y-M-d HH_mm_ss} export.csv");
        File.WriteAllText(filePath, sb.ToString());
        //AssetDatabase.Refresh();
        Debug.Log($"CSV file written to \"{filePath}\"");
        return filePath;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteDataToText : MonoBehaviour
{
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Data/");

      //  CreateTextFile();
    }


    public void CreateTextFile()
    {
        string txtDocumentName = Application.streamingAssetsPath + "/Data/" + "MiniMaxSimple-MiniMaxAdvanced" + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "Title \n");
        }
    }

    static public void CreateTextFile(string txtFileName, string folderName)
    {

        string directoryName = Application.streamingAssetsPath + "/Data/" + folderName + "/";

        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        string txtDocumentName = directoryName + txtFileName + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "");
        }
    }


    static public void WriteText(string txtFileName, string folderName, int line, string info)
    {

        string txtDocumentName = Application.streamingAssetsPath + "/Data/" + folderName + "/" + txtFileName + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            Debug.LogWarning("File didn't exist yet, making new file");
            CreateTextFile(txtFileName, folderName);
        }

        LineChanger(info, txtDocumentName, line);


    }

    static public void LineChanger(string newText, string fileName, int line_to_edit)
    {
        string[] arrLine = File.ReadAllLines(fileName);

        if (arrLine == null || arrLine.Length == 0) arrLine = new string[100];

        if (arrLine.Length > line_to_edit)
        {
            arrLine[line_to_edit - 1] = arrLine[line_to_edit - 1] + ";" + newText;
        }
        else arrLine[line_to_edit - 1] = newText;
        File.WriteAllLines(fileName, arrLine);
    }
}

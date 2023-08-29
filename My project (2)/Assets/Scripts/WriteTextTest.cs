using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteTextTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Data/");

        CreateTextFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateTextFile()
    {
        string txtDocumentName = Application.streamingAssetsPath + "/Data/" + "MiniMaxSimple-MiniMaxAdvanced" + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "Title \n");
        }
    }



}

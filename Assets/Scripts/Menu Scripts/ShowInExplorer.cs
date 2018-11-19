using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ShowInExplorer : MonoBehaviour {

    public void ShowExplorer()
    {
        if(!Directory.Exists(Application.dataPath + "/../Maps/")){

        }
        string filePath = Application.dataPath + "/../Maps/";
        filePath = filePath.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/open,"+ filePath);
    }
}

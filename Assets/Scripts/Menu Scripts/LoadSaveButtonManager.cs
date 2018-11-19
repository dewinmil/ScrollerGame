using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSaveButtonManager : MonoBehaviour {

    string[] saves;
    public GameObject SaveLoadButtonPrefab;

	// Use this for initialization
	public void GetFileNames () {

        string filePath = Application.persistentDataPath + "/Maps/";

        if (Directory.Exists(filePath))
        {
            //do nothing
        }
        else
        {
            //create folder
            Directory.CreateDirectory(filePath);
        }

        DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + "/Maps/");
        FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
        saves = new string[Files.Length];
        int count = 0;
        foreach (FileInfo file in Files)
        {
            saves[count] = file.Name.Substring(0, file.Name.Length - 4);
            count++;
        }
    }

    public void ListSaves()
    {
        GetFileNames();
        ClearExistingButtons();
        CreateNewSaveLoadButtons(saves);
    }

    private void ClearExistingButtons()
    {
        var buttons = GetComponentsInChildren<SaveLoadButton>();
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
    }

    private void CreateNewSaveLoadButtons(string[] str)
    {
        foreach (var s in str)
        {
            GameObject button = Instantiate(SaveLoadButtonPrefab);
            button.GetComponent<SaveLoadButton>().Initialize(this.transform, s);
        }
    }
}

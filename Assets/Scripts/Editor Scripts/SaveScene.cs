using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveScene : MonoBehaviour
{

    GameObject[] terrain;
    GameObject[] spawnEnd;
    public TMP_InputField userInput;
    string serializedObject;
    public GameObject menuPanel;
    public GameObject savePanel;
    public GameObject confirmSavePanel;

    public void Start()
    {
        this.GetComponent<Button>().interactable = false;
    }

    public void Save()
    {
        string filePath = Application.persistentDataPath + "/Maps/" + userInput.text + ".txt";
        if (File.Exists(filePath))
        {
            //warn user
            savePanel.SetActive(false);
            confirmSavePanel.SetActive(true);
            return;
        }
        else
        {
            //create file
            using (FileStream fs = File.Create(filePath))
            {

            }
        }
        spawnEnd = GameObject.FindGameObjectsWithTag("SpawnEnd");
        terrain = GameObject.FindGameObjectsWithTag("Terrain");

        GameObject[] saveFile = new GameObject[spawnEnd.Length + terrain.Length];
        spawnEnd.CopyTo(saveFile, 0);
        terrain.CopyTo(saveFile, spawnEnd.Length);

        int count = 0;
        foreach (GameObject t in saveFile)
        {
            if (count < 2)
            {
                serializedObject = (t.name + Environment.NewLine + t.transform.position.x + Environment.NewLine + t.transform.position.y
                    + Environment.NewLine + t.transform.position.z + Environment.NewLine + t.transform.eulerAngles.x + Environment.NewLine
                    + t.transform.eulerAngles.y + Environment.NewLine + t.transform.eulerAngles.z + Environment.NewLine);
            }
            else
            {
                serializedObject = (t.name.Substring(0, t.name.Length - 7) + Environment.NewLine + t.transform.position.x + Environment.NewLine + t.transform.position.y
                + Environment.NewLine + t.transform.position.z + Environment.NewLine + t.transform.eulerAngles.x + Environment.NewLine
                + t.transform.eulerAngles.y + Environment.NewLine + t.transform.eulerAngles.z + Environment.NewLine);
            }
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.AppendAllText(filePath, serializedObject);
            count++;
        }
        /*
        foreach (GameObject t in terrain)
        {
            serializedObject = (t.name.Substring(0, t.name.Length - 7) + Environment.NewLine + t.transform.position.x + Environment.NewLine + t.transform.position.y
                + Environment.NewLine + t.transform.position.z + Environment.NewLine + t.transform.eulerAngles.x + Environment.NewLine
                + t.transform.eulerAngles.y + Environment.NewLine + t.transform.eulerAngles.z + Environment.NewLine);
            //File.SetAttributes(filePath, FileAttributes.Normal);
            File.AppendAllText(filePath, serializedObject);
        }
        */
        savePanel.SetActive(false);
        menuPanel.SetActive(true);
        return;
    }


    public void ForceSave()
    {
        string filePath = Application.persistentDataPath + "/Maps/" + userInput.text + ".txt";
        if (File.Exists(filePath))
        {
            //warn user
            using (FileStream fs = File.Create(filePath))
            {

            }
        }
        else
        {
            //File.CreateText(filePath);
            using (FileStream fs = File.Create(filePath))
            {

            }
        }
        terrain = GameObject.FindGameObjectsWithTag("Terrain");

        foreach (GameObject t in terrain)
        {
            serializedObject = (t.name.Substring(0, t.name.Length - 7) + Environment.NewLine + t.transform.position.x + Environment.NewLine + t.transform.position.y
                + Environment.NewLine + t.transform.position.z + Environment.NewLine + t.transform.eulerAngles.x + Environment.NewLine
                + t.transform.eulerAngles.y + Environment.NewLine + t.transform.eulerAngles.z + Environment.NewLine);
            print(serializedObject);
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.AppendAllText(filePath, serializedObject);
        }
    }

    public void Delete()
    {
        string filePath = Application.persistentDataPath + "/Maps/" + userInput.text + ".txt";
        if (File.Exists(filePath))
        {
            //create file
            File.Delete(filePath);
            return;
        }
    }

    public void UpdateInteractable()
    {
        if (userInput.text.Length > 0)
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

}

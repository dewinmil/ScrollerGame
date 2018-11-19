using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.Networking;

public class LoadScene : NetworkBehaviour
{

    public TMP_InputField userInput;
    public LoadSaveButtonManager manager;
    public GameObject parent;
    GameObject obj;
    GameObject temp;
    GameObject[] terrain;
    GameObject[] spawnEnd;
    Vector3 transformPos;
    Vector3 eulerAngles;

    public void Start()
    {
        transformPos = new Vector3(0, 0, 0);
        eulerAngles = new Vector3(0, 0, 0);
    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/Maps/" + userInput.text + ".txt";
        print(Application.persistentDataPath + "/Maps/" + userInput.text + ".txt");
        if (File.Exists(filePath))
        {
            terrain = GameObject.FindGameObjectsWithTag("Terrain");
            spawnEnd = GameObject.FindGameObjectsWithTag("SpawnEnd");

            foreach (GameObject t in terrain)
            {
                GameObject.Destroy(t);
            }

            foreach (GameObject t in spawnEnd)
            {
                GameObject.Destroy(t);
            }
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader sr = new StreamReader(filePath);
            try
            {
                while ((line = Regex.Replace(sr.ReadLine(), @"(\r\n|\n)", String.Empty)) != null)
                {
                    if (counter % 7 == 0)// prefab name
                    {
                        temp = Resources.Load<GameObject>(line);
                        obj = Instantiate(temp);
                    }
                    if (counter % 7 == 1)// transform x
                    {
                        transformPos.x = float.Parse(line);
                    }
                    if (counter % 7 == 2)// transform y
                    {
                        transformPos.y = float.Parse(line);
                    }
                    if (counter % 7 == 3)// transform z
                    {
                        transformPos.z = float.Parse(line);
                    }
                    if (counter % 7 == 4)// rotation x
                    {
                        eulerAngles.x = float.Parse(line);
                    }
                    if (counter % 7 == 5)// rotation x
                    {
                        eulerAngles.y = float.Parse(line);
                    }
                    if (counter % 7 == 6)// rotation z
                    {
                        eulerAngles.z = float.Parse(line);

                        obj.transform.position = transformPos;
                        obj.transform.eulerAngles = eulerAngles;
                        obj.transform.SetParent(parent.transform);
                        obj.transform.Translate(new Vector3(0, .0001f, 0));
                        obj.transform.Translate(new Vector3(0, -.0001f, 0));
                    }
                    counter++;
                }
            }
            catch
            {
                //do nothing last call on while loop throws error as regex cannot be null;
            }

            sr.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
        else
        {
            print("does not exist");
        }
    }

    public string[] LoadToFile(string networkInput)
    {
        string[] str;
        str = new string[1];
        string filePath = Application.persistentDataPath + "/Maps/" + networkInput + ".txt";
        if (File.Exists(filePath))
        {
            var lineCount = File.ReadAllLines(filePath).Length;
            str = new string[lineCount];
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader sr = new StreamReader(filePath);
            try
            {
                while ((line = Regex.Replace(sr.ReadLine(), @"(\r\n|\n)", String.Empty)) != null)
                {
                    str[counter] = line;
                    counter++;
                }
            }
            catch
            {
                //do nothing
            }
            sr.Close();
        }
        else
        {
            print("does not exist");
        }
        return str;
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
        TextMeshProUGUI[] list = manager.GetComponentsInChildren<TextMeshProUGUI>();
        bool interact = false;
        foreach (TextMeshProUGUI item in list)
        {
            if (item.text == userInput.text)
            {
                interact = true;
            }
        }
        this.GetComponent<Button>().interactable = interact;
    }


}

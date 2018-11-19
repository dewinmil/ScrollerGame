using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMenu : NetworkBehaviour {

    public static bool gameIsPaused;

    public GameObject gameMenu;
    public GameObject parent; //not 100% needed as this could be placed on parent
    public GameObject editorMenu;

    public void Start()
    {
        gameIsPaused = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleBool();
            if (gameIsPaused)
            {
                gameMenu.SetActive(true);
                if(editorMenu != null)
                {
                    editorMenu.SetActive(false);
                }
            }
            else
            {
                for(int i = 0; i < parent.transform.childCount; i++)
                {
                    parent.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (editorMenu != null)
                {
                    editorMenu.SetActive(true);
                }
            }
        }
    }

    public void toggleBool()
    {
        if (gameIsPaused)
        {
            gameIsPaused = false;
        }
        else
        {
            gameIsPaused = true;
        }
    }

    public void EndConnection()
    {
        FindObjectOfType<PlayerController>().EndConnection();
    }
}

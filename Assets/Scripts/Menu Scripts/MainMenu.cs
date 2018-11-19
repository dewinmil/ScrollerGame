using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour {

    CustomNetworkManager manager;
    GameObject[] players;
    GameObject[] terrain;

    private void Start()
    {
        manager = FindObjectOfType<CustomNetworkManager>();
    }

    public void PlayGame()
    {


        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        manager.menuObjects = GameObject.FindGameObjectsWithTag("Menu Object");


        foreach(GameObject obj in manager.menuObjects)
        {
            obj.SetActive(false);
        }
    }

    public void LoadEditor()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        manager.menuObjects = GameObject.FindGameObjectsWithTag("Menu Object");


        foreach (GameObject obj in manager.menuObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ReturnToMenu()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneAt(1));
        }

        SceneManager.UnloadSceneAsync(1);

        foreach (GameObject obj in manager.menuObjects)
        {
            obj.SetActive(true);
        }

        terrain = GameObject.FindGameObjectsWithTag("Terrain");

        foreach (GameObject t in terrain)
        {
            GameObject.Destroy(t);
        }

        terrain = GameObject.FindGameObjectsWithTag("SpawnEnd");

        foreach (GameObject t in terrain)
        {
            GameObject.Destroy(t);
        }
        NetworkMessage disconnect = new NetworkMessage();
        manager.SafeDisconnect(disconnect);
    }

    public void ReturnToMenuFromEditor()
    {
        SceneManager.UnloadSceneAsync(2);

        foreach (GameObject obj in manager.menuObjects)
        {
            obj.SetActive(true);
        }

        terrain = GameObject.FindGameObjectsWithTag("Terrain");

        foreach (GameObject t in terrain)
        {
            GameObject.Destroy(t);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

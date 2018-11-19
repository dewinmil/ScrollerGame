using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{

    public GameObject[] menuObjects;
    public GameObject[] players;
    public LoadScene sceneLoader;
    string gameName;
    string gamePassword;
    string map;
    string[] str;

    public TMP_InputField mapNameInput;
    public TMP_InputField mapSelectionInput;
    public GameObject passwordMenu;
    public GameObject matchMenu;
    public MainMenu mainMenu;
    public int currentPlayers;
    MatchInfoSnapshot requestedMatch;
    string userInputPassword;
    bool isPrivateMatch;
    public int dead, winners, round;

    public void Start()
    {
        round = 0;
        currentPlayers = 0;
        dead = 0;
        winners = 0;
        isPrivateMatch = false;
        userInputPassword = "";
    }


    public void StartHosting()
    {
        StartMatchMaker();
        matchMaker.CreateMatch(gameName, 4, true, gamePassword, "", "", 0, 0, OnMatchCreated);
    }

    private void OnMatchCreated(bool success, string extendedInfo, MatchInfo responseData)
    {
        base.StartHost(responseData);
    }

    public void RefreshMatches()
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }
        matchMaker.ListMatches(0, 10, "", false, 0, 0, HandleListMatchesComplete);
    }

    private void HandleListMatchesComplete(bool success, string extendedInfo,
        List<MatchInfoSnapshot> responseData)
    {
        //responseData.RemoveAt(0); // I get this odd "ghost" session being hosted for some reason, don't know why it exists
        //this is a poor work-around that may later induce errors.
        AvailableMatchesList.HandleNewMatchList(responseData);
    }

    public void JoinMatch(MatchInfoSnapshot match)
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }
        isPrivateMatch = false;
        matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, HandleJoinedMatch);
    }

    public void PasswordPrompt(MatchInfoSnapshot match)
    {
        matchMenu.SetActive(false);
        passwordMenu.SetActive(true);
        requestedMatch = match;
    }

    public void JoinPrivateMatch()
    {
        if (matchMaker == null)
        {
            StartMatchMaker();
        }
        isPrivateMatch = true;
        matchMaker.JoinMatch(requestedMatch.networkId, userInputPassword, "", "", 0, 0, HandleJoinedMatch);
    }

    private void HandleJoinedMatch(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (success == true)
        {
            if (isPrivateMatch)
            {
                passwordMenu.SetActive(false);
                matchMenu.SetActive(true);
            }
            mainMenu.PlayGame();
            StartClient(responseData);
        }
    }

    public void LoadToFile()
    {
        str = sceneLoader.LoadToFile(mapSelectionInput.text);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        conn.RegisterHandler(MsgType.Disconnect, SafeDisconnect);
        //NetworkServer.RegisterHandler(MsgType.Disconnect, ClientDisconnected);
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        players = GameObject.FindGameObjectsWithTag("Player");
        //PlayerController p = FindObjectOfType<PlayerController>();
        //p.RpcLoadFromServer(str);
        currentPlayers++;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().SetColor(conn.connectionId);
            player.GetComponent<PlayerController>().CallRpcSetColor();
        }
    }

    public void startGame()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().RpcLoadFromServer(str);
        }
    }

    public void SafeDisconnect(NetworkMessage msg)
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneAt(1));
        }

        SceneManager.UnloadSceneAsync(1);

        foreach (GameObject obj in menuObjects)
        {
            obj.SetActive(true);
        }
        StopClient();
        NetworkManager.Shutdown();
        NetworkServer.Reset();
        winners = 0;
        dead = 0;
        round = 0;
    }

    public void UpdateName(TMP_InputField name)
    {
        gameName = name.text;
    }

    public void UpdatePassword(TMP_InputField password)
    {
        if (password.text.Length.Equals(null))
        {
            gamePassword = "";
        }
        else
        {
            gamePassword = password.text;
        }
    }

    public void UpdateUserInputPassword(TMP_InputField password)
    {
        if (userInputPassword.Length.Equals(null))
        {
            userInputPassword = "";
        }
        else
        {
            userInputPassword = password.text;
        }
    }

    public void UpdateInteractableCreateButton(Button createButton)
    {
        if (gameName.Length > 0 && mapSelectionInput.text.Length > 0)
        {
            createButton.interactable = true;
        }
        else
        {
            createButton.interactable = false;
        }
    }

    public void UpdateInteractableEnterButton(Button enterButton)
    {
        if (userInputPassword.Length > 0)
        {
            enterButton.interactable = true;
        }
        else
        {
            enterButton.interactable = false;
        }
    }

    public void UpdateMapSelectionInput()
    {
        LoadToFile();
        mapSelectionInput.GetComponentInChildren<TextMeshProUGUI>().text = mapSelectionInput.text;
        try
        {
            mapNameInput.text = mapSelectionInput.text;
            mapNameInput.GetComponentInChildren<TextMeshProUGUI>().text = mapSelectionInput.text;
        }
        catch
        {
            //do nothing - if you don't interact with the mapNameInput before hand it errors out saying that the
            //object does not exist, but it does exist and functions as intended. I do not know why I get this error.
        }
    }

    public void HandleEmptyPassword()
    {
        if (gamePassword.Equals(null))
        {
            gamePassword = "";
        }
    }

    public void CheckReset()
    {
        bool reset = true;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerController>().health == 1)
            {
                reset = false;
            }
            if (player.GetComponent<PlayerController>().health == 2)
            {
                if (player.GetComponent<PlayerController>().scored == false)
                {
                    player.GetComponent<PlayerController>().score += 20 - (winners * 5);
                    player.GetComponent<PlayerController>().scored = true;
                    winners += 1;
                }
            }
            if (player.GetComponent<PlayerController>().health == 0)
            {
                if (player.GetComponent<PlayerController>().scored == false)
                {
                    if (dead == 0)
                    {
                        player.GetComponent<PlayerController>().gimp = true;
                    }
                    player.GetComponent<PlayerController>().scored = true;
                    dead += 1;
                }
            }
        }
        if (reset)
        {

            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().CallRpcResetPos();
            }

            round++;
            dead = 0;
            winners = 0;
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().CallRpcSetScores();
            }
            if (round >= 5)
            {
                NetworkMessage disconnect = new NetworkMessage();
                SafeDisconnect(disconnect);
            }
        }
    }
}

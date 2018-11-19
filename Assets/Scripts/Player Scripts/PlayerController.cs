using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    public int health;
    public float inputDelay;
    public float maxVelocity;
    public float jumpVelocity;
    public BoxCollider top;
    public BoxCollider bottom;
    public BoxCollider right;
    public BoxCollider left;
    public BoxCollider parentCollider;
    public GameObject[] terrain;
    GameObject[] spawnEnd;
    GameObject obj;
    GameObject temp;
    Vector3 transformPos;
    Vector3 eulerAngles;
    Vector3 forward = new Vector3(1, 0, 0);
    Vector3 upward = new Vector3(0, 1, 0);
    Vector3 downward = new Vector3(0, -1, 0);

    Vector3 cameraPos;
    Vector3 setPos;
    Rigidbody playerBody;
    public int topCollisions, bottomCollisions, leftCollisions, rightCollisions;
    public CustomNetworkManager manager;
    int dead, winners, numPlayers;
    public bool justTakenOut;
    public bool scored;
    public bool gimp;
    public Vector3 startPos;
    public int score;
    public TextMeshProUGUI text;
    GameObject scoreBoard;
    public int playerNum;
    public Color32 textColor;

    int currentCollisions;

    // Use this for initialization
    void Start()
    {
        score = 0;
        scored = false;
        gimp = false;
        justTakenOut = false;
        numPlayers = 0;
        Physics.IgnoreCollision(top, left, true);
        Physics.IgnoreCollision(top, right, true);
        Physics.IgnoreCollision(bottom, left, true);
        Physics.IgnoreCollision(bottom, right, true);
        Physics.IgnoreCollision(parentCollider, top, true);
        Physics.IgnoreCollision(parentCollider, bottom, true);
        Physics.IgnoreCollision(parentCollider, left, true);
        Physics.IgnoreCollision(parentCollider, right, true);

        manager = FindObjectOfType<CustomNetworkManager>();
        health = 1;
        maxVelocity = 5;
        jumpVelocity = 15f;
        cameraPos = Camera.main.transform.position;
        currentCollisions = 0;
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        dead = 0;
        winners = 0;
        transformPos = new Vector3(0, 0, 0);
        eulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            numPlayers = this.GetComponent<NetworkIdentity>().observers.Count;
        }
        else
        {
            if (hasAuthority)
            {
                CmdUpdateServer(health, scored, gimp);
            }
        }
        if (health == 1)
        {
            if (isLocalPlayer)
            {
                if (Input.GetButton("d"))
                {
                    playerBody.AddForce(forward * 5 * maxVelocity);

                    if (playerBody.velocity.x > maxVelocity)
                    {
                        playerBody.AddForce(forward * -(playerBody.velocity.x - maxVelocity));
                    }
                }

                if (Input.GetButton("a"))
                {
                    playerBody.AddForce(forward * 5 * -maxVelocity);

                    if (playerBody.velocity.x < -maxVelocity)
                    {
                        playerBody.AddForce(forward * -(playerBody.velocity.x + maxVelocity));
                    }
                }
                if (Input.GetKeyDown("w") || Input.GetKeyDown("space"))
                {

                    if (bottomCollisions > 0)
                    {
                        playerBody.velocity = new Vector3(playerBody.velocity.x, jumpVelocity, playerBody.velocity.z);
                        //playerBody.AddForce(upward * jumpVelocity);
                    }
                    else if (leftCollisions > 0)
                    {
                        playerBody.velocity = new Vector3(jumpVelocity * .7f, jumpVelocity, playerBody.velocity.z);
                        //playerBody.AddForce(upAndRight * jumpVelocity);
                    }
                    else if (rightCollisions > 0)
                    {
                        playerBody.velocity = new Vector3(jumpVelocity * -.7f, jumpVelocity, playerBody.velocity.z);
                        //playerBody.AddForce(upAndLeft * jumpVelocity);
                    }
                }

                if (playerBody.velocity.y > -15)
                {
                    playerBody.AddForce(downward * 10);
                    if (playerBody.velocity.y < -15)
                    {
                        playerBody.AddForce(upward * -(playerBody.velocity.y + 15));
                    }
                }

                setPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, cameraPos.z);
                Camera.main.transform.position = setPos;
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
            }
        }
        else
        {
            playerBody.velocity = new Vector3(0, 0, 0);
            if (justTakenOut)
            {
                justTakenOut = false;
                if (health != 1)
                {
                    if (isServer)
                    {
                        if (hasAuthority)
                        {
                            RpcCheckReset();
                        }
                    }
                    else
                    {
                        if (hasAuthority)
                        {
                            CmdCheckReset();
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if (Camera.main.transform.position.z < -21)
            {
                cameraPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, cameraPos.z + 20);
                Camera.main.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, cameraPos.z);
            }
        }
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            if (Camera.main.transform.position.z > -61)
            {
                cameraPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, cameraPos.z - 20);
                Camera.main.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, cameraPos.z);
            }
        }

    }

    [ClientRpc]
    public void RpcLoadFromServer(string[] saveFile)
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

        // Read the file and display it line by line.  
        foreach (string str in saveFile)
        {
            if (counter % 7 == 0)// transform x
            {
                temp = Resources.Load<GameObject>(str);
                obj = Instantiate(temp);
            }
            if (counter % 7 == 1)// transform x
            {
                transformPos.x = float.Parse(str);
            }
            if (counter % 7 == 2)// transform y
            {
                transformPos.y = float.Parse(str);
            }
            if (counter % 7 == 3)// transform z
            {
                transformPos.z = float.Parse(str);
            }
            if (counter % 7 == 4)// rotation x
            {
                eulerAngles.x = float.Parse(str);
            }
            if (counter % 7 == 5)// rotation x
            {
                eulerAngles.y = float.Parse(str);
            }
            if (counter % 7 == 6)// rotation z
            {
                eulerAngles.z = float.Parse(str);

                obj.transform.position = transformPos;
                obj.transform.eulerAngles = eulerAngles;
                obj.transform.Translate(new Vector3(0, .0001f, 0));
                obj.transform.Translate(new Vector3(0, -.0001f, 0));
                //obj.transform.SetParent(parent.transform);
            }
            counter++;
        }
        RpcStartPos();
        RpcResetPos();
        playerBody.velocity = new Vector3(0, 0, 0);
    }

    public bool CheckIsServer()
    {
        if (isServer)
        {
            return hasAuthority;
        }
        return false;
    }

    public void EndConnection()
    {
        if (isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    public void SetColor(int playerNum)
    {
        if (textColor.Equals(new Color32(0, 0, 0, 0)))
        {
            text.text = "Player " + (playerNum + 1);
            this.playerNum = playerNum;
        }
    }

    public void CallRpcSetColor()
    {
        if (isServer)
        {
            RpcSetColor(playerNum);
        }
    }

    [ClientRpc]
    public void RpcSetColor(int playerNum)
    {
        text.text = "Player " + (playerNum + 1);
        if (playerNum == 0)
        {
            scoreBoard = GameObject.Find("Player 1 Score");
            textColor = new Color32(0, 0, 255, 255);
            text.color = textColor;
        }
        else if (playerNum == 1)
        {
            scoreBoard = GameObject.Find("Player 2 Score");
            textColor = new Color32(255, 0, 0, 255);
            text.color = textColor;
        }
        else if (playerNum == 2)
        {
            scoreBoard = GameObject.Find("Player 3 Score");
            textColor = new Color32(0, 255, 0, 255);
            text.color = textColor;
        }
        else if (playerNum == 3)
        {
            scoreBoard = GameObject.Find("Player 4 Score");
            textColor = new Color32(255, 255, 0, 255);
            text.color = textColor;
        }
        this.playerNum = playerNum;
    }

    [Command]
    public void CmdCheckReset()
    {
        RpcCheckReset();
    }

    [ClientRpc]
    public void RpcCheckReset()
    {
        manager.currentPlayers = numPlayers; //update number of connected players
        manager.CheckReset();
    }

    [ClientRpc]
    public void RpcResetPos()
    {
        this.transform.position = startPos;
        health = 1;
        scored = false;
        gimp = false;
        if (hasAuthority)
        {
            CmdUpdateServer(health, scored, gimp);
        }
    }

    [ClientRpc]
    public void RpcSetScores(int score)
    {
        this.score = score;
        scoreBoard.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }

    [ClientRpc]
    public void RpcStartPos()
    {
        this.startPos = GameObject.Find("Spawn Position 1").transform.position;
    }

    [Command]
    void CmdUpdateServer(int health, bool score, bool gimp)
    {
        this.health = health;
        this.scored = score;
        this.gimp = gimp;
    }

    public void CallRpcResetPos()
    {
        if (isServer)
        {
            RpcResetPos();
        }
    }

    public void CallRpcSetScores()
    {
        if (isServer)
        {
            RpcSetScores(score);//the server has an accurate score
        }
    }

    public void CallStartGame()
    {
        if (isServer)
        {
            manager.startGame();
        }
    }

    public void CallRpcStartPos()
    {
        if (isServer)
        {
            RpcStartPos();
            RpcResetPos();
        }
    }
}

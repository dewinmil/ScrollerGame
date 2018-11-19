using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    Vector3 cameraPos;
    Vector3 worldBounds;
    public bool editorMode;
    bool typing;
    private void Start()
    {
        worldBounds = Camera.main.transform.position;
        worldBounds.x = Mathf.Clamp(worldBounds.x, -250, 250);
        worldBounds.y = Mathf.Clamp(worldBounds.y, -100, 100);
        typing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!typing)
        {
            cameraPos = Camera.main.transform.position;
            if (Input.GetButton("a"))
            {
                cameraPos.x = Mathf.Clamp(cameraPos.x - 1, -250, 250);
                Camera.main.transform.position = cameraPos;

            }
            if (Input.GetButton("d"))
            {
                cameraPos.x = Mathf.Clamp(cameraPos.x + 1, -250, 250);
                Camera.main.transform.position = cameraPos;
            }
            if (Input.GetButton("w"))
            {
                cameraPos.y = Mathf.Clamp(cameraPos.y + 1, -100, 100);
                Camera.main.transform.position = cameraPos;
            }
            if (Input.GetButton("s"))
            {
                cameraPos.y = Mathf.Clamp(cameraPos.y - 1, -100, 100);
                Camera.main.transform.position = cameraPos;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cameraPos.x = Mathf.Clamp(cameraPos.x - 1, -250, 250);
                Camera.main.transform.position = cameraPos;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                cameraPos.x = Mathf.Clamp(cameraPos.x + 1, -250, 250);
                Camera.main.transform.position = cameraPos;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                cameraPos.y = Mathf.Clamp(cameraPos.y + 1, -100, 100);
                Camera.main.transform.position = cameraPos;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                cameraPos.y = Mathf.Clamp(cameraPos.y - 1, -100, 100);
                Camera.main.transform.position = cameraPos;
            }
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if (Camera.main.transform.position.z < -21)
            {
                cameraPos = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z + 20);
                Camera.main.transform.position = new Vector3(cameraPos.x, cameraPos.y - 1, cameraPos.z);
            }
        }
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            if (Camera.main.transform.position.z > -201)
            {
                cameraPos = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z - 20);
                Camera.main.transform.position = new Vector3(cameraPos.x, cameraPos.y - 1, cameraPos.z);
            }
        }
    }

    public void TypingTrue()
    {
        typing = true;
    }

    public void TypingFalse()
    {
        typing = false;
    }
}

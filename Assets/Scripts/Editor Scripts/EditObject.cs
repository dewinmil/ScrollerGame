using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditObject : MonoBehaviour
{
    Material[] mats;
    Material[] childMats;
    public Material outline;
    public bool selected;
    public bool moving;
    public bool selectedColorApplied;
    Ray castPoint;
    RaycastHit hit;
    public bool editorMode;

    private void Start()
    {
        if(Camera.main.GetComponent<MoveCamera>().editorMode == true)
        {
            editorMode = true;
        }
        else
        {
            editorMode = false;
        }
        selectedColorApplied = false;
        try
        {
            mats = this.gameObject.GetComponent<MeshRenderer>().materials;
        }
        catch
        {
            //do nothing rendere in children
        }
        try
        {
            childMats = this.gameObject.GetComponentInChildren<MeshRenderer>().materials;
        }
        catch
        {
            //do nothing renderer was in parent
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (editorMode)
        {
            if (selected)
            {
                if (moving)
                {
                    castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                    {
                        this.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                    }
                }

                if (Input.GetButton("q"))
                {
                    this.transform.Rotate(new Vector3(-1, 0, 0));
                }
                if (Input.GetButton("e"))
                {
                    this.transform.Rotate(new Vector3(1, 0, 0));
                }
                if (selectedColorApplied == false)
                {
                    AddSelectedColor();
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {

                        if (selected)
                        {
                            for (int i = 0; i < childMats.Length; i++)
                            {
                                this.gameObject.GetComponentInChildren<MeshRenderer>().materials[i].shader = Shader.Find("Standard");
                            }
                            selected = false;
                            moving = false;
                        }
                        else
                        {
                            try
                            {
                                for (int i = 0; i < mats.Length; i++)
                                {
                                    this.gameObject.GetComponent<MeshRenderer>().materials[i].shader = Shader.Find("N3K/Outline");
                                    selectedColorApplied = true;
                                }
                            }
                            catch
                            {
                                //do nothing renderer in children
                            }

                            try
                            {
                                for (int i = 0; i < childMats.Length; i++)
                                {
                                    this.gameObject.GetComponentInChildren<MeshRenderer>().materials[i].shader = Shader.Find("N3K/Outline");
                                    selectedColorApplied = true;
                                }
                            }
                            catch
                            {
                                //do nothing rendere was in parent
                            }

                            selected = true;
                            if (moving)
                            {
                                moving = false;
                            }
                            else
                            {
                                moving = true;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < childMats.Length; i++)
                        {
                            this.gameObject.GetComponentInChildren<MeshRenderer>().materials[i].shader = Shader.Find("Standard");
                        }
                        selected = false;
                    }
                }
            }
        }
    }

    public void AddSelectedColor()
    {
        try
        {
            for (int i = 0; i < mats.Length; i++)
            {
                this.gameObject.GetComponent<MeshRenderer>().materials[i].shader = Shader.Find("N3K/Outline");
            }
        }
        catch
        {
            //do nothing renderer in children
        }

        try
        {
            for (int i = 0; i < childMats.Length; i++)
            {
                this.gameObject.GetComponentInChildren<MeshRenderer>().materials[i].shader = Shader.Find("N3K/Outline");
            }
        }
        catch
        {
            //do nothing renderer was in parent
        }
    }
}

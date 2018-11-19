using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour {

    Ray castPoint;
    RaycastHit hit;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Terrain")
                {
                    GameObject.Destroy(hit.collider.gameObject);
                }
            }
        }

    }
}

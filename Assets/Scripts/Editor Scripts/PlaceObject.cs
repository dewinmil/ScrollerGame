using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{

    public GameObject prefab;
    public GameObject parent;
    GameObject instance;
    Ray castPoint;
    RaycastHit hit;

    public void placeObject()
    {
        castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            instance = Instantiate(prefab, hit.point, Quaternion.identity);
            instance.transform.eulerAngles = new Vector3(0, 90, 0);
            instance.GetComponent<EditObject>().selected = true;
            instance.GetComponent<EditObject>().moving = true;
            instance.transform.SetParent(parent.transform);
        }
        else
        {
            instance = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
            instance.transform.eulerAngles = new Vector3(0, 90, 0);
            instance.GetComponent<EditObject>().selected = true;
            instance.GetComponent<EditObject>().moving = true;
            instance.transform.SetParent(parent.transform);
        }
    }
}

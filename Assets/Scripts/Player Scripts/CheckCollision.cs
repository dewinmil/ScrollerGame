using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {

    public void OnTriggerEnter(Collider c)
    {
        switch (this.gameObject.name)
        {
            case "top":
                GetComponentInParent<PlayerController>().topCollisions += 1;
                break;
            case "bottom":
                GetComponentInParent<PlayerController>().bottomCollisions += 1;
                break;
            case "right":
                GetComponentInParent<PlayerController>().rightCollisions += 1;
                break;
            case "left":
                GetComponentInParent<PlayerController>().leftCollisions += 1;
                break;
            default:
                //donothing
                break;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        switch (this.gameObject.name)
        {
            case "top":
                GetComponentInParent<PlayerController>().topCollisions -= 1;
                break;
            case "bottom":
                GetComponentInParent<PlayerController>().bottomCollisions -= 1;
                break;
            case "right":
                GetComponentInParent<PlayerController>().rightCollisions -= 1;
                break;
            case "left":
                GetComponentInParent<PlayerController>().leftCollisions -= 1;
                break;
            default:
                //donothing
                break;
        }
    }
}

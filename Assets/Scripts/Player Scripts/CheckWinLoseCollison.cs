using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWinLoseCollison : MonoBehaviour
{

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<KillPlayer>())
        {
            if (this.gameObject.GetComponent<PlayerController>().health == 1)
            {
                this.gameObject.GetComponent<PlayerController>().health = 0;
                this.gameObject.GetComponent<PlayerController>().justTakenOut = true;
            }
        }
        if (c.gameObject.GetComponent<Win>())
        {
            if (this.gameObject.GetComponent<PlayerController>().health == 1)
            {
                this.gameObject.GetComponent<PlayerController>().health = 2;
                this.gameObject.GetComponent<PlayerController>().justTakenOut = true;
            }
        }
        if (c.gameObject.GetComponent<StartGame>())
        {
            if (this.gameObject.GetComponent<PlayerController>().CheckIsServer())
            {
                this.gameObject.GetComponent<PlayerController>().CallStartGame();
            }
        }

        if (c.tag == "Player")
        {
            Physics.IgnoreCollision(c, this.gameObject.GetComponent<BoxCollider>());
        }
    }
}

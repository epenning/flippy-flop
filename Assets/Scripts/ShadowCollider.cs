using UnityEngine;
using System.Collections;

public class ShadowCollider : MonoBehaviour {

    public GameObject player;

    public PlayerController playerController;

    public int numColls;

	// Use this for initialization
	void Start () {
        playerController = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(numColls <= 0)
        {
            playerController.canFlip = true;
        } else
        {
            playerController.canFlip = false;
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Debug.Log("shadow triggered");
        //Debug.Log(coll.gameObject.tag);


        // disallow player flipping on shadow collision with obstacles, platforms
        if (coll.gameObject.tag == "platform" || coll.gameObject.tag == "obstacle" || coll.gameObject.tag == "trap" || coll.gameObject.tag == "enemy")
        {
            numColls++;
            //playerController.canFlip = false;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        //Debug.Log("shadow de-triggered");


        // allow player to flip again
        if (coll.gameObject.tag == "platform" || coll.gameObject.tag == "obstacle" || coll.gameObject.tag == "trap" || coll.gameObject.tag == "enemy")
        {
            numColls--;
            //playerController.canFlip = true;
        }
    }
}

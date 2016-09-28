using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointController : MonoBehaviour {

    public int numKeys;
    public int flipSide = 1;

    public int startSide;

    public bool activated;

    public Vector3 rotPoint;

    public List<GameObject> pickups;

    PlayerController playerScript;
    LevelController levelScript;

    public Sprite inactiveSpriteFront;
    public Sprite activeSpriteFront;

    public Sprite inactiveSpriteBack;
    public Sprite activeSpriteBack;

    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }

	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = inactiveSpriteBack;
    }
	
	// Update is called once per frame
	void Update () {
	    //if(activated)
     //   {
     //       GetComponent<SpriteRenderer>().sprite = activeSpriteFront;
     //       transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = activeSpriteBack;
     //   } else
     //   {
     //       GetComponent<SpriteRenderer>().sprite = inactiveSpriteFront;
     //       transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = inactiveSpriteBack;
     //   }
	}
    
    public void activatePickups()
    {
        foreach(GameObject pickup in pickups)
        {
            pickup.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(!activated && coll.gameObject.tag == "Player")
        {
            numKeys = playerScript.keys;
            activated = true;
            flipSide = levelScript.flipSide;
            playerScript.lastCheckpoint = gameObject;

            GetComponent<SpriteRenderer>().sprite = activeSpriteFront;
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = activeSpriteBack;
        }
    }
}

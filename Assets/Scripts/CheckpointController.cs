using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointController : MonoBehaviour {

    public int numKeys;
    public int flipSide = 1;

    public bool activated;

    public Vector3 rotPoint;

    public List<GameObject> pickups;

    PlayerController playerScript;
    LevelController levelScript;

    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
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
        }
    }
}

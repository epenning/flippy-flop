using UnityEngine;
using System.Collections;

public class BlackBarController : MonoBehaviour {

    public GameObject player;
    public GameObject levelController;

    public float distToPlayer;
    public float playerToPlatform;

    // Use this for initialization
    void Start () {
        distToPlayer = (player.transform.position - transform.position).magnitude;
        playerToPlatform = (player.transform.position - transform.position).magnitude;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

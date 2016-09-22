using UnityEngine;
using System.Collections;

public class BlackBoxController : MonoBehaviour {

    public GameObject player;
    public GameObject levelController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;


    }
}

using UnityEngine;
using System.Collections;

public class VolumeFadeOut : MonoBehaviour {

    public float baseXval;
    public float distToSilence;

    public float currDist;

    GameObject playerObj;

    public float maxVol = 1f;
    public float currVol;

	// Use this for initialization
	void Start () {
        playerObj = GameObject.Find("Player");
        baseXval = playerObj.transform.position.x;

	}
	
	// Update is called once per frame
	void Update () {
        currDist = Mathf.Abs(baseXval - playerObj.transform.position.x);

        currVol = ((distToSilence - currDist) / distToSilence) * maxVol;
	}
}

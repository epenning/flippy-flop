using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    public float thickness;

	// Use this for initialization
	void Start () {
        thickness = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

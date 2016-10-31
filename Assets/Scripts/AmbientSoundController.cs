using UnityEngine;
using System.Collections;

public class AmbientSoundController : MonoBehaviour {

    public bool persistThroughLevels;

	// Use this for initialization
	void Start () {
        if(persistThroughLevels)
            DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

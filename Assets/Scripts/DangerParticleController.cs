using UnityEngine;
using System.Collections;

public class DangerParticleController : MonoBehaviour {

    public bool active;

    ParticleSystem partSys;

    LevelController levelScript;

	// Use this for initialization
	void Awake () {
        levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        partSys = GetComponent<ParticleSystem>();
        partSys.GetComponent<Renderer>().sortingOrder = -70;
	}
	
	// Update is called once per frame
	void Update () {
        if(levelScript.currDir == 1)
        {
            active = true;
        } else
        {
            active = false;
        }


        if(partSys.isPlaying)
        {
            if(!active)
            {
                partSys.Stop();
            }
        } else
        {
            if(active)
            {
                partSys.Play();
            }
        }
    }
}

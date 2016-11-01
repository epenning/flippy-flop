using UnityEngine;
using System.Collections;

public class MasterVolumeController : MonoBehaviour {

    public float masterVol = 1f;
    public float musicVol = 1f;
    public float checkpointVol = 1f;
    public float flipVol = 1f;
    public float footstepVol = 1f;
    public float landingVol = 1f;
    public float flipDisabledVol = 1f;
    public float keyVol = 1f;
    public float doorVol = 1f;


    AudioSource flipSource;

    LevelController levelScript;
    TriggeredSFX trigSFXscript;
    FootstepSFX footSFXscript;

    // Use this for initialization
    void Start () {
        flipSource = GameObject.Find("Flip SFX").GetComponent<AudioSource>();

	    levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        trigSFXscript = GameObject.Find("Triggered SFX").GetComponent<TriggeredSFX>();
        footSFXscript = GameObject.Find("Footstep SFX").GetComponent<FootstepSFX>();

        levelScript.masterVol = masterVol;
        levelScript.musicVol = musicVol;
        levelScript.checkpointVol = checkpointVol;

        flipSource.volume = flipVol;

        trigSFXscript.landingVol = landingVol;
        trigSFXscript.flipDisabledVol = flipDisabledVol;
        trigSFXscript.keyVol = keyVol;
        trigSFXscript.doorVol = doorVol;

        footSFXscript.volume = footstepVol;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

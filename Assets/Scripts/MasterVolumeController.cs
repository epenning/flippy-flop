using UnityEngine;
using System.Collections;

public class MasterVolumeController : MonoBehaviour {

    public float currInnerVol;

    public float masterVol = 1f;
    public float musicVol = 1f;
    public float checkpointVol = 1f;
    public float flipVol = 1f;
    public float footstepVol = 1f;
    public float landingVol = 1f;
    public float flipDisabledVol = 1f;
    public float keyVol = 1f;
    public float doorVol = 1f;
    public float innerGameVol = 1f;


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
        levelScript.musicVol = innerGameVol * musicVol;
        levelScript.checkpointVol = innerGameVol * checkpointVol;

        flipSource.volume = innerGameVol * flipVol;

        trigSFXscript.landingVol = innerGameVol * landingVol;
        trigSFXscript.flipDisabledVol = innerGameVol * flipDisabledVol;
        trigSFXscript.keyVol = innerGameVol * keyVol;
        trigSFXscript.doorVol = innerGameVol * doorVol;

        footSFXscript.volume = innerGameVol * footstepVol;

        currInnerVol = innerGameVol;
    }

    public void fadeInnerVolTo(float tgtVol, float time)
    {
        Hashtable volArgs = new Hashtable();
        volArgs.Add("from", currInnerVol);
        volArgs.Add("to", tgtVol);
        volArgs.Add("time", time);
        volArgs.Add("onupdate", "updateInnerVol");
        volArgs.Add("onupdatetarget", gameObject);

        iTween.ValueTo(gameObject, volArgs);
    }

    public void updateInnerVol(float val)
    {
        currInnerVol = val;
    }

    // Update is called once per frame
    void Update () {
        levelScript.masterVol = masterVol;
        levelScript.musicVol = currInnerVol * musicVol;
        levelScript.checkpointVol = currInnerVol * checkpointVol;

        flipSource.volume = currInnerVol * flipVol;

        trigSFXscript.landingVol = currInnerVol * landingVol;
        trigSFXscript.flipDisabledVol = currInnerVol * flipDisabledVol;
        trigSFXscript.keyVol = currInnerVol * keyVol;
        trigSFXscript.doorVol = currInnerVol * doorVol;

        footSFXscript.volume = currInnerVol * footstepVol;
    }
}

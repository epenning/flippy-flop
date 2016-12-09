using UnityEngine;
using System.Collections;

public class Level4MusicVolFade : MonoBehaviour {

    VolumeFadeOut volAdjuster;
    MasterVolumeController masterVolScript;

    public float musicVolMax;
    public float footstepVolMax;

	// Use this for initialization
	void Start () {
        volAdjuster = GameObject.Find("Volume Adjuster").GetComponent<VolumeFadeOut>();
        masterVolScript = GameObject.Find("MasterVolumeController").GetComponent<MasterVolumeController>();

        musicVolMax = masterVolScript.musicVol;
        footstepVolMax = masterVolScript.footstepVol;
	}
	
	// Update is called once per frame
	void Update () {
        masterVolScript.musicVol = volAdjuster.currVol * musicVolMax;
        masterVolScript.footstepVol = volAdjuster.currVol * footstepVolMax;
    }
}

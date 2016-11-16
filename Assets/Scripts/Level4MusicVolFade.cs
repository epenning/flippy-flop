using UnityEngine;
using System.Collections;

public class Level4MusicVolFade : MonoBehaviour {

    VolumeFadeOut volAdjuster;
    MasterVolumeController masterVolScript;

	// Use this for initialization
	void Start () {
        volAdjuster = GameObject.Find("Volume Adjuster").GetComponent<VolumeFadeOut>();
        masterVolScript = GameObject.Find("MasterVolumeController").GetComponent<MasterVolumeController>();
	}
	
	// Update is called once per frame
	void Update () {
        masterVolScript.musicVol = volAdjuster.currVol;
        masterVolScript.footstepVol = volAdjuster.currVol;
    }
}

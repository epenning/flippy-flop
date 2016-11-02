using UnityEngine;
using System.Collections;

public class Level4MusicVolFade : MonoBehaviour {

    VolumeAttenuation volAdjuster;
    MasterVolumeController masterVolScript;

	// Use this for initialization
	void Start () {
        volAdjuster = GameObject.Find("Volume Adjuster").GetComponent<VolumeAttenuation>();
        masterVolScript = GameObject.Find("MasterVolumeController").GetComponent<MasterVolumeController>();
	}
	
	// Update is called once per frame
	void Update () {
        masterVolScript.musicVol = volAdjuster.currVol;
	}
}

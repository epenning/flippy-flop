using UnityEngine;
using System.Collections;

public class TriggeredSFX : MonoBehaviour {

    AudioSource source;

    public AudioClip key;
    public AudioClip door;
    public AudioClip[] dayland;
    public AudioClip[] nightland;
    public AudioClip hellland;
    public AudioClip flipdisabled;

    public float volume = 1f;
    public float landingVol = 1f;
    public float flipDisabledVol = 1f;
    public float keyVol = 1f;
    public float doorVol = 1f;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
	public void Play(string type)
    {
        if (type == "key")
        {
            source.PlayOneShot(key, keyVol);
        }
        if (type == "door")
        {
            source.PlayOneShot(door, doorVol);
        }
        if (type == "dayland")
        {
            source.PlayOneShot(dayland[Random.Range(0, dayland.Length - 1)], landingVol);
        }
        if (type == "nightland")
        {
            source.PlayOneShot(nightland[Random.Range(0, nightland.Length - 1)], landingVol);
        }
        if (type == "hellland")
        {
            source.PlayOneShot(hellland, landingVol);
        }
        if (type == "flipdisabled")
        {
            source.PlayOneShot(flipdisabled, flipDisabledVol);
        }
    }
}

using UnityEngine;
using System.Collections;

public class TriggeredSFX : MonoBehaviour {

    AudioSource source;

    public AudioClip key;
    public AudioClip door;
    public AudioClip grassland;
    public AudioClip hellland;
    public AudioClip flipdisabled;

    public float volume;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
	public void Play(string type)
    {
        if (type == "key")
        {
            source.PlayOneShot(key, volume * 1f);
        }
        if (type == "door")
        {
            source.PlayOneShot(door, volume * 1f);
        }
        if (type == "grassland")
        {
            source.PlayOneShot(grassland, volume * 1f);
        }
        if (type == "hellland")
        {
            source.PlayOneShot(hellland, volume * 1f);
        }
        if (type == "flipdisabled")
        {
            source.PlayOneShot(flipdisabled, volume * 0.5f);
        }
    }
}

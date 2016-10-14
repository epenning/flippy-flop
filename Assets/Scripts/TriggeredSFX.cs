using UnityEngine;
using System.Collections;

public class TriggeredSFX : MonoBehaviour {

    AudioSource source;

    public AudioClip key;
    public AudioClip door;
    public AudioClip grassland;
    public AudioClip hellland;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
	public void Play(string type)
    {
        if (type == "key")
        {
            source.PlayOneShot(key, 1f);
        }
        if (type == "door")
        {
            source.PlayOneShot(door, 1f);
        }
        if (type == "grassland")
        {
            source.PlayOneShot(grassland, 2f);
        }
        if (type == "hellland")
        {
            source.PlayOneShot(hellland, 2f);
        }
    }
}

using UnityEngine;
using System.Collections;

public class RepeatingSoundController : MonoBehaviour {
    
    AudioSource audSrc;

    public AudioClip[] soundList;
    public float masterListVol = 1f;
    public float[] volumeList;

    public float repeatInterval = 5f;

    public int currentIndex = 0;

    public float timeLeft;

    // Use this for initialization
    void Start () {
        audSrc = GetComponent<AudioSource>();
        timeLeft = repeatInterval;
        audSrc.volume = masterListVol;

        for(int i = 0; i < volumeList.Length; i++) 
        {
            volumeList[i] *= masterListVol;
        }

        playCurrentSound();
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            playCurrentSound();
        }
	}

    void playCurrentSound()
    {
        timeLeft = repeatInterval;
        audSrc.PlayOneShot(soundList[currentIndex], volumeList[currentIndex]);
        currentIndex++;
        if (currentIndex >= soundList.Length)
        {
            currentIndex = 0;
        }
    }
}

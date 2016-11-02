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

    public float startDelay;
    public bool hasStarted = false;

    // Use this for initialization
    void Start () {
        audSrc = GetComponent<AudioSource>();
        timeLeft = startDelay;
        audSrc.volume = masterListVol;

        for(int i = 0; i < volumeList.Length; i++) 
        {
            volumeList[i] *= masterListVol;
        }

        //playCurrentSound();
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            if(!hasStarted)
            {
                hasStarted = true;
            }
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

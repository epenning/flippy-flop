using UnityEngine;
using System.Collections;

public class FootstepSFX : MonoBehaviour {

    public AudioSource footstepSource;

    public AudioClip[] hellSteps;
    public AudioClip woodStep;
    
    public float timeBetween;
    public float timer;

    bool playing = false;
    int stepType = 0;
    
	public void Play(int type)
    {
        stepType = type;
        playing = true;
    }

    public void Stop()
    {
        playing = false;
        timer = 0f;
    }

    void Update()
    {
        // play step sounds if on
        if (playing)
        {
            // count timer up until play the sound
            if (timer >= timeBetween)
            {
                timer = 0f;

                AudioClip step;

                if (stepType == 3)
                {
                    step = hellSteps[Random.Range(0, hellSteps.Length - 1)];
                }
                else
                {
                    step = woodStep;
                }

                footstepSource.PlayOneShot(step, 0.5f);
            } else
            {
                timer += Time.deltaTime;
            }
        }
    }
}

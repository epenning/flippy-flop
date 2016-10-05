using UnityEngine;
using System.Collections;

public class FootstepSFX : MonoBehaviour {

    public AudioSource footstepSource;

    public AudioClip[] hellSteps;
    public AudioClip woodStep;
    public AudioClip grassStep;
    public AudioClip carpetStep;

    public GameObject playerController;
    PlayerController playerScript;
    
    public float timeBetween;
    public float timer = 0f;

    bool playing = false;
    int stepType = 0;   // 1 means day, 2 means night, 3 means hell

    void Start()
    {
        playerScript = playerController.GetComponent<PlayerController>();
    }
    
	public void Play()
    {
        // start steps playing
        playing = true;
    }

    public void Stop()
    {
        // stop steps playing
        playing = false;
        timer = 0f;
    }

    void Update()
    {
        // set step type by world
        if (playerScript.levelScript.levelNum == 1 || playerScript.levelScript.flipSide == 1)
        {
            // day steps
            stepType = 1;
        }
        else if (playerScript.levelScript.levelNum == 2)
        {
            // night steps
            stepType = 2;
        }
        else
        {
            // hell steps
            stepType = 3;
        }

        // play step sounds if on
        if (playing)
        {
            // count timer up until play the sound
            if (timer >= timeBetween)
            {
                timer = 0f;

                AudioClip step;
                float volume = 1f;

                if (stepType == 3)
                {
                    step = hellSteps[Random.Range(0, hellSteps.Length - 1)];
                    volume = 0.5f;
                }
                else
                {
                    step = grassStep;
                    volume = 1f;
                }

                footstepSource.PlayOneShot(step, volume);
            } else
            {
                timer += Time.deltaTime;
            }
        }
    }
}

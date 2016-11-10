using UnityEngine;
using System.Collections;

public class AudioSourceController : MonoBehaviour
{

    public float currVolume;
    public float defaultVolume = 1f;

    // 1 for daytime, -1 for other side, 0 for both
    public int playingSide = 1;

    public AudioSource audSrc;

    public bool prioritySound;
    public bool playing = false;
    public float reducedInnerVol = 0.5f;
    public float timeToReduce = 0.25f;

    MasterVolumeController mVolScript;

    public bool isTriggeredSound;
    public bool triggerLightning;
    public float lightningDuration;
    public float lightningDelay;
    public float lightningBrightness;
    public bool lightningEnded = false;
    Color originalLightColor;

    GameObject playerObj;
    GameObject mainLight;
    public bool hasPlayed = false;

    // Use this for initialization
    void Start()
    {
        mVolScript = GameObject.Find("MasterVolumeController").GetComponent<MasterVolumeController>();
        
        audSrc = GetComponent<AudioSource>();

        if(isTriggeredSound)
        {
            playerObj = GameObject.Find("Player");
            if (triggerLightning)
            {
                mainLight = GameObject.Find("Main Light");
                originalLightColor = mainLight.GetComponent<Light>().color;
            }
        }

        if (playingSide >= 0)
        {
            currVolume = defaultVolume;
        }
    }

    public void fadeVolTo(float tgtVol)
    {
        Hashtable volArgs = new Hashtable();
        volArgs.Add("from", currVolume);
        volArgs.Add("to", tgtVol);
        volArgs.Add("time", timeToReduce);
        volArgs.Add("onupdate", "updateVol");
        volArgs.Add("onupdatetarget", gameObject);

        iTween.ValueTo(gameObject, volArgs);
    }

    public void updateVol(float val)
    {
        currVolume = val;
    }

    // Update is called once per frame
    void Update()
    {

        // lightning code
        if (isTriggeredSound && triggerLightning && hasPlayed && !lightningEnded)
        {
            if (lightningDelay <= 0)
            {
                mainLight.GetComponent<Light>().color = new Color(0.7f,0.7f,1f,1f);

                if (lightningDuration > 0)
                {
                    lightningDuration -= Time.deltaTime;
                    if (mainLight.GetComponent<Light>().intensity > 1)
                        mainLight.GetComponent<Light>().intensity = 1;
                    else
                        mainLight.GetComponent<Light>().intensity = lightningBrightness;
                }

                if (lightningDuration <= 0)
                {
                    mainLight.GetComponent<Light>().intensity = 1;
                    lightningEnded = true;
                    mainLight.GetComponent<Light>().color = originalLightColor;
                }
            }
            else if (lightningDuration > 0)
            {
                lightningDelay -= Time.deltaTime;
            }
        }

        // trigger code
        if (isTriggeredSound && !hasPlayed)
        {
            if (playerObj.transform.position.x >= transform.position.x)
            {
                hasPlayed = true;
                audSrc.Play();
            }
        }


        if(audSrc.isPlaying)
        {
            if(!playing)
            {
                playing = true;
                if(prioritySound && currVolume > 0f)
                {
                    mVolScript.fadeInnerVolTo(reducedInnerVol, timeToReduce);
                }
            }
        } else
        {
            if (playing)
            {
                playing = false;
                if (prioritySound && currVolume > 0f)
                {
                    mVolScript.fadeInnerVolTo(mVolScript.innerGameVol, timeToReduce);
                }
            }
        }
        audSrc.volume = currVolume;
    }
}

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

    // Use this for initialization
    void Start()
    {
        mVolScript = GameObject.Find("MasterVolumeController").GetComponent<MasterVolumeController>();

        audSrc = GetComponent<AudioSource>();
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

        if(audSrc.isPlaying)
        {
            if(!playing)
            {
                playing = true;
                if(prioritySound)
                {
                    mVolScript.fadeInnerVolTo(reducedInnerVol, timeToReduce);
                }
            }
        } else
        {
            if (playing)
            {
                playing = false;
                if (prioritySound)
                {
                    mVolScript.fadeInnerVolTo(mVolScript.innerGameVol, timeToReduce);
                }
            }
        }
        audSrc.volume = currVolume;
    }
}

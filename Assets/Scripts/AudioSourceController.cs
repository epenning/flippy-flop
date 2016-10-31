using UnityEngine;
using System.Collections;

public class AudioSourceController : MonoBehaviour {

    public float currVolume;
    public float defaultVolume = 1f;

    // 1 for daytime, -1 for other side, 0 for both
    public int playingSide = 1;

	// Use this for initialization
	void Start () {
	    if(playingSide >= 0)
        {
            currVolume = defaultVolume;
        }
	}

    public void fadeVolTo(float tgtVol)
    {
        Hashtable volArgs = new Hashtable();
        volArgs.Add("from", currVolume);
        volArgs.Add("to", tgtVol);
        volArgs.Add("time", GameObject.Find("LevelController").GetComponent<LevelController>().flipDuration);
        volArgs.Add("onupdate", "updateVol");
        volArgs.Add("onupdatetarget", gameObject);

        iTween.ValueTo(gameObject, volArgs);
    }

    public void updateVol(float val)
    {
        currVolume = val;
    }

    // Update is called once per frame
    void Update () {
        gameObject.GetComponent<AudioSource>().volume = currVolume;
	}
}

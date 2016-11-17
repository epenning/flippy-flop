using UnityEngine;
using System.Collections;

public class VolumeAttenuation : MonoBehaviour
{

    public float currDist;
    public float maxDist = 10f;

    GameObject playerObj;

    public float minVol = 0f;
    public float maxVol = 1f;
    public float currVol;

    public bool persist;
    public bool maintainMaxVol;


    // Use this for initialization
    void Start()
    {
        if(persist)
            DontDestroyOnLoad(gameObject);

        playerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= playerObj.transform.position.x && maintainMaxVol)
        {
            currVol = maxVol;
        } else
        {
            currDist = Mathf.Abs(transform.position.x - playerObj.transform.position.x);

            currDist = Mathf.Min(currDist, maxDist);

            currVol = minVol + ((Mathf.Abs(maxDist - currDist) / maxDist) * (maxVol - minVol));
        }


        GetComponent<AudioSource>().volume = currVol;
    }
}

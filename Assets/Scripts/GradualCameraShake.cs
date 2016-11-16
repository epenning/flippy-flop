using UnityEngine;
using System.Collections;

public class GradualCameraShake : MonoBehaviour {

    public float currDist;
    public float maxDist = 10f;

    GameObject playerObj;

    public float minVal = 0f;
    public float maxVal = 0.2f;
    public float currVal;


    // Use this for initialization
    void Start()
    {
        playerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= playerObj.transform.position.x)
        {
            currVal = maxVal;
        }
        else
        {
            currDist = Mathf.Abs(transform.position.x - playerObj.transform.position.x);

            currDist = Mathf.Min(currDist, maxDist);

            currVal = minVal + ((Mathf.Abs(maxDist - currDist) / maxDist) * (maxVal - minVal));
        }


        GetComponent<CameraShaker>().shakeAmount = currVal;
    }
}

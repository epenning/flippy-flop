using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

    public bool startShaking;
    public float shakeAmount;

    GameObject secondCam;
 
    // Use this for initialization
    void Start () {
        secondCam = GameObject.Find("Secondary Camera");
	}
	
	// Update is called once per frame
	void Update () {
        if (startShaking == true)
        {
            Vector3 newCameraPos = new Vector3(Random.insideUnitSphere.x * shakeAmount, Random.insideUnitSphere.y * shakeAmount, secondCam.transform.localPosition.z);
            secondCam.transform.localPosition = newCameraPos;
        }
    }
}

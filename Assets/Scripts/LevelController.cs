using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public GameObject levelRoot;

    public Vector3 currRotPoint;
    public int currDir = -1;

    public bool rotating = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if (rotating)
        {
            if(currDir == 1)
            {
                //Debug.Log(levelRoot.transform.rotation.eulerAngles.x);
                if(levelRoot.transform.parent.transform.rotation.eulerAngles.x > 180f)
                {
                    Debug.Log("done rotating");
                    rotating = false;
                    levelRoot.transform.parent.transform.rotation = Quaternion.Euler(180, 0, 0);
                    
                } else
                {
                    rotateLevel();
                }
            } else
            {
                if (levelRoot.transform.parent.transform.localRotation.x > 0f)
                {
                    rotateLevel();
                }
                else
                {
                    rotating = false;
                    levelRoot.transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
	}

    void rotateLevel()
    {
        if(currDir == 1)
        {
            //levelRoot.transform.RotateAround(currRotPoint, Vector3.right, currDir * 10 * (0.8f * Mathf.Abs(180 - levelRoot.transform.rotation.eulerAngles.x)) * Time.deltaTime);
            levelRoot.transform.parent.transform.Rotate(new Vector3(currDir * 10 * (0.8f * Mathf.Abs(180 - levelRoot.transform.rotation.eulerAngles.x)) * Time.deltaTime, 0, 0));
        } else
        {
            //levelRoot.transform.RotateAround(currRotPoint, Vector3.right, currDir * 10 * (0.8f * Mathf.Abs(levelRoot.transform.rotation.eulerAngles.x - 180)) * Time.deltaTime);
            levelRoot.transform.parent.transform.Rotate(new Vector3(currDir * 10 * (0.8f * Mathf.Abs(levelRoot.transform.rotation.eulerAngles.x - 180)) * Time.deltaTime, 0, 0));
        }
        //levelRoot.transform.RotateAround(currRotPoint, Vector3.right, currDir * 500 * Time.deltaTime);
    }
}

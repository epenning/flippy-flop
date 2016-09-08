using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public GameObject levelRoot;

    public GameObject exit;

    public Vector3 currRotPoint;
    public int currDir = -1;

    public bool rotating = false;

    public float rotSpeed;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if (rotating)
        {
            if(currDir == 1)
            {
                if(levelRoot.transform.parent.transform.rotation.eulerAngles.x > 180f)
                {
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
        levelRoot.transform.parent.transform.Rotate(new Vector3(currDir * rotSpeed * Mathf.Abs(currDir * (180 - levelRoot.transform.rotation.eulerAngles.x)) * Time.deltaTime, 0, 0));
    }
}

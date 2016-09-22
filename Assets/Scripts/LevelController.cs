using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public GameObject levelRoot;

    public GameObject exit;

    public AudioSource dayMusic;
    public AudioSource nightMusic;

    public Vector3 currRotPoint;
    public int currDir = -1;

    public bool rotating = false;
    public bool halfRotated = false;

    public float rotSpeed;

    public int flipSide;

    public bool startRotate;

    public float flipDuration;

    public float volControl;

    // Use this for initialization
    void Start () {
        flipSide = 1;

        // control music to daytime
        dayMusic.volume = 1f;
        nightMusic.volume = 0f;

        // enable day sprites, disable night sprites
        foreach (Transform child in levelRoot.transform)
        {
            if (child.gameObject.tag == "obstacle" || child.gameObject.tag == "trap")
            {
                child.gameObject.GetComponent<Renderer>().enabled = true;
                child.GetChild(0).GetComponent<Renderer>().enabled = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (rotating)
        {
            if(currDir == 1)
            {

                if (!halfRotated && (levelRoot.transform.parent.localRotation.x > 0.75))
                {
                    halfRotated = true;
                    Debug.Log("swap sprites");
                    // enable night sprites, disable day sprites
                    foreach (Transform child in levelRoot.transform)
                    {
                        if (child.gameObject.tag == "obstacle" || child.gameObject.tag == "trap")
                        {
                            child.gameObject.GetComponent<Renderer>().enabled = false;
                            child.GetChild(0).GetComponent<Renderer>().enabled = true;
                        }
                    }
                }
            }
            else
            {

                if (!halfRotated && (levelRoot.transform.parent.localRotation.x < 0.75))
                {
                    halfRotated = true;
                    Debug.Log("swap sprites");
                    // enable day sprites, disable night sprites
                    foreach (Transform child in levelRoot.transform)
                    {
                        if (child.gameObject.tag == "obstacle" || child.gameObject.tag == "trap")
                        {
                            child.gameObject.GetComponent<Renderer>().enabled = true;
                            child.GetChild(0).GetComponent<Renderer>().enabled = false;
                        }
                    }
                }
            }
            

            if (!startRotate)
                rotateLevel();

        }
	}

    void rotateLevel()
    {
        startRotate = true;

        Hashtable rotArgs = new Hashtable();
        rotArgs.Add("amount", new Vector3(0.5f, 0, 0));
        rotArgs.Add("time", flipDuration);
        rotArgs.Add("easetype", iTween.EaseType.easeInOutCirc);
        rotArgs.Add("oncomplete", "finishRotating");
        rotArgs.Add("oncompletetarget", gameObject);

        iTween.RotateBy(levelRoot.transform.parent.gameObject, rotArgs);

        Hashtable volArgs = new Hashtable();
        volArgs.Add("from", volControl);
        volArgs.Add("to", 1f);
        volArgs.Add("time", flipDuration);
        volArgs.Add("onupdate", "updateMusicVols");
        volArgs.Add("onupdatetarget", gameObject);

        iTween.ValueTo(gameObject, volArgs);

    }

    public void updateMusicVols(float val)
    {
        if (currDir == -1)
        {
            dayMusic.volume = val;
            nightMusic.volume = 1 - val;
        }
        else
        {
            dayMusic.volume = 1 - val;
            nightMusic.volume = val;
        }
    }

    public void finishRotating()
    {
        rotating = false;
        startRotate = false;

        halfRotated = false;
        levelRoot.transform.parent.transform.rotation = Quaternion.Euler(180 * ((currDir + 1) / 2), 0, 0);

        volControl = 0f;

    }

    void loadLastCheckpoint()
    {

    }
}

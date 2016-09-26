using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelController : MonoBehaviour {

    public GameObject levelRoot;

    public GameObject exit;

    public AudioSource dayMusic;
    public AudioSource nightMusic;

    public int currDir = -1;

    public bool rotating = false;
    public bool halfRotated = false;

    public float rotSpeed;

    public int flipSide;

    public bool startRotate;

    public float flipDuration;

    public float volControl;

    public GameObject[] platformBlockList;
    public GameObject[] obstacleList;
    public GameObject[] trapList;
    public GameObject[] enemyList;

    public GameObject[] objList;

    // Use this for initialization
    void Start () {
        flipSide = 1;

        // control music to daytime
        dayMusic.volume = 1f;
        nightMusic.volume = 0f;

        platformBlockList = GameObject.FindGameObjectsWithTag("platform block");
        obstacleList = GameObject.FindGameObjectsWithTag("obstacle");
        trapList = GameObject.FindGameObjectsWithTag("trap");
        enemyList = GameObject.FindGameObjectsWithTag("enemy");

        objList = platformBlockList.Concat<GameObject>(obstacleList).ToArray<GameObject>().Concat<GameObject>(trapList).ToArray<GameObject>().Concat<GameObject>(enemyList).ToArray<GameObject>();

        foreach(GameObject obj in objList)
        {
            obj.GetComponent<Renderer>().enabled = true;
            obj.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
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
                    // enable night sprites, disable day sprites
                    foreach (GameObject obj in objList)
                    {
                        obj.GetComponent<Renderer>().enabled = false;
                        obj.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    }
                }
            }
            else
            {

                if (!halfRotated && (levelRoot.transform.parent.localRotation.x < 0.75))
                {
                    halfRotated = true;
                    // enable day sprites, disable night sprites
                    foreach (GameObject obj in objList)
                    {
                        obj.GetComponent<Renderer>().enabled = true;
                        obj.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
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

    // Use this when the player dies and the last checkpoint is on the other side of the world (prevents weird-looking repawn)
    public void instantFlip()
    {
        Hashtable rotArgs = new Hashtable();
        rotArgs.Add("amount", new Vector3(0.5f, 0, 0));
        rotArgs.Add("time", 0);
        rotArgs.Add("easetype", iTween.EaseType.easeInOutCirc);
        rotArgs.Add("oncomplete", "finishRotating");
        rotArgs.Add("oncompletetarget", gameObject);

        iTween.RotateBy(levelRoot.transform.parent.gameObject, rotArgs);

        Hashtable volArgs = new Hashtable();
        volArgs.Add("from", volControl);
        volArgs.Add("to", 1f);
        volArgs.Add("time", 0);
        volArgs.Add("onupdate", "updateMusicVols");
        volArgs.Add("onupdatetarget", gameObject);

        iTween.ValueTo(gameObject, volArgs);

        if (currDir == 1)
        {
            // enable night sprites, disable day sprites
            foreach (GameObject obj in objList)
            {
                obj.GetComponent<Renderer>().enabled = false;
                obj.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            // enable day sprites, disable night sprites
            foreach (GameObject obj in objList)
            {
                obj.GetComponent<Renderer>().enabled = true;
                obj.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            }

        }

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
        //instantFlip = false;
        halfRotated = false;
        levelRoot.transform.parent.transform.rotation = Quaternion.Euler(180 * ((currDir + 1) / 2), 0, 0);

        volControl = 0f;

    }

}

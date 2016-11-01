using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelController : MonoBehaviour {

    public GameObject levelRoot;

    public GameObject exit;

    AudioSource upMusic;
    AudioSource downMusic;

    public AudioClip dayIntroMusic;
    public AudioClip dayLoopMusic;
    public AudioClip nightLoopMusic;
    public AudioClip hellLoopMusic;

    // -1 for front side, 1 for back side
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
    public GameObject[] checkpointList;
    public GameObject[] doorList;
    public GameObject[] backgroundList;

    public GameObject[] objList;

    public GameObject[] audioList;

    public int levelNum;

    public float musicVol = 1f;
    public float checkpointVol = 1f;
    public float masterVol = 1f;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;


        flipSide = 1;

        // set upMusic and downMusic
        upMusic = GameObject.Find("Up Music").GetComponent<AudioSource>();
        downMusic = GameObject.Find("Down Music").GetComponent<AudioSource>();

        // control music to daytime
        upMusic.volume = musicVol;
        downMusic.volume = 0f;

        AudioListener.volume = masterVol;
        
        platformBlockList = GameObject.FindGameObjectsWithTag("platform");
        obstacleList = GameObject.FindGameObjectsWithTag("obstacle");
        trapList = GameObject.FindGameObjectsWithTag("trap");
        enemyList = GameObject.FindGameObjectsWithTag("enemy");
        checkpointList = GameObject.FindGameObjectsWithTag("checkpoint");
        doorList = GameObject.FindGameObjectsWithTag("exit");
        backgroundList = GameObject.FindGameObjectsWithTag("background");

        objList = platformBlockList.Concat<GameObject>(obstacleList).Concat<GameObject>(trapList).Concat<GameObject>(enemyList).Concat<GameObject>(checkpointList).Concat<GameObject>(doorList).Concat<GameObject>(backgroundList).ToArray<GameObject>();

        audioList = GameObject.FindGameObjectsWithTag("ambient sound");

        foreach (GameObject obj in objList)
        {
            obj.GetComponent<Renderer>().enabled = true;
            obj.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        }

        // set up and down audiosources to correct clips
        if (levelNum == 1)
        {
            StartCoroutine(playIntroMusic());
        }
        else if (levelNum == 2)
        {
            upMusic.clip = dayLoopMusic;
            downMusic.clip = nightLoopMusic;
            upMusic.Play();
            downMusic.Play();
        }
        else if (levelNum == 3)
        {
            upMusic.clip = dayLoopMusic;
            downMusic.clip = hellLoopMusic;
            upMusic.Play();
            downMusic.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (rotating)
        {
            if (currDir == 1)
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
                if (!halfRotated && (Mathf.Abs(levelRoot.transform.parent.localRotation.x) < 0.75))
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
        } else
        {
            if(currDir == 1)
            {
                upMusic.volume = 0f;
                downMusic.volume = musicVol;
            } else
            {
                upMusic.volume = musicVol;
                downMusic.volume = 0f;
            }
        }
	}

    IEnumerator playIntroMusic()
    {
        upMusic.clip = dayIntroMusic;
        downMusic.clip = dayIntroMusic;
        upMusic.Play();
        downMusic.Play();
        yield return new WaitForSeconds(dayIntroMusic.length);
        upMusic.clip = dayLoopMusic;
        downMusic.clip = dayLoopMusic;
        upMusic.Play();
        downMusic.Play();
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
        volArgs.Add("to", musicVol);
        volArgs.Add("time", flipDuration);
        volArgs.Add("onupdate", "updateMusicVols");
        volArgs.Add("onupdatetarget", gameObject);

        iTween.ValueTo(gameObject, volArgs);

        if (currDir == 1)
        {
            foreach (GameObject aud in audioList)
            {
                AudioSourceController volScript = aud.GetComponent<AudioSourceController>();

                if (volScript.playingSide == -1)
                {
                    // enable backside sounds
                    volScript.fadeVolTo(volScript.defaultVolume);
                }
                else if (volScript.playingSide == 1)
                {
                    // disable frontside sounds
                    volScript.fadeVolTo(0f);
                }
            }
        }
        else
        {
            foreach (GameObject aud in audioList)
            {
                AudioSourceController volScript = aud.GetComponent<AudioSourceController>();
                if (volScript.playingSide == -1)
                {
                    // disable backside sounds
                    volScript.fadeVolTo(0f);
                }
                else if (volScript.playingSide == 1)
                {
                    // enable frontside sounds
                    volScript.fadeVolTo(volScript.defaultVolume);
                }
            }

        }

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
        volArgs.Add("to", musicVol);
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

            foreach (GameObject aud in audioList)
            {
                AudioSourceController volScript = aud.GetComponent<AudioSourceController>();

                if (volScript.playingSide == -1)
                {
                    // enable backside sounds
                    volScript.fadeVolTo(volScript.defaultVolume);
                } else if(volScript.playingSide == 1)
                {
                    // disable frontside sounds
                    volScript.fadeVolTo(0f);
                }
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

            foreach (GameObject aud in audioList)
            {
                AudioSourceController volScript = aud.GetComponent<AudioSourceController>();
                if (volScript.playingSide == -1)
                {
                    // disable backside sounds
                    volScript.fadeVolTo(0f);
                }
                else if (volScript.playingSide == 1)
                {
                    // enable frontside sounds
                    volScript.fadeVolTo(volScript.defaultVolume);
                }
            }

        }

    }

    public void updateMusicVols(float val)
    {
        if (currDir == -1)
        {
            upMusic.volume = val;
            downMusic.volume = musicVol - val;
        }
        else
        {
            upMusic.volume = musicVol - val;
            downMusic.volume = val;
        }
    }

    public void finishRotating()
    {
        rotating = false;
        startRotate = false;
        //instantFlip = false;
        halfRotated = false;
        levelRoot.transform.parent.transform.rotation = Quaternion.Euler(180 * ((currDir + 1) / 2), 0, 0);
        GameObject.Find("Player").GetComponent<BoxCollider2D>().enabled = true;
        volControl = 0f;
    }
}

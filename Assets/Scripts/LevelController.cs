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

    // Use this for initialization
    void Start () {
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
                if (levelRoot.transform.parent.transform.rotation.eulerAngles.x > 180f)
                {
                    rotating = false;
                    halfRotated = false;
                    levelRoot.transform.parent.transform.rotation = Quaternion.Euler(180, 0, 0);

                    // control music to nighttime
                    dayMusic.volume = 0f;
                    nightMusic.volume = 1f;
                }
                else
                {
                    rotateLevel();

                    // fade dayMusic out, fade nightMusic in
                    if (dayMusic.volume > 0)
                        dayMusic.volume -= 0.05f;
                    if (nightMusic.volume < 1)
                        nightMusic.volume += 0.05f;

                    // activates once halfway through the rotation
                    if (!halfRotated && (levelRoot.transform.parent.transform.localRotation.x > 0.5))
                    {
                        halfRotated = true;

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
            }
            else
            {
                if (levelRoot.transform.parent.transform.localRotation.x > 0f)
                {
                    rotateLevel();

                    // fade nightMusic out, fade dayMusic in
                    if (nightMusic.volume > 0)
                        nightMusic.volume -= 0.05f;
                    if (dayMusic.volume < 1)
                        dayMusic.volume += 0.05f;

                    // activates once halfway through the rotation
                    if (!halfRotated && (levelRoot.transform.parent.transform.localRotation.x < 0.5))
                    {
                        halfRotated = true;

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
                else
                {
                    rotating = false;
                    halfRotated = false;
                    levelRoot.transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);

                    // control music to daytime
                    dayMusic.volume = 1f;
                    nightMusic.volume = 0f;
                }
            }
        }
	}

    void rotateLevel()
    {
        levelRoot.transform.parent.transform.Rotate(new Vector3(currDir * rotSpeed * Mathf.Abs(currDir * (180 - levelRoot.transform.rotation.eulerAngles.x)) * Time.deltaTime, 0, 0));
    }
}

using UnityEngine;
using System.Collections;

public class BlackBarController : MonoBehaviour {

    public GameObject player;
    public GameObject levelController;
    public LevelController levelScript;

    public float distToPlayerY;
    public float distToPlayerX;
    public float playerToPlatform;

    public bool moving;

    public int barType;


    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        levelController = GameObject.Find("LevelController");


        Vector3 worldCameraMin = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 worldCameraMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        Vector3 worldCameraCenter = Camera.main.ViewportToWorldPoint(Camera.main.rect.center);

        Debug.Log("min (rect): " + Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)));
        Debug.Log("min: " + worldCameraMin);
        Debug.Log("max: " + worldCameraMax);
        Debug.Log("center: " + worldCameraCenter);

        switch (barType)
        {
            // Left
            case 1:
                transform.position = new Vector3(worldCameraMin.x - (transform.localScale.x / 2), worldCameraCenter.y, 0);
                break;
            // Top
            case 2:
                transform.position = new Vector3(worldCameraCenter.x, worldCameraMax.y + (transform.localScale.y / 2), 0);
                break;
            // Right
            case 3:
                transform.position = new Vector3(worldCameraMax.x + (transform.localScale.x / 2), worldCameraCenter.y, 0);
                break;
            // Bottom
            case 4:
                transform.position = new Vector3(worldCameraCenter.x, worldCameraMax.y - (transform.localScale.y / 2), 0);
                break;
            default:
                break;

        }

        distToPlayerY = transform.position.y - player.transform.position.y;
        distToPlayerX = transform.position.x - player.transform.position.x;
        levelScript = levelController.GetComponent<LevelController>();
        transform.parent = levelScript.levelRoot.transform;
    }

    public void toggleMoving()
    {
        // Need a slight delay at the end of the iTween call to avoid translating the box twice in one rotation
        Invoke("turnOffMoving", 0.05f);
    }

    public void turnOffMoving()
    {
        moving = false;

    }

    // Update is called once per frame
    void Update () {

	    if(levelScript.rotating)
        {
            transform.parent = levelScript.levelRoot.transform;

            if (!moving)
            {
                // Get distance between the player anf the platform the world is rotating around
                playerToPlatform = player.transform.position.y - levelScript.levelRoot.transform.parent.position.y;
                moving = true;

                
                // Not sure why the value is 2.75, but it works
                Vector3 tgtLocalPos = new Vector3(transform.localPosition.x, transform.localPosition.y + levelScript.currDir * -2.75f, transform.localPosition.z);

                //Debug.Log(tgtLocalPos);

                Hashtable moveArgs = new Hashtable();
                moveArgs.Add("position", tgtLocalPos);
                moveArgs.Add("islocal", true);
                moveArgs.Add("time", levelScript.flipDuration);
                moveArgs.Add("easetype", iTween.EaseType.easeInQuad);
                moveArgs.Add("oncomplete", "toggleMoving");
                moveArgs.Add("oncompletetarget", gameObject);

                iTween.MoveTo(gameObject, moveArgs);
            }
        } else
        {
            // Convert the box's desired world position to the root's local coordinates
            Vector3 tgtLocalPos = new Vector3(player.transform.position.x + distToPlayerX, player.transform.position.y + (levelScript.currDir * -1 * distToPlayerY), 0);
            tgtLocalPos = levelScript.levelRoot.transform.InverseTransformPoint(tgtLocalPos);
            transform.localPosition = tgtLocalPos;
        }

	}
}

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

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        levelController = GameObject.Find("LevelController");

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

                
                // Not sure why the value is 3, but it works
                Vector3 tgtLocalPos = new Vector3(transform.localPosition.x, transform.localPosition.y + levelScript.currDir * -3, transform.localPosition.z);

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

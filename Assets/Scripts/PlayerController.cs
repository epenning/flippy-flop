using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rbody;

    public bool canMove = true;

    public GameObject levelController;

    LevelController levelScript;

    public GameObject oldPlatform;
    public GameObject currPlatform;

    public float height;
    
	// Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody2D>();
        levelScript = levelController.GetComponent<LevelController>();
        height = transform.localScale.y;
    }
	
	// Update is called once per frame
	void Update () {


        canMove = !levelScript.rotating;
        if(canMove)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                levelScript.rotating = true;
                levelScript.currDir *= -1;
                //levelScript.currRotPoint = new Vector3(transform.position.x, currPlatform.transform.position.y, 0);
                levelScript.currRotPoint = new Vector3(transform.position.x, transform.position.y - ((height / 2) + (currPlatform.GetComponent<PlatformController>().thickness / 2)), 0);
                Debug.Log(levelScript.currRotPoint);
            }

            rbody.isKinematic = false;
            if (Input.GetKey(KeyCode.D))
            {
                rbody.AddForce(new Vector2(10, 0), ForceMode2D.Force);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rbody.AddForce(new Vector2(-10, 0), ForceMode2D.Force);
            }
        } else
        {
            rbody.isKinematic = true;
        }

	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("collided");
        if(coll.gameObject.tag == "platform" && canMove)
        {
            oldPlatform = currPlatform;
            currPlatform = coll.gameObject.transform.parent.gameObject;
            currPlatform.transform.parent = null;
            levelScript.levelRoot.transform.parent = currPlatform.transform;
            if(oldPlatform)
                oldPlatform.transform.parent = levelScript.levelRoot.transform;
        }
    }
}

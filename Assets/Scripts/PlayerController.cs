using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rbody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public int numColliding;

    public GameObject levelController;

    LevelController levelScript;

    public GameObject oldPlatform;
    public GameObject currPlatform;

    public float height;
    public float minYBounds;

    public GameObject background;

    Bounds bgSpriteBounds;

    public bool canMove = true;
    public bool canFlip = false;

    public float maxSpeed;
    public float currSpeed;
    public int currDir = 0;
    public float horizAccel;

    public float grav;
    public float jumpSpeed;
    public bool midair = true;
    public float vertAccel;
    public bool hasJumped = false;

    public Vector2 velocity;

    public float collCorrection;

	public int keys;
    public GameObject keyIndicator;
    
    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelScript = levelController.GetComponent<LevelController>();
        //height = GetComponent<BoxCollider2D>().size.y;
        //minYBounds = GetComponent<BoxCollider2D>().offset.y - (height / 2);
        height = 1.9f;
        minYBounds = -0.035f - (height / 2);
        bgSpriteBounds = background.GetComponent<SpriteRenderer>().sprite.bounds;
    }
	
	// Update is called once per frame
	void Update () {

        canMove = !levelScript.rotating;

        
        float bgMinY = background.transform.position.y + bgSpriteBounds.min.y;
        float bgMinX = background.transform.position.x + bgSpriteBounds.min.x;
        float bgMaxX = background.transform.position.x + bgSpriteBounds.max.x;

        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < bgMinY || transform.position.x > bgMaxX || transform.position.x < bgMinX)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(canMove)
        {
            rbody.isKinematic = false;

            // jumping
            if (Input.GetKeyDown(KeyCode.Space) && !hasJumped && !midair)
            {
                velocity.y = jumpSpeed;
                midair = true;
                hasJumped = true;

                // trigger jump animation
                animator.SetTrigger("Jump");
            }

            // flipping
            if (canFlip && !midair)    
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    levelScript.rotating = true;
                    levelScript.currDir *= -1;
                    levelScript.currRotPoint = new Vector3(transform.position.x, transform.position.y + minYBounds -  ((currPlatform.GetComponent<PlatformController>().thickness / 2)), 0);
                    Debug.Log(levelScript.currRotPoint);
                }
            }
        } else
        {
            rbody.isKinematic = true;
        }

        // display key indicator
        if (keys > 0)
        {
            keyIndicator.SetActive(true);
        }
        else
        {
            keyIndicator.SetActive(false);
        }

        // animations
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        if (velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        animator.SetBool("Midair", midair);
	}

    // use for physics manipulations
    void FixedUpdate ()
    {
        // handle movement
        if (canMove)
        {
            if (Input.GetKey(KeyCode.D))
            {
                currDir = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currDir = -1;
                
            } else
            {
                currDir = 0;
            }

            if(currDir != 0)
            {
                currSpeed += horizAccel * ((maxSpeed * currDir) - currSpeed);

            } else
            {
                currSpeed += 1.2f * horizAccel * -currSpeed;
            }

            if (midair)
            {
                velocity.y += vertAccel * (-grav - velocity.y);
            } else
            {
                velocity.y = 0;
            }

            velocity.x = currSpeed;

            rbody.MovePosition(rbody.position + new Vector2(velocity.x, velocity.y) * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("collided");
        numColliding++;

        //Debug.Log(coll.gameObject.transform.position.y);
        //Debug.Log(transform.position.y + minYBounds + collCorrection - (coll.gameObject.transform.localScale.y / 2));

        // landed on a platform
        if (canMove && coll.gameObject.transform.position.y <= (transform.position.y + minYBounds + collCorrection - (coll.gameObject.transform.localScale.y / 2)))
        {
            midair = false;
            hasJumped = false;

            if (coll.gameObject.tag == "platform")
            {
                // set which platform the flip occurs around
                oldPlatform = currPlatform;
                currPlatform = coll.gameObject;
                currPlatform.transform.parent = null;
                levelScript.levelRoot.transform.parent = currPlatform.transform;
                if (oldPlatform)
                    oldPlatform.transform.parent = levelScript.levelRoot.transform;

                // allow flip on collision with platform - likely to break when colliding with side of platform
                //canFlip = true;
            }
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        // staying standing on a platform
        if (!hasJumped && canMove && coll.gameObject.transform.position.y <= (transform.position.y + minYBounds + collCorrection - (coll.gameObject.transform.localScale.y / 2)))
        {
            midair = false;
            // allow flip on collision with platform - likely to break when colliding with side of platform
            //canFlip = true;
        }
        else if(!levelScript.rotating)
        {
            // this line was making the "midair" variable true whenever touching two objects
            // what was this for?
            //midair = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        Debug.Log("de-collided");
        numColliding--;
        if (numColliding <= 0)
        {
            // disallow flipping and jumping midair
            //canFlip = false;
            if (!levelScript.rotating)
                midair = true;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("triggered");
        // handle end condition
        if (coll.gameObject.tag == "exit")
        {
            if (keys >= 1)
            {
                keys--;
                SceneManager.LoadScene("end");
            }
            else
            {
                Debug.Log("You need a key to open this door!");
            }
        }
		if (coll.gameObject.tag == "key") 
		{
			Debug.Log ("Picked up a key!");
			keys++;
			Destroy (coll.gameObject);
		}
    }
}

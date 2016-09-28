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
    public float distToFeet;

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

    public float collCorrection;

	public int keys;
    public GameObject keyIndicator;

    public float bgMinY;
    public float bgMinX;
    public float bgMaxX;

    public float bgYScale;
    public float bgXScale;

    public Sprite cooldownFlipIcon;
    public Sprite disabledFlipIcon;
    public Sprite readyFlipIcon;

    public float flipCooldown;
    public float timeToNextFlip;
    public bool flipOnCooldown;

    public GameObject flipIcon;

    public GameObject lastCheckpoint;
    public bool respawning;

    public GameObject checkpointPrefab;


    /* New movement + colllision system */
    public float jumpHeight = 4;

    // Time to reach the highest point of the jump
    public float timeToJumpApex = .4f;

    // Time to reach max horizontal speed (or come to rest)
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    // Calculated based on jumpHeight and timeToJumpApex
    public float jumpVelocity;
    public float gravity;

    public Vector3 velocity;

    // Used for horizontal drag
    public float velocityXSmoothing;

    MovementController controller;


    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelScript = levelController.GetComponent<LevelController>();



        controller = GetComponent<MovementController>();

        // Use kinematics equations to calculate necessary gravity and jumpVelocity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;




        // hardcoded values from when we had a BoxCollider2D. should be changed but I tried changing it and everything broke
        height = 1.9f;
        distToFeet = Mathf.Abs(-0.035f - (height / 2));

        bgSpriteBounds = background.GetComponent<SpriteRenderer>().sprite.bounds;
        bgYScale = background.transform.localScale.y;
        bgXScale = background.transform.localScale.x;

        //GameObject startingCheckpoint = GameObject.Instantiate(checkpointPrefab);
        //startingCheckpoint.transform.position = transform.position;
        //startingCheckpoint.transform.parent = levelScript.levelRoot.transform;
    }
	
	// Update is called once per frame
	void Update () {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if(controller.collisions.below)
        {
            midair = false;
        } else
        {
            midair = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));



        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;

                // trigger jump animation
                animator.SetTrigger("Jump");
            }

            // Target velocity at full speed
            float targetVelocityX = input.x * moveSpeed;

            // Smooth out horizontal velocity (changes depending on if we're grounded or airborne)
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            // flipping
            if (canFlip && !midair && !flipOnCooldown)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    timeToNextFlip = Time.time + flipCooldown;
                    flipOnCooldown = true;
                    levelScript.rotating = true;
                    levelScript.currDir *= -1;
                    levelScript.flipSide *= -1;
                    //Debug.Log(levelScript.currRotPoint);
                }
            }
        }
        else
        {
            // Freeze the character
            velocity = Vector3.zero;
        }


        canMove = !levelScript.rotating && !respawning;


        if(timeToNextFlip <= Time.time)
        {
            flipOnCooldown = false;

            if(canFlip && !midair)
            {
                //flipIcon.GetComponent<SpriteRenderer>().sprite = readyFlipIcon;
                flipIcon.GetComponent<SpriteRenderer>().color = Color.white;
            } else
            {
                //flipIcon.GetComponent<SpriteRenderer>().sprite = disabledFlipIcon;
                flipIcon.GetComponent<SpriteRenderer>().color = new Color(90f / 255, 30f / 255, 0, 104f / 255);
            }
        } else
        {
            //flipIcon.GetComponent<SpriteRenderer>().sprite = cooldownFlipIcon;
            flipIcon.GetComponent<SpriteRenderer>().color = new Color(90f / 255, 30f / 255, 0, 104f / 255);
        }


        // Get boundaries of background (taking scale into account)
        bgMinY = background.transform.position.y + bgSpriteBounds.min.y * bgYScale;
        bgMinX = background.transform.position.x + bgSpriteBounds.min.x * bgXScale;
        bgMaxX = background.transform.position.x + bgSpriteBounds.max.x * bgXScale;

        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < bgMinY || transform.position.x > bgMaxX || transform.position.x < bgMinX)
        {
            // Reached the border of the background or hit the reset button

            Die();
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

    public void updatePlatform(GameObject obj)
    {
        if (obj.tag == "platform")
        {
            // set which platform the flip occurs around
            oldPlatform = currPlatform;
            currPlatform = obj.gameObject;
            currPlatform.transform.parent = null;
            levelScript.levelRoot.transform.parent = currPlatform.transform;
            if (oldPlatform)
                oldPlatform.transform.parent = levelScript.levelRoot.transform;

        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Debug.Log("triggered");

        // don't activate triggers if hitting in the middle of flipping
        if (!levelScript.rotating)
        {
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
                    //Debug.Log("You need a key to open this door!");
                }
            }
            // handle key pickup
            if (coll.gameObject.tag == "key")
            {
                //Debug.Log("Picked up a key!");
                keys++;
                coll.gameObject.SetActive(false);
                lastCheckpoint.GetComponent<CheckpointController>().pickups.Add(coll.gameObject);
            }
            // handle trap collision (only if flipped upside-down)
            if ((coll.gameObject.tag == "trap" || coll.gameObject.tag == "enemy"))
            {
                Die();
            }
        }
    }

    void Die()
    {
        if (respawning || levelScript.rotating)
            return;

        respawning = true;
        velocity = Vector2.zero;
        CheckpointController checkpointScript = lastCheckpoint.GetComponent<CheckpointController>();
        if(checkpointScript.flipSide != levelScript.flipSide)
        {
            
            levelScript.currDir *= -1;
            levelScript.flipSide *= -1;
            GetComponent<SpriteRenderer>().enabled = false;
            levelScript.instantFlip();
            // Wait a bit to allow the world to flip (it's supposed to be instant, but the values take a bit to update)
            //Debug.Log(Time.deltaTime);
            Invoke("resetPosToCheckpoint", 0.05f);

        } else
        {

            resetPosToCheckpoint();
        }
        keys = checkpointScript.numKeys;
        checkpointScript.activatePickups();
    }

    void resetPosToCheckpoint()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        respawning = false;
        transform.position = lastCheckpoint.GetComponent<CheckpointController>().transform.position;
    }
}

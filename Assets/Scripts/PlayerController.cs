using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    // SFX
    public AudioSource flipSFX;
    public FootstepSFX footstepSFX;
    public TriggeredSFX triggeredSFX;

    // player components
    Animator animator;
    SpriteRenderer spriteRenderer;
    MovementController movementScript;

    // level variables
    public GameObject levelController;
    public LevelController levelScript;
    public GameObject background;
    Bounds bgSpriteBounds;
    public float bgMinY;
    public float bgMinX;
    public float bgMaxX;
    public float bgYScale;
    public float bgXScale;
    public bool gamePaused;

    // contacted platforms
    public GameObject oldPlatform;
    public GameObject currPlatform;

    // physical state
    public bool canMove = true;
    public bool canFlip = false;
    public bool midair = true;

    // keys
	public int keys;
    public GameObject keyIndicator;
    
    // flipping
    public float flipCooldown;
    public float timeToNextFlip;
    public bool flipOnCooldown;

    public GameObject flipIcon;
    FlipIconController flipIconScript;

    // checkpoints
    public GameObject lastCheckpoint;
    public bool respawning;
    
    /*
     * New movement + colllision system
     */
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


    GameObject shadowCollider;

    // Use this for initialization
    void Start () {
        // get components and scripts
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelScript = levelController.GetComponent<LevelController>();
        flipIconScript = flipIcon.GetComponent<FlipIconController>();
        movementScript = GetComponent<MovementController>();
        shadowCollider = GameObject.Find("Flip Shadow");

        // Use kinematics equations to calculate necessary gravity and jumpVelocity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        
        // calculate level bounds from background
        bgSpriteBounds = background.GetComponent<SpriteRenderer>().sprite.bounds;
        bgYScale = background.transform.localScale.y;
        bgXScale = background.transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        if(gamePaused)
        {
            return;
        }

        // foosteps SFX
        if ((velocity.x < -0.1f || velocity.x > 0.1f) && !midair)
        {
            footstepSFX.Play();
        } else
        {
            footstepSFX.Stop();
        }

        // hit something vertically
        if (movementScript.collisions.above || movementScript.collisions.below)
        {
            velocity.y = 0;
        }

        // landed on something
        if(movementScript.collisions.below)
        {
            midair = false;

        } else
        {
            midair = true;
        }

        // movement input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // not rotating or respawning
        if (canMove)
        {
            // jump
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Button A")) && !midair)
            {
                velocity.y = jumpVelocity;

                // trigger jump animation
                animator.SetTrigger("Jump");
            }

            // Target velocity at full speed
            float targetVelocityX = input.x * moveSpeed;

            // Smooth out horizontal velocity (changes depending on if we're grounded or airborne)
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (movementScript.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;

            movementScript.Move(velocity * Time.deltaTime);

            // flipping
            if (canFlip && !midair && !flipOnCooldown && !flipIconScript.hidden)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Button X"))
                {
                    // play flip SFX
                    flipSFX.Play();

                    GetComponent<BoxCollider2D>().enabled = false;
                    timeToNextFlip = Time.time + flipCooldown;
                    flipOnCooldown = true;

                    levelScript.rotating = true;
                    levelScript.currDir *= -1;
                    levelScript.flipSide *= -1;
                    //Debug.Log(levelScript.currRotPoint);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.S) && !flipIconScript.hidden)
                {
                    flipIconScript.flashDisabled();
                    triggeredSFX.Play("flipdisabled");
                }
            }
        }
        else
        {
            // Freeze the character
            velocity = Vector3.zero;
        }

        if (timeToNextFlip <= Time.time)
        {
            flipOnCooldown = false;
        }

        canMove = !levelScript.rotating && !respawning;

        // if flipping mechanic has been introduced
        if(!flipIconScript.hidden)
        {
            if (!flipOnCooldown)
            {
                // flip is off cooldown now
                flipOnCooldown = false;

                if (canFlip && !midair)
                {
                    flipIconScript.updateColor(Color.white);
                }
                else
                {
                    flipIconScript.updateColor(new Color(90f / 255, 30f / 255, 0, 104f / 255));
                }
            }
            else
            {
                flipIconScript.updateColor(new Color(90f / 255, 30f / 255, 0, 104f / 255));
            }
        }



        // Get boundaries of background (taking scale into account)
        bgMinY = background.transform.position.y + bgSpriteBounds.min.y * bgYScale;
        bgMinX = background.transform.position.x + bgSpriteBounds.min.x * bgXScale;
        bgMaxX = background.transform.position.x + bgSpriteBounds.max.x * bgXScale;

        // respawn from checkpoint
        if(Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Button B"))
        {
            if(lastCheckpoint)
                Die(false);
        }

        // respawn from falling off map
        if (transform.position.y < bgMinY)
        {
            // Reached the border of the background
            Die(true);
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

        // force landing animation to play
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_falling") && !midair)
        {
            animator.Play("player_land");
            // play landing sfx
            if (levelScript.levelNum == 3 && levelScript.flipSide == -1)
                triggeredSFX.Play("hellland");
            else if (levelScript.levelNum == 2 && levelScript.flipSide == -1)
                triggeredSFX.Play("nightland");
            else
                triggeredSFX.Play("dayland");
        }

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
            // handle last level game end condition
            if (coll.gameObject.tag == "end")
            {
                // go back to first main menu
                //SceneManager.LoadScene(0);
                //fadeBlackNextLevel();
                if (levelScript.levelNum == 4)
                {
                    fadeBlackNextLevel();

                    //SceneManager.LoadScene(0);
                }
            }
            // handle level end condition
            if (coll.gameObject.tag == "exit")
            {
                if (keys >= 1)
                {
                    // play door sound
                    triggeredSFX.Play("door");

                    // progress to next level
                    keys--;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
                // play key pickup sfx
                triggeredSFX.Play("key");

                // update state
                keys++;
                coll.gameObject.SetActive(false);
                if(lastCheckpoint)
                    lastCheckpoint.GetComponent<CheckpointController>().pickups.Add(coll.gameObject);
            }
            // handle trap collision (only if flipped upside-down)
            if ((coll.gameObject.tag == "trap" || coll.gameObject.tag == "enemy"))
            {
                Die(false);
            }
        }
    }

    void Die(bool outOfBounds)
    {
        if (respawning || levelScript.rotating)
            return;

        if (!lastCheckpoint)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        respawning = true;
        velocity = Vector2.zero;
        
        CheckpointController checkpointScript = lastCheckpoint.GetComponent<CheckpointController>();


        shadowCollider.GetComponent<BoxCollider2D>().enabled = false;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        //levelScript.flipDuration *= 0.5f;
        if (checkpointScript.flipSide != levelScript.flipSide)
        {
            levelScript.rotating = true;
            levelScript.currDir *= -1;
            levelScript.flipSide *= -1;



            //levelScript.instantFlip();

            // Wait a bit to allow the world to flip (it's supposed to be instant, but the values take a bit to update)
            //Debug.Log(Time.deltaTime);

            
            Invoke("resetPosToCheckpoint", levelScript.flipDuration);

        } else
        {

            resetPosToCheckpoint();
        }
        keys = checkpointScript.numKeys;
        checkpointScript.activatePickups();
    }

    void resetPosToCheckpoint()
    {
        Hashtable moveArgs = new Hashtable();
        moveArgs.Add("position", lastCheckpoint.GetComponent<CheckpointController>().transform.position);
        moveArgs.Add("time", 0.5f + 0.25f * (transform.position - lastCheckpoint.GetComponent<CheckpointController>().transform.position).magnitude / 15);
        moveArgs.Add("easetype", iTween.EaseType.easeInOutQuad);
        moveArgs.Add("oncomplete", "finishRespawning");
        moveArgs.Add("oncompletetarget", gameObject);

        iTween.MoveTo(gameObject, moveArgs);
    }

    void finishRespawning()
    {
        shadowCollider.GetComponent<ShadowCollider>().numColls = 0;
        shadowCollider.GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;        
        GetComponent<SpriteRenderer>().enabled = true;
        respawning = false; 
    }

    void fadeBlackNextLevel()
    {
        gamePaused = true;

        iTween.CameraFadeAdd();

        Hashtable fadeArgs = new Hashtable();
        fadeArgs.Add("amount", 1.0f);
        if (levelScript.levelNum == 4)
        {
            fadeArgs.Add("time", 5.0f);
            fadeArgs.Add("easetype", iTween.EaseType.linear);
        } else
        {
            fadeArgs.Add("time", 2.0f);
        }
        fadeArgs.Add("oncomplete", "finishFadeBlack");
        fadeArgs.Add("oncompletetarget", gameObject);

        iTween.CameraFadeTo(fadeArgs);
    }

    void finishFadeBlack()
    {
        if(levelScript.levelNum == 4)
        {
            SceneManager.LoadScene(0);
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

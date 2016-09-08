using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rbody;
    Animator animator;
    SpriteRenderer spriteRenderer;



    public GameObject levelController;

    LevelController levelScript;

    public GameObject oldPlatform;
    public GameObject currPlatform;

    public float height;

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

    public Vector2 velocity;

    



    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelScript = levelController.GetComponent<LevelController>();
        height = transform.localScale.y;
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

            if (canFlip)    // flipping and moving quickly on platforms
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = jumpSpeed;
                    midair = true;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    levelScript.rotating = true;
                    levelScript.currDir *= -1;
                    levelScript.currRotPoint = new Vector3(transform.position.x, transform.position.y - ((height / 2) + (currPlatform.GetComponent<PlatformController>().thickness / 2)), 0);
                    Debug.Log(levelScript.currRotPoint);
                }
            }

        } else
        {
            rbody.isKinematic = true;
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
	}

    // use for physics manipulations
    void FixedUpdate ()
    {
        if (canMove)
        {
            if (!canFlip)
            {
                // horizontal drag while falling
                //Vector2 v = rbody.velocity;
                //v.x = 0.95f * v.x;
                //rbody.velocity = v;
            }


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
                currSpeed += horizAccel * ((maxSpeed * currDir) - currSpeed);
            else
                currSpeed += 1.2f * horizAccel * ((maxSpeed * currDir) - currSpeed);

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
        
        if (coll.gameObject.tag == "platform" && canMove && coll.gameObject.transform.position.y < (transform.position.y - height))
        {
            midair = false;
            oldPlatform = currPlatform;
            currPlatform = coll.gameObject;
            currPlatform.transform.parent = null;
            levelScript.levelRoot.transform.parent = currPlatform.transform;
            if(oldPlatform)
                oldPlatform.transform.parent = levelScript.levelRoot.transform;

            // allow flip on collision with platform - likely to break when colliding with side of platform
            canFlip = true;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "platform" && canMove)
        {
            // allow flip on collision with platform - likely to break when colliding with side of platform
            canFlip = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        Debug.Log("de-collided");
        if(coll.gameObject.tag == "platform" && canMove)
        {
            // disallow 
            canFlip = false;
            midair = true;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("triggered");
        if (coll.gameObject.tag == "exit")
        {
            SceneManager.LoadScene("end");
        }
    }
}

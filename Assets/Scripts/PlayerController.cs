using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rbody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public bool canMove = true;
    public bool canFlip = false;

    public GameObject levelController;

    LevelController levelScript;

    public GameObject oldPlatform;
    public GameObject currPlatform;

    public float height;

    public GameObject background;

    Bounds bgSpriteBounds;

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

        //Debug.Log(GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite.bounds.max);

        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < bgMinY || transform.position.x > bgMaxX || transform.position.x < bgMinX)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(canMove)
        {
            rbody.isKinematic = false;

            if (canFlip)    // flipping and moving quickly on platforms
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    levelScript.rotating = true;
                    levelScript.currDir *= -1;
                    //levelScript.currRotPoint = new Vector3(transform.position.x, currPlatform.transform.position.y, 0);
                    levelScript.currRotPoint = new Vector3(transform.position.x, transform.position.y - ((height / 2) + (currPlatform.GetComponent<PlatformController>().thickness / 2)), 0);
                    Debug.Log(levelScript.currRotPoint);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    rbody.AddForce(new Vector2(10, 0), ForceMode2D.Force);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rbody.AddForce(new Vector2(-10, 0), ForceMode2D.Force);
                }
            }
            else            
            {
                // moving slowly while falling
                if (Input.GetKey(KeyCode.D))
                {
                    rbody.AddForce(new Vector2(10, 0), ForceMode2D.Force);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rbody.AddForce(new Vector2(-10, 0), ForceMode2D.Force);
                }
            }
        } else
        {
            rbody.isKinematic = true;
        }

        // animations
        Vector2 velocity = rbody.velocity;
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
                Vector2 v = rbody.velocity;
                v.x = 0.95f * v.x;
                rbody.velocity = v;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("collided");
        if(coll.gameObject.tag == "platform" && canMove)
        {
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

using UnityEngine;
using System.Collections;

public class EnemyPathController : MonoBehaviour {

    public float speed;
    public float direction;

    Rigidbody2D rbody;
    Animator animatorUp;
    Animator animatorDown;
    public SpriteRenderer spriteRendererUp;
    public SpriteRenderer spriteRendererDown;

    float startingY;

    bool justFlipped;

    int ignoreCollTimer;

    // Use this for initialization
    void Start () {
        rbody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        animatorUp = GetComponentInParent<Animator>();
        animatorDown = transform.parent.GetChild(0).GetComponent<Animator>();
        spriteRendererUp = GetComponentInParent<SpriteRenderer>();
        spriteRendererDown = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
        startingY = transform.parent.localPosition.y;
    }
	
	// Update is called once per frame
	void Update () {

        if(justFlipped)
        {
            ignoreCollTimer++;
        }

        if(ignoreCollTimer >= 30)
        {
            justFlipped = false;
            ignoreCollTimer = 0;
        }

        // move forward
        if (GameObject.Find("LevelController").GetComponent<LevelController>().rotating)
        {
            rbody.velocity = Vector3.zero;
        } else
        {
            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, startingY, 0);
            rbody.velocity = new Vector3(speed * direction, 0, 0);

        }

        // update animation
        float vx = rbody.velocity.x;
        if (animatorUp)
            animatorUp.SetFloat("Speed", Mathf.Abs(vx));
        animatorDown.SetFloat("Speed", Mathf.Abs(vx));
        //Debug.Log(vx);
        if (vx < 0)
        {
            spriteRendererUp.flipX = true;
            spriteRendererDown.flipX = true;
        }
        else if (vx > 0)
        {
            spriteRendererUp.flipX = false;
            spriteRendererDown.flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // turn around if about to hit an obstacle
        if (coll.gameObject.tag == "obstacle")
        {
            if(!GameObject.Find("LevelController").GetComponent<LevelController>().rotating && !justFlipped) {
                direction *= -1;
                justFlipped = true;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        // turn around if about to move off platform
        if (coll.gameObject.tag == "platform")
            //if(!GameObject.Find("LevelController").GetComponent<LevelController>().rotating) {
                direction *= -1;
            //}
    }
}

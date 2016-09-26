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

    // Use this for initialization
    void Start () {
        rbody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        animatorUp = GetComponentInParent<Animator>();
        animatorDown = transform.parent.GetChild(0).GetComponent<Animator>();
        spriteRendererUp = GetComponentInParent<SpriteRenderer>();
        spriteRendererDown = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        // move forward
        rbody.velocity = new Vector3(speed * direction, 0, 0);

        // update animation
        float vx = rbody.velocity.x;
        if (animatorUp)
            animatorUp.SetFloat("Speed", Mathf.Abs(vx));
        animatorDown.SetFloat("Speed", Mathf.Abs(vx));
        Debug.Log(vx);
        if (vx < 0)
        {
            spriteRendererUp.flipX = true;
            spriteRendererDown.flipX = true;
        }
        else
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
            direction *= -1;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        // turn around if about to move off platform
        if (coll.gameObject.tag == "platform")
            direction *= -1;
    }
}

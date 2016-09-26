using UnityEngine;
using System.Collections;

public class EnemyPathController : MonoBehaviour {

    public float speed;
    public float direction;

    Rigidbody2D rbody;
    Animator animatorUp;
    Animator animatorDown;
    SpriteRenderer spriteRendererUp;
    SpriteRenderer spriteRendererDown;

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
        if (vx < 0)
        {
            spriteRendererUp.flipX = true;
        }
        else
        {
            spriteRendererUp.flipX = false;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        // turn around if about to move off platform
        if (coll.gameObject.tag == "platform")
            direction *= -1;
    }
}

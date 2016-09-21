using UnityEngine;
using System.Collections;

public class EnemyPathController : MonoBehaviour {

    public float speed;
    public float direction;

    Rigidbody2D rbody;

	// Use this for initialization
	void Start () {
        rbody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        rbody.velocity = new Vector3(speed * direction, 0, 0);
	}

    void OnTriggerExit2D(Collider2D coll)
    {
        // turn around if about to move off platform
        if (coll.gameObject.tag == "platform")
            direction *= -1;
    }
}

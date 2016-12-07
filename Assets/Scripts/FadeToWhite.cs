using UnityEngine;
using System.Collections;

public class FadeToWhite : MonoBehaviour {

    public float baseXval;
    public float distToWhite;

    public float currDist;

    GameObject playerObj;

    public float currVal;

    SpriteRenderer sRend;

    // Use this for initialization
    void Start()
    {
        playerObj = GameObject.Find("Player");
        sRend = GetComponent<SpriteRenderer>();
        baseXval = playerObj.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        currDist = Mathf.Abs(baseXval - playerObj.transform.position.x);

        currVal = (currDist / distToWhite) * 255f;

        sRend.color = new Color(1, 1, 1, currVal / 255f);
    }
}

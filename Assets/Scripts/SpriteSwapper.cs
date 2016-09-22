using UnityEngine;
using System.Collections;

public class SpriteSwapper : MonoBehaviour {

    public Sprite frontSprite;
    public Sprite backSprite;

    public SpriteRenderer sRend;

    public LevelController levelScript;

    public bool flipped;

	// Use this for initialization
	void Start () {
        sRend = GetComponent<SpriteRenderer>();
        levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }
	
	// Update is called once per frame
	void Update () {
        if(levelScript.rotating)
        {
            if (Mathf.Abs(transform.rotation.eulerAngles.x - 90f) <= 5f)
            {
                if (!flipped)
                {
                    flipped = true;
                    if (sRend.sprite == backSprite)
                    {
                        sRend.sprite = frontSprite;
                        sRend.flipY = false;
                    }
                    else
                    {
                        sRend.sprite = backSprite;
                        sRend.flipY = true;
                    }
                }

            }

        } else
        {
            flipped = false;
        }

	}
}

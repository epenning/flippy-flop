using UnityEngine;
using System.Collections;

public class FlipIconController : MonoBehaviour {

    public bool canFlip;

    public bool hiddenOnStart;

    public Color tgtColor;

    public bool hidden;

    public SpriteRenderer spriteRend;

    public bool changingColor = false;

	// Use this for initialization
	void Start () {
        spriteRend = GetComponent<SpriteRenderer>();

        if(hiddenOnStart)
        {
            spriteRend.color = Color.clear;
            tgtColor = Color.clear;
            hidden = true;
        } else
        {
            spriteRend.color = Color.white;
            tgtColor = Color.white;
            hidden = false;
        }

        

    }

    public void updateColor(Color newCol)
    {
        if (tgtColor == newCol || spriteRend.color == newCol)
            return;

        if (hidden)
            hidden = false;

        tgtColor = newCol;


        Hashtable colArgs = new Hashtable();
        colArgs.Add("from", spriteRend.color);
        colArgs.Add("to", tgtColor);
        colArgs.Add("time", 0.1f);
        colArgs.Add("onupdate", "OnColorUpdated");
        colArgs.Add("easetype", iTween.EaseType.easeInQuad);
        iTween.ValueTo(gameObject, colArgs);

    }

    public void flashDisabled()
    {
        Hashtable colArgs = new Hashtable();
        colArgs.Add("from", spriteRend.color);
        colArgs.Add("to", new Color(1, 0, 0, 0.5f));
        colArgs.Add("time", 0.1f);
        colArgs.Add("easetype", iTween.EaseType.easeInQuad);
        colArgs.Add("onupdate", "OnColorUpdated");
        colArgs.Add("oncomplete", "fadeFromRed");
        colArgs.Add("oncompletetarget", gameObject);
        iTween.ValueTo(gameObject, colArgs);
    }

    public void fadeFromRed()
    {
        Hashtable colArgs = new Hashtable();
        colArgs.Add("from", spriteRend.color);
        colArgs.Add("to", tgtColor);
        colArgs.Add("time", 0.1f);
        colArgs.Add("easetype", iTween.EaseType.easeInQuad);
        colArgs.Add("onupdate", "OnColorUpdated");
        iTween.ValueTo(gameObject, colArgs);
    }

    public void OnColorUpdated(Color color)
    {
        spriteRend.color = color;
    }
}

using UnityEngine;
using System.Collections;

public class Trigger_FlipIconFadeIn : MonoBehaviour {

    public bool triggered = false;

    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (!triggered && coll.gameObject.tag == "Player")
        {
            triggered = true;
            EventManager.triggerEvent("flipIconFadeIn");
        }
    }

}

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Event_FlipIconFadeIn : MonoBehaviour {

    private UnityAction listener;

    void Awake()
    {
        listener = new UnityAction(triggerFlipIconEntrance);

    }

    void OnEnable()
    {
        EventManager.startListening("flipIconFadeIn", listener);
    }

    void OnDisable()
    {
        EventManager.stopListening("flipIconFadeIn", listener);
    }

    void triggerFlipIconEntrance() {
        GameObject.Find("Flip Icon").GetComponent<FlipIconController>().updateColor(Color.white);
        EventManager.stopListening("flipIconFadeIn", listener);
    }

}

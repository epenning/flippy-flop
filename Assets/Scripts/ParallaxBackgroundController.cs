using UnityEngine;
using System.Collections;

public class ParallaxBackgroundController : MonoBehaviour {

    GameObject player;

    LevelController levelScript;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();

        transform.position = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if(levelScript.rotating)
        {
            transform.parent = levelScript.levelRoot.transform;
            Vector3 tgtLocalPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            tgtLocalPos = levelScript.levelRoot.transform.InverseTransformPoint(tgtLocalPos);
            transform.localPosition = tgtLocalPos;
        } else
        {
            transform.parent = player.transform;
        }

    }
}

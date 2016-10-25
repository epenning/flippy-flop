using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour {
	
    void Start()
    {
        Cursor.visible = false;
    }

	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
            SceneManager.LoadScene(1);
    }
}

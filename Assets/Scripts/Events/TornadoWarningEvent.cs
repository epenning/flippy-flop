using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TornadoWarningEvent : MonoBehaviour {

    public bool hasStarted = false;
    public GameObject tornadoCanvas;

    GameObject playerObj;
    LevelController levelScript;
    GameObject mainCamera;
    MasterVolumeController masterVolScript;
    PlayerController playerScript;

	// Use this for initialization
	void Start () {
        playerObj = GameObject.Find("Player");
        levelScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        //tornadoCanvas = GameObject.Find("Tornado Warning Canvas");
        masterVolScript = GameObject.Find("MasterVolumeController").GetComponent<MasterVolumeController>();
        mainCamera = Camera.main.gameObject;

        playerScript = playerObj.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!hasStarted)
        {
            if (playerObj.transform.position.x >= transform.position.x)
            {
                hasStarted = true;
                startScreenTwitch();
            }
        }
    }

    void startScreenTwitch()
    {
        playerScript.gamePaused = true;
        masterVolScript.innerGameMuted = true;
        masterVolScript.updateInnerVol(0f);
        mainCamera.GetComponent<postVHSPro>().twitchVOn = true;
        Invoke("activateCanvas", 0.1f);
    }

    void activateCanvas()
    {
        tornadoCanvas.SetActive(true);
        Invoke("stopScreenTwitch", 0.1f);
    }

    void stopScreenTwitch()
    {
        mainCamera.GetComponent<postVHSPro>().twitchVOn = false;
        Invoke("reactivateTwitch", 30f);
    }

    void reactivateTwitch()
    {
        mainCamera.GetComponent<postVHSPro>().twitchVOn = true;
        Invoke("loadNextScene", 0.3f);
    }

    void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

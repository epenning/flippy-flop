using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    //Initiate the pasue menu as invisable
    private bool panelDisplayed = false;
    public GameObject pausePanel;

    //Define the amount of buttons displayed on screen
    public int buttonsCount = 3;

    //Define the button selector
    public int selector;

    //Define the number of button
    //public int ID;

    public PlayerController playerScript;

    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject exitButton;


    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update(){
        //Display or hide the pause menu based on keyboard input
        if (Input.GetKeyDown(KeyCode.Escape) && !panelDisplayed)
        {
            Time.timeScale = 0;
            ShowPanel();
            
        } else if (Input.GetKeyDown(KeyCode.Escape) && panelDisplayed){
            HidePanel();
            playerScript.gamePaused = false;
        }
         
    }

    //Display the pause menu
    private void ShowPanel(){
        panelDisplayed = true;
        pausePanel.SetActive(panelDisplayed);
        playerScript.gamePaused = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);

 /*       //Assign the vaule to selector based on keyboard input
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Restric the minimun value of the selector
            if (selector < buttonsCount)
            {
                selector += 1;
            }
            else
            {
                selector = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Restric the maxmun value of teh selector
            if (selector > buttonsCount)
            {
                selector -= 1;
            }
            else
            {
                selector = buttonsCount;
            }
        }

        //Change scenes based on the button selected
        if (Input.GetKeyDown(KeyCode.Return) && ID == selector)
        {
            switch (selector)
            {
                case 1:
                    ResumeGame();
                    break;

                case 2:
                    RestartGame();
                    break;

                case 3:
                    ExitGame();
                    break;
            }
        }*/
    }

    //Hide the pause menu
    private void HidePanel(){
        panelDisplayed = false;
        pausePanel.SetActive(panelDisplayed);
        Time.timeScale = 1;
        //playerScript.gamePaused = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    //Resume function
    public void ResumeGame(){
        panelDisplayed = false;
        pausePanel.SetActive(panelDisplayed);
        Time.timeScale = 1;
        playerScript.gamePaused = false;
    }

    //Restart function
    public void RestartGame(){
        Application.LoadLevel("Sprint 1 Level");
        Time.timeScale = 1;
    }

    //Exit function
    public void ExitGame(){
        Application.LoadLevel("UI Test");
        Time.timeScale = 1;
    }

}

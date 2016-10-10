using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    private bool panelDisplayed = false;
    public GameObject pausePanel;

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) && !panelDisplayed){
            ShowPanel();
        } else if (Input.GetKeyDown(KeyCode.Escape) && panelDisplayed){
            HidePanel();
        } 
    }

    private void ShowPanel(){
        panelDisplayed = true;
        pausePanel.SetActive(panelDisplayed);
        Time.timeScale = 0;
    }

    private void HidePanel(){
        panelDisplayed = false;
        pausePanel.SetActive(panelDisplayed);
        Time.timeScale = 1;
    }

    public void ResumeGame(){
        panelDisplayed = false;
        pausePanel.SetActive(panelDisplayed);
        Time.timeScale = 1;
    }

    public void RestartGame(){
        Application.LoadLevel("Sprint 1 Level");
        Time.timeScale = 1;
    }

    public void ExitGame(){
        Application.LoadLevel("UI Test");
        Time.timeScale = 1;
    }
}

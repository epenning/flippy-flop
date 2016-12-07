using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour {

    void Start()
    {
        Cursor.visible = true;
    }

    public void NextLevelButton(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void NextLevelButton(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetButtonDown("Button Start"))
        {
            SceneManager.LoadScene(1);
        }
    }
}

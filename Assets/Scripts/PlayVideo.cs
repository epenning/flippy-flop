using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayVideo : MonoBehaviour {

    public MovieTexture movie;
    
	void Start () {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        StartCoroutine(playIntroMusic());
	}

    IEnumerator playIntroMusic()
    {
        movie.Play();
        yield return new WaitForSeconds(movie.duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

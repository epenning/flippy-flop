using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoopingVideo : MonoBehaviour
{

    public MovieTexture movie;

    void Start()
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        movie.loop = true;
        movie.Play();
    }
}

//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class SoundController : MonoBehaviour
{
    //// Instance Variables
    private AudioSource fAudioSource;

    //// Events   
    void Start ()
    {
        // load components
        fAudioSource = GetComponent<AudioSource>();
    }

    void Update ()
    {
        // check if done
        if (fAudioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}

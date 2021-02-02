//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class ExplosionController : MonoBehaviour
{
    //// Instance Variables
    private ParticleSystem fParticleSystem;

    //// Events   
	void Start ()
    {
        // load components
        fParticleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update ()
    {
	    // check if done
        if (fParticleSystem.IsAlive() == false)
        {
            Destroy(gameObject);
        }
	}
}

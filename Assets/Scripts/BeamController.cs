//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class BeamController : MonoBehaviour
{
    //// Configurables
    public int fBlastDuration = 60;

    //// Instance Variables
    private bool fIsFiring = false;
    private int fCounter = 0;

	//// Events
	void Start ()
    {
        // initial state
        GetComponent<Renderer>().enabled = false;
        fCounter = fBlastDuration;
	}
	
	void FixedUpdate ()
    {
        if (fIsFiring)
        {
            fCounter--;
            // check counter
            if (fCounter == 0)
            {
                fIsFiring = false;
                GetComponent<Renderer>().enabled = false;
                fCounter = fBlastDuration;
            }
        }
	}

    //// Methods
    public void fireBeam()
    {
        fIsFiring = true;
        GetComponent<Renderer>().enabled = true;
    }
}

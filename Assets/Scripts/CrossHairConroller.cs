//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class CrossHairConroller : MonoBehaviour
{
    //// Events
	void Start ()
    {
	    // pass
	}
	
	void Update ()
    {
	    // animation
        transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y + 5f,0));
	}
}

//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class LightController : MonoBehaviour
{
    //// Confirgurables
    public Material fLightOn;
    public Material fLightOff;
    public float fActivationDistance = 1f;

    //// Instance Variables
    private Transform fPlayerTransform;
    private GameObject fSpotLight;
    private Renderer fRenderer;
    
    //// Events
	void Start()
    {
	    // load components
        fRenderer = GetComponentsInChildren<Renderer>()[1];
        fPlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        fSpotLight = transform.GetChild(0).gameObject;
	}
	
	void LateUpdate ()
    {
        if (Vector3.Magnitude(fPlayerTransform.position - transform.position) < fActivationDistance)
        {
            fSpotLight.SetActive(true);
            fRenderer.material = fLightOn;
        }
        else
        {
            fSpotLight.SetActive(false);
            fRenderer.material = fLightOff;
        }
	}
}

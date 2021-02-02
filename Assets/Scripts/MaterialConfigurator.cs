//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class MaterialConfigurator : MonoBehaviour
{
    //// Configurables
    public Material fTheme1;
    public Material fTheme2;
    public Material fTheme3;
    public Material fTargetTheme;

    //// Instance Variables
    private Renderer fRenderer;
    private int fAssignedColor = 0;

	//// Events
	void Start ()
    {
        // load components
        fRenderer = GetComponentInChildren<Renderer>();
	    // assign material
        fAssignedColor = Random.Range(0, 3);
        setNormalMaterial();
	}
	
	void Update ()
    {
	    // pass
	}

    //// Methods
    public void setTargetMaterial()
    {
        fRenderer.material = fTargetTheme;
    }

    public void setNormalMaterial()
    {
        switch (fAssignedColor)
        {
            case 0:
                fRenderer.material = fTheme1;
                break;
            case 1:
                fRenderer.material = fTheme2;
                break;
            case 2:
                fRenderer.material = fTheme3;
                break;
        }
    }
}

//// Imports
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//// Class
public class GameController : MonoBehaviour
{
	//// Configurables
	public Camera fCameraWeapon;
	public Camera fCameraChase;
	public Camera fCameraOrbital;

    //// Instance Variables
    private Text fGameText;

	//// Events
	void Start ()
	{
        // load components
        fGameText = GameObject.FindWithTag("GameText").GetComponent<Text>();
		// Null Check
		if ((fCameraChase == null) || (fCameraOrbital == null) || (fCameraWeapon == null))
		{
			print ("Warning: Please attach the cameras to the PlayMaker.");
		}
		else
		{
			// default camera
			fCameraChase.enabled = true;
			fCameraChase.GetComponent<CameraController>().setActive(true);
			fCameraOrbital.enabled = false;
			fCameraOrbital.GetComponent<CameraController>().setActive(false);
			fCameraWeapon.enabled = false;
			fCameraWeapon.GetComponent<CameraController>().setActive(false);
		}
	}

	void Update ()
	{
		// process input
		processKeys();
	}

	//// Methods
	public void processKeys()
	{
		// camera controls
		if (Input.GetKeyDown(KeyCode.F1) == true)
		{
            fGameText.text = "1st Person Mode";
			fCameraChase.enabled = false;
			fCameraChase.GetComponent<CameraController>().setActive(false);
			fCameraOrbital.enabled = false;
			fCameraOrbital.GetComponent<CameraController>().setActive(false);
			fCameraWeapon.enabled = true;
			fCameraWeapon.GetComponent<CameraController>().setActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.F2) == true)
		{
            fGameText.text = "3rd Person Mode";
			fCameraChase.enabled = true;
			fCameraChase.GetComponent<CameraController>().setActive(true);
			fCameraOrbital.enabled = false;
			fCameraOrbital.GetComponent<CameraController>().setActive(false);
			fCameraWeapon.enabled = false;
			fCameraWeapon.GetComponent<CameraController>().setActive(false);
		}
		else if (Input.GetKeyDown(KeyCode.F3) == true)
		{
            fGameText.text = "Orbital View Mode";
			fCameraChase.enabled = false;
			fCameraChase.GetComponent<CameraController>().setActive(false);
			fCameraOrbital.enabled = true;
			fCameraOrbital.GetComponent<CameraController>().setActive(true);
			fCameraWeapon.enabled = false;
			fCameraWeapon.GetComponent<CameraController>().setActive(false);
		}
		// orbit controls
        if (Input.GetKey(KeyCode.Minus) == true)
        {
            if (fCameraOrbital.GetComponent<CameraController>().isActive() == true)
            {
                fCameraOrbital.GetComponent<CameraController>().rotateAboutPlayer(false);
            }
        }
        else if (Input.GetKey(KeyCode.Equals) == true)
        {
            if (fCameraOrbital.GetComponent<CameraController>().isActive() == true)
            {
                fCameraOrbital.GetComponent<CameraController>().rotateAboutPlayer(true);
            }
        }
        // auto orbit controls
        if (Input.GetKeyDown(KeyCode.O) == true)
        {
            fCameraOrbital.GetComponent<CameraController>().fAutoOrbit = !(fCameraOrbital.GetComponent<CameraController>().fAutoOrbit);
        }
        // horizontal camera offset controls
        if (Input.GetKey(KeyCode.M) == true)
        {
            if (fCameraOrbital.GetComponent<CameraController>().isActive() == true)
            {
                fCameraOrbital.GetComponent<CameraController>().changeDistance(true);
            }
            else if (fCameraChase.GetComponent<CameraController>().isActive() == true)
            {
                fCameraChase.GetComponent<CameraController>().changeDistance(true);
            }
                
        }
        else if (Input.GetKey(KeyCode.N) == true)
        {
            if (fCameraOrbital.GetComponent<CameraController>().isActive() == true)
            {
                fCameraOrbital.GetComponent<CameraController>().changeDistance(false);
            }
            if (fCameraChase.GetComponent<CameraController>().isActive() == true)
            {
                fCameraChase.GetComponent<CameraController>().changeDistance(false);
            }
        }
        // vertical camera offset controls
        if (Input.GetKey(KeyCode.L) == true)
        {
            if (fCameraOrbital.GetComponent<CameraController>().isActive() == true)
            {
                fCameraOrbital.GetComponent<CameraController>().changeHeight(true);
            }
            else if (fCameraChase.GetComponent<CameraController>().isActive() == true)
            {
                fCameraChase.GetComponent<CameraController>().changeHeight(true);
            }

        }
        else if (Input.GetKey(KeyCode.K) == true)
        {
            if (fCameraOrbital.GetComponent<CameraController>().isActive() == true)
            {
                fCameraOrbital.GetComponent<CameraController>().changeHeight(false);
            }
            if (fCameraChase.GetComponent<CameraController>().isActive() == true)
            {
                fCameraChase.GetComponent<CameraController>().changeHeight(false);
            }
        }
	}
}

//// Configs
/* ----------
 * Orbital
 *  - H: 2
 *  - V: 1.5
 * ----------
 * Chase
 *  - H: 0.6
 *  - V: 0.25
 * ----------
 * Weapon
 * - H: 0.02
 * - V: 0.004
 */

//// Imports
using UnityEngine;
using System.Collections;

//// Class
public class CameraController : MonoBehaviour
{
	//// Configurables
	public bool fFollowPlayerRotation = true;
	public float fHorzontalOffset = 0.6f;
	public float fVerticalOffset = 0.25f;
	public float fMoveSpeed = 10;
	public float fMinRotationSpeed = 0.2f;																				// The speed at which the player rotates.
	public float fMaxRotationSpeed = 5f;																				// The speed at which the player rotates.
	public float fOrbitDelta = 1f;
    public float fSafeBandThickness = 0.4f;																				// Band where player rotation doesn't occur in screen dimension %.
	public bool fSideScroll = true;
	public bool fIsActive = false;
    public bool fLerpMotion = false;
    public bool fAutoOrbit = false;
    public float fOffSetDelta = 0.05f;

	//// Instance Variables
    private Vector3 fPlayerPosition;
	private Transform fPlayerTransform;
	private bool fHasBeenSetup = false;
    private float fOrbitAngle = 0f;

	//// Events
	void Start ()
	{
		// load components
		fPlayerTransform = GameObject.FindWithTag("Player").transform;
        // initialize
        fPlayerPosition = fPlayerTransform.position;
	}
		
	void FixedUpdate ()
	{
		if (fHasBeenSetup == false)
		{
			// initial alignment
			followPlayer ();
			// change flag
			fHasBeenSetup = true;
		}
		if (fIsActive == true)
		{
			// camera position
			if (fFollowPlayerRotation == true)
			{
				followPlayer();
			}
            else // case: orbital camera
            {
                // auto orbit
                if (fAutoOrbit == true)
                {
                    rotateAboutPlayer(false);
                }
                // update angle
                updateOrbitalPosition();
                // determine move
                Vector3 vMove = fPlayerTransform.position - fPlayerPosition;
                vMove.y = 0;
                // move camera
                transform.position += vMove;
                // update position tracker
                fPlayerPosition = fPlayerTransform.position;
            }
			// update player direction
			if (fSideScroll == true)
			{	
				applySideScroll();
			}
			else
			{
				applyLookAt();
			}
		}
	}

	//// Methods
	public void followPlayer()
	{
        // initialize
        Vector3 vTargetPos = new Vector3(0,0,0);
		// Determine Target camera position
        if (fFollowPlayerRotation == false)
        {
            // orientation correction
            Quaternion qOriginal = fPlayerTransform.rotation;
            fPlayerTransform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
            // determine target point
            vTargetPos = fPlayerTransform.position + fPlayerTransform.up * fVerticalOffset + (-1 * fPlayerTransform.forward * fHorzontalOffset);
            // undo orientation correction
            fPlayerTransform.rotation = qOriginal;
        }
        else
        {
            // determine target point
            vTargetPos = fPlayerTransform.position + fPlayerTransform.up * fVerticalOffset + (-1 * fPlayerTransform.forward * fHorzontalOffset);
        }
        // Move to this position
        if (fLerpMotion == true)
        {
            transform.position = Vector3.Lerp(transform.position, vTargetPos, Time.deltaTime * 3f);
        }
        else
        {
            transform.position = vTargetPos;
        }
		// rotate camera to face player
		transform.LookAt (fPlayerTransform);
	}

	public void applySideScroll()
	{
		// initialize
		Vector2 vMousePos = Input.mousePosition;
		// calculate primatives
		float dScreenMidpointW = Camera.main.pixelWidth / 2;
		float dSafeBandW = Camera.main.pixelWidth * (fSafeBandThickness / 2);
		// look right & left
		if (vMousePos.x > dScreenMidpointW + dSafeBandW)
		{
			float dSpecialPoint = dScreenMidpointW + dSafeBandW;
			fPlayerTransform.Rotate (0, (fMaxRotationSpeed - fMinRotationSpeed) * ((vMousePos.x - dSpecialPoint) / dSpecialPoint) + fMinRotationSpeed, 0);
		}
		else if (vMousePos.x < dScreenMidpointW - dSafeBandW)
		{
			float dSpecialPoint = dScreenMidpointW - dSafeBandW;
			fPlayerTransform.Rotate (0, -1 * ((fMaxRotationSpeed - fMinRotationSpeed) * ((dSpecialPoint - vMousePos.x) / dSpecialPoint) + fMinRotationSpeed), 0);
		}
	}

	public void applyLookAt()
	{
		// initialize
		float dPointDistance = 0.0f;
		// generate primatives
		Plane pTerrain = new Plane(Vector3.up, (new Vector3(0,0,0)));
		Ray rayCameraToMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
		// process ray intersection location
		if (pTerrain.Raycast (rayCameraToMouse, out dPointDistance)) // case: if plane not parallel to ray
		{
			// get 3D intersection point
			Vector3 vTargetPoint = rayCameraToMouse.GetPoint(dPointDistance);
			Vector3 vTargetDirection = vTargetPoint - fPlayerTransform.position;
			// look at target position
			fPlayerTransform.rotation = Quaternion.Euler(new Vector3(0,Quaternion.LookRotation(vTargetDirection).eulerAngles.y,0));
		}
	}

    private void updateOrbitalPosition()
    {
        if (fOrbitAngle > 0)
        {
            // determine rotation
            float dAngle = fOrbitAngle * 0.5f < fOrbitDelta ? fOrbitAngle : fOrbitAngle * 0.5f;
            // apply rotation
            transform.RotateAround(fPlayerTransform.position, fPlayerTransform.up, dAngle);
            // update orbit angle
            fOrbitAngle -= dAngle;
        }
        else if (fOrbitAngle < 0)
        {
            // determine rotation
            float dAngle = -1 * fOrbitAngle * 0.5f < fOrbitDelta ? fOrbitAngle : fOrbitAngle * 0.5f;
            // apply rotation
            transform.RotateAround(fPlayerTransform.position, fPlayerTransform.up, dAngle);
            // update orbit angle
            fOrbitAngle -= dAngle;
        }
    }

	public void setActive(bool isActive)
	{
		fIsActive = isActive;
	}

    public bool isActive()
    {
        return fIsActive;
    }

    //// Controls
    public void rotateAboutPlayer(bool fIsClockwise)
    {
        // apply rotation
        if (fIsClockwise == false)
        {
            fOrbitAngle += fOrbitDelta;
        }
        else
        {
            fOrbitAngle -= fOrbitDelta;
        }
    }

    public void changeHeight(bool isPositive)
    {
        // apply change
        if (isPositive == true)
        {
            fVerticalOffset += fOffSetDelta;
        }
        else
        {
            fVerticalOffset -= fOffSetDelta;
        }
        // clamping
        if (fVerticalOffset < 0)
        {
            fVerticalOffset = 0;
        }
        // apply
        followPlayer();
    }

    public void changeDistance(bool isPositive)
    {
        if (isPositive == true)
        {
            fHorzontalOffset += fOffSetDelta;
        }
        else
        {
            fHorzontalOffset -= fOffSetDelta;
        }
        // clamping
        if (fHorzontalOffset < 0.2f)
        {
            fHorzontalOffset = 0.2f;
        }
        // apply
        if (fFollowPlayerRotation == false)
        {
            followPlayer();
        }
    }
}

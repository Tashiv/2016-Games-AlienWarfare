//// Imports
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//// Class
public class PlayerController : MonoBehaviour
{
	//// Configurables
	public float fMoveSpeed = 1;																					// The players move speed.
	public float fGunMoveSpeed = 5;
	public Vector3 fGunInitialOffset = new Vector3(7.32f,270f,238.97f);
	public KeyCode fKeyForward = KeyCode.W;
	public KeyCode fKeyBackward = KeyCode.S;
	public KeyCode fKeyLeft = KeyCode.A;
	public KeyCode fKeyRight = KeyCode.D;
    public GameObject fExpolisionPrefab;
    public GameObject fGunFireSoundPrefab;
    public GameObject fExplosionSoundPrefab;
    public GameObject fBumpSoundPrefab;
    public GameObject fLeftGun;
    public GameObject fRightGun;
    public GameObject fLeftGunBeam;
    public GameObject fRightGunBeam;
    public GameObject fThoughtText;
    public GameObject fScoreText;

	//// Instance Variables
	private Transform fPlayerTransform;
    private Rigidbody fPlayerRigidBody;
	private Transform fRightGunTransform;
	private Transform fLeftGunTransform;
	private Transform fCrossHairTransform;
    private Renderer[] fCrossHairRenderer;
    private Animator fAnimator;
    private int fScore = 0;
    private string[] fFireCommands = {"Prepare to die Mr Shape.","What are we waiting for ?!?", "Waiting on you to pull the trigger ..."};
    private string[] fCantFireCommands = {"I cant shoot that ...", "Nope, cant do it.", "Maybe try aiming at something else ?"};
    private bool fCanShoot = true;

	//// Events
	void Awake ()
	{
        // load components
        fPlayerTransform = GameObject.FindWithTag("Player").transform;
        fPlayerRigidBody = (Rigidbody) GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        fRightGunTransform = fRightGun.transform;
        fLeftGunTransform = fLeftGun.transform;
        fCrossHairTransform = GameObject.FindWithTag("CrossHair").transform;
        fCrossHairRenderer = GameObject.FindWithTag("CrossHair").GetComponentsInChildren<Renderer>();
        fAnimator = GetComponentInChildren<Animator>();
	}
	
	void FixedUpdate ()
	{
        // Motion 
        updatePlayerMovement();
	}
        
	void LateUpdate()
	{
		// update gun motion
		if (getAimAngle() >= -105f) // case: right hand active
		{
            Vector3 vTargetPoint = updateActiveGunDirection(fRightGunTransform);
			updateInactiveGunDirection(fLeftGunTransform);
            // targeting
            checkTargetAndFire(true,vTargetPoint);
		}
		else // case: left hand active
		{
            Vector3 vTargetPoint = updateActiveGunDirection(fLeftGunTransform);
			updateInactiveGunDirection(fRightGunTransform);
            // targeting & firing
            checkTargetAndFire(false,vTargetPoint);
		}
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pillar")
        {
            fPlayerRigidBody.velocity = new Vector3(0, 0, 0);
            Instantiate(fBumpSoundPrefab, transform.position, transform.rotation);
        }
        else if (other.gameObject.tag == "Collidable")
        {
            Instantiate(fBumpSoundPrefab, transform.position, transform.rotation);
        }
    }


	//// Methods
	public float getAimAngle()
	{
		// initialize
		float dPointDistance = 0.0f;
		// generate primatives
		Plane pTerrain = new Plane(Vector3.up, (new Vector3(0,0,0)));
		Ray rayCameraToMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
		// process ray intersection location
		if (pTerrain.Raycast (rayCameraToMouse, out dPointDistance)) // case: if plane not parallel to ray
		{
			// determine aim vectors
			Vector3 vTargetPoint = rayCameraToMouse.GetPoint(dPointDistance);
			Vector3 vAimVector = vTargetPoint - fPlayerTransform.position;
			Vector3 vPlayerVector = fPlayerTransform.forward;
			// limit to x-z axes
			Vector3 vPlayerVectorProjected = Vector3.Cross(Vector3.up, vPlayerVector);
			// Get the angle [0,180]
			float dAngle = Vector3.Angle(vAimVector, vPlayerVectorProjected);
			// Determine sign
			float dSign = Mathf.Sign(Vector3.Dot(vAimVector, vPlayerVectorProjected));
			float dSignedAngle = dSign * dAngle;
			// done
			return dSignedAngle;
		}
		else // fail case
		{
			return 0;
		}
	}

	public void updateInactiveGunDirection(Transform inactiveArmTransform)
	{
		inactiveArmTransform.rotation = Quaternion.Lerp (inactiveArmTransform.rotation, Quaternion.Euler (new Vector3 (90, 0, 0)), fGunMoveSpeed * Time.deltaTime);
	}

    public Vector3 updateActiveGunDirection(Transform activeArmTransform)
	{
		// initialize
		float dPointDistance = 0.0f;
        Vector3 vTargetPoint = new Vector3(0,0,0);
		// generate primatives
		Plane pTerrain = new Plane(Vector3.up, (new Vector3(0,0,0)));
		Ray rayCameraToMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
		// process ray intersection location
		if (pTerrain.Raycast (rayCameraToMouse, out dPointDistance)) // case: if plane not parallel to ray
		{
			// get 3D intersection point
			vTargetPoint = rayCameraToMouse.GetPoint(dPointDistance);
            // lerp preperation
            Quaternion qOriginal = activeArmTransform.rotation;
            // look at target position
            activeArmTransform.LookAt(vTargetPoint);
            Quaternion qNew = activeArmTransform.rotation;
            // lerp
            activeArmTransform.transform.rotation = qOriginal;
            activeArmTransform.transform.rotation = Quaternion.Lerp(qOriginal, qNew, fGunMoveSpeed * Time.deltaTime * 4);
			// update crosshair
			fCrossHairTransform.position = vTargetPoint;
		}
        // done
        return vTargetPoint;
	}

	public void updatePlayerMovement()
	{
		// initialize
        Vector3 fPlayerMoveVelocity = new Vector3(0,0,0);
        // Forward & backward movement
        if (Input.GetKey(fKeyForward))
        {
            fPlayerMoveVelocity += fPlayerTransform.forward * fMoveSpeed;
        }
        else if (Input.GetKey(fKeyBackward))
        {
            fPlayerMoveVelocity += fPlayerTransform.forward * (-1 * fMoveSpeed);
        }
		//Side to side movement
		if (Input.GetKey (fKeyRight))
		{
			fPlayerMoveVelocity += fPlayerTransform.right * fMoveSpeed;
            fAnimator.SetInteger("Turning", 1);
		}
		else if (Input.GetKey (fKeyLeft))
		{
			fPlayerMoveVelocity += fPlayerTransform.right * (-1 * fMoveSpeed);
            fAnimator.SetInteger("Turning", -1);
		}
        else
        {
            fAnimator.SetInteger("Turning", 0);
        }
		// apply move
        fPlayerRigidBody.velocity = fPlayerMoveVelocity;
	}

    public void checkTargetAndFire(bool isActiveArmRight, Vector3 targetPoint)
    {
        // clear targetting
        GameObject[] Collidables = GameObject.FindGameObjectsWithTag("Collidable");
        GameObject Target = null;
        foreach (GameObject Collidable in Collidables)
        {
            Collidable.GetComponentInParent<MaterialConfigurator>().setNormalMaterial();
        }
        // target highlight
        RaycastHit[] rHits;
        if (isActiveArmRight == true)
        {
            // cast ray
            rHits = Physics.RaycastAll(fRightGunTransform.position, targetPoint - fRightGunTransform.position);
        }
        else
        {
            // cast ray
            rHits = Physics.RaycastAll(fLeftGunTransform.position, targetPoint - fLeftGunTransform.position);
        }
        // process hits
        if (rHits.Length == 0)
        {
            changeCrossHairVisibility(true);
        }
        else
        {
            bool bHasCollidable = false;

            foreach (RaycastHit rHit in rHits)
            {  
                if (rHit.collider.gameObject.tag != "Collidable")
                {
                    continue;
                }
                bHasCollidable = true;
                // process hit 
                rHit.collider.gameObject.GetComponentInParent<MaterialConfigurator>().setTargetMaterial();
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Target = rHit.collider.gameObject;
                }
                else
                {
                    if (fCanShoot != true)
                    {
                    fThoughtText.GetComponent<Text>().text = fFireCommands[Random.Range(0, fCantFireCommands.Length)];
                    fCanShoot = true;
                    }
                }
                // done
                break;
            }

            if (bHasCollidable == false)
            {
                changeCrossHairVisibility(true);
                if (fCanShoot != false)
                {
                    fThoughtText.GetComponent<Text>().text = fCantFireCommands[Random.Range(0, fCantFireCommands.Length)];
                    fCanShoot = false;
                }
            }
        }
        // fire at target
        if (Target != null)
        {
            // fire animation
            if ((isActiveArmRight == true))
            {
                fRightGunBeam.gameObject.GetComponent<BeamController>().fireBeam();
            }
            else
            {
                fLeftGunBeam.gameObject.GetComponent<BeamController>().fireBeam();
            }
            // gun sound
            Instantiate(fGunFireSoundPrefab,fPlayerTransform.position,fPlayerTransform.rotation);
            // recoil animation
            fAnimator.SetTrigger("Fire");
            // destroy target
            Target.SetActive(false);
            Instantiate(fExpolisionPrefab, Target.transform.position, Target.transform.rotation);
            Instantiate(fExplosionSoundPrefab, Target.transform.position, Target.transform.rotation);
            // score text
            fScore++;
            fScoreText.GetComponent<Text>().text = fScore.ToString();
            // free target memory
            Destroy(Target);
        }
    }

    public void changeCrossHairVisibility(bool isVisible)
    {
        foreach (Renderer theRenderer in fCrossHairRenderer)
        {
            theRenderer.enabled = isVisible;
        }
    }
}

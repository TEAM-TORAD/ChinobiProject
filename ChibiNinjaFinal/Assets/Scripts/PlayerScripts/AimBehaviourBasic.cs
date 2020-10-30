using UnityEngine;
using System.Collections;
using System.Linq;

// AimBehaviour inherits from GenericBehaviour. This class corresponds to aim and strafe behaviour.
public class AimBehaviourBasic : MonoBehaviour
{
	public Texture2D crosshair;                                           // Crosshair texture.
	public float aimTurnSmoothing = 0.15f;                                // Speed of turn response when aiming to match camera facing.
	public Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f,  0f);         // Offset to repoint the camera when aiming.
	public Vector3 aimCamOffset   = new Vector3(0f, 0.4f, -0.7f);         // Offset to relocate the camera when aiming.
	
	
	public float aimMovementSpeed;
	public bool turnAimOn;

	public Camera playerCamera;
	public ThirdPersonOrbitCamBasic cameraScript;
	public BasicBehaviour behaviourManager;
	

	private void Start()
	{
		turnAimOn = false;
		playerCamera.GetComponent<SmartCrosshair>().drawCrosshair = false;
	}

	private void Update()
	{
		if(turnAimOn)
		{
			AimOn();
		}
		else
		{
			AimOff();
		}
	}
	public void AimOn()
	{
		if (cameraScript && behaviourManager)
		{
			turnAimOn = true;
			cameraScript.SetTargetOffsets(aimPivotOffset, aimCamOffset);
			Vector3 forwardPos = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
			// Player is moving on ground, Y component of camera facing is not relevant.
			forwardPos.y = 0.0f;
			forwardPos = forwardPos.normalized;

			// Always rotates the player according to the camera horizontal rotation in aim mode.
			Quaternion targetRotation = Quaternion.Euler(0, behaviourManager.GetCamScript.GetH, 0);

			float minSpeed = Quaternion.Angle(transform.rotation, targetRotation) * aimTurnSmoothing;
			// Rotate entire player to face camera.

			behaviourManager.SetLastDirection(forwardPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minSpeed * Time.deltaTime);
			SmartCrosshair crosshair = playerCamera.GetComponent<SmartCrosshair>();
			crosshair.drawCrosshair = true;
			crosshair.spread.sSpread -= crosshair.spread.decreasePerSecond * Time.deltaTime;
			crosshair.spread.sSpread = Mathf.Clamp(crosshair.spread.sSpread, crosshair.spread.minSpread, crosshair.spread.maxSpread);

			//Cursor.lockState = CursorLockMode.Locked;
			//Cursor.visible = false;
		}
	}

	public void AimOff()
	{
		StartCoroutine(AimOffDelay());
	}

	IEnumerator AimOffDelay()
	{
		yield return new WaitForSeconds(1);
		turnAimOn = false;
		cameraScript.ResetTargetOffsets();
		playerCamera.GetComponent<SmartCrosshair>().drawCrosshair = false;
		//Cursor.lockState = CursorLockMode.None;
		//Cursor.visible = true;
	}
	
}

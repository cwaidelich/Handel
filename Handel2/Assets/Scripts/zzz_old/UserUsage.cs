/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserUsage : MonoBehaviour {


	public Transform target;
	public Transform cameraController;
	public TextControl ui;

	private Vector3[] positionCamera;
	private Vector3[] positionTarget;
	private Vector3[] rotationsCamera;
	private float[] fieldOfViews;
	private bool[] screenSet;

	private bool activeControls = true;


	void Start() {
		positionCamera = new Vector3[5];
		positionTarget = new Vector3[5];
		rotationsCamera = new Vector3[5];
		fieldOfViews = new float[5];
		screenSet = new bool[5];

		MouseController.guiOpen += deactivateUserControl;
		MouseController.closeAllGui += activateUserControl;
	}

	void deactivateUserControl() {
		activeControls = false;
	}

	void activateUserControl() {
		activeControls = true;
	}

	void Update() {

		cameraController.LookAt (target);

		if (activeControls) {

			//print (Camera.main.fieldOfView);
			//Camera.main.transform.LookAt (target);
			if (Input.GetAxis ("Mouse ScrollWheel") < 0 && Camera.main.fieldOfView < GeneralSettings.MaxFieldOfView) { // forward
				Camera.main.fieldOfView++;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") > 0 && Camera.main.fieldOfView > GeneralSettings.MinFieldOfView) { // back
				Camera.main.fieldOfView--;
			}




			if (Input.GetAxisRaw ("Vertical") > 0) {
//			Camera.main.transform.RotateAround (target.position, new Vector3(Camera.main.transform.localRotation.eulerAngles.y *GeneralSettings.CameraMovementSpeed,0,0),Time.deltaTime*Vector3.Distance (Camera.main.transform.position, target.position));
				target.localPosition -= Vector3.up*GeneralSettings.CameraRotationSpeed;
			}

			if (Input.GetAxisRaw ("Vertical") < 0) {
//			Camera.main.transform.RotateAround (target.position, new Vector3(-Camera.main.transform.localRotation.eulerAngles.y *GeneralSettings.CameraMovementSpeed,0,0),Time.deltaTime*Vector3.Distance (Camera.main.transform.position, target.position));
				//cameraController.RotateAround(target.position, Vector3.right, GeneralSettings.CameraMovementSpeed);
				target.localPosition += Vector3.up*GeneralSettings.CameraRotationSpeed;
			}

			if (Input.GetAxisRaw ("Horizontal") > 0) {
//			Camera.main.transform.RotateAround (target.position, new Vector3(0,-Camera.main.transform.localRotation.eulerAngles.x * GeneralSettings.CameraMovementSpeed,0),Time.deltaTime*Vector3.Distance (Camera.main.transform.position, target.position));
				cameraController.RotateAround(target.position, Vector3.down, GeneralSettings.CameraMovementSpeed);
			}

			if (Input.GetAxisRaw ("Horizontal") < 0) {
//			Camera.main.transform.RotateAround (target.position,  new Vector3(0,Camera.main.transform.localRotation.eulerAngles.x * GeneralSettings.CameraMovementSpeed,0),Time.deltaTime*Vector3.Distance (Camera.main.transform.position, target.position));
				cameraController.RotateAround(target.position, Vector3.up, GeneralSettings.CameraMovementSpeed);
			}


			SaveScreen (KeyCode.Alpha1, 0);
			SaveScreen (KeyCode.Alpha2, 1);
			SaveScreen (KeyCode.Alpha3, 2);
			SaveScreen (KeyCode.Alpha4, 3);
			SaveScreen (KeyCode.Alpha5, 4);

		}
	}

	void SaveScreen(KeyCode _k, int _i) {
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(_k)) {
			SetScreenposition (_i);
		}

		if (Input.GetKey(_k) && !Input.GetKey(KeyCode.LeftControl)) {
			GetScreenposition (_i);
		}

	}

	void SetScreenposition(int _i) {
		positionTarget[_i] = target.position;
		rotationsCamera[_i] = cameraController.rotation.eulerAngles;
		positionCamera[_i] = cameraController.position;
		fieldOfViews[_i] = Camera.main.fieldOfView;
		screenSet [_i] = true;
		ui.UpdateInformationText ("Screenposition "+(_i+1)+" saved", Color.green);
	}

	void GetScreenposition(int _i) {
		if (screenSet[_i] == false) {
			ui.UpdateInformationText ("Screenposition "+(_i+1)+" not set", Color.red);
			return;
		}
		target.position = positionTarget[_i];
		cameraController.rotation = Quaternion.Euler(rotationsCamera[_i]);
		cameraController.position = positionCamera[_i];
		Camera.main.fieldOfView = fieldOfViews[_i];

		ui.UpdateInformationText ("Screenposition "+(_i+1), Color.white);
	}

}
*/
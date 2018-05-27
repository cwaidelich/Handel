using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenericUIController : MonoBehaviour {


	public bool isActive;
	public string guiName;

	private List<GameObject> selectedGameObjects = new List<GameObject> ();

	//public static Action< 

	// Use this for initialization
	void Start () {
		MouseController.showGui += ActivateUI;
		MouseController.closeAllGui += CloseUI;

		transform.gameObject.SetActive (false);
		isActive = false;
	}



	void ActivateUI(Vector3 _position, GameObject _go, string _name) {
		Debug.Log ("ActivateUI called: " + _go.name);


		if (_name == guiName) {
			if (!selectedGameObjects.Find(g => g == _go))
				selectedGameObjects.Add (_go);
			isActive = true;
			transform.gameObject.SetActive (true);
			//print (initialGameObject);
			transform.position = Camera.main.WorldToScreenPoint (_position);

		}
	}

	public void CloseUI() {
		isActive = false;
		transform.gameObject.SetActive (false);		
		selectedGameObjects.Clear ();
	}




	public List<GameObject> UIcaller {
		get {
			return selectedGameObjects;
		}
	}



}

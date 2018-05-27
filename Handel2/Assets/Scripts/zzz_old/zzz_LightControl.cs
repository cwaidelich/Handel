using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {

	Grid theGrid;
	GameObject theLight;
	bool isNight;

	void Start() {
		theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
		//theLight = transform.FindChild ("Lights").gameObject;
	}

	void Update() {
		Node currentNode = theGrid.NodeFromWorldPoint (transform.position);
		if (currentNode.IsNight) {
			//theLight.SetActive (true);
			isNight = true;
		} else {
			//theLight.SetActive (false);
			isNight = false;
		}
	}

	public bool isNightForObject() {
		return isNight;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInformationParent : MonoBehaviour {

	public GameObject RessourceParent;
	public GameObject CoordinatesParent;
	public GameObject WeightPenalty;
	public GameObject TemperaturInfo;

	private int showStatus = 0;
	// Use this for initialization
	void Start () {
		Setting_Button_Controller.showRessourceCap += Setting_Button_Controller_showRessourceCap;
	}

	void Setting_Button_Controller_showRessourceCap() {

		Debug.Log ("Setting_Button_Controller_showRessourceCap");

		if (showStatus < 4)
			showStatus++;
		else
			showStatus = 0;

		switch (showStatus) {
		case 0:
			RessourceParent.SetActive (false);
			WeightPenalty.SetActive (false);
			TemperaturInfo.SetActive (false);
			CoordinatesParent.SetActive (false);
			break;
		case 1:
			RessourceParent.SetActive (true);
			WeightPenalty.SetActive (false);
			TemperaturInfo.SetActive (false);
			CoordinatesParent.SetActive (false);
			break;
		case 2:
			RessourceParent.SetActive (false);
			WeightPenalty.SetActive (true);
			TemperaturInfo.SetActive (false);
			CoordinatesParent.SetActive (false);
			break;
		case 3:
			RessourceParent.SetActive (false);
			WeightPenalty.SetActive (false);
			TemperaturInfo.SetActive (true);
			CoordinatesParent.SetActive (false);
			break;
		case 4:
			RessourceParent.SetActive (false);
			WeightPenalty.SetActive (false);
			TemperaturInfo.SetActive (false);
			CoordinatesParent.SetActive (true);
			break;
		}



	}
	
}

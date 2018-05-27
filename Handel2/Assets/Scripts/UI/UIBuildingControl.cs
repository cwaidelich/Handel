using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(GenericUIController))]
public class UIBuildingControl : MonoBehaviour {

	BuildingBehaivor3 bb;
	GameObject go;

	public Text statusText, nameText, invText;
	public GameObject flagGroup;
	public Toggle showProductionCheckBox;


	private string lastButtonCalled;
	private int offset;
	private bool activeBuilding;

	private List<GameObject> selectedGameObjects = new List<GameObject>();


	void Update () {
		if (GetComponent<GenericUIController> ().isActive) {
			
			selectedGameObjects = GetComponent<GenericUIController> ().UIcaller;
			if (selectedGameObjects.Count == 1) {
				bb = selectedGameObjects [0].GetComponent<BuildingBehaivor3> ();
				nameText.text = bb.BuildingName;
				statusText.text = bb.CurrentState;
				invText.text = bb.GetComponent<Inventory> ().ContentReadable ();
				showProductionCheckBox.isOn = bb.ActiveBuilding;
				flagGroup.SetActive (!bb.ActiveBuilding);
			} else {
				nameText.text = "several..";
				statusText.text = "undefined";
				invText.text = "undefined";

			}


		} 
	}

	public void OnClickProduction () {

		if (selectedGameObjects.Count == 1) {
			if (bb.ActiveBuilding) {
				bb.ActiveBuilding = false;
			} else {
				bb.ActiveBuilding = true;
			}
		} else {
			if (showProductionCheckBox.isOn) {
				foreach (GameObject _b in selectedGameObjects) {
					_b.GetComponent<BuildingBehaivor3> ().ActiveBuilding = false;
				}
			} else {
				foreach (GameObject _b in selectedGameObjects) {
					_b.GetComponent<BuildingBehaivor3> ().ActiveBuilding = true;
				}

			}
				
		}

	}

	public void OnClickedFlag(Button _button) {
		if (selectedGameObjects.Count == 1) {
			if (lastButtonCalled == _button.name) {
				if (offset == 0)
					offset = 1;
				else
					offset = 0;
			} else {
				offset = 0;
			}

			print ("pressed: " + _button.name + ",Offset: " + offset);
			bb.setFlag (_button.name, offset);
			lastButtonCalled = _button.name;
		}
	}



}

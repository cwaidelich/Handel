using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(GenericUIController))]
public class UIEinwohnerControl : MonoBehaviour {

	EinwohnerBehaivor2 eb2;

	public Text statusText, capacity, battery;
	private List<GameObject> selectedGameObjects = new List<GameObject>();

	void Update () {
		if (GetComponent<GenericUIController> ().isActive) {
			selectedGameObjects = GetComponent<GenericUIController> ().UIcaller;
			if (selectedGameObjects.Count == 1) {

				eb2 = selectedGameObjects [0].GetComponent<EinwohnerBehaivor2> ();

				battery.text = "Battery: "+Mathf.RoundToInt(eb2.batteryStatus*100)+" %";
				capacity.text = "Inventory: " + eb2.TotalElementCount +",WO:"+eb2.currentWorkOrder.Number.ToString();

				statusText.text = eb2.CurrentState;
			} else {

				battery.text = "Battery: "+Mathf.RoundToInt(eb2.batteryStatus*100)+" %";
				capacity.text = "Inventory: " + eb2.TotalElementCount +",WO:"+eb2.currentWorkOrder.Number.ToString();

				statusText.text = eb2.CurrentState;

			}

			/*
			GameObject go = GetComponent<GenericUIController> ().UIcaller;
			*/
		}
	}
	
}

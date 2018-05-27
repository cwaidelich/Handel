using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIStreetControl : MonoBehaviour {

	public Text section;
	[SerializeField]
	private GameObject SectionPanel;
	StreetBehaivor sb;

	private List<GameObject> selectedGameObjects = new List<GameObject>();


	void Update () {
		if (GetComponent<GenericUIController> ().isActive) {
			//GameObject go = GetComponent<GenericUIController> ().UIcaller;
			selectedGameObjects = GetComponent<GenericUIController> ().UIcaller;
			if (selectedGameObjects.Count == 1) {
				sb = selectedGameObjects [0].GetComponent<StreetBehaivor> ();
				if (sb.ElementToTransport == null)
					section.text = "Open";
				else
					section.text = sb.ElementToTransport.ToString ();
			} else {
				section.text = ReturnSection (selectedGameObjects);
			}

		} 
	}

	public string ReturnSection(List<GameObject> _goL) {

		if (_goL.Count > 0) {
			if (_goL [0].GetComponent<StreetBehaivor> () == null)
				return "undefined.";
			string returnVal = _goL [0].GetComponent<StreetBehaivor> ().ElementToTransport.ToString ();

			for (int i = 0; i < _goL.Count; i++) {
				string elementToTransport = _goL [i].GetComponent<StreetBehaivor> ().ElementToTransport.ToString ();
				if (returnVal != elementToTransport)
					returnVal = "several..";
			}
			return returnVal;
		}
		return "undefined..";
	}

	public void ShowSectionPanel(bool _v) {
		SectionPanel.SetActive (_v);
	}

	public void DefineElement (Element _element) {
		foreach (GameObject _go in selectedGameObjects) {
			_go.GetComponent<StreetBehaivor> ().ElementToTransport = _element;
		}
	}

	public void UndefineElement () {
		foreach (GameObject _go in selectedGameObjects) {
			_go.GetComponent<StreetBehaivor> ().ElementToTransport = new Element();
		}
	}

}

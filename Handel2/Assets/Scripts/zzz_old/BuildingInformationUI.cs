using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingInformationUI : MonoBehaviour {

	public GameObject buildingNameField;

	private Text nameField;

	void Start() {
		nameField = GameObject.Find ("Field_Name").GetComponent<Text>();
	}

	void Update() {
		//nameField.text = buildingNameField.GetComponent<WohnungsBehaivor> ().buildingName.ToString();
	}
}

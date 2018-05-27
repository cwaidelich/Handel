using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureTextUpdate : MonoBehaviour {

	Node currentNode;
	Grid theGrid;
	[SerializeField]
	TextMesh temperatureText;
	// Use this for initialization
	void Start () {
		theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
		currentNode = theGrid.NodeFromWorldPoint (transform.position);


	}
	
	// Update is called once per frame
	void Update () {
		temperatureText.text = currentNode.Temperature.ToString ();
	}

}

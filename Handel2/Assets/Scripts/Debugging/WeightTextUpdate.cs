using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightTextUpdate : MonoBehaviour {

	Node currentNode;
	Grid theGrid;

	// Use this for initialization
	void Start () {
		theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
		currentNode = theGrid.NodeFromWorldPoint (transform.position);


	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh> ().text = currentNode.movementPenalty.ToString ();
	}

}

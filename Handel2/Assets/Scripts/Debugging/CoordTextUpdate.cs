
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordTextUpdate : MonoBehaviour {

	Node currentNode;
	Grid theGrid;
	Vector2 gridPos;
	// Use this for initialization
	void Start () {
		theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
		currentNode = theGrid.NodeFromWorldPoint (transform.position);
		gridPos = currentNode.getGridPosition ();


	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh> ().text = Mathf.RoundToInt (gridPos.x) + "," + Mathf.RoundToInt (gridPos.y);
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class background_beha : MonoBehaviour {

	public Sprite day,night;
	public GameObject ResText;
	public GameObject CoordText;
	public GameObject WeightPenaltyText;
	public GameObject TemperatureText;


	public GameObject elementIcon;
	public Vector3 localSlotLocation2;
	public Vector3 localSize = new Vector3 (0.4f, 0.4f, 1f);
	private List<GameObject> localSlot2 = new List<GameObject>();

	[SerializeField]
	private int showStatus = 0;

	private Grid theGrid;
	private Transform refObject;

	private Node currentNode;
	private float worldSizeX;
	private SpriteRenderer sr;
	private Inventory inventory;


	// Use this for initialization
	void Start () {
		
		theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
		refObject = GameObject.Find ("Night0.1Ref").transform;
		currentNode = theGrid.NodeFromWorldPoint (transform.position);

		sr = GetComponent<SpriteRenderer> ();

		Planet planet = theGrid.getPlanet;
		worldSizeX = planet.gridWorldSize.x;

		inventory = currentNode.inventory;
	}



	void Update() {

		float dif = Mathf.Abs (refObject.position.x - transform.position.x);
		float alpha = Mathf.Sin (dif * Mathf.PI / worldSizeX);

		if (currentNode == null) {
			Debug.LogError ("currentNode is null!");
			theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
			currentNode = theGrid.NodeFromWorldPoint (transform.position);
		}

		if (alpha >= 0.5) {
			sr.sprite = day;	
			currentNode.IsNight = false;
		} else {
			sr.sprite = night;
			currentNode.IsNight = true;
		}

	}

	/*
	public void Setting_Button_Controller_showRessourceCap ()
	{
		Debug.Log ("Setting_Button_Controller_showRessourceCap");

		if (showStatus < 3)
			showStatus++;
		else
			showStatus = 0;

		switch (showStatus) {
		case 0:
			ResText.SetActive (false);
			CoordText.SetActive (false);
			WeightPenaltyText.SetActive (false);
			TemperatureText.SetActive(false);
			break;
		case 1:
			ResText.SetActive (true);
			CoordText.SetActive (true);
			WeightPenaltyText.SetActive (false);
			TemperatureText.SetActive(false);
			break;
		case 2:
			ResText.SetActive (false);
			CoordText.SetActive (true);
			WeightPenaltyText.SetActive (true);
			TemperatureText.SetActive(false);
			break;
		case 3:
			ResText.SetActive (false);
			CoordText.SetActive (false);
			WeightPenaltyText.SetActive (false);
			TemperatureText.SetActive(true);
			break;
		}

	}
*/
	void ShowInv() {
		int counter = 0;
		foreach (Element _e in inventory.AllElementsinInventory()) {
			if (_e != currentNode.PlanetsElement) {

				//print (gameObject.name+ ":found Element in Inventory: " + _e.elementName);
				GameObject tempSlot = localSlot2.Find (i => i.name == "LocalSlot_" + _e.name);
				if (tempSlot == null) {
					tempSlot = (GameObject)Instantiate (elementIcon, transform);
					tempSlot.transform.localScale = localSize;
					tempSlot.transform.localPosition = localSlotLocation2 + new Vector3 (localSlotLocation2.x + 2 * counter, localSlotLocation2.y, localSlotLocation2.z);
					tempSlot.name = "LocalSlot_" + _e.name;
					tempSlot.GetComponent<ElementBehaivor> ().Element = _e;
					tempSlot.GetComponent<ElementBehaivor> ().Amount = inventory.Count (_e);

					localSlot2.Add (tempSlot);
				} else {
					if (inventory.Count (_e) > 0) {
						tempSlot.SetActive (true);
						tempSlot.transform.localPosition = localSlotLocation2 + new Vector3 (localSlotLocation2.x + 2 * counter, localSlotLocation2.y, localSlotLocation2.z);
						tempSlot.GetComponent<ElementBehaivor> ().Amount = inventory.Count (_e);
						counter++;
					} else {
						tempSlot.SetActive (false);
					}
				}

			}
		}

	}

}

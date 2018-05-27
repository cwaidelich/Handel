using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory:MonoBehaviour  {

	public Vector3 localSlotLocation1;
	public Vector3 localSlotLocation2;
	public Vector3 localSize = new Vector3 (0.4f, 0.4f, 1f);
	public float capacity;

	private float inventorySlotMax; 
	private List<InventorySlots> inventorySlots = new List<InventorySlots>();
	public GameObject elementIcon;

	private List<GameObject> localSlot1 = new List<GameObject>();
	private List<GameObject> localSlot2 = new List<GameObject>();



	void Start() {
		inventorySlotMax = capacity;
	}



	public void ShowSlot () { 


		if (Count () > 0) {


			if (localSlot1 == null)
				localSlot1 = new List<GameObject> ();
			if (localSlot2 == null)
				localSlot2 = new List<GameObject> ();


			int counter = 0;
			//print (gameObject.name+ ":ShowSlot started, Elements in Inv:"+AllElementsInInventory().Length);
			foreach (Element _e in AllElementsInInventoryNotGive ()) {
				//print (gameObject.name+ ":found Element in Inventory: " + _e.elementName);
				GameObject tempSlot = localSlot1.Find (i => i.name == "LocalSlot_" + _e.name);
				if (tempSlot == null) {
					tempSlot = (GameObject)Instantiate (elementIcon, transform);
					tempSlot.transform.localScale = localSize;
					tempSlot.transform.localPosition = localSlotLocation1 + new Vector3 (localSlotLocation1.x + 2 * counter, localSlotLocation1.y, localSlotLocation1.z);
					tempSlot.name = "LocalSlot_" + _e.name;
					tempSlot.GetComponent<ElementBehaivor> ().Element = _e;
					tempSlot.GetComponent<ElementBehaivor> ().Amount = Count (_e);

					localSlot1.Add (tempSlot);
				} else {
					if (Count (_e) > 0) {
						tempSlot.SetActive (true);
						tempSlot.transform.localPosition = localSlotLocation1 + new Vector3 (localSlotLocation1.x + 2 * counter, localSlotLocation1.y, localSlotLocation1.z);
						tempSlot.GetComponent<ElementBehaivor> ().Amount = Count (_e);
						counter++;
					} else {
						tempSlot.SetActive (false);
					}
				}
			}


			counter = 0;



			foreach (Element _e in AllElementsInInventoryGive ()) {
				//print (gameObject.name+ ":found Element in Inventory: " + _e.elementName);
				GameObject tempSlot = localSlot2.Find (i => i.name == "LocalSlot_" + _e.name);
				if (tempSlot == null) {
					tempSlot = (GameObject)Instantiate (elementIcon, transform);
					tempSlot.transform.localScale = localSize;
					tempSlot.transform.localPosition = localSlotLocation2 + new Vector3 (localSlotLocation2.x + 2 * counter, localSlotLocation2.y, localSlotLocation2.z);
					tempSlot.name = "LocalSlot_" + _e.name;
					tempSlot.GetComponent<ElementBehaivor> ().Element = _e;
					tempSlot.GetComponent<ElementBehaivor> ().Amount = Count (_e);

					localSlot2.Add (tempSlot);
				} else {
					if (Count (_e) > 0) {
						tempSlot.SetActive (true);
						tempSlot.transform.localPosition = localSlotLocation2 + new Vector3 (localSlotLocation2.x + 2 * counter, localSlotLocation2.y, localSlotLocation2.z);
						tempSlot.GetComponent<ElementBehaivor> ().Amount = Count (_e);
						counter++;
					} else {
						tempSlot.SetActive (false);
					}
				}
			}


		} else {
			for (int i = 0; i < localSlot1.Count; i++) {
				if (localSlot1[i] != null)
					GameObject.Destroy (localSlot1 [i]);
				localSlot1.Remove (localSlot1 [i]);
			}
			for (int i = 0; i < localSlot2.Count; i++) {
				if (localSlot2[i] != null)
					GameObject.Destroy (localSlot2 [i]);
				localSlot2.Remove (localSlot2 [i]);
			}
		}
	}

	private void hideSlots() {
		for (int i = 0; i < localSlot1.Count; i++) {
			GameObject.Destroy (localSlot1 [i]);
			localSlot1.Remove (localSlot1 [i]);
		}
		for (int i = 0; i < localSlot2.Count; i++) {
			GameObject.Destroy (localSlot2 [i]);
			localSlot2.Remove (localSlot2 [i]);
		}

	}

	public void Transfer (Element _element, float _amount, Inventory _destination, bool _isGiveOwn, bool _isGiveDestination) {
		//reducing own Inv
		float ownInacceptableAmount = Increment (_element, -_amount,_isGiveOwn);
		ownInacceptableAmount = _destination.Increment (_element, -ownInacceptableAmount,_isGiveDestination);
		//incrementing destiny Inv
		float destinyInacceptableAmount =_destination.Increment(_element,_amount,_isGiveDestination);
		float a = Increment (_element, -destinyInacceptableAmount,_isGiveOwn);
	}

	// gibt was es nicht ins Inv rein tun konnte als int zurück
	public float Increment(Element _element, float _amount, bool _isGive) {
		float returnVal,delta;
		returnVal = Max - (Count() + _amount);
		if (returnVal > 0)
			returnVal = 0;
		delta = _amount + returnVal;

		if (inventorySlots.Find (e => e.elementKey == _element.Key) != null) {
			inventorySlots.Find (e => e.elementKey == _element.Key).currentSlotCapacity += delta;
		} else {
			inventorySlots.Add (new InventorySlots () { element = _element, currentSlotCapacity = delta, isGive = _isGive });
		}

		StatsContainer.UpdateStat ("ELEMENT_AMOUNT", _element.Key, "RAISE", delta);
		return returnVal;
	}

	public void Define(Element _element, float _amount, bool _isGive) {
		Zero (_element);
		Increment (_element, _amount, _isGive);
	}

	public float Count(Element _element) {
		float counter = 0;
		for (int i = 0; i < inventorySlots.Count; i++) {
			if (inventorySlots [i].elementKey == _element.Key)
				counter = counter+inventorySlots[i].currentSlotCapacity;
		}
		return counter;
	}
		
	public float Count() {
		float counter = 0;
		for (int i = 0; i < inventorySlots.Count; i++) {
			counter = counter+inventorySlots[i].currentSlotCapacity;
		}
		return counter;
	}

	public float CountAllBut(Element _element) {
		float counter = 0;
		for (int i = 0; i < inventorySlots.Count; i++) {
			if (inventorySlots [i].element != _element)
				counter += Count (inventorySlots [i].element);
		}
		return counter;
	}

	public float Max {
		get {
			return inventorySlotMax;
		}
		set {
			inventorySlotMax = value;
		}
	}

	public void Zero(Element _element) {
		StatsContainer.UpdateStat ("ELEMENT_AMOUNT", _element.Key, "REDUCE", Count(_element));
		inventorySlots.Remove (inventorySlots.Find (e => e.elementKey == _element.Key));
	}

	public void ZeroAll() {
		for (int i = 0; i < inventorySlots.Count; i++) {
			Zero (inventorySlots [i].element);
		}
	}

	public string ContentReadable() {
		string output = "";
		for (int i = 0; i < inventorySlots.Count; i++) {
			output = output+inventorySlots[i].element.Shorty +":"+ inventorySlots[i].currentSlotCapacity+";";
		}
		output = output + "("+Count ()+")";
		return output;
	}

	private Element[] AllElementsInInventoryGive() {
		List <Element> returnValue = new List<Element>();
		for (int i = 0; i < inventorySlots.Count; i++) {
			if (inventorySlots[i].isGive == true)
				returnValue.Add (inventorySlots [i].element);		
		}
		return returnValue.ToArray ();
	}

	private Element[] AllElementsInInventoryNotGive() {
		List <Element> returnValue = new List<Element>();
		for (int i = 0; i < inventorySlots.Count; i++) {
			if (inventorySlots[i].isGive == false)
				returnValue.Add (inventorySlots [i].element);		
		}
		return returnValue.ToArray ();
	}

	public Element[] AllElementsinInventory() {
		List <Element> returnValue = new List<Element>();
		for (int i = 0; i < inventorySlots.Count; i++) {
			returnValue.Add (inventorySlots [i].element);		
		}
		return returnValue.ToArray ();

	}

	public Element GetFirstCarryingElement () {
		for (int i = 0; i < inventorySlots.Count; i++) {
			if (inventorySlots [i].currentSlotCapacity > 0)
				return inventorySlots [i].element;
		}
		return new Element ();
	}
}

class InventorySlots : List<InventorySlots> {

	public Element element;
	public float currentSlotCapacity;
	public bool isGive;

	public int elementKey {
		get {
			return element.Key;
		}
	}
}
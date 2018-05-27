using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {

	private Vector3 worldCoordinates;
	private Vector3 snapPosition;

	private bool isWalkable, isEmptyToBuild;
	private bool hasName;
	private Vector2 gridPosition; //lon, lat
	private string targetName;
	private int heapIndex;

	public enum Content {empty,street,building}; 
	public Content content;
	public int movementPenalty;
	private Element elementInStreet; 

	private float currentTemperature;
	private Element primaryElement;
	private int primaryElementAmountPerTile;

	public Inventory inventory;

	private bool isNight;

	public int gCost;
	public int hCost;

	public Node parent;

	public Node (Vector3 _worldCoor, bool _isWalkable, float _longitud, float _latitud, Vector3 _snap, Element _element, int _elementAmount, float _startTempInKelvin) {
		worldCoordinates = _worldCoor;
		primaryElement = _element;
		primaryElementAmountPerTile = _elementAmount;

		isWalkable = _isWalkable;
		isEmptyToBuild = true;

		gridPosition.x = _longitud;
		gridPosition.y = _latitud;
		snapPosition = _snap;

		hasName = false;
		content = Content.empty;

		movementPenalty = 1000;

		currentTemperature = _startTempInKelvin;

		inventory = new Inventory ();
		inventory.Max = primaryElementAmountPerTile*10;
		inventory.Increment (primaryElement, primaryElementAmountPerTile, false);
	}




	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public float amountPrimaryElement {
		get { return inventory.Count(primaryElement); }
		set { inventory.Define (primaryElement, value, false); }
	}


	public bool hasElementRessource(Element _e) {
		if (inventory.Count(_e)>0)
			return true;
		return false;
	}

	public void reducePrimaryElement(float _amount) {
		inventory.Increment (primaryElement, -_amount, false);
	}

	public bool getHasName() {
		return hasName;
	}

	public string getName() {
		return targetName;
	}

	public void setName(string n) {
		targetName = n;
		hasName = true;
	}

	public Vector3 getWorldCoor() {
		return worldCoordinates;	
	}

	public Vector3 getWorldCoorForSnap(int _width, int _height, float zPos) {
		float xPos, yPos;
		if (_width % 2 == 0)
			xPos = snapPosition.x;
		else
			xPos = worldCoordinates.x;

		if (_height % 2 == 0)
			yPos = snapPosition.y;
		else
			yPos = worldCoordinates.y;
		
		return new Vector3 (xPos, yPos, zPos);

	}


	public Vector2 getGridPosition () {
		return gridPosition;
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node verticeToCompare) {
		int compare = fCost.CompareTo (verticeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (verticeToCompare.hCost);
		}
		return -compare;
	}


	// GET AND SETTER

	public bool Walkable {
		get {
			return isWalkable;
		}
		set {
			isWalkable = value;
		}
	}


	public bool IsEmptyToBuild {
		get {
			return isEmptyToBuild;
		}
		set {
			isEmptyToBuild = value;
		}
	}

	public Element ElementForTransport {
		get {
			return elementInStreet;
		}
		set {
			elementInStreet = value;
		}
	}

	public Element PlanetsElement {
		get {
			return primaryElement;
		}
	}

	public bool IsNight {

		set {
			isNight = value;
		}
		get {
			return isNight;
		}
	}

	public float Temperature {
		set {
			currentTemperature = value;
		}
		get {
			return currentTemperature;
		}
	}
}

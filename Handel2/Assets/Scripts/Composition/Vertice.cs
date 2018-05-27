using UnityEngine;
using System.Collections;

public class Vertice : IHeapItem<Vertice> {

	private Vector3 worldCoordinates;

	private bool isWalkable;
	private bool hasName;
	private Vector2 gridPosition; //lon, lat
	private string targetName;
	private int heapIndex;


	public int gCost;
	public int hCost;

	public Vertice parent;

	public Vertice (Vector3 _worldCoor, bool _isWalkable, float _longitud, float _latitud) {
		worldCoordinates = _worldCoor;
		isWalkable = _isWalkable;
		gridPosition.x = _longitud;
		gridPosition.y = _latitud;
		hasName = false;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public void setWalkable(bool v) {
		isWalkable = v;
	}

	public bool getWalkable() {
		return isWalkable;
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

	public int CompareTo(Vertice verticeToCompare) {
		int compare = fCost.CompareTo (verticeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (verticeToCompare.hCost);
		}
		return -compare;
	}

}

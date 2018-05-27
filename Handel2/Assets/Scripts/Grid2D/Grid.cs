using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {


	public GameObject CoordinatesTextPrefab;
	public Transform CoordinatesTextParent;

	public GameObject WeightTextPrefab;
	public Transform WeightTextParent;

	public GameObject TemperatureTextPrefab;
	public Transform TemperatureTextParent;

	public GameObject RessourceTextPrefab;
	public Transform RessourceTextParent;

	public Transform backgroundSprite;
	public Transform backgroundParent;

	public LayerMask unwalkable;

	public bool drawGridObstacles = false;
	public bool drawGridMap = false;

	[SerializeField]
	private Planet planet;


	Node[,] grid;


	float nodeDiameter;
	int gridSizeX, gridSizeY;
	Vector3 worldBottomLeft;

	void Start() {
		nodeDiameter = planet.nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (planet.gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (planet.gridWorldSize.y / nodeDiameter);

		print ("gridSizeX: " + gridSizeX);
		print ("gridSizeY: " + gridSizeY);

		print ("ideal GridWorldSize X: " + gridSizeX*nodeDiameter);
		print ("ideal GridWorldSize Y: " + gridSizeY*nodeDiameter);



		CreateBuild ();
	}



	void CreateBuild() {
		grid = new Node[gridSizeX, gridSizeY];
		worldBottomLeft = transform.position - Vector3.right * planet.gridWorldSize.x / 2 - Vector3.up * planet.gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x*nodeDiameter+planet.nodeRadius) + Vector3.up *(y*nodeDiameter+planet.nodeRadius);
				Vector3 snapPoint = worldBottomLeft + Vector3.right * (x*nodeDiameter+nodeDiameter) + Vector3.up *(y*nodeDiameter+nodeDiameter);

				Node actualNode = new Node (worldPoint, true, x, y,snapPoint,planet.primaryElement,planet.primaryElementAmountPerTile,planet.temperatureKelvin);
				grid [x, y] = actualNode;
				Instantiate (backgroundSprite.gameObject, grid[x,y].getWorldCoor (), Quaternion.Euler (Vector3.zero),backgroundParent);
				Vector3 location = grid[x,y].getWorldCoor ();
				Vector3 newLocation = new Vector3(location.x,location.y,-2f);
				Instantiate (RessourceTextPrefab, newLocation, Quaternion.Euler (Vector3.zero),RessourceTextParent);
				Instantiate (CoordinatesTextPrefab, newLocation, Quaternion.Euler (Vector3.zero),CoordinatesTextParent);
				Instantiate (WeightTextPrefab, newLocation, Quaternion.Euler (Vector3.zero),WeightTextParent);
				Instantiate (TemperatureTextPrefab, newLocation, Quaternion.Euler (Vector3.zero),TemperatureTextParent);

			}
		}
		RessourceTextParent.gameObject.SetActive (false);
		CoordinatesTextParent.gameObject.SetActive (false);
		WeightTextParent.gameObject.SetActive (false);
		TemperatureTextParent.gameObject.SetActive (false);
	}

	public int getGridSize {
		get { return gridSizeX * gridSizeY;}
	}

	public Node getNote(int x, int y) {
		return grid [x, y];
	}

	public Node getNodeOffset(Node _node, int _offsetDown, int _offsetRight) {
		Vector2 gridpos = _node.getGridPosition ();
		int gridposX = (int) gridpos.x;
		int gridposY = (int) gridpos.y;

		return grid [gridposX - _offsetRight, gridposY - _offsetDown];
	}

	public Node NodeFromWorldPoint (Vector3 _worldPosition) {
		float percentX = (_worldPosition.x + planet.gridWorldSize.x / 2) / planet.gridWorldSize.x;
		float percentY = (_worldPosition.y + planet.gridWorldSize.y / 2) / planet.gridWorldSize.y;
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid [x, y];
	}

	public Vector3 returnWorldCoorFromNode(int x, int y, bool returnSnapValue, int zVal, int width, int height) {
		if (returnSnapValue) 
			return grid[x,y].getWorldCoorForSnap (width,height,zVal);
		return grid[x,y].getWorldCoor ();
	}


	//checkInfo can be: Ressource
	public bool checkNodeFor(int x, int y, Element _element) {
		//Debug.Log (x + " , " + y + " , ");// + grid [x, y].primaryElementAmount);
		if (grid [x, y].hasElementRessource(_element))
			return true;
		return false;
	}


	public List<Node> GetNeighbours (Node v) {
		List<Node> neighbours = new List<Node>();
		Vector2 currentGridValue = v.getGridPosition();
		Debug.Log ("current Grid Value: " + currentGridValue);
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				int neighbourValueX = (int) currentGridValue.x + i;
				int neighbourValueY = (int) currentGridValue.y + j;
				if (neighbourValueX > 0 && neighbourValueX < gridSizeX && neighbourValueY > 0 && neighbourValueY < gridSizeY) {
					Node currNode = grid [neighbourValueX, neighbourValueY];
					if(!IsObjectColliding(Range(currNode,1,1).ToArray(),"walk"))
						neighbours.Add (currNode);
				}
			}
		}
		return neighbours;
	}

	public List<Node> GetNeighbours (Node[] _nodes) {
		List<Node> neighbours = new List<Node> ();
		foreach (Node node in _nodes) {
			neighbours.AddRange (GetNeighbours (node));
		}
		return neighbours;
	}

	public List<Node> Range (Node _current, int _weight, int _height) {
		List<Node> range = new List<Node> ();
		Vector2 curPos = _current.getGridPosition ();
		for (int i = 0; i < _weight; i++) {
			for (int j = 0; j < _height; j++) {
				Node currNode = grid [(int)curPos.x + i, (int)curPos.y + j];
				range.Add (currNode);
			}
		}
		return range;
	}




	public bool IsObjectColliding(Node[] _current, string _type) {

		foreach (var _node in _current) {
			if (_type == "walk") {
				if (_node.Walkable == false)
					return true;
			}
			if (_type == "build") {
				if (_node.IsEmptyToBuild == false)
					return true;
			}
		}
		return false;
	}

	public void DefineGridContent (Node[] _nodes, string _content) {
		Node.Content currentContent;
		bool walkable,empty;
		print (_content);
		switch (_content) {
		case "building":
			currentContent = Node.Content.building;
			walkable = false;
			empty = false;
			break;
		case "street":
			currentContent = Node.Content.street;
			walkable = true;
			empty = false;
			break;
		default:
			currentContent = Node.Content.empty;
			walkable = true;
			empty = true;
			break;
		}

		for (int i = 0; i < _nodes.Length; i++) {
			_nodes [i].content = currentContent;
			_nodes [i].Walkable = walkable;
			_nodes [i].IsEmptyToBuild = empty;
		}
	}


	void OnDrawGizmos() {
		//Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,gridWorldSize.y,1));
		if (grid != null && drawGridObstacles && !drawGridMap) {
			foreach (Node n in grid) {
				Gizmos.color = n.IsEmptyToBuild ? Color.white : Color.red;
				Gizmos.DrawCube (n.getWorldCoor(), Vector3.one * (nodeDiameter - 0.1f));
			}
		} else if (grid != null && !drawGridObstacles && drawGridMap) {
			foreach (Node n in grid) {
				switch (n.content) {
				case Node.Content.building:
					Gizmos.color = Color.blue;
					break;
				case Node.Content.street:
					Gizmos.color = Color.cyan;
					break;
				default:
					Gizmos.color = Color.white;
					break;
				}
				Gizmos.DrawCube (n.getWorldCoor(), Vector3.one * (nodeDiameter - 0.1f));
			}

		}

	}

	public Planet getPlanet {
		get {
			return planet;
		}
	}


}

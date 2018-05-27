using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFinding : MonoBehaviour {


	private PathRequestManager pathRequestManager;
	private Grid grid;
	//private PlanetShape pS;
	private Transform planet;



	void Awake() {
		pathRequestManager = GetComponent<PathRequestManager> ();
		grid = GameObject.Find ("A*").GetComponent<Grid> ();
		//planet = gameObject.transform;
 	}

	public void startFindPath(Vector3 startPath, Vector3 endPath, Element element) {
		StartCoroutine (FindPath (startPath, endPath, element));
	}


	IEnumerator FindPath(Vector3 _start, Vector3 _end, Element _element) {

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(_start);
		Node targetNode = grid.NodeFromWorldPoint(_end);

		if (startNode.Walkable && targetNode.Walkable) {

			Heap<Node> openSet = new Heap<Node> (grid.getGridSize );
			HashSet<Node> closedSet = new HashSet<Node> ();

			openSet.Add (startNode);

			while (openSet.Count > 0) {


				Node currentNode = openSet.RemoveFirst ();
				closedSet.Add (currentNode);

				if (currentNode == targetNode) {
					print ("// ziel gefunden :)");
					pathSuccess = true;

					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.Walkable || closedSet.Contains (neighbour)) {
						continue;
					}

					int elementPenalty = 200;
					if (currentNode.ElementForTransport == _element)
						elementPenalty = 0;
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour) + neighbour.movementPenalty + elementPenalty;
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GeneralStaticClass.GetDistance(neighbour, targetNode);

						neighbour.parent = currentNode;

						if (!openSet.Contains (neighbour)) {
							openSet.Add (neighbour);
						} else {
							openSet.UpdateItem (neighbour);
						}


					}

				}

			}
		}
		yield return null;
		if (pathSuccess) {
			waypoints = RetracePath (startNode, targetNode);
		} else {
			print ("didnt find path");
		}
		pathRequestManager.FinishedProcessingPath (waypoints, pathSuccess);
	}

	private Vector3[] RetracePath (Node start, Node end) {
		List<Node> path = new List<Node> ();
		Node currentNode = end;

		while (currentNode != start) {
			path.Add (currentNode);
			currentNode = currentNode.parent;

		}
		Vector3[] waypoints = SymplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;
	}

	Vector3[] SymplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3> ();

		for (int i = 0; i < path.Count; i++) {
			Vector3 t = path [i].getWorldCoor ();
			waypoints.Add (new Vector3(t.x,t.y,-5));
			print ("waypoint(" + (i) + "):" + waypoints[i]);
			print ("node grid value: " + path [i].getGridPosition ().ToString());
		}
		return waypoints.ToArray ();
	}

	//siehe GeneralStaticClass ->!!
	private int GetDistance(Node v1, Node v2) {
		Vector2 v1Pos = v1.getGridPosition ();//lon, lat
		Vector2 v2Pos = v2.getGridPosition ();//lon, lat

		int dstLon = (int) Mathf.Abs (v1Pos.x - v2Pos.x);
		int dstLat = (int) Mathf.Abs (v1Pos.y - v2Pos.y);

 		if (dstLon > dstLat) {
			return 14 * dstLat + 10 * (dstLon - dstLat);
		} else {
			return 14 * dstLon + 10 * (dstLat - dstLon);
		}


	}

}

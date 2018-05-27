using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFinding_backup : MonoBehaviour {


	private PathRequestManager pathRequestManager;
	private Grid grid;
	//private PlanetShape pS;
	private Transform planet;
	createMesh cM;


	void Awake() {
		pathRequestManager = GetComponent<PathRequestManager> ();
		grid = GameObject.Find ("A*").GetComponent<Grid> ();
		cM = GetComponent<createMesh> ();
		//planet = gameObject.transform;
 	}

	public void startFindPath(Vector3 startPath, Vector3 endPath) {
		StartCoroutine (FindPath (startPath, endPath));
	}


	IEnumerator FindPath(Vector3 _start, Vector3 _end) {

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Vertice startVertice = cM.WorldCoorToGridPosition (_start, planet);
		Vertice targetVertice = cM.WorldCoorToGridPosition (_end, planet);
		if (startVertice.getWalkable () && targetVertice.getWalkable ()) {

			Heap<Vertice> openSet = new Heap<Vertice> (cM.GridMaxSize);
			HashSet<Vertice> closedSet = new HashSet<Vertice> ();

			openSet.Add (startVertice);

			while (openSet.Count > 0) {


				Vertice currentVertice = openSet.RemoveFirst ();
				closedSet.Add (currentVertice);

				if (currentVertice == targetVertice) {
					print ("// ziel gefunden :)");
					pathSuccess = true;

					break;
				}

				foreach (Vertice neighbour in cM.GetNeighbours(currentVertice)) {
					if (!neighbour.getWalkable () || closedSet.Contains (neighbour)) {
						continue;
					}

					int newMovementCostToNeighbour = currentVertice.gCost + GetDistance (currentVertice, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance (neighbour, targetVertice);

						neighbour.parent = currentVertice;

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
			waypoints = RetracePath(startVertice,targetVertice);
		}
		pathRequestManager.FinishedProcessingPath (waypoints, pathSuccess);
	}

	private Vector3[] RetracePath (Vertice start, Vertice end) {
		List<Vertice> path = new List<Vertice> ();
		Vertice currentVertice = end;

		while (currentVertice != start) {
			path.Add (currentVertice);
			currentVertice = currentVertice.parent;

		}
		Vector3[] waypoints = SymplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;
	}

	Vector3[] SymplifyPath(List<Vertice> path) {
		List<Vector3> waypoints = new List<Vector3> ();

		for (int i = 0; i < path.Count; i++) {
			waypoints.Add (planet.transform.TransformPoint(path [i].getWorldCoor ()));
			print ("waypoint(" + (i-1) + "):" + waypoints[i-1]);
			print ("node grid value: " + path [i].getGridPosition ().ToString());
		}
		return waypoints.ToArray ();
	}

	//siehe GeneralStaticClass ->!!
	private int GetDistance(Vertice v1, Vertice v2) {
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

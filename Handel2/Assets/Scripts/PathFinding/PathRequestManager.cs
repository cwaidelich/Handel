using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest> ();
	PathRequest currentPathRequest;
	PathFinding pathFinding;
	bool isProcessingPath;

	static PathRequestManager instance;

	void Awake() {
		instance = this;
		isProcessingPath = false;
		pathFinding = GetComponent<PathFinding> ();
	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[],bool> callback, Element element) {
		PathRequest newRequest = new PathRequest (pathStart, pathEnd, callback, element);
		instance.pathRequestQueue.Enqueue (newRequest);
		instance.TryProcessNext ();
	}

	private void TryProcessNext () {
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue ();
			isProcessingPath = true;
			pathFinding.startFindPath (currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.element); 
		}
	}

	public void FinishedProcessingPath(Vector3[] path, bool success) {


		currentPathRequest.callback (path, success);
		isProcessingPath = false;
		TryProcessNext ();
	}

	struct PathRequest {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[],bool> callback;
		public Element element;

		public PathRequest (Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[],bool> _callback,Element _element) {
			pathStart = _pathStart;
			pathEnd = _pathEnd;
			callback = _callback;
			element = _element;

		}
	}

}

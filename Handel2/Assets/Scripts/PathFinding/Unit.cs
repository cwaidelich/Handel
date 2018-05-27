using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public float speed = 5f;

	Vector3[] path;
	int targetIndex;
	bool drawGizmos = false;

	public void requestPath(Vector3 start, Vector3 end, Element element) {
		PathRequestManager.RequestPath (start, end, OnPathFound,element);
	}

	void OnPathFound(Vector3[] newPath, bool pathSuccess) {
		if (pathSuccess && newPath != null) {
			//print ("On PathFound Called, path successfull");
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		} 
	}


	IEnumerator FollowPath() {
		// PAth aufzeichnen

		drawGizmos = true;
		targetIndex = 0;
		Vector3 currentWayPoint = path [targetIndex];
		while (true) {
			if (currentWayPoint == transform.position) {
				//print ("i am at current waypoint" + transform.position);
				targetIndex++;
				//print ("targetIndex " + targetIndex);
				//print ("path.length " + path.Length);
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWayPoint = path [targetIndex];

			}
			//print ("move from " + transform.position + " to next waypoint " + currentWayPoint);
			//print ("difference between next waypoint and cur position: " + Vector3.Distance (transform.position, currentWayPoint));
			transform.position = Vector3.MoveTowards (transform.position, currentWayPoint, speed*Time.deltaTime/10);
			yield return null;
		}


	}

	void OnDrawGizmos() {
		if (drawGizmos) {
			for (int i = 0; i < path.Length; i++) {
				Gizmos.DrawSphere (path [i], 0.5f);
				if (i != 0)
					Gizmos.DrawLine (path [i - 1], path [i]);
			}
		}
	}
}

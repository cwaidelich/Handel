using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlanetShape : MonoBehaviour {

	public float overlapshere = 1f;
	public LayerMask buildingMask;
	public LayerMask targetMask;
	public createMesh cM;

	public bool drawGizmos = false;

	private Vertice[,] grid;
	private float timeBetweenCollisionCheck = 1f;

	public static event Action<float, float> lastCollitionCheck;

	void Start() {

		cM = GetComponent<createMesh> ();
		grid = createMesh.grid; //
		StartCoroutine(checkCollision());





	}


	private IEnumerator checkCollision()
	{

		while (true) {
			for (int _lat = 0; _lat < cM.gridLatLength; _lat++) {
				for (int _lon = 0; _lon < cM.gridLonLength; _lon++) {

					if (!cM.getHasName(_lat,_lon) && Physics.OverlapSphere (transform.TransformPoint (cM.getVerticeInGrid (_lat, _lon)), overlapshere, buildingMask).Length > 0) {
						cM.setWalkable (_lat, _lon, false);
						//print ("collision detected");
					} else {
						cM.setWalkable (_lat, _lon, true);
					}
				}
			}

			if (lastCollitionCheck!= null)
				lastCollitionCheck (Time.time, timeBetweenCollisionCheck);

			yield return new WaitForSeconds (timeBetweenCollisionCheck);
		}

	}

	void OnDrawGizmos() {
		if (!Application.isPlaying)
			return;

		if (drawGizmos && grid != null) {
			for (int _lat = 0; _lat < cM.gridLatLength; _lat++) {
				for (int _lon = 0; _lon < cM.gridLonLength; _lon++) {
					if (cM.getWalkable (_lat, _lon) == false) {
						Gizmos.color = Color.red;
					} else if (cM.getHasName (_lat, _lon)) {
						Gizmos.color = Color.blue;
					} else {
						Gizmos.color = Color.white;
					}


					Gizmos.DrawWireSphere (transform.TransformPoint(cM.getVerticeInGrid(_lat,_lon)), 0.05f);

				}
			}
		}
	}



	float GetDistanceBetweenVertices(Vector3 startVertice, Vector3 endVertice, float _radius) {
		return Mathf.Round(Mathf.Acos(Vector3.Dot(startVertice.normalized, endVertice.normalized))*_radius*1000f)/1000f;
	}

}

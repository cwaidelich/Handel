using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class meshCalcTest : MonoBehaviour {

	Mesh mesh;
	Vector3[] vertices;
	int[] triangles;

	List<int> triagleNum;
	List<Vector3> searchNear = new List<Vector3>();

	Vector3[] search;

	bool draw = false;
	// Use this for initialization
	void Start () {

		if (mesh != null) {
			mesh = GetComponent<MeshCollider> ().sharedMesh;

			vertices = mesh.vertices;
			triangles = mesh.triangles;

			print ("vertexCount: " + mesh.vertexCount);
			print ("triangleCount: " + triangles.Length);

			search = new Vector3[] {
				vertices [triangles [50]],
			};


			triagleNum = findNearVertices (vertices [triangles [50]], triangles);
			print ("triagleNum: " + triagleNum.Count);

			foreach (int i in triagleNum) {

				print ("i=" + i);
				print ("triangles [i]" + triangles [i]);
				print ("vertices[triangles [i]]" + vertices [triangles [i]]);
				print ("vertices[triangles [i+1]]" + vertices [triangles [i + 1]]);
				print ("vertices[triangles [i+2]]" + vertices [triangles [i + 2]]);
				/*
			searchNear.Add (vertices [triangles [i + 0]]);
			searchNear.Add (vertices [triangles [i + 1]]);
			searchNear.Add (vertices [triangles [i + 2]]);
			*/
				searchNear.Add (findVerticesByTriangle (i, 0));
				searchNear.Add (findVerticesByTriangle (i, 1));
				searchNear.Add (findVerticesByTriangle (i, 2));

			}


			draw = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private List<int> findNearVertices(Vector3 toSearch, int[] _triangles) {

		List<int> intResults = new List<int> ();


		//print ("searching :" + toSearch);
		for (int i = 0; i < _triangles.Length; i++) {
			if (CompareVectorPercetange (vertices [_triangles[i]], toSearch)) {
				print ("found "+vertices [_triangles [i]]+" in "+ i + "i mod 3="+(i%3));
				intResults.Add(i);
			}
		}
		return intResults;
	}

	private Vector3 findVerticesByTriangle(int _triangle,int pos) {
		Vector3 returnVertice = new Vector3();

		returnVertice = vertices [triangles [_triangle + pos - (_triangle % 3)]];
	
		return returnVertice;

	}


	void OnDrawGizmos() {
		if (!Application.isPlaying || !draw)
			return;
		if (vertices.Length != 0) {

			Gizmos.color = Color.red;
			foreach (Vector3 v in vertices) {
				Gizmos.DrawWireSphere (transform.TransformPoint (v), 0.05f);
			}
		}

		if (searchNear.Count != 0) {
			Gizmos.color = Color.blue;
			foreach (Vector3 n in searchNear) {
				if (n != Vector3.zero) {
					Gizmos.DrawWireSphere (transform.TransformPoint (n), 0.05f);
				}
			}
			//print ("draw Vector3 " + s);
		}
		if (search.Length != 0) {
			Gizmos.color = Color.white;
			foreach (Vector3 s in search) {
				Gizmos.DrawWireSphere (transform.TransformPoint (s), 0.05f);
				//print ("draw Vector3 " + s);
			}
		}



	}

	public bool CompareVectorPercetange(Vector3 me, Vector3 other)
	{
		if (me.x == other.x && me.y == other.y && me.z == other.z)
			return true;
		return false;
	
	}


}

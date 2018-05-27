using UnityEngine;
using System.Collections;

public class BackGroundHandler : MonoBehaviour {

	public GameObject backGroundTile;

	private Vector2 tilesAmount;
	private Vector2 startPoint;
	private Vector2 sizeTile;


	// Use this for initialization
	void Start () {
		
		Mesh planeMesh = backGroundTile.GetComponent<MeshFilter> ().sharedMesh;
		Bounds bounds = planeMesh.bounds;

		sizeTile.x = (backGroundTile.transform.localScale.x * bounds.size.x);
		sizeTile.y = (backGroundTile.transform.localScale.y * bounds.size.z);

		tilesAmount.x = Mathf.Floor((Camera.main.aspect * Camera.main.orthographicSize*2f) / sizeTile.x)-2; // 2 werden weggenommen, weil jeweils auf jeder seite ein halbes rausragt.
		tilesAmount.y = Mathf.Floor((Camera.main.orthographicSize*2f) / sizeTile.y)-2;

		startPoint.x = -tilesAmount.x * sizeTile.x / 2;
		startPoint.y = tilesAmount.y * sizeTile.y / 2;


		/*
		print ("bounds.size.y=" +bounds.size.x);
		print ("sizeTile.y=" + sizeTile.y);
		print ("Camera.main.orthographicSize*2f=" + Camera.main.orthographicSize * 2f);
		print ("tilesAmount.y = " + tilesAmount.y);
		print ("startPoint.y =" + startPoint.y);
*/

		Vector3 spawnposition = new Vector3 (startPoint.x, 0,startPoint.y);

		Instantiate (backGroundTile,spawnposition,Quaternion.Euler(new Vector3(0,0,0)));

		Vector2 spawnposition2 = new Vector2 (startPoint.x + sizeTile.x, 0);

		Instantiate (backGroundTile,spawnposition2,Quaternion.Euler(new Vector3(0,0,0)));



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

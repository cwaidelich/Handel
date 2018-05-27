using UnityEngine;
using System.Collections;

public class idleState : MonoBehaviour {

	public GameObject Einwohner;
	public float idleSpeed;
	
	// Update is called once per frame
	void Start () {
		transform.position = Vector3.zero;
	}

	void Update () {
		transform.position = Vector3.zero;
		//transform.RotateAround (Vector3.zero, Vector3.right, idleSpeed * Time.deltaTime);
	}
}

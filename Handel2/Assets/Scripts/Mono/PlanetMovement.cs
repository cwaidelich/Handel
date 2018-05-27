using UnityEngine;
using System.Collections;

public class PlanetMovement : MonoBehaviour {

	public Transform UserPlanet;
	public float orditspeed;
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (UserPlanet.position, new Vector3(0.1f,1f,0), Time.deltaTime * orditspeed);

	}
}

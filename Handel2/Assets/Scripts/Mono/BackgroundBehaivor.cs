using UnityEngine;
using System.Collections;

public class BackgroundBehaivor : MonoBehaviour {

	private string BackgroundType; //Mine1, Mine2, Bewohnbar, Grube, Berg


	// Use this for initialization
	void Start () {
		Color a = new Color(0.5f,0,1);
		changeColor (a);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeColor(Color c) {
		this.GetComponent<MeshRenderer>().material.color = c;
	}
}

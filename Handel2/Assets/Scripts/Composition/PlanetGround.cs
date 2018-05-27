using UnityEngine;
using System.Collections;

public class PlanetGround  {

	//Mine1, Mine2, Bewohnbar, Grube, Berg

	private string type;
	private Vector2 id;

	public PlanetGround () {
		type = "A";
	}

	public Vector2 defineId {
		get { return id; }
		set { id = value; }
	}


}

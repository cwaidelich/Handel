using UnityEngine;
using System.Collections;

public class BuildingElement:MonoBehaviour {

	private string bType;
	public GameObject Einwohner;

	public BuildingElement(string type) {
		bType = type;
		//Instantiate (Einwohner);
		print ("Building Element called");

	}


	public string getType {
		get { return bType; }
//		set { bType = value; }
	}

}

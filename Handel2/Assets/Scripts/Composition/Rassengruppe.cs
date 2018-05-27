using UnityEngine;
using System.Collections;

public class Rassengruppe {

	private string RassenName;
	private int RassenNr;


	public Rassengruppe() {
		// Rassengruppe aus DB ziehen
		RassenName = "Die Menschen";
		RassenNr = 1;
	}

	public string Name {
		get 
		{ 
			return RassenName;
		}
	}

	public int Nummer {
		get 
		{ 
			return RassenNr;
		}
	}



}

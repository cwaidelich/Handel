using UnityEngine;
using System.Collections;

public class Rasse  {

	private Element grundElement;
	private Element forschungsElement;
	private Element fortpflanzung;
	private Rassengruppe Rassengruppe;
	private bool ClassDataOk;

	public Rasse (string key) {
		RassenwerteErmitteln (key);
	}


	bool RassenwerteErmitteln (string key){
		// key wird danach für db auslesungen benutzt

		ClassDataOk = false;
		if (key == "1") {
			/*
			grundElement = new Element ("1");
			forschungsElement = new Element ("2");
			fortpflanzung = new Element ("3");
			*/
			Rassengruppe = new Rassengruppe ();

			ClassDataOk = true;
		}

		return ClassDataOk;


	}

	public bool isData {
		get { return ClassDataOk; }
	}




}

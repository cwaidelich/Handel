using UnityEngine;
using System.Collections;
using System;

public class Setting_Button_Controller : MonoBehaviour {

	// wird von background_beha aufgerufen..
	public static event Action showRessourceCap;

	public void onClickSettingButton(int i) {
		if (i == 1) {
			//aktiviert bei den Backgroud tiles die Textanzeige um die Ressourcenkazität zu zeigen
			if (showRessourceCap != null) {
				showRessourceCap ();
			}
		}

	}



}

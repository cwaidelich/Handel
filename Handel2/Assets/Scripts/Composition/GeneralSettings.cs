using UnityEngine;
using System.Collections;

public class GeneralSettings:MonoBehaviour {

	public void ChangeGameSpeed(float _speed) {
		Time.timeScale = _speed;
		Debug.Log ("Time.timeScale changed to: " + Time.timeScale.ToString ());
	}


}

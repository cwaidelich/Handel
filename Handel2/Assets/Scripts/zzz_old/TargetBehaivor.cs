using UnityEngine;
using System.Collections;

public class TargetBehaivor : MonoBehaviour {

	public Light targetLight;
	public bool isPlaced;

	// Update is called once per frame
	void Update () {
		if (isPlaced) {
			targetLight.gameObject.SetActive (true);
		}
	}
}

using UnityEngine;
using System.Collections;

public class nightCheck : MonoBehaviour {

	public float range01;
	public float range01gamma = 0.1f;

	private Transform refObject;
	private SpriteRenderer sr;
	private Color originalColor;

	private float worldSizeX = 105;

	private bool dayNightOff = false;

	void Start() {
		refObject = GameObject.Find ("Night0.1Ref").transform;
		sr = GetComponent<SpriteRenderer> ();
		sr.color = originalColor;
		range01 = range01 * 1.28f;
	}

	void Update() {
		float dif = Mathf.Abs (refObject.position.x - transform.position.x);
		float alpha = Mathf.Sin (dif * Mathf.PI / worldSizeX);
		if (dayNightOff) {
			alpha = alpha * 0.9f;
		} 
		sr.color = new Color (255, 255, 255, alpha);

	}

	void FixedUpdate() {

		RaycastHit hit;
		if (Physics.Raycast (transform.position, new Vector3 (0, 0, 1), out hit, 35f)) {
			dayNightOff = true;
		} else {
			dayNightOff = false;
		}

	}

}

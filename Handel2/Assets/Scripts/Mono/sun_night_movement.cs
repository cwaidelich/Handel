using UnityEngine;
using System.Collections;

public class sun_night_movement : MonoBehaviour {

	[Range(0.2f,1f)]
	public float speed;

	void Start() {
		StartCoroutine ("moveSun");
	}

	IEnumerator moveSun() {
		while (true) {
			transform.Translate (Vector2.left * Time.deltaTime*speed);

			if (transform.position.x < -50) {
				transform.position = new Vector3(50, transform.position.y);
			}
			yield return null;
		}
	}
}

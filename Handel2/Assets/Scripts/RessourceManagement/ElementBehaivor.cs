using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ElementBehaivor : MonoBehaviour {

	[SerializeField]
	private float amount;
	[SerializeField]
	private Element element;

	private SpriteRenderer sr;
	private Sprite[] sprites;

	void Start() {
		sr = GetComponent<SpriteRenderer> ();
	}

	void Update() {
		sprites = element.Sprite;
		if (amount <= 1)
			sr.sprite = sprites [0];
		else if (amount > 1 && amount <= 10)
			sr.sprite = sprites [1];
		else
			sr.sprite = sprites [2];
	}

	public Element Element {
		set {
			element = value;
		}
	}

	private int ElementKey {
		get {
			return element.Key;
		}
	}

	public float Amount {
		set {
			amount = value;
		}
	}

}

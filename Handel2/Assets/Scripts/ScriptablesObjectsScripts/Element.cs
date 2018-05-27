using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="Handel/Element")]
public class Element:ScriptableObject {

	public string elementName;
	public float inventorySlot;
	[SerializeField]
	private int key;
	[SerializeField]
	private string elementKuerzel;

	[SerializeField]
	private Sprite[] elementSprite;

	/* Ideen für später:
	private int elektronenAnzahl;
	private int atomareMasse;
	private int elementArt;
	*/


	public int Key {
		get {
			return key;
		}
	}

	public string Shorty {
		get {
			return elementKuerzel;
		}
	}

	public Sprite[] Sprite {
		get {
			return elementSprite;
		}
	}
}

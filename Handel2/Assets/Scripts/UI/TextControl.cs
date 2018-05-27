using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextControl : MonoBehaviour {

	public Text InformationBar;
	public Text InformationText;

	public float fadeVelocity = 1f;
	void Start () {
			
	}
	// Update is called once per frame
	void Update () {
		

//		InformationBar.text = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
		InformationBar.text = "H: " + StatsContainer.GetValueF ("ELEMENT_AMOUNT", 1);
		InformationBar.text += ", H*: " + StatsContainer.GetValueF ("ELEMENT_AMOUNT", 3);
		InformationBar.text += ", Speed: " + Time.timeScale.ToString();

	}

	public void UpdateInformationText(string _text, Color _color) {
		StartCoroutine(showText(_text,_color));

	}

	IEnumerator showText(string _text, Color _color) {
		// hier noch eine Fade In Funktion
		InformationText.color = _color;
		InformationText.text = _text;
		yield return new WaitForSeconds (3);
		InformationText.color = new Color(_color.r,_color.g,_color.b,0.3f);

	}

}

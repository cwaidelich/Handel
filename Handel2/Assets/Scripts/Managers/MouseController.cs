using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {

	public Camera cam;
	public GameObject selecterIndicator;
	[SerializeField]
	private Transform BuildingCountainer;

	// wird von Generic UI Controller abgehört
	public static event Action<Vector3,GameObject,string> showGui;
	public static event Action guiOpen; //informs the World, that a GUI is open 
	public static event Action closeAllGui;

	private List<GameObject> selecterIndicatorList = new List<GameObject>();
	private int countUIActive;
	private Vector3 firstPosition, secondPosition;
	private string selectedTag = null;
	bool drawRect;


	// Update is called once per frame
	void Update () {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			//print (hit.collider.tag);
			//print (selectedTag);
			if (countUIActive > 0 && Input.GetMouseButtonDown (0) && !Input.GetKey (KeyCode.LeftShift)) {
				// prüft ob eine UI zwischen Camera und hit ist. 
				// Führt die Aktion nicht aus, da die Optionen in GUI bearbeitet werden müssen
				DeactivateGuis ();
			} else if (hit.collider.tag == "Buildings" && (selectedTag == null || selectedTag == "Buildings")) {
				if (Input.GetMouseButtonDown (0) && hit.collider.GetComponentInParent<BuildingBehaivor3> ().ReadyToShowUI) {
					selectedTag = "Buildings";
					ActivateGui (hit.collider.transform.position, hit.collider.gameObject, hit.collider.GetComponentInParent<BuildingBehaivor3> ().BuildingName);
				}
			} else if (hit.collider.tag == "Einwohner" && (selectedTag == null || selectedTag == "Einwohner")) {
				if (Input.GetMouseButtonDown (0)) {
					selectedTag = "Einwohner";
					ActivateGui (hit.collider.transform.position, hit.collider.gameObject, "Einwohner");
				}
			} else if (hit.collider.tag == "Streets" && hit.collider.GetComponentInParent<StreetBehaivor> ().isPlaced && (selectedTag == null || selectedTag == "Streets")) {
				if (Input.GetMouseButtonDown (0)) {
					selectedTag = "Streets";

					ActivateGui (hit.collider.transform.position, hit.collider.gameObject, "Street");
				}
			} else if (Input.GetKey (KeyCode.LeftControl) && Input.GetMouseButtonDown( 0 )) {
				firstPosition = Input.mousePosition;
				drawRect = true;
			} 

			if (Input.GetMouseButtonUp( 0 )) {
				drawRect = false;
			}



		} else {
			if (countUIActive > 0 && Input.GetMouseButtonDown (0)) {
				// prüft ob eine UI zwischen Camera und hit ist. 
				// Führt die Aktion nicht aus, da die Optionen in GUI bearbeitet werden müssen
				DeactivateGuis();
			}
		}

		if (drawRect)
			CheckForRectInside (BuildingCountainer);

	}

	void CheckForRectInside(Transform _container) {
		for (int i = 0; i < _container.childCount; i++) {
			GameObject _go = _container.transform.GetChild (i).gameObject;
			if (IsWithinSelectionBounds (_go) )
				ActivateGui (_go.transform.position, _go, _go.name);

		}
	}

	void ActivateGui(Vector3 _position,GameObject _go,string _name) {
		if (showGui != null) {
			showGui (_position, _go, _name);
			if (guiOpen != null)
				guiOpen ();
			countUIActive++;
			GameObject selectInd = (GameObject)Instantiate (selecterIndicator, _go.transform);
			selectInd.transform.localPosition = Vector3.zero;
			selectInd.transform.localScale = _go.GetComponent<BoxCollider> ().bounds.size * 0.8f;
			selecterIndicatorList.Add (selectInd);
			if (selecterIndicatorList.FindAll (x => x.transform.parent == _go.transform).ToArray ().Length > 1) {
				selecterIndicatorList.Remove (selectInd);
				GameObject.Destroy (selectInd);
			}
			Debug.Log (selecterIndicator.activeSelf);
		}
	}



	void DeactivateGuis() {
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			if (closeAllGui != null) {
				closeAllGui ();
				firstPosition = Vector3.zero;
				countUIActive = 0;
				selectedTag = null;

				foreach (GameObject _sI in selecterIndicatorList) {
					GameObject.Destroy (_sI);
				}
				selecterIndicatorList.Clear ();
			}
		}

	}

	public bool IsWithinSelectionBounds( GameObject gameObject )
	{
		
		Camera camera = Camera.main;
		Bounds viewportBounds =
			MouseUtils.GetViewportBounds( camera, firstPosition, Input.mousePosition );
		
		return viewportBounds.Contains(
			camera.WorldToViewportPoint( gameObject.transform.position ) );
	}

	void OnDrawGizmos() {

		if (drawRect) {
			Gizmos.color = Color.white;
			Camera camera = Camera.main;
			Bounds viewportBounds =	MouseUtils.GetViewportBounds( camera, firstPosition, Input.mousePosition );
			Debug.Log (viewportBounds.ToString()+" Gizmo");
			Gizmos.DrawWireCube (viewportBounds.center,viewportBounds.size);
			for (int i = 0; i < BuildingCountainer.childCount; i++) {
				GameObject _go = BuildingCountainer.transform.GetChild (i).gameObject;
				if (IsWithinSelectionBounds (_go))
					Gizmos.color = Color.blue;
				else
					Gizmos.color = Color.white;
				Gizmos.DrawSphere (camera.WorldToViewportPoint (_go.transform.position), 0.01f);
			}
		}
	}

	void OnGUI()
	{
		if (drawRect) {
			// Create a rect from both mouse positions
			var rect = MouseUtils.GetScreenRect (firstPosition, Input.mousePosition);
			MouseUtils.DrawScreenRect (rect, new Color (0.8f, 0.8f, 0.95f, 0.25f));
			MouseUtils.DrawScreenRectBorder (rect, 2, new Color (0.8f, 0.8f, 0.95f));
		}
	}
}

//http://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/
public static class MouseUtils {

	static Texture2D _whiteTexture;
	public static Texture2D WhiteTexture
	{
		get
		{
			if( _whiteTexture == null )
			{
				_whiteTexture = new Texture2D( 1, 1 );
				_whiteTexture.SetPixel( 0, 0, Color.white );
				_whiteTexture.Apply();
			}

			return _whiteTexture;
		}
	}

	public static void DrawScreenRect( Rect rect, Color color )
	{
		GUI.color = color;
		GUI.DrawTexture( rect, WhiteTexture );
		GUI.color = Color.white;
	}

	public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )
	{
		// Top
		MouseUtils.DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
		// Left
		MouseUtils.DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
		// Right
		MouseUtils.DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
		// Bottom
		MouseUtils.DrawScreenRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
	}

	public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		// Move origin from bottom left to top left
		screenPosition1.y = Screen.height - screenPosition1.y;
		screenPosition2.y = Screen.height - screenPosition2.y;
		// Calculate corners
		var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
		var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );
		// Create Rect
		return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
	}

	public static Bounds GetViewportBounds( Camera camera, Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		var v1 = Camera.main.ScreenToViewportPoint( screenPosition1 );
		var v2 = Camera.main.ScreenToViewportPoint( screenPosition2 );
		var min = Vector3.Min( v1, v2 );
		var max = Vector3.Max( v1, v2 );
		min.z = camera.nearClipPlane;
		max.z = camera.farClipPlane;

		var bounds = new Bounds();
		bounds.SetMinMax( min, max );
		return bounds;
	}

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingBehaivor : MonoBehaviour {

	//Kümmert sich um die ganze Bewegungslogig der Gebäude...
	// Definirt ein BuildingElement, was die Funktion des Gebäudes kontrolliert.


	public int Buildtype;

	public LayerMask planetenMaske;
	public GameObject Einwohner;

	private UIControl ui;

	public float rotationSpeed  = 5f;
	public bool canRotate;
	public float fahneRadius = 5f;

	private bool isSelected;
	private bool isPlaced; // finaller ablage
	private bool isTargetSetVar;

	private Transform planet;



	// Use this for initialization
	void Start () {
		print (Buildtype + " BuildType");
		isSelected = true;

		planet = GameObject.Find ("Planet").transform;
		ui = GameObject.Find ("UIControl").GetComponent<UIControl> ();



	}
	
	// Update is called once per frame
	void Update () {

		if (isSelected) {
			isSelectedBehaivor ();
		}
		if (isPlaced) {
			StartCoroutine (wait (0.2f));			
		}


			
	}

	IEnumerator wait(float sec){
		yield return new WaitForSeconds (sec);
		isPlacedBehavor ();

	}




	private void isPlacedBehavor() {
		if (isTargetSet) {
		// hier müssen PlayData Daten aktualisiert werden, je nach Typ des Gebäudes
		}

	}

	void isSelectedBehaivor() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		print (transform);

		transform.LookAt (planet); //transform.localPosition.x

		//transform.rotation = Quaternion.LookRotation(transform.position - planet.position,Vector3.back);

		if (Physics.Raycast (ray, out hitInfo,500f,planetenMaske) && transform != null) {
			//print (hitInfo.collider.gameObject.name);
			transform.gameObject.SetActive (true);
			transform.position = hitInfo.point;

			if (Input.GetMouseButton (0)) {

				transform.position = hitInfo.point;
				//transform.rotation = transform.LookAt;
				isSelected = false;
				isPlaced = true;

			}
			CancelBuildingAction (Input.GetMouseButton (1), this.gameObject);

		}
	}

	private void CancelBuildingAction(bool cancelInput, GameObject toDestroy) {
		if (cancelInput) {
			print ("Cancel Action");
			Destroy (toDestroy);
			isSelected = false;
			isPlaced = false;
		}
	}

	public bool isTargetSet{
		get { return isTargetSetVar;}
		set { isTargetSetVar = value; }
	}







}

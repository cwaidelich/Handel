using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(Inventory))]

public class BuildingBehaivor3 : MonoBehaviour {

	private enum State {selected, colliding, idle, requesting, constructing, producing, stopped}

	private State currentState;
//	private State freezeState;

	private bool isBuild; //unterscheidet, ob die Request für eigene Bauen oder für die Produktion sind

	public LayerMask planetenMaske;
	public LayerMask buildingMaske;

	private Grid grid;
	private Inventory inventory;

	private GameObject thisPrefab;
	public GameObject buildUI, produceUI, warningUI; //Icon was beim Bauen erscheint...
	GameObject warningIndicator = null;

	// Flag = Ort wor der Einwohner kontakt mit dem Gebäude hat
	public GameObject flagPrefab;
	private GameObject flagPostition;
	private bool flagPositionSet;

	public Node placedNode;

	//wird von StasContainer aufgerufen
	public static event Action<int, string> buildingPlaceChange;

	//wird von WorkOfferManager aufgerufen
	public static event Action<GameObject,Vector3,RessourcesController,float> CreateWorkOrder; 
	public static event Func<GameObject,RessourcesController, bool> CheckStatusOfWorkOrder; 
	public static event Action<GameObject> DeactivateAllGiveWorkOrder; 
	public static event Action<GameObject, RessourcesController> DeactivateRunningWorkOrder; 
	public static event Action<GameObject> ResetErrorQueue; 

	private bool currentWorkOrderIsError = false;

	private RessourcesController[] rcGive;

	public BuildingStats buildingStats;

	private Animator animator; // muss im BuildingStat vorhanden sein

	//debug
	string requestedElementName;


	void Awake() {
		thisPrefab = this.gameObject;
	}

	void Start () {

		currentState = State.selected;
		grid = GameObject.Find ("A*").GetComponent<Grid> ();

		inventory = GetComponent<Inventory> ();

		animator = GetComponent<Animator>();
		transform.gameObject.SetActive (true);
		gameObject.name = buildingStats.buildingName;

		if (transform.Find ("Flag") == null) {
			flagPostition = (GameObject)Instantiate (flagPrefab, transform);
			flagPostition.transform.position = new Vector3 (0, 0, -5);
			flagPostition.name = "Flag";
			flagPostition.SetActive (false);
			flagPostition.GetComponent<SpriteRenderer> ().sortingOrder = 1;
		} else {
			flagPostition = this.transform.Find ("Flag").gameObject;
		}

		flagPositionSet = false;

		isBuild = false;

		if (buildingStats.hasProduction) {
			rcGive = new RessourcesController[buildingStats.productionOrder.rc_Out.Length];

			for (int i = 0; i < rcGive.Length; i++) {
				rcGive [i] = new RessourcesController(0,buildingStats.productionOrder.rc_Out[i].Element,RessourcesController.workOrderType.Give_Ressource);
			}

		}

		WorkOrderManager.CommunicationWorkOrderInError += CommunicationWorkOrderInError;

	}

	// Update is called once per frame
	void Update () {


		if (currentState == State.selected || currentState == State.colliding) {
			isNotPlaced ();
		} else {
			// Sobald das Gebäude im Bildschirm ausgewählt ist, wird dieser Teil aufgerufen.
			// Placed Verhalten
			InventoryManagement ();
			if (currentState == State.stopped || !ReadyToRequest || currentWorkOrderIsError) {
				if (warningIndicator == null) {
					warningIndicator = (GameObject) Instantiate (warningUI, transform);
					warningIndicator.transform.localScale = GetComponent<BoxCollider> ().size*0.8f;
					warningIndicator.transform.localPosition = new Vector3(0,0,-0.001f);
				}
				ResetErrorQueue (gameObject);
				if (currentState != State.stopped)
					currentState = State.stopped;
				UnrequestElements (buildingStats.rc_toBuild);
				if (buildingStats.hasProduction) {
					UnrequestElements (buildingStats.productionOrder.rc_In);
				}
			} else {
				if (warningIndicator != null)
					GameObject.Destroy (warningIndicator);

				if (!isBuild) {
					//Gebäude ist plaziert, aber nicht gebaut
					if (CheckInvForElements (buildingStats.rc_toBuild) && currentState != State.constructing) {
						//startet sich selber zu bauen
						UnrequestElements (buildingStats.rc_toBuild);
						if (currentState != State.constructing)
							StartCoroutine ("startConstruction");
						currentState = State.constructing;
					} else {
						foreach (RessourcesController _rc in buildingStats.rc_toBuild) {
							if (!CheckInvForElement (_rc)) {
								currentState = State.requesting;
								RequestElements (buildingStats.rc_toBuild);
							}
						}
					}
				} else if (isBuild && buildingStats.hasProduction) {
					//Gebäude ist gebaut, kann jetzt es produzieren
					if (inventory.Count () < inventory.Max) {
						if (CheckInvForElements (buildingStats.productionOrder.rc_In) && currentState != State.producing) {
							//startet zu produzieren
							UnrequestElements (buildingStats.productionOrder.rc_In);
							if (currentState != State.producing)
								StartCoroutine ("startProduction");
							currentState = State.producing;
							
						} else {
							foreach (RessourcesController _rc in buildingStats.productionOrder.rc_In) {
								if (!CheckInvForElement (_rc)) {
									currentState = State.requesting;
									RequestElements (buildingStats.productionOrder.rc_In);
								}
							}
						}
					} else
						Debug.Log ("Inventory Full");
				} else if (isBuild && !buildingStats.hasProduction) {
					flagPostition.SetActive (false);
					transform.position = new Vector3 (transform.position.x, transform.position.y, -4);
				}
			}
		}

//		Debug.Log(currentState.ToString());
	}


	void OnDestroy() {
		WorkOrderManager.CommunicationWorkOrderInError -= CommunicationWorkOrderInError;
	}

	// Verhalten, wenn Gebäude nicht im Bildschirm plaziert ist.
	void isNotPlaced() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo, 50f, planetenMaske) && transform != null) {

			transform.position = snapToGrid (hitInfo.collider.transform.position);
			if (Input.GetKeyDown (KeyCode.R)) {
				transform.Rotate (new Vector3 (0, 0, 90));
			}
			if (currentState == State.colliding) {

				print ("cannot be placed");
			}

			if (Input.GetMouseButton (1)) {
				GameObject.Destroy (gameObject);
			}

			if (Input.GetMouseButton (0) && currentState == State.selected) {

				transform.position = snapToGrid (hitInfo.collider.transform.position);
				placedNode = grid.NodeFromWorldPoint (hitInfo.collider.transform.position);
				currentState = State.idle;
				animator.SetBool ("isPlaced", true);
				grid.DefineGridContent (grid.Range (placedNode, buildingStats.buildingSizeWeight, buildingStats.buildingSizeHeight).ToArray (), buildingStats.content.ToString ());
				if (!buildingStats.hasProduction)
					setFlag ("self", 0);

				if (Input.GetKey (KeyCode.LeftShift))
					Instantiate (thisPrefab, transform.parent);
			} 
		}
	}


	private void InventoryManagement() {
		inventory.ShowSlot ();
		if (isBuild && buildingStats.hasProduction) {
			for (int i = 0; i < buildingStats.productionOrder.rc_Out.Length; i++) {
				RessourcesController _rc = buildingStats.productionOrder.rc_Out [i];
				if (inventory.Count (_rc.Element) > 0 && rcGive [i].TotalAmount != inventory.Count (_rc.Element)) {
					if (DeactivateAllGiveWorkOrder != null)
						DeactivateAllGiveWorkOrder (gameObject);
					
					rcGive [i] = new RessourcesController (inventory.Count (_rc.Element), _rc.Element, RessourcesController.workOrderType.Give_Ressource);
					if (CreateWorkOrder != null)
						CreateWorkOrder (gameObject, flagPostition.transform.position, rcGive [i], inventory.Count (rcGive [i].Element));
				} else if (inventory.Count (_rc.Element) <= 0) {
					DeactivateAllGiveWorkOrder (gameObject);
				}

			}
		}
	}


	void RequestElements(RessourcesController[] _rcArray) {
		foreach (RessourcesController _rc in _rcArray) {
			bool doneRequest = false;
			if (CheckStatusOfWorkOrder != null) {
				doneRequest = CheckStatusOfWorkOrder (gameObject, _rc);
				//print ("doneRequest: " + doneRequest);
			}
			if (CreateWorkOrder != null && !doneRequest) {
				//wird von WorkOrderManager aufgerufen
				float calculatedAmount = _rc.TotalAmount-inventory.Count(_rc.Element);
				if (_rc.Type == RessourcesController.workOrderType.Give_Ressource.ToString())
					calculatedAmount = inventory.Count (_rc.Element);
				CreateWorkOrder (gameObject , flagPostition.transform.position, _rc, calculatedAmount);
				requestedElementName = _rc.Element.Shorty + " (" + calculatedAmount+") "+_rc.TypeShorty; //debug
			}
		}
	}


	void UnrequestElements(RessourcesController[] _rcArray) {
		if (DeactivateRunningWorkOrder != null) {
			foreach (RessourcesController _rc in _rcArray) {
				DeactivateRunningWorkOrder (gameObject, _rc);
			}
		}
	}
	
	IEnumerator startProduction() {
		float produceStatus = 0;
		GameObject produceIcon = (GameObject) Instantiate (produceUI, transform, false);
		produceIcon.transform.localPosition = new Vector3 (3, -3, -5);
		foreach (RessourcesController _rc in buildingStats.productionOrder.rc_In) {
			inventory.Increment (_rc.Element, -_rc.TotalAmount, false);
		}
		while (produceStatus < buildingStats.productionOrder.produceTimeInSeconds) {
			produceStatus += buildingStats.productionOrder.produceVelocityInSeconds;
			print ("produce Status: " + produceStatus / buildingStats.productionOrder.produceTimeInSeconds);
			yield return new WaitForSeconds(buildingStats.productionOrder.produceVelocityInSeconds);
		}
		foreach (RessourcesController _rc in buildingStats.productionOrder.rc_Out) {
			inventory.Increment (_rc.Element, +_rc.TotalAmount, true);
		}

		GameObject.Destroy (produceIcon);
		//RequestElements (buildingStats.productionOrder.rc_Out);
		yield return null;
	}

	IEnumerator startConstruction() {

		float buildStatus = 0;
		GameObject buildIcon = (GameObject) Instantiate (buildUI, transform, false);
		buildIcon.transform.localPosition = new Vector3 (3, -3, -5);

		while (buildStatus < buildingStats.buildTimeInSeconds) {
			buildStatus += buildingStats.buildVelocityInSeconds;
			print ("constructionStatus: " + buildStatus / buildingStats.buildTimeInSeconds);
			animator.SetFloat ("constructionStatus", buildStatus / buildingStats.buildTimeInSeconds);
			yield return new WaitForSeconds(buildingStats.buildVelocityInSeconds);
		}
		foreach (RessourcesController _rc in buildingStats.rc_toBuild) {
			inventory.Increment (_rc.Element, -_rc.TotalAmount, false);
		}
		GameObject.Destroy (buildIcon);
		isBuild = true;

		//Informiert General Stats
		if (buildingPlaceChange != null) {
			buildingPlaceChange (buildingStats.buildingID,"BUILDING_PLACED");
		}
	}

	// wenn alle Elemente im Inv sind, gibt es true zurück
	bool CheckInvForElements(RessourcesController[] _rcArray) {
		bool returnVal = true;
		foreach (RessourcesController _rc in _rcArray) {
			if (inventory.Count (_rc.Element) < _rc.TotalAmount)
				returnVal = false;
		}
		return returnVal;
	}

	bool CheckInvForElement(RessourcesController _rc) {
		if (inventory.Count (_rc.Element) < _rc.TotalAmount)
			return false;
		return true;
	}


	private Vector3 snapToGrid(Vector3 _mouse) {
		Node selectedNode = grid.NodeFromWorldPoint (_mouse);

		if (grid.IsObjectColliding (grid.Range(selectedNode, buildingStats.buildingSizeWeight, buildingStats.buildingSizeHeight).ToArray(),"build")) {
			currentState = State.colliding;
			animator.SetBool ("isColliding", true);
	 	} else {
			currentState = State.selected;
			animator.SetBool ("isColliding", false);
		}
		return selectedNode.getWorldCoorForSnap (buildingStats.buildingSizeWeight, buildingStats.buildingSizeHeight,-5);
	}


	public string CurrentState { 
		get {
			string returnVal;
			switch (currentState) {
			case State.requesting:
				returnVal = "Request:"+requestedElementName;
				break;
			case State.colliding:
				returnVal = "Colliding";
				break;
			case State.constructing:
				returnVal = "Constructing";
				break;
			case State.producing:
				returnVal = "Productive";
				break;
			case State.selected:
				returnVal = "Selected";
				break;			
			case State.stopped:
			case State.idle:
				returnVal = "Warning!";
				break;
			default:
				returnVal = "unknown";
				Debug.LogError ("Unknown Current State: "+currentState);
				break;
			}
			return returnVal;
		}
	}

	public bool ReadyToShowUI {
		get {
			if (currentState == State.selected || currentState == State.colliding)
				return false;
			return true;
		}
	}

	public bool ReadyToRequest {
		get {
			if (!flagPositionSet || currentState == State.colliding || currentState == State.selected || currentState == State.stopped)
				return false;
			return true;
		}
	}

	public bool ActiveBuilding {
		set {
			if (currentState == State.stopped) {
				currentState = State.idle;
				currentWorkOrderIsError = false;
			} else
				currentState = State.stopped;
		}
		get {
			if (currentState == State.stopped)
				return false;
			else
				return true;

		}
	}

	void CommunicationWorkOrderInError(GameObject _go, RessourcesController _rc) {
		if (_go == null)
			Debug.LogError ("Go == null");
		Debug.LogWarning ("go:" + _go.name);
		Debug.LogWarning ("GameObject:" + gameObject.name);
		if (gameObject != null && _go != null) {
			if (_go == gameObject)
				currentWorkOrderIsError = true;
			else
				currentWorkOrderIsError = false;
		}
	}

	// **** Wird von der UI aufgerufen ****

	public void setFlag(string _direction, int _alternativeOffset) {
		int oP = _alternativeOffset; //alternative Position wird kalkuliert
		Node flagNode;
		switch (_direction) {
		case "FlagNord":
			flagNode = grid.getNodeOffset (placedNode, -2, 0 - oP);	//Norden
			break;
		case "FlagSouth":
			flagNode = grid.getNodeOffset (placedNode, 1, 0-oP);	//Süden
			break;
		case "FlagWest":
			flagNode = grid.getNodeOffset (placedNode, -1+oP, -2);	//Westen
			break;
		case "FlagEast":
			flagNode = grid.getNodeOffset (placedNode, 0-oP, 1);	//Osten
			break;
		case "self":
			flagNode = grid.getNodeOffset (placedNode, 0, 0);	//transform.position
			break;
		default:
			flagNode = grid.getNodeOffset (placedNode, 0, 0);	//error
			break;
		}
		flagPostition.SetActive (true);
		Vector3 nodePosition = flagNode.getWorldCoor ();
		flagPostition.transform.position = new Vector3 (nodePosition.x, nodePosition.y, -5f);
		//flagPostition.transform.localPosition = new Vector3 (0,0, -0.001f);
		flagPositionSet = true;

	}

	public string BuildingName {
		get {
			return buildingStats.buildingName;
		}
	}

	public bool IsBuild {
		get {
			return isBuild;
		}
	}


}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Inventory))]

public class EinwohnerBehaivor2 : MonoBehaviour {


	private enum Status	{ idle, searchingRes, workIdle, problem,transport ,arrivedAtLocation};
	[SerializeField]
	private Status currentStatus;

	private Grid theGrid;
	private Unit unit;

	public float batteryStatus; 

	public float timeBetweenRessourceGatherning = 2f;
	public float ressourceGatherTimeFromGround = 2f;

//	public int currentCapacity; 
	public float _elementCapacityMax = 50;

	private Inventory inventory;

	public WorkOrder currentWorkOrder; //sollte private sein, zum debug auf public. s. UIEInwohnerControl
	//wird von WorkOfferManager aufgerufen
	public static event Func<GameObject,WorkOrder> sucheArbeit; 
	public static event Action<WorkOrder,GameObject> ReserveWorkOrder; 
	public static event Func<Element,string,WorkOrder> SearchForWorkOrder;
	public static event Action<GameObject,Vector3,RessourcesController,float> CreateDigWorkOrder; 
	public static event Func<GameObject,float, Element, bool> CheckStatusOfWorkOrder; 
	public static event Action<GameObject,RessourcesController> DeactivateRunningWorkOrder; 

	// Use this for initialization
	void Start () {
		theGrid = GameObject.Find ("A*").GetComponent<Grid> ();
		unit = GetComponent<Unit> ();
		batteryStatus = 1;

		inventory = GetComponent<Inventory> ();

		StartCoroutine("BatteryLifeCycle");
		StartCoroutine("OwnBehaivor");
	}

	void Update () {
		inventoryManagement ();

	}


	IEnumerator OwnBehaivor() {
		while (true) {
			if (batteryStatus > 0) {

				if (currentStatus == Status.idle) {
					if (sucheArbeit != null) {
						currentWorkOrder = sucheArbeit (gameObject);
						if (!currentWorkOrder.IsNull())
							currentStatus = Status.workIdle;
					}
				} else {
					print ("current WorkOrder: " + currentWorkOrder.WorkOrderType);
					if (currentWorkOrder.WorkOrderType == "Need_Ressource") {
						// Prüft, ob er im eigenen Inv die Ressourcen hat. Falls ja, geht er zum Platz und hinterlässt sein kompletten Inv.
						// Falls nein, reserviert er den jetzigen Workorder
						if (inventory.Count(currentWorkOrder.resController.Element) <= 0) {
							//Debug.LogError ("carring unrequested inv - deleting everything - debug");
							DroppingElementInGround ();
						}

						if (inventory.Count (currentWorkOrder.resController.Element) > 0) {
							print ("Inventory über 0");
							if (currentStatus != Status.transport) {
								currentStatus = Status.transport;
								transportToLocation (currentWorkOrder.getLocationOfWork);
							}

							if (currentStatus != Status.arrivedAtLocation)
								TransferElementsWithBuilding (currentWorkOrder.WorkOrderType);
						} else if (inventory.Count (currentWorkOrder.resController.Element) <= 0) {
							print ("Inventory unter 0");
							ReserveWorkOrder (currentWorkOrder, gameObject);
							WorkOrder giveWorkOrder = SearchForWorkOrder (currentWorkOrder.resController.Element, "Give_Ressource");
							if (giveWorkOrder != null) {
								currentWorkOrder = giveWorkOrder;
								currentWorkOrder.status = WorkOrder.workOrderStatus.Running;
							} else {
								RessourcesController rc_Dig = new RessourcesController (currentWorkOrder.Amount, currentWorkOrder.resController.Element, RessourcesController.workOrderType.Dig_Ressource);
								bool doneRequest = false;
								if (CheckStatusOfWorkOrder != null) {
									doneRequest = CheckStatusOfWorkOrder (gameObject, currentWorkOrder.Amount, currentWorkOrder.resController.Element);
									print ("Einwohner doneRequest: " + doneRequest);
								}
								if (CreateDigWorkOrder != null && !doneRequest) {
									//wird von WorkOrderManager aufgerufen

									CreateDigWorkOrder (gameObject, Vector3.zero, rc_Dig, rc_Dig.TotalAmount - inventory.Count (rc_Dig.Element));
									currentStatus = Status.idle;
								}
							} 
						} else {
							Debug.LogError ("error, check");
						}
					} else if (currentWorkOrder.WorkOrderType == "Dig_Ressource") {
						gatheringRessources (currentWorkOrder.resController.Element);
					} else if (currentWorkOrder.WorkOrderType == "Give_Ressource") {
						if (currentStatus != Status.transport) {
							currentStatus = Status.transport;
							transportToLocation (currentWorkOrder.getLocationOfWork);
						}
						if (currentStatus != Status.arrivedAtLocation)
							TransferElementsWithBuilding (currentWorkOrder.WorkOrderType);
					}



				}

			}


			yield return new WaitForSeconds (2);
		}
	}


	void DroppingElementInGround() {
		Node currentNode = theGrid.NodeFromWorldPoint (transform.position);
		foreach (Element _e in inventory.AllElementsinInventory()) {
			inventory.Transfer (_e, inventory.Count (_e), currentNode.inventory, false, true);
		}
	}

	//sucht Element in der ganzen Welt um es in einem Ziel zu lassen
	void gatheringRessources(Element _element) {


		if (currentStatus != Status.searchingRes) {
			currentStatus = Status.searchingRes;
			StartCoroutine( getRessourceDirectFromPlanet (searchForNearestRessourcePoint (4, _element)));
			if (currentStatus == Status.problem)
				Debug.Log ("no additional ressources...");
		}




	}
		

	IEnumerator getRessourceDirectFromPlanet(Vector2[] _workLoc) {

		float elementCapacity;

		if (currentWorkOrder.Amount < inventory.Max)
			elementCapacity = currentWorkOrder.Amount;
		else
			elementCapacity = inventory.Max;


		if (_workLoc.Length == 0) {
			yield return null;
		}
		Node currentNode = theGrid.NodeFromWorldPoint (transform.position);
		print ("Total Needed: " + currentWorkOrder.Amount);

		foreach (var wL in _workLoc) {
			print ("current Capacity: " + inventory.Count(currentWorkOrder.resController.Element));
			if (inventory.Count(currentWorkOrder.resController.Element) >= elementCapacity) {
				break;
			}
	
			transportToLocation (wL);

			while (transform.position != new Vector3 (wL.x, wL.y, -5)) {
				yield return null;
			}

			currentNode = theGrid.NodeFromWorldPoint (transform.position);

			while (inventory.Count(currentWorkOrder.resController.Element) < elementCapacity && currentNode.hasElementRessource(currentWorkOrder.resController.Element)) {
				yield return new WaitForSeconds (2);
				inventory.Increment(currentWorkOrder.resController.Element,1f, false);
				currentNode.reducePrimaryElement(1);
				print ("CCount: " + inventory.Count (currentWorkOrder.resController.Element));
			}
		}
		if (inventory.Count(currentWorkOrder.resController.Element) < elementCapacity) {
			print ("keine Elemente im Planet gefunden");
			DeactivateRunningWorkOrder (gameObject, currentWorkOrder.resController);
			currentStatus = Status.idle;
		} else {
			print ("setting Idle Status");
			DeactivateRunningWorkOrder (gameObject, currentWorkOrder.resController);
			currentStatus = Status.idle;
		}
	}




	bool inTransport = false;
	Vector3 start,end;

	private void transportToLocation(Vector2 target) {
		Vector3 targetV3 = new Vector3 (target.x, target.y, -5);
		start = transform.position;
		end = targetV3;
		inTransport = true;
		Element currentElement;
		currentElement = inventory.GetFirstCarryingElement();
		if (transform.position != targetV3)
			unit.requestPath (transform.position,targetV3,currentElement);
	}

	//debug

	void OnDrawGizmosSelected() {
		if (inTransport) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (start, 0.5f);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere (end, 0.7f);
		}
	}

	void TransferElementsWithBuilding (string _workOrderType) {
		
		//print ("current pos: " + transform.transform.position);
		//print ("workEnergy LocationInfo: " + currentWorkOrder.getLocationOfWork);
		if (_workOrderType == "Need_Ressource") {
			if (transform.position == currentWorkOrder.getLocationOfWork) {
				currentStatus = Status.arrivedAtLocation;
				DeactivateRunningWorkOrder (currentWorkOrder.requestor, currentWorkOrder.resController);
				inventory.Transfer (currentWorkOrder.resController.Element, inventory.Count (currentWorkOrder.resController.Element), currentWorkOrder.requestor.GetComponent<Inventory> (),false,false);
				currentStatus = Status.idle;
				Debug.Log ("TransferElementsWithBuilding | Need_Ressource");
			}
		} else if (_workOrderType == "Give_Ressource") {
			if (transform.position == currentWorkOrder.getLocationOfWork) {
				currentStatus = Status.arrivedAtLocation;
				DeactivateRunningWorkOrder (currentWorkOrder.requestor, currentWorkOrder.resController);
				Inventory originInv = currentWorkOrder.requestor.GetComponent<Inventory> ();
				originInv.Transfer (currentWorkOrder.resController.Element, originInv.Count(currentWorkOrder.resController.Element), inventory,true,false);
				currentStatus = Status.idle;
				Debug.Log ("TransferElementsWithBuilding | Give_Ressource");
			}
		}
	}

	void inventoryManagement() {

		inventory.ShowSlot ();

	}


	IEnumerator BatteryLifeCycle() {
		yield return new WaitForSeconds (10);
		while (true) {
			Node currentNode = theGrid.NodeFromWorldPoint (transform.position);
			bool isNight = currentNode.IsNight;
			if (batteryStatus >= 0) {
				if (!isNight && batteryStatus < 1) {
					batteryStatus += 0.1f;
					if (batteryStatus >= 1)
						batteryStatus = 1;
					Debug.Log ("charging");
				} 
				if (getEneryLevel > 0) {
					//Debug.Log (workEnergy);
					batteryStatus -= getEneryLevel;
					if (batteryStatus <= 0)
						batteryStatus = 0;
					
				} 
			}
			yield return new WaitForSeconds(2);
		}

	}

	private float getEneryLevel {
		get {
			float returnVal;
			switch (currentStatus) {
			case Status.idle:
				returnVal = 0.005f;
				break;
			case Status.workIdle:
				returnVal = 0.005f;
				break;
			case Status.searchingRes:
				returnVal = 0.03f;
				break;
			case Status.arrivedAtLocation:
				returnVal = 0.01f;
				break;
			case Status.transport:
				returnVal = 0.02f;
				break;
			default:
				returnVal = -1f;
				Debug.LogError ("Unknown Current State: "+currentStatus);
				break;
			}

			return returnVal;
		}
	}

	public string CurrentState { 
		get {
			string returnVal;
			switch (currentStatus) {
			case Status.idle:
				returnVal = "Idle";
				break;
			case Status.workIdle:
				returnVal = "Starting to Work";
				break;
			case Status.searchingRes:
				returnVal = "Searching for Ressources";
				break;
			case Status.arrivedAtLocation:
				returnVal = "Depositing Elements";
				break;
			case Status.transport:
				returnVal = "Traveling";
				break;
			case Status.problem:
				returnVal = "Problem";
				break;
			default:
				returnVal = "unknown";
				Debug.LogError ("Unknown Current State: "+currentStatus);
				break;
			}

			return returnVal;
		}
	}



	private Vector2[] searchForNearestRessourcePoint(int nodeRadius, Element _element) {
		Node currentNode = theGrid.NodeFromWorldPoint (transform.position);
		Vector2 currentNodeGrid = currentNode.getGridPosition ();
		List<Vector2> returnVal = new List<Vector2> ();

		for (int radiusIndex = 0; radiusIndex < nodeRadius; radiusIndex++) {
			for (int xPos = -radiusIndex; xPos < radiusIndex; xPos++) {
				for (int yPos = -radiusIndex; yPos < radiusIndex; yPos++) {
					int xVal = xPos + Mathf.RoundToInt (currentNodeGrid.x);
					int yVal = yPos + Mathf.RoundToInt (currentNodeGrid.y);
					Node actualNode = theGrid.getNote (xVal, yVal);
					if (theGrid.checkNodeFor (xVal, yVal, _element) == true && !theGrid.IsObjectColliding(theGrid.Range(actualNode,1,1).ToArray(),"build")) {
						Vector2 WorldCoorOfNode = theGrid.returnWorldCoorFromNode (xVal, yVal, false, -5,1,1);
						if(!returnVal.Contains(WorldCoorOfNode))
							returnVal.Add(WorldCoorOfNode);
					}
				}
			}
		}

		return returnVal.ToArray ();
	}




	//debug

	public string TotalElementCount {
		get {
			return inventory.ContentReadable();
		}
	}

}

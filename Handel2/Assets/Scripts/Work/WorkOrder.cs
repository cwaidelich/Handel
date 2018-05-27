using UnityEngine;
using System.Collections;


public class WorkOrder {

	public GameObject requestor;
	public RessourcesController resController;
	private int number;

	public enum workOrderStatus
	{
		Queue,
		Reserved,
		Running,
		Finish,
		Error
	};

	public workOrderStatus status;

	private Vector3 locationOfOrder;
	private string typeOfOrder;
	private string requestorName;
	private bool active;
	private GameObject executer;
	private float amount;
	public int errorCount;

	public WorkOrder (GameObject _requestedGo, Vector3 _locOfOrder, RessourcesController _rC, float _amount, int _num) {
		requestor = _requestedGo;
		locationOfOrder = _locOfOrder;
		resController = _rC;
		typeOfOrder = _rC.Type;
		status = workOrderStatus.Queue;
		number = _num;
		errorCount = 0;
		amount = _amount;
		if (amount <= 0)
			Debug.LogError ("Error amount < 0:"+amount);
	}

	public int Number {
		get {
			return number;
		}
	}

	public void IncreaseErrorCount(int _i) {
		errorCount += _i;
		if (errorCount > 3)
			status = workOrderStatus.Error;
	}

	public GameObject Executer {
		set {
			executer = value;
		}
		get {
			return executer;
		}
	}

	public string ExecuterName {
		get {
			if (executer == null)
				return "undefined";
			return executer.name;
		}
	}

	public WorkOrder (string _null) {
		typeOfOrder = "NULL";
	}

	public bool isInactive {
		get {
			if (status==workOrderStatus.Finish)
				return true;
			return false;
		}
	}

	public void SetInactive() {
		status = workOrderStatus.Finish;
	}

	public bool IsNull() {
		if (typeOfOrder == "NULL") {
			return true;
		}
		return false;
	}

	public string WorkOrderType {
		get {
			return typeOfOrder;
		}
	}

	public Vector3 getLocationOfWork {
		get { return locationOfOrder; }
	}

	public int ErrorCount {
		get {
			return errorCount;
		}
	}

	public float Amount {
		get {
			return amount;
		}
	}

}

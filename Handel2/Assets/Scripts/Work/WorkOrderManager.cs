using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorkOrderManager : MonoBehaviour {

	//wird von Building Behaivor3 aufgerufen
	public static event Action<GameObject,RessourcesController> CommunicationWorkOrderInError; 

	public List<WorkOrder> workOrderQueue = new List<WorkOrder>();
	private int counter = 0;

	// Use this for initialization
	void Start () {
		
		BuildingBehaivor3.CreateWorkOrder += CreateWorkOrder;
		BuildingBehaivor3.CheckStatusOfWorkOrder += CheckWorkOrder;
		BuildingBehaivor3.DeactivateAllGiveWorkOrder += DeactivateAllGiveWorkOrder;
		BuildingBehaivor3.DeactivateRunningWorkOrder += DeactivateRunningWorkOrder;
		BuildingBehaivor3.ResetErrorQueue += ResetErrorQueue;

		EinwohnerBehaivor2.ReserveWorkOrder += ReserveWorkOrder;
		EinwohnerBehaivor2.sucheArbeit += EinwohnerBehaivor2_sucheArbeit;
		EinwohnerBehaivor2.SearchForWorkOrder += SearchForWorkOrder;
		EinwohnerBehaivor2.CreateDigWorkOrder += CreateWorkOrder;
		EinwohnerBehaivor2.CheckStatusOfWorkOrder += CheckWorkOrderByElement;
		EinwohnerBehaivor2.DeactivateRunningWorkOrder += DeactivateRunningWorkOrder;
	}

	WorkOrder EinwohnerBehaivor2_sucheArbeit (GameObject _exec)
	{
		if (workOrderQueue.Count > 0) {
			WorkOrder nextWorkOrder;
			nextWorkOrder = workOrderQueue.Find (w => w.requestor == _exec && (w.status != WorkOrder.workOrderStatus.Finish && w.status != WorkOrder.workOrderStatus.Error));
			print ("found WorkOrder for Requestor");

			if (nextWorkOrder == null) {
				nextWorkOrder = workOrderQueue.Find (w => w.status == WorkOrder.workOrderStatus.Reserved && w.Executer == _exec);
				print ("found WorkOrder in Reserve");
			}

			if (nextWorkOrder == null) {
				nextWorkOrder = workOrderQueue.Find (w => w.status == WorkOrder.workOrderStatus.Queue &&
					(w.resController.Type == RessourcesController.workOrderType.Need_Ressource.ToString() || w.resController.Type == RessourcesController.workOrderType.Dig_Ressource.ToString()));
				print ("found WorkOrder in Queue");

			}


			if (nextWorkOrder != null) {
				if (nextWorkOrder.ErrorCount > 3) {
					nextWorkOrder.status = WorkOrder.workOrderStatus.Error;
					if (CommunicationWorkOrderInError!=null)
						CommunicationWorkOrderInError (nextWorkOrder.requestor, nextWorkOrder.resController);
				}
				else {
					nextWorkOrder.status = WorkOrder.workOrderStatus.Running;
					nextWorkOrder.IncreaseErrorCount (1);
					return nextWorkOrder;
				}
			}
		}
		print ("did not find a WorkOrder in Queue");
		return new WorkOrder("null");
	}
	

	void CreateWorkOrder(GameObject _go, Vector3 _locationOfWork, RessourcesController _rs, float _missingAmount) {
		print ("creating Workorder");
		workOrderQueue.Add(new WorkOrder(_go, _locationOfWork, _rs, _missingAmount, counter));
		counter++;
	}

	bool CheckWorkOrderByElement(GameObject _go, float _amount, Element _element) {
		WorkOrder workQ = workOrderQueue.Find (w => w.requestor == _go && w.Amount == _amount && w.resController.Element == _element &&
			(w.status == WorkOrder.workOrderStatus.Queue || w.status == WorkOrder.workOrderStatus.Running|| w.status == WorkOrder.workOrderStatus.Reserved));
		if (workQ != null)
			return true;
		return false;
	}

	bool CheckWorkOrder(GameObject _go, RessourcesController _rs) {
		WorkOrder workQ = workOrderQueue.Find (w => w.resController == _rs && w.requestor == _go &&
		                  (w.status == WorkOrder.workOrderStatus.Queue || w.status == WorkOrder.workOrderStatus.Running 
				|| w.status == WorkOrder.workOrderStatus.Reserved || w.status == WorkOrder.workOrderStatus.Error ));
		if (workQ != null)
			return true;
		return false;
	}

	void DeactivateAllGiveWorkOrder (GameObject _go) {
		foreach (WorkOrder _wo in workOrderQueue.FindAll(w => w.requestor == _go && w.status == WorkOrder.workOrderStatus.Queue && w.resController.Type == RessourcesController.workOrderType.Give_Ressource.ToString())) {
			_wo.SetInactive ();
		}
	}

	void DeactivateRunningWorkOrder(GameObject _go, RessourcesController _rs) {
		WorkOrder work = workOrderQueue.Find (w => w.resController == _rs && w.requestor == _go && 
			(w.status == WorkOrder.workOrderStatus.Running || w.status == WorkOrder.workOrderStatus.Reserved));
		if (work != null) {
			work.SetInactive ();
			//workOrderQueue.Remove (work);
		}
	}

	void DeactivateRunningWorkOrderByElement(GameObject _go, float _amount, Element _element) {
		WorkOrder work = workOrderQueue.Find (w => w.requestor == _go && w.Amount == _amount && w.resController.Element == _element 
			&& (w.status == WorkOrder.workOrderStatus.Running || w.status == WorkOrder.workOrderStatus.Reserved));
		work.SetInactive ();
		//workOrderQueue.Remove (work);
	}



	void FinishRunningWorkOrder(WorkOrder _work) {
		if (_work != null) {
			_work.SetInactive();
			//workOrderQueue.Remove (_work);
		}
	}

	void ReserveWorkOrder (WorkOrder _work, GameObject _executer ) {
		if (_work != null) {
			print ("Workorder steht auf reserve");
			_work.status = WorkOrder.workOrderStatus.Reserved;
			_work.Executer = _executer;
		}
	}

	void ResetErrorQueue(GameObject _requestor) {
		List<WorkOrder> errorQueue = new List<WorkOrder>();
		errorQueue = workOrderQueue.FindAll (w => w.requestor == _requestor && w.status == WorkOrder.workOrderStatus.Error);
		foreach (WorkOrder _wo in errorQueue) {
			_wo.status = WorkOrder.workOrderStatus.Queue;
			_wo.errorCount = 0;
		}
	}

	WorkOrder SearchForWorkOrder (Element _e, string _type) {
		WorkOrder work = workOrderQueue.Find (w => w.resController.Element == _e && w.WorkOrderType == _type && w.status == WorkOrder.workOrderStatus.Queue);
		return work;
	}

	// nicht benutzen
	WorkOrder ReservedWorkOrders(GameObject _exec) {
		WorkOrder reserved = workOrderQueue.Find (r => r.Executer == _exec);
		if (reserved != null)
			return reserved;
		else
			return new WorkOrder ("null");
	}



}

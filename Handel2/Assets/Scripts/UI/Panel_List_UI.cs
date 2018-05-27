using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Panel_List_UI : MonoBehaviour {

	public WorkOrder wo;

	[SerializeField]
	private Text nr, who, type, element, amount, status, errorcount;
	
	// Update is called once per frame
	void Update () {
		nr.text = wo.Number.ToString();
		who.text = wo.requestor.name;
		type.text = wo.resController.TypeShorty;
		element.text = wo.resController.Element.Shorty;
		errorcount.text = wo.ErrorCount.ToString();
		amount.text = wo.Amount.ToString();
		status.text = wo.status.ToString();
		switch (wo.status) {
		case WorkOrder.workOrderStatus.Finish:
			status.color = Color.black;
			break;
		case WorkOrder.workOrderStatus.Queue:
			status.color = Color.yellow;
			break;
		case WorkOrder.workOrderStatus.Reserved:
			status.color = Color.red;
			break;
		case WorkOrder.workOrderStatus.Running:
			status.color = Color.green;
			break;
		default:
			break;
		}


	}
}


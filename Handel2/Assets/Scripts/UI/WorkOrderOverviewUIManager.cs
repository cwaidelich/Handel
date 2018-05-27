using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorkOrderOverviewUIManager : MonoBehaviour {

	private WorkOrderManager workOrderManager;
	private List<WorkOrder> UIList = new List<WorkOrder>();

	private Vector2 startPos = new Vector2(0,0);
	private float distanceBetweenPanel = -35f;

	public Text totalText;
	public RectTransform PanelPreFab;
	public RectTransform parent;

	// Use this for initialization
	void Start () {
		workOrderManager = GameObject.Find ("WorkOrderManager").GetComponent<WorkOrderManager> ();
	}
	
	// Update is called once per frame
	void Update() {

		totalText.text = workOrderManager.workOrderQueue.Count.ToString();
		foreach (WorkOrder _wo in workOrderManager.workOrderQueue) {
			if (_wo.status != WorkOrder.workOrderStatus.Finish && !UIList.Exists (w => w == _wo)) {
				UIList.Add (_wo);
				UpdateList ();
			} else if (_wo.status == WorkOrder.workOrderStatus.Finish && UIList.Exists (w => w == _wo)) {
				UIList.Remove (_wo);
			}
		}
	}

	void UpdateList() {
		for (int i = 0; i < parent.transform.childCount; i++) {
			GameObject.Destroy(parent.GetChild (i).gameObject);
		}

		int j = 0;

		foreach (WorkOrder _wo in UIList) {
			RectTransform rt = (RectTransform)Instantiate (PanelPreFab, parent.transform);
			rt.GetComponent<Panel_List_UI> ().wo = _wo;
			rt.anchoredPosition = new Vector2 (startPos.x, startPos.y + j * distanceBetweenPanel);
			j++;
		}

	}
}

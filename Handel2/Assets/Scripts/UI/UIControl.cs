using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {

	public Canvas MainMenu;
	public Canvas WorkOrderOverview;

	[SerializeField]
	private Transform Container;
	public GameObject WohnungBuilding;
	public GameObject MineBuilding;
	public GameObject LagerBuilding;
	public GameObject StreetBuilding;

	private bool isBuildMenuActive; //SubMenu Build
	private bool isWorkOrderOverviewMenuActive; //Workorder Overview


	void Start () {
		isBuildMenuActive = false;	
		isWorkOrderOverviewMenuActive = false;
	}

	public void onClickBuildMenu() {
		isBuildMenuActive = activeSubMenu (MainMenu, isBuildMenuActive);
	}

	public void onClickWorkOrderOverviewButton() {
		isWorkOrderOverviewMenuActive = activeSubMenu (WorkOrderOverview, isWorkOrderOverviewMenuActive);
	}


	// *** SUBMENU *** //

	//Kontrolliert die Knöpfte des Build - Sub Menus
	public void onClickBuilingButton(string buttonName) {

		GameObject toCall = null;
		bool istoCall;
		istoCall = false;

		if (buttonName == "Wohnung") {
			toCall = WohnungBuilding;
			istoCall = true;
		} else if (buttonName == "Mine") {
			toCall = MineBuilding;
			istoCall = true;
		} else if (buttonName == "Lager") {
			toCall = LagerBuilding;
			istoCall = true;
		} else if (buttonName == "Street") {
			toCall = StreetBuilding;
			istoCall = true;

		}

		if (istoCall) {
			Instantiate (toCall,Container);
			isBuildMenuActive = activeSubMenu (MainMenu, isBuildMenuActive);
		}
	}

	// GENERISCHE METHODEN //

	//prüft ob Menu aktiv ist oder nicht um es zu (de-)aktieren
	private bool activeSubMenu(Canvas _subCanvas, bool _checkVar) {
		if (!_checkVar) {
			_subCanvas.gameObject.SetActive(true);
			_checkVar = true;
		} else{
			_subCanvas.gameObject.SetActive(false);
			_checkVar = false;
		}
		return _checkVar;
	}





}

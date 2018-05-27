using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="Handel/Ressource Controller")]
public class RessourcesController : ScriptableObject {

	[SerializeField]
	private float totalAmount;
	[SerializeField]
	private Element element;
	public enum workOrderType
	{
		Need_Ressource,
		Give_Ressource,
		Dig_Ressource,
	};
	[SerializeField]
	private workOrderType currentWorkOrder;

	public RessourcesController (float _totalAmount,Element _element, workOrderType _type) {
		element = _element;
		currentWorkOrder = _type;
		totalAmount = _totalAmount;
	}


	public string Type {
		get {
			return currentWorkOrder.ToString ();
		}
	}
		
	public float TotalAmount {
		get {
			return totalAmount;
		}
	}

	public float TotalAmountHack {
		set {
			totalAmount = value;
		}
	}

	public string TypeShorty {
		get {
			switch (currentWorkOrder) {

			case workOrderType.Dig_Ressource:
				return "DIG";
			case workOrderType.Give_Ressource:
				return "GIVE";
			case workOrderType.Need_Ressource:
				return "NEED";
			default:
				return "?";
			}
		}
	}

	public Element Element {
		get {
			return element;
		}
	}

	public void setDig() {
		currentWorkOrder = workOrderType.Dig_Ressource;
	}



}

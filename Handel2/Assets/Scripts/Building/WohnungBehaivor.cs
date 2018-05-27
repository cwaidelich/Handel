using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LightControl))]
[RequireComponent(typeof(BuildingBehaivor3))]
public class sWohnungBehaivor : MonoBehaviour {
	//
	public float batteryCapacityMax = 50f;
	public float chargingTimeInSeconds = 3f;
	private float currentCapacity;

	private LightControl lightControl;
	private BuildingBehaivor3 bb;
	// Use this for initialization
	void Start () {
		currentCapacity = 0;
		lightControl = GetComponent<LightControl> ();
		bb = GetComponent<BuildingBehaivor3> ();

		StartCoroutine ("chargingBatteryBehaivor");
	}

	IEnumerator chargingBatteryBehaivor() {
		while (true) {
			if (currentCapacity < batteryCapacityMax && lightControl.isNightForObject() == false && bb.CurrentState == "Productive") {	
				currentCapacity++;
			}

			if (currentCapacity >= batteryCapacityMax)
				currentCapacity = batteryCapacityMax;

			yield return new WaitForSeconds (chargingTimeInSeconds);
		}
	}

	public string EnergyState {
		get	{
			return currentCapacity.ToString () + "/" + batteryCapacityMax.ToString ();
		}
	}

}

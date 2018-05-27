using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayData : MonoBehaviour {

	public static float lastTimeCollitionCheck;
	public static float timeBetweenCollision;


	void BuildingAmountUpdate(int _buildID, string _command ) {

		if (_command == "BUILDING_PLACED") {
			StatsContainer.UpdateStat ("BUILDING_AMOUNT", _buildID, "RAISE", 1);
		} else if (_command == "BUILDING_DESTROYED") {
			StatsContainer.UpdateStat ("BUILDING_AMOUNT", _buildID, "REDUCE", 1);
		} else {
			Debug.LogError ("Command not found");
		}
	}

	void updateTimeCollition(float _time,float _timeBetween) {
		lastTimeCollitionCheck = _time;
		timeBetweenCollision = _timeBetween;

	}


}

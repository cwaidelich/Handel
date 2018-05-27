using UnityEngine;
using System.Collections;

public class Stats {


	private string typ;
	private int object_id;
	private int index;
	private float value;
	private bool isValueInt;
	private bool negativeAllow;
	private string statName;

	public Stats (string _typ, int _id, string _statName, int _index) {
		typ = _typ;
		object_id = _id;
		statName = _statName;
		index = _index;
		value = 0;
		checkReturnType();
	}

	public string Typ {
		get { return typ; }
	}

	public int ID {
		get { return object_id; }
	}

	public int getValueInt () {
		if (isValueInt) {
			return (int)value;
		}
		return -9827272;
	}

	public float getValueFloat () {
		if (!isValueInt) {
			return value;
		}
		return -982.7272f;
	}


	public void ReduceToStat (float f) {
		value = value - f;
	}

	public void AddToStat (float f) {
		value = value + f;
	}

	public void OverwriteStat (float _value) {
		value = _value;
	}

	private void checkReturnType() {
		isValueInt = false;
		negativeAllow = true;
		if (typ == Building_Amount) {
			isValueInt = true;
			negativeAllow = false;
		}
	}

	//Types
	public string Building_Amount {
		get {return "BUILDING_AMOUNT"; }
	}

}

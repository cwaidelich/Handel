using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StatsContainer  {

	//liste
	private static List<Stats> valueBox = new List<Stats>();
	private static string[] acceptedTypes = new string[] {"BUILDING_AMOUNT","ELEMENT_AMOUNT"};
	private static string[] acceptedActions = new string[] {"RAISE","REDUCE","DEFINE"};

	private static void CreateStatEntry(string _typ, int _id, string _statName) {
		if (IsStringAllowed (_typ,acceptedTypes)) {
			valueBox.Add (new Stats (_typ, _id, _statName,  (valueBox.Count+1)));
		}
		return;
	}

	// Funktion wird benutzt um schnelle Update auf den Stats zu machen
	public static void UpdateStat(string _typ, int _id, string _action, float _value) {
		if (!valueBox.Exists (x => x.ID == _id && x.Typ == _typ)) {
			CreateStatEntry (_typ, _id, "");
		}

		if (IsStringAllowed(_action, acceptedActions)) {
			switch(_action) {
				case "RAISE":
					valueBox.Find (x => x.ID == _id && x.Typ == _typ).AddToStat(_value);
				break;

				case "REDUCE":
					valueBox.Find (x => x.ID == _id && x.Typ == _typ).ReduceToStat(_value);
				break;

				case "DEFINE":
					valueBox.Find (x => x.ID == _id && x.Typ == _typ).OverwriteStat(_value);
				break;
			}
		}
	}

	public static float GetValueF(string _typ, int _id) {
		if (valueBox.Exists (x => x.ID == _id && x.Typ == _typ)) {
			return valueBox.Find (x => x.ID == _id && x.Typ == _typ).getValueFloat ();
		} 
		return 0;

	}

	public static int GetValueI(string _typ, int _id) {
		if (valueBox.Exists (x => x.ID == _id && x.Typ == _typ)) {
			return valueBox.Find (x => x.ID == _id && x.Typ == _typ).getValueInt ();
		}
		return 0;
	}

	private static bool IsStringAllowed(string _typ, string[] _arr) {
		bool typavailable = false;
		foreach (string t in _arr) {
			if (t == _typ) {
				typavailable = true;
				break;

			}
		}
		return typavailable;
	}
		


}

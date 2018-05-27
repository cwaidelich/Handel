using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="Handel/Production Order")]
public class ProductionOrder : ScriptableObject {

	public RessourcesController[] rc_In;
	public RessourcesController[] rc_Out;

	public float produceTimeInSeconds;
	public float produceVelocityInSeconds;

	

}

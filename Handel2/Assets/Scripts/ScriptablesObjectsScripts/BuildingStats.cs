using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
[CreateAssetMenu(menuName="Handel/BuildingStats")]

public class BuildingStats : ScriptableObject {

	public int buildingSizeWeight;
	public int buildingSizeHeight;

	public int buildingID;
	public string buildingName;

	public RessourcesController[] rc_toBuild;

	public float buildTimeInSeconds;
	public float buildVelocityInSeconds;

	public bool hasProduction;
	public ProductionOrder productionOrder;

	public Node.Content content;
}

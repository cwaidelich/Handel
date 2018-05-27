using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName="Handel/Planet")]
public class Planet : ScriptableObject{


	public Element primaryElement;
	public int primaryElementAmountPerTile;

	public float nodeRadius = GeneralStaticClass.nodeRadius;
	public Vector2 gridWorldSize;

	[Range(0,3000)]
	public float temperatureKelvin;


	public float differenceBetweenNightAndDayInKelvin;

}

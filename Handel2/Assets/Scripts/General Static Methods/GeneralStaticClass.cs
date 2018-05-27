using UnityEngine;
using System.Collections;

public static class GeneralStaticClass  {

	public const float nodeRadius = 1.28f;

	public static Vector2 getErrorVector2 {
		get { return new Vector2(99999,99999); }
	}

	public static int GetDistance(Node v1, Node v2) {
		Vector2 v1Pos = v1.getGridPosition ();//lon, lat
		Vector2 v2Pos = v2.getGridPosition ();//lon, lat

		int dstLon = (int) Mathf.Abs (v1Pos.x - v2Pos.x);
		int dstLat = (int) Mathf.Abs (v1Pos.y - v2Pos.y);

		if (dstLon > dstLat) {
			return 14 * dstLat + 10 * (dstLon - dstLat);
		} else {
			return 14 * dstLon + 10 * (dstLat - dstLon);
		}


	}

	public static double KelvinToCelcius(float _kelvin) {
		return _kelvin - 273.15;
	}
}

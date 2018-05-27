using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class createMesh : MonoBehaviour {


	public float Radius;
	public int Longitude;
	public int Latitude;

	public static Vertice[,] grid; // lat, lon -- Achtung, anders als bei Vertice Vector2 lon lan
	private Mesh mesh;

	// Use this for initialization
	void Awake () {

		MeshFilter filter = gameObject.AddComponent< MeshFilter >();
		mesh = filter.mesh;
		mesh.Clear();

		float radius = Radius;
		// Longitude |||
		int nbLong = Longitude;
		// Latitude ---
		int nbLat = Latitude;


		#region Vertices
		Vector3[] vertices = new Vector3[(nbLong+1) * nbLat + 2];

		grid = new Vertice[nbLat+2,nbLong+1];

		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;

		vertices[0] = Vector3.up * radius;
		for (int i_ = 0; i_ <= nbLong; i_++) {
			grid[0,i_] = new Vertice(vertices[0],true,(float) i_,0f);

		}


		for( int lat = 0; lat < nbLat; lat++ )
		{
			float a1 = _pi * (float)(lat+1) / (nbLat+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);

			for( int lon = 0; lon <= nbLong; lon++ )
			{
				float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
				float sin2 = Mathf.Sin(a2);
				float cos2 = Mathf.Cos(a2);
				int currentVertice = lon + lat * (nbLong + 1) + 1;

				// print ("lat:"+lat+";lon:"+lon+"verticeIndex:"+currentVertice);
				vertices[currentVertice] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * radius;
				//print (vertices[currentVertice]);
				grid[lat+1,lon] = new Vertice(vertices[currentVertice],true,lon,lat+1);
			}
		}
		vertices[vertices.Length-1] = Vector3.up * -radius;
		for (int i_ = 0; i_ <= nbLong; i_++) {
			grid[nbLat+1,i_] = new Vertice(vertices[vertices.Length-1],true,nbLat,(float) i_);
		}

		#endregion

		#region Normales		
		Vector3[] normales = new Vector3[vertices.Length];
		for( int n = 0; n < vertices.Length; n++ )
			normales[n] = vertices[n].normalized;
		#endregion

		#region UVs
		Vector2[] uvs = new Vector2[vertices.Length];
		uvs[0] = Vector2.up;
		uvs[uvs.Length-1] = Vector2.zero;
		for( int lat = 0; lat < nbLat; lat++ )
			for( int lon = 0; lon <= nbLong; lon++ )
				uvs[lon + lat * (nbLong + 1) + 1] = new Vector2( (float)lon / nbLong, 1f - (float)(lat+1) / (nbLat+1) );
		#endregion

		#region Triangles
		int nbFaces = vertices.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[ nbIndexes ];

		//Top Cap
		int i = 0;
		for( int lon = 0; lon < nbLong; lon++ )
		{
			triangles[i++] = lon+2;
			triangles[i++] = lon+1;
			triangles[i++] = 0;
		}

		//Middle
		for( int lat = 0; lat < nbLat - 1; lat++ )
		{
			for( int lon = 0; lon < nbLong; lon++ )
			{
				int current = lon + lat * (nbLong + 1) + 1;
				int next = current + nbLong + 1;

				triangles[i++] = current;
				triangles[i++] = current + 1;
				triangles[i++] = next + 1;

				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = next;
			}
		}

		//Bottom Cap
		for( int lon = 0; lon < nbLong; lon++ )
		{
			triangles[i++] = vertices.Length - 1;
			triangles[i++] = vertices.Length - (lon+2) - 1;
			triangles[i++] = vertices.Length - (lon+1) - 1;
		}
		#endregion

		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		mesh.RecalculateBounds();
		;

		gameObject.AddComponent<MeshCollider> ().sharedMesh = mesh;
		//print (grid);
	}
		
	public int gridLatLength {
		get {
			return grid.GetLength (0);
		}
	}

	public int gridLonLength {
		get {
			return grid.GetLength (1) - 1;
		}
	}

	public int GridMaxSize {
		get {
			return gridLatLength * gridLonLength; 
		}
	}

	public Vector3 getVerticeInGrid(int _lat, int _lon) {
		return grid [_lat, _lon].getWorldCoor ();
	}

	public void setWalkable(int _lat, int _lon, bool _walk) {
		grid [_lat, _lon].setWalkable (_walk);
		return;
	}

	public void setName(int _lat, int _lon, string _name) {
		grid [_lat, _lon].setName (_name);
		return;
	}


	public bool getWalkable(int _lat, int _lon) {
		return grid [_lat, _lon].getWalkable();
	}

	public string getName(int _lat, int _lon) {
		return grid [_lat, _lon].getName();
	}

	public bool getHasName(int _lat, int _lon) {
		return grid [_lat, _lon].getHasName();
	}

	public float getRadius(){
		return Radius;
	}



	public Vertice WorldCoorToGridPosition (Vector3 _worldCoordinates, Transform _verticeToWorldTransform) {

		for (int _lat = 0; _lat < gridLatLength; _lat++) {
			for (int _lon = 0; _lon <= gridLonLength; _lon++) {
				if (_verticeToWorldTransform.TransformPoint(grid [_lat, _lon].getWorldCoor()) == _worldCoordinates) {
					return grid [_lat, _lon]; //lon, lat
				}
			}
		}
		return null;

	}


	public List<Vertice> GetNeighbours (Vertice v) {
		List<Vertice> neighbours = new List<Vertice>();

		Vector2 vPos = v.getGridPosition (); //x:lon, y:lat

		if (vPos.x == 0) {
			for (int i = 0; i < gridLatLength; i++) {
				neighbours.Add(grid[i,(int)vPos.x+1]);
			}
			return neighbours;
		}

		if (vPos.x == gridLonLength) {
			for (int i = 0; i < gridLatLength; i++) {
				neighbours.Add(grid[i,(int)vPos.x-1]);
			}
			return neighbours;
		}


		for (int lon = -1; lon <= 1; lon++) {
			for (int lat = -1; lat <= 1; lat++) {
				if (lon == 0 && lat == 0)
					continue;

				int correctLat;
				Vector2 nPos = grid [(int)vPos.y+lat, (int)vPos.x+lon].getGridPosition ();
				if (nPos.y < 0) {
					correctLat = gridLatLength + (int)nPos.y;
				} else {
					correctLat = (int)nPos.y;
				}
				neighbours.Add (grid [correctLat, (int)nPos.x]);
			}
		}

		return neighbours;
	}



}

using UnityEngine;
using System.Collections;

public class StreetBehaivor : MonoBehaviour {

	SpriteRenderer sr;

	private BuildingBehaivor3 bb;
	private Grid grid;
	private Animator animator;

	public bool isPlaced;
	Node currentNode;
	private Element elementToTransport ;
	[SerializeField]
	private GameObject spriteObject;

	void Start () {
		bb = GetComponent<BuildingBehaivor3> ();
		animator = GetComponent<Animator> ();
		grid = GameObject.Find ("A*").GetComponent<Grid> ();

		isPlaced = false;
	
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPlaced && bb.IsBuild) {
			isPlaced = true;
			currentNode = bb.placedNode;
			currentNode.movementPenalty = 10;

		} else if (bb.IsBuild) {

			defineAnimation ();
			if (ElementToTransport == null)
				currentNode.ElementForTransport = new Element ();
			else {
				currentNode.ElementForTransport = ElementToTransport;
				if (ElementToTransport.Sprite != null)
					spriteObject.GetComponent<SpriteRenderer> ().sprite = ElementToTransport.Sprite [0];
				else
					spriteObject.GetComponent<SpriteRenderer> ().sprite = null;
			}

		}
		sr.color = Color.gray;

	
	}


	public Element ElementToTransport {
		get {
			if (elementToTransport == null)
				return new Element();
			return elementToTransport;
		}
		set {
			elementToTransport = value;
		}
	}


	// Direction Status const; Sind im Animator fest eingetippt. Nicht verändern
	int cross = 1;
	int vertical = 2;
	int horizontal = 3;
	int SO = 4;
	int NO = 5;
	int SW = 6;
	int NW = 7;
	int NOS = 8;
	int NWS = 9;
	int WNO = 10;
	int WSO = 11;
	int N_ = 12;
	int S_ = 13;
	int W_ = 14;
	int O_ = 15;

	void defineAnimation() {
		Node Nord, Sued, West, Ost;
		bool N = false, S = false, W = false, O = false;


		Vector2 gridPosition = currentNode.getGridPosition ();
		West = grid.getNote ((int)gridPosition.x - 1, (int)gridPosition.y);
		Ost = grid.getNote ((int)gridPosition.x + 1, (int)gridPosition.y);
		Sued = grid.getNote ((int)gridPosition.x, (int)gridPosition.y - 1);
		Nord = grid.getNote ((int)gridPosition.x, (int)gridPosition.y + 1);

		if (Nord.content == Node.Content.street)
			N = true;
		if (Sued.content == Node.Content.street)
			S = true;
		if (West.content == Node.Content.street)
			W = true;
		if (Ost.content == Node.Content.street)
			O = true;

		if (N && S && W && O)
			animator.SetInteger ("directionStatus", cross);
		else if (N && O && S)
			animator.SetInteger ("directionStatus", NOS);
		else if (N && W && S)
			animator.SetInteger ("directionStatus", NWS);
		else if (W && N && O)
			animator.SetInteger ("directionStatus", WNO);
		else if (W && S && O)
			animator.SetInteger ("directionStatus", WSO);
		else if (N && S)
			animator.SetInteger ("directionStatus", vertical);
		else if (W && O)
			animator.SetInteger ("directionStatus", horizontal);
		else if (S && O)
			animator.SetInteger ("directionStatus", SO);
		else if (N && O)
			animator.SetInteger ("directionStatus", NO);
		else if (S && W)
			animator.SetInteger ("directionStatus", SW);
		else if (N && W)
			animator.SetInteger ("directionStatus", NW);
		else if (N)
			animator.SetInteger ("directionStatus", N_);
		else if (S)
			animator.SetInteger ("directionStatus", S_);
		else if (W)
			animator.SetInteger ("directionStatus", W_);
		else if (O)
			animator.SetInteger ("directionStatus", O_);
		else
			animator.SetInteger ("directionStatus", 0);

		//Debug.Log("Animator, directionStatus: "+ animator.GetInteger("directionStatus").ToString());
	}
}

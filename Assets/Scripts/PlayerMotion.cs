using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {
	private Rigidbody2D rb;
	private CircleCollider2D cc2d;
	private Vector2 mousePosition, startVector, finishVector, velocity, drag;
	private string colName;
	private float dragCoef = .005f;
	private bool xFlag, nxFlag, yFlag, nyFlag;

	public GameObject gameLogic;
	private LevelManager levelManager;

	//private float startVX, finishVX, startVY, finishVY;

	// Use this for initialization
	void Start () {
		//Debug.Log ("start p");
		rb = GetComponent<Rigidbody2D> ();
		cc2d = GetComponent<CircleCollider2D> ();
		//rb.drag = drag;
		//SetPosition(-5, 0);

		levelManager = gameLogic.GetComponent<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("update p");
		//Debug.Log (GetComponent<CircleCollider2D> ().OverlapPoint());
		xFlag = yFlag = nxFlag = nyFlag = false;
		if (velocity.x >= 0)
			xFlag = true;
		if (velocity.y >= 0)
			yFlag = true;
		velocity += dragCoef * drag;
		rb.velocity = velocity;

		if (velocity.x >= 0)
			nxFlag = true;
		if (velocity.y >= 0)
			nyFlag = true;

		if (((xFlag && nxFlag) || (!xFlag && !nxFlag)) && ((yFlag && nyFlag) || (!yFlag && !nyFlag))) {
		} else {
			velocity.x = velocity.y = 0;
			rb.velocity = velocity;
		}


		if(Input.GetMouseButtonDown(0)){
			mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if(cc2d.OverlapPoint(mousePosition))
			{
				//Debug.Log("player");
				startVector = mousePosition;

			}
		}

		if(Input.GetMouseButtonUp(0)){
			mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			//if(cc2d.OverlapPoint(mousePosition))
			//{
				//Debug.Log("player");
			finishVector = mousePosition;
			velocity = finishVector - startVector;
			Debug.Log (velocity);
			rb.velocity = velocity;
			drag = -velocity;
			//}
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		colName = other.gameObject.name;
		Debug.Log(colName);
		if (colName.Equals ("BottomWall") || colName.Equals ("TopWall")) {
			velocity.y *= -1;
			rb.velocity = velocity;
			drag = -velocity;
		} else if (colName.Equals ("LeftWall") || colName.Equals ("RightWall")) {
			velocity.x *= -1;
			rb.velocity = velocity;
			drag = -velocity;
		} else if (colName.Equals ("Target")) {
			velocity.x = velocity.y = 0;
			rb.velocity = velocity;
			drag = velocity;
			levelManager.LevelWin ();
		} else if (colName.Equals ("Mine")) {
			velocity.x = velocity.y = 0;
			rb.velocity = velocity;
			drag = velocity;
			levelManager.LevelLose ();
		} else {}
	}
	/*
	public void SetPosition(float x, float y) {
		gameObject.transform.SetPositionAndRotation (new Vector2 (x, y), Quaternion.identity);
	}
	*/
}

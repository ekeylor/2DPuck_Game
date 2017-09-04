using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Possible Power ups:
 * 	Get another try
 * 	Reduce friction
 *	Penetrate obstacles
 * 
 * Possible Levels:
 * 	Bounce one target into another
 *  Complete within certain number of tries
 * 	Use "teleport gate"
 * 	Avoid moving obstacles (random motion / "track" motion / target the player)
 * 	Require to hit specific walls
 */

public class LevelManager : MonoBehaviour {
	private int levelNum = -1;
	private bool newLevelFlag;
	private bool resetLevelFlag;

	public GameObject player;
	public GameObject target;
	public GameObject mine;
	private GameObject mineObj;

	private List<Vector2> playerStart;
	private List<Vector2> targetStart;

	public Text instruction;
	private List<string> instructions;
	private float alpha;

	// Use this for initialization
	void Start () {
		playerStart = new List<Vector2>();
		playerStart.Add (new Vector2 (-5, 0));
		playerStart.Add (new Vector2 (-5, 0));
		playerStart.Add (new Vector2 (-5, 0));

		targetStart = new List<Vector2>();
		targetStart.Add (new Vector2 (5, 0));
		targetStart.Add (new Vector2 (5, 0));
		targetStart.Add (new Vector2 (5, 0));

		newLevelFlag = true;
		resetLevelFlag = true;

		instructions = new List<string> ();
		instructions.Add("Welcome to Hockey Puzzle!\n\nClick on the green puck, drag, and release to\n" +
			"fling it at the red puck!\n\nThe longer you click, the faster it\'ll go!\n\nTry to hit the red puck in one shot.");
		instructions.Add("Great!  Bounce the puck off the wall to\nAVOID THE MINE.");
		instructions.Add("Keep going!  Thing's will get more challenging.");
		instructions.Add("That's all for now.");
		alpha = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (resetLevelFlag) {
			resetLevelFlag = false;
			StopCoroutine ("Fade");
			if (newLevelFlag) {
				newLevelFlag = false;
				levelNum++;
			}
			SetPositions ();
			SetMines ();
			SetInterface ();
		}

	}

	private void SetInterface() {
		if (levelNum >= instructions.Count)
			return;
		
		//alpha = 1.0f;
		instruction.text = instructions [levelNum];
		StartCoroutine ("Fade");
	}

	IEnumerator Fade() {
		alpha = 1.0f;
		Color c = new Color(instruction.color.r, instruction.color.g, instruction.color.b, alpha);
		instruction.color = c;
		yield return new WaitForSeconds (3.0f);
		while (alpha > 0) {
			c = new Color(instruction.color.r, instruction.color.g, instruction.color.b, alpha);
			instruction.color = c;
			alpha -= 0.01f;
			yield return new WaitForSeconds (0.02f);
		}
		StopCoroutine ("Fade");

	}

	private void SetPositions() {
		if (levelNum >= playerStart.Count)
			return;
		
		player.transform.SetPositionAndRotation (playerStart[levelNum], Quaternion.identity);
		target.transform.SetPositionAndRotation (targetStart[levelNum], Quaternion.identity);
	}

	private void SetMines() {
		if (levelNum == 0)
			return;
		else if (levelNum == 1) {
			mineObj = Instantiate (mine, new Vector2 (0, 0), Quaternion.identity);
			mineObj.name = "Mine";
		}
		else if (levelNum == 2) {
			mineObj = Instantiate (mine, new Vector2(0, 0), Quaternion.identity);
			mineObj.name = "Mine";
			mineObj = Instantiate (mine, new Vector2(0, -2), Quaternion.identity);
			mineObj.name = "Mine";
		} else {}
	}

	public void LevelWin() {
		resetLevelFlag = true;
		newLevelFlag = true;
	}

	public void LevelLose() {
		resetLevelFlag = true;
	}
}
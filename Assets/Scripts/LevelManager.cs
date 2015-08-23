using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public float timeBetweenSpawns = 10f;
	public float timeSinceSpawns;
	public GameObject person;
	public GameObject police;

	public Text timer;
	public float levelTime = 5 * 60; //seconds before level ends


	bool transition;

	// Use this for initialization
	void Start () {
		timeSinceSpawns = timeBetweenSpawns;
	}


	public void endLevel() {
		//interact with static script to keep statistics (eg voice level, body count)

		//adjust diffidulty for next level

		//load next level
		Application.LoadLevel (1);
	}

	public void lose() {
		Application.LoadLevel ("Gameover");
	}

	
	// Update is called once per frame
	void Update () {
		timeSinceSpawns += Time.deltaTime;
		if (timeSinceSpawns > timeBetweenSpawns) {
			SpawnNPC ();
			timeSinceSpawns = 0f;
		}
		levelTime -= Time.deltaTime;
		if (levelTime <= 0) {
			endLevel ();
		}
		int mins = (int)(levelTime / 60);
		int secs = (int)(levelTime % 60);
		string txt = mins + ":" + secs;
		if (secs < 10) {
			txt = mins + ":0" + secs;
		}
		timer.text = txt;
	}
	
	void SpawnNPC(){
		int random = Random.Range (0, 10);
		if (random == 0) {
			GameObject policeNPC = Instantiate (police);
			if (policeNPC.GetComponent<MoveBetweenPoints>().moveType == MoveBetweenPoints.MoveType.wait) {
				policeNPC.GetComponent<MoveBetweenPoints>().moveType = MoveBetweenPoints.MoveType.patrol;
			}
		} else {
			Instantiate (person);
		}
	}
}

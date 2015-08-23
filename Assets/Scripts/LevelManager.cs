using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public float timeBetweenSpawns = 10f;
	public float timeSinceSpawns;
	public GameObject person;
	public GameObject police;

	// Use this for initialization
	void Start () {
		timeSinceSpawns = timeBetweenSpawns;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceSpawns += Time.deltaTime;
		if (timeSinceSpawns > timeBetweenSpawns) {
			SpawnNPC ();
			timeSinceSpawns = 0f;
		}
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

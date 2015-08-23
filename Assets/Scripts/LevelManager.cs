using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public float timeBetweenSpawns = 10f;
	public float timeSinceSpawns;
	public GameObject person;
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
		GameObject npc = Instantiate (person);
	}
}

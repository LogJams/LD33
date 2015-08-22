using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vision))]
public class Behavior : MonoBehaviour {
	Vision vision;
	public bool isPolice = false;
	float timeSinceLastAction;
		public float timeBetweenActions = 15f;
	// Use this for initialization
	void Start () {
		vision = GetComponent<Vision> ();
		timeSinceLastAction = timeBetweenActions;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastAction += Time.deltaTime;
		if (vision.inMain == true && timeSinceLastAction > timeBetweenActions){
				Detect ();
		} else if (vision.inPeripheral == true) {
			TurnToInvestigate();
		}
	}

	void Detect (){
		int rand = Random.Range (0, 100);

		if (isPolice == true) {
			// detect player, you lose
			Debug.Log("POLICE: Hey, that's a monster!");
		} else {
			// 50 run away, 30 call police, 20 do nothing
			if (rand < 50) {
				Debug.Log("RUN AWAY");
				// run away
			} else if (rand < 80) {
				Debug.Log("Hey, that's a monster!");
				// detect player, you lose
			} // else do nothing
		}
		timeSinceLastAction = 0f;
	}

	void TurnToInvestigate(){
		Debug.Log ("turning");
	}

}

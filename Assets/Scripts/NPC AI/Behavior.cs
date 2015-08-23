using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vision))]
public class Behavior : MonoBehaviour {
	Vision vision;
	public bool isPolice = false;
	public float timeBetweenActions = 15f;

	float timeSinceLastAction;


	bool running;

	float rotSpeed = 2;//rotation speed

	LevelManager manager;
	// Use this for initialization
	void Start () {
		timeSinceLastAction = timeBetweenActions; //allows immediate acting on spawn
		vision = GetComponent<Vision> ();
		timeSinceLastAction = timeBetweenActions;
		manager = (GameObject.FindGameObjectsWithTag ("Manager"))[0].GetComponent<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!running) {
			timeSinceLastAction += Time.deltaTime;
			if (vision.inMain == true && timeSinceLastAction > timeBetweenActions) {
				Detect ();
			} else if (vision.inPeripheral == true || vision.inMain == true) {
				TurnToInvestigate ();
			} else if (vision.inMain == false) {
				if (this.gameObject.GetComponent<MoveBetweenPoints> () != null) {
					this.gameObject.GetComponent<MoveBetweenPoints> ().enabled = true;
				}
			} 
		} else {
			//run away!
		}
	}

	void Detect (){
		int rand = Random.Range (0, 100);

		if (isPolice == true) {
			// detect player, you lose
			Debug.Log("POLICE: Hey, that's a monster!");
			manager.lose();
		} else {
			// 50 run away, 30 call police, 20 do nothing
			if (rand < 50) {
				Debug.Log("RUN AWAY");
				// run away
				running = true;
				GetComponent<MoveBetweenPoints>().enabled = false;
			} else if (rand < 80) {
				Debug.Log("Hey, that's a monster!");
				manager.lose();
				// detect player, you lose
			} // else do nothing
		}
		timeSinceLastAction = 0f;
	}

	void TurnToInvestigate(){
		GameObject closestTarget = null;
		float closestDisctance = float.MaxValue;
		if (this.gameObject.GetComponent<MoveBetweenPoints>()!= null ){
			this.gameObject.GetComponent<MoveBetweenPoints>().enabled = false;
		}
		foreach (GameObject target in vision.inLineOfSight){
			if (target.CompareTag("Player") == true) {
				closestTarget = target;
				break;
			} else {
				Vector2 vector = target.transform.position - this.gameObject.transform.position;
				if (vector.magnitude < closestDisctance) {
					closestTarget = target;
				}
			}
		}
		Vector2 sightVector = closestTarget.transform.position - this.gameObject.transform.position;
		// get angle of vision and compare between main and peripheral angles to determine what form of vision is being used
		// raycast accordingly
		float angle = Mathf.Rad2Deg * Mathf.Atan2 (sightVector.y, sightVector.x);
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (new Vector3 (0, 0, angle)), Time.deltaTime * rotSpeed);;
	}

}

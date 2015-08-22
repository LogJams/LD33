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
		if (vision.inMain == true && timeSinceLastAction > timeBetweenActions) {
			Detect ();
		} else if (vision.inPeripheral == true) {
			TurnToInvestigate ();
		} else if(vision.inMain == false){
			if (this.gameObject.GetComponent<MoveBetweenPoints>()!= null ){
				this.gameObject.GetComponent<MoveBetweenPoints>().enabled = true;
			}
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
		float angle = Mathf.Rad2Deg * Mathf.Atan2(sightVector.y, sightVector.x) - vision.angleToFront;
		transform.Rotate (transform.eulerAngles, -angle);
	}

}

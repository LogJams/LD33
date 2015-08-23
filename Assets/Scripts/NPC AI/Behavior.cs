using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vision))]
public class Behavior : MonoBehaviour {
	Vision vision;
	public bool isPolice = false;
	public float timeBetweenActions = 15f;
	public float detectDelay = .5f;
	bool detectSoundPlayed = false;

	float timeSinceDetection;
	float timeSinceLastAction = 0f;


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
				timeSinceDetection += Time.deltaTime;
				if (timeSinceDetection > detectDelay){
					Detect ();
					timeSinceDetection = 0f;
					detectSoundPlayed = false;
				}else if (!detectSoundPlayed) {
					//play detect sound
					detectSoundPlayed = true;
				}
			} else if (vision.inPeripheral == true || vision.inMain == true) {
				TurnToInvestigate ();
				detectSoundPlayed = false;
			} else if (vision.inMain == false) {
				detectSoundPlayed = false;
			} else{
				detectSoundPlayed = false;
			}
		}
	}

	void Detect (){
		int rand = Random.Range (0, 100);

		if (isPolice == true) {
			// detect player, you lose
			Debug.Log("POLICE: Hey, that's a monster!");
			GameInfo.loseCondition = GameInfo.LoseCondition.PoliceCaught;
			manager.beginFade(true);
		} else {
			// 50 run away, 30 call police, 20 do nothing
			if (rand < 70) {
				Debug.Log("RUN AWAY");
				// run away
				running = true;
				GetComponent<MoveBetweenPoints>().startRunning();
			} else {
				Debug.Log("Hey, that's a monster!");
				GameInfo.loseCondition = GameInfo.LoseCondition.PoliceCalled;
				manager.beginFade(true);
				// detect player, you lose
			} // else do nothing
		}
		timeSinceLastAction = 0f;
	}

	void TurnToInvestigate(){
		GameObject closestTarget = null;
		float closestDisctance = float.MaxValue;
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

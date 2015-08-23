using UnityEngine;
using System.Collections;

public class MoveBetweenPoints : MonoBehaviour {

	public Transform[] waypoints;
	public enum MoveType{patrol, pass, wait};
	public MoveType moveType;
	public bool patrol;

	int currentWaypoint;

	float speed = 0.75f;
	float rotSpeed = 5;

	float desiredRot; //desired rotation at waypoint
	float rotation; //desired rotation + sinusoidal offset

	Vector3 velocity;



	Rigidbody2D body;


	// Use this for initialization
	void Start () {
		velocity = new Vector3 ();
		body = GetComponent<Rigidbody2D> ();
		int rand = Random.Range (0, 100);

		if (rand < 50) {
			moveType = MoveType.pass;
		} else if (rand < 80) {
			moveType = MoveType.patrol;
		} else {
			moveType = MoveType.wait;
		}
		GetPath ();
		currentWaypoint = 0;
		transform.position = waypoints [0].position;
	}
	
	// Update is called once per frame
	void Update () {
		if (waypoints.Length > 0 && currentWaypoint < waypoints.Length) { //if we're moving toward a waypoint
			//if we're at the target, increment the current waypoint
			if ((transform.position - waypoints[currentWaypoint].position).sqrMagnitude < 0.1f) {
				currentWaypoint ++;
			}
			//if we're patrolling modulate the waypoint by the length
			if (moveType == MoveType.patrol) {
				currentWaypoint = currentWaypoint % waypoints.Length;
			}
			//move toward current waypoint
			if (currentWaypoint < waypoints.Length) {
			Vector3 targetPos = waypoints[currentWaypoint].position;
			velocity = targetPos - transform.position;
			velocity = velocity.normalized * speed;
			}
			//set desired rotation
			desiredRot = Mathf.Atan2 (velocity.y, velocity.x) * Mathf.Rad2Deg;
		}
	}

	void FixedUpdate() {
		if (waypoints.Length > 0 && currentWaypoint < waypoints.Length) { //if we're moving toward a waypoint
			//move toward target
			if (velocity.magnitude * Time.fixedDeltaTime > (waypoints [currentWaypoint].position - transform.position).magnitude) {
				body.MovePosition (waypoints [currentWaypoint].position);
			} else {
				body.MovePosition (transform.position + velocity * Time.fixedDeltaTime);
			}
			//rotate toward the desired value
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, desiredRot), Time.fixedDeltaTime * rotSpeed);
		}
	}

	void GetPath(){
		GameObject pathfinders = GameObject.FindGameObjectWithTag ("Pathfinder"); //get all paths
		Pathfinder[] paths = pathfinders.GetComponents<Pathfinder>(); //acquire pathfinder array from paths
		GameObject[] chosenPath = paths[Random.Range(0, paths.Length)].path; //pick a random path

		int pathLength = chosenPath.Length;
		if (moveType != MoveType.pass) {
			pathLength = Random.Range (2, chosenPath.Length);
		}
		waypoints = new Transform[pathLength];
		for (int i = 0; i < pathLength; i++){
			waypoints[i] = chosenPath[i].transform;
		}
	}
}

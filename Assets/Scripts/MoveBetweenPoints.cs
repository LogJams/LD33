using UnityEngine;
using System.Collections;

public class MoveBetweenPoints : MonoBehaviour {

	public Transform[] waypoints;
	public bool patrol;

	int currentWaypoint;

	float speed = 2f;
	float rotSpeed = 5;

	float desiredRot; //desired rotation at waypoint
	float rotation; //desired rotation + sinusoidal offset

	Vector3 velocity;



	Rigidbody2D body;


	// Use this for initialization
	void Start () {
		velocity = new Vector3 ();
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (waypoints.Length > 0 & currentWaypoint < waypoints.Length) { //if we're moving toward a waypoint
		//	Debug.Log (currentWaypoint + " waypoint");

			//if we're at the target, increment the current waypoint
			if ((transform.position - waypoints[currentWaypoint].position).sqrMagnitude < 0.1f) {
				currentWaypoint ++;
			}
			//if we're patrolling modulate the waypoint by the length
			if (patrol) {
				currentWaypoint = currentWaypoint % waypoints.Length;
			}
			//move toward current waypoint
			Vector3 targetPos = waypoints[currentWaypoint].position;
			velocity = targetPos - transform.position;
			velocity = velocity.normalized * speed;

			//set desired rotation
			desiredRot = Mathf.Atan2 (velocity.y, velocity.x) * Mathf.Rad2Deg;
			Debug.Log (desiredRot);
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
}

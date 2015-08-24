using UnityEngine;
using System.Collections;

public class MoveBetweenPoints : MonoBehaviour {

	public Transform[] waypoints;
	public enum MoveType{patrol, pass, wait};
	public MoveType moveType;

	public float angleOverride = 1000;

	//used to "ping-pong" loop movement
	public bool patrolReverse;
	//if the NPC is running away
	bool running = false;

	public int currentWaypoint;

	float speed = 0.75f;
	float rotSpeed = 5;

	float desiredRot; //desired rotation at waypoint
	float rotation; //desired rotation + sinusoidal offset

	Vector3 velocity;

	Animator anim;


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
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (waypoints.Length > 0) { //if we're moving toward a waypoint
			anim.SetBool ("Walking", velocity.sqrMagnitude > 0.1);

			if (!running) {
				//if we're patrolling then also move reverse sometimes
				if (moveType == MoveType.patrol) {
					if (currentWaypoint >= waypoints.Length) {
						currentWaypoint --;
						patrolReverse = true;
					} else if (currentWaypoint <= 1 && patrolReverse) { //don't patroll all the way back to the spawn...
						currentWaypoint = 1;
						patrolReverse = false;
					}
				} if (currentWaypoint == waypoints.Length) {
					anim.SetBool ("Walking", false);
					return;
				}

				if (moveType == MoveType.pass){
					if ((transform.position - waypoints[waypoints.Length-1].position).sqrMagnitude < 0.1f) {
						Destroy(this.gameObject);
						return;
					}
				}
				//if we're at the target, increment the current waypoint
				if ((transform.position - waypoints[currentWaypoint].position).sqrMagnitude < 0.1f) {
					if (patrolReverse) {
						currentWaypoint--;
					} else {
						currentWaypoint ++;
					}
				}
			} else {
				if ((transform.position - waypoints[0].position).sqrMagnitude < 0.1f) {
					Destroy(this.gameObject);
					return;
				}
				if ((transform.position - waypoints[currentWaypoint].position).sqrMagnitude < 0.1f) {
					currentWaypoint--;
				}
			}
			//move toward current waypoint
			if (currentWaypoint < waypoints.Length) {
				Vector3 targetPos = waypoints[currentWaypoint].position;
				velocity = targetPos - transform.position;
				velocity = velocity.normalized * speed;
				//set desired rotation
				desiredRot = Mathf.Atan2 (velocity.y, velocity.x) * Mathf.Rad2Deg;
			} else {
				desiredRot += (90*Mathf.Sign(transform.right.x + transform.right.y));
			}
		}
		if (running) {
			anim.speed = 3;
		}



	}

	void FixedUpdate() {
		if (waypoints.Length > 0 && currentWaypoint < waypoints.Length && currentWaypoint >= 0) { //if we're moving toward a waypoint
			//move toward target
			if (velocity.magnitude * Time.fixedDeltaTime > (waypoints [currentWaypoint].position - transform.position).magnitude) {
				body.MovePosition (waypoints [currentWaypoint].position);
			} else {
				body.MovePosition (transform.position + velocity * Time.fixedDeltaTime);
			}
		}
		if (angleOverride > 361) {
			//rotate toward the desired value
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, desiredRot), Time.fixedDeltaTime * rotSpeed);
		} else {
			Debug.Log (angleOverride + ", " + (angleOverride < 361));
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, angleOverride), Time.fixedDeltaTime * rotSpeed);
		}
	}

	void GetPath(){
		GameObject pathfinders = GameObject.FindGameObjectWithTag ("Pathfinder"); //get all paths
		Pathfinder[] paths = pathfinders.GetComponents<Pathfinder>(); //acquire pathfinder array from paths
		GameObject[] chosenPath = paths[Random.Range(0, paths.Length)].path; //pick a random path

		int pathLength = chosenPath.Length;
		if (moveType != MoveType.pass) {
			pathLength = Random.Range (2, chosenPath.Length);
			if (moveType == MoveType.patrol && pathLength == 2) {
				pathLength ++;
			}
		}
		waypoints = new Transform[pathLength];
		for (int i = 0; i < pathLength; i++){
			waypoints[i] = chosenPath[i].transform;
		}
	}

	public void startRunning(){
		running = true;
		speed = 2.5f;
		currentWaypoint = Mathf.Max (currentWaypoint - 1, 0);
	}
}

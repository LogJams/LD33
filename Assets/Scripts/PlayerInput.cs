using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {
	//is the player currently dragging a body?
	public bool dragging;
	public GameObject draggedBody;
	float dragDist = 0.5f;
	DumpBody van;
	//player movement speed
	float speed = 3; //m/s

	//list of targets in range to knock out
	List<GameObject> targets;

	//current rotation and velocity of the player
	float rotation;
	Vector3 velocity;

	//players rigidbody - used for movement and collision
	Rigidbody2D body;

	void Awake() {
		van = GameObject.FindGameObjectWithTag ("Finish").GetComponent<DumpBody> ();
	}

	//Initialize variables
	void Start () {
		velocity = new Vector3 ();
		body = GetComponent<Rigidbody2D> ();
		targets = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		//check if the player tries to grab a body
		if (Input.GetButtonUp ("Jump")) {
			if (!dragging) { //if we aren't currently dragging a body
				//grab the nearest body in the list of targets
				float minDist = Mathf.Infinity;
				GameObject nearest = null;
				//loop through all grabbable targets and select the closest
				foreach (GameObject obj in targets) {
					float dist = (transform.position - obj.transform.position).sqrMagnitude;
					if (dist < minDist) {
						minDist = dist;
						nearest = obj;
					}
				}
				draggedBody = nearest;
				if (draggedBody != null) { //if we grabbed someone
					draggedBody.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.None;
					//destroy their colliders
					foreach (Collider2D c in draggedBody.GetComponents<Collider2D>()) {
						Destroy (c);
					}
					BoxCollider2D box = draggedBody.AddComponent <BoxCollider2D>();
					box.isTrigger = true;
				}
			} else if (draggedBody != null) { //drop the body
				if (van.kill (draggedBody)) { //try to drop them in the van
					targets.Remove (draggedBody); //remove them from the list of things that can be grabbed
					Destroy (draggedBody);
				} else { //if not, drop them on the floor
					draggedBody.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
				}
				draggedBody = null;
			}
		}

		dragging = draggedBody != null; //check if we're carrying anything

		//set velocity direction from input
		velocity.Set (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
		velocity.Normalize (); //normalize speed to a unit vector
		if (dragging) {
			velocity = velocity * 0.33f; //slow down the player if they're carrying a body
		}

		//calculate the vector from the player's position to the mouse
		Vector3 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		rotation = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg; //find the angle of the direction vector
	}

	void FixedUpdate() {
		//rotate the player and move along the x/y axis
		//rotation is done in transform because Z rotation is fixed in the rigidbody
		transform.rotation = Quaternion.AngleAxis (rotation, Vector3.forward);
		body.MovePosition(transform.position + velocity * Time.fixedDeltaTime * speed);
		//move whatever we're dragging
		if (draggedBody != null) {
			draggedBody.transform.position = transform.position + transform.right * dragDist;
			draggedBody.transform.rotation = transform.rotation;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Human") && !targets.Contains(other.gameObject)) {
			targets.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (targets.Contains(other.gameObject)) {
			targets.Remove (other.gameObject);
		}
	}
}

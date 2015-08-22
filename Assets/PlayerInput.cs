using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {

	float speed = 3; //m/s

	float rotation;
	Vector3 velocity;
	Rigidbody2D body;

	// Use this for initialization
	void Start () {
		velocity = new Vector3 ();
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		velocity.Set (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
		velocity.Normalize ();

		Vector3 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		rotation = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
	}

	void FixedUpdate() {
		transform.rotation = Quaternion.AngleAxis (rotation, Vector3.forward);
		body.MovePosition(transform.position + velocity * Time.fixedDeltaTime * speed);
	}
}

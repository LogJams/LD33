using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {

	float speed = 5; //m/s

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
	}

	void FixedUpdate() {
		body.MovePosition(transform.position + velocity * Time.fixedDeltaTime * speed);
	}
}

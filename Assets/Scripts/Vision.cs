using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vision : MonoBehaviour {
	// is the player in peripheral vision?
	public bool inPeripheral;
	// is the player in main vision?
	public bool inMain;
	// range in (?) unts and angle of peripheral vision
	float peripheralRange = 1.5f;
	float peripheralAngle = 110;
	// range in (?) unts and angle of main vision
	float mainRange = 3;
	float mainAngle = 25;
	// list of GameObjects that are in the NPC's vision range
	List<GameObject> inVision;
	
	void Start () {
		inPeripheral = false;
		inMain = false;
		inVision = new List<GameObject> ();
	}

	void Update () {
		// resset vision bools to false
		inPeripheral = false;
		inMain = false;
		Vector2 vector;
		// the base angle of the NPC's vision
		float angleToFront = transform.rotation.eulerAngles.z;
		// angle between the NPC and teh target
		float angle;

		if (angleToFront > 180){
			angleToFront = 180 - angleToFront;
		}
		// check each target that is in vision range to determine if it can be seen
		foreach (GameObject target in inVision){
			if (target == null) {
				inVision.Remove(target);
				break;
			}
			// get vector between NPC and targer
			vector = target.transform.position - this.gameObject.transform.position;
			// get angle of vision and compare between main and peripheral angles to determine what form of vision is being used
			// raycast accordingly
			angle = Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x) - angleToFront;
			if (angle <= mainAngle && angle >= -mainAngle) {
				inMain = FireRayCast(vector, true);
			} else if (angle - (transform.rotation.z * Mathf.Rad2Deg) <= peripheralAngle && angle - (transform.rotation.z * Mathf.Rad2Deg) >= -peripheralAngle){
				inPeripheral = FireRayCast(vector, false);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (inVision.Contains(collider.gameObject) == false && collider.isTrigger == false){
			inVision.Add(collider.gameObject);
			Debug.Log("Enter");
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if (inVision.Contains(collider.gameObject) == true && collider.isTrigger == false){
			inVision.Remove(collider.gameObject);
			Debug.Log("Exit");
		}
	}

	bool FireRayCast(Vector2 vector, bool mainVision){
		int hitCount = 0;
		float castRange = mainRange;

		if (mainVision == false) {
			castRange = peripheralRange;
		}
		RaycastHit2D hit;

		for (int i = -4; i <= 4; i+=2) {
			Vector2 temp = Quaternion.Euler(0,0,i) * vector;
			if ((hit = raycast (Quaternion.Euler(0,0,i) * vector, castRange)).collider != null){ //if we hit something
				if (hit.collider.CompareTag("Player") == true){
					hitCount++;
				}
			}	
		}
		return hitCount > 1;
	}


	RaycastHit2D raycast(Vector2 vector, float castRange) {
		//scale the vision vector to cast range for line drawing
		vector.Normalize ();
		vector *= castRange;
		//fire a raycast in the vector direction at the cast range
		RaycastHit2D hit = Physics2D.Raycast (transform.position, vector, castRange);
		if (hit.collider == null) { //if we don't hit anything draw a debug line to the end of the vision range
			Debug.DrawLine (transform.position, transform.position + new Vector3 (vector.x, vector.y), Color.red, 0.5f);
		} else { //if we hit something draw a debug line to it
			Debug.DrawLine (transform.position, new Vector3(hit.point.x, hit.point.y), Color.red, 0.5f);
		}
		return hit;
	}


}

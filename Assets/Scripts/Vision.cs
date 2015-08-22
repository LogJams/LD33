﻿using UnityEngine;
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
	// the angle to the NPC's front of vision
	public float angleToFront = 0f;
	// list of GameObjects that are in the NPC's vision range
	List<GameObject> inVisionRange;
	public List<GameObject> inLineOfSight;
	
	void Start () {
		inPeripheral = false;
		inMain = false;
		inVisionRange = new List<GameObject> ();
		inLineOfSight = new List<GameObject> ();
	}

	void Update () {
		// resset vision bools to false
		inPeripheral = false;
		inMain = false;
		Vector2 vector;
		// the base angle of the NPC's vision
		angleToFront = transform.rotation.eulerAngles.z;
		// angle between the NPC and teh target
		float angle;

		if (angleToFront > 180){
			angleToFront = 180 - angleToFront;
		}
		inLineOfSight.Clear ();
		// check each target that is in vision range to determine if it can be seen
		foreach (GameObject target in inVisionRange){
			if (target == null) {
				inVisionRange.Remove(target);
				break;
			}
			// get vector between NPC and targer
			vector = target.transform.position - this.gameObject.transform.position;
			// get angle of vision and compare between main and peripheral angles to determine what form of vision is being used
			// raycast accordingly
			angle = Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x) - angleToFront;
			if (angle <= mainAngle && angle >= -mainAngle) {
				inMain = FireRayCast(target, vector, true);
			} else if (angle - (transform.rotation.z * Mathf.Rad2Deg) <= peripheralAngle && angle - (transform.rotation.z * Mathf.Rad2Deg) >= -peripheralAngle){
				inPeripheral = FireRayCast(target, vector, false);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (inVisionRange.Contains(collider.gameObject) == false && collider.isTrigger == false
		    && (collider.CompareTag("Player") == true || collider.CompareTag("Body") == true)){
			inVisionRange.Add(collider.gameObject);
			Debug.Log("Enter");
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if (inVisionRange.Contains(collider.gameObject) == true && collider.isTrigger == false){
			inVisionRange.Remove(collider.gameObject);
			Debug.Log("Exit");
		}
	}

	bool FireRayCast(GameObject target, Vector2 vector, bool mainVision){
		bool inVision = false;
		int hitCount = 0;
		float castRange = mainRange;

		if (mainVision == false) {
			castRange = peripheralRange;
		}
		RaycastHit2D hit;

		for (int i = -4; i <= 4; i+=2) {
			if ((hit = raycast (Quaternion.Euler(0,0,i) * vector, castRange)).collider != null){ //if we hit something
				if (hit.collider.CompareTag("Player") == true || hit.collider.CompareTag("Body") == true){
					hitCount++;
				}
			}	
		}
		inVision = hitCount > 1;
		if (inVision && inLineOfSight.Contains (target) == false) {
			inLineOfSight.Add(target);
		}
		return inVision;
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

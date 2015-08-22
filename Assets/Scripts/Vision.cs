using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vision : MonoBehaviour {
	public bool inPeripheral;
	public bool inMain;
	float peripheralRange = 1.5f;
	float peripheralAngle = 110;
	float mainRange = 3;
	float mainAngle = 25;

	List<GameObject> inVision;

	// Use this for initialization
	void Start () {
		inPeripheral = false;
		inMain = false;
		inVision = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		inPeripheral = false;
		inMain = false;
		Vector2 vector;
		float angle;

		foreach (GameObject target in inVision){
			vector = target.transform.position - this.gameObject.transform.position;
			angle = Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x) - transform.rotation.eulerAngles.z;
			Debug.Log (angle);
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
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if (inVision.Contains(collider.gameObject) == true && collider.isTrigger == false){
			inVision.Remove(collider.gameObject);
		}
	}

	bool FireRayCast(Vector2 vector, bool mainVision){
		bool isVisible = false;
		float castRange = mainRange;

		if (mainVision == false) {
			castRange = peripheralRange;
		}
		RaycastHit2D hit;

		//for (int i = -2; i <= 2; i++) {
		//	if (Physics.Raycast (new Ray(this.gameObject.transform.position, vector), castRange, hit) = true ){
		//		if (hit.collider.CompareTag("player") = true){
		//			isVisible = true;
		//		}
		//	}
		//}

		if ((hit = raycast (vector, castRange)).collider != null){ //if we hit something
			if (hit.collider.CompareTag("Player") == true){
				isVisible = true;
			}
		}		
		return isVisible;
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

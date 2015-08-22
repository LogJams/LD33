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
			angle = Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x);
			angle = (angle + 360) % 360;
			if (angle <= mainAngle) {
				inMain = FireRayCast(vector, true);
			} else if (angle <= peripheralAngle){
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
		//for (int i = -2; i <= 2; i++) {
		//	if (Physics.Raycast (new Ray(this.gameObject.transform.position, vector), castRange, hit) = true ){
		//		if (hit.collider.CompareTag("player") = true){
		//			isVisible = true;
		//		}
		//	}
		//}
		RaycastHit2D hit;
		if ((hit = Physics2D.Raycast (transform.position, vector, castRange)).collider != null){
			if (hit.collider.CompareTag("Player") == true){
				isVisible = true;
			}
		}		
		return isVisible;
		//return true;
	}
}

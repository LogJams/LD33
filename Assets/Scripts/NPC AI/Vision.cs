using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
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

	public GameObject meshFilter;
	Mesh shortRange;
	Mesh longRange;
	int meshPts = 20;

	Vector3[] visionPoints;

	bool dead;

	void Start () {
		inPeripheral = false;
		inMain = false;
		inVisionRange = new List<GameObject> ();
		inLineOfSight = new List<GameObject> ();

		visionPoints = new Vector3[meshPts];

		MeshFilter m = ((GameObject)Instantiate (meshFilter, new Vector3(0, 0, -1f), Quaternion.identity)).GetComponent<MeshFilter> ();
		longRange = m.mesh;
		m = ((GameObject)Instantiate (meshFilter, new Vector3(0, 0, -0.1f), Quaternion.identity)).GetComponent<MeshFilter> ();
		shortRange = m.mesh;
	}

	public void kill() {
		longRange.Clear ();
		shortRange.Clear ();
		dead = true;
	}

	void UpdateMesh() {
		shortRange.Clear ();
		visionPoints [0] = transform.position;
		shortRange.vertices = visionRaycast(peripheralAngle, peripheralRange);

		int numIndices = (meshPts - 2) * 3;
		int[] indices = new int[numIndices];
		int largeIndex = 2;
		for (int index = 0; index < numIndices; index+=3) {
			indices[index] = 0;
			indices[index+1] = largeIndex-1;
			indices[index+2] = largeIndex;
			largeIndex ++;
		}
		shortRange.triangles = indices;
		Color[] color = new Color[visionPoints.Length];
		for (int i = 0; i < color.Length; i++) {
			color [i] = Color.yellow;
		}
		shortRange.colors = color;

		longRange.Clear ();
		visionPoints [0] = transform.position;
		longRange.vertices = visionRaycast(mainAngle, mainRange);
		
		numIndices = (meshPts - 2) * 3;
		indices = new int[numIndices];
		largeIndex = 2;
		for (int index = 0; index < numIndices; index+=3) {
			indices[index] = 0;
			indices[index+1] = largeIndex-1;
			indices[index+2] = largeIndex;
			largeIndex ++;
		}
		longRange.triangles = indices;
		color = new Color[visionPoints.Length];
		for (int i = 0; i < color.Length; i++) {
			color [i] = Color.red;
		}
		longRange.colors = color;

		shortRange.Clear ();
	}

	Vector3[] visionRaycast(float theta, float range) {
		Vector3[] pts = new Vector3[meshPts];
		pts [0] = transform.position;

		float dTheta = 2 * theta / (meshPts-2);

		for (int i = 1; i < meshPts; i++) {
			Vector3 direction = Quaternion.Euler (0, 0, -theta + dTheta * (i-1)) * transform.right;
			RaycastHit2D ray;
			if ((ray = Physics2D.Raycast (transform.position, direction, range)).collider != null) { // if there was a hit
				pts[i] = new Vector3(ray.point.x, ray.point.y, 0);
			} else {
				pts[i] = transform.position + direction*range;
			}
		}
		return pts;
	}

	void Update () {
		if (dead)
			return;
		//update the vision mesh
		UpdateMesh ();

		// resset vision bools to false
		inPeripheral = false;
		inMain = false;
		Vector2 vector;
		// the base angle of the NPC's vision
		angleToFront = transform.rotation.eulerAngles.z;
		// angle between the NPC and teh target
		float angle;

		if (angleToFront > 180){
			angleToFront = angleToFront - 360;
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
			if (angle > 180) {angle -= 360;}
			if (angle < -180) {angle += 360;}
			if (angle <= mainAngle && angle >= -mainAngle) {
				inMain = FireRayCast(target, vector, true);
			} else if (angle <= peripheralAngle && angle >= -peripheralAngle){
				inPeripheral = FireRayCast(target, vector, false);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (inVisionRange.Contains(collider.gameObject) == false && collider.isTrigger == false
		    && (collider.CompareTag("Player") == true || collider.CompareTag("Body") == true)){
			inVisionRange.Add(collider.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if (inVisionRange.Contains(collider.gameObject) == true && collider.isTrigger == false){
			inVisionRange.Remove(collider.gameObject);
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

		int index = 1;

		for (int i = -4; i <= 4; i+=2) {
			if ((hit = raycast (Quaternion.Euler(0,0,i) * vector, castRange)).collider != null){ //if we hit something
				if (hit.collider.CompareTag("Player") == true || hit.collider.CompareTag("Body") == true){
					hitCount++;
				}
				visionPoints[index] = hit.point;
			} else {
				visionPoints[index] = new Vector3(vector.normalized.x, vector.normalized.y) * castRange + transform.position;
			}
			index ++;
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

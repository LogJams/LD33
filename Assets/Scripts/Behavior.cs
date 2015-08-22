using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vision))]
public class Behavior : MonoBehaviour {
	Vision vision;

	// Use this for initialization
	void Start () {
		vision = GetComponent<Vision> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (vision.inMain == true){
			Debug.Log("DETECTED");
		} else if (vision.inPeripheral == true) {
			Debug.Log("IN PERIPHERAL");
			//
		}
	}

	void Detect (){

	}

}

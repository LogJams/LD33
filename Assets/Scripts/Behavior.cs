using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vision))]
public class Behavior : MonoBehaviour {
	Vision vision;
	public bool isPolice = false;

	// Use this for initialization
	void Start () {
		vision = GetComponent<Vision> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (vision.inMain == true){
			Detect ();
		} else if (vision.inPeripheral == true) {
			Debug.Log("IN PERIPHERAL");
			TurnToInvestigate();
		}
	}

	void Detect (){
		if (isPolice == true) {

		} else {

		}
	}

	void TurnToInvestigate(){

	}

}

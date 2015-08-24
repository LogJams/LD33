using UnityEngine;
using System.Collections;

public class PoliceLightScript : MonoBehaviour {

	Light[] lights;

	float period = 0.5f;
	float time = 0;

	// Use this for initialization
	void Start () {
		lights = GetComponentsInChildren<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Behavior.policeFrenzy) {
			time += Time.deltaTime; 
			time = time % period;

			float progress = (time / period);

			//if progress > 1 increase light 2 and decrease light 1
			//if progress < 1 increase light 1 and decrease light 2

			if (progress > 0.5) {
				lights [0].intensity = 0;
				lights [1].intensity = 8;
			} else {
				lights [0].intensity = 8;
				lights [1].intensity = 0;
			}
		}
	}
}

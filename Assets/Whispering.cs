using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Whispering : MonoBehaviour {

	AudioSource sound;

	float normalVolume = 1;

	Slider slider;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource> ();
		normalVolume = sound.volume;
		slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		sound.volume = normalVolume * slider.value;
	}
}

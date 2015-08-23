using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseScript : MonoBehaviour {

	bool pause;

	public Text text;
	public Image img;
	public GameObject button;

	// Use this for initialization
	void Start () {
		visible (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("Pause")) {
			pause = !pause;
			visible (pause);
		}
	}


	void visible(bool show) {

		text.enabled = show;
		img.enabled = show;
		button.SetActive(show);

		if (show) {
			Time.timeScale = 0;

		} else {
			Time.timeScale = 1;
		}
	}

	public void mainMenu() {
		Application.LoadLevel (0);
	}
}

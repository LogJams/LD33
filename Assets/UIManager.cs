using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public void StartGame() {
		Application.LoadLevel ("Master Scene");
	}

	public void Exit() {
		Application.Quit ();
	}
}

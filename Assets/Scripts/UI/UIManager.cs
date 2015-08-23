using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public void StartGame() {
		Application.LoadLevel ("TutorialScene");
	}

	public void Exit() {
		Application.Quit ();
	}
}

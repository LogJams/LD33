using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int bodycount = GameInfo.bodyCount;
		int nightNumber = GameInfo.nightNumber;
		GameInfo.LoseCondition lose = GameInfo.loseCondition;
		GameInfo.resetData ();
		Text text = this.gameObject.GetComponent<Text> ();
		if (nightNumber == 1) {
			if (bodycount ==1) {
				if (lose == GameInfo.LoseCondition.Insane) {
					text.text = "You killed " + bodycount + " person in " + nightNumber + " night, but you still couldn't silence the voices.";
				} else if (lose == GameInfo.LoseCondition.PoliceCaught) {
					text.text = "The police have caught you, ending your reign of terror after " + nightNumber + " night.  You left " + bodycount + " victim.";
				} else {
					text.text = "After " + nightNumber + " night of hunting a potential victim called the police and you were arrested.  You left " + bodycount + " victim.";
				}
			} else {
				if (lose == GameInfo.LoseCondition.Insane) {
				text.text = "You killed " + bodycount + " people in " + nightNumber + " night, but you still couldn't silence the voices.";
				} else if (lose == GameInfo.LoseCondition.PoliceCaught) {
					text.text = "The police have caught you, ending your reign of terror after " + nightNumber + " night.  You left " + bodycount + " victims.";
				} else {
					text.text = "After " + nightNumber + " night of hunting a potential victim called the police and you were arrested.  You left " + bodycount + " victims.";
				}
			}
		} else {
			if (lose == GameInfo.LoseCondition.Insane) {
				text.text = "You killed " + bodycount + " people in " + nightNumber + " nights, but you still couldn't silence the voices.";
			} else if (lose == GameInfo.LoseCondition.PoliceCaught) {
				text.text = "The police have caught you, ending your reign of terror after " + nightNumber + " nights.  You left " + bodycount + " victims.";
			} else {
				text.text = "After " + nightNumber + " nights of hunting a potential victim called the police and you were arrested.  You left " + bodycount + " victims.";
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

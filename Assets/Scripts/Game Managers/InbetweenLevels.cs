using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InbetweenLevels : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text text = this.gameObject.GetComponent<Text> ();
		text.text = "The voices has been silenced ... for tonight.  You have done terrible things but the voices are comming back.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextLevel(){
		Application.LoadLevel ("TutorialScene");
	}
}

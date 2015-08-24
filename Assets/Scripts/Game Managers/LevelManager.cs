using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
	float timeBetweenSpawns;
	float timeSinceSpawns;
	public GameObject person;
	public GameObject police;
	public Slider slider;
	public Image blackout;
	public AudioClip siren;


	float spawnCooldownDecay = 0.9f;

	float fadeMod = 1;

	float loseValue = .5f;
	int spawnLayer = 1;
	Color color;
	bool isFaded = false;
	bool fadeOut = false;
	bool lostGame = false;

	public Text info;

	bool transition;

	bool playedSiren = false;
	AudioSource sound;

	// Use this for initialization
	void Start () {
		Behavior.policeFrenzy = false;
		color = blackout.color;
		color.a = 1f;
		blackout.color = color;
		timeBetweenSpawns = GameInfo.spawnInterval;
		timeSinceSpawns = timeBetweenSpawns;
		GameInfo.nightNumber++;

		string txt = "Night: " + GameInfo.nightNumber;
		info.text = txt;
		sound = GetComponent<AudioSource> ();
	}

	public void beginFade(bool lose){
		lostGame = lose;
		if (lose) {
			fadeMod = 3.5f;
		}
		fadeOut = true;
	}
	public void endLevel() {
		if (slider.value > loseValue) {
			GameInfo.loseCondition = GameInfo.LoseCondition.Insane;
			lose ();
		} else {
			//update statistics and load next level
			GameInfo.silencingModifier *= .75f;
			GameInfo.spawnInterval *= spawnCooldownDecay;
			Application.LoadLevel ("Inbetween");
		}
	}

	public void lose() {
		Application.LoadLevel ("Gameover");
	}

	
	// Update is called once per frame
	void Update () {
		if (!playedSiren && Behavior.policeFrenzy) {
			sound.PlayOneShot(siren);
			playedSiren = true;
		}

		timeSinceSpawns += Time.deltaTime;
		if (timeSinceSpawns > timeBetweenSpawns) {
			SpawnNPC ();
			timeSinceSpawns = 0f;
			if (Behavior.policeFrenzy) {
				timeSinceSpawns += timeBetweenSpawns/2;
			}
		}
//		if (levelTime <= 0) {
//			beginFade (false);
//		}
		if (!isFaded) {
			color = blackout.color;
			if (color.a > .01f) {
				color.a -= .2f * Time.deltaTime;
			} else {
				color.a = 0f;
				isFaded = true;
			}
			blackout.color = color;
		} else if (fadeOut) {
			color = blackout.color;
			if (color.a < .99f) {
				color.a += fadeMod * 0.4f * Time.deltaTime;
			} else {
				color.a = 1f;
				blackout.color = color;
				if (lostGame){
					lose ();
				}else{
					endLevel();
				}
			}
			blackout.color = color;
		}
	}
	
	void SpawnNPC(){
		int random = Random.Range (0, 10);
		GameObject npc;
		if (random <= 1 || Behavior.policeFrenzy) {
			npc = Instantiate (police);
			if (npc.GetComponent<MoveBetweenPoints>().moveType == MoveBetweenPoints.MoveType.wait) {
				npc.GetComponent<MoveBetweenPoints>().moveType = MoveBetweenPoints.MoveType.patrol;
			}
		} else {
			npc = Instantiate (person);
		}
		SpriteRenderer[] sprites = npc.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer sr in sprites) {
			sr.sortingLayerID += spawnLayer;
		}
		spawnLayer+=5;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teachWindow : MonoBehaviour {
	public static GamePlayer player;

	public void takeClasses(){
		player.credit += 5;
		player.energy -= 15;
		player.health -= 5;
		GameController.checkAndNotify();
		player.Finished();
		Destroy(gameObject);
	}

	public void No(){
		player.Finished();
		Destroy(gameObject);
	}
	// Use this for initialization
	void Start () {
		player = GameController.clientPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

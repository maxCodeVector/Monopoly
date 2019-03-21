using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workWindow : MonoBehaviour {
	public static GamePlayer player;

	public void work(){
		player.wealth += 1000;
		player.energy -= 20;
		player.health -= 10;
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

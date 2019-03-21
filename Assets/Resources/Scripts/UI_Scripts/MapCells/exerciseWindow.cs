using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exerciseWindow : MonoBehaviour {
	public static GamePlayer player;

	public void confirm(){
		player.health += 20;
		player.energy -= 15;
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

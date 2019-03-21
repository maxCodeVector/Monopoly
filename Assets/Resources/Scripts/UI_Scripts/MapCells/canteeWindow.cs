using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canteeWindow : MonoBehaviour {
	public static GamePlayer player;
	public void haveMeal(){
		player.wealth -= 300;
		player.health += 3;
		player.energy += 20;
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

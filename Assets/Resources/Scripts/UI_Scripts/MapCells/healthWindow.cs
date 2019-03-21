using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthWindow : MonoBehaviour {
	public static GamePlayer player;

	public void haveTreatment(){
		player.wealth -= 1500;
		player.health += 50;
		player.energy += 30;
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

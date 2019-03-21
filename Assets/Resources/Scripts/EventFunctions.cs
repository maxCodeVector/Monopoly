using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFunctions : MonoBehaviour {
	public static GamePlayer player;

	public static void setClientPlayer(GamePlayer aPlayer){
		player = aPlayer;
	}


	public void Fortune(){

	}


	// public void buyItem(string itemName){

	// }

	

	


	public void checkAndNotify(){
		if(player.wealth <= 0)
			GlobalControl.sendPoorFailed();
		if(player.health >= 100)
			player.health = 100;
		if(player.energy > 100)
			player.energy = 100;
		// player.notify();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

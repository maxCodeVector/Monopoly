using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tollHouse : MonoBehaviour {
	private HouseController hc;
	public UILabel owner;
	public UILabel level;
	public UILabel paid;
	private GamePlayer player;

	public void setController(HouseController hc0){
		player = hc0.player;
		hc = hc0;
		owner.text = hc.owner.nickName;
		level.text = hc.level.ToString();
		paid.text = hc.getToll().ToString();
		player.wealth -= hc.getToll();
		GameController.checkAndNotify();
		GlobalControl.sendToll(hc.owner.id,hc.getToll());
	}
	public void rest(){
		player.energy += 10;
		GameController.checkAndNotify();
		player.Finished();
		Destroy(gameObject);
	}

	public void study(){
		player.credit += 2;
		GameController.checkAndNotify();
		player.Finished();
		Destroy(gameObject);
	}

	public void exit(){
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

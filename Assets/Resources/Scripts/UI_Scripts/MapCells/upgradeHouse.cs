using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeHouse : MonoBehaviour {
	private HouseController hc;
	public UILabel level;
	public UILabel cost;
	private GamePlayer player;

	public void setController(HouseController hc0){
		hc = hc0;
		level.text = hc.level.ToString();
		cost.text = hc.getUpgradeCost().ToString();
	}
	public void upgrade(){
		player.wealth -= hc.getUpgradeCost();
		checkAndNotify();
		hc.level += 1;
		GlobalControl.sendUpgradeHouse(hc.level, hc.mapCell.gameObject.name);
		string hatname = string.Format("C{0}hat",player.role);
		GameObject hat = Resources.Load("Prefabs/Characters/"+hatname) as GameObject;
		hat = Instantiate(hat, player.transform.position, player.transform.rotation);
		player.Finished();
		Destroy(gameObject);
	}
	public void rest(){
		player.energy += 10;
		checkAndNotify();
		player.Finished();
		Destroy(gameObject);
	}

	public void study(){
		player.credit += 2;
		checkAndNotify();
		player.Finished();
		Destroy(gameObject);
	}

	public void exit(){
		player.Finished();
		Destroy(gameObject);
	}

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
		player = GameController.clientPlayer;		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

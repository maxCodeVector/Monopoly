using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buyHouse : MonoBehaviour {
	private HouseController hc;
	private GamePlayer player;

	public void setController(HouseController c){
		hc = c;
	}

	public void buyHouseButton(){
		hc.owner = player;
		player.wealth -= hc.getUpgradeCost();
		GameController.checkAndNotify();
		hc.level = 1;
		GlobalControl.sendBuyHouse(hc.mapCell.gameObject.name);
		string hatname = string.Format("C{0}hat",player.role);
		GameObject hat = Resources.Load("Prefabs/Characters/"+hatname) as GameObject;
		hat = Instantiate(hat, player.transform.position, player.transform.rotation);
		player.Finished();
		Destroy(gameObject);
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

	public void leave(){
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

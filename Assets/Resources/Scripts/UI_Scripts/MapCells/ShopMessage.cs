using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMessage : MonoBehaviour {
	public int cardID;
	public int cost;
	public UIAtlas itemAtlas;
	public UISprite sprite;
	public UILabel message;
	public UILabel price;


	public void setMessage(string buttonName){
		itemAtlas = Resources.Load("UI/Window Atals", typeof(UIAtlas)) as UIAtlas;
		switch (buttonName){
			case "Item1":
				sprite.atlas = itemAtlas;
				sprite.spriteName = "如意骰子";
				price.text = "$2000";
				message.text = "You can determine the next dice number.";
				cardID = 1;
				cost = 2000;
				break;
			case "Item2":
				sprite.atlas = itemAtlas;
				sprite.spriteName = "地雷卡";
				price.text = "$2000";
				message.text = "You can place a mine at a map cell. When a player stops on that position, the mine will explode and take the injured player to infirmary for two rounds.";
				cardID = 2;
				cost = 2000;
				break;
			case "Item3":
				sprite.atlas = itemAtlas;
				sprite.spriteName = "运动卡";
				price.text = "$800";
				message.text = "You will gain 10 health value ";
				cardID = 3;
				cost = 800;
				break;
			// case "Item4":
			// 	sprite.atlas = itemAtlas;
			// 	sprite.spriteName = "购房卡";
			// 	price.text = "$2000";
			// 	message.text = "You can use it when stop on someone else's house area, then you can pay certain money to get the ownership of that house.";
			// 	cardID = 4;
			// 	break;
			// case "Item5":
			// 	sprite.atlas = itemAtlas;
			// 	sprite.spriteName = "效率卡";
			// 	price.text = "$2500";
			// 	message.text = "The gain of credit and health will double in the next 3 rounds.";
			// 	cardID = 5;
			// 	break;
			// case "Item6":
			// 	sprite.atlas = itemAtlas;
			// 	sprite.spriteName = "陷害卡";
			// 	price.text = "$2000";
			// 	message.text = "The specified player will be detained in the dark house for one round.";
			// 	cardID = 6;
			// 	break;
		}
	}

	public void perchase(){
		GameController.buyCards(cardID,cost);
		// GameController.showCards();
		GameController.clientPlayer.Finished();
		GameObject father = GameObject.Find("UI Root/shopWindow");
		Destroy(father);
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

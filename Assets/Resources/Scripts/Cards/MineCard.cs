using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCard : MonoBehaviour, CardItems {
	public static int id = 2;
	public GameObject getGameObject(){
		return gameObject;
	}
	public int getID(){
		return id;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void function(){
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID){
			if(GameController.clientPlayer.standingCell.bomb == null){
				GameObject bomb = Resources.Load("Prefabs/Characters/boom") as GameObject;
				bomb = Instantiate(bomb, GameController.clientPlayer.standingCell.transform.position, 
					GameController.clientPlayer.transform.rotation);
				GameController.clientPlayer.standingCell.bomb = bomb;
				GlobalControl.setBomb(GameController.clientPlayer.standingCell.gameObject.name);

				Transform slot = transform.parent;
				GameObject emptyCard = Resources.Load("Prefabs/Cards/emptyCard") as GameObject;
				NGUITools.AddChild(slot.gameObject, emptyCard);
				// Destroy(gameObject);
				foreach(CardItems card in GameController.clientPlayer.cards){
					if(card.getID() == 2){
						GameController.clientPlayer.cards.Remove(card);
						GameController.showCards();
						return;
					}
				}
			}else{
				GameController.setInfor("Here already has a bomb!");
			}
		}
	}
}

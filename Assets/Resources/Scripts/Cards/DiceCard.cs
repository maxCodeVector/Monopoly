using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCard : MonoBehaviour, CardItems {
	public Dice dice;
	public GameObject uiRoot;
	public int id = 1;
	public GameObject getGameObject(){
		return gameObject;
	}

	public int getID(){
		return id;
	}
	// Use this for initialization
	void Start () {
		uiRoot = GameObject.Find("UI Root");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void function(){
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID){
			GameObject messageBox = Resources.Load("prefabs/UI/diceCardWindow") as GameObject;
			messageBox = NGUITools.AddChild(uiRoot, messageBox);
			messageBox.transform.localScale *= 1.5f;

			Transform slot = transform.parent;
			GameObject emptyCard = Resources.Load("Prefabs/Cards/emptyCard") as GameObject;
			NGUITools.AddChild(slot.gameObject, emptyCard);

			foreach(CardItems card in GameController.clientPlayer.cards){
				if(card.getID() == id){
					GameController.clientPlayer.cards.Remove(card);
					GameController.showCards();
					return;
				}
			}
			// Destroy(gameObject);
		}
	}
}

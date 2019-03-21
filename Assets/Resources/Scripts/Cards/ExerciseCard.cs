using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseCard : MonoBehaviour, CardItems  {
	public static int id = 3;
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
		GameController.clientPlayer.health += 10;
		GameController.checkAndNotify();

		foreach(CardItems card in GameController.clientPlayer.cards){
			if(card.getID() == id){
				GameController.clientPlayer.cards.Remove(card);
				GameController.showCards();
				return;
			}
		}
	}
}

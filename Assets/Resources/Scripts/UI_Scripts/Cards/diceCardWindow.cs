using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceCardWindow : MonoBehaviour {
	public GameObject uiRoot;
	public Dice dice;
	public void dice1(){
		dice.dicing = false;
		dice.disableDice();
		dice.diceNum = 1;
		dice.label.text = dice.diceNum.ToString();
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
			GameController.startPlayer.diced();
		Destroy(gameObject);
	}
	public void dice2(){
		dice.dicing = false;
		dice.disableDice();
		dice.diceNum = 2;
		dice.label.text = dice.diceNum.ToString();
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
			GameController.startPlayer.diced();
		Destroy(gameObject);
	}
	public void dice3(){
		dice.dicing = false;
		dice.disableDice();
		dice.diceNum = 3;
		dice.label.text = dice.diceNum.ToString();
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
			GameController.startPlayer.diced();
		Destroy(gameObject);
	}
	public void dice4(){
		dice.dicing = false;
		dice.disableDice();
		dice.diceNum = 4;
		dice.label.text = dice.diceNum.ToString();
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
			GameController.startPlayer.diced();
		Destroy(gameObject);
	}
	public void dice5(){
		dice.dicing = false;
		dice.disableDice();
		dice.diceNum = 5;
		dice.label.text = dice.diceNum.ToString();
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
			GameController.startPlayer.diced();
		Destroy(gameObject);
	}
	public void dice6(){
		dice.dicing = false;
		dice.disableDice();
		dice.diceNum = 6;
		dice.label.text = dice.diceNum.ToString();
		if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
			GameController.startPlayer.diced();
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		dice = GameObject.Find("UI Root/GameMainUI/Anchor_BR/Dice").GetComponent<Dice>();
		uiRoot = GameObject.Find("UI Root");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATMsave : MonoBehaviour {
	public UISlider slider;
	public UILabel depositLabel;
	public UILabel cashLabel;
	private GamePlayer player;

	public void onValueChange(){
		double ratio = slider.value;
		double newCash = (1-ratio) * player.wealth/1;
		double newDeposit = (player.deposit + ratio * player.wealth)/1;
		showValue(newDeposit, newCash);
	}

	private void showValue(double deposit, double cash){
		depositLabel.text = ((int)deposit).ToString();
		cashLabel.text = ((int)cash).ToString();
	}

	public void confirm(){
		player.wealth = int.Parse(cashLabel.text);
		player.deposit = int.Parse(depositLabel.text);
		GameController.checkAndNotify();
		player.Finished();
		GameObject ATM = GameObject.Find("UI Root/ATMWindow");
		GameController.clientPlayer.Finished();
		Destroy(ATM);
		Destroy(gameObject);
	}

	public void exit(){
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		slider.value = 0;
		player = GameController.clientPlayer;
		showValue(player.deposit, player.wealth);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

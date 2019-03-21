using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATMWithdraw : MonoBehaviour {
	public UISlider slider;
	public UILabel depositLabel;
	public UILabel cashLabel;
	private GamePlayer player;

	public void onValueChange(){
		double ratio = slider.value;
		double newDeposit = (1-ratio) * player.deposit;
		double newCash =  player.wealth + ratio * player.deposit;
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
		GameObject ATM = GameObject.Find("UI Root/ATMWindow");
		player.Finished();
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

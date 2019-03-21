using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIATM : MonoBehaviour {
	public UISlider slider;
	public UILabel depositLabel;
	public UILabel cashLabel;
	private GamePlayer player;

	public void onValueChange(){
		float ratio = slider.value;
		int newCash = (int) (1-ratio) * player.wealth;
		int newDeposit = player.deposit + (int) ratio * player.wealth;
		showValue(newDeposit, newCash);
	}

	private void showValue(int deposit, int cash){
		depositLabel.text = deposit.ToString();
		cashLabel.text = cash.ToString();
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

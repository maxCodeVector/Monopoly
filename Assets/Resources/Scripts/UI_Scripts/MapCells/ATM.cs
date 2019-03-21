using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATM : MonoBehaviour {
	public UILabel depositLabel;
	public UILabel cashLabel;
	private GamePlayer player;
	private GameObject uiRoot;

	private void showValue(int deposit, int cash){
		depositLabel.text = deposit.ToString();
		cashLabel.text = cash.ToString();
	}

	public void withdraw(){
		GameObject withdrawWindow = Resources.Load("Prefabs/UI/ATMWithdraw") as GameObject;
		withdrawWindow = NGUITools.AddChild(uiRoot, withdrawWindow);
	}
	public void save(){
		GameObject saveWindow = Resources.Load("Prefabs/UI/ATMSave") as GameObject;
		saveWindow = NGUITools.AddChild(uiRoot, saveWindow);
	}
	// Use this for initialization
	void Start () {
		player = GameController.clientPlayer;
		showValue(player.deposit, player.wealth);
		uiRoot = GameObject.Find("UI Root");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

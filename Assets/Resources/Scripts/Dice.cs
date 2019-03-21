using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {
	System.Random rand = new System.Random();
	public int diceNum;
	public UILabel label;
	public UIButton button;
	public bool dicing;
	// Use this for initialization

	public void buttonClicked(){
		if(!GameController.isGameOver){
			dicing = false;
			disableDice();
			diceNum = rand.Next(1,7);
			label.text = diceNum.ToString();
			// button只有在client 开始走时启用！
			if(int.Parse(GameController.startPlayer.id) == GlobalControl.clientID)
				GameController.startPlayer.diced();
		}
	}

	public void enableDice(){
		button.state = UIButton.State.Normal;
		button.GetComponent<BoxCollider>().enabled = true;
		dicing = true;
	}

	public void disableDice(){
		button.state = UIButton.State.Disabled;
		// 如果不取消掉碰撞器，那么鼠标滑过按钮的时候还会变成hover状态
		button.GetComponent<BoxCollider>().enabled = false;
		dicing = false;
	}

	void Start () {
		if(int.Parse(GameController.startPlayer.id) != GlobalControl.clientID)
			disableDice();
	}
	
	// Update is called once per frame
	void Update () {
		if(dicing){
			diceNum = rand.Next(1,7);
			label.text = diceNum.ToString();
		}
	}
}

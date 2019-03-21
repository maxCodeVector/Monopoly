using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButtonMethods : MonoBehaviour {
	public static List<UIButton> buttons = new List<UIButton>();
	public static void click(string name){
		int index = int.Parse(name);
		foreach(UIButton button in buttons){
			if(button.GetComponent<RoomRoleButton>().isSelected == false){
				button.state = UIButton.State.Normal;
				button.GetComponent<BoxCollider>().enabled = true;
			}
		}
		buttons[index-1].state = UIButton.State.Disabled;
		buttons[index-1].GetComponent<BoxCollider>().enabled = false;
	}
	
	public void confirmClick(){
		for(int i = 0; i < buttons.Count; i++){
			if(buttons[i].state == UIButton.State.Disabled && 
					buttons[i].GetComponent<RoomRoleButton>().isSelected == false){
				GlobalControl.sendChooseCharacterMessage(i+1);
				Debug.Log("sent!");
			}
		}
	}

	public static void initCharacterButtons(){
		buttons.Clear();
		Transform characterButton = GameObject.Find("UI Root/Room/RoomButtons").transform;
		for(int i = 1; i < 7; i++){
			UIButton button = characterButton.Find(""+i).GetComponent<UIButton>();
			buttons.Add(button);
		}
	}
	// Use this for initialization
	void Start () {
		initCharacterButtons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

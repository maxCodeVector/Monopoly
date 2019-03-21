using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItem : MonoBehaviour {
	private int roomID;
	private string roomName;
	private int playerNum;
	public UILabel nameLabel;
	public UILabel numLabel;

	private void OnClick(){
		int SessionID = Random.Range(1, 100000000);
		GlobalControl.sendJoinRoom(roomID, SessionID);
		RoomController.roomName = roomName;
	}

	public void setAndShowValues(int id, string name, int pNum){
		roomID = id;
		roomName = name;
		playerNum = pNum;
		nameLabel.text = name;
		numLabel.text = pNum+" / 3";
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

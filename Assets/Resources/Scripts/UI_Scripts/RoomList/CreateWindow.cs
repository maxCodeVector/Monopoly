using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWindow : MonoBehaviour {
	public UIInput input;

	public void confirm(){
		int SessionID = Random.Range(1, 100000000);
		string roomName = input.value;
		GlobalControl.sendCreateRoom(roomName, SessionID);
		RoomController.roomName = roomName;
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButton : MonoBehaviour {
	GameObject anchorCenter;
	private void OnClick(){
		anchorCenter = GameObject.Find("UI Root/AnchorCenter");
		GameObject createWindow = Resources.Load("Prefabs/UI/RoomList/CreateRoomWindow") as GameObject;
		createWindow = NGUITools.AddChild(anchorCenter, createWindow);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

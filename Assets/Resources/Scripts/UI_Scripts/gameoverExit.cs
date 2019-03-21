using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameoverExit : MonoBehaviour {

	public void OnClick(){
		SceneManager.LoadScene("RoomList");
		RoomListController.isBack = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

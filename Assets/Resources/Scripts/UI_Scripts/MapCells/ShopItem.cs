using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour {

	void OnClick(){
		GameObject uiRoot = GameObject.Find("UI Root");
		GameObject messageWindow =  Resources.Load("Prefabs/UI/shopMessage") as GameObject;
		messageWindow = NGUITools.AddChild(uiRoot, messageWindow);
		messageWindow.GetComponent<ShopMessage>().setMessage(gameObject.name);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

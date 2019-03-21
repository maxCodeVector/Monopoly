using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCard : MonoBehaviour, CardItems {
	public static int id = 0;
	public GameObject getGameObject(){
		return gameObject;
	}
	public int getID(){
		return id;
	}
	// Use this for initialization
	void Start () {
		UIButton button = GetComponent<UIButton>();
		button.state = UIButton.State.Disabled;
		button.GetComponent<BoxCollider>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void function(){
		
	}
}

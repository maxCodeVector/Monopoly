using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoWindow : MonoBehaviour {
	public UILabel label;

	public void exit(){
		Destroy(gameObject);
	}
	public void setMessage(string m){
		label.text = m;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

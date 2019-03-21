using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setting : MonoBehaviour {
	public GameObject uiRoot;
	public void OnClick(){
		GameObject window = Resources.Load("Prefabs/UI/settingWindow") as GameObject;
		window = NGUITools.AddChild(uiRoot, window);

	}
	// Use this for initialization
	void Start () {
		uiRoot = GameObject.Find("UI Root");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

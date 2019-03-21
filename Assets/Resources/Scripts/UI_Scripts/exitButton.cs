using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitButton : MonoBehaviour {
	public GameObject father;
	// Use this for initialization
	void Start () {
		father = transform.parent.gameObject;
	}
	
	private void OnClick(){
		GameController.clientPlayer.Finished();
		Destroy(father);
		GameController.clientPlayer.Finished();
	}
	// Update is called once per frame
	void Update () {
		
	}
}

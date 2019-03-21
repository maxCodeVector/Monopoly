using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entertainWindow : MonoBehaviour {


	public void confirm(){
		GameController.clientPlayer.health += 5;
		GameController.clientPlayer.energy += 15;
		GameController.clientPlayer.intell += 5;
		GameController.checkAndNotify();
		GameController.clientPlayer.Finished();
		Destroy(gameObject);
	}

	public void No(){
		GameController.clientPlayer.Finished();
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

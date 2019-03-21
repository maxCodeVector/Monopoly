using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour {
	void OnTriggerEnter(Collider collider){
		if(collider.GetComponent<GamePlayer>()==GameController.clientPlayer){
			GameController.enterAreas(gameObject.name);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

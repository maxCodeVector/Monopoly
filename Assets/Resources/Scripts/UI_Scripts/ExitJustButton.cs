using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitJustButton : MonoBehaviour {
	public GameObject father;
	// Use this for initialization
	void Start () {
		father = transform.parent.gameObject;
	}
	private void OnClick(){
		Destroy(father);
	}

	void Update () {
		
	}
}

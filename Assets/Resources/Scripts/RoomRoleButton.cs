using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRoleButton : MonoBehaviour {
	public bool isSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnClick(){
		RoomButtonMethods.click(gameObject.name);
	}
}
